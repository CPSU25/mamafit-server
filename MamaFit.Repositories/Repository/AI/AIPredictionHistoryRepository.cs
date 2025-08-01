using MamaFit.BusinessObjects.DBContext;
using MamaFit.BusinessObjects.Entity;
using MamaFit.BusinessObjects.Entity.AI;
using MamaFit.Repositories.Implement;
using MamaFit.Repositories.Interface.AI;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace MamaFit.Repositories.Repository.AI;

public class AIPredictionHistoryRepository : GenericRepository<AIPredictionHistory>, IAIPredictionHistoryRepository
{
    public AIPredictionHistoryRepository(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor) 
        : base(context, httpContextAccessor)
    {
    }

    public async Task<AIPredictionHistory?> GetLatestPredictionAsync(string diaryId, int targetWeek)
        {
            return await _dbSet
                .Where(p => p.MeasurementDiaryId == diaryId && 
                           p.TargetWeek == targetWeek && 
                           !p.IsDeleted)
                .OrderByDescending(p => p.PredictedAt)
                .FirstOrDefaultAsync();
        }

        public async Task<List<AIPredictionHistory>> GetUserPredictionsAsync(string userId, int limit = 10)
        {
            return await _dbSet
                .Where(p => p.UserId == userId && !p.IsDeleted)
                .OrderByDescending(p => p.PredictedAt)
                .Take(limit)
                .ToListAsync();
        }

        public async Task<double> CalculateAverageAccuracyAsync(string modelVersion)
        {
            var predictions = await _dbSet
                .Where(p => p.ModelVersion == modelVersion && 
                           p.AccuracyScore.HasValue && 
                           !p.IsDeleted)
                .ToListAsync();

            return predictions.Any() 
                ? predictions.Average(p => p.AccuracyScore.Value) 
                : 0;
        }

        public async Task UpdateAccuracyScoreAsync(string predictionId, string actualMeasurementId)
        {
            var prediction = await GetByIdAsync(predictionId);
            if (prediction != null)
            {
                var actualMeasurement = await _context.Measurements
                    .FirstOrDefaultAsync(m => m.Id == actualMeasurementId);
                
                if (actualMeasurement != null)
                {
                    prediction.ActualMeasurementId = actualMeasurementId;
                    prediction.AccuracyScore = CalculateAccuracy(prediction, actualMeasurement);
                    await UpdateAsync(prediction);
                }
            }
        }

        private float CalculateAccuracy(AIPredictionHistory prediction, Measurement actual)
        {
            var accuracies = new List<float>
            {
                CalculateFieldAccuracy(prediction.PredictedWeight, actual.Weight),
                CalculateFieldAccuracy(prediction.PredictedBust, actual.Bust),
                CalculateFieldAccuracy(prediction.PredictedWaist, actual.Waist),
                CalculateFieldAccuracy(prediction.PredictedHip, actual.Hip),
                CalculateFieldAccuracy(prediction.PredictedNeck, actual.Neck),
                CalculateFieldAccuracy(prediction.PredictedStomach, actual.Stomach),
                CalculateFieldAccuracy(prediction.PredictedThigh, actual.Thigh)
            };

            return accuracies.Average();
        }

        private float CalculateFieldAccuracy(float predicted, float actual)
        {
            if (actual == 0) return 0;
            var error = Math.Abs(predicted - actual) / actual;
            return Math.Max(0, 1 - error);
        }
}