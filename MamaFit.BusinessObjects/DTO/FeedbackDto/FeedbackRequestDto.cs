namespace MamaFit.BusinessObjects.DTO.FeedbackDto;

public class FeedbackRequestDto
{
    public string? OrderItemId { get; set; }
    public string? Description { get; set; }
    public List<string>? Images { get; set; } = new List<string>();
    public float? Rated { get; set; }
}