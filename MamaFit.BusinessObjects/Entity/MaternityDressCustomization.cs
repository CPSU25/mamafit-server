using MamaFit.BusinessObjects.Base;
using MamaFit.BusinessObjects.Enum;

namespace MamaFit.BusinessObjects.Entity
{
    public class MaternityDressCustomization : BaseEntity
    {
        public string? UserId { get; set; }
        public string? OrderItemId { get; set; }
        public CustomizationEnum? CustomizationType { get; set; }

        //Navigation properties
        public virtual ICollection<MaternityDressSelection> MaternityDressSelections { get; set; } = new List<MaternityDressSelection>();
        public virtual MaternityDressTask MaternityDressTask { get; set; }
        public virtual ApplicationUser? Users { get; set; }
        public virtual OrderItem? OrderItems { get; set; }
    }
}
