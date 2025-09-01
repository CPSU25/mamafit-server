using System;

namespace MamaFit.BusinessObjects.DTO.PresetDto;

public class PresetCreateForDesignRequestDto : PresetBaseRequestDto
{
    public string DesignRequestId { get; set; } = string.Empty;
    public string OrderId { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
}
