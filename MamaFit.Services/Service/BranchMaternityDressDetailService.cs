using AutoMapper;
using MamaFit.BusinessObjects.DTO.BranchMaternityDressDetailDto;
using MamaFit.BusinessObjects.Entity;
using MamaFit.Repositories.Infrastructure;
using MamaFit.Repositories.Interface;
using MamaFit.Services.Interface;

namespace MamaFit.Services.Service;

public class BranchMaternityDressDetailService : IBranchMaternityDressDetailService
{
    private readonly IBranchMaternityDressDetailRepository _repository;
    private readonly IMapper _mapper;
    private readonly IValidationService _validation;
    
    public BranchMaternityDressDetailService(IBranchMaternityDressDetailRepository repository, IMapper mapper, IValidationService validation)
    {
        _repository = repository;
        _mapper = mapper;
        _validation = validation;
    }
    
    public async Task<PaginatedList<BranchMaternityDressDetailDto>> GetAllAsync(int index, int pageSize, string? search)
    {
        var dressDetails = await _repository.GetAllAsync(index, pageSize, search);
        var responseList = dressDetails.Items.Select(item => _mapper.Map<BranchMaternityDressDetailDto>(item)).ToList();
        
        return new PaginatedList<BranchMaternityDressDetailDto>(
            responseList,
            dressDetails.TotalCount,
            dressDetails.PageNumber,
            dressDetails.PageSize
        );
    }
    
    public async Task<BranchMaternityDressDetailDto> GetByIdAsync(string branchId, string dressId)
    {
        var dressDetail = await _repository.GetByIdAsync(branchId, dressId);
        _validation.CheckNotFound(dressDetail, $"Dress detail with branchId: {branchId} and dressId: {dressId} is not found");
        return _mapper.Map<BranchMaternityDressDetailDto>(dressDetail);
    }
    
    public async Task<BranchMaternityDressDetailDto> CreateAsync(BranchMaternityDressDetailDto request)
    {
        var dressDetail = _mapper.Map<BranchMaternityDressDetail>(request);
        await _repository.InsertAsync(dressDetail);
        return _mapper.Map<BranchMaternityDressDetailDto>(dressDetail);
    }
    
    public async Task<BranchMaternityDressDetailDto> UpdateAsync(BranchMaternityDressDetailDto request)
    {
        var dressDetail = await _repository.GetByIdAsync(request.BranchId, request.MaternityDressDetailId);
        _validation.CheckNotFound(dressDetail, $"Dress detail with branchId: {request.BranchId} and dressId: {request.MaternityDressDetailId} is not found");
        _mapper.Map(request, dressDetail);
        await _repository.UpdateAsync(dressDetail);
        return _mapper.Map<BranchMaternityDressDetailDto>(dressDetail);
    }
    
    public async Task DeleteAsync(string branchId, string dressId)
    {
        var dressDetail = await _repository.GetByIdAsync(branchId, dressId);
        _validation.CheckNotFound(dressDetail, $"Dress detail with branchId: {branchId} and dressId: {dressId} is not found");
        await _repository.DeleteAsync(branchId, dressId);
    }
}