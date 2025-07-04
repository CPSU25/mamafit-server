namespace MamaFit.BusinessObjects.DTO.GhtkDto.Fee;

public class GhtkFeeRequestDto
{
    public string Province { get; set; }
    public string District { get; set; }
    public int Weight { get; set; }
    public string DeliverOption { get; set; }
    public string? Address { get; set; }
    public string? PickProvince { get; set; }
    public string? PickDistrict { get; set; }
    public string? PickAddressId { get; set; }
    public string? PickAddress { get; set; }
    public string? PickWard { get; set; }
    public string? PickStreet { get; set; }
    public string? Ward { get; set; }
    public string? Street { get; set; }
    public int? Value { get; set; }
    public string? Transport { get; set; }
    public string[]? Tags { get; set; }
}