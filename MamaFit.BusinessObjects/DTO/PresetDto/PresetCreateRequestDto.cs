namespace MamaFit.BusinessObjects.DTO.PresetDto
{
    public class PresetCreateRequestDto : PresetBaseRequestDto
    {
        public string StyleId { get; set; }
        public string Name { get; set; } = string.Empty;
        public List<string> ComponentOptionIds { get; set; }
    }
}
