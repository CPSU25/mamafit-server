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
}