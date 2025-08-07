using MamaFit.BusinessObjects.DTO.MaternityDressDetailDto;
using MamaFit.BusinessObjects.DTO.PresetDto;
using MamaFit.BusinessObjects.Enum;

namespace MamaFit.BusinessObjects.DTO.CartItemDto
{
    public class CartItem
    {
        public string? ItemId { get; set; }
        public int Quantity { get; set; }
        public ItemType Type { get; set; }

        public PresetGetAllResponseDto? Preset { get; set; }
        public MaternityDressDetailResponseDto? MaternityDressDetail { get; set; }
    }
}
