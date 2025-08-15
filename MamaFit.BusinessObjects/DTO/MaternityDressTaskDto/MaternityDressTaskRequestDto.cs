using MamaFit.BusinessObjects.Enum;

namespace MamaFit.BusinessObjects.DTO.MaternityDressTaskDto
{
    public class MaternityDressTaskRequestDto
    {
        public string? MilestoneId { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public int SequenceOrder { get; set; }
        public int EstimateTimeSpan { get; set; }
    }
}
