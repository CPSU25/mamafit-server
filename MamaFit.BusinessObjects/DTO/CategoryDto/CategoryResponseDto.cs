using MamaFit.BusinessObjects.DTO.StyleDto;

namespace MamaFit.BusinessObjects.DTO.CategoryDto
{
    public class CategoryResponseDto
    {
        public string? Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public List<string> Images { get; set; } = [];
        public List<StyleResponseDto>? Styles { get; set; }
    }
}
