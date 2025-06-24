using MamaFit.BusinessObjects.DTO.CartItemDto;
using MamaFit.Repositories.Infrastructure;

namespace MamaFit.Services.Interface;

public interface ICartItemService
{
    Task<PaginatedList<CartItemResponseDto>> GetAllAsync(int index, int pageSize);
    Task<CartItemResponseDto> GetByIdAsync(int id);
    Task<CartItemResponseDto> CreateAsync(CartItemRequestDto model);
    Task<CartItemResponseDto> UpdateAsync(int id, CartItemRequestDto cartItemUpdateDto);
    Task<bool> DeleteAsync(int id);
}