using MamaFit.BusinessObjects.DTO.MaternityDressTaskDto;

namespace MamaFit.BusinessObjects.DTO.MilestoneDto
{
    public class MilestoneGetByIdOrderTaskResponseDto : MilestoneRequestDto
    {
        public List<MaternityDressTaskOrderTaskResponseDto>? MaternityDressTasks { get; set; }
    }
}
