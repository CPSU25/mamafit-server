using System.Globalization;
using System.Net.Http.Json;
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
using Newtonsoft.Json;

namespace MamaFit.Services.ExternalService.Sepay;

public class SepayService : ISepayService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly SepaySettings _sepaySettings;
    private readonly IMapper _mapprer;
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
        _mapprer = mapper;
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
            throw new ErrorException(StatusCodes.Status400BadRequest, ApiCodes.INVALID_INPUT, "No order code found in payment content");
        }
        
        var order = await _unitOfWork.TransactionRepository.GetOrderByPaymentCodeAsync(orderCode);
        
        await _transactionService.CreateTransactionAsync(payload, order.Id, order.Code);
        
        await _orderService.UpdateOrderStatusAsync(
            order.Id, 
            OrderStatus.CONFIRMED,
            PaymentStatus.PAID);
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
        throw new ErrorException(StatusCodes.Status400BadRequest, ApiCodes.BAD_REQUEST, "Order is not in pending payment status");
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
        OrderWithItem = _mapprer.Map<OrderWithItemResponseDto>(order),
    };
    return qrResponse;
}

private string GenerateSepayQrUrl(string accountNumber, string bankCode, float amount, string description, string template, string download)
{
    var baseUrl = $"{_sepaySettings.ApiBaseUri}";
    
    var queryParams = new Dictionary<string, string>
    {
        ["acc"] = accountNumber,
        ["bank"] = bankCode,
        ["amount"] = amount.ToString("0"), 
        ["des"] = description,
        ["template"] = template,
        ["download"] = download
    };

    // Tạo query string
    var queryString = string.Join("&", queryParams
        .Where(kv => !string.IsNullOrEmpty(kv.Value))
        .Select(kv => $"{Uri.EscapeDataString(kv.Key)}={Uri.EscapeDataString(kv.Value)}"));

    return $"{baseUrl}?{queryString}";
}
    
    private bool ValidateAuthHeader(string authHeader)
    {
        return authHeader == $"ApiKey {_sepaySettings.ApiKey}";
    }

    private string ExtractOrderCodeFromContent(string content)
    {
        // Tìm mã đơn hàng theo format SEVQR{orderCode}
        var match = Regex.Match(content, @"SEVQR(\w+)");
        return match.Success ? match.Groups[1].Value : null;
    }
    
    private string ExtractPaymentCode(string content)
    {
        var match = Regex.Match(content, @"PAY\d+");
        return match.Success ? match.Value : null;
    }
}