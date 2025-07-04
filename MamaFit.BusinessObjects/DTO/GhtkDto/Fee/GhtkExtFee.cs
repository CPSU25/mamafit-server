using Newtonsoft.Json;

namespace MamaFit.BusinessObjects.DTO.GhtkDto.Fee;

public class GhtkExtFee
{
    [JsonProperty("display")]
    public string Display { get; set; }
    [JsonProperty("title")]
    public string Title { get; set; }
    [JsonProperty("amount")]
    public int Amount { get; set; }
    [JsonProperty("type")]
    public string Type { get; set; }
}