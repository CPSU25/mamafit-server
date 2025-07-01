using MamaFit.BusinessObjects.DTO.MeasurementDto;
using MamaFit.Repositories.Infrastructure;

namespace MamaFit.Services.Interface;

public interface IMeasurementService
{
    Task GenerateMissingMeasurementsAsync();
    Task CheckAndSendRemindersAsync();
    Task<PaginatedList<MeasurementResponseDto>> GetAllMeasurementsAsync(int index, int pageSize, DateTime? startDate,
        DateTime? endDate);
    Task<MeasurementResponseDto> GetMeasurementByIdAsync(string id);
    Task<MeasurementDto> GenerateMeasurementPreviewAsync(MeasurementCreateDto dto);
    Task<MeasurementDto> GenerateMeasurementDiaryPreviewAsync(MeasurementDiaryDto dto);
    Task<string> CreateDiaryWithMeasurementAsync(MeasurementDiaryCreateRequest request);
    Task<MeasurementDto> CreateMeasurementAsync(CreateMeasurementDto dto);
    Task<MeasurementDto> UpdateMeasurementAsync(string id, UpdateMeasurementDto dto);
    Task<bool> DeleteMeasurementAsync(string id);
}
