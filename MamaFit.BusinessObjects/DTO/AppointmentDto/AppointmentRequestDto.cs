using MamaFit.BusinessObjects.Enum;

namespace MamaFit.BusinessObjects.DTO.Appointment
{
    public class AppointmentRequestDto
    {
        public string? UserId { get; set; }
        public string? BranchId { get; set; }
        public string? FullName { get; set; } = null!;
        public string? PhoneNumber { get; set; } = null!;
        public DateTime BookingTime { get; set; }
        public string? Note { get; set; }
        public AppointmentStatus? Status { get; set; }
        public DateTime? CanceledAt { get; set; }
        public string? CanceledReason { get; set; }
    }
}
