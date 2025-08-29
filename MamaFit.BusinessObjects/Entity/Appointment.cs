using MamaFit.BusinessObjects.Base;
using MamaFit.BusinessObjects.Enum;

namespace MamaFit.BusinessObjects.Entity
{
    public class Appointment : BaseEntity
    {
        public string? UserId { get; set; }
        public string? BranchId { get; set; }
        public DateTime BookingTime { get; set; }
        public string? Note { get; set; }
        public AppointmentStatus? Status { get; set; }
        public DateTime? CanceledAt { get; set; }
        public string? CanceledReason { get; set; }
        //CronJob
        public string? ReminderJobId { get; set; }
        public DateTime? Reminder30SentAt { get; set; } 
        
        //Navigation properties
        public ApplicationUser? User { get; set; }
        public Branch? Branch { get; set; }
    }
}
