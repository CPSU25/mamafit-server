using MamaFit.BusinessObjects.Enum;

namespace MamaFit.BusinessObjects.Entity
{
    public sealed class Branch : BaseEntity
    {
        public Guid BranchManagerId { get; set; }
        public ApplicationUser? BranchManager { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Address { get; set; }
        public string? ContactEmail { get; set; }
        public string? PhoneNumber { get; set; }
        public List<string>? Image { get; set; }
        public BranchStatus? Status { get; set; }
        public ICollection<Appointment>? Appointment { get; set; } = new List<Appointment>();
        public ICollection<Dress>? Dress { get; set; } = new List<Dress>();
    }
}
