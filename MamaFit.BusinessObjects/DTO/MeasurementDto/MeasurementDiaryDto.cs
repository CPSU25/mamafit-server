
using Swashbuckle.AspNetCore.Annotations;

namespace MamaFit.BusinessObjects.DTO.MeasurementDto;

public class MeasurementDiaryDto
{
    public string? UserId { get; set; }
    public string? Name { get; set; }
    public int? Age { get; set; }
    public float Height { get; set; }
    public float Weight { get; set; }
    public float Bust { get; set; }
    public float Waist { get; set; }
    public float Hip { get; set; }
    public DateTime? FirstDateOfLastPeriod { get; set; }
    public int? AverageMenstrualCycle { get; set; }
    public int? NumberOfPregnancy { get; set; }
    public DateTime? UltrasoundDate { get; set; }
    public int WeeksFromUltrasound { get; set; }
    [SwaggerSchema(ReadOnly = true)]
    public DateTime? PregnancyStartDate { get; set; }
    public bool IsActive { get; set; }
}
