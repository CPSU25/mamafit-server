using MamaFit.BusinessObjects.Enum;

namespace MamaFit.BusinessObjects.DTO.MaternityDressTask
{
    public class MaternityDressTaskRequestDto
    {
        public string? MilestoneId { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public int SequenceOrder { get; set; }
    }
}
