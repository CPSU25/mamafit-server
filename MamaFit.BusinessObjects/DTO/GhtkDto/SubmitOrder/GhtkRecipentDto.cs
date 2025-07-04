namespace MamaFit.BusinessObjects.DTO.GhtkDto.Response;

public class GhtkRecipentDto
{
    public string CustomerPhone { get; set; }
    public string CustomerName { get; set; }
    public string CustomerAddress { get; set; }
    public string Province { get; set; }
    public string District { get; set; }
    public string Ward { get; set; }
    public string IsFreeship { get; set; }
    public string? Note { get; set; }
}