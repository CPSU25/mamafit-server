namespace MamaFit.Services.ExternalService.AI.Interface;

public interface ILLMProvider
{
    Task<string> GenerateResponseAsync(string prompt);
    Task<bool> IsAvailable();
    string GetProviderName();
}