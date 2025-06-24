namespace MamaFit.BusinessObjects.DTO.AddressDto;

public class AddressRequestDto
{
    public string? UserId { get; set; } 
    public string? MapId { get; set; }
    public string? ShortName { get; set; }
    public string? LongName { get; set; }
    public float? Latitude { get; set; }
    public float? Longitude { get; set; }
}