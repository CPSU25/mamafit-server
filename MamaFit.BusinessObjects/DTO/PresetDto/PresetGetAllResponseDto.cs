namespace MamaFit.BusinessObjects.DTO.PresetDto
{
    public class PresetGetAllResponseDto : PresetBaseRequestDto
    {
        public string Id { get; set; } 
        public string? StyleId { get; set; }
        public string? StyleName { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; } = string.Empty;
        public DateTime UpdatedAt { get; set; }
        public string UpdatedBy { get; set; } = string.Empty;
    }
}
