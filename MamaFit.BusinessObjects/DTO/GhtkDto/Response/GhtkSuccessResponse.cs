using Newtonsoft.Json;

namespace MamaFit.BusinessObjects.DTO.GhtkDto.Response;

public class GhtkSuccessResponse : GhtkBaseResponse
{
    [JsonProperty("data")]
    public object? Data { get; set; }
}