using MamaFit.BusinessObjects.Base;

namespace MamaFit.BusinessObjects.Entity
{
    public class MaternityDressDetail : BaseEntity
    {
        public string? MaternityDressId { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Image { get; set; }
        public decimal Weight { get; set; }
        public string? Color { get; set; }
        public string? Size { get; set; }
        public float Price { get; set; }
        public int Quantity { get; set; }
        
        // Navigation properties
        public MaternityDress? MaternityDress { get; set; }
        public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
        public virtual ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();
        public virtual ICollection<BranchMaternityDressDetail> BranchMaternityDressDetails { get; set; } = [];
    }
}
