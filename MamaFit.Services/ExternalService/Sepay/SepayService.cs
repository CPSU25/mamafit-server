using System.Linq;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using AutoMapper;
using MamaFit.BusinessObjects.DTO.NotificationDto;
using MamaFit.BusinessObjects.DTO.OrderDto;
using MamaFit.BusinessObjects.DTO.OrderItemDto;
using MamaFit.BusinessObjects.DTO.SepayDto;
using MamaFit.BusinessObjects.Entity;
using MamaFit.BusinessObjects.Enum;
using MamaFit.Repositories.Helper;
using MamaFit.Repositories.Implement;
using MamaFit.Repositories.Infrastructure;
using MamaFit.Services.ExternalService.Redis;
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
    private readonly ICacheService _cacheService;
    private readonly IWarrantyRequestService _warrantyRequestService;

    public SepayService(IUnitOfWork unitOfWork,
        IOptions<SepaySettings> sepaySettings,
        IValidationService validationService,
        IOrderService orderService,
        ITransactionService transactionService,
        IMapper mapper,
        INotificationService notificationService,
        IOrderItemService orderItemService,
        ICacheService cacheService,
        IWarrantyRequestService warrantyRequestService)
    {
        _unitOfWork = unitOfWork;
        _sepaySettings = sepaySettings.Value;
        _validationService = validationService;
        _orderService = orderService;
        _transactionService = transactionService;
        _mapper = mapper;
        _notificationService = notificationService;
        _orderItemService = orderItemService;
        _cacheService = cacheService;
        _warrantyRequestService = warrantyRequestService;
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

        if (order.PaymentStatus == PaymentStatus.PAID_FULL ||
            order.PaymentStatus == PaymentStatus.PAID_DEPOSIT_COMPLETED)
        {
            throw new ErrorException(StatusCodes.Status400BadRequest, ApiCodes.BAD_REQUEST, "Order already paid");
        }

        await _transactionService.CreateTransactionAsync(payload, order.Id, order.Code);

        if (order.PaymentType == PaymentType.DEPOSIT)
        {
            if (order.PaymentStatus == PaymentStatus.PENDING)
            {
                // Lần đầu thanh toán (cọc)
                await _orderService.UpdateOrderStatusAsync(
                    order.Id,
                    OrderStatus.CONFIRMED,
                    PaymentStatus.PAID_DEPOSIT
                );

                await _notificationService.SendAndSaveNotificationAsync(new NotificationRequestDto
                {
                    NotificationTitle = "Thanh toán cọc thành công",
                    NotificationContent =
                        $"Bạn đã thanh toán cọc cho đơn {order.Code}. Vui lòng thanh toán phần còn lại để chúng tôi tiến hành sản xuất.",
                    Metadata = new()
                    {
                        { "orderId", order.Id },
                        { "paymentStatus", PaymentStatus.PAID_DEPOSIT.ToString() },
                        { "remainingBalance", order.RemainingBalance?.ToString() ?? "0" }
                    },
                    Type = NotificationType.PAYMENT,
                    ReceiverId = order.UserId
                });

                await AssignTasksForOrder(order);
            }
            else if (order.PaymentStatus == PaymentStatus.PAID_DEPOSIT)
            {
                // Thanh toán phần còn lại
                await _orderService.UpdateOrderStatusAsync(
                    order.Id,
                    OrderStatus.PACKAGING,
                    PaymentStatus.PAID_DEPOSIT_COMPLETED
                );

                await _notificationService.SendAndSaveNotificationToMultipleAsync(new NotificationMultipleRequestDto
                {
                    NotificationTitle = "Thanh toán phần còn lại thành công",
                    NotificationContent = $"Bạn đã thanh toán phần còn lại cho đơn {order.Code}. Chúng tôi sẽ tiến hành sản xuất và giao hàng cho bạn.",
                    Metadata = new()
                    {
                        { "orderId", order.Id },
                        { "paymentStatus", PaymentStatus.PAID_DEPOSIT_COMPLETED.ToString() }
                    },
                    Type = NotificationType.PAYMENT,
                    ReceiverIds = new List<string>
                    {
                        order.UserId,
                        "1a3bcd123456789012345678901234561a3bcd12345678901234567890123456"
                    }
                });
            }
        }
        else
        {
            // Thanh toán full
            if (order.Type == OrderType.WARRANTY)
            {
                if (order.ShippingFee > 0)
                {
                    await _orderService.UpdateOrderStatusAsync(
                        order.Id,
                        OrderStatus.PICKUP_IN_PROGRESS,
                        PaymentStatus.PAID_FULL
                    );
                }
                await _orderService.UpdateOrderStatusAsync(
                    order.Id,
                    OrderStatus.IN_PROGRESS,
                    PaymentStatus.PAID_FULL
                );


                await _notificationService.SendAndSaveNotificationAsync(new NotificationRequestDto
                {
                    NotificationTitle = "Thanh toán phí bảo hành thành công",
                    NotificationContent =
                        $"Bạn đã thanh toán phí bảo hành cho đơn {order.Code}. Chúng tôi sẽ tiến hành lên đơn vận chuyển.",
                    Metadata = new()
                    {
                        { "orderId", order.Id },
                        { "paymentStatus", PaymentStatus.PAID_FULL.ToString() }
                    },
                    Type = NotificationType.PAYMENT,
                    ReceiverId = order.UserId
                });

                await _warrantyRequestService.AssignWarrantyTasksAfterPaidAsync(order);
                return;
            }

            await _orderService.UpdateOrderStatusAsync(
                order.Id,
                OrderStatus.CONFIRMED,
                PaymentStatus.PAID_FULL
            );

            await _notificationService.SendAndSaveNotificationAsync(new NotificationRequestDto
            {
                NotificationTitle = "Thanh toán thành công",
                NotificationContent = $"Bạn đã thanh toán thành công đơn {order.Code}. Chúng tôi sẽ tiến hành sản xuất và giao hàng cho bạn.",
                Metadata = new()
                {
                    { "orderId", order.Id },
                    { "paymentStatus", PaymentStatus.PAID_FULL.ToString() }
                },
                Type = NotificationType.PAYMENT,
                ReceiverId = order.UserId
            });
            await AssignTasksForOrder(order);
        }
    }

    public async Task<SepayQrResponse> CreatePaymentQrAsync(string orderId)
    {
        var order = await _unitOfWork.OrderRepository.GetByIdWithItems(orderId);
        if (order == null)
        {
            throw new ErrorException(StatusCodes.Status404NotFound, ApiCodes.NOT_FOUND, "Order not found");
        }

        if (order.PaymentStatus == PaymentStatus.PAID_FULL ||
            order.PaymentStatus == PaymentStatus.PAID_DEPOSIT_COMPLETED)
        {
            throw new ErrorException(StatusCodes.Status400BadRequest, ApiCodes.BAD_REQUEST,
                "Order has already been fully paid");
        }

        decimal paymentAmount;
        if (order.PaymentType == PaymentType.DEPOSIT)
        {
            if (order.PaymentStatus == PaymentStatus.PAID_DEPOSIT)
            {
                // Lần 2 thanh toán
                paymentAmount = order.RemainingBalance ?? 0;
            }
            else
            {
                // Lần đầu thanh toán
                paymentAmount = order.TotalPaid ?? 0;
            }
        }
        else
        {
            // Thanh toán 1 lần
            paymentAmount = order.TotalAmount ?? 0;
        }

        var qrUrl = GenerateSepayQrUrl(
            accountNumber: _sepaySettings.AccountNumber,
            bankCode: _sepaySettings.BankCode,
            amount: paymentAmount,
            description: $"{order.Code}",
            template: "qronly",
            download: "true");

        return new SepayQrResponse
        {
            QrUrl = qrUrl,
            OrderWithItem = _mapper.Map<OrderWithItemResponseDto>(order),
        };
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

    private async Task AssignTasksForOrder(Order order)
    {
        var milestoneList = await _unitOfWork.MilestoneRepository.GetAllWithInclude();

        var assignRequests = order.OrderItems.Select(orderItem =>
        {
            var matchingMilestones = milestoneList
                .Where(m => m.ApplyFor.Contains((ItemType)orderItem.ItemType!))
                .Select(m => m.Id)
                .ToList();

            if (orderItem.OrderItemAddOnOptions != null && orderItem.OrderItemAddOnOptions.Any())
            {
                var addOnMilestones = milestoneList
                    .Where(m => m.ApplyFor.Contains(ItemType.ADD_ON))
                    .Select(m => m.Id);
                matchingMilestones.AddRange(addOnMilestones);
            }

            return new AssignTaskToOrderItemRequestDto
            {
                OrderItemId = orderItem.Id,
                MilestoneIds = matchingMilestones
            };
        }).ToList();

        foreach (var assignRequest in assignRequests)
        {
            await _orderItemService.AssignTaskToOrderItemAsync(assignRequest);
        }

        await _cacheService.RemoveByPrefixAsync("MilestoneAchiveOrderItemResponseDto_");
    }

    private bool ValidateAuthHeader(string authHeader)
    {
        return authHeader == $"Apikey {_sepaySettings.ApiKey}";
    }

    private string ExtractOrderCodeFromContent(string content)
    {
        var match = Regex.Match(content, @"ORD\d{7}");
        return match.Success ? match.Value : null;
    }
}