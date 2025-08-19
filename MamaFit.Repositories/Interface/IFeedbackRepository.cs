using MamaFit.BusinessObjects.Entity;
using MamaFit.Repositories.Implement;
using MamaFit.Repositories.Infrastructure;

namespace MamaFit.Repositories.Interface;

public interface IFeedbackRepository : IGenericRepository<Feedback>
{
    Task<PaginatedList<Feedback>> GetAllAsync(int index, int pageSize, DateTime? startDate, DateTime? endDate);
    Task<List<Feedback>> GetAllByUserId(string userId);
    Task<Feedback> GetFeedbackByUserIdAndOrderItemId(string userId, string orderItemId);
    Task<List<Feedback>> GetAllByDressId(string dressId);
}