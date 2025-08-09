using MamaFit.BusinessObjects.DTO.OrderDto;

namespace MamaFit.BusinessObjects.DTO.WarrantyRequestDto
{
    public class WarrantyDetailResponseDto
    {
        public WarrantyRequestGetAllDto? WarrantyRequest { get; set; }
        public List<OrderWarrantyOnlyCode>? OriginalOrders { get; set; }
    }
}
