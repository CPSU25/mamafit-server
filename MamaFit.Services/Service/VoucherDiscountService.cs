using AutoMapper;
using MamaFit.BusinessObjects.DTO.VoucherDiscountDto;
using MamaFit.BusinessObjects.Entity;
using MamaFit.Repositories.Implement;
using MamaFit.Repositories.Infrastructure;
using MamaFit.Repositories.Interface;
using MamaFit.Services.Interface;
using Microsoft.AspNetCore.Http;

namespace MamaFit.Services.Service;

public class VoucherDiscountService : IVoucherDiscountService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IValidationService _validation;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public VoucherDiscountService(IUnitOfWork unitOfWork, IMapper mapper, IValidationService validation, IHttpContextAccessor httpContextAccessor)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _validation = validation;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<PaginatedList<VoucherDiscountResponseDto>> GetAllAsync(int index, int pageSize, string? codeSearch)
    {
        var voucherDiscounts = await _unitOfWork.VoucherDiscountRepository.GetAllAsync(index, pageSize, codeSearch);
        var responseItems = voucherDiscounts.Items
            .Select(voucher => _mapper.Map<VoucherDiscountResponseDto>(voucher))
            .ToList();
        
        return new PaginatedList<VoucherDiscountResponseDto>(
            responseItems,
            voucherDiscounts.TotalCount,
            index,
            pageSize
        );
    }
    
    public async Task<VoucherDiscountResponseDto> GetByIdAsync(string id)
    {
        var voucherDiscount = await _unitOfWork.VoucherDiscountRepository.GetByIdNotDeletedAsync(id);
        _validation.CheckNotFound(voucherDiscount, "Voucher discount not found");
        
        return _mapper.Map<VoucherDiscountResponseDto>(voucherDiscount);
    }
    
    public async Task CreateAsync(VoucherDiscountRequestDto request)
    {
        await _validation.ValidateAndThrowAsync(request);
        var user = await _unitOfWork.UserRepository.GetByIdNotDeletedAsync(request.UserId);
        _validation.CheckNotFound(user, "User not found");

        var voucherBatch = await _unitOfWork.VoucherBatchRepository.GetByIdNotDeletedAsync(request.VoucherBatchId);
        _validation.CheckNotFound(voucherBatch, "Voucher batch not found");
        voucherBatch!.RemainingQuantity -= 1;

        var voucherDiscount = _mapper.Map<VoucherDiscount>(request);

        await _unitOfWork.VoucherDiscountRepository.InsertAsync(voucherDiscount);
        await _unitOfWork.SaveChangesAsync();
    }
    
    public async Task UpdateAsync(string id, VoucherDiscountRequestDto request)
    {
        var voucherDiscount = await _unitOfWork.VoucherDiscountRepository.GetByIdNotDeletedAsync(id);
        _validation.CheckNotFound(voucherDiscount, "Voucher discount not found");
        var user = await _unitOfWork.UserRepository.GetByIdNotDeletedAsync(request.UserId);
        _validation.CheckNotFound(user, "User not found");
        var voucherBatch = await _unitOfWork.VoucherBatchRepository.GetByIdNotDeletedAsync(request.VoucherBatchId);
        _validation.CheckNotFound(voucherBatch, "Voucher batch not found");
        _mapper.Map(request, voucherDiscount);
        await _unitOfWork.VoucherDiscountRepository.UpdateAsync(voucherDiscount);
        await _unitOfWork.SaveChangesAsync();
    }
    
    public async Task DeleteAsync(string id)
    {
        var voucherDiscount = await _unitOfWork.VoucherDiscountRepository.GetByIdNotDeletedAsync(id);
        _validation.CheckNotFound(voucherDiscount, "Voucher discount not found");
        
        await _unitOfWork.VoucherDiscountRepository.SoftDeleteAsync(id);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task<List<VoucherDiscountResponseDto>> GetAllByCurrentUser()
    {
        var userId = GetCurrentUserId();
        var listVoucher = await _unitOfWork.VoucherDiscountRepository.GetAllByCurrentUserAsync(userId);

        _validation.CheckNotFound(listVoucher, "No voucher discounts found for the current user.");
        return listVoucher.Select(voucher => _mapper.Map<VoucherDiscountResponseDto>(voucher)).ToList();
    }

    private string GetCurrentUserId()
    {
        return _httpContextAccessor.HttpContext?.User.FindFirst("userId")?.Value
               ?? throw new InvalidOperationException("User ID not found in the current context.");
    }
}