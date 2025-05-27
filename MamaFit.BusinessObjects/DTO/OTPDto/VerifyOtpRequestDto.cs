using MamaFit.BusinessObjects.Enum;

namespace MamaFit.BusinessObjects.DTO.OTPDto;

public class VerifyOtpRequestDto
{
    public string Email { get; set; }
    public string Code { get; set; }
    public OTPType OTPType { get; set; }
}