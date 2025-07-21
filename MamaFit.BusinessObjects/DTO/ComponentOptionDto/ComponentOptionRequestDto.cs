using MamaFit.BusinessObjects.Entity;
using MamaFit.BusinessObjects.Enum;

namespace MamaFit.BusinessObjects.DTO.ComponentOptionDto
{
    public class ComponentOptionRequestDto
    {
        public string? ComponentId { get; set; }
        public string? Name { get; set; }
        public float Price { get; set; }
        public string? Description { get; set; }
        public Tag? Tag { get; set; }
        public List<string>? Images { get; set; } = [];
    }
}
