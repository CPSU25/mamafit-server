using MamaFit.BusinessObjects.DTO.MeasurementDto;

namespace MamaFit.Services.Interface;

public interface IMeasurementService
{
    Task<MeasurementDto> GenerateMeasurementPreviewAsync(MeasurementCreateDto dto);
    Task<MeasurementDto> GenerateMeasurementDiaryPreviewAsync(MeasurementDiaryDto dto);
    Task<string> CreateDiaryWithMeasurementAsync(MeasurementDiaryCreateRequest request);
    Task<MeasurementDto> CreateMeasurementAsync(CreateMeasurementDto dto);
}
