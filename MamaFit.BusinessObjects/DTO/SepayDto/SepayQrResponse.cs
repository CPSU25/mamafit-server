namespace MamaFit.BusinessObjects.DTO.SepayDto;

public class SepayQrResponse
{
    public string code { get; set; }
    public string desc { get; set; }
    public QrData data { get; set; }
}

public class QrData
{
    public string qrCode { get; set; }
    public string qrDataURL { get; set; }
}
