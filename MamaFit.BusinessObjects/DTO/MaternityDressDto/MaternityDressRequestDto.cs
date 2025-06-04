using MamaFit.BusinessObjects.DTO.MaternityDressDetailDto;

namespace MamaFit.BusinessObjects.DTO.MaternityDressDto
{
    public class MaternityDressRequestDto
    {
        public string? StyleId { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public List<string> Images { get; set; } = new List<string>();
        public string? Slug { get; set; }
    }
}
