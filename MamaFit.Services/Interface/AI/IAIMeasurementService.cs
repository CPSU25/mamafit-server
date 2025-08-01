using MamaFit.BusinessObjects.DTO.AI;
using MamaFit.BusinessObjects.DTO.MeasurementDto;

namespace MamaFit.Services.Interface.AI;

public interface IAIMeasurementService
{
    Task<MeasurementPredictionDto> PredictMeasurementsAsync(MeasurementPredictionRequestDto request);
    Task<AITrainingResultDto> TrainModelsAsync(bool forceRetrain = false);
    Task<bool> IsModelReadyAsync();
    Task<Dictionary<string, ModelMetricsDto>> GetModelMetricsAsync();
    Task<MeasurementDto> EnhanceMeasurementWithAIAsync(MeasurementCreateDto dto);
}