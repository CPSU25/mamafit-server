using Contentful.Core;
using MamaFit.BusinessObjects.DTO.CMSDto;
using MamaFit.Repositories.Helper;
using MamaFit.Services.ExternalService.Redis;
using MamaFit.Services.Interface;
using Microsoft.Extensions.Options;

namespace MamaFit.Services.Service
{
    public class ConfigService : IConfigService
    {
        private readonly ICacheService _cacheService;
        private readonly ContentfulClient _contentfulClient;
        private readonly ContentfulSettings _contentfulSettings;

        public ConfigService(
            ICacheService cacheService, 
            IHttpClientFactory httpClientFactory, 
            IOptions<ContentfulSettings> contentfulOptions)
        {
            _cacheService = cacheService;
            _contentfulSettings = contentfulOptions.Value;
            var httpClient = httpClientFactory.CreateClient("ContentfulClient");
            _contentfulClient = new ContentfulClient(
                httpClient,
                _contentfulSettings.ContentDeliveryKey,
                null,
                _contentfulSettings.SpaceId,
                false
            );
        }

        public async Task<CmsServiceBaseDto> GetConfig()
        {
            var response = await _cacheService.GetAsync<CmsServiceBaseDto>("cms:service:base");
            if (response?.Fields == null)
            {
                var contentfulResponse = await _contentfulClient.GetEntry<CmsFieldDto>(_contentfulSettings.EntryId);
                var config = new CmsServiceBaseDto
                {
                    Fields = contentfulResponse
                };
                await _cacheService.SetAsync("cms:service:base", config, TimeSpan.FromDays(30));
                response = config;
            }
            return response;
        }
    }
}