using MamaFit.BusinessObjects.DTO.VoucherDiscountDto;
using MamaFit.Repositories.Infrastructure;

namespace MamaFit.Services.Interface;

public interface IVoucherDiscountService
{
    Task<PaginatedList<VoucherDiscountResponseDto>> GetAllAsync(int index, int pageSize, string? codeSearch);
    Task<VoucherDiscountResponseDto> GetByIdAsync(string id);
    Task CreateAsync(VoucherDiscountRequestDto request);
    Task UpdateAsync(string id, VoucherDiscountRequestDto request);
    Task DeleteAsync(string id);
}