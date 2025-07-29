using MamaFit.BusinessObjects.DTO.MaternityDressTaskDto;

namespace MamaFit.BusinessObjects.DTO.MilestoneDto
{
    public class MilestoneAchiveOrderItemResponseDto
    {
        public string MilestoneId { get; set; } = string.Empty;
        public string MilestoneName { get; set; } = string.Empty;
        public float Progress { get; set; } = 0.0f;
        public MaternityDressTaskForMilestoneAchiveResponseDto CurrentTask { get; set; } = new();
    }
}
