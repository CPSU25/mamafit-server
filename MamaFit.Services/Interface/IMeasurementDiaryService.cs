using MamaFit.BusinessObjects.DTO.MeasurementDto;
using MamaFit.Repositories.Infrastructure;

namespace MamaFit.Services.Interface;

public interface IMeasurementDiaryService
{
    Task<PaginatedList<MeasurementDiaryResponseDto>> GetAllAsync(int index, int pageSize, string? nameSearch);
    Task<DiaryWithMeasurementDto> GetDiaryByIdAsync(string id, DateTime? startDate, DateTime? endDate);

    Task<PaginatedList<MeasurementDiaryResponseDto>> GetDiariesByUserIdAsync(int index, int pageSize, string userId,
        string? nameSearch = null);
    Task<bool> DeleteDiaryAsync(string id);
    Task SetActiveDiaryAsync(string diaryId, string accessToken);
    Task<MeasurementResponseDto> CalculateWeeksPregnantByDiaryIdAsync(string diaryId);
}