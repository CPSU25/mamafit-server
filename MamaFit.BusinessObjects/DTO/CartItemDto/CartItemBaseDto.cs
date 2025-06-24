namespace MamaFit.BusinessObjects.DTO.CartItemDto;

public class CartItemBaseDto
{
    public string? UserId { get; set; }
    public int Quantity { get; set; }
    public float TotalAmount { get; set; }
    public string? MaternityDressDetailId { get; set; }
}