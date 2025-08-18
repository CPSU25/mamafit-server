using MamaFit.BusinessObjects.Enum;

namespace MamaFit.BusinessObjects.DTO.TicketDto
{
    public class TicketBaseDto
    {
        public string? Title { get; set; }
        public List<string>? Images { get; set; }
        public List<string>? Videos { get; set; }
        public TicketType? Type { get; set; }
        public string? Description { get; set; }
        public string? Status { get; set; }
    }
}
