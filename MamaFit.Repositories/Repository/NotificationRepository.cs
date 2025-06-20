using MamaFit.BusinessObjects.DbContext;
using MamaFit.BusinessObjects.Entity;
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

    public async Task<PaginatedList<Notification>> GetAllAsync(int index, int pageSize, string? search)
    {
        var query = _dbSet.AsNoTracking().Where(n => !n.IsDeleted);
        if (!string.IsNullOrEmpty(search))
        {
            query = query.Where(n => n.NotificationTitle.Contains(search) || n.NotificationContent.Contains(search));
        }
        return await query.GetPaginatedList(index, pageSize);
    }
}