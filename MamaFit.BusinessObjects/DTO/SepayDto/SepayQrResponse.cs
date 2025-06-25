using MamaFit.BusinessObjects.DTO.OrderDto;
using MamaFit.BusinessObjects.DTO.OrderItemDto;

namespace MamaFit.BusinessObjects.DTO.SepayDto;

public class SepayQrResponse
{
    public string QrUrl { get; set; }
    public OrderWithItemResponseDto OrderWithItem { get; set; }
}
