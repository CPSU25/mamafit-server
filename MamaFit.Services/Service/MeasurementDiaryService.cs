using AutoMapper;
using MamaFit.BusinessObjects.DTO.MeasurementDto;
using MamaFit.Repositories.Infrastructure;
using MamaFit.Repositories.Interface;
using MamaFit.Services.Interface;
using Microsoft.AspNetCore.Http;

namespace MamaFit.Services.Service;

public class MeasurementDiaryService : IMeasurementDiaryService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IValidationService _validation;
    public MeasurementDiaryService(IUnitOfWork unitOfWork, IMapper mapper, IValidationService validation)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _validation = validation;
    }
    
    public async Task<MeasurementResponseDto> CalculateWeeksPregnantByDiaryIdAsync(string diaryId)
    {
        var diary = await _unitOfWork.MeasurementDiaryRepository.GetByIdNotDeletedAsync(diaryId);
        _validation.CheckNotFound(diary, "Measurement diary not found");
    
        if (diary.PregnancyStartDate == null)
            throw new ErrorException(StatusCodes.Status400BadRequest, ApiCodes.BAD_REQUEST, "Missing pregnancy start date");

        var weeks = (int)((DateTime.UtcNow - diary.PregnancyStartDate.Value).TotalDays / 7);

        var measurement = await _unitOfWork.MeasurementRepository
            .FindAsync(m => m.MeasurementDiaryId == diaryId &&
                            m.WeekOfPregnancy == weeks &&
                            !m.IsDeleted);

        if (measurement != null)
        {
            return _mapper.Map<MeasurementResponseDto>(measurement);
        }
        
        return new MeasurementResponseDto
        {
            WeekOfPregnancy = weeks
        };
    }

    
    public async Task<PaginatedList<MeasurementDiaryResponseDto>> GetAllAsync(int index = 1, int pageSize = 10, string? nameSearch = null)
    {
        var diaries = await _unitOfWork.MeasurementDiaryRepository.GetAllDiariesAsync(index, pageSize, nameSearch);
        
        if (diaries == null)
            throw new ErrorException(StatusCodes.Status404NotFound, ApiCodes.NOT_FOUND, "No measurement diaries found");
        
        var responseItems = diaries.Items
            .Select(diary => _mapper.Map<MeasurementDiaryResponseDto>(diary))
            .ToList();
        
        var responsePaginatedList = new PaginatedList<MeasurementDiaryResponseDto>(
            responseItems,
            diaries.TotalCount,
            diaries.PageNumber,
            pageSize
        );
        
        return responsePaginatedList;
    }

    public async Task<DiaryWithMeasurementDto> GetDiaryByIdAsync(string id, DateTime? startDate, DateTime? endDate)
    {
        var diary = await _unitOfWork.MeasurementDiaryRepository.GetDiaryByIdAsync(id, startDate, endDate);
        if (diary == null)
            throw new ErrorException(StatusCodes.Status404NotFound, ApiCodes.NOT_FOUND, "Measurement diary not found");
        return _mapper.Map<DiaryWithMeasurementDto>(diary);
    }
    
    public async Task<PaginatedList<MeasurementDiaryResponseDto>> GetDiariesByUserIdAsync(int index, int pageSize, string userId, string? nameSearch = null)
    {
        var diaries = await _unitOfWork.MeasurementDiaryRepository.GetByUserIdAsync(index, pageSize, userId, nameSearch);
        
        if (diaries == null)
            throw new ErrorException(StatusCodes.Status404NotFound, ApiCodes.NOT_FOUND, "No measurement diaries found for this user");
        
        var responseItems = diaries.Items
            .Select(diary => _mapper.Map<MeasurementDiaryResponseDto>(diary))
            .ToList();
        
        var responsePaginatedList = new PaginatedList<MeasurementDiaryResponseDto>(
            responseItems,
            diaries.TotalCount,
            diaries.PageNumber,
            pageSize
        );
        
        return responsePaginatedList;
    }
    
    public async Task<bool> DeleteDiaryAsync(string id)
    {
        var diary = await _unitOfWork.MeasurementDiaryRepository.GetByIdNotDeletedAsync(id);
        if (diary == null)
            throw new ErrorException(StatusCodes.Status404NotFound, ApiCodes.NOT_FOUND, "Measurement diary not found");
        
        await _unitOfWork.MeasurementDiaryRepository.SoftDeleteAsync(id);
        await _unitOfWork.SaveChangesAsync();
        return true;
    }
}