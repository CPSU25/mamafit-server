using MamaFit.BusinessObjects.DTO.AddOnDto;
namespace MamaFit.BusinessObjects.DTO.PresetDto
{
    public class PresetListIdRequestDto
    {
        public string? Id { get; set; }
        public List<AddOnOrderItemRequestDto>? Options { get; set; }
    }
}
