namespace MamaFit.Services.ExternalService.AI.Models;

public class GroqChoice
{
    public LLMMessage Message { get; set; }
    public string FinishReason { get; set; }
}