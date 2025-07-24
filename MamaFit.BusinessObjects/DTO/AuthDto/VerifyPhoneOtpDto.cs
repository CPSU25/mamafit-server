namespace MamaFit.BusinessObjects.DTO.AuthDto;

public class VerifyPhoneOtpDto
{
    public string PhoneNumber { get; set; } = string.Empty;
    public string OtpCode { get; set; } = string.Empty;
}