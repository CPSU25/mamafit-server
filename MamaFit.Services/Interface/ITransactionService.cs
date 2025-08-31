using MamaFit.BusinessObjects.DTO.SepayDto;
using MamaFit.BusinessObjects.DTO.TransactionDto;
using MamaFit.BusinessObjects.Entity;
using MamaFit.BusinessObjects.Enum;
using MamaFit.Repositories.Infrastructure;

namespace MamaFit.Services.Interface;

public interface ITransactionService
{
    Task<PaginatedList<TransactionResponseDto>> GetTransactionsAsync(int index, int pageSize,
        DateTime? startDate = null, DateTime? endDate = null);
    Task<TransactionResponseDto?> GetTransactionByOrderIdAsync(string orderId);
    Task CreateTransactionAsync(SepayWebhookPayload payload, string orderId, string orderCode);
    Task<DashboardSummaryResponse> GetDashboardSummaryAsync(DateTime startTime, DateTime endTime);
    Task<List<RevenuePointDto>> GetRevenueAsync(string groupBy, string range, bool compareYoy);
    Task<OrderStatusResponse> GetOrderStatusAsync(string range);
    Task<BranchTopResponse> GetTopBranchesAsync(string metric, int limit, string range);
    Task<RecentOrdersResponse> GetRecentOrdersAsync(int limit);
    Task SendPaymentReceiptAsync(Order order, Transaction txn, PaymentStatus newPaymentStatus);
}    