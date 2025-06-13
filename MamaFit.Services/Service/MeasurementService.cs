using AutoMapper;
using MamaFit.BusinessObjects.DTO.MeasurementDto;
using MamaFit.BusinessObjects.Entity;
using MamaFit.Repositories.Infrastructure;
using MamaFit.Repositories.Interface;
using MamaFit.Services.Interface;
using Microsoft.AspNetCore.Http;

namespace MamaFit.Services.Service;

public class MeasurementService : IMeasurementService
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public MeasurementService(IMapper mapper,
        IUnitOfWork unitOfWork)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }

    public async Task<PaginatedList<MeasurementResponseDto>> GetAllMeasurementsAsync(int index, int pageSize)
    {
        var measurements = await _unitOfWork.MeasurementRepository.GetAllAsync(index, pageSize);
        if (measurements == null)
            throw new ErrorException(StatusCodes.Status404NotFound, ErrorCode.NotFound, "No measurements found");

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
            throw new ErrorException(StatusCodes.Status404NotFound, ErrorCode.NotFound, "Measurement not found");
        return _mapper.Map<MeasurementResponseDto>(measurement);
    }
    
    public async Task<MeasurementDto> GenerateMeasurementPreviewAsync(MeasurementCreateDto dto)
    {
        var diary = await _unitOfWork.MeasurementDiaryRepository.GetByIdAsync(dto.MeasurementDiaryId);
        if (diary == null)
            throw new ErrorException(StatusCodes.Status404NotFound, ErrorCode.NotFound, "Measurement Diary not found");
        
        var pregnancyStartDate = diary.PregnancyStartDate;
        var weeksPregnant = CalculateWeeksPregnant(pregnancyStartDate);
        var existingMeasurement = ValidateMeasurementExistenceAsync(dto.MeasurementDiaryId, weeksPregnant);
        if (existingMeasurement != null)
            throw new ErrorException(StatusCodes.Status400BadRequest, ErrorCode.BadRequest, "Measurement for this week already exists");
        
        return new MeasurementDto
        {
            WeekOfPregnancy = weeksPregnant,
            Weight = dto.Weight,
            Bust = dto.Bust,
            Waist = dto.Waist,
            Hip = dto.Hip,
            Stomach = dto.Waist + 5,
            Coat = dto.Bust + 5,
            ChestAround = dto.Bust + 3,
            Neck = 34,
            ShoulderWidth = 37,
            SleeveLength = 25,
            PantsWaist = dto.Waist - 5,
            DressLength = 105,
            Thigh = (dto.Hip + 5) / 2,
            LegLength = 80,
        };
    }
    
    public async Task<MeasurementDto> CreateMeasurementAsync(CreateMeasurementDto dto)
    {
        var diary = await _unitOfWork.MeasurementDiaryRepository.GetByIdAsync(dto.MeasurementDiaryId);
        if (diary == null)
            throw new ErrorException(StatusCodes.Status404NotFound, ErrorCode.NotFound, "Measurement Diary not found");

        var pregnancyStartDate = diary.PregnancyStartDate;
        var weeksPregnant = CalculateWeeksPregnant(pregnancyStartDate);
        
        var existingMeasurement = ValidateMeasurementExistenceAsync(dto.MeasurementDiaryId, weeksPregnant);
        if (existingMeasurement != null)
            throw new ErrorException(StatusCodes.Status400BadRequest, ErrorCode.BadRequest, "Measurement for this week already exists");

        var measurementEntity = _mapper.Map<Measurement>(dto);
        measurementEntity.MeasurementDiaryId = diary.Id;
        measurementEntity.WeekOfPregnancy = weeksPregnant;

        await _unitOfWork.MeasurementRepository.InsertAsync(measurementEntity);
        await _unitOfWork.SaveChangesAsync();

        return _mapper.Map<MeasurementDto>(measurementEntity);
    }
    
    public async Task<MeasurementDto> GenerateMeasurementDiaryPreviewAsync(MeasurementDiaryDto dto)
    {
        var user = await _unitOfWork.UserRepository.GetByIdAsync(dto.UserId);
        if (user == null)
            throw new ErrorException(StatusCodes.Status404NotFound, ErrorCode.NotFound, "User not found");
        var pregnancyStartDate = CalculatePregnancyStartDate(dto);

        var weeks = CalculateWeeksPregnant(pregnancyStartDate);
        
        float waist = dto.Waist ?? 70;
        float bust = dto.Bust ?? 85;
        float hip = dto.Hip ?? 90;

        return new MeasurementDto
        {
            WeekOfPregnancy = weeks,
            Weight = dto.Weight ?? 60,
            Waist = waist + (weeks <= 12 ? 0 : weeks <= 26 ? (weeks - 12) * 1.5f : (14 * 1.5f + (weeks - 26) * 2f)),
            Bust = bust + (weeks <= 12 ? 0 : (weeks - 12) * 1.2f),
            Hip = hip + (weeks <= 12 ? 0 : (weeks - 12) * 1.5f),
            Stomach = waist + 5,
            Coat = bust + 5,
            ChestAround = bust + 3,
            Neck = 34,  
            ShoulderWidth = 37,
            SleeveLength = 25,
            PantsWaist = waist - 5,
            DressLength = 105,
            Thigh = (hip + 5) / 2,
            LegLength = 80
        };
    }

    public async Task<string> CreateDiaryWithMeasurementAsync(MeasurementDiaryCreateRequest request)
    {
        var user = await _unitOfWork.UserRepository.GetByIdAsync(request.Diary.UserId);
        if (user == null)
            throw new ErrorException(StatusCodes.Status404NotFound, ErrorCode.NotFound, "User not found");
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
            throw new ErrorException(StatusCodes.Status404NotFound, ErrorCode.NotFound, "Measurement not found");

        var diary = await _unitOfWork.MeasurementDiaryRepository.GetByIdAsync(measurement.MeasurementDiaryId);
        if (diary == null)
            throw new ErrorException(StatusCodes.Status404NotFound, ErrorCode.NotFound, "Measurement Diary not found");
        
        _mapper.Map(dto, measurement);
        await _unitOfWork.MeasurementRepository.UpdateAsync(measurement);
        await _unitOfWork.SaveChangesAsync();

        return _mapper.Map<MeasurementDto>(measurement);
    }
    
    public async Task<bool> DeleteMeasurementAsync(string id)
    {
        var measurement = await _unitOfWork.MeasurementRepository.GetByIdAsync(id);
        if (measurement == null)
            throw new ErrorException(StatusCodes.Status404NotFound, ErrorCode.NotFound, "Measurement not found");

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
        var existingMeasurement = await _unitOfWork.MeasurementRepository
            .FindAsync(m => m.MeasurementDiaryId == measurementDiaryId && m.WeekOfPregnancy == weekOfPregnancy);
        return true;
    }
}

