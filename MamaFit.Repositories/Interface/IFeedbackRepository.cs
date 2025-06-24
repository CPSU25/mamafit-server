using MamaFit.BusinessObjects.Entity;
using MamaFit.Repositories.Implement;
using MamaFit.Repositories.Infrastructure;

namespace MamaFit.Repositories.Interface;

public interface IFeedbackRepository : IGenericRepository<Feedback>
{
    Task<PaginatedList<Feedback>> GetAllAsync(int index, int pageSize, DateTime? startDate, DateTime? endDate);
}