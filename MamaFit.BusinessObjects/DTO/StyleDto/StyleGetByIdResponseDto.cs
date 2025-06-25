using MamaFit.BusinessObjects.DTO.ComponentDto;
using MamaFit.BusinessObjects.Enum;

namespace MamaFit.BusinessObjects.DTO.StyleDto
{
    public class StyleGetByIdResponseDto
    {
        public string? Id { get; set; }
        public string? Name { get; set; }
        public bool? IsCustom { get; set; }
        public GlobalStatus GlobalStatus { get; set; }
        public string? Description { get; set; }
        public List<string>? Images { get; set; } = [];
        public List<ComponentResponseDto>? Components { get; set; } = [];
        public string? CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
