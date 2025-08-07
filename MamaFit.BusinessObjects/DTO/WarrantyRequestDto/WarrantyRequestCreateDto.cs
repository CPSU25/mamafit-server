using MamaFit.BusinessObjects.DTO.WarrantyRequestItemDto;

namespace MamaFit.BusinessObjects.DTO.WarrantyRequestDto;

public class WarrantyRequestCreateDto
{
    public List<WarrantyRequestItemCreateDto> Items { get; set; } = new();
}