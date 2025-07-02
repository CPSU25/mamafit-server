
namespace MamaFit.BusinessObjects.DTO.WarrantyRequestDto
{
    public class WarrantyRequestCreateDto : WarrantyBaseDto
    {
        public string? OriginalOrderItemId { get; set; }
        public string? WarrantyOrderItemId { get; set; }
    }
}
