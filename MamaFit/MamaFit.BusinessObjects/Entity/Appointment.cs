namespace MamaFit.BusinessObjects.Entity
{
    public sealed class Appointment : BaseEntity
    {
        public string? ApointmentName { get; set; }
        public string? Description { get; set; }
        public DateTime ApointmentDate { get; set; }
        public int? Duration { get; set; }
        public Guid BranchId { get; set; }
        public Branch? Branch { get; set; }
        public Guid UserId { get; set; }
        public ApplicationUser? User { get; set; }
    }
}
