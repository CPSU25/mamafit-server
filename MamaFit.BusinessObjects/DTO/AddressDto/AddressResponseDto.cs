namespace MamaFit.BusinessObjects.DTO.AddressDto;

public class AddressResponseDto
{
    public string Id { get; set; }
    public string? UserId { get; set; } 
    public string? MapId { get; set; }
    public string? ShortName { get; set; }
    public string? LongName { get; set; }
    public float? Latitude { get; set; }
    public float? Longitude { get; set; }
    public string? CreatedBy { get; set; }
    public string? UpdatedBy { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}