// using Azure.Core;
// using Contentful.Core;
// using MamaFit.BusinessObjects.DTO.CMSDto;
// using MamaFit.Repositories.Helper;
// using MamaFit.Services.ExternalService.Redis;
// using MamaFit.Services.Interface;
// using Microsoft.Extensions.Options;
// using System.Net.Http;
// using System.Net.Http.Headers;
// using System.Text;
// using System.Text.Json;

// namespace MamaFit.Services.Service
// {
//     public class ConfigService : IConfigService
//     {
//         private readonly ICacheService _cacheService;
//         private readonly ContentfulClient _contentfulClient;
//         private readonly ContentfulSettings _contentfulSettings;
//         private readonly HttpClient _httpClient;

//         public ConfigService(
//             ICacheService cacheService,
//             IHttpClientFactory httpClientFactory,
//             IOptions<ContentfulSettings> contentfulOptions,
//             HttpClient httpClient)
//         {
//             _cacheService = cacheService;
//             _contentfulSettings = contentfulOptions.Value;
//             var deliveryClient = httpClientFactory.CreateClient("ContentfulClient");
//             _contentfulClient = new ContentfulClient(
//                 deliveryClient,
//                 _contentfulSettings.ContentDeliveryKey,
//                 null,
//                 _contentfulSettings.SpaceId,
//                 false
//             );
//             _httpClient = httpClient;
//         }

//         public async Task<CmsServiceBaseDto> GetConfig()
//         {
//             var response = await _cacheService.GetAsync<CmsServiceBaseDto>("cms:service:base");
//             if (response?.Fields == null)
//             {
//                 var contentfulResponse = await _contentfulClient.GetEntry<CmsFieldDto>(_contentfulSettings.EntryId);
//                 var config = new CmsServiceBaseDto
//                 {
//                     Fields = contentfulResponse
//                 };
//                 await _cacheService.SetAsync("cms:service:base", config, TimeSpan.FromDays(30));
//                 response = config;
//             }
//             return response;
//         }

//         public async Task<bool> UpdateConfigAsync(CmsFieldDto newConfig)
//         {
//             // Base URL Contentful Management API
//             _httpClient.BaseAddress = new Uri("https://api.contentful.com/");
//             _httpClient.DefaultRequestHeaders.Clear();
//             _httpClient.DefaultRequestHeaders.Authorization =
//                 new AuthenticationHeaderValue("Bearer", _contentfulSettings.ManagementToken);

//             var entryUrl = $"spaces/{_contentfulSettings.SpaceId}/environments/master/entries/{_contentfulSettings.EntryId}";

//             // 1. Get entry to retrieve version
//             var getEntryResponse = await _httpClient.GetAsync(entryUrl);
//             getEntryResponse.EnsureSuccessStatusCode();

//             var entryJson = await getEntryResponse.Content.ReadAsStringAsync();
//             using var doc = JsonDocument.Parse(entryJson);
//             var version = doc.RootElement.GetProperty("sys").GetProperty("version").GetInt32();

//             // 2. Build update payload
//             var payload = new
//             {
//                 fields = new
//                 {
//                     name = new Dictionary<string, object> { ["en-US"] = newConfig.Name },
//                     designRequestServiceFee = new Dictionary<string, object> { ["en-US"] = newConfig.DesignRequestServiceFee },
//                     depositRate = new Dictionary<string, object> { ["en-US"] = newConfig.DepositRate },
//                     presetVersions = new Dictionary<string, object> { ["en-US"] = newConfig.PresetVersions },
//                     warrantyTime = new Dictionary<string, object> { ["en-US"] = newConfig.WarrantyTime },
//                     appointmentSlotInterval = new Dictionary<string, object> { ["en-US"] = newConfig.AppointmentSlotInterval },
//                     maxAppointmentPerDay = new Dictionary<string, object> { ["en-US"] = newConfig.MaxAppointmentPerDay },
//                     maxAppointmentPerUser = new Dictionary<string, object> { ["en-US"] = newConfig.MaxAppointmentPerUser },
//                     warrantyPeriod = new Dictionary<string, object> { ["en-US"] = newConfig.WarrantyPeriod }
//                 }
//             };

