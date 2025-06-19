namespace MamaFit.BusinessObjects.DTO.OrderItemDto;

public class OrderItemResponseDto : OrderItemBaseDto
{
    public string Id { get; set; }
    public string? CreatedBy { get; set; }
    public string? UpdatedBy { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}