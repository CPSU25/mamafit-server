using MamaFit.BusinessObjects.DTO.OrderDto;

namespace MamaFit.BusinessObjects.DTO.DesignRequestDto
{
    public class DesignRequestOrderResponseDto : DesignResponseDto
    {
        public List<OrderGetByIdResponseDto>? Order { get; set; }
    }
}
