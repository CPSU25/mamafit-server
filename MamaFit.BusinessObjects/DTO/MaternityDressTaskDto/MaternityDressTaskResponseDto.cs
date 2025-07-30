using MamaFit.BusinessObjects.DTO.MilestoneDto;
using MamaFit.BusinessObjects.Enum;

namespace MamaFit.BusinessObjects.DTO.MaternityDressTaskDto
{
    public class MaternityDressTaskResponseDto
    {
        public string? Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public int SequenceOrder { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string? CreatedBy { get; set; } = string.Empty;
        public DateTime? UpdatedAt { get; set; } = null;
        public string? UpdatedBy { get; set; } = string.Empty;
        
        public MilestoneResponseDto? Milestone { get; set; }
    }
}
