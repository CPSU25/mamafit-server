namespace MamaFit.BusinessObjects.DTO.StyleDto
{
    public class StyleAssignComponentRequestDto
    {
        public string StyleId { get; set; } = string.Empty;
        public List<string> ComponentIds { get; set; } = [];
    }
}
