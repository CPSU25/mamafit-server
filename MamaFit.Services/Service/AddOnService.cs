using AutoMapper;
using MamaFit.BusinessObjects.DTO.AddOnDto;
using MamaFit.BusinessObjects.Enum;
using MamaFit.Repositories.Implement;
using MamaFit.Repositories.Infrastructure;
using MamaFit.Services.Interface;

namespace MamaFit.Services.Service;

public class AddOnService : IAddOnService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidationService _validationService;
    private readonly IMapper _mapper;
    
    public AddOnService(IUnitOfWork unitOfWork, IValidationService validationService, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _validationService = validationService;
        _mapper = mapper;
    }
    
    public async Task<PaginatedList<AddOnDto>> GetAllAsync(int index, int pageSize, string? search, EntitySortBy? sortBy)
    {
        var maternityDressServiceList = await _unitOfWork.AddOnRepository.GetAllAsync(index, pageSize, search, sortBy);
        
        var responseList = maternityDressServiceList.Items.Select(item => _mapper.Map<AddOnDto>(item)).ToList();
        
        var paginatedResponse = new PaginatedList<AddOnDto>(
            responseList,
            maternityDressServiceList.TotalCount,
            maternityDressServiceList.PageNumber,
            maternityDressServiceList.PageSize
        );
        
        return paginatedResponse;
    }

    public async Task CreateAsync(AddOnRequestDto requestDto)
    {
        await _validationService.ValidateAndThrowAsync(requestDto);

        var newMaternityDressService = _mapper.Map<BusinessObjects.Entity.AddOn>(requestDto);

        await _unitOfWork.AddOnRepository.InsertAsync(newMaternityDressService);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task<AddOnDto> GetByIdAsync(string id)
    {
        var maternityDressService = await _unitOfWork.AddOnRepository.GetByIdAsync(id);
        _validationService.CheckNotFound(maternityDressService, "Maternity Dress Service not found!");
        return _mapper.Map<AddOnDto>(maternityDressService);
    }

    public async Task UpdateAsync(string id, AddOnRequestDto requestDto)
    {
        var oldMaternityDressService = await _unitOfWork.AddOnRepository.GetByIdAsync(id);
        _validationService.CheckNotFound(oldMaternityDressService, "Maternity Dress Service not found!");
        await _validationService.ValidateAndThrowAsync(requestDto);
        _mapper.Map(requestDto, oldMaternityDressService);
        await _unitOfWork.AddOnRepository.UpdateAsync(oldMaternityDressService);
        await _unitOfWork.SaveChangesAsync();
    }
    public async Task DeleteAsync(string id)
    {
        var oldMaternityDressService = await _unitOfWork.AddOnRepository.GetByIdAsync(id);
        _validationService.CheckNotFound(oldMaternityDressService, "Maternity Dress Service not found!");
        await _unitOfWork.AddOnRepository.SoftDeleteAsync(id);
        await _unitOfWork.SaveChangesAsync();
    }
}