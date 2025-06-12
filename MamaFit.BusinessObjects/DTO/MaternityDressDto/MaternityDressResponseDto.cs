using MamaFit.BusinessObjects.DTO.MaternityDressDetailDto;
using MamaFit.BusinessObjects.Entity;

namespace MamaFit.BusinessObjects.DTO.MaternityDressDto
{
    public class MaternityDressResponseDto
    {
        public string? Id { get; set; }
        public string? StyleName { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public List<string> Images { get; set; } = new List<string>();
        public string? Slug { get; set; }
        public string? CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public List<MaternityDressDetailResponseDto> Details { get; set; } = new List<MaternityDressDetailResponseDto>();
    }
}
