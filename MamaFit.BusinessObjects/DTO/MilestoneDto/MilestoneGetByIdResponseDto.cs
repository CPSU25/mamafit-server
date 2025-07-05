using MamaFit.BusinessObjects.DTO.MaternityDressTaskDto;

namespace MamaFit.BusinessObjects.DTO.MilestoneDto
{
    public class MilestoneGetByIdResponseDto :MilestoneResponseDto
    {
        public List<MaternityDressTaskDetailResponseDto>? Tasks { get; set; }
    }
}
