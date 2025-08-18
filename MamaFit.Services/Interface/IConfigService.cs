using MamaFit.BusinessObjects.DTO.CMSDto;

namespace MamaFit.Services.Interface
{
    public interface IConfigService
    {
        Task<CmsServiceBaseDto> GetConfig();
        Task<bool> UpdateConfigAsync(System.Text.Json.JsonElement body);

    }
}
