namespace MamaFit.BusinessObjects.DTO.StyleDto
{
    public class StyleResponseDto
    {
        public string? Id { get; set; }
        public string? Name { get; set; }
        public bool? IsCustom { get; set; }
        public string? Description { get; set; }
        public List<string>? Images { get; set; } = [];
    }
}
