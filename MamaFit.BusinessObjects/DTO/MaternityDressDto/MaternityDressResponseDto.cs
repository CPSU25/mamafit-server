using MamaFit.BusinessObjects.DTO.MaternityDressDetailDto;
using MamaFit.BusinessObjects.Entity;

namespace MamaFit.BusinessObjects.DTO.MaternityDressDto
{
    public class MaternityDressResponseDto
    {
        public string? Id { get; set; }
        public Style? Style { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public List<string> Images { get; set; } = new List<string>();
        public string? Slug { get; set; }
        public List<MaternityDressDetailResponseDto>? Details { get; set; }
    }
}
