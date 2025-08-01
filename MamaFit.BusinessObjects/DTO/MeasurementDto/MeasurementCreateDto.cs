namespace MamaFit.BusinessObjects.DTO.MeasurementDto;

public class MeasurementCreateDto
{
    public string MeasurementId { get; set; } = string.Empty;
    public float Weight { get; set; }
    public float Bust { get; set; }
    public float Waist { get; set; }
    public float Hip { get; set; }
}