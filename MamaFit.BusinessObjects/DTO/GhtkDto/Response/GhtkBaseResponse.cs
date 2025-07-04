using Newtonsoft.Json;

namespace MamaFit.BusinessObjects.DTO.GhtkDto.Response;

public class GhtkBaseResponse
{
    [JsonProperty("success")]
    public bool Success { get; set; }

    [JsonProperty("message")]
    public string Message { get; set; }
}