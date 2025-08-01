using MamaFit.AI.Dto;
using MamaFit.AI.Infrastructure;
using MamaFit.AI.Interfaces;
using MamaFit.AI.Models;
using Microsoft.Extensions.Logging;
using Microsoft.ML;
using Microsoft.ML.Data;

namespace MamaFit.AI.Services.Implementation;

public class MLModelService : IMLModelService
    {
        private readonly MLContext _mlContext;
        private readonly ILogger<MLModelService> _logger;
        private ITransformer? _weightModel;
        private ITransformer? _bustModel;
        private ITransformer? _waistModel;
        private ITransformer? _hipModel;
        private readonly string _modelsPath = "TrainedModels";

        public MLModelService(ILogger<MLModelService> logger)
        {
            _logger = logger;
            _mlContext = new MLContext(seed: 42);
            LoadModels();
        }

        private void LoadModels()
        {
            try
            {
                var weightModelPath = Path.Combine(_modelsPath, "weight_model.zip");
                var bustModelPath = Path.Combine(_modelsPath, "bust_model.zip");
                var waistModelPath = Path.Combine(_modelsPath, "waist_model.zip");
                var hipModelPath = Path.Combine(_modelsPath, "hip_model.zip");

                if (File.Exists(weightModelPath))
                {
                    _weightModel = _mlContext.Model.Load(weightModelPath, out _);
                    _logger.LogInformation("Weight model loaded successfully");
                }

                if (File.Exists(bustModelPath))
                {
                    _bustModel = _mlContext.Model.Load(bustModelPath, out _);
                    _logger.LogInformation("Bust model loaded successfully");
                }

                if (File.Exists(waistModelPath))
                {
                    _waistModel = _mlContext.Model.Load(waistModelPath, out _);
                    _logger.LogInformation("Waist model loaded successfully");
                }

                if (File.Exists(hipModelPath))
                {
                    _hipModel = _mlContext.Model.Load(hipModelPath, out _);
                    _logger.LogInformation("Hip model loaded successfully");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading models");
            }
        }

        public async Task<MeasurementPrediction> PredictAsync(MeasurementPredictionRequest request)
        {
            if (!await IsModelReady())
            {
                throw new InvalidOperationException("ML models are not ready. Please train the models first.");
            }

            var input = new MeasurementData
            {
                UserId = float.Parse(request.UserId.GetHashCode().ToString()),
                Age = request.Age,
                Height = request.Height,
                PrePregnancyWeight = request.PrePregnancyWeight,
                CurrentWeight = request.CurrentWeight,
                CurrentBust = request.CurrentBust,
                CurrentWaist = request.CurrentWaist,
                CurrentHip = request.CurrentHip,
                CurrentWeek = request.CurrentWeek,
                TargetWeek = request.TargetWeek
            };

            var prediction = new MeasurementPrediction();

            // Predict each measurement separately for better accuracy
            await Task.Run(() =>
            {
                // Weight prediction
                var weightEngine = _mlContext.Model.CreatePredictionEngine<MeasurementData, WeightPrediction>(_weightModel);
                var weightPred = weightEngine.Predict(input);
                prediction.Weight = weightPred.Score;
                prediction.WeightConfidence = CalculateConfidence(weightPred.Score, request.CurrentWeight, request.TargetWeek - request.CurrentWeek);

                // Bust prediction
                var bustEngine = _mlContext.Model.CreatePredictionEngine<MeasurementData, BustPrediction>(_bustModel);
                var bustPred = bustEngine.Predict(input);
                prediction.Bust = bustPred.Score;
                prediction.BustConfidence = CalculateConfidence(bustPred.Score, request.CurrentBust, request.TargetWeek - request.CurrentWeek);

                // Waist prediction
                var waistEngine = _mlContext.Model.CreatePredictionEngine<MeasurementData, WaistPrediction>(_waistModel);
                var waistPred = waistEngine.Predict(input);
                prediction.Waist = waistPred.Score;
                prediction.WaistConfidence = CalculateConfidence(waistPred.Score, request.CurrentWaist, request.TargetWeek - request.CurrentWeek);

                // Hip prediction
                var hipEngine = _mlContext.Model.CreatePredictionEngine<MeasurementData, HipPrediction>(_hipModel);
                var hipPred = hipEngine.Predict(input);
                prediction.Hip = hipPred.Score;
                prediction.HipConfidence = CalculateConfidence(hipPred.Score, request.CurrentHip, request.TargetWeek - request.CurrentWeek);
            });

            // Apply medical constraints
            prediction = ApplyMedicalConstraints(prediction, request);

            _logger.LogInformation($"Prediction completed for user {request.UserId}, week {request.TargetWeek}");

            return prediction;
        }

        private float CalculateConfidence(float predictedValue, float currentValue, int weeksDifference)
        {
            // Base confidence starts at 95%
            float confidence = 0.95f;
            
            // Reduce confidence for longer predictions
            confidence -= weeksDifference * 0.02f;
            
            // Ensure confidence doesn't go below 60%
            return Math.Max(confidence, 0.6f);
        }

        private MeasurementPrediction ApplyMedicalConstraints(MeasurementPrediction prediction, MeasurementPredictionRequest request)
        {
            // Apply IOM (Institute of Medicine) guidelines for weight gain
            var bmi = request.PrePregnancyWeight / ((request.Height / 100) * (request.Height / 100));
            var weeksDiff = request.TargetWeek - request.CurrentWeek;
            
            float maxWeightGain;
            if (bmi < 18.5f) // Underweight
                maxWeightGain = weeksDiff * 0.5f;
            else if (bmi < 25f) // Normal weight
                maxWeightGain = weeksDiff * 0.4f;
            else if (bmi < 30f) // Overweight
                maxWeightGain = weeksDiff * 0.3f;
            else // Obese
                maxWeightGain = weeksDiff * 0.2f;

            // Constrain weight prediction
            if (prediction.Weight > request.CurrentWeight + maxWeightGain)
            {
                prediction.Weight = request.CurrentWeight + maxWeightGain;
                prediction.WeightConfidence *= 0.9f;
            }

            // Ensure proportional growth
            var waistToBustRatio = prediction.Waist / prediction.Bust;
            if (waistToBustRatio > 1.1f) // Waist shouldn't be much larger than bust
            {
                prediction.Waist = prediction.Bust * 1.05f;
                prediction.WaistConfidence *= 0.9f;
            }

            return prediction;
        }

        public async Task<bool> IsModelReady()
        {
            return await Task.FromResult(_weightModel != null && _bustModel != null && _waistModel != null && _hipModel != null);
        }

        public async Task<ModelMetrics> GetModelMetrics()
        {
            // Load saved metrics from training
            var metricsPath = Path.Combine(_modelsPath, "model_metrics.json");
            if (File.Exists(metricsPath))
            {
                var json = await File.ReadAllTextAsync(metricsPath);
                return System.Text.Json.JsonSerializer.Deserialize<ModelMetrics>(json) ?? new ModelMetrics();
            }
            
            return new ModelMetrics();
        }

        public async Task<List<MeasurementPrediction>> BatchPredictAsync(List<MeasurementPredictionRequest> requests)
        {
            var predictions = new List<MeasurementPrediction>();
            
            // Process in parallel for better performance
            var tasks = requests.Select(r => PredictAsync(r));
            predictions.AddRange(await Task.WhenAll(tasks));
            
            return predictions;
        }
    }

    // Prediction classes for each measurement type
    public class WeightPrediction
    {
        [ColumnName("Score")]
        public float Score { get; set; }
    }

    public class BustPrediction
    {
        [ColumnName("Score")]
        public float Score { get; set; }
    }

    public class WaistPrediction
    {
        [ColumnName("Score")]
        public float Score { get; set; }
    }

    public class HipPrediction
    {
        [ColumnName("Score")]
        public float Score { get; set; }
    }