using MamaFit.BusinessObjects.Base;
using MamaFit.BusinessObjects.Enum;

namespace MamaFit.BusinessObjects.Entity
{
    public class Appointment : BaseEntity
    {
        public string? UserId { get; set; }
        public string? StaffId { get; set; }
        public string? BranchId { get; set; }
        public Branch? Branch { get; set; }
        public string? FullName { get; set; } = null!;
        public string? PhoneNumber { get; set; } = null!;
        public DateTime BookingTime { get; set; }
        public string? Note { get; set; }
        public AppointmentStatus? Status { get; set; }
        public DateTime? CanceledAt { get; set; }
        public string? CanceledReason { get; set; }
        
        //Navigation properties
        public ApplicationUser? User { get; set; }
        public ApplicationUser? Staff { get; set; }
    }
}
