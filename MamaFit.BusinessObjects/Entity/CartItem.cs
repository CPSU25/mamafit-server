using MamaFit.BusinessObjects.Base;

namespace MamaFit.BusinessObjects.Entity
{
    public class CartItem : BaseEntity
    {
        public string? UserId { get; set; }
        public int Quantity { get; set; }
        public float TotalAmount { get; set; }
        public string? MaternityDressDetailId { get; set; }
        
        // Navigation properties
        public ApplicationUser? User { get; set; }
        public MaternityDressDetail? MaternityDressDetail { get; set; }
    }
}
