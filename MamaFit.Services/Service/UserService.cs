using AutoMapper;
using CloudinaryDotNet.Actions;
using MamaFit.BusinessObjects.DTO.UserDto;
using MamaFit.BusinessObjects.Entity;
using MamaFit.BusinessObjects.Enum;
using MamaFit.Repositories.Infrastructure;
using MamaFit.Repositories.Interface;
using MamaFit.Services.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace MamaFit.Services.Service;

public class UserService : IUserService
{
    private IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UserService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task SendRegisterOtpAsync(SendOTPRequestDto model)
    {
        var userRepo = _unitOfWork.GetRepository<ApplicationUser>();
        
        bool isExist = await userRepo.Entities.AnyAsync(x => x.UserEmail == model.Email && x.PhoneNumber == model.PhoneNumber && x.IsVerify == true);
        if (isExist)
            throw new ErrorException(StatusCodes.Status400BadRequest, "Email đã được sử dụng!");
        
        var user = await userRepo.Entities.FirstOrDefaultAsync(x => x.UserEmail == model.Email && x.IsVerify == false);
        if (user == null)
        {
            user = new ApplicationUser
            {
                UserEmail = model.Email,
                PhoneNumber = model.PhoneNumber,
                IsVerify = false,
                IsDeleted = false,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
            await userRepo.InsertAsync(user);
            await _unitOfWork.SaveAsync();
        }
        else
        {
            // Xóa OTP cũ nếu có
            var oldOtps = _unitOfWork.GetRepository<OTP>().Entities.Where(x => x.UserId == user.Id && x.OTPType == OTPType.REGISTER);
            foreach (var oldOtp in oldOtps)
            {
                _unitOfWork.GetRepository<OTP>().Delete(oldOtp.Id);
            }
            await _unitOfWork.SaveAsync();
        }
        var otpRepo = _unitOfWork.GetRepository<OTP>();
        string otpCode = GenerateOtpCode();
        var otp = new OTP
        {
            UserId = user.Id,
            Code = otpCode,
            ExpiredAt = DateTime.UtcNow.AddMinutes(5),
            OTPType = OTPType.REGISTER
        };
        await otpRepo.InsertAsync(otp);
        await _unitOfWork.SaveAsync();

        // TODO: Gửi mail ở đây (SendMailService.SendOtpEmail(email, otpCode))
    }

    public async Task CompleteRegisterAsync(RegisterUserRequestDto model)
    {
        var userRepo = _unitOfWork.GetRepository<ApplicationUser>();
        var user = await userRepo.Entities
            .FirstOrDefaultAsync(x =>
                x.UserEmail == model.Email &&
                x.PhoneNumber == model.PhoneNumber &&
                x.IsVerify == true &&
                (x.HashPassword == null || x.HashPassword == "")
            );

        
        if (user == null)
            throw new ErrorException(StatusCodes.Status400BadRequest, "Bạn chưa xác thực OTP hoặc user đã tạo mật khẩu!");

        var salt = PasswordHelper.GenerateSalt();
        var hashPassword = PasswordHelper.HashPassword(model.Password, salt);

        user.HashPassword = hashPassword;
        user.Salt = salt;
        user.UpdatedAt = DateTime.UtcNow;
        
        var roleRepo = _unitOfWork.GetRepository<ApplicationUserRole>();
        var defaultRole = await roleRepo.Entities.FirstOrDefaultAsync(r => r.RoleName == "User");
        if (defaultRole != null)
            user.RoleId = defaultRole.Id;

        await userRepo.UpdateAsync(user);
        await _unitOfWork.SaveAsync();
    }

    public async Task<List<UserReponseDto>> GetAllUsersAsync()
    {
        var userRepo = _unitOfWork.GetRepository<ApplicationUser>();
        var users = await userRepo.Entities
            .Include(u => u.Role)
            .Where(u => u.IsDeleted == false)
            .ToListAsync();
        
        return _mapper.Map<List<UserReponseDto>>(users);
    }
    
    public async Task<UserReponseDto> GetUserByIdAsync(string userId)
    {
        var userRepo = _unitOfWork.GetRepository<ApplicationUser>();
        var user = await userRepo.Entities
            .Include(u => u.Role)
            .FirstOrDefaultAsync(u => u.Id == userId && u.IsDeleted == false);
        
        if (user == null)
            throw new ErrorException(StatusCodes.Status404NotFound, "Người dùng không tồn tại!");

        return _mapper.Map<UserReponseDto>(user);
    }

    public static string GenerateOtpCode(int length = 6)
    {
        var random = new Random();
        int min = (int)Math.Pow(10, length - 1);
        int max = (int)Math.Pow(10, length) - 1;
        var code = random.Next(min, max + 1); // Cộng 1 để lấy đủ số chữ số
        return code.ToString();
    }

    
}