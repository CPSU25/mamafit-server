using MamaFit.BusinessObjects.DTO.MaternityDressTaskDto;

namespace MamaFit.BusinessObjects.DTO.MilestoneDto
{
    public class MilestoneAchiveOrderItemResponseDto
    {
        public MilestoneResponseDto? Milestone {  get; set; }
        public float Progress { get; set; } = 0.0f;
        public bool IsDone { get; set; } = false;
        public MaternityDressTaskForMilestoneAchiveResponseDto CurrentTask { get; set; } = new();
    }
}
