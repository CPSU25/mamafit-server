using MamaFit.BusinessObjects.DTO.WarrantyRequestItemDto;

namespace MamaFit.BusinessObjects.DTO.WarrantyRequestDto
{
    public class WarrantyGetByIdResponseDto : WarrantyRequestGetAllDto
    {
        public string? OrderStatus { get; set; }
        public string? PickAddressId { get; set; }
        public List<WarrantyRequestItemDetailResponseDto>? Items { get; set; }
    }
}
