using MamaFit.BusinessObjects.Entity;
using MamaFit.Repositories.Infrastructure;

namespace MamaFit.Repositories.Interface;

public interface IMeasurementRepository : IGenericRepository<Measurement>
{
    Task<PaginatedList<Measurement>> GetAllAsync(int index, int pageSize, DateTime? startDate, DateTime? endDate);
    Task<Measurement?> GetLatestMeasurementByDiaryIdAsync(string diaryId);
    Task<Measurement?> GetByIdAsync(string id);
}