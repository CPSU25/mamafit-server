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
}