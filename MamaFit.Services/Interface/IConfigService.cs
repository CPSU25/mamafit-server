using MamaFit.BusinessObjects.DTO.CMSDto;

namespace MamaFit.Services.Interface
{
    public interface IConfigService
    {
        Task<CmsServiceBaseDto> GetConfig();
    }
}
