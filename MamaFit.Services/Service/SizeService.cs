using AutoMapper;
using MamaFit.BusinessObjects.DTO.SizeDto;
using MamaFit.Repositories.Implement;
using MamaFit.Repositories.Infrastructure;
using MamaFit.Services.Interface;

namespace MamaFit.Services.Service;

public class SizeService : ISizeService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IValidationService _validationService;
    
    public SizeService(IUnitOfWork unitOfWork, IMapper mapper, IValidationService validationService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _validationService = validationService;
    }
    
    public async Task<PaginatedList<SizeDto>> GetAllAsync(int index, int pageSize, string? search)
    {
        var sizeList = await _unitOfWork.SizeRepository.GetAllAsync(index, pageSize, search);
        
        var responseList = sizeList.Items.Select(item => _mapper.Map<SizeDto>(item)).ToList();
        
        var paginatedResponse = new PaginatedList<SizeDto>(
            responseList,
            sizeList.TotalCount,
            sizeList.PageNumber,
            sizeList.PageSize
        );
        
        return paginatedResponse;
    }
    
    public async Task CreateAsync(SizeRequestDto requestDto)
    {
        await _validationService.ValidateAndThrowAsync(requestDto);
        
        var newSize = _mapper.Map<BusinessObjects.Entity.Size>(requestDto);
        
        await _unitOfWork.SizeRepository.InsertAsync(newSize);
        await _unitOfWork.SaveChangesAsync();
    }
    
    public async Task<SizeDto> GetByIdAsync(string id)
    {
        var size = await _unitOfWork.SizeRepository.GetByIdAsync(id);
        _validationService.CheckNotFound(size, "Size not found!");
        return _mapper.Map<SizeDto>(size);
    }
    
    public async Task UpdateAsync(string id, SizeRequestDto requestDto)
    {
        var oldSize = await _unitOfWork.SizeRepository.GetByIdAsync(id);
        _validationService.CheckNotFound(oldSize, "Size not found!");
        await _validationService.ValidateAndThrowAsync(requestDto);
        
        var updatedSize = _mapper.Map(requestDto, oldSize);
        
        _unitOfWork.SizeRepository.Update(updatedSize);
        await _unitOfWork.SaveChangesAsync();
    }
    
    public async Task DeleteAsync(string id)
    {
        var size = await _unitOfWork.SizeRepository.GetByIdAsync(id);
        _validationService.CheckNotFound(size, "Size not found!");
        
        await _unitOfWork.SizeRepository.DeleteAsync(size);
        await _unitOfWork.SaveChangesAsync();
    }
}