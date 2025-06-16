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

    public MeasurementService(IMapper mapper,
        IUnitOfWork unitOfWork, IBodyGrowthCalculator calculator)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
        _calculator = calculator;
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
        var diary = await _unitOfWork.MeasurementDiaryRepository.GetByIdAsync(dto.MeasurementDiaryId)
                    ?? throw new ErrorException(StatusCodes.Status404NotFound, ApiCodes.NOT_FOUND,
                        "Measurement Diary not found");

        var weeksPregnant = CalculateWeeksPregnant(diary.PregnancyStartDate);

        if (await ValidateMeasurementExistenceAsync(dto.MeasurementDiaryId, weeksPregnant))
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
            var last = (await _unitOfWork.MeasurementRepository
                    .FindListAsync(m => m.MeasurementDiaryId == diary.Id))
                .OrderByDescending(m => m.WeekOfPregnancy)
                .FirstOrDefault();

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
        var diary = await _unitOfWork.MeasurementDiaryRepository.GetByIdAsync(dto.MeasurementDiaryId);
        if (diary == null)
            throw new ErrorException(StatusCodes.Status404NotFound, ApiCodes.NOT_FOUND, "Measurement Diary not found");

        var pregnancyStartDate = diary.PregnancyStartDate;
        var weeksPregnant = CalculateWeeksPregnant(pregnancyStartDate);

        if (await ValidateMeasurementExistenceAsync(dto.MeasurementDiaryId, weeksPregnant))
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
        if (dto.UserId != null)
        {
            var user = await _unitOfWork.UserRepository.GetByIdAsync(dto.UserId);
            if (user == null)
                throw new ErrorException(StatusCodes.Status404NotFound, ApiCodes.NOT_FOUND, "User not found");
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
        }

        _mapper.Map(dto, measurement);
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

    private async Task<bool> ValidateMeasurementExistenceAsync(string measurementDiaryId, int weekOfPregnancy)
    {
        var existing = await _unitOfWork.MeasurementRepository
            .FindAsync(m => m.MeasurementDiaryId == measurementDiaryId && m.WeekOfPregnancy == weekOfPregnancy);
        return existing != null;
    }
}