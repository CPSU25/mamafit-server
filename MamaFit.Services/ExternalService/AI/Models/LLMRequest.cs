using Newtonsoft.Json;

namespace MamaFit.Services.ExternalService.AI.Models;

public class LLMRequest
{
    [JsonProperty("model")]
    public string Model { get; set; }

    [JsonProperty("messages")]
    public List<LLMMessage> Messages { get; set; }

    [JsonProperty("temperature")]
    public float Temperature { get; set; }

    [JsonProperty("max_tokens")]
    public int MaxTokens { get; set; }
}