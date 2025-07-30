using MamaFit.BusinessObjects.Enum;

namespace MamaFit.BusinessObjects.DTO.MaternityDressTaskDto
{
    public class MaternityDressTaskOrderTaskResponseDto : MaternityDressTaskResponseDto
    {
        public string? Image { get; set; }
        public string? Note { get; set; }
        public OrderItemTaskStatus? Status { get; set; }
    }
}
