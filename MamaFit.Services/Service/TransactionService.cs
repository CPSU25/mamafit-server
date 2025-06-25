using AutoMapper;
using MamaFit.BusinessObjects.DTO.SepayDto;
using MamaFit.BusinessObjects.DTO.TransactionDto;
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
    
    public TransactionService(IUnitOfWork unitOfWork, IMapper mapper, IValidationService validation, ICacheService cache)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _validation = validation;
        _cache = cache;
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
            throw new ErrorException(StatusCodes.Status409Conflict, ApiCodes.CONFLICT, "Transaction already exists");
        }
        
        var transaction = new Transaction
        {
            OrderId = orderId,
            SepayId = payload.id,
            Gateway = payload.gateway,
            TransactionDate = DateTime.Parse(payload.transactionDate),
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
    }
    
    private string GeneratePaymentCode() 
    {
        string prefix = "PAY";
        string datePart = DateTime.UtcNow.ToString("yyyyMMdd");
        string randomPart = new Random().Next(1000, 9999).ToString();
        return $"{prefix}-{datePart}-{randomPart}";
    }
}