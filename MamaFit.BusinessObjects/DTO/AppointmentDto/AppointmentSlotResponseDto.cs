namespace MamaFit.BusinessObjects.DTO.AppointmentDto
{
    public class AppointmentSlotResponseDto
    {
        public List<TimeOnly> Slot { get; set; } = new List<TimeOnly>(2);
        public bool IsBooked { get; set; }
    }
}
