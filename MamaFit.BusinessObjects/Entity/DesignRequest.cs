using MamaFit.BusinessObjects.Base;using MamaFit.BusinessObjects.Enum;

namespace MamaFit.BusinessObjects.Entity
{
    public class DesignRequest : BaseEntity
    {
        public string? UserId { get; set; }
        public string? Description { get; set; }
        public List<string>? Images { get; set; } = new List<string>();
        
        //Navigation properties
        public OrderItem? OrderItem { get; set; }
        public ApplicationUser? User { get; set; }
        public ICollection<MaternityDressTask> MaternityDressTasks { get; set; } = new List<MaternityDressTask>();
    }
}
