namespace MamaFit.BusinessObjects.DTO.MaternityDressSelectionDto
{
    public class SelectionResponseDto : SelectionCreateRequestDto
    {
        public string? Id { get; set; }
        public float Price { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string? UpdatedBy { get; set; }
    }
}
