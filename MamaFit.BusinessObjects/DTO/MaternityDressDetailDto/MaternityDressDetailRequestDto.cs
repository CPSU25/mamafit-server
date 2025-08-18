namespace MamaFit.BusinessObjects.DTO.MaternityDressDetailDto
{
    public class MaternityDressDetailRequestDto
    {
        public string? MaternityDressId { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Color { get; set; }
        public List<string>? Image { get; set; }
        public string? Size { get; set; }
        public float Price { get; set; }
        public int Quantity { get; set; }
    }
}
