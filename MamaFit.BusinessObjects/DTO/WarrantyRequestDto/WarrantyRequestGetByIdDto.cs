using MamaFit.BusinessObjects.DTO.OrderItemDto;

namespace MamaFit.BusinessObjects.DTO.WarrantyRequestDto
{
    public class WarrantyRequestGetByIdDto : WarrantyRequestGetAllDto
    {
        public OrderItemBaseDto? WarrantyOrderItem { get; set; }
        public OrderItemBaseDto? OriginalOrderItem { get; set; }
    }
}