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
    public async Task<IActionResult> GetAll(
        [FromQuery] int index = 1,
        [FromQuery] int pageSize = 10)
    {
        var cartItems = await _cartItemService.GetAllAsync(index, pageSize);
        return Ok(new ResponseModel<PaginatedList<CartItemResponseDto>>(
            StatusCodes.Status200OK,
            ApiCodes.SUCCESS,
            cartItems,
            "Get all cart item successfully!"
        ));
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById([FromRoute] int id)
    {
        var cartItem = await _cartItemService.GetByIdAsync(id);
        return Ok(new ResponseModel<CartItemResponseDto>(
            StatusCodes.Status200OK,
            ApiCodes.SUCCESS,
            cartItem,
            "Get cart item successfully!"
        ));
    }
    
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CartItemRequestDto requestDto)
    {
        var createdCartItem = await _cartItemService.CreateAsync(requestDto);
        return StatusCode(StatusCodes.Status201Created,
            new ResponseModel<CartItemResponseDto>(
                StatusCodes.Status201Created,
                ApiCodes.CREATED,
                createdCartItem,
                "Created cart item successfully!"
            ));
    }
    
    [HttpPut("{id}")]
    public async Task<IActionResult> Update([FromRoute] int id, [FromBody] CartItemRequestDto requestDto)
    {
        var updatedCartItem = await _cartItemService.UpdateAsync(id, requestDto);
        return Ok(new ResponseModel<CartItemResponseDto>(
            StatusCodes.Status200OK,
            ApiCodes.SUCCESS,
            updatedCartItem,
            "Updated cart item successfully!"
        ));
    }
    
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete([FromRoute] int id)
    {
        await _cartItemService.DeleteAsync(id);
        return Ok(new ResponseModel<string>(
            StatusCodes.Status200OK,
            ApiCodes.SUCCESS,
            null,
            "Deleted cart item successfully!"
        ));
    }
}