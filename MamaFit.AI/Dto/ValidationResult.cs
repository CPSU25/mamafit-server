namespace MamaFit.AI.Dto;

public class ValidationResult
{
    public bool IsValid { get; set; }
    public Dictionary<string, float> Accuracies { get; set; } = new();
    public List<string> Issues { get; set; } = new();
}