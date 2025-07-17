using Contentful.Core;
using Contentful.Core.Models;
using MamaFit.BusinessObjects.DTO.CMSDto;
using MamaFit.Services.ExternalService.Redis;
using MamaFit.Services.Interface;
using Microsoft.Extensions.Configuration;

namespace MamaFit.Services.Service
{
    public class ConfigService : IConfigService
    {
        private readonly ICacheService _cacheService;
        private readonly HttpClient _httpClient;
        private readonly ContentfulClient _contentfulClient;
        private readonly IConfigurationSection _contentfulSettings;
        private readonly IConfiguration _configuration;

        public ConfigService(ICacheService cacheService, HttpClient httpClient, IConfiguration configuration)
        {
            _cacheService = cacheService;

            _httpClient = httpClient;
            _configuration = configuration;
            _contentfulSettings = configuration.GetSection("Contentful");
            var spaceId = _contentfulSettings!.GetSection("SpaceId").Value;
            var contentKey = _contentfulSettings!.GetSection("ContentDeliveryKey").Value;
            var entryId = _contentfulSettings!.GetSection("EntryId").Value;
            _contentfulClient = new ContentfulClient(httpClient, contentKey, null, spaceId, false);
        }

        public async Task<CmsServiceBaseDto> GetConfig()
        {
            var response = await _cacheService.GetAsync<CmsServiceBaseDto>("cms:service:base");

            if (response?.Fields == null)
            {
                var entryId = _contentfulSettings!.GetSection("EntryId").Value;
                var contentfulResponse = await _contentfulClient.GetEntry<CmsFieldDto>(entryId);
                var config = new CmsServiceBaseDto
                {
                    Fields = contentfulResponse
                };
                await _cacheService.SetAsync("cms:service:base", config);

                response = config;
            }
            return response;
        }
    }
}
