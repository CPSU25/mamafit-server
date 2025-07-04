using MamaFit.BusinessObjects.DTO.GhtkDto;
using MamaFit.BusinessObjects.DTO.GhtkDto.Fee;
using MamaFit.BusinessObjects.DTO.GhtkDto.Response;
using MamaFit.Services.ExternalService.Ghtk;
using Microsoft.AspNetCore.Mvc;

namespace MamaFit.API.Controllers;

[ApiController]
public class GhtkController : ControllerBase
{
    private readonly IGhtkService _ghtkService;
    
    public GhtkController(IGhtkService ghtkService)
    {
        _ghtkService = ghtkService;
    }
    
    [HttpGet("ghtk-authenticated")]
    public async Task<IActionResult> GhtkAuthenticated()
    {
        var result = await _ghtkService.AuthenticateGhtkAsync();
        if (!result.Success)
            return StatusCode(401, result);
        return Ok(result);
    }
    
    [HttpPost("ghtk-submit-order/{orderId}")]
    public async Task<IActionResult> SubmitOrderExpress([FromRoute] string orderId, [FromBody] GhtkRecipentDto dto)
    {
        var result = await _ghtkService.SubmitOrderExpressAsync(orderId, dto);
        if (!result.Success)
            return StatusCode(400, result);
        return Ok(result);
    }
    
    [HttpGet("ghtk-order-status/{trackingOrderCode}")]
    public async Task<IActionResult> GetGhtkOrderStatus(string trackingOrderCode)
    {
        var result = await _ghtkService.GetOrderStatusAsync(trackingOrderCode);
        if (result == null || !result.Success)
            return NotFound(new { success = false, message = result?.Message ?? "Không lấy được trạng thái đơn hàng" });
        return Ok(result);
    }
    
    [HttpPost("ghtk-cancel-order/{trackingOrderCode}")]
    public async Task<IActionResult> CancelGhtkOrder(string trackingOrderCode)
    {
        var result = await _ghtkService.CancelOrderAsync(trackingOrderCode);
        if (result == null)
            return StatusCode(500, new { success = false, message = "Không kết nối được GHTK" });
        if (!result.Success)
            return BadRequest(result);
        return Ok(result);
    }

    [HttpGet("ghtk-address-level4")]
    public async Task<IActionResult> GetAddressLevel4([FromQuery] string province, [FromQuery] string district, [FromQuery] string wardStreet, [FromQuery] string? address = null)
    {
        var result = await _ghtkService.GetAddressLevel4Async(province, district, wardStreet, address);
        if (result == null || !result.Success)
            return BadRequest(new { success = false, message = "Không lấy được danh sách địa chỉ cấp 4" });
        return Ok(result);
    }

    [HttpGet("ghtk-pick-addresses")]
    public async Task<IActionResult> GetPickupAddressList()
    {
        var result = await _ghtkService.GetListPickAddressAsync();
        if (result == null || !result.Success)
            return BadRequest(new { success = false, message = "Không lấy được danh sách địa chỉ lấy hàng" });
        return Ok(result);
    }
    
    [HttpGet("ghtk-fee")]
    public async Task<IActionResult> GetGhtkFee([FromQuery] GhtkFeeRequestDto dto)
    {
        var result = await _ghtkService.GetFeeAsync(dto);
        if (result == null || !result.Success)
            return BadRequest(new { success = false, message = result?.Message ?? "Không tính được phí vận chuyển" });
        return Ok(result);
    }

}