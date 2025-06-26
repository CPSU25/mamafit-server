namespace MamaFit.BusinessObjects.DTO.PresetDto
{
    public class PresetCreateRequestDto : PresetBaseRequestDto
    {
        public List<string>? ComponentOptionIds { get; set; }
    }
}
