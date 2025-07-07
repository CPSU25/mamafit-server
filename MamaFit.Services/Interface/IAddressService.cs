using MamaFit.BusinessObjects.DTO.AddressDto;
using MamaFit.Repositories.Infrastructure;

namespace MamaFit.Services.Interface;

public interface IAddressService
{
    Task<PaginatedList<AddressResponseDto>> GetAllAsync(int index, int pageSize);
    Task<AddressResponseDto> GetByIdAsync(string id);
    Task<List<AddressResponseDto>> GetByAccessTokenAsync(string accessToken);
    Task<AddressResponseDto> CreateAsync(AddressRequestDto requestDto, string accessToken);
    Task UpdateAsync(string id, AddressRequestDto requestDto);
    Task DeleteAsync(string id);
}