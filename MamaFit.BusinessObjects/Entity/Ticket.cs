using MamaFit.BusinessObjects.Base;
using MamaFit.BusinessObjects.Enum;

namespace MamaFit.BusinessObjects.Entity
{
    public class Ticket : BaseEntity
    {
        public string? OrderItemId { get; set; }
        public string? Title { get; set; }
        public List<string>? Images { get; set; }
        public List<string>? Videos { get; set; }
        public TicketType? Type { get; set; }
        public string? Description { get; set; }
        public string? Status { get; set; }

        public OrderItem? OrderItem { get; set; }
    }
}
