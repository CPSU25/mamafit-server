using MamaFit.BusinessObjects.DTO.ComponentOptionDto;
using MamaFit.BusinessObjects.Enum;

namespace MamaFit.BusinessObjects.DTO.ComponentDto
{
    public class ComponentGetByIdResponseDto
    {
        public string? Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public List<string>? Images { get; set; } = [];
        public GlobalStatus GlobalStatus { get; set; }
        public List<ComponentOptionResponseDto> Options { get; set; } = [];
        public string? CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
