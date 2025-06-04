using MamaFit.BusinessObjects.DTO.StyleDto;

namespace MamaFit.BusinessObjects.DTO.CategoryDto
{
    public class CategoryResponseDto
    {
        public string? Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public List<string> Images { get; set; } = [];
        public string? CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
