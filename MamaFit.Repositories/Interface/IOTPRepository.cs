using MamaFit.BusinessObjects.Entity;
using MamaFit.BusinessObjects.Enum;
using MamaFit.Repositories.Implement;

namespace MamaFit.Repositories.Interface;

public interface IOTPRepository : IGenericRepository<OTP>
{
    Task<OTP?> GetOTPAsync(string userId, string otp , OTPType? otpType);
    Task CreateOTPAsync(OTP otp);
    Task DeleteOTPAsync(OTP otp);
}