using MamaFit.BusinessObjects.DTO.MeasurementDto;
using MamaFit.Repositories.Infrastructure;

namespace MamaFit.Services.Interface;

public interface IMeasurementDiaryService
{
    Task<PaginatedList<MeasurementDiaryResponseDto>> GetAllAsync(int index, int pageSize, string? nameSearch);
    Task<DiaryWithMeasurementDto> GetDiaryByIdAsync(string id);
    Task<List<MeasurementDiaryResponseDto>> GetDiariesByUserIdAsync(string userId);
    Task<bool> DeleteDiaryAsync(string id);
}