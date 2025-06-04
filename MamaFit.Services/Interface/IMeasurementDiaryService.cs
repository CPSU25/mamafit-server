using MamaFit.BusinessObjects.DTO.MeasurementDiaryDto;
using MamaFit.Repositories.Infrastructure;

namespace MamaFit.Services.Interface;

public interface IMeasurementDiaryService
{
    Task<PaginatedList<MeasurementDiaryResponseDto>> GetAllAsync(int index, int pageSize, string? nameSearch);
    Task<MeasurementDiaryResponseDto> GetByIdAsync(string id);
    Task<MeasurementDiaryResponseDto> CreateAsync(MeasurementDiaryRequestDto requestDto);
    Task<MeasurementDiaryResponseDto> UpdateAsync(string id, UpdateMeasurementDiaryDto model);
    Task DeleteAsync(string id);
}