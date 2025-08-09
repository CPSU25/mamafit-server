using Azure.Core;
using Contentful.Core;
using MamaFit.BusinessObjects.DTO.CMSDto;
using MamaFit.Repositories.Helper;
using MamaFit.Services.ExternalService.Redis;
using MamaFit.Services.Interface;
using Microsoft.Extensions.Options;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace MamaFit.Services.Service
{
    public class ConfigService : IConfigService
    {
        private readonly ICacheService _cacheService;
        private readonly ContentfulClient _contentfulClient;
        private readonly ContentfulSettings _contentfulSettings;
        private readonly HttpClient _httpClient;

        public ConfigService(
            ICacheService cacheService,
            IHttpClientFactory httpClientFactory,
            IOptions<ContentfulSettings> contentfulOptions,
            HttpClient httpClient)
        {
            _cacheService = cacheService;
            _contentfulSettings = contentfulOptions.Value;
            var deliveryClient = httpClientFactory.CreateClient("ContentfulClient");
            _contentfulClient = new ContentfulClient(
                deliveryClient,
                _contentfulSettings.ContentDeliveryKey,
                null,
                _contentfulSettings.SpaceId,
                false
            );
            _httpClient = httpClient;
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

        public async Task<bool> UpdateConfigAsync(CmsFieldDto newConfig)
        {
            // Base URL Contentful Management API
            _httpClient.BaseAddress = new Uri("https://api.contentful.com/");
            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", _contentfulSettings.ManagementToken);

            var entryUrl = $"spaces/{_contentfulSettings.SpaceId}/environments/master/entries/{_contentfulSettings.EntryId}";

            // 1. Get entry to retrieve version
            var getEntryResponse = await _httpClient.GetAsync(entryUrl);
            getEntryResponse.EnsureSuccessStatusCode();

            var entryJson = await getEntryResponse.Content.ReadAsStringAsync();
            using var doc = JsonDocument.Parse(entryJson);
            var version = doc.RootElement.GetProperty("sys").GetProperty("version").GetInt32();

            // 2. Build update payload
            var payload = new
            {
                fields = new
                {
                    name = new Dictionary<string, object> { ["en-US"] = newConfig.Name },
                    designRequestServiceFee = new Dictionary<string, object> { ["en-US"] = newConfig.DesignRequestServiceFee },
                    depositRate = new Dictionary<string, object> { ["en-US"] = newConfig.DepositRate },
                    presetVersions = new Dictionary<string, object> { ["en-US"] = newConfig.PresetVersions },
                    warrantyTime = new Dictionary<string, object> { ["en-US"] = newConfig.WarrantyTime },
                    appointmentSlotInterval = new Dictionary<string, object> { ["en-US"] = newConfig.AppointmentSlotInterval },
                    maxAppointmentPerDay = new Dictionary<string, object> { ["en-US"] = newConfig.MaxAppointmentPerDay },
                    maxAppointmentPerUser = new Dictionary<string, object> { ["en-US"] = newConfig.MaxAppointmentPerUser },
                    warrantyPeriod = new Dictionary<string, object> { ["en-US"] = newConfig.WarrantyPeriod }
                }
            };

            var jsonPayload = JsonSerializer.Serialize(payload);
            var content = new StringContent(jsonPayload, Encoding.UTF8,
                "application/vnd.contentful.management.v1+json");

            // 3. Update entry
            content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/vnd.contentful.management.v1+json");
            _httpClient.DefaultRequestHeaders.Remove("X-Contentful-Version");
            _httpClient.DefaultRequestHeaders.Add("X-Contentful-Version", version.ToString());

            var updateResponse = await _httpClient.PutAsync(entryUrl, content);
            updateResponse.EnsureSuccessStatusCode();

            // 4. Publish entry (POST, not PUT)
            var publishUrl = $"{entryUrl}/published";
            var publishResponse = await _httpClient.PostAsync(publishUrl, null);

            // 5. Clear cache
            await _cacheService.RemoveAsync("cms:service:base");

            return true;
        }

    }
}
