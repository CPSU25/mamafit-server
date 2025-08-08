using MamaFit.BusinessObjects.DTO.OrderDto;

namespace MamaFit.BusinessObjects.DTO.WarrantyRequestItemDto;

public class WarrantyOrderWithOrderItemDto
{
    public OrderResponseDto Order { get; set; } = new();
}