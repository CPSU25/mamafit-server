using MamaFit.AI.Dto;

namespace MamaFit.AI.Interfaces;

public interface IModelTrainingService
{
    Task<TrainingResult> TrainModelsAsync(bool forceRetrain = false);
    Task<bool> PrepareTrainingDataAsync();
    Task<ValidationResult> ValidateModelsAsync();
    Task<bool> ExportTrainingDataAsync(string path);
}