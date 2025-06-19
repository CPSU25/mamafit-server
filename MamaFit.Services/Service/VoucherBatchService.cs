using AutoMapper;
using MamaFit.BusinessObjects.DTO.VoucherBatchDto;
using MamaFit.BusinessObjects.Entity;
using MamaFit.Repositories.Infrastructure;
using MamaFit.Repositories.Interface;
using MamaFit.Services.Interface;

namespace MamaFit.Services.Service;

public class VoucherBatchService : IVoucherBatchService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IValidationService _validation;
    
    public VoucherBatchService(IUnitOfWork unitOfWork, IMapper mapper, IValidationService validation)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _validation = validation;
    }

    public async Task<PaginatedList<VoucherBatchResponseDto>> GetAllVoucherBatchesAsync(int index = 1,
        int pageSize = 10, string? nameSearch = null)
    {
        var voucherBatches = await _unitOfWork.VoucherBatchRepository.GetAllAsync(index, pageSize, nameSearch);
        var responseItems = voucherBatches.Items
            .Select(batch => _mapper.Map<VoucherBatchResponseDto>(batch))
            .ToList();
        return new PaginatedList<VoucherBatchResponseDto>(
            responseItems,
            voucherBatches.TotalCount,
            voucherBatches.PageNumber,
            pageSize
        );
    }

    public async Task<VoucherBatchResponseDto?> GetVoucherBatchByIdAsync(string id)
    {
        var voucherBatch = await _unitOfWork.VoucherBatchRepository.GetByIdNotDeletedAsync(id);
        _validation.CheckNotFound(voucherBatch, "Voucher batch not found");
        return _mapper.Map<VoucherBatchResponseDto>(voucherBatch);
    }
    
    public async Task<VoucherBatchResponseDto> CreateVoucherBatchAsync(VoucherBatchDto requestDto)
    {
        await _validation.ValidateAndThrowAsync(requestDto);
        var existedBatch = await _unitOfWork.VoucherBatchRepository.IsBatchExistedAsync(requestDto.BatchCode, requestDto.BatchName);
        _validation.CheckConflict(existedBatch, "Voucher batch with the same code and name already exists");
        var voucherBatch = _mapper.Map<VoucherBatch>(requestDto);
        await _unitOfWork.VoucherBatchRepository.InsertAsync(voucherBatch);
        await _unitOfWork.SaveChangesAsync();
        return _mapper.Map<VoucherBatchResponseDto>(voucherBatch);
    }
    
    public async Task<VoucherBatchResponseDto> UpdateVoucherBatchAsync(string id, VoucherBatchDto requestDto)
    {
        await _validation.ValidateAndThrowAsync(requestDto);
        var voucherBatch = await _unitOfWork.VoucherBatchRepository.GetByIdNotDeletedAsync(id);
        _validation.CheckNotFound(voucherBatch, "Voucher batch not found");
        
        var existedBatch = await _unitOfWork.VoucherBatchRepository.IsBatchExistedAsync(requestDto.BatchCode, requestDto.BatchName);
        _validation.CheckConflict(existedBatch, "Voucher batch with the same code and name already exists");
        
        voucherBatch = _mapper.Map(requestDto, voucherBatch);
        await _unitOfWork.VoucherBatchRepository.UpdateAsync(voucherBatch);
        await _unitOfWork.SaveChangesAsync();
        return _mapper.Map<VoucherBatchResponseDto>(voucherBatch);
    }
    
    public async Task DeleteVoucherBatchAsync(string id)
    {
        var voucherBatch = await _unitOfWork.VoucherBatchRepository.GetByIdNotDeletedAsync(id);
        _validation.CheckNotFound(voucherBatch, "Voucher batch not found");
        await _unitOfWork.VoucherBatchRepository.SoftDeleteAsync(voucherBatch.Id);
        await _unitOfWork.SaveChangesAsync();
    }
}