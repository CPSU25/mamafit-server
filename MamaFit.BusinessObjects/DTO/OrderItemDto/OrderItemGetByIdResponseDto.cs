using MamaFit.BusinessObjects.DTO.DesignRequestDto;
using MamaFit.BusinessObjects.DTO.MaternityDressDetailDto;

namespace MamaFit.BusinessObjects.DTO.OrderItemDto
{
    public class OrderItemGetByIdResponseDto : OrderItemResponseDto
    {
        public DesignResponseDto? Desisgn { get; set; }
        public MaternityDressDetailResponseDto? MaternityDressDetail { get; set; }
    }
}
