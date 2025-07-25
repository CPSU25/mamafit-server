using MamaFit.BusinessObjects.DTO.BranchDto;
using MamaFit.BusinessObjects.DTO.UserDto;
using MamaFit.BusinessObjects.Entity;
using MamaFit.BusinessObjects.Enum;

namespace MamaFit.BusinessObjects.DTO.AppointmentDto
{
    public class AppointmentResponseDto
    {
        public string Id { get; set; } = string.Empty;
        public UserReponseDto? User { get; set; }
        public BranchResponseDto? Branch { get; set; }
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
