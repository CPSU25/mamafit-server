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
        private readonly ContentfulClient _contentfulClient;
        private readonly ContentfulSettings _contentfulSettings;
        private readonly HttpClient _httpClient;

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

        private async Task PublishAsync(int version)
        {
            var publishReq = new HttpRequestMessage(HttpMethod.Put, $"{EntryUrl}/published");
            publishReq.Headers.Authorization =
                new AuthenticationHeaderValue("Bearer", _contentfulSettings.ManagementToken);
            publishReq.Headers.Add("X-Contentful-Version", version.ToString());
            publishReq.Headers.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/vnd.contentful.management.v1+json"));

            var publishRes = await _httpClient.SendAsync(publishReq);
            publishRes.EnsureSuccessStatusCode();
        }

        public async Task<CmsServiceBaseDto> GetConfig()
        {
            var response = await _cacheService.GetAsync<CmsServiceBaseDto>(CacheKey);
            if (response?.Fields == null)
            {
                var contentfulResponse = await _contentfulClient.GetEntry<CmsFieldDto>(_contentfulSettings.EntryId);

                contentfulResponse.Colors ??= new List<string>();
                contentfulResponse.Sizes ??= new List<string>();
                contentfulResponse.JobTitles ??= new List<string>();

                var config = new CmsServiceBaseDto { Fields = contentfulResponse };
                await _cacheService.SetAsync(CacheKey, config, TimeSpan.FromDays(30));
                response = config;
            }
            return response;
        }

        public async Task<bool> UpdateConfigAsync(JsonElement body)
        {
            SetAuth();

            // 1) GET entry -> version + fields + detect locale
            var getRes = await _httpClient.GetAsync(EntryUrl);
            getRes.EnsureSuccessStatusCode();

            var currentJson = await getRes.Content.ReadAsStringAsync();
            using var current = JsonDocument.Parse(currentJson);

            var version = current.RootElement.GetProperty("sys").GetProperty("version").GetInt32();
            var fieldsEl = current.RootElement.TryGetProperty("fields", out var f) ? f : default;

            var locale = DetectLocaleFromFields(fieldsEl, "en-US"); // auto detect, fallback en-US

            bool Has(string field)
                => fieldsEl.ValueKind != JsonValueKind.Undefined
                   && fieldsEl.TryGetProperty(CmsId(field), out var node)
                   && node.TryGetProperty(locale, out _);

            var ops = new List<object>();

            // --------- helpers ----------
            void PatchNumber(string name, Func<JsonElement, double> read)
            {
                if (!body.TryGetProperty(name, out var el)) return;
                var id = CmsId(name);

                if (el.ValueKind == JsonValueKind.Null)
                {
                    if (Has(name)) ops.Add(new { op = "remove", path = $"/fields/{id}/{locale}" });
                    return;
                }

                var value = read(el);
                var exists = Has(name);
                var path = exists ? $"/fields/{id}/{locale}" : $"/fields/{id}";
                object payload = exists
                    ? (object)value
                    : new Dictionary<string, object?> { [locale] = value };

                ops.Add(new { op = exists ? "replace" : "add", path, value = payload });
            }

            void PatchString(string name)
            {
                if (!body.TryGetProperty(name, out var el)) return;
                var id = CmsId(name);

                if (el.ValueKind == JsonValueKind.Null)
                {
                    if (Has(name)) ops.Add(new { op = "remove", path = $"/fields/{id}/{locale}" });
                    return;
                }

                var value = el.GetString();
                var exists = Has(name);
                var path = exists ? $"/fields/{id}/{locale}" : $"/fields/{id}";
                object payload = exists
                    ? (object?)value
                    : new Dictionary<string, object?> { [locale] = value };

                ops.Add(new { op = exists ? "replace" : "add", path, value = payload });
            }

            void PatchStringArray(string name)
            {
                if (!body.TryGetProperty(name, out var el)) return;
                var id = CmsId(name);

                if (el.ValueKind == JsonValueKind.Null)
                {
                    if (Has(name)) ops.Add(new { op = "remove", path = $"/fields/{id}/{locale}" });
                    return;
                }

                var list = new List<string>();
                if (el.ValueKind == JsonValueKind.Array)
                {
                    foreach (var item in el.EnumerateArray())
                        if (item.ValueKind == JsonValueKind.String)
                        {
                            var s = item.GetString();
                            if (!string.IsNullOrWhiteSpace(s)) list.Add(s.Trim());
                        }
                    list = list.Distinct(StringComparer.OrdinalIgnoreCase).ToList();
                }

                var exists = Has(name);
                var path = exists ? $"/fields/{id}/{locale}" : $"/fields/{id}";
                object payload = exists
                    ? (object)list
                    : new Dictionary<string, object?> { [locale] = list };

                ops.Add(new { op = exists ? "replace" : "add", path, value = payload });
            }
            // --------- end helpers ----------

            // 2) Build ops theo body (truyền gì update nấy)
            PatchString("name");
            PatchNumber("designRequestServiceFee", e => e.GetDouble());
            PatchNumber("depositRate", e => e.GetDouble());
            PatchNumber("presetVersions", e => e.GetDouble());
            PatchNumber("warrantyTime", e => e.GetDouble());
            PatchNumber("appointmentSlotInterval", e => e.GetDouble());
            PatchNumber("maxAppointmentPerDay", e => e.GetDouble());
            PatchNumber("maxAppointmentPerUser", e => e.GetDouble());
            PatchNumber("warrantyPeriod", e => e.GetDouble());
            PatchStringArray("colors");
            PatchStringArray("sizes");
            PatchStringArray("jobTitles");

            if (ops.Count == 0) return true;

            // 3) PATCH (CMA yêu cầu vnd.contentful…)
            var opsJson = JsonSerializer.Serialize(ops, JsonOpts);

            var req = new HttpRequestMessage(HttpMethod.Patch, EntryUrl)
            {
                Content = new StringContent(opsJson, Encoding.UTF8)
            };
            req.Content.Headers.ContentType =
                new MediaTypeHeaderValue("application/json-patch+json");
            req.Headers.Accept.Clear();
            req.Headers.Authorization =
                new AuthenticationHeaderValue("Bearer", _contentfulSettings.ManagementToken);
            req.Headers.Add("X-Contentful-Version", version.ToString());
            req.Headers.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/vnd.contentful.management.v1+json"));

            var patchRes = await _httpClient.SendAsync(req);
            patchRes.EnsureSuccessStatusCode(); // nếu 4xx/5xx -> throw ngay với message từ CMA

            // 4) Lấy version mới từ PATCH rồi publish
            var patchBody = await patchRes.Content.ReadAsStringAsync();
            using var patchDoc = JsonDocument.Parse(patchBody);
            if (!patchDoc.RootElement.TryGetProperty("sys", out var sys) ||
                !sys.TryGetProperty("version", out var verEl))
                throw new InvalidOperationException($"PATCH ok nhưng thiếu sys.version: {patchBody}");

            var newVersion = verEl.GetInt32();
            await PublishAsync(newVersion); // nhớ EnsureSuccessStatusCode trong PublishAsync

            // 5) Clear cache
            await _cacheService.RemoveAsync("cms:service:base");
            return true;
        }

        private static readonly IReadOnlyDictionary<string, string> FieldIdMap =
            new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                ["name"] = "name",
                ["designRequestServiceFee"] = "designRequestServiceFee",
                ["depositRate"] = "depositRate",
                ["presetVersions"] = "presetVersions",
                ["warrantyTime"] = "warrantyTime",
                ["appointmentSlotInterval"] = "appointmentSlotInterval",
                ["maxAppointmentPerDay"] = "maxAppointmentPerDay",
                ["maxAppointmentPerUser"] = "maxAppointmentPerUser",
                ["warrantyPeriod"] = "warrantyPeriod",
                ["colors"] = "colors",
                ["sizes"] = "sizes",
                ["jobTitles"] = "jobTitles"
            };
        private static string CmsId(string name)
            => FieldIdMap.TryGetValue(name, out var id) ? id : name;
        private static string DetectLocaleFromFields(JsonElement fieldsEl, string fallback = "en-US")
        {
            if (fieldsEl.ValueKind == JsonValueKind.Object)
            {
                foreach (var field in fieldsEl.EnumerateObject())
                {
                    if (field.Value.ValueKind != JsonValueKind.Object) continue;
                    foreach (var loc in field.Value.EnumerateObject())
                        return loc.Name;
                }
            }
            return fallback;
        }

    }
}