//             var jsonPayload = JsonSerializer.Serialize(payload);
//             var content = new StringContent(jsonPayload, Encoding.UTF8,
//                 "application/vnd.contentful.management.v1+json");

//             // 3. Update entry
//             content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/vnd.contentful.management.v1+json");
//             _httpClient.DefaultRequestHeaders.Remove("X-Contentful-Version");
//             _httpClient.DefaultRequestHeaders.Add("X-Contentful-Version", version.ToString());

//             var updateResponse = await _httpClient.PutAsync(entryUrl, content);
//             updateResponse.EnsureSuccessStatusCode();

//             // 4. Publish entry (POST, not PUT)
//             var publishUrl = $"{entryUrl}/published";
//             var publishResponse = await _httpClient.PostAsync(publishUrl, null);

//             // 5. Clear cache
//             await _cacheService.RemoveAsync("cms:service:base");

//             return true;
//         }

//         public async Task<bool> UpdateAttributesAsync(IEnumerable<string>? colors = null,
//                                                       IEnumerable<string>? sizes = null)
//         {
//             // Chuẩn hóa list
//             static List<string> Norm(IEnumerable<string> src) =>
//                 src.Where(s => !string.IsNullOrWhiteSpace(s))
//                    .Select(s => s.Trim())
//                    .Distinct(StringComparer.OrdinalIgnoreCase)
//                    .ToList();

//             var ops = new List<object>();

//             // 1) Lấy version + biết field đã tồn tại chưa
//             var entryUrl = $"https://api.contentful.com/spaces/{_contentfulSettings.SpaceId}/environments/master/entries/{_contentfulSettings.EntryId}";
//             _httpClient.DefaultRequestHeaders.Clear();
//             _httpClient.DefaultRequestHeaders.Authorization =
//                 new AuthenticationHeaderValue("Bearer", _contentfulSettings.ManagementToken);

//             var getRes = await _httpClient.GetAsync(entryUrl);
//             getRes.EnsureSuccessStatusCode();

//             var json = await getRes.Content.ReadAsStringAsync();
//             using var doc = JsonDocument.Parse(json);
//             var version = doc.RootElement.GetProperty("sys").GetProperty("version").GetInt32();
//             var fieldsEl = doc.RootElement.GetProperty("fields");

//             bool hasColors = fieldsEl.TryGetProperty("colors", out var cNode) && cNode.TryGetProperty("en-US", out _);
//             bool hasSizes = fieldsEl.TryGetProperty("sizes", out var sNode) && sNode.TryGetProperty("en-US", out _);

//             // 2) Build JSON Patch ops
//             if (colors is not null)
//             {
//                 var list = Norm(colors);
//                 ops.Add(new
//                 {
//                     op = hasColors ? "replace" : "add",
//                     path = hasColors ? "/fields/colors/en-US" : "/fields/colors",
//                     value = hasColors ? list : new Dictionary<string, object> { ["en-US"] = list }
//                 });
//             }

//             if (sizes is not null)
//             {
//                 var list = Norm(sizes);
//                 ops.Add(new
//                 {
//                     op = hasSizes ? "replace" : "add",
//                     path = hasSizes ? "/fields/sizes/en-US" : "/fields/sizes",
//                     value = hasSizes ? list : new Dictionary<string, object> { ["en-US"] = list }
//                 });
//             }

//             if (ops.Count == 0) return true; // Không có gì để update

//             var req = new HttpRequestMessage(new HttpMethod("PATCH"), entryUrl)
//             {
//                 Content = new StringContent(JsonSerializer.Serialize(ops), Encoding.UTF8, "application/json-patch+json")
//             };
//             req.Headers.Add("X-Contentful-Version", version.ToString());

//             var patchRes = await _httpClient.SendAsync(req);
//             patchRes.EnsureSuccessStatusCode();

//             // Lấy version mới từ response để publish
//             var updated = JsonDocument.Parse(await patchRes.Content.ReadAsStringAsync());
//             var newVersion = updated.RootElement.GetProperty("sys").GetProperty("version").GetInt32();

