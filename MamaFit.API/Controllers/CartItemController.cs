using MamaFit.BusinessObjects.DTO.CartItemDto;
using MamaFit.Repositories.Infrastructure;
using MamaFit.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace MamaFit.API.Controllers;

[ApiController]
[Route("api/cart-item")]
public class CartItemController : ControllerBase
{
    private readonly ICartItemService _cartItemService;
    
    public CartItemController(ICartItemService cartItemService)
    {
        _cartItemService = cartItemService;
    }
    
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var cartItems = await _cartItemService.GetAllAsync();
        return Ok(new ResponseModel<List<CartItemResponseDto>>(
            StatusCodes.Status200OK,
            ApiCodes.SUCCESS,
            cartItems,
            "Get all cart item successfully!"
        ));
    }
    
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CartItemRequestDto requestDto)
    {
        var createdCartItem = await _cartItemService.CreateAsync(requestDto);
        return StatusCode(StatusCodes.Status201Created,
            new ResponseModel<List<CartItemResponseDto>>(
                StatusCodes.Status201Created,
                ApiCodes.CREATED,
                createdCartItem,
                "Created cart item successfully!"
            ));
    }
    
    [HttpPut]
    public async Task<IActionResult> Update([FromBody] CartItemRequestDto requestDto)
    {
        var updatedCartItem = await _cartItemService.UpdateAsync(requestDto);
        return Ok(new ResponseModel<List<CartItemResponseDto>>(
            StatusCodes.Status200OK,
            ApiCodes.SUCCESS,
            updatedCartItem,
            "Updated cart item successfully!"
        ));
    }
    
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete([FromRoute] string itemId)
    {
        await _cartItemService.DeleteAsync(itemId);
        return Ok(new ResponseModel<string>(
            StatusCodes.Status200OK,
            ApiCodes.SUCCESS,
            null,
            "Deleted cart item successfully!"
        ));
    }

    [HttpDelete("all-item")]
    public async Task<IActionResult> DeleteAllItemCurrentUser()
    {
        await _cartItemService.ClearCartAsync();
        return Ok(new ResponseModel<string>(
            StatusCodes.Status200OK,
            ApiCodes.SUCCESS,
            null,
            "Deleted all cart item successfully!"
        ));
    }
}