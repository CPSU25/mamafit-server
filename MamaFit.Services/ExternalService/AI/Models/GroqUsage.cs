namespace MamaFit.Services.ExternalService.AI.Models;

public class GroqUsage
{
    public int PromptTokens { get; set; }
    public int CompletionTokens { get; set; }
    public int TotalTokens { get; set; }
}

