using System.Text.Json.Serialization;

namespace MamaFit.BusinessObjects.DTO.SepayDto;

public class SepayWebhookPayload
{
    public int id { get; set; }
    public string gateway { get; set; }
    public string transactionDate { get; set; }
    public string? accountNumber { get; set; }
    public string? code { get; set; }
    public string content { get; set; }
    public string transferType { get; set; }
    public float transferAmount { get; set; }
    public long accumulated { get; set; }
    public string? subAccount { get; set; }
    public string referenceCode { get; set; }
    public string description { get; set; }
}