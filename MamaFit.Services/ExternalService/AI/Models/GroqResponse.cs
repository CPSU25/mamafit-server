namespace MamaFit.Services.ExternalService.AI.Models;

public class GroqResponse
{
    public string Id { get; set; }
    public List<GroqChoice> Choices { get; set; }
    public GroqUsage Usage { get; set; }
}