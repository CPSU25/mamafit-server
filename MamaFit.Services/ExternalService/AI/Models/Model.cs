using Newtonsoft.Json;

namespace MamaFit.Services.ExternalService.AI.Models
{
    public class Model
    {
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

        public class LLMMessage
        {
            [JsonProperty("role")]
            public string Role { get; set; }

            [JsonProperty("content")]
            public string Content { get; set; }
        }

        public class GroqResponse
        {
            public string Id { get; set; }
            public List<GroqChoice> Choices { get; set; }
            public GroqUsage Usage { get; set; }
        }

        public class GroqChoice
        {
            public LLMMessage Message { get; set; }
            public string FinishReason { get; set; }
        }

        public class GroqUsage
        {
            public int PromptTokens { get; set; }
            public int CompletionTokens { get; set; }
            public int TotalTokens { get; set; }
        }

        public class OllamaRequest
        {
            public string Model { get; set; }
            public string Prompt { get; set; }
            public bool Stream { get; set; }
            public OllamaOptions Options { get; set; }
        }

        public class OllamaOptions
        {
            public float Temperature { get; set; }
            public int NumPredict { get; set; }
        }

        public class OllamaResponse
        {
            public string Model { get; set; }
            public string Response { get; set; }
            public bool Done { get; set; }
        }
        
        public class MeasurementFeedbackDto
        {
            public string MeasurementId { get; set; }
            public Dictionary<string, float> ActualMeasurements { get; set; }
            public string? UserComments { get; set; }
            public int AccuracyRating { get; set; } // 1-5
        }
    }
}