using MamaFit.BusinessObjects.DTO.OrderDto;
using MamaFit.BusinessObjects.DTO.OrderItemDto;

namespace MamaFit.BusinessObjects.DTO.WarrantyRequestItemDto
{
    public class WarrantyRequestItemDetailDto
    {
        public WarrantyRequestItemGetAllDto? WarrantyRequestItems { get; set; }
        public OrderGetByIdResponseDto? ParentOrder { get; set; }
    }
}
