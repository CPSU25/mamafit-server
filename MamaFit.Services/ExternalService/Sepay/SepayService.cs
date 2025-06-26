using System.Text.RegularExpressions;
using AutoMapper;
using MamaFit.BusinessObjects.DTO.OrderDto;
using MamaFit.BusinessObjects.DTO.SepayDto;
using MamaFit.BusinessObjects.Enum;
using MamaFit.Repositories.Helper;
using MamaFit.Repositories.Implement;
using MamaFit.Repositories.Infrastructure;
using MamaFit.Services.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace MamaFit.Services.ExternalService.Sepay;

public class SepayService : ISepayService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly SepaySettings _sepaySettings;
    private readonly IMapper _mapper;
    private readonly IValidationService _validationService;
    private readonly IOrderService _orderService;
    private readonly ITransactionService _transactionService;

    public SepayService(IUnitOfWork unitOfWork,
        IOptions<SepaySettings> sepaySettings,
        IValidationService validationService,
        IOrderService orderService,
        ITransactionService transactionService,
        IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _sepaySettings = sepaySettings.Value;
        _validationService = validationService;
        _orderService = orderService;
        _transactionService = transactionService;
        _mapper = mapper;
    }

    public async Task ProcessPaymentWebhookAsync(SepayWebhookPayload payload, string authHeader)
    {
        if (!ValidateAuthHeader(authHeader))
        {
            throw new ErrorException(StatusCodes.Status401Unauthorized, ApiCodes.UNAUTHORIZED, "Invalid API key");
        }

        var orderCode = ExtractOrderCodeFromContent(payload.content);
        if (string.IsNullOrEmpty(orderCode))
        {
            throw new ErrorException(StatusCodes.Status400BadRequest, ApiCodes.INVALID_INPUT,
                "No order code found in payment content");
        }

        var order = await _unitOfWork.OrderRepository.GetByCodeAsync(orderCode);
        _validationService.CheckNotFound(order, $"Order with code {orderCode} not found");

        await _transactionService.CreateTransactionAsync(payload, order.Id, order.Code);
        await _orderService.UpdateOrderStatusAsync(order.Id, OrderStatus.CONFIRMED, PaymentStatus.PAID);
    }

    public async Task<SepayQrResponse> CreatePaymentQrAsync(string orderId, string callbackUrl)
    {
        var order = await _unitOfWork.OrderRepository.GetByIdWithItems(orderId);
        if (order == null)
        {
            throw new ErrorException(StatusCodes.Status404NotFound, ApiCodes.NOT_FOUND, "Order not found");
        }

        if (order.PaymentStatus != PaymentStatus.PENDING)
        {
            throw new ErrorException(StatusCodes.Status400BadRequest, ApiCodes.BAD_REQUEST,
                "Order is not in pending payment status or already paid");
        }

        var qrUrl = GenerateSepayQrUrl(
            accountNumber: _sepaySettings.AccountNumber,
            bankCode: _sepaySettings.BankCode,
            amount: order.TotalAmount,
            description: $"SEVQR{order.Code}",
            template: "qronly",
            download: "true");

        var qrResponse = new SepayQrResponse
        {
            QrUrl = qrUrl,
            OrderWithItem = _mapper.Map<OrderWithItemResponseDto>(order),
        };
        return qrResponse;
    }

    private string GenerateSepayQrUrl(string accountNumber, string bankCode, float amount, string description,
        string template, string download)
    {
        string paymentCode = GeneratePaymentCode();
        string fullDescription = $"{paymentCode}{description}";
        var baseUrl = $"{_sepaySettings.ApiBaseUri}";

        var queryParams = new Dictionary<string, string>
        {
            ["acc"] = accountNumber,
            ["bank"] = bankCode,
            ["amount"] = amount.ToString("0"),
            ["des"] = fullDescription,
            ["template"] = template,
            ["download"] = download
        };

        var queryString = string.Join("&", queryParams
            .Where(kv => !string.IsNullOrEmpty(kv.Value))
            .Select(kv => $"{Uri.EscapeDataString(kv.Key)}={Uri.EscapeDataString(kv.Value)}"));

        return $"{baseUrl}?{queryString}";
    }

    private string GeneratePaymentCode()
    {
        var prefix = "TRF";
        var randomPart = GenerateRandomString(7);
        return $"{prefix}{randomPart}";
    }

    private string GenerateRandomString(int length)
    {
        const string chars = "0123456789";
        var random = new Random();
        return new string(Enumerable.Repeat(chars, length)
            .Select(s => s[random.Next(s.Length)])
            .ToArray());
    }

    private bool ValidateAuthHeader(string authHeader)
    {
        return authHeader == $"Apikey {_sepaySettings.ApiKey}";
    }

    private string ExtractOrderCodeFromContent(string content)
    {
        var startIndex = content.IndexOf("SEVQR");
        if (startIndex != -1)
        {
            var orderCode = content.Substring(startIndex + "SEVQR".Length, 6);
            return orderCode;
        }
        return null;
    }
}