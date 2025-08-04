using Newtonsoft.Json;

namespace MamaFit.Services.ExternalService.AI.Models;

public class LLMMessage
{
    [JsonProperty("role")]
    public string Role { get; set; }

    [JsonProperty("content")]
    public string Content { get; set; }
}