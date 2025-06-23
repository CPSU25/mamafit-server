namespace MamaFit.BusinessObjects.DTO.TransactionDto;

public class TransactionResponseDto : TransactionBaseDto
{
    public string Id { get; set; }
    public string? CreatedBy { get; set; }
    public string? UpdatedBy { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}