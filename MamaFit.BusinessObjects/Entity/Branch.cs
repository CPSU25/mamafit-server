using MamaFit.BusinessObjects.Base;

namespace MamaFit.BusinessObjects.Entity
{
    public class Branch : BaseEntity
    {
        public string? BranchManagerId { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? OpeningHour { get; set; }
        public List<string> Images { get; set; } = new List<string>();
        public string? MapId { get; set; }
        public string? ShortName { get; set; }
        public string? LongName { get; set; }
        public float? Latitude { get; set; }
        public float? Longitude { get; set; }
        
        // Navigation properties
        public ApplicationUser? BranchManager { get; set; }
        public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
        public virtual ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
        public virtual ICollection<BranchMaternityDressDetail> BranchMaternityDressDetail { get; set; } = [];
    }
}
