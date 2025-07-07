namespace MamaFit.BusinessObjects.DTO.AddressDto;

public class AddressRequestDto
{
    public string? MapId { get; set; }
    public string? Province { get; set; }
    public string? District { get; set; }
    public string? Ward { get; set; }
    public string? Street { get; set; }
    public float? Latitude { get; set; }
    public float? Longitude { get; set; }
    public bool? IsDefault { get; set; }
}