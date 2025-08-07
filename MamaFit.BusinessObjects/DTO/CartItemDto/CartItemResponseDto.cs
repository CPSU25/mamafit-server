using MamaFit.BusinessObjects.DTO.MaternityDressDetailDto;
using MamaFit.BusinessObjects.DTO.PresetDto;

namespace MamaFit.BusinessObjects.DTO.CartItemDto;

public class CartItemResponseDto : CartItemRequestDto
{
    public PresetGetAllResponseDto? Preset { get; set; }
    public MaternityDressDetailResponseDto? MaternityDressDetail { get; set; }
}