using MamaFit.BusinessObjects.Enum;
using MamaFit.BusinessObjects.Base;

namespace MamaFit.BusinessObjects.Entity
{
    public class Address : BaseEntity
    {
        public string? UserId { get; set; } 
        public string? MapId { get; set; }
        public string? ShortName { get; set; }
        public string? LongName { get; set; }
        public float? Latitude { get; set; }
        public float? Longitude { get; set; }
        
        // Navigation properties
        public ApplicationUser? User { get; set; }
    }
}
