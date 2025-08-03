using MamaFit.BusinessObjects.DTO.MeasurementDto;
using MamaFit.BusinessObjects.Entity;

namespace MamaFit.Services.ExternalService.AI;

public interface IAIMeasurementCalculationService
{
    Task<MeasurementDto> CalculateMeasurementsAsync(
            MeasurementDiaryDto diary,
            MeasurementCreateDto? currentInput,
            Measurement? lastMeasurement,
            int targetWeek);
    Task<bool> IsAvailable();
}