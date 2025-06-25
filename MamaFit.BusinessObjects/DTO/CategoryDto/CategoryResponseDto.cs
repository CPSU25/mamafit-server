using MamaFit.BusinessObjects.Enum;

namespace MamaFit.BusinessObjects.DTO.CategoryDto
{
    public class CategoryResponseDto
    {
        public string? Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public GlobalStatus Status { get; set; }
        public List<string> Images { get; set; } = new List<string>();
        public string? CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
