using AutoMapper;
using MamaFit.BusinessObjects.DTO.CartItemDto;
using MamaFit.BusinessObjects.Entity;
using MamaFit.Repositories.Implement;
using MamaFit.Repositories.Infrastructure;
using MamaFit.Services.Interface;

namespace MamaFit.Services.Service;

public class CartItemService : ICartItemService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IValidationService _validation;
    
    public CartItemService(IUnitOfWork unitOfWork, IMapper mapper, IValidationService validation)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _validation = validation;
    }
    
    public async Task<PaginatedList<CartItemResponseDto>> GetAllAsync(int index, int pageSize)
    {
        var cartItems = await _unitOfWork.CartItemRepository.GetAllAsync(index, pageSize);
        return _mapper.Map<PaginatedList<CartItemResponseDto>>(cartItems);
    }
    
    public async Task<CartItemResponseDto> GetByIdAsync(int id)
    {
        var cartItem = await _unitOfWork.CartItemRepository.GetByIdAsync(id);
        return _mapper.Map<CartItemResponseDto>(cartItem);
    }

    public async Task<CartItemResponseDto> CreateAsync(CartItemRequestDto model)
    {
        var cartItem = _mapper.Map<CartItem>(model);
        await _unitOfWork.CartItemRepository.InsertAsync(cartItem);
        await _unitOfWork.SaveChangesAsync();
        
        return _mapper.Map<CartItemResponseDto>(cartItem);
    }
    
    public async Task<CartItemResponseDto> UpdateAsync(int id, CartItemRequestDto cartItemUpdateDto)
    {
        var cartItem = await _unitOfWork.CartItemRepository.GetByIdAsync(id);
        _validation.CheckNotFound(cartItem, $"Cart item with ID {id} not found.");

        _mapper.Map(cartItemUpdateDto, cartItem);
        _unitOfWork.CartItemRepository.Update(cartItem);
        await _unitOfWork.SaveChangesAsync();
        
        return _mapper.Map<CartItemResponseDto>(cartItem);
    }
    
    public async Task<bool> DeleteAsync(int id)
    {
        var cartItem = await _unitOfWork.CartItemRepository.GetByIdAsync(id);
        _validation.CheckNotFound(cartItem, $"Cart item with ID {id} not found.");
        _unitOfWork.CartItemRepository.SoftDeleteAsync(cartItem);
        await _unitOfWork.SaveChangesAsync();
        
        return true;
    }
}