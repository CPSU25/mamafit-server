using MamaFit.BusinessObjects.DTO.StyleDto;

namespace MamaFit.BusinessObjects.DTO.CategoryDto
{
    public class CategoryRequestDto
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public List<string> Images { get; set; } = [];
    }
}
