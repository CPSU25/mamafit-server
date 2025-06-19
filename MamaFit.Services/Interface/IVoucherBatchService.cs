using MamaFit.BusinessObjects.DTO.VoucherBatchDto;
using MamaFit.Repositories.Infrastructure;

namespace MamaFit.Services.Interface;

public interface IVoucherBatchService
{
    Task<PaginatedList<VoucherBatchResponseDto>> GetAllVoucherBatchesAsync(int index, int pageSize, string? search);
    Task<VoucherBatchResponseDto?> GetVoucherBatchByIdAsync(string id);
    Task<VoucherBatchResponseDto> CreateVoucherBatchAsync(VoucherBatchDto requestDto);
    Task<VoucherBatchResponseDto> UpdateVoucherBatchAsync(string id, VoucherBatchDto requestDto);
    Task DeleteVoucherBatchAsync(string id);
}