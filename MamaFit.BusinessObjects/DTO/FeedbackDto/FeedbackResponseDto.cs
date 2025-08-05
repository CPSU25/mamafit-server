namespace MamaFit.BusinessObjects.DTO.FeedbackDto;

public class FeedbackResponseDto
{
    public string Id { get; set; }
    public string? UserId { get; set; }
    public string? OrderId { get; set; }
    public string? OrderCode { get; set; }
    public string? Description { get; set; }
    public List<string>? Images { get; set; } = new List<string>();
    public float? Rated { get; set; }
    public string? CreatedBy { get; set; }
    public string? UpdatedBy { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}