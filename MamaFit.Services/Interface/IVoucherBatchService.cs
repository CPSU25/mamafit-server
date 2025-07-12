using MamaFit.BusinessObjects.DTO.VoucherBatchDto;
using MamaFit.Repositories.Infrastructure;

namespace MamaFit.Services.Interface;

public interface IVoucherBatchService
{
    Task<PaginatedList<VoucherBatchResponseDto>> GetAllVoucherBatchesAsync(int index, int pageSize, string? search);
    Task<List<VoucherBatchDetailResponseDto>> GetAllMyVoucherBatchAsync();
    Task<VoucherBatchResponseDto?> GetVoucherBatchByIdAsync(string id);
    Task<VoucherBatchResponseDto> CreateVoucherBatchAsync(VoucherBatchRequestDto requestDto);
    Task<VoucherBatchResponseDto> UpdateVoucherBatchAsync(string id, VoucherBatchRequestDto requestDto);
    Task DeleteVoucherBatchAsync(string id);
}