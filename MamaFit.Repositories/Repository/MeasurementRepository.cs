using MamaFit.BusinessObjects.DBContext;
using MamaFit.BusinessObjects.Entity;
using MamaFit.Repositories.Implement;
using MamaFit.Repositories.Infrastructure;
using MamaFit.Repositories.Interface;
using Microsoft.AspNetCore.Http;

namespace MamaFit.Repositories.Repository;

public class MeasurementRepository : GenericRepository<Measurement>, IMeasurementRepository
{
    public MeasurementRepository(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor) 
        : base(context, httpContextAccessor)
    {
    }

    public async Task<PaginatedList<Measurement>> GetAllAsync(int index, int pageSize)
    {
        var query = _dbSet.Where(x => !x.IsDeleted);
        return await query.GetPaginatedList(index, pageSize);
    }

    public async Task<Measurement?> GetByIdAsync(string id)
    {
        return await GetByIdNotDeletedAsync(id);   
    }
}