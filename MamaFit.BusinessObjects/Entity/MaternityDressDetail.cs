using MamaFit.BusinessObjects.Base;
using MamaFit.BusinessObjects.DTO.CartItemDto;

namespace MamaFit.BusinessObjects.Entity
{
    public class MaternityDressDetail : BaseEntity
    {
        public string? MaternityDressId { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Image { get; set; }
        public int? Weight { get; set; }
        public string? Color { get; set; }
        public string? Size { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        
        // Navigation properties
        public MaternityDress? MaternityDress { get; set; }
        public virtual ICollection<OrderItem>? OrderItems { get; set; } = new List<OrderItem>();
        public virtual ICollection<BranchMaternityDressDetail>? BranchMaternityDressDetails { get; set; } = [];
    }
}
