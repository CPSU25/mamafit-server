using AutoMapper;
using MamaFit.BusinessObjects.DTO.MaternityDressServiceDto;
using MamaFit.BusinessObjects.Enum;
using MamaFit.Repositories.Implement;
using MamaFit.Repositories.Infrastructure;
using MamaFit.Services.Interface;

namespace MamaFit.Services.Service;

public class MaternityDressServiceService : IMaternityDressServiceService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidationService _validationService;
    private readonly IMapper _mapper;
    
    public MaternityDressServiceService(IUnitOfWork unitOfWork, IValidationService validationService, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _validationService = validationService;
        _mapper = mapper;
    }
    
    public async Task<PaginatedList<MaternityDressServiceDto>> GetAllAsync(int index, int pageSize, string? search, EntitySortBy? sortBy)
    {
        var maternityDressServiceList = await _unitOfWork.MaternityDressServiceRepository.GetAllAsync(index, pageSize, search, sortBy);
        
        var responseList = maternityDressServiceList.Items.Select(item => _mapper.Map<MaternityDressServiceDto>(item)).ToList();
        
        var paginatedResponse = new PaginatedList<MaternityDressServiceDto>(
            responseList,
            maternityDressServiceList.TotalCount,
            maternityDressServiceList.PageNumber,
            maternityDressServiceList.PageSize
        );
        
        return paginatedResponse;
    }

    public async Task CreateAsync(MaternityDressServiceRequestDto requestDto)
    {
        await _validationService.ValidateAndThrowAsync(requestDto);

        var newMaternityDressService = _mapper.Map<BusinessObjects.Entity.MaternityDressService>(requestDto);

        await _unitOfWork.MaternityDressServiceRepository.InsertAsync(newMaternityDressService);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task<MaternityDressServiceDto> GetByIdAsync(string id)
    {
        var maternityDressService = await _unitOfWork.MaternityDressServiceRepository.GetByIdAsync(id);
        _validationService.CheckNotFound(maternityDressService, "Maternity Dress Service not found!");
        return _mapper.Map<MaternityDressServiceDto>(maternityDressService);
    }

    public async Task UpdateAsync(string id, MaternityDressServiceRequestDto requestDto)
    {
        var oldMaternityDressService = await _unitOfWork.MaternityDressServiceRepository.GetByIdAsync(id);
        _validationService.CheckNotFound(oldMaternityDressService, "Maternity Dress Service not found!");
        await _validationService.ValidateAndThrowAsync(requestDto);
        _mapper.Map(requestDto, oldMaternityDressService);
        await _unitOfWork.MaternityDressServiceRepository.UpdateAsync(oldMaternityDressService);
        await _unitOfWork.SaveChangesAsync();
    }
    public async Task DeleteAsync(string id)
    {
        var oldMaternityDressService = await _unitOfWork.MaternityDressServiceRepository.GetByIdAsync(id);
        _validationService.CheckNotFound(oldMaternityDressService, "Maternity Dress Service not found!");
        await _unitOfWork.MaternityDressServiceRepository.SoftDeleteAsync(id);
        await _unitOfWork.SaveChangesAsync();
    }
}