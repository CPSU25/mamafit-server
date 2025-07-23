using MamaFit.BusinessObjects.Base;

namespace MamaFit.BusinessObjects.Entity
{
    public class Branch : BaseEntity
    {
        public string? BranchManagerId { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public TimeOnly? OpeningHour { get; set; }
        public TimeOnly? ClosingHour { get; set; }
        public List<string> Images { get; set; } = new List<string>();
        public string MapId { get; set; } = string.Empty!;
        public string? Province { get; set; }
        public string? District { get; set; }
        public string? Ward { get; set; }
        public string? Street { get; set; }
        public float? Latitude { get; set; }
        public float? Longitude { get; set; }

        // Navigation properties
        public ApplicationUser? BranchManager { get; set; }
        public virtual ICollection<Order>? Orders { get; set; } = new List<Order>();
        public virtual ICollection<Appointment>? Appointments { get; set; } = new List<Appointment>();
        public virtual ICollection<BranchMaternityDressDetail>? BranchMaternityDressDetail { get; set; } = [];
    }
}
