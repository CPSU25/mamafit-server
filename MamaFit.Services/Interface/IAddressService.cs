using MamaFit.BusinessObjects.DTO.AddressDto;
using MamaFit.Repositories.Infrastructure;

namespace MamaFit.Services.Interface;

public interface IAddressService
{
    Task<PaginatedList<AddressResponseDto>> GetAllAsync(int index, int pageSize);
    Task<AddressResponseDto> GetByIdAsync(string id);
    Task CreateAsync(AddressRequestDto requestDto);
    Task UpdateAsync(string id, AddressRequestDto requestDto);
    Task DeleteAsync(string id);
}