using MamaFit.BusinessObjects.Enum;

namespace MamaFit.BusinessObjects.DTO.AppointmentDto
{
    public class AppointmentRequestDto
    {
        public string? UserId { get; set; }
        public string? BranchId { get; set; }
        public DateTime BookingTime { get; set; }
        public string? Note { get; set; }
    }
}
