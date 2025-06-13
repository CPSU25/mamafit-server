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
    public MeasurementDiaryService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }
    
    public async Task<PaginatedList<MeasurementDiaryResponseDto>> GetAllAsync(int index = 1, int pageSize = 10, string? nameSearch = null)
    {
        var diaries = await _unitOfWork.MeasurementDiaryRepository.GetAllDiariesAsync(index, pageSize, nameSearch);
        
        if (diaries == null)
            throw new ErrorException(StatusCodes.Status404NotFound, ErrorCode.NotFound, "No measurement diaries found");
        
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

    public async Task<MeasurementDiaryResponseDto> GetDiaryByIdAsync(string id)
    {
        var diary = await _unitOfWork.MeasurementDiaryRepository.GetByIdNotDeletedAsync(id);
        if (diary == null)
            throw new ErrorException(StatusCodes.Status404NotFound, ErrorCode.NotFound, "Measurement diary not found");
        return _mapper.Map<MeasurementDiaryResponseDto>(diary);
    }
    
    public async Task<bool> DeleteDiaryAsync(string id)
    {
        var diary = await _unitOfWork.MeasurementDiaryRepository.GetByIdNotDeletedAsync(id);
        if (diary == null)
            throw new ErrorException(StatusCodes.Status404NotFound, ErrorCode.NotFound, "Measurement diary not found");
        
        await _unitOfWork.MeasurementDiaryRepository.SoftDeleteAsync(id);
        await _unitOfWork.SaveChangesAsync();
        return true;
    }
}