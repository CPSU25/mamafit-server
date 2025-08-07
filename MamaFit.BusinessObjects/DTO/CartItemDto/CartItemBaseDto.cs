using MamaFit.BusinessObjects.Enum;

namespace MamaFit.BusinessObjects.DTO.CartItemDto;

public class CartItemBaseDto
{
    public int Quantity { get; set; }
    public ItemType Type { get; set; }
    public string? ItemId { get; set; }
}