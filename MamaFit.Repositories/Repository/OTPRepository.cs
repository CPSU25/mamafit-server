using MamaFit.BusinessObjects.DBContext;
using MamaFit.BusinessObjects.Entity;
using MamaFit.BusinessObjects.Enum;
using MamaFit.Repositories.Implement;
using MamaFit.Repositories.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace MamaFit.Repositories.Repository;

public class OTPRepository : GenericRepository<OTP>, IOTPRepository
{
    public OTPRepository(ApplicationDbContext context, IHttpContextAccessor accessor) : base(context, accessor)
    {
    }
    
    public async Task<OTP?> GetOTPAsync(string userId = null, string otp = null, OTPType? otpType = null)
    {
        var query = _dbSet.AsQueryable();

        if (!string.IsNullOrEmpty(userId))
        {
            query = query.Where(o => o.UserId == userId);
        }
    
        if (!string.IsNullOrEmpty(otp))
        {
            query = query.Where(o => o.Code == otp);
        }

        if (otpType.HasValue)
        {
            query = query.Where(o => o.OTPType == otpType);
        }

        return await query.FirstOrDefaultAsync();
    }

    
    public async Task CreateOTPAsync(OTP otp)
    {
        await InsertAsync(otp);
    }
    public async Task DeleteOTPAsync(OTP otp)
    {
        await DeleteAsync(otp);
    }
}