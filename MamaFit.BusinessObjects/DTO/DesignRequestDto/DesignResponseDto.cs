using MamaFit.BusinessObjects.DTO.OrderItemDto;

namespace MamaFit.BusinessObjects.DTO.DesignRequestDto
{
    public class DesignResponseDto
    {
        public string? Id { get; set; }
        public string? UserId { get; set; }
        public string? Username { get; set; }
        public string? Description { get; set; }
        public List<string>? Images { get; set; } = new List<string>();
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string? CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }
        
        public OrderItemResponseDto? OrderItem { get; set; }
    }
}
