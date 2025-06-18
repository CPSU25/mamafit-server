using AutoMapper;
using MamaFit.BusinessObjects.DTO.MeasurementDto;
using MamaFit.BusinessObjects.Entity;
using MamaFit.Repositories.Infrastructure;
using MamaFit.Repositories.Interface;
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

    public MeasurementService(IMapper mapper,
        IUnitOfWork unitOfWork, IBodyGrowthCalculator calculator, IValidationService validation)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
        _calculator = calculator;
        _validation = validation;
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

                // Build và lưu vào DB
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

    public async Task<MeasurementResponseDto> GetMeasurementByIdAsync(string id)
    {
        var measurement = await _unitOfWork.MeasurementRepository.GetByIdAsync(id);
        if (measurement == null)
            throw new ErrorException(StatusCodes.Status404NotFound, ApiCodes.NOT_FOUND, "Measurement not found");
        return _mapper.Map<MeasurementResponseDto>(measurement);
    }

    public async Task<MeasurementDto> GenerateMeasurementPreviewAsync(MeasurementCreateDto dto)
    {
        await _validation.ValidateAndThrowAsync(dto);
        var diary = await _unitOfWork.MeasurementDiaryRepository.GetByIdNotDeletedAsync(dto.MeasurementDiaryId); 
        _validation.CheckNotFound(diary, "Measurement diary not found");
        var weeksPregnant = CalculateWeeksPregnant(diary.PregnancyStartDate);

        if (await _unitOfWork.MeasurementRepository.ValidateMeasurementExistenceAsync(dto.MeasurementDiaryId,
                weeksPregnant))
            throw new ErrorException(StatusCodes.Status400BadRequest, ApiCodes.NOT_FOUND,
                "Measurement for this week already exists");

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
            var last = await _unitOfWork.MeasurementRepository.GetLatestMeasurementByDiaryIdAsync(diary.Id);

            float baseBust = last?.Bust ?? diary.Bust;
            float baseWaist = last?.Waist ?? diary.Waist;
            float baseHip = last?.Hip ?? diary.Hip;
            float baseWeight = last?.Weight ?? diary.Weight;

            int baseWeek = last!.WeekOfPregnancy;

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

    public async Task<MeasurementDto> CreateMeasurementAsync(CreateMeasurementDto dto)
    {
        await _validation.ValidateAndThrowAsync(dto);
        var diary = await _unitOfWork.MeasurementDiaryRepository.GetByIdNotDeletedAsync(dto.MeasurementDiaryId);
        _validation.CheckNotFound(diary, "Measurement diary not found");
        var pregnancyStartDate = diary.PregnancyStartDate;
        var weeksPregnant = CalculateWeeksPregnant(pregnancyStartDate);

        if (await _unitOfWork.MeasurementRepository.ValidateMeasurementExistenceAsync(dto.MeasurementDiaryId,
                weeksPregnant))
            throw new ErrorException(StatusCodes.Status400BadRequest, ApiCodes.BAD_REQUEST,
                "Measurement for this week already exists");

        var measurementEntity = _mapper.Map<Measurement>(dto);
        measurementEntity.MeasurementDiaryId = diary.Id;
        measurementEntity.WeekOfPregnancy = weeksPregnant;

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
            _validation.CheckNotFound(user, "User not found");;
        }

        var pregnancyStartDate = CalculatePregnancyStartDate(dto);
        var currentWeek = CalculateWeeksPregnant(pregnancyStartDate);

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

        var diaryEntity = _mapper.Map<MeasurementDiary>(request.Diary);
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
}