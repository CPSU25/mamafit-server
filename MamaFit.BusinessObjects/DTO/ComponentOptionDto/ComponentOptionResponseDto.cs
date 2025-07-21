using MamaFit.BusinessObjects.Entity;
using MamaFit.BusinessObjects.Enum;

namespace MamaFit.BusinessObjects.DTO.ComponentOptionDto
{
    public class ComponentOptionResponseDto
    {
        public string? Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? ComponentId { get; set; }
        public string? ComponentName { get; set; }
        public float Price { get; set; }
        public Tag? Tag { get; set; }
        public List<string>? Images { get; set; } = [];
        public string? CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
