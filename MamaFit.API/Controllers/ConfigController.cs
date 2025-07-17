using MamaFit.BusinessObjects.DTO.AddOnDto;
using MamaFit.BusinessObjects.DTO.CMSDto;
using MamaFit.Repositories.Infrastructure;
using MamaFit.Services.Interface;
using MamaFit.Services.Service;
using Microsoft.AspNetCore.Mvc;

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

        [HttpGet]
        public async Task<IActionResult> GetConfig()
        {
            var config = await _configService.GetConfig();
            return Ok(new ResponseModel<CmsServiceBaseDto>(
            StatusCodes.Status200OK,
            ApiCodes.SUCCESS,
            config,
            "Get config successfully!"
        ));
        }
    }
}
