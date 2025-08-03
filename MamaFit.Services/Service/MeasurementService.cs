using AutoMapper;
using MamaFit.BusinessObjects.DTO.MeasurementDto;
using MamaFit.BusinessObjects.DTO.NotificationDto;
using MamaFit.BusinessObjects.Entity;
using MamaFit.Repositories.Implement;
using MamaFit.Repositories.Infrastructure;
using MamaFit.Services.ExternalService.AI;
using MamaFit.Services.Interface;
using MamaFit.Services.Service.Caculator;
using Microsoft.AspNetCore.Http;

namespace MamaFit.Services.Service;

public class MeasurementService : IMeasurementService
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IBodyGrowthCalculator _calculator;
    private readonly IValidationService _validation;
    private readonly INotificationService _notificationService;
    private readonly IAIMeasurementCalculationService _aiCalculationService;

    public MeasurementService(IMapper mapper,
        IUnitOfWork unitOfWork,
        IBodyGrowthCalculator calculator,
        IValidationService validation,
        INotificationService notificationService,
        IAIMeasurementCalculationService aiCalculationService)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
        _calculator = calculator;
        _validation = validation;
        _notificationService = notificationService;
        _aiCalculationService = aiCalculationService;
    }

    public async Task GenerateMissingMeasurementsAsync()
    {
        var diaries = await _unitOfWork.MeasurementDiaryRepository.GetAllAsync();

        foreach (var diary in diaries)
        {
            if (diary.PregnancyStartDate == null) continue;
            await GenerateMissingMeasurementsForDiary(diary);
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
            await CheckAndSendReminderForDiary(diary);
        }
    }

    public async Task<MeasurementResponseDto> GetMeasurementByIdAsync(string id)
    {
        var measurement = await _unitOfWork.MeasurementRepository.GetByIdAsync(id);
        _validation.CheckNotFound(measurement, $"Measurement with id: {id} is not found");

        var response = _mapper.Map<MeasurementResponseDto>(measurement);

        response.IsLocked = measurement.Orders.Any();
        return response;
    }

    public async Task<MeasurementResponseDto> GetMeasurementByDiaryIdAsync(string diaryId)
    {
        var measurement = await _unitOfWork.MeasurementRepository.GetLatestMeasurementByDiaryIdAsync(diaryId);

        var response = _mapper.Map<MeasurementResponseDto>(measurement);
        response.IsLocked = measurement.Orders.Any();

        return response;
    }

    public async Task<MeasurementDto> GenerateMeasurementPreviewAsync(MeasurementCreateDto dto)
    {
        await _validation.ValidateAndThrowAsync(dto);
        var diary = await _unitOfWork.MeasurementDiaryRepository.GetByIdNotDeletedAsync(dto.MeasurementId);
        _validation.CheckNotFound(diary, "Measurement diary not found");
        var weeksPregnant = CalculateWeeksPregnant(diary.PregnancyStartDate);
        
        await ValidateNoExistingMeasurement(dto.MeasurementId, weeksPregnant);
        
        var diaryDto = _mapper.Map<MeasurementDiaryDto>(diary);
        
        // Get last measurement if exists
        var lastMeasurement = await _unitOfWork.MeasurementRepository
            .GetLatestMeasurementByDiaryIdAsync(diary.Id);

        return await CalculateMeasurementsWithAI(diaryDto, dto, lastMeasurement, weeksPregnant);
    }

    public async Task<MeasurementDto> CreateMeasurementAsync(CreateMeasurementDto dto)
    {
        await _validation.ValidateAndThrowAsync(dto);
        var diary = await _unitOfWork.MeasurementDiaryRepository.GetByIdNotDeletedAsync(dto.MeasurementId);
        _validation.CheckNotFound(diary, "Measurement diary not found");
        var pregnancyStartDate = diary.PregnancyStartDate;
        var weeksPregnant = CalculateWeeksPregnant(pregnancyStartDate);

        await ValidateNoExistingMeasurement(dto.MeasurementId, weeksPregnant);

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

        var createDto = new MeasurementCreateDto
        {
            Weight = dto.Weight,
            Bust = dto.Bust,
            Waist = dto.Waist,
            Hip = dto.Hip
        };

        return await CalculateMeasurementsWithAI(dto, createDto, null, currentWeek);
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

    private async Task GenerateMissingMeasurementsForDiary(MeasurementDiary diary)
    {
        var currentWeek = CalculateWeeksPregnant(diary.PregnancyStartDate);
        var lastMeasurement = await _unitOfWork.MeasurementRepository
            .GetLatestMeasurementByDiaryIdAsync(diary.Id);

        int baseWeek = lastMeasurement?.WeekOfPregnancy ?? 0;

        for (int week = baseWeek + 2; week <= currentWeek; week += 2)
        {
            bool exists = await _unitOfWork.MeasurementRepository
                .ValidateMeasurementExistenceAsync(diary.Id, week);
            if (exists) continue;

            var measurement = CreateMissingMeasurement(diary, lastMeasurement, week);
            await _unitOfWork.MeasurementRepository.InsertAsync(measurement);
        }
    }

    private Measurement CreateMissingMeasurement(MeasurementDiary diary, Measurement? lastMeasurement, int week)
    {
        float baseBust = lastMeasurement?.Bust ?? diary.Bust;
        float baseWaist = lastMeasurement?.Waist ?? diary.Waist;
        float baseHip = lastMeasurement?.Hip ?? diary.Hip;
        float baseWeight = lastMeasurement?.Weight ?? diary.Weight;
        int baseWeek = lastMeasurement?.WeekOfPregnancy ?? 0;

        float bust = _calculator.CalculateBust(baseBust, baseWeek, week);
        float waist = _calculator.CalculateWaist(baseWaist, baseWeek, week);
        float hip = _calculator.CalculateHip(baseHip, baseWeek, week);
        float weight = _calculator.CalculateWeight(baseWeight, baseWeek, week);

        return new Measurement
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
            Neck = diary.Height / 5f,
            ShoulderWidth = diary.Height / 4.3f,
            SleeveLength = diary.Height / 6.4f,
            DressLength = diary.Height * 0.66f,
            LegLength = diary.Height * 0.48f,
            CreatedAt = DateTime.UtcNow,
            CreatedBy = "SystemAuto"
        };
    }

    private async Task CheckAndSendReminderForDiary(MeasurementDiary diary)
    {
        var currentWeek = CalculateWeeksPregnant(diary.PregnancyStartDate);
        bool hasMeasurement = await _unitOfWork.MeasurementRepository
            .ValidateMeasurementExistenceAsync(diary.Id, currentWeek);

        if (!hasMeasurement)
        {
            var notification = new NotificationRequestDto
            {
                ReceiverId = diary.UserId,
                NotificationTitle = "Measurement Reminder",
                NotificationContent =
                    $"You have not recorded your measurements for week {currentWeek} of pregnancy. Please update your measurements.",
            };
            await _notificationService.SendAndSaveNotificationAsync(notification);
        }
    }

    private async Task<MeasurementDiary> GetValidatedDiary(string diaryId)
    {
        var diary = await _unitOfWork.MeasurementDiaryRepository.GetByIdNotDeletedAsync(diaryId);
        _validation.CheckNotFound(diary, "Measurement diary not found");
        return diary;
    }

    private async Task ValidateNoExistingMeasurement(string diaryId, int week)
    {
        if (await _unitOfWork.MeasurementRepository.ValidateMeasurementExistenceAsync(diaryId, week))
            throw new ErrorException(StatusCodes.Status400BadRequest, ApiCodes.BAD_REQUEST,
                "Measurement for this week already exists");
    }

    private async Task<MeasurementDto> CalculateMeasurementsWithAI(
        MeasurementDiaryDto diaryDto,
        MeasurementCreateDto? dto,
        Measurement? lastMeasurement,
        int weeksPregnant)
    {
        try
        {
            if (await _aiCalculationService.IsAvailable())
            {
                return await _aiCalculationService.CalculateMeasurementsAsync(
                    diaryDto, dto, lastMeasurement, weeksPregnant);
            }
        }
        catch (Exception)
        {
            throw new ErrorException(StatusCodes.Status500InternalServerError, ApiCodes.EXTERNAL_SERVICE_ERROR,
                "AI measurement calculation failed. Please try again later.");
        }

        return GenerateWithOriginalCalculator(dto, diaryDto, lastMeasurement, weeksPregnant);
    }

    private MeasurementDto GenerateWithOriginalCalculator(
        MeasurementCreateDto? dto,
        MeasurementDiaryDto diary,
        Measurement? lastMeasurement,
        int weeksPregnant)
    {
        bool hasManualInput = dto != null && dto.Weight > 0 && dto.Bust > 0 && dto.Waist > 0 && dto.Hip > 0;

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

        return CreateMeasurementDto(weight, bust, waist, hip, diary.Height, weeksPregnant);
    }

    private MeasurementDto CreateMeasurementDto(float weight, float bust, float waist, float hip, float height,
        int week)
    {
        return new MeasurementDto
        {
            WeekOfPregnancy = week,
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
}