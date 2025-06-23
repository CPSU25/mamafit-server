using System.Text.RegularExpressions;
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