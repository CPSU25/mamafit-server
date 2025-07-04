using Newtonsoft.Json;

namespace MamaFit.BusinessObjects.DTO.GhtkDto;

public class GhtkOrderExpressRequest
{
    [JsonProperty("products")]
    public List<GhtkProductDto> Products { get; set; } = new List<GhtkProductDto>();
    [JsonProperty("order")]
    public GhtkOrderExpressInfo Order { get; set; } = new GhtkOrderExpressInfo();
}