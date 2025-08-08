using AutoMapper;
using MamaFit.BusinessObjects.DTO.SepayDto;
using MamaFit.BusinessObjects.DTO.TransactionDto;
using MamaFit.BusinessObjects.DTO.RealtimeDto;
using MamaFit.BusinessObjects.Entity;
using MamaFit.Repositories.Implement;
using MamaFit.Repositories.Infrastructure;
using MamaFit.Services.ExternalService.Redis;
using MamaFit.Services.Interface;
using Microsoft.AspNetCore.Http;

namespace MamaFit.Services.Service;

public class TransactionService : ITransactionService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IValidationService _validation;
    private readonly ICacheService _cache;
    private readonly IRealtimeEventService _realtimeEventService;
    
    public TransactionService(IUnitOfWork unitOfWork, IMapper mapper, IValidationService validation, ICacheService cache, IRealtimeEventService realtimeEventService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _validation = validation;
        _cache = cache;
        _realtimeEventService = realtimeEventService;
    }
    
    public async Task<PaginatedList<TransactionResponseDto>> GetTransactionsAsync(int index, int pageSize, DateTime? startDate = null, DateTime? endDate = null)
    {
        var transactions = await _unitOfWork.TransactionRepository.GetAllAsync(index, pageSize, startDate, endDate);
        var responseItems = transactions.Items
            .Select(transaction => _mapper.Map<TransactionResponseDto>(transaction))
            .ToList();

        return new PaginatedList<TransactionResponseDto>(
            responseItems,
            transactions.TotalCount,
            transactions.PageNumber,
            pageSize
        );
    }
    
    public async Task<TransactionResponseDto?> GetTransactionByOrderIdAsync(string orderId)
    {
        var transaction = await _unitOfWork.TransactionRepository.GetByOrderIdAsync(orderId);
        _validation.CheckNotFound(transaction, "Transaction not found");
        return _mapper.Map<TransactionResponseDto>(transaction);
    }
    
    public async Task CreateTransactionAsync(SepayWebhookPayload payload, string orderId, string orderCode)
    {
        var exist = await _unitOfWork.TransactionRepository
            .FindAsync(x => x.SepayId == payload.id);
        if (exist != null)
        {
            throw new ErrorException(StatusCodes.Status409Conflict, ApiCodes.CONFLICT, "Transaction already processed");
        }
        
        DateTime? transactionDateUtc = DateTime.TryParse(payload.transactionDate, out var parsedDate)
            ? parsedDate.ToUniversalTime()
            : null;
        
        var transaction = new Transaction
        {
            OrderId = orderId,
            SepayId = payload.id,
            Gateway = payload.gateway,
            TransactionDate = transactionDateUtc,
            AccountNumber = payload.accountNumber,
            Code = payload.code,
            Content = payload.content,
            TransferType = payload.transferType,
            TransferAmount = payload.transferAmount,
            Accumulated = payload.accumulated,
            SubAccount = payload.subAccount,
            ReferenceCode = payload.referenceCode,
            Description = $"Payment received via {payload.gateway} for order {orderCode}"
        };

        await _unitOfWork.TransactionRepository.InsertAsync(transaction);
        await _unitOfWork.SaveChangesAsync();
        
        // Get order details for user notification
        var order = await _unitOfWork.OrderRepository.GetByIdNotDeletedAsync(orderId);
        if (order != null)
        {
            // Publish realtime event for payment received
            await _realtimeEventService.PublishPaymentStatusChangedAsync(new PaymentStatusChangedEventDto
            {
                EventType = RealtimeEventTypes.PAYMENT_RECEIVED,
                EntityId = transaction.Id,
                EntityType = RealtimeEntityTypes.TRANSACTION,
                Data = new
                {
                    TransactionId = transaction.Id,
                    OrderId = orderId,
                    OrderCode = orderCode,
                    Amount = payload.transferAmount,
                    Gateway = payload.gateway,
                    PaymentDate = transactionDateUtc,
                    UserId = order.UserId
                },
                TargetUserId = order.UserId,
                TransactionId = transaction.Id,
                OrderId = orderId,
                Amount = (decimal)payload.transferAmount,
                PaymentStatus = order.PaymentStatus,
                Gateway = payload.gateway,
                OrderCode = orderCode,
                Metadata = new Dictionary<string, object>
                {
                    { "orderId", orderId },
                    { "orderCode", orderCode },
                    { "userId", order.UserId },
                    { "amount", payload.transferAmount }
                }
            });
        }
    }
}