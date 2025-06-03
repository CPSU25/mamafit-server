using MamaFit.BusinessObjects.Base;

namespace MamaFit.BusinessObjects.Entity
{
    public class DesignRequest : BaseEntity
    {
        public string? UserId { get; set; }
        public string? OrderItemId { get; set; }
        public string? Description { get; set; }
        public List<string>? Images { get; set; } = new List<string>();
        //Navigation properties
        public virtual OrderItem? OrderItem { get; set; }
        public virtual ApplicationUser? User { get; set; }
        public virtual MaternityDressTask? MaternityDressTask { get; set; }
    }
}
