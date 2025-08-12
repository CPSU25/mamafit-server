using MamaFit.BusinessObjects.DTO.OrderDto;

namespace MamaFit.BusinessObjects.DTO.WarrantyRequestItemDto
{
    public class WarrantyRequestItemDetailListDto
    {
        public WarrantyRequestItemGetAllDto? WarrantyRequestItems { get; set; }
        public OrderGetByIdResponseDto? Order { get; set; }
    }
}
