using Newtonsoft.Json;

namespace MamaFit.BusinessObjects.DTO.GhtkDto;

public class GhtkProductDto
{
    [JsonProperty("name")]
    public string Name { get; set; }

    [JsonProperty("weight")]
    public int Weight { get; set; } 

    [JsonProperty("quantity")]
    public int Quantity { get; set; }
}