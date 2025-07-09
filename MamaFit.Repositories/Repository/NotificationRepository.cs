using MamaFit.BusinessObjects.DbContext;
using MamaFit.BusinessObjects.Entity;
using MamaFit.BusinessObjects.Enum;
using MamaFit.Repositories.Implement;
using MamaFit.Repositories.Infrastructure;
using MamaFit.Repositories.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace MamaFit.Repositories.Repository;

public class NotificationRepository : GenericRepository<Notification>, INotificationRepository
{
    public NotificationRepository(ApplicationDbContext context, IHttpContextAccessor accessor) : base(context, accessor)
    {
    }

    public async Task<PaginatedList<Notification>> GetAllAsync(int index, int pageSize, string? search, NotificationType? type, string? sortBy)
    {
        var query = _dbSet.Where(x => !x.IsDeleted);
        if (!string.IsNullOrWhiteSpace(search))
        {
            query = query.Where(x =>
                (x.NotificationTitle != null && x.NotificationTitle.Contains(search)) ||
                (x.NotificationContent != null && x.NotificationContent.Contains(search))
            );
        }

        if (type != null)
        {
            query = query.Where(x => x.Type == type);
        }

        query = sortBy?.ToLower() switch
        {
            "createdat_asc" => query.OrderBy(x => x.CreatedAt),
            "createdat_desc" => query.OrderByDescending(x => x.CreatedAt),
            _ => query.OrderByDescending(x => x.CreatedAt)
        };
        return await query.GetPaginatedList(index, pageSize);
    }

    public async Task<PaginatedList<Notification>> GetAllByTokenAsync(string receiverId, int index, int pageSize,
        string? search, NotificationType? type, string? sortBy)
    {
        var query = _dbSet.Where(x => !x.IsDeleted && x.ReceiverId == receiverId);

        if (!string.IsNullOrWhiteSpace(search))
        {
            query = query.Where(x =>
                (x.NotificationTitle != null && x.NotificationTitle.Contains(search)) ||
                (x.NotificationContent != null && x.NotificationContent.Contains(search))
            );
        }

        if (type != null)
        {
            query = query.Where(x => x.Type == type);
        }

        query = sortBy?.ToLower() switch
        {
            "createdat_asc" => query.OrderBy(x => x.CreatedAt),
            "createdat_desc" => query.OrderByDescending(x => x.CreatedAt),
            _ => query.OrderByDescending(x => x.CreatedAt)
        };

        return await query.GetPaginatedList(index, pageSize);
    }
}