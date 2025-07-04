using Newtonsoft.Json;

namespace MamaFit.BusinessObjects.DTO.GhtkDto.GhtkAddressLevel4Response;

public class GhtkAddressLevel4Response
{
    [JsonProperty("success")]
    public bool Success { get; set; }

    [JsonProperty("data")]
    public List<string> Data { get; set; }
}