using MamaFit.BusinessObjects.DTO.OrderDto;

namespace MamaFit.BusinessObjects.DTO.WarrantyRequestDto
{
    public class WarrantyDetailResponseDto : WarrantyRequestGetAllDto
    {
        public OrderWarrantyOnlyCode? OriginalOrder { get; set; }
    }
}
