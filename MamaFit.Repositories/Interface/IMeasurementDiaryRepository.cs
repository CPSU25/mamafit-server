using MamaFit.BusinessObjects.Entity;
using MamaFit.Repositories.Infrastructure;

namespace MamaFit.Repositories.Interface;

public interface IMeasurementDiaryRepository : IGenericRepository<MeasurementDiary>
{
    Task<PaginatedList<MeasurementDiary>> GetAllDiariesAsync(int index, int pageSize, string? nameSearch);
}