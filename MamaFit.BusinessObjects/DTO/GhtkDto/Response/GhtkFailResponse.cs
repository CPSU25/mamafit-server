using Newtonsoft.Json;

namespace MamaFit.BusinessObjects.DTO.GhtkDto.Response;

public class GhtkFailResponse : GhtkBaseResponse
{
    [JsonProperty("error_code")]
    public string? ErrorCode { get; set; }
}