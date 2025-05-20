using MamaFit.BusinessObjects.Base;
using MamaFit.BusinessObjects.Enum;

namespace MamaFit.BusinessObjects.Entity
{
    public class MaternityDressCustomization : BaseEntity
    {
        public string? UserId { get; set; }
        public string? DesignerId { get; set; }
        public CustomizationEnum? CustomizationType { get; set; }
        
        //Navigation property
        public virtual ICollection<MaternityDressSelection> MaternityDressSelections { get; set; } = new List<MaternityDressSelection>();
        public ApplicationUser? Designers { get; set; }
        public ApplicationUser? Users { get; set; }
        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    }
}
