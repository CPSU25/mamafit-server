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

    [HttpPost("webhook")]
    public async Task<IActionResult> ProcessPaymentWebhook([FromBody] SepayWebhookPayload payload,
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
}