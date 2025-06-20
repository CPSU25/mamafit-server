using MamaFit.BusinessObjects.Enum;

namespace MamaFit.BusinessObjects.DTO.MaternityDressTask
{
    public class MaternityDressTaskResponseDto : MaternityDressTaskRequestDto
    {
        public string? Id { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string? CreatedBy { get; set; } = string.Empty;
        public DateTime? UpdatedAt { get; set; } = null;
        public string? UpdatedBy { get; set; } = string.Empty;
    }
}
