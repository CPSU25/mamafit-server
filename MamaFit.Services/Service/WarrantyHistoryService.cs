using AutoMapper;
using MamaFit.BusinessObjects.DTO.WarrantyHistoryDto;
using MamaFit.BusinessObjects.Entity;
using MamaFit.Repositories.Implement;
using MamaFit.Repositories.Infrastructure;
using MamaFit.Services.Interface;

namespace MamaFit.Services.Service;

public class WarrantyHistoryService : IWarrantyHistoryService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IValidationService _validationService;
    
    public WarrantyHistoryService(IUnitOfWork unitOfWork, IMapper mapper, IValidationService validationService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _validationService = validationService;
    }
    
    public async Task<PaginatedList<WarrantyHistoryResponseDto>> GetAllAsync(int index = 1, int pageSize = 10, DateTime? startDate = null, DateTime? endDate = null)
    {
        var warrantyHistories = await _unitOfWork.WarrantyHistoryRepository.GetAllAsync(index, pageSize, startDate, endDate);
        var responseItems = warrantyHistories.Items
            .Select(history => _mapper.Map<WarrantyHistoryResponseDto>(history))
            .ToList();
        
        return new PaginatedList<WarrantyHistoryResponseDto>(
            responseItems,
            warrantyHistories.TotalCount,
            warrantyHistories.PageNumber,
            pageSize
        );
    }
    
    public async Task<WarrantyHistoryResponseDto> GetByIdAsync(string id) 
    {
        var warrantyHistory = await _unitOfWork.WarrantyHistoryRepository.GetByIdNotDeletedAsync(id);
        _validationService.CheckNotFound(warrantyHistory, "Warranty history not found");
        return _mapper.Map<WarrantyHistoryResponseDto>(warrantyHistory);
    }
    
    public async Task<WarrantyHistoryResponseDto> CreateAsync(WarrantyHistoryRequestDto requestDto)
    {
        var warrantyHistory = _mapper.Map<WarrantyHistory>(requestDto);
        await _unitOfWork.WarrantyHistoryRepository.InsertAsync(warrantyHistory);
        await _unitOfWork.SaveChangesAsync();
        return _mapper.Map<WarrantyHistoryResponseDto>(warrantyHistory);
    }
    
    public async Task<WarrantyHistoryResponseDto> UpdateAsync(string id, WarrantyHistoryRequestDto requestDto)
    {
        var warrantyHistory = await _unitOfWork.WarrantyHistoryRepository.GetByIdNotDeletedAsync(id);
        _validationService.CheckNotFound(warrantyHistory, "Warranty history not found");
        
        _mapper.Map(requestDto, warrantyHistory);
        await _unitOfWork.WarrantyHistoryRepository.UpdateAsync(warrantyHistory);
        await _unitOfWork.SaveChangesAsync();
        
        return _mapper.Map<WarrantyHistoryResponseDto>(warrantyHistory);
    }
    
    public async Task DeleteAsync(string id)
    {
        var warrantyHistory = await _unitOfWork.WarrantyHistoryRepository.GetByIdNotDeletedAsync(id);
        _validationService.CheckNotFound(warrantyHistory, "Warranty history not found");
        
        await _unitOfWork.WarrantyHistoryRepository.SoftDeleteAsync(id);
        await _unitOfWork.SaveChangesAsync();
    }
}