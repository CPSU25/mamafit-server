namespace MamaFit.BusinessObjects.DTO.GhtkDto.SubmitOrder;

public class GhtkCreateAndCancelResult
{
    public object? CreateOrder { get; set; }
    public object? CancelOrder { get; set; }
    public bool Success { get; set; }
    public string? Message { get; set; }
}
