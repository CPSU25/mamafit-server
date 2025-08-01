namespace MamaFit.BusinessObjects.DTO.AI;

public class MeasurementPredictionRequestDto
{
    public string UserId { get; set; }
    public string MeasurementDiaryId { get; set; }
        
    // Basic Info
    public int Age { get; set; }
    public float Height { get; set; }
    public float PrePregnancyWeight { get; set; }
        
    // Current Measurements (từ measurement gần nhất)
    public float CurrentWeight { get; set; }
    public float CurrentNeck { get; set; }
    public float CurrentCoat { get; set; }
    public float CurrentBust { get; set; }
    public float CurrentChestAround { get; set; }
    public float CurrentStomach { get; set; }
    public float CurrentPantsWaist { get; set; }
    public float CurrentThigh { get; set; }
    public float CurrentDressLength { get; set; }
    public float CurrentSleeveLength { get; set; }
    public float CurrentShoulderWidth { get; set; }
    public float CurrentWaist { get; set; }
    public float CurrentLegLength { get; set; }
    public float CurrentHip { get; set; }
        
    // Time Info
    public int CurrentWeek { get; set; }
    public int TargetWeek { get; set; }
        
    // Options
    public bool UseAIEnhancement { get; set; } = true;
}