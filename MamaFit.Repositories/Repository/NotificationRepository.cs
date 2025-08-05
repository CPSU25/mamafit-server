using MamaFit.BusinessObjects.DBContext;
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

    public async Task<PaginatedList<Notification>> GetAllAsync(int index, int pageSize, string? search, NotificationType? type, EntitySortBy? sortBy)
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

        query = sortBy switch
        {
            EntitySortBy.CREATED_AT_ASC => query.OrderBy(u => u.CreatedAt),
            EntitySortBy.CREATED_AT_DESC => query.OrderByDescending(u => u.CreatedAt),
            _ => query.OrderByDescending(x => x.CreatedAt)
        };
        return await query.GetPaginatedList(index, pageSize);
    }

    public async Task<PaginatedList<Notification>> GetAllByTokenAsync(string receiverId, int index, int pageSize,
        string? search, NotificationType? type, EntitySortBy? sortBy)
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

        query = sortBy switch
        {
            EntitySortBy.CREATED_AT_ASC => query.OrderBy(u => u.CreatedAt),
            EntitySortBy.CREATED_AT_DESC => query.OrderByDescending(u => u.CreatedAt),
            _ => query.OrderByDescending(x => x.CreatedAt)
        };

        return await query.GetPaginatedList(index, pageSize);
    }

    public async Task<List<Notification>> GetAllByUserId(string userId)
    {
        var result = await _dbSet.Where(x => x.ReceiverId == userId && !x.IsDeleted).ToListAsync();
        return result;
    }
}