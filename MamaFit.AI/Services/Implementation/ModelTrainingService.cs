// using MamaFit.AI.Dto;
// using MamaFit.AI.Infrastructure;
// using MamaFit.AI.Interfaces;
// using MamaFit.AI.Models;
// using MamaFit.Repositories.Implement;
// using Microsoft.Extensions.Logging;
// using Microsoft.ML;
//
// namespace MamaFit.AI.Services.Implementation;
//
// public class ModelTrainingService : IModelTrainingService
//     {
//         private readonly MLContext _mlContext;
//         private readonly ILogger<ModelTrainingService> _logger;
//         private readonly IUnitOfWork _unitOfWork;
//         private readonly string _modelsPath = "TrainedModels";
//         private readonly string _dataPath = "TrainingData";
//
//         public ModelTrainingService(
//             ILogger<ModelTrainingService> logger,
//             IUnitOfWork unitOfWork)
//         {
//             _logger = logger;
//             _unitOfWork = unitOfWork;
//             _mlContext = new MLContext(seed: 42);
//             
//             // Ensure directories exist
//             Directory.CreateDirectory(_modelsPath);
//             Directory.CreateDirectory(_dataPath);
//         }
//
//         public async Task<TrainingResult> TrainModelsAsync(bool forceRetrain = false)
//         {
//             var result = new TrainingResult { TrainedAt = DateTime.UtcNow };
//
//             try
//             {
//                 // Check if models exist and skip if not forcing retrain
//                 if (!forceRetrain && ModelsExist())
//                 {
//                     result.Success = true;
//                     result.Message = "Models already exist. Use forceRetrain=true to retrain.";
//                     return result;
//                 }
//
//                 _logger.LogInformation("Starting model training...");
//
//                 // Prepare training data
//                 if (!await PrepareTrainingDataAsync())
//                 {
//                     result.Success = false;
//                     result.Message = "Failed to prepare training data";
//                     return result;
//                 }
//
//                 // Load training data
//                 var dataPath = Path.Combine(_dataPath, "training_data.csv");
//                 var dataView = _mlContext.Data.LoadFromTextFile<MeasurementData>(
//                     dataPath,
//                     hasHeader: true,
//                     separatorChar: ',');
//
//                 // Split data for training and testing
//                 var split = _mlContext.Data.TrainTestSplit(dataView, testFraction: 0.2);
//
//                 // Train models for each measurement
//                 result.Metrics["Weight"] = await TrainWeightModel(split);
//                 result.Metrics["Bust"] = await TrainBustModel(split);
//                 result.Metrics["Waist"] = await TrainWaistModel(split);
//                 result.Metrics["Hip"] = await TrainHipModel(split);
//
//                 // Save metrics
//                 await SaveMetrics(result.Metrics);
//
//                 result.Success = true;
//                 result.Message = "All models trained successfully";
//                 
//                 _logger.LogInformation("Model training completed successfully");
//             }
//             catch (Exception ex)
//             {
//                 _logger.LogError(ex, "Error during model training");
//                 result.Success = false;
//                 result.Message = $"Training failed: {ex.Message}";
//             }
//
//             return result;
//         }
//
//         private async Task<ModelMetrics> TrainWeightModel(DataOperationsCatalog.TrainTestData split)
//         {
//             _logger.LogInformation("Training weight prediction model...");
//
//             var pipeline = _mlContext.Transforms.CopyColumns("Label", nameof(MeasurementData.TargetWeight))
//                 .Append(_mlContext.Transforms.Concatenate("Features",
//                     nameof(MeasurementData.Age),
//                     nameof(MeasurementData.Height),
//                     nameof(MeasurementData.PrePregnancyWeight),
//                     nameof(MeasurementData.CurrentWeight),
//                     nameof(MeasurementData.CurrentWeek),
//                     nameof(MeasurementData.TargetWeek)))
//                 .Append(_mlContext.Transforms.NormalizeMinMax("Features"))
//                 .Append(_mlContext.Regression.Trainers.FastTree(
//                     numberOfTrees: 100,
//                     minimumExampleCountPerLeaf: 10,
//                     learningRate: 0.2));
//
//             var model = pipeline.Fit(split.TrainSet);
//             
//             // Evaluate the model
//             var predictions = model.Transform(split.TestSet);
//             var metrics = _mlContext.Regression.Evaluate(predictions);
//
//             // Save the model
//             var modelPath = Path.Combine(_modelsPath, "weight_model.zip");
//             _mlContext.Model.Save(model, split.TrainSet.Schema, modelPath);
//
//             return new ModelMetrics
//             {
//                 MeanAbsoluteError = metrics.MeanAbsoluteError,
//                 MeanSquaredError = metrics.MeanSquaredError,
//                 RootMeanSquaredError = metrics.RootMeanSquaredError,
//                 RSquared = metrics.RSquared
//             };
//         }
//
//         private async Task<ModelMetrics> TrainBustModel(DataOperationsCatalog.TrainTestData split)
//         {
//             _logger.LogInformation("Training bust prediction model...");
//
//             var pipeline = _mlContext.Transforms.CopyColumns("Label", nameof(MeasurementData.TargetBust))
//                 .Append(_mlContext.Transforms.Concatenate("Features",
//                     nameof(MeasurementData.Height),
//                     nameof(MeasurementData.CurrentBust),
//                     nameof(MeasurementData.CurrentWeight),
//                     nameof(MeasurementData.CurrentWeek),
//                     nameof(MeasurementData.TargetWeek)))
//                 .Append(_mlContext.Transforms.NormalizeMinMax("Features"))
//                 .Append(_mlContext.Regression.Trainers.FastTree());
//
//             var model = pipeline.Fit(split.TrainSet);
//             var predictions = model.Transform(split.TestSet);
//             var metrics = _mlContext.Regression.Evaluate(predictions);
//
//             var modelPath = Path.Combine(_modelsPath, "bust_model.zip");
//             _mlContext.Model.Save(model, split.TrainSet.Schema, modelPath);
//
//             return new ModelMetrics
//             {
//                 MeanAbsoluteError = metrics.MeanAbsoluteError,
//                 MeanSquaredError = metrics.MeanSquaredError,
//                 RootMeanSquaredError = metrics.RootMeanSquaredError,
//                 RSquared = metrics.RSquared
//             };
//         }
//
//         private async Task<ModelMetrics> TrainWaistModel(DataOperationsCatalog.TrainTestData split)
//         {
//             _logger.LogInformation("Training waist prediction model...");
//
//             var pipeline = _mlContext.Transforms.CopyColumns("Label", nameof(MeasurementData.TargetWaist))
//                 .Append(_mlContext.Transforms.Concatenate("Features",
//                     nameof(MeasurementData.Height),
//                     nameof(MeasurementData.CurrentWaist),
//                     nameof(MeasurementData.CurrentWeight),
//                     nameof(MeasurementData.CurrentWeek),
//                     nameof(MeasurementData.TargetWeek)))
//                 .Append(_mlContext.Transforms.NormalizeMinMax("Features"))
//                 .Append(_mlContext.Regression.Trainers.FastTree());
//
//             var model = pipeline.Fit(split.TrainSet);
//             var predictions = model.Transform(split.TestSet);
//             var metrics = _mlContext.Regression.Evaluate(predictions);
//
//             var modelPath = Path.Combine(_modelsPath, "waist_model.zip");
//             _mlContext.Model.Save(model, split.TrainSet.Schema, modelPath);
//
//             return new ModelMetrics
//             {
//                 MeanAbsoluteError = metrics.MeanAbsoluteError,
//                 MeanSquaredError = metrics.MeanSquaredError,
//                 RootMeanSquaredError = metrics.RootMeanSquaredError,
//                 RSquared = metrics.RSquared
//             };
//         }
//
//         private async Task<ModelMetrics> TrainHipModel(DataOperationsCatalog.TrainTestData split)
//         {
//             _logger.LogInformation("Training hip prediction model...");
//
//             var pipeline = _mlContext.Transforms.CopyColumns("Label", nameof(MeasurementData.TargetHip))
//                 .Append(_mlContext.Transforms.Concatenate("Features",
//                     nameof(MeasurementData.Height),
//                     nameof(MeasurementData.CurrentHip),
//                     nameof(MeasurementData.CurrentWeight),
//                     nameof(MeasurementData.CurrentWeek),
//                     nameof(MeasurementData.TargetWeek)))
//                 .Append(_mlContext.Transforms.NormalizeMinMax("Features"))
//                 .Append(_mlContext.Regression.Trainers.FastTree());
//
//             var model = pipeline.Fit(split.TrainSet);
//             var predictions = model.Transform(split.TestSet);
//             var metrics = _mlContext.Regression.Evaluate(predictions);
//
//             var modelPath = Path.Combine(_modelsPath, "hip_model.zip");
//             _mlContext.Model.Save(model, split.TrainSet.Schema, modelPath);
//
//             return new ModelMetrics
//             {
//                 MeanAbsoluteError = metrics.MeanAbsoluteError,
//                 MeanSquaredError = metrics.MeanSquaredError,
//                 RootMeanSquaredError = metrics.RootMeanSquaredError,
//                 RSquared = metrics.RSquared
//             };
//         }
//
//         public async Task<bool> PrepareTrainingDataAsync()
//         {
//             try
//             {
//                 _logger.LogInformation("Preparing training data from database...");
//
//                 // Get all measurements with their diaries
//                 var measurements = await _unitOfWork.MeasurementRepository.GetAllWithDiariesAsync();
//                 
//                 if (measurements.Count < 100)
//                 {
//                     _logger.LogWarning($"Insufficient training data. Found {measurements.Count} records, need at least 100.");
//                     return false;
//                 }
//
//                 // Create training data by pairing measurements from the same diary
//                 var trainingData = new List<MeasurementData>();
//
//                 foreach (var diary in measurements.GroupBy(m => m.MeasurementDiaryId))
//                 {
//                     var diaryMeasurements = diary.OrderBy(m => m.WeekOfPregnancy).ToList();
//                     
//                     // Create pairs of current -> future measurements
//                     for (int i = 0; i < diaryMeasurements.Count - 1; i++)
//                     {
//                         for (int j = i + 1; j < diaryMeasurements.Count; j++)
//                         {
//                             var current = diaryMeasurements[i];
//                             var future = diaryMeasurements[j];
//
//                             trainingData.Add(new MeasurementData
//                             {
//                                 UserId = current.MeasurementDiary?.UserId?.GetHashCode() ?? 0,
//                                 Age = current.MeasurementDiary?.Age ?? 25,
//                                 Height = current.MeasurementDiary?.Height ?? 165,
//                                 PrePregnancyWeight = current.MeasurementDiary?.Weight ?? current.Weight,
//                                 CurrentWeight = current.Weight,
//                                 CurrentBust = current.Bust,
//                                 CurrentWaist = current.Waist,
//                                 CurrentHip = current.Hip,
//                                 CurrentWeek = current.WeekOfPregnancy,
//                                 TargetWeek = future.WeekOfPregnancy,
//                                 TargetWeight = future.Weight,
//                                 TargetBust = future.Bust,
//                                 TargetWaist = future.Waist,
//                                 TargetHip = future.Hip
//                             });
//                         }
//                     }
//                 }
//
//                 // Save to CSV
//                 var csvPath = Path.Combine(_dataPath, "training_data.csv");
//                 await SaveTrainingDataToCsv(trainingData, csvPath);
//
//                 _logger.LogInformation($"Prepared {trainingData.Count} training samples");
//                 return true;
//             }
//             catch (Exception ex)
//             {
//                 _logger.LogError(ex, "Error preparing training data");
//                 return false;
//             }
//         }
//
//         private async Task SaveTrainingDataToCsv(List<MeasurementData> data, string path)
//         {
//             using var writer = new StreamWriter(path);
//             
//             // Write header
//             await writer.WriteLineAsync("UserId,Age,Height,PrePregnancyWeight,CurrentWeight,CurrentBust,CurrentWaist,CurrentHip,CurrentWeek,TargetWeek,TargetWeight,TargetBust,TargetWaist,TargetHip");
//             
//             // Write data
//             foreach (var item in data)
//             {
//                 await writer.WriteLineAsync($"{item.UserId},{item.Age},{item.Height},{item.PrePregnancyWeight},{item.CurrentWeight},{item.CurrentBust},{item.CurrentWaist},{item.CurrentHip},{item.CurrentWeek},{item.TargetWeek},{item.TargetWeight},{item.TargetBust},{item.TargetWaist},{item.TargetHip}");
//             }
//         }
//
//         private bool ModelsExist()
//         {
//             return File.Exists(Path.Combine(_modelsPath, "weight_model.zip")) &&
//                    File.Exists(Path.Combine(_modelsPath, "bust_model.zip")) &&
//                    File.Exists(Path.Combine(_modelsPath, "waist_model.zip")) &&
//                    File.Exists(Path.Combine(_modelsPath, "hip_model.zip"));
//         }
//
//         private async Task SaveMetrics(Dictionary<string, ModelMetrics> metrics)
//         {
//             var metricsPath = Path.Combine(_modelsPath, "model_metrics.json");
//             var json = System.Text.Json.JsonSerializer.Serialize(metrics, new System.Text.Json.JsonSerializerOptions
//             {
//                 WriteIndented = true
//             });
//             await File.WriteAllTextAsync(metricsPath, json);
//         }
//
//         public async Task<ValidationResult> ValidateModelsAsync()
//         {
//             var result = new ValidationResult();
//
//             try
//             {
//                 // Load test data
//                 var testDataPath = Path.Combine(_dataPath, "validation_data.csv");
//                 if (!File.Exists(testDataPath))
//                 {
//                     result.IsValid = false;
//                     result.Issues.Add("Validation data not found");
//                     return result;
//                 }
//
//                 var dataView = _mlContext.Data.LoadFromTextFile<MeasurementData>(
//                     testDataPath,
//                     hasHeader: true,
//                     separatorChar: ',');
//
//                 // Validate each model
//                 result.Accuracies["Weight"] = await ValidateWeightModel(dataView);
//                 result.Accuracies["Bust"] = await ValidateBustModel(dataView);
//                 result.Accuracies["Waist"] = await ValidateWaistModel(dataView);
//                 result.Accuracies["Hip"] = await ValidateHipModel(dataView);
//
//                 // Check if all models meet minimum accuracy threshold
//                 result.IsValid = result.Accuracies.All(a => a.Value > 0.85f);
//
//                 if (!result.IsValid)
//                 {
//                     var lowAccuracy = result.Accuracies.Where(a => a.Value <= 0.85f);
//                     foreach (var model in lowAccuracy)
//                     {
//                         result.Issues.Add($"{model.Key} model accuracy ({model.Value:P}) is below threshold (85%)");
//                     }
//                 }
//             }
//             catch (Exception ex)
//             {
//                 result.IsValid = false;
//                 result.Issues.Add($"Validation error: {ex.Message}");
//             }
//
//             return result;
//         }
//
//         private async Task<float> ValidateWeightModel(IDataView dataView)
//         {
//             var modelPath = Path.Combine(_modelsPath, "weight_model.zip");
//             var model = _mlContext.Model.Load(modelPath, out _);
//             
//             var predictions = model.Transform(dataView);
//             var metrics = _mlContext.Regression.Evaluate(predictions, "Label", "Score");
//             
//             return (float)metrics.RSquared;
//         }
//
//         private async Task<float> ValidateBustModel(IDataView dataView)
//         {
//             var modelPath = Path.Combine(_modelsPath, "bust_model.zip");
//             var model = _mlContext.Model.Load(modelPath, out _);
//             
//             var predictions = model.Transform(dataView);
//             var metrics = _mlContext.Regression.Evaluate(predictions, "Label", "Score");
//             
//             return (float)metrics.RSquared;
//         }
//
//         private async Task<float> ValidateWaistModel(IDataView dataView)
//         {
//             var modelPath = Path.Combine(_modelsPath, "waist_model.zip");
//             var model = _mlContext.Model.Load(modelPath, out _);
//             
//             var predictions = model.Transform(dataView);
//             var metrics = _mlContext.Regression.Evaluate(predictions, "Label", "Score");
//             
//             return (float)metrics.RSquared;
//         }
//
//         private async Task<float> ValidateHipModel(IDataView dataView)
//         {
//             var modelPath = Path.Combine(_modelsPath, "hip_model.zip");
//             var model = _mlContext.Model.Load(modelPath, out _);
//             
//             var predictions = model.Transform(dataView);
//             var metrics = _mlContext.Regression.Evaluate(predictions, "Label", "Score");
//             
//             return (float)metrics.RSquared;
//         }
//
//         public async Task<bool> ExportTrainingDataAsync(string path)
//         {
//             try
//             {
//                 var sourcePath = Path.Combine(_dataPath, "training_data.csv");
//                 if (File.Exists(sourcePath))
//                 {
//                     File.Copy(sourcePath, path, overwrite: true);
//                     return true;
//                 }
//                 return false;
//             }
//             catch (Exception ex)
//             {
//                 _logger.LogError(ex, "Error exporting training data");
//                 return false;
//             }
//         }
//     }