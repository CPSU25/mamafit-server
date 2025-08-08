using MamaFit.BusinessObjects.DTO.WarrantyRequestItemDto;

namespace MamaFit.BusinessObjects.DTO.WarrantyRequestDto
{
    public class WarrantyGetByIdResponseDto : WarrantyRequestGetAllDto
    {
        public List<WarrantyRequestItemDetailResponseDto>? Items { get; set; }
    }
}
