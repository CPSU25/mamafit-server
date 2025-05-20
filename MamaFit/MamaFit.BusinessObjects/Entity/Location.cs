using MamaFit.BusinessObjects.Enum;
using MamaFit.BusinessObjects.Base;

namespace MamaFit.BusinessObjects.Entity
{
    public class Location : BaseEntity
    {
        public string? UserId { get; set; }
        public ApplicationUser? User { get; set; }
        public string? MapId { get; set; }
        public string? Name { get; set; }
        public string? Address { get; set; }
        public float? Latitude { get; set; }
        public float? Longitude { get; set; }
        public LocationType? Type { get; set; }
    }
}
