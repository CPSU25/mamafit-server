using System.Net.Http.Json;
using System.Text.RegularExpressions;
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
    private readonly IValidationService _validationService;
    private readonly IOrderService _orderService;
    private readonly ITransactionService _transactionService;

    public SepayService(IUnitOfWork unitOfWork,
        IOptions<SepaySettings> sepaySettings,
        IValidationService validationService,
        IOrderService orderService,
        ITransactionService transactionService)
    {
        _unitOfWork = unitOfWork;
        _sepaySettings = sepaySettings.Value;
        _validationService = validationService;
        _orderService = orderService;
        _transactionService = transactionService;
    }

    public async Task ProcessPaymentWebhookAsync(SepayWebhookPayload payload, string authHeader)
    {
        if (!ValidateAuthHeader(authHeader))
        {
            throw new ErrorException(StatusCodes.Status401Unauthorized, ApiCodes.UNAUTHORIZED, "Invalid API key");
        }
        
        
        var paymentCode = ExtractPaymentCode(payload.content);
        if (string.IsNullOrEmpty(paymentCode))
        {
            throw new ErrorException(StatusCodes.Status400BadRequest, ApiCodes.INVALID_INPUT, "No payment code found");
        }
        
        var order = await _unitOfWork.TransactionRepository.GetOrderByPaymentCodeAsync(paymentCode);
        
        await _transactionService.CreateTransactionAsync(payload, order.Id, paymentCode);
        
        await _orderService.UpdateOrderStatusAsync(
            order.Id, 
            OrderStatus.CONFIRMED,
            PaymentStatus.PAID);
    }
    
    public async Task<SepayQrResponse> CreatePaymentQrAsync(string orderId, string callbackUrl)
{
    // 1. Kiểm tra đơn hàng
    var order = await _unitOfWork.OrderRepository.GetByIdNotDeletedAsync(orderId);
    if (order == null)
    {
        throw new ErrorException(StatusCodes.Status404NotFound, ApiCodes.NOT_FOUND, "Order not found");
    }

    if (order.PaymentStatus != PaymentStatus.PENDING)
    {
        throw new ErrorException(StatusCodes.Status400BadRequest, ApiCodes.BAD_REQUEST, "Order is not in pending payment status");
    }

    // 2. Tạo URL QR code theo docs Sepay
    var qrUrl = GenerateSepayQrUrl(
        accountNumber: _sepaySettings.AccountNumber,
        bankCode: _sepaySettings.BankCode,
        amount: order.TotalAmount,
        description: $"DH{order.Code}", // Format theo yêu cầu Sepay: DH + mã đơn
        template: "compact");

    // 3. Tạo response object
    var qrResponse = new SepayQrResponse
    {
        code = "SUCCESS",
        desc = "QR code generated successfully",
        data = new QrData
        {
            qrCode = qrUrl,
            qrDataURL = qrUrl
        }
    };

    // 4. Lưu transaction (nếu cần)
    await _transactionService.CreateQrTransactionAsync(orderId, qrResponse);

    return qrResponse;
}

private string GenerateSepayQrUrl(string accountNumber, string bankCode, float amount, string description, string template = "compact")
{
    var baseUrl = "https://qr.sepay.vn/img";
    
    var queryParams = new Dictionary<string, string>
    {
        ["acc"] = accountNumber,
        ["bank"] = bankCode,
        ["amount"] = amount.ToString("0"), 
        ["des"] = description,
        ["template"] = template,
        ["download"] = "false"
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

    private string ExtractPaymentCode(string content)
    {
        var match = Regex.Match(content, @"PAY\d+");
        return match.Success ? match.Value : null;
    }
}