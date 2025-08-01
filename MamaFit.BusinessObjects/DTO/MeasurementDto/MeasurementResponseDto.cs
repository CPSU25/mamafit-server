namespace MamaFit.BusinessObjects.DTO.MeasurementDto;

public class MeasurementResponseDto
{
    public string Id { get; set; } = string.Empty;
    public int WeekOfPregnancy { get; set; }
    public float Weight { get; set; }
    public float Neck { get; set; }
    public float Coat { get; set; }
    public float Bust { get; set; }
    public float ChestAround { get; set; }
    public float Stomach { get; set; }
    public float PantsWaist { get; set; }
    public float Thigh { get; set; }
    public float DressLength { get; set; }
    public float SleeveLength { get; set; }
    public float ShoulderWidth { get; set; }
    public float Waist { get; set; }
    public float LegLength { get; set; }
    public float Hip { get; set; }
    public bool IsLocked { get; set; } = false;
    public string? CreatedBy { get; set; }
    public string? UpdatedBy { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}