namespace MamaFit.BusinessObjects.DTO.MaternityDressDetailDto
{
    public class MaternityDressDetailResponseDto
    {
        public string? Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Color { get; set; }
        public string? Image { get; set; }
        public string? Size { get; set; }
        public float Price { get; set; }
        public int Quantity { get; set; }
        public string? CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
