namespace MamaFit.BusinessObjects.DTO.StyleDto
{
    public class StyleRequestDto
    {
        public string? Name { get; set; }
        public bool? IsCustom { get; set; }
        public string? Description { get; set; }
        public List<string>? Images { get; set; } = [];
    }
}
