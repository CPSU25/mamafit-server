namespace MamaFit.BusinessObjects.DTO.AI;

public class MeasurementPredictionRequestDto
{
    public string UserId { get; set; }
    public int Age { get; set; }
    public float Height { get; set; }
    public float PrePregnancyWeight { get; set; }
    public float CurrentWeight { get; set; }
    public float CurrentBust { get; set; }
    public float CurrentWaist { get; set; }
    public float CurrentHip { get; set; }
    public int CurrentWeek { get; set; }
    public int TargetWeek { get; set; }
    public bool IncludeExplanation { get; set; } = false;
}