using MamaFit.BusinessObjects.Entity;
using MamaFit.Repositories.Implement;
using MamaFit.Repositories.Infrastructure;

namespace MamaFit.Repositories.Interface;

public interface IWarrantyHistoryRepository : IGenericRepository<WarrantyHistory>
{
    Task<PaginatedList<WarrantyHistory>> GetAllAsync(int index, int pageSize, DateTime? startDate, DateTime? endDate);
}