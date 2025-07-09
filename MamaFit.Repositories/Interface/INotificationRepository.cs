using MamaFit.BusinessObjects.Entity;
using MamaFit.BusinessObjects.Enum;
using MamaFit.Repositories.Implement;
using MamaFit.Repositories.Infrastructure;

namespace MamaFit.Repositories.Interface;

public interface INotificationRepository : IGenericRepository<Notification>
{
    Task<PaginatedList<Notification>> GetAllAsync(int index, int pageSize, string? search, NotificationType? type,
        string? sortBy);
    Task<PaginatedList<Notification>> GetAllByTokenAsync(string receiverId, int index, int pageSize, string? search, NotificationType? type, string? sortBy);
}