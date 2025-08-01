using MamaFit.AI.Dto;
using MamaFit.AI.Infrastructure;
using MamaFit.AI.Models;

namespace MamaFit.AI.Interfaces;

public interface IMLModelService
{
    Task<MeasurementPrediction> PredictAsync(MeasurementPredictionRequest request);
    Task<bool> IsModelReady();
    Task<ModelMetrics> GetModelMetrics();
    Task<List<MeasurementPrediction>> BatchPredictAsync(List<MeasurementPredictionRequest> requests);
}