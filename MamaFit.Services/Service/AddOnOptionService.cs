using AutoMapper;
using MamaFit.BusinessObjects.DTO.AddOnOptionDto;
using MamaFit.BusinessObjects.Enum;
using MamaFit.Repositories.Implement;
using MamaFit.Repositories.Infrastructure;
using MamaFit.Services.Interface;

namespace MamaFit.Services.Service;

public class AddOnOptionService : IAddOnOptionService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidationService _validationService;
    private readonly IMapper _mapper;
    
    public AddOnOptionService(IUnitOfWork unitOfWork, IValidationService validationService, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _validationService = validationService;
        _mapper = mapper;
    }
    
    public async Task<PaginatedList<AddOnOptionDto>> GetAllAsync(int index, int pageSize, string? search, EntitySortBy? sortBy)
    {
        var addOnOptionList = await _unitOfWork.AddOnOptionRepository.GetAllAsync(index, pageSize, search, sortBy);
        
        var responseList = addOnOptionList.Items.Select(item => _mapper.Map<AddOnOptionDto>(item)).ToList();
        
        var paginatedResponse = new PaginatedList<AddOnOptionDto>(
            responseList,
            addOnOptionList.TotalCount,
            addOnOptionList.PageNumber,
            addOnOptionList.PageSize
        );
        
        return paginatedResponse;
    }
    
    public async Task CreateAsync(AddOnOptionRequestDto requestDto)
    {
        await _validationService.ValidateAndThrowAsync(requestDto);

        var newAddOnOption = _mapper.Map<BusinessObjects.Entity.AddOnOption>(requestDto);

        await _unitOfWork.AddOnOptionRepository.InsertAsync(newAddOnOption);
        await _unitOfWork.SaveChangesAsync();
    }
    
    public async Task<AddOnOptionDto> GetByIdAsync(string id)
    {
        var addOnOption = await _unitOfWork.AddOnOptionRepository.GetByIdAsync(id);
        _validationService.CheckNotFound(addOnOption, "Add-On Option not found!");
        return _mapper.Map<AddOnOptionDto>(addOnOption);
    }
    
    public async Task UpdateAsync(string id, AddOnOptionRequestDto requestDto)
    {
        var oldAddOnOption = await _unitOfWork.AddOnOptionRepository.GetByIdAsync(id);
        _validationService.CheckNotFound(oldAddOnOption, "Add-On Option not found!");
        await _validationService.ValidateAndThrowAsync(requestDto);

        var updatedAddOnOption = _mapper.Map(requestDto, oldAddOnOption);
        _unitOfWork.AddOnOptionRepository.Update(updatedAddOnOption);
        await _unitOfWork.SaveChangesAsync();
    }
    
    public async Task DeleteAsync(string id)
    {
        var addOnOption = await _unitOfWork.AddOnOptionRepository.GetByIdAsync(id);
        _validationService.CheckNotFound(addOnOption, "Add-On Option not found!");
        
        await _unitOfWork.AddOnOptionRepository.DeleteAsync(addOnOption);
        await _unitOfWork.SaveChangesAsync();
    }
}