namespace MamaFit.BusinessObjects.DTO.DesignRequestDto
{
    public class DesignRequestCreateDto
    {
        public string? UserId { get; set; }
        public string? OrderItemId { get; set; }
        public string? Description { get; set; }
        public List<string>? Images { get; set; } = new List<string>();
    }
}