//             // 3) Publish (PUT)
//             var publishReq = new HttpRequestMessage(HttpMethod.Put, $"{entryUrl}/published");
//             publishReq.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _contentfulSettings.ManagementToken);
//             publishReq.Headers.Add("X-Contentful-Version", newVersion.ToString());
//             var publishRes = await _httpClient.SendAsync(publishReq);
//             publishRes.EnsureSuccessStatusCode();

//             await _cacheService.RemoveAsync("cms:service:base");
//             return true;
//         }

//     }
// }

using Contentful.Core;
using MamaFit.BusinessObjects.DTO.CMSDto;
using MamaFit.Repositories.Helper;
using MamaFit.Services.ExternalService.Redis;
using MamaFit.Services.Interface;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace MamaFit.Services.Service
{
    public class ConfigService : IConfigService
    {
        private readonly ICacheService _cacheService;
        private readonly ContentfulClient _contentfulClient;     // Delivery API
        private readonly ContentfulSettings _contentfulSettings;
        private readonly HttpClient _httpClient;                  // Management API

        private const string DefaultLocale = "en-US";
        private const string CacheKey = "cms:service:base";

        private static readonly JsonSerializerOptions JsonOpts = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
        };

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
            _httpClient.BaseAddress = new Uri("https://api.contentful.com/");
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/vnd.contentful.management.v1+json"));
        }

        private string EntryUrl =>
            $"spaces/{_contentfulSettings.SpaceId}/environments/master/entries/{_contentfulSettings.EntryId}";

        private void SetAuth()
        {
            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", _contentfulSettings.ManagementToken);
        }

        private static int ExtractVersion(HttpResponseMessage response)
        {
            var json = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
            using var doc = JsonDocument.Parse(json);
            return doc.RootElement.GetProperty("sys").GetProperty("version").GetInt32();
        }

        private async Task PublishAsync(int version)
        {
            var publishReq = new HttpRequestMessage(HttpMethod.Put, $"{EntryUrl}/published");
            publishReq.Headers.Authorization =
                new AuthenticationHeaderValue("Bearer", _contentfulSettings.ManagementToken);
            publishReq.Headers.Add("X-Contentful-Version", version.ToString());
            await _httpClient.SendAsync(publishReq);
        }

        public async Task<CmsServiceBaseDto> GetConfig()
        {
            var response = await _cacheService.GetAsync<CmsServiceBaseDto>(CacheKey);
            if (response?.Fields == null)
            {
                var contentfulResponse = await _contentfulClient.GetEntry<CmsFieldDto>(_contentfulSettings.EntryId);

                // đảm bảo không null để FE render
                contentfulResponse.Colors ??= new List<string>();
                contentfulResponse.Sizes ??= new List<string>();
                contentfulResponse.JobTitles ??= new List<string>();

                var config = new CmsServiceBaseDto { Fields = contentfulResponse };
                await _cacheService.SetAsync(CacheKey, config, TimeSpan.FromDays(30));
                response = config;
            }
            return response;
        }

        /// <summary>
        /// Universal update (PATCH): truyền field nào thì update field đó.
        /// - Không gửi: giữ nguyên
        /// - null: remove
        /// - có giá trị: add/replace tùy trạng thái tồn tại
        /// </summary>
        public async Task<bool> UpdateConfigAsync(JsonElement body)
        {
            SetAuth();

            // 1) Lấy entry hiện tại để biết version & field nào đã tồn tại (theo locale)
            var getRes = await _httpClient.GetAsync(EntryUrl);
            var currentJson = await getRes.Content.ReadAsStringAsync();
            using var current = JsonDocument.Parse(currentJson);

            var version = current.RootElement.GetProperty("sys").GetProperty("version").GetInt32();
            var fieldsEl = current.RootElement.TryGetProperty("fields", out var f) ? f : default;

            bool Has(string field)
                => fieldsEl.ValueKind != JsonValueKind.Undefined
                   && fieldsEl.TryGetProperty(field, out var node)
                   && node.TryGetProperty(DefaultLocale, out _);

            var ops = new List<object>();

            // Helper: add/remove/replace for scalar number
            void PatchNumber<T>(string name, Func<JsonElement, T> read) where T : struct
            {
                if (!body.TryGetProperty(name, out var el)) return;

                if (el.ValueKind == JsonValueKind.Null)
                {
                    if (Has(name)) ops.Add(new { op = "remove", path = $"/fields/{name}/{DefaultLocale}" });
                    return;
                }

                var value = read(el);
                var exists = Has(name);
                var path = exists ? $"/fields/{name}/{DefaultLocale}" : $"/fields/{name}";
                object payload = exists
                    ? (object)value
                    : new Dictionary<string, object?> { [DefaultLocale] = value };

                ops.Add(new { op = exists ? "replace" : "add", path, value = payload });
            }

            // Helper: add/remove/replace for string
            void PatchString(string name)
            {
                if (!body.TryGetProperty(name, out var el)) return;

                if (el.ValueKind == JsonValueKind.Null)
                {
                    if (Has(name)) ops.Add(new { op = "remove", path = $"/fields/{name}/{DefaultLocale}" });
                    return;
                }

                var value = el.GetString();
                var exists = Has(name);
                var path = exists ? $"/fields/{name}/{DefaultLocale}" : $"/fields/{name}";
                object payload = exists
                    ? (object?)value
                    : new Dictionary<string, object?> { [DefaultLocale] = value };

                ops.Add(new { op = exists ? "replace" : "add", path, value = payload });
            }

            // Helper: add/remove/replace for string[]
            void PatchStringArray(string name)
            {
                if (!body.TryGetProperty(name, out var el)) return;

                if (el.ValueKind == JsonValueKind.Null)
                {
                    if (Has(name)) ops.Add(new { op = "remove", path = $"/fields/{name}/{DefaultLocale}" });
                    return;
                }

                var list = new List<string>();
                if (el.ValueKind == JsonValueKind.Array)
                {
                    foreach (var item in el.EnumerateArray())
                    {
                        if (item.ValueKind == JsonValueKind.String)
                        {
                            var s = item.GetString();
                            if (!string.IsNullOrWhiteSpace(s))
                                list.Add(s.Trim());
                        }
                    }
                    // unique (case-insensitive)
                    list = list.Distinct(StringComparer.OrdinalIgnoreCase).ToList();
                }

                var exists = Has(name);
                var path = exists ? $"/fields/{name}/{DefaultLocale}" : $"/fields/{name}";
                object payload = exists
                    ? (object)list
                    : new Dictionary<string, object?> { [DefaultLocale] = list };

                ops.Add(new { op = exists ? "replace" : "add", path, value = payload });
            }

            // 2) Build ops cho tất cả field bạn đang dùng
            PatchString("name");
            PatchNumber<decimal>("designRequestServiceFee", e => e.GetDecimal());
            PatchNumber<double>("depositRate", e => e.GetDouble());
            PatchNumber<int>("presetVersions", e => e.GetInt32());
            PatchNumber<int>("warrantyTime", e => e.GetInt32());
            PatchNumber<int>("appointmentSlotInterval", e => e.GetInt32());
            PatchNumber<int>("maxAppointmentPerDay", e => e.GetInt32());
            PatchNumber<int>("maxAppointmentPerUser", e => e.GetInt32());
            PatchNumber<int>("warrantyPeriod", e => e.GetInt32());
            PatchStringArray("colors");
            PatchStringArray("sizes");
            PatchStringArray("jobTitle");

            if (ops.Count == 0) return true; 

            // 3) PATCH
            var opsJson = JsonSerializer.Serialize(ops, JsonOpts);

            var req = new HttpRequestMessage(HttpMethod.Patch, EntryUrl);
            req.Content = new StringContent(opsJson, Encoding.UTF8);
            req.Content.Headers.ContentType =
                new MediaTypeHeaderValue("application/vnd.contentful.management.v1+json"); 

            req.Headers.Authorization =
                new AuthenticationHeaderValue("Bearer", _contentfulSettings.ManagementToken);
            req.Headers.Add("X-Contentful-Version", version.ToString());

            // (không bắt buộc nhưng gọn gàng)
            req.Headers.Accept.Clear();
            req.Headers.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/vnd.contentful.management.v1+json"));

            var patchRes = await _httpClient.SendAsync(req);

            // 5) Clear cache
            await _cacheService.RemoveAsync(CacheKey);
            return true;
        }
    }
}
