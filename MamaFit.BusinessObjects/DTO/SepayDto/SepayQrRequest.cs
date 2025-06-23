namespace MamaFit.BusinessObjects.DTO.SepayDto;

public class SepayQrRequest
{
    public string accountNo { get; set; }
    public string accountName { get; set; }
    public string acqId { get; set; }
    public float amount { get; set; }
    public string addInfo { get; set; }
    public string template { get; set; }
    public string orderCode { get; set; }
    public string url { get; set; }
}