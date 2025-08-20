using AutoMapper;
using MamaFit.BusinessObjects.DTO.BranchMaternityDressDetailDto;
using MamaFit.BusinessObjects.Entity;
using MamaFit.Repositories.Helper;
using MamaFit.Repositories.Implement;
using MamaFit.Repositories.Infrastructure;
using MamaFit.Repositories.Interface;
using MamaFit.Services.Interface;
using Microsoft.AspNetCore.Http;

namespace MamaFit.Services.Service;

public class BranchMaternityDressDetailService : IBranchMaternityDressDetailService
{
    private readonly IBranchMaternityDressDetailRepository _repository;
    private readonly IMapper _mapper;
    private readonly IValidationService _validation;
    private readonly IUnitOfWork _unitOfWork;

    public BranchMaternityDressDetailService(
        IBranchMaternityDressDetailRepository repository,
        IMapper mapper,
        IValidationService validation,
        IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _mapper = mapper;
        _validation = validation;
        _unitOfWork = unitOfWork;
    }

    public async Task<PaginatedList<GetDetailById>> GetAllAsync(int index, int pageSize, string accessToken, string? search)
    {
        var userId = JwtTokenHelper.ExtractUserId(accessToken);
        var dressDetails = await _repository.GetAllAsync(index, pageSize, userId, search);
        var responseList = dressDetails.Items.Select(item => _mapper.Map<GetDetailById>(item)).ToList();

        return new PaginatedList<GetDetailById>(
            responseList,
            dressDetails.TotalCount,
            dressDetails.PageNumber,
            dressDetails.PageSize
        );
    }

    public async Task<GetDetailById> GetByIdAsync(string branchId, string dressId)
    {
        var dressDetail = await _repository.GetByIdAsync(branchId, dressId);
        _validation.CheckNotFound(dressDetail,
            $"Dress detail with branchId: {branchId} and dressId: {dressId} is not found");
        return _mapper.Map<GetDetailById>(dressDetail);
    }

    public async Task<BranchMaternityDressDetailDto> CreateAsync(BranchMaternityDressDetailDto request)
    {
        var mdd = await _unitOfWork.MaternityDressDetailRepository.GetByIdAsync(request.MaternityDressDetailId);
        _validation.CheckNotFound(mdd,
            $"Maternity dress detail with id: {request.MaternityDressDetailId} is not found");
        if (request.BranchId != null)
        {
            var branch = await _unitOfWork.BranchRepository.GetByIdAsync(request.BranchId);
            _validation.CheckNotFound(branch, $"Branch with id: {request.BranchId} is not found");
        }
        if (request.Quantity > mdd.Quantity)
            throw new ErrorException(StatusCodes.Status400BadRequest, ApiCodes.BAD_REQUEST,
                $"Quantity {request.Quantity} exceeds available quantity {mdd.Quantity} for dress detail with id: {request.MaternityDressDetailId}");

        mdd.Quantity -= request.Quantity ?? 0;
        await _unitOfWork.MaternityDressDetailRepository.UpdateAsync(mdd);
        var dressDetail = _mapper.Map<BranchMaternityDressDetail>(request);
        await _repository.InsertAsync(dressDetail);
        return _mapper.Map<BranchMaternityDressDetailDto>(dressDetail);
    }

    public async Task<BranchMaternityDressDetailDto> UpdateAsync(BranchMaternityDressDetailDto request)
    {
        var dressDetail = await _repository.GetByIdAsync(request.BranchId, request.MaternityDressDetailId);
        _validation.CheckNotFound(dressDetail,
            $"Dress detail with branchId: {request.BranchId} and dressId: {request.MaternityDressDetailId} is not found");
        _mapper.Map(request, dressDetail);
        await _repository.UpdateAsync(dressDetail);
        return _mapper.Map<BranchMaternityDressDetailDto>(dressDetail);
    }

    public async Task DeleteAsync(string branchId, string dressId)
    {
        var dressDetail = await _repository.GetByIdAsync(branchId, dressId);
        _validation.CheckNotFound(dressDetail,
            $"Dress detail with branchId: {branchId} and dressId: {dressId} is not found");
        await _repository.DeleteAsync(branchId, dressId);
    }
}