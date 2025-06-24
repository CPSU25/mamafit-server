namespace MamaFit.BusinessObjects.DTO.TransactionDto;

public class TransactionBaseDto
{
    public string? OrderId { get; set; }
    public string? SepayId { get; set; }
    public string? Gateway { get; set; }
    public DateTime? TransactionDate { get; set; }
    public string? AccountNumber { get; set; }
    public string? Code { get; set; }
    public string? Content { get; set; }
    public string? TransferType { get; set; }
    public float? TransferAmount { get; set; }
    public string? Accumulated { get; set; }
    public string? SubAccount { get; set; }
    public string? ReferenceCode { get; set; }
    public string? Description { get; set; }
}