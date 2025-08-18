using MamaFit.BusinessObjects.DTO.CMSDto;
using MamaFit.Repositories.Infrastructure;
using MamaFit.Services.Interface;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace MamaFit.API.Controllers
{
    [ApiController]
    [Route("api/config")]
    public class ConfigController : ControllerBase
    {
        private readonly IConfigService _configService;

        public ConfigController(IConfigService configService)
        {
            _configService = configService;
        }

        // GET /api/config
        [HttpGet]
        [ProducesResponseType(typeof(ResponseModel<CmsServiceBaseDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetConfig()
        {
            var config = await _configService.GetConfig();
            return Ok(new ResponseModel<CmsServiceBaseDto>(
                StatusCodes.Status200OK, ApiCodes.SUCCESS, config, "Get config successfully!"
            ));
        }

        // PATCH /api/config  — universal update
        [HttpPatch]
        [ProducesResponseType(typeof(ResponseModel<object?>), StatusCodes.Status200OK)]
        public async Task<IActionResult> PatchConfig([FromBody] JsonElement body)
        {
            await _configService.UpdateConfigAsync(body);
            return Ok(new ResponseModel<object?>(
                StatusCodes.Status200OK, ApiCodes.SUCCESS, null, "Update config successfully!"
            ));
        }

        // (Optional) Giữ tương thích cũ: POST cũng gọi PATCH
        [HttpPost]
        [ProducesResponseType(typeof(ResponseModel<object?>), StatusCodes.Status200OK)]
        public Task<IActionResult> PostConfig([FromBody] JsonElement body)
            => PatchConfig(body);
    }
}
