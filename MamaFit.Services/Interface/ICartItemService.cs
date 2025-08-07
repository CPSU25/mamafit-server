using MamaFit.BusinessObjects.DTO.CartItemDto;

namespace MamaFit.Services.Interface;

public interface ICartItemService
{
    Task<List<CartItemResponseDto>> GetAllAsync();
    Task<List<CartItemResponseDto>> CreateAsync(CartItemRequestDto model);
    Task<List<CartItemResponseDto>> UpdateAsync( CartItemRequestDto request);
    Task<bool> DeleteAsync(string itemId);
    Task<bool> ClearCartAsync();
}