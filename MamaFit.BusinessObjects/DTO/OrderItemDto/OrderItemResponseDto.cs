using MamaFit.BusinessObjects.DTO.DesignRequestDto;
using MamaFit.BusinessObjects.DTO.MaternityDressDetailDto;
using MamaFit.BusinessObjects.DTO.PresetDto;

namespace MamaFit.BusinessObjects.DTO.OrderItemDto;

public class OrderItemResponseDto : OrderItemBaseDto
{
    public string Id { get; set; }
    public string? ParentOrderItemId { get; set; }
    public string? CreatedBy { get; set; }
    public string? UpdatedBy { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public MaternityDressDetailResponseDto MaternityDressDetail { get; set; }
    public PresetGetAllResponseDto Preset { get; set; }
    public DesignResponseDto DesignRequest { get; set; }
    public List<AddOnOptionDto.AddOnOptionDto>? AddOnOptions { get; set; }
}