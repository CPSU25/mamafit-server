using MamaFit.BusinessObjects.DTO.SepayDto;
using MamaFit.BusinessObjects.DTO.TransactionDto;
using MamaFit.Repositories.Infrastructure;

namespace MamaFit.Services.Interface;

public interface ITransactionService
{
    Task<PaginatedList<TransactionResponseDto>> GetTransactionsAsync(int index, int pageSize,
        DateTime? startDate = null, DateTime? endDate = null);
    Task<TransactionResponseDto?> GetTransactionByOrderIdAsync(string orderId);
    Task CreateQrTransactionAsync(string orderId, SepayQrResponse qrResponse);
    Task CreateTransactionAsync(SepayWebhookPayload payload, string orderId, string paymentCode);
}    