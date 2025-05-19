using MamaFit.BusinessObjects.Enum;

namespace MamaFit.BusinessObjects.Entity
{
    public class DesignOrder : BaseEntity
    {
        public string? UserId { get; set; }
        public ApplicationUser? User { get; set; }
        public string? DesignerId { get; set; }
        public ApplicationUser? Designer { get; set; }
        public string? Description { get; set; }
        public List<string>? Images { get; set; } = new List<string>();
    }
}
