using MamaFit.BusinessObjects.Entity;
using MamaFit.BusinessObjects.Enum;

namespace MamaFit.BusinessObjects.DTO.AppointmentDto
{
    public class AppointmentResponseDto
    {
        public ApplicationUser? User { get; set; }
        public ApplicationUser? Staff { get; set; }
        public Branch? Branch { get; set; }
        public string? FullName { get; set; } = null!;
        public string? PhoneNumber { get; set; } = null!;
        public DateTime BookingTime { get; set; }
        public string? Note { get; set; }
        public AppointmentStatus? Status { get; set; }
        public DateTime? CanceledAt { get; set; }
        public string? CanceledReason { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string? CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }
    }
}
