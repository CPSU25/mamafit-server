using MamaFit.BusinessObjects.Entity;
using MamaFit.Repositories.Implement;
using MamaFit.Repositories.Infrastructure;

namespace MamaFit.Repositories.Interface;

public interface INotificationRepository : IGenericRepository<Notification>
{
    Task<PaginatedList<Notification>> GetAllAsync(int index, int pageSize, string? search);
}