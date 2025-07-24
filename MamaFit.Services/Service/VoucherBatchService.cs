using AutoMapper;
using MamaFit.BusinessObjects.DTO.VoucherBatchDto;
using MamaFit.BusinessObjects.Entity;
using MamaFit.BusinessObjects.Enum;
using MamaFit.Repositories.Implement;
using MamaFit.Repositories.Infrastructure;
using MamaFit.Services.Interface;
using Microsoft.AspNetCore.Http;

namespace MamaFit.Services.Service;

public class VoucherBatchService : IVoucherBatchService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IValidationService _validation;
    private readonly IHttpContextAccessor _context;

    public VoucherBatchService(IUnitOfWork unitOfWork, IMapper mapper, IValidationService validation, IHttpContextAccessor context)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _validation = validation;
        _context = context;
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
    
    public async Task<VoucherBatchResponseDto> CreateVoucherBatchAsync(VoucherBatchRequestDto requestDto)
    {
        await _validation.ValidateAndThrowAsync(requestDto);

        var existedBatch = await _unitOfWork.VoucherBatchRepository.IsBatchExistedAsync(requestDto.BatchCode!, requestDto.BatchName!);
        _validation.CheckConflict(existedBatch, "Voucher batch with the same code and name already exists");

        var voucherBatch = _mapper.Map<VoucherBatch>(requestDto);
        voucherBatch.RemainingQuantity = requestDto.TotalQuantity;

        if (voucherBatch.DiscountType == DiscountType.PERCENTAGE && voucherBatch.DiscountValue > 100)
        {
            throw new ErrorException(StatusCodes.Status400BadRequest, ApiCodes.BAD_REQUEST, "DiscountValue must be between 0 and 100");
        }

        await _unitOfWork.VoucherBatchRepository.InsertAsync(voucherBatch);
        await _unitOfWork.SaveChangesAsync();
        
        if (voucherBatch.IsAutoGenerate == true && voucherBatch.TotalQuantity.HasValue)
        {
            var generatedVouchers = GenerateVoucherDiscounts(voucherBatch, voucherBatch.TotalQuantity.Value);
            foreach (var voucher in generatedVouchers)
            {
                await _unitOfWork.VoucherDiscountRepository.InsertAsync(voucher);
            }
            await _unitOfWork.SaveChangesAsync();
        }

        return _mapper.Map<VoucherBatchResponseDto>(voucherBatch);
    }

    
    public async Task<VoucherBatchResponseDto> UpdateVoucherBatchAsync(string id, VoucherBatchRequestDto requestDto)
    {
        await _validation.ValidateAndThrowAsync(requestDto);
        var voucherBatch = await _unitOfWork.VoucherBatchRepository.GetByIdNotDeletedAsync(id);
        _validation.CheckNotFound(voucherBatch, "Voucher batch not found");
        
        var existedBatch = await _unitOfWork.VoucherBatchRepository.IsBatchExistedAsync(requestDto.BatchCode!, requestDto.BatchName!);
        _validation.CheckConflict(existedBatch, "Voucher batch with the same code and name already exists");
        
        voucherBatch = _mapper.Map(requestDto, voucherBatch);
        await _unitOfWork.VoucherBatchRepository.UpdateAsync(voucherBatch!);
        await _unitOfWork.SaveChangesAsync();
        return _mapper.Map<VoucherBatchResponseDto>(voucherBatch);
    }
    
    public async Task DeleteVoucherBatchAsync(string id)
    {
        var voucherBatch = await _unitOfWork.VoucherBatchRepository.GetByIdNotDeletedAsync(id);
        _validation.CheckNotFound(voucherBatch, "Voucher batch not found");
        await _unitOfWork.VoucherBatchRepository.SoftDeleteAsync(voucherBatch!.Id);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task<List<VoucherBatchDetailResponseDto>> GetAllMyVoucherBatchAsync()
    {
        var userId = GetCurrentUserId();
        var user = await _unitOfWork.UserRepository.GetByIdNotDeletedAsync(userId);
        _validation.CheckNotFound(user, $"User with id: {userId}");

        var myVoucherBatchList = await _unitOfWork.VoucherBatchRepository.GetAllMyVoucherAsync(userId);
        var responseItems = myVoucherBatchList
            .Select(batch => _mapper.Map<VoucherBatchDetailResponseDto>(batch))
            .ToList();

        return responseItems;
    }

    private List<VoucherDiscount> GenerateVoucherDiscounts(VoucherBatch batch, int quantity)
    {
        var vouchers = new List<VoucherDiscount>();
        for (int i = 0; i < quantity; i++)
        {
            var code = $"{batch.BatchCode?.ToUpper()}-{GenerateRandomCode(6)}";
            var voucher = new VoucherDiscount
            {
                VoucherBatchId = batch.Id,
                Code = code,
                Status = VoucherStatus.ACTIVE,
                UserId = GetCurrentUserId(),
                CreatedAt = DateTime.UtcNow,
                CreatedBy = "System"
            };
            vouchers.Add(voucher);
        }

        return vouchers;
    }

    private static string GenerateRandomCode(int length)
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        var random = new Random();
        return new string(Enumerable.Repeat(chars, length)
            .Select(s => s[random.Next(s.Length)]).ToArray());
    }
    
    private string GetCurrentUserId()
    {
        var userId = _context.HttpContext?.User.FindFirst("userId")?.Value;
        return userId ?? string.Empty;
    }
}