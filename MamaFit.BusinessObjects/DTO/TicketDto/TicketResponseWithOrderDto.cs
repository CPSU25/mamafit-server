using MamaFit.BusinessObjects.DTO.OrderDto;

namespace MamaFit.BusinessObjects.DTO.TicketDto
{
    public class TicketResponseWithOrderDto : TicketBaseResponseDto
    {
        public OrderGetByIdResponseDto? Order { get; set; }
    }
}
