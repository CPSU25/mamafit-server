namespace MamaFit.BusinessObjects.DTO.GhtkDto.Fee;

public class GhtkFeeRequestDto
{
    public string Province { get; set; }
    public string District { get; set; }
    public int Weight { get; set; }
    public string? DeliveryOption { get; set; }
    public string? Transport { get; set; }
}