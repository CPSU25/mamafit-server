using AutoMapper;
using MamaFit.BusinessObjects.DTO.MeasurementDto;
using MamaFit.BusinessObjects.DTO.NotificationDto;
using MamaFit.BusinessObjects.Entity;
using MamaFit.Repositories.Implement;
using MamaFit.Repositories.Infrastructure;
using MamaFit.Services.ExternalService.AI;
using MamaFit.Services.ExternalService.AI.Interface;
using MamaFit.Services.Interface;
using MamaFit.Services.Service.Caculator;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace MamaFit.Services.Service;

public class MeasurementService : IMeasurementService
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IBodyGrowthCalculator _calculator;
    private readonly IValidationService _validation;
    private readonly INotificationService _notificationService;
    private readonly IAIMeasurementCalculationService _aiCalculationService;
    private readonly ILogger<MeasurementService> _logger;

    public MeasurementService(IMapper mapper,
        IUnitOfWork unitOfWork,
        IBodyGrowthCalculator calculator,
        IValidationService validation,
        INotificationService notificationService,
        IAIMeasurementCalculationService aiCalculationService,
        ILogger<MeasurementService> logger)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
        _calculator = calculator;
        _validation = validation;
        _notificationService = notificationService;
        _aiCalculationService = aiCalculationService;
        _logger = logger;
    }

    public async Task GenerateMissingMeasurementsAsync()
    {
        var diaries = await _unitOfWork.MeasurementDiaryRepository.GetAllAsync();

        foreach (var diary in diaries)
        {
            if (diary.PregnancyStartDate == null) continue;

            var currentWeek = CalculateWeeksPregnant(diary.PregnancyStartDate);
            var last = await _unitOfWork.MeasurementRepository
                .GetLatestMeasurementByDiaryIdAsync(diary.Id);

            int baseWeek = last.WeekOfPregnancy;

            for (int week = baseWeek + 2; week <= currentWeek; week += 2)
            {
                bool exists = await _unitOfWork.MeasurementRepository
                    .ValidateMeasurementExistenceAsync(diary.Id, week);
                if (exists) continue;

                float baseBust = last?.Bust ?? diary.Bust;
                float baseWaist = last?.Waist ?? diary.Waist;
                float baseHip = last?.Hip ?? diary.Hip;
                float baseWeight = last?.Weight ?? diary.Weight;
                float height = diary.Height;

                float bust = _calculator.CalculateBust(baseBust, baseWeek, week);
                float waist = _calculator.CalculateWaist(baseWaist, baseWeek, week);
                float hip = _calculator.CalculateHip(baseHip, baseWeek, week);
                float weight = _calculator.CalculateWeight(baseWeight, baseWeek, week);

                var entity = new Measurement
                {
                    MeasurementDiaryId = diary.Id,
                    WeekOfPregnancy = week,
                    Bust = bust,
                    Waist = waist,
                    Hip = hip,
                    Weight = weight,
                    Stomach = waist + 5f,
                    PantsWaist = waist - 5f,
                    Coat = bust + 5f,
                    ChestAround = bust + 3f,
                    Thigh = (hip + 5f) / 2f,
                    Neck = height / 5f,
                    ShoulderWidth = height / 4.3f,
                    SleeveLength = height / 6.4f,
                    DressLength = height * 0.66f,
                    LegLength = height * 0.48f,
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = "SystemAuto"
                };

                await _unitOfWork.MeasurementRepository.InsertAsync(entity);
            }

            await _unitOfWork.SaveChangesAsync();
        }
    }

    public async Task<PaginatedList<MeasurementResponseDto>> GetAllMeasurementsAsync(int index, int pageSize,
        DateTime? startDate, DateTime? endDate)
    {
        var measurements = await _unitOfWork.MeasurementRepository.GetAllAsync(index, pageSize, startDate, endDate);
        if (measurements == null)
            throw new ErrorException(StatusCodes.Status404NotFound, ApiCodes.NOT_FOUND, "No measurements found");

        var responseItems = measurements.Items
            .Select(measurement => _mapper.Map<MeasurementResponseDto>(measurement))
            .ToList();

        return new PaginatedList<MeasurementResponseDto>(
            responseItems,
            measurements.TotalCount,
            measurements.PageNumber,
            pageSize
        );
    }

    public async Task CheckAndSendRemindersAsync()
    {
        var diaries = await _unitOfWork.MeasurementDiaryRepository.GetAllAsync();
        foreach (var diary in diaries)
        {
            var currentWeek = CalculateWeeksPregnant(diary.PregnancyStartDate);
            bool hasMeasurement = await _unitOfWork.MeasurementRepository
                .ValidateMeasurementExistenceAsync(diary.Id, currentWeek);

            if (!hasMeasurement)
            {
                var model = new NotificationRequestDto
                {
                    ReceiverId = diary.UserId,
                    NotificationTitle = "Measurement Reminder",
                    NotificationContent =
                        $"You have not recorded your measurements for week {currentWeek} of pregnancy. Please update your measurements.",
                };
                await _notificationService.SendAndSaveNotificationAsync(model);
            }
        }
    }

    public async Task<MeasurementResponseDto> GetMeasurementByIdAsync(string id)
    {
        var measurement = await _unitOfWork.MeasurementRepository.GetByIdAsync(id);
        _validation.CheckNotFound(measurement, $"Measurement with id: {id} is not found");

        var response = _mapper.Map<MeasurementResponseDto>(measurement);

        if (measurement.Orders.Any())
        {
            response.IsLocked = true;
        }
        return response;
    }

    public async Task<MeasurementResponseDto> GetMeasurementByDiaryIdAsync(string diaryId)
    {
        var measurement = await _unitOfWork.MeasurementRepository.GetLatestMeasurementByDiaryIdAsync(diaryId);

        var response = _mapper.Map<MeasurementResponseDto>(measurement);

        if (measurement.Orders.Any())
        {
            response.IsLocked = true;
        }

        return response;
    }

    public async Task<MeasurementDto> GenerateMeasurementPreviewAsync(MeasurementCreateDto dto)
    {
        await _validation.ValidateAndThrowAsync(dto);
        var diary = await _unitOfWork.MeasurementDiaryRepository.GetByIdNotDeletedAsync(dto.MeasurementId);
        _validation.CheckNotFound(diary, "Measurement diary not found");
        var weeksPregnant = CalculateWeeksPregnant(diary.PregnancyStartDate);

        if (await _unitOfWork.MeasurementRepository.ValidateMeasurementExistenceAsync(dto.MeasurementId,
                weeksPregnant))
            throw new ErrorException(StatusCodes.Status400BadRequest, ApiCodes.NOT_FOUND,
                "Measurement for this week already exists");

        var diaryDto = _mapper.Map<MeasurementDiaryDto>(diary);
            
        // Add ID if not mapped
        // if (string.IsNullOrEmpty(diaryDto.Id))
        // {
        //     diaryDto.Id = diary.Id;
        // }
            
        // Get last measurement if exists
        var lastMeasurement = await _unitOfWork.MeasurementRepository
            .GetLatestMeasurementByDiaryIdAsync(diary.Id);

        try
        {
            // Try AI calculation first
            if (await _aiCalculationService.IsAvailable())
            {
                _logger.LogInformation($"Using AI for measurement calculation - Week {weeksPregnant}");
                    
                var aiResult = await _aiCalculationService.CalculateMeasurementsAsync(
                    diaryDto, 
                    dto, 
                    lastMeasurement, 
                    weeksPregnant);
                    
                _logger.LogInformation($"AI calculation successful: Weight={aiResult.Weight:F1}kg, " +
                                       $"Bust={aiResult.Bust:F1}cm, Waist={aiResult.Waist:F1}cm, Hip={aiResult.Hip:F1}cm");
                    
                return aiResult;
            }
            else
            {
                _logger.LogWarning("AI service not available, using calculator");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "AI calculation failed, using fallback calculator");
        }
        
        // bool hasManualInput = dto.Weight > 0 && dto.Bust > 0 && dto.Waist > 0 && dto.Hip > 0;
        //
        // float weight, bust, waist, hip;
        //
        // if (hasManualInput)
        // {
        //     weight = dto.Weight;
        //     bust = dto.Bust;
        //     waist = dto.Waist;
        //     hip = dto.Hip;
        // }
        // else
        // {
        //     var last = await _unitOfWork.MeasurementRepository.GetLatestMeasurementByDiaryIdAsync(diary.Id);
        //
        //     float baseBust = last?.Bust ?? diary.Bust;
        //     float baseWaist = last?.Waist ?? diary.Waist;
        //     float baseHip = last?.Hip ?? diary.Hip;
        //     float baseWeight = last?.Weight ?? diary.Weight;
        //
        //     int baseWeek = last!.WeekOfPregnancy;
        //
        //     bust = _calculator.CalculateBust(baseBust, baseWeek, weeksPregnant);
        //     waist = _calculator.CalculateWaist(baseWaist, baseWeek, weeksPregnant);
        //     hip = _calculator.CalculateHip(baseHip, baseWeek, weeksPregnant);
        //     weight = _calculator.CalculateWeight(baseWeight, baseWeek, weeksPregnant);
        // }
        //
        // float height = diary.Height;
        //
        // return new MeasurementDto
        // {
        //     WeekOfPregnancy = weeksPregnant,
        //     Weight = weight,
        //     Waist = waist,
        //     Bust = bust,
        //     Hip = hip,
        //     Stomach = waist + 5f,
        //     PantsWaist = waist - 5f,
        //     Coat = bust + 5f,
        //     ChestAround = bust + 3f,
        //     Thigh = (hip + 5f) / 2f,
        //
        //     Neck = height / 5f,
        //     ShoulderWidth = height / 4.3f,
        //     SleeveLength = height / 6.4f,
        //     DressLength = height * 0.66f,
        //     LegLength = height * 0.48f
        // };
        return await GenerateWithOriginalCalculator(dto, diary, lastMeasurement, weeksPregnant);
    }

    public async Task<MeasurementDto> CreateMeasurementAsync(CreateMeasurementDto dto)
    {
        await _validation.ValidateAndThrowAsync(dto);
        var diary = await _unitOfWork.MeasurementDiaryRepository.GetByIdNotDeletedAsync(dto.MeasurementId);
        _validation.CheckNotFound(diary, "Measurement diary not found");
        var pregnancyStartDate = diary.PregnancyStartDate;
        var weeksPregnant = CalculateWeeksPregnant(pregnancyStartDate);

        if (await _unitOfWork.MeasurementRepository.ValidateMeasurementExistenceAsync(dto.MeasurementId,
                weeksPregnant))
            throw new ErrorException(StatusCodes.Status400BadRequest, ApiCodes.BAD_REQUEST,
                "Measurement for this week already exists");

        var measurementEntity = _mapper.Map<Measurement>(dto);
        measurementEntity.MeasurementDiaryId = diary.Id;
        measurementEntity.WeekOfPregnancy = weeksPregnant;
        diary.UpdatedAt = DateTime.UtcNow;
        await _unitOfWork.MeasurementRepository.InsertAsync(measurementEntity);
        await _unitOfWork.SaveChangesAsync();

        return _mapper.Map<MeasurementDto>(measurementEntity);
    }

    public async Task<MeasurementDto> GenerateMeasurementDiaryPreviewAsync(MeasurementDiaryDto dto)
    {
        await _validation.ValidateAndThrowAsync(dto);
        if (dto.UserId != null)
        {
            var user = await _unitOfWork.UserRepository.GetByIdNotDeletedAsync(dto.UserId);
            _validation.CheckNotFound(user, "User not found");
        }

        var pregnancyStartDate = CalculatePregnancyStartDate(dto);
        var currentWeek = CalculateWeeksPregnant(pregnancyStartDate);

        try
        {
            // Try AI calculation
            if (await _aiCalculationService.IsAvailable())
            {
                _logger.LogInformation("Using AI for diary preview calculation");
                    
                // Create a pseudo CreateDto from diary data
                var createDto = new MeasurementCreateDto
                {
                    //MeasurementId = dto.Id ?? "",
                    Weight = dto.Weight,
                    Bust = dto.Bust,
                    Waist = dto.Waist,
                    Hip = dto.Hip
                };
                    
                return await _aiCalculationService.CalculateMeasurementsAsync(
                    dto, 
                    createDto, 
                    null, // No last measurement for new diary
                    currentWeek);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "AI calculation failed for diary preview");
        }
        
        float baseBust = dto.Bust;
        float baseWaist = dto.Waist;
        float baseHip = dto.Hip;
        float height = dto.Height;

        float bust = _calculator.CalculateBust(baseBust, 0, currentWeek);
        float waist = _calculator.CalculateWaist(baseWaist, 0, currentWeek);
        float hip = _calculator.CalculateHip(baseHip, 0, currentWeek);

        return new MeasurementDto
        {
            WeekOfPregnancy = currentWeek,
            Weight = dto.Weight,
            Bust = dto.Bust,
            Waist = dto.Waist,
            Hip = dto.Hip,
            Stomach = waist + 5f,
            PantsWaist = waist - 5f,
            Coat = bust + 5f,
            ChestAround = bust + 3f,
            Thigh = (hip + 5f) / 2f,
            Neck = height / 5f,
            ShoulderWidth = height / 4.3f,
            SleeveLength = height / 6.4f,
            DressLength = height * 0.66f,
            LegLength = height * 0.48f
        };
    }

    public async Task<string> CreateDiaryWithMeasurementAsync(MeasurementDiaryCreateRequest request)
    {
        var user = await _unitOfWork.UserRepository.GetByIdAsync(request.Diary.UserId);
        if (user == null)
            throw new ErrorException(StatusCodes.Status404NotFound, ApiCodes.NOT_FOUND, "User not found");
        if (request.Diary != null)
        {
            var pregnancyStartDate = CalculatePregnancyStartDate(request.Diary);
            request.Diary.PregnancyStartDate = pregnancyStartDate;
        }

        await _unitOfWork.MeasurementDiaryRepository.SetActiveFalseForAllAsync(user.Id);
        var diaryEntity = _mapper.Map<MeasurementDiary>(request.Diary);
        diaryEntity.IsActive = true;
        diaryEntity.UserId = request.Diary?.UserId;
        await _unitOfWork.MeasurementDiaryRepository.InsertAsync(diaryEntity);
        await _unitOfWork.SaveChangesAsync();

        var measurementEntity = _mapper.Map<Measurement>(request.Measurement);
        measurementEntity.MeasurementDiaryId = diaryEntity.Id;
        measurementEntity.WeekOfPregnancy = CalculateWeeksPregnant(diaryEntity.PregnancyStartDate);
        await _unitOfWork.MeasurementRepository.InsertAsync(measurementEntity);
        await _unitOfWork.SaveChangesAsync();

        return diaryEntity.Id;
    }

    public async Task<MeasurementDto> UpdateMeasurementAsync(string id, UpdateMeasurementDto dto)
    {
        var measurement = await _unitOfWork.MeasurementRepository.GetByIdAsync(id);
        if (measurement == null)
            throw new ErrorException(StatusCodes.Status404NotFound, ApiCodes.NOT_FOUND, "Measurement not found");

        if (measurement.MeasurementDiaryId != null)
        {
            var diary = await _unitOfWork.MeasurementDiaryRepository.GetByIdAsync(measurement.MeasurementDiaryId);
            if (diary == null)
                throw new ErrorException(StatusCodes.Status404NotFound, ApiCodes.NOT_FOUND,
                    "Measurement Diary not found");

            _mapper.Map(dto, measurement);
            diary.UpdatedAt = DateTime.UtcNow;
        }

        await _unitOfWork.MeasurementRepository.UpdateAsync(measurement);
        await _unitOfWork.SaveChangesAsync();

        return _mapper.Map<MeasurementDto>(measurement);
    }

    public async Task<bool> DeleteMeasurementAsync(string id)
    {
        var measurement = await _unitOfWork.MeasurementRepository.GetByIdAsync(id);
        if (measurement == null)
            throw new ErrorException(StatusCodes.Status404NotFound, ApiCodes.NOT_FOUND, "Measurement not found");

        await _unitOfWork.MeasurementRepository.SoftDeleteAsync(id);
        await _unitOfWork.SaveChangesAsync();
        return true;
    }

    private DateTime? CalculatePregnancyStartDate(MeasurementDiaryDto dto)
    {
        DateTime? start = null;

        if (dto.UltrasoundDate.HasValue && dto.WeeksFromUltrasound > 0)
            start = dto.UltrasoundDate.Value.AddDays(-7 * dto.WeeksFromUltrasound);
        else if (dto.FirstDateOfLastPeriod.HasValue)
            start = dto.FirstDateOfLastPeriod.Value.AddDays((dto.AverageMenstrualCycle ?? 28) - 14);

        return start;
    }

    private int CalculateWeeksPregnant(DateTime? pregnancyStartDate)
    {
        if (!pregnancyStartDate.HasValue)
            return 0;

        return (int)((DateTime.UtcNow - pregnancyStartDate.Value).TotalDays / 7);
    }
    
    private async Task<MeasurementDto> GenerateWithOriginalCalculator(
            MeasurementCreateDto dto, 
            MeasurementDiary diary, 
            Measurement? lastMeasurement, 
            int weeksPregnant)
        {
            bool hasManualInput = dto.Weight > 0 && dto.Bust > 0 && dto.Waist > 0 && dto.Hip > 0;

            float weight, bust, waist, hip;

            if (hasManualInput)
            {
                weight = dto.Weight;
                bust = dto.Bust;
                waist = dto.Waist;
                hip = dto.Hip;
            }
            else
            {
                float baseBust = lastMeasurement?.Bust ?? diary.Bust;
                float baseWaist = lastMeasurement?.Waist ?? diary.Waist;
                float baseHip = lastMeasurement?.Hip ?? diary.Hip;
                float baseWeight = lastMeasurement?.Weight ?? diary.Weight;
                int baseWeek = lastMeasurement?.WeekOfPregnancy ?? 0;

                bust = _calculator.CalculateBust(baseBust, baseWeek, weeksPregnant);
                waist = _calculator.CalculateWaist(baseWaist, baseWeek, weeksPregnant);
                hip = _calculator.CalculateHip(baseHip, baseWeek, weeksPregnant);
                weight = _calculator.CalculateWeight(baseWeight, baseWeek, weeksPregnant);
            }

            float height = diary.Height;

            return new MeasurementDto
            {
                WeekOfPregnancy = weeksPregnant,
                Weight = weight,
                Waist = waist,
                Bust = bust,
                Hip = hip,
                Stomach = waist + 5f,
                PantsWaist = waist - 5f,
                Coat = bust + 5f,
                ChestAround = bust + 3f,
                Thigh = (hip + 5f) / 2f,
                Neck = height / 5f,
                ShoulderWidth = height / 4.3f,
                SleeveLength = height / 6.4f,
                DressLength = height * 0.66f,
                LegLength = height * 0.48f
            };
        }
}