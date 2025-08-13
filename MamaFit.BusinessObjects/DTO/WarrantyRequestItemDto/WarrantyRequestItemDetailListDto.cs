using MamaFit.BusinessObjects.DTO.OrderDto;

namespace MamaFit.BusinessObjects.DTO.WarrantyRequestItemDto
{
    public class WarrantyRequestItemDetailListDto
    {
        public WarrantyRequestItemOrderResponseDto? WarrantyRequestItems { get; set; }
        public OrderGetByIdResponseDto? Order { get; set; }
    }
}
