using System.Linq;
using System.Security.Cryptography;
using AutoMapper;
using MamaFit.BusinessObjects.DTO.NotificationDto;
using MamaFit.BusinessObjects.DTO.OrderDto;
using MamaFit.BusinessObjects.DTO.OrderItemDto;
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
    private readonly INotificationService _notificationService;
    private readonly IOrderItemService _orderItemService;

    public SepayService(IUnitOfWork unitOfWork,
        IOptions<SepaySettings> sepaySettings,
        IValidationService validationService,
        IOrderService orderService,
        ITransactionService transactionService,
        IMapper mapper,
        INotificationService notificationService,
        IOrderItemService orderItemService)
    {
        _unitOfWork = unitOfWork;
        _sepaySettings = sepaySettings.Value;
        _validationService = validationService;
        _orderService = orderService;
        _transactionService = transactionService;
        _mapper = mapper;
        _notificationService = notificationService;
        _orderItemService = orderItemService;
    }

    public async Task<string> GetPaymentStatusAsync(string orderId)
    {
        var order = await _unitOfWork.OrderRepository.GetByIdWithItems(orderId);
        _validationService.CheckNotFound(order, "Order not found");

        return order.PaymentStatus.ToString();
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

        var milestoneList = await _unitOfWork.MilestoneRepository.GetAllAsync();

        var assignRequests = order.OrderItems.Select(orderItem =>
        {
            var matchingMilestones = milestoneList
                .Where(m => m.ApplyFor != null && m.ApplyFor.Contains((ItemType)orderItem.ItemType))
                .Select(m => m.Id)
                .ToList();

            return new AssignTaskToOrderItemRequestDto
            {
                OrderItemId = orderItem.Id,
                MilestoneIds = matchingMilestones
            };
        }).ToList();

        foreach(var assignRequest in assignRequests)
        {
            await _orderItemService.AssignTaskToOrderItemAsync(assignRequest);
        }

        await _transactionService.CreateTransactionAsync(payload, order.Id, order.Code);
        await _orderService.UpdateOrderStatusAsync(
            order.Id,
            order.PaymentType == PaymentType.DEPOSIT ? OrderStatus.CONFIRMED : OrderStatus.IN_PRODUCTION,
            order.PaymentType == PaymentType.DEPOSIT ? PaymentStatus.PAID_DEPOSIT : PaymentStatus.PAID_FULL
        );

        await _notificationService.SendAndSaveNotificationAsync(new NotificationRequestDto
        {
            NotificationTitle = "Payment Successful",
            NotificationContent = $"Your payment for order {order.Code} has been successfully processed.",
            Metadata = new Dictionary<string, string>()
            {
                { "orderId", order.Id },
                { "paymentStatus", PaymentStatus.PAID_FULL.ToString() }
            },
            Type = NotificationType.PAYMENT,
            ReceiverId = order.UserId
        });
    }

    public async Task<SepayQrResponse> CreatePaymentQrAsync(string orderId)
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
    private string GenerateSepayQrUrl(string accountNumber, string bankCode, decimal? amount, string description,
        string template, string download)
    {
        string paymentCode = GeneratePaymentCode();
        string fullDescription = $"{paymentCode}{description}";
        var baseUrl = $"{_sepaySettings.ApiBaseUri}";

        var queryParams = new Dictionary<string, string?>
        {
            ["acc"] = accountNumber,
            ["bank"] = bankCode,
            ["amount"] = amount?.ToString("0"),
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
        var prefix = "TF";
        var randomPart = GenerateRandomString(7);
        return $"{prefix}{randomPart}";
    }

    private string GenerateRandomString(int length)
    {
        const string chars = "0123456789";
        var data = new byte[length];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(data);
        }
        var result = new char[length];
        for (int i = 0; i < length; i++)
        {
            result[i] = chars[data[i] % chars.Length];
        }
        return new string(result);
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