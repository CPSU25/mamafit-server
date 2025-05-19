using MamaFit.BusinessObjects.Enum;

namespace MamaFit.BusinessObjects.Entity
{
    public class Location : BaseEntity
    {
        public string? MapId { get; set; }
        public string? Name { get; set; }
        public string? Address { get; set; }
        public float? Latitude { get; set; }
        public float? Longitude { get; set; }
        public LocationType? Type { get; set; }
    }
}
