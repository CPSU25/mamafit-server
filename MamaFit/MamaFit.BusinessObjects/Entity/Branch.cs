namespace MamaFit.BusinessObjects.Entity
{
    public class Branch : BaseEntity
    {
        public string? BranchManagerId { get; set; }
        public ApplicationUser? BranchManager { get; set; }
        public string? LocationId { get; set; }
        public Location? Location { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? OpeningHour { get; set; }
        public List<string> Images { get; set; } = new List<string>();
        public virtual ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
    }
}
