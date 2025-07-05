namespace MamaFit.BusinessObjects.DTO.PresetDto
{
    public class PresetCreateRequestDto : PresetBaseRequestDto
    {
        public string? StyleId { get; set; }
        public string? DesignRequestId { get; set; }
        public List<string>? ComponentOptionIds { get; set; }
    }
}
