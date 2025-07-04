using MamaFit.BusinessObjects.DTO.SepayDto;
using MamaFit.Repositories.Infrastructure;
using MamaFit.Services.ExternalService.Sepay;
using Microsoft.AspNetCore.Mvc;

namespace MamaFit.API.Controllers;

[ApiController]
[Route("api/sepay-auth")]
public class SepayController : ControllerBase
{
    private readonly ISepayService _sepayService;

    public SepayController(ISepayService sepayService)
    {
        _sepayService = sepayService;
    }
    
    [HttpGet("status/{orderId}")]
    public async Task<IActionResult> GetPaymentStatus(string orderId)
    {
        var status = await _sepayService.GetPaymentStatusAsync(orderId);
        return Ok(new ResponseModel<string>(
            StatusCodes.Status200OK,
            ApiCodes.SUCCESS,
            status
        ));
    }

    [HttpPost("hooks/payment")]
    public async Task<IActionResult> ProcessPaymentWebhook(
        [FromBody] SepayWebhookPayload payload,
        [FromHeader(Name = "Authorization")] string authHeader)
    {
        await _sepayService.ProcessPaymentWebhookAsync(payload, authHeader);
        return Ok(new ResponseModel<string>(
            StatusCodes.Status200OK,
            ApiCodes.SUCCESS,
            null,
            "Payment processed successfully!"
        ));
    }

    [HttpGet("generate-qr/{orderId}")]
    public async Task<IActionResult> GeneratePaymentQr(string orderId)
    {
        var qrResponse = await _sepayService.CreatePaymentQrAsync(orderId);

        return Ok(new ResponseModel<SepayQrResponse>(
            StatusCodes.Status201Created,
            ApiCodes.SUCCESS,
            qrResponse,
            "QR code generated successfully"
        ));
    }
}