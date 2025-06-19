using MamaFit.BusinessObjects.Enum;

namespace MamaFit.BusinessObjects.DTO.OrderItemDto;

public class OrderItemBaseDto
{
    public string? OrderId { get; set; }
    public string? MaternityDressDetailId { get; set; }
    public ItemType? ItemType { get; set; }
    public float Price { get; set; }
    public int Quantity { get; set; }
    public DateTime? WarrantyDate { get; set; }
    public int WarrantyNumber { get; set; } = 3;
}