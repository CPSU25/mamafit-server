using System.Security.Claims;
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
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UserService(IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _httpContextAccessor = httpContextAccessor;
    }

    private string GetCurrentUserName()
    {
        return _httpContextAccessor.HttpContext?.User?.FindFirst("name")?.Value ?? "System";
    }

    public async Task ResendOtpAsync(SendOTPRequestDto model)
    {
        var userRepo = _unitOfWork.GetRepository<ApplicationUser>();
        var user = await userRepo.Entities.FirstOrDefaultAsync(x =>
            x.UserEmail == model.Email && x.PhoneNumber == model.PhoneNumber && x.IsVerify == false);

        if (user == null)
            throw new ErrorException(StatusCodes.Status404NotFound,
                ErrorCode.NotFound, "User not found or already registered!");
        
        var oldOtps = _unitOfWork.GetRepository<OTP>().Entities
            .Where(x => x.UserId == user.Id && x.OTPType == OTPType.REGISTER);
        foreach (var oldOtp in oldOtps)
        {
            await _unitOfWork.GetRepository<OTP>().DeleteAsync(oldOtp.Id);
        }

        await _unitOfWork.SaveAsync();

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
    }

    public async Task SendRegisterOtpAsync(SendOTPRequestDto model)
    {
        var userRepo = _unitOfWork.GetRepository<ApplicationUser>();
        
        bool isExist = await userRepo.Entities.AnyAsync(x => x.UserEmail == model.Email && x.PhoneNumber == model.PhoneNumber && x.IsVerify == true);
        if (isExist)
            throw new ErrorException(StatusCodes.Status400BadRequest,
                ErrorCode.BadRequest, "Email or phone number has already been registered!");
        
        var user = await userRepo.Entities.FirstOrDefaultAsync(x => x.UserEmail == model.Email && x.IsVerify == false);
        if (user == null)
        {
            user = new ApplicationUser
            {
                UserEmail = model.Email,
                PhoneNumber = model.PhoneNumber,
                IsVerify = false,
                IsDeleted = false,
                CreatedAt = DateTime.UtcNow
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
                await _unitOfWork.GetRepository<OTP>().DeleteAsync(oldOtp.Id);
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
            throw new ErrorException(StatusCodes.Status400BadRequest,
                ErrorCode.BadRequest, "User not found or already registered!");

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

    public async Task<PaginatedList<UserReponseDto>> GetAllUsersAsync(
        int index = 1, int pageSize = 10, string? nameSearch = null, string? roleName = null)
    {
        var userRepo = _unitOfWork.GetRepository<ApplicationUser>();
        var query = userRepo.Entities
            .Include(u => u.Role)
            .Where(u => !u.IsDeleted);

        if (!string.IsNullOrWhiteSpace(nameSearch))
        {
            query = query.Where(u => u.FullName.Contains(nameSearch) || u.UserName.Contains(nameSearch));
        }

        if (!string.IsNullOrWhiteSpace(roleName))
        {
            query = query.Where(u => u.Role.RoleName == roleName);
        }

        var pagedResult = await userRepo.GetPagging(query, index, pageSize);

        var responseItems = pagedResult.Items
            .Select(_mapper.Map<UserReponseDto>)
            .ToList();

        var responsePaginatedList = new PaginatedList<UserReponseDto>(
            responseItems,
            pagedResult.TotalCount,
            pagedResult.PageNumber,
            pageSize
        );

        return responsePaginatedList;
    }

    public async Task<UserReponseDto> GetUserByIdAsync(string userId)
    {
        var userRepo = _unitOfWork.GetRepository<ApplicationUser>();
        var user = await userRepo.Entities
            .Include(u => u.Role)
            .FirstOrDefaultAsync(u => u.Id == userId && u.IsDeleted == false);
        
        if (user == null)
            throw new ErrorException(StatusCodes.Status404NotFound,
                ErrorCode.NotFound, "User not found!");

        return _mapper.Map<UserReponseDto>(user);
    }

    public async Task<UserReponseDto> UpdateUserAsync(string userId, UpdateUserRequestDto model)
    {
        var userRepo = _unitOfWork.GetRepository<ApplicationUser>();
        var user = await userRepo.Entities
            .FirstOrDefaultAsync(u => u.Id == userId && u.IsDeleted == false);
        
        if (user == null)
            throw new ErrorException(StatusCodes.Status404NotFound,
                ErrorCode.NotFound, "User not found!");
        
        user.UserName = model.Username;
        user.FullName = model.FullName;
        user.PhoneNumber = model.PhoneNumber;
        user.UpdatedAt = DateTime.UtcNow;
        user.UpdatedBy = GetCurrentUserName();
        user.DateOfBirth = model.DateOfBirth;
        user.RoleId = model.RoleId;
        
        await userRepo.UpdateAsync(user);
        await _unitOfWork.SaveAsync();
        
        var updatedUser = await userRepo.Entities
            .Include(u => u.Role)
            .FirstOrDefaultAsync(u => u.Id == userId && u.IsDeleted == false);

        return _mapper.Map<UserReponseDto>(updatedUser);
    }
    
    public async Task DeleteUserAsync(string userId)
    {
        var userRepo = _unitOfWork.GetRepository<ApplicationUser>();
        var user = await userRepo.Entities
            .FirstOrDefaultAsync(u => u.Id == userId && u.IsDeleted == false);
        
        if (user == null)
            throw new ErrorException(StatusCodes.Status404NotFound,
                ErrorCode.NotFound, "User not found!");

        user.IsDeleted = true;
        user.UpdatedAt = DateTime.UtcNow;
        user.UpdatedBy = GetCurrentUserName();
        await userRepo.UpdateAsync(user);
        await _unitOfWork.SaveAsync();
    }
    
    public static string GenerateOtpCode(int length = 6)
    {
        var random = new Random();
        int min = (int)Math.Pow(10, length - 1);
        int max = (int)Math.Pow(10, length) - 1;
        var code = random.Next(min, max + 1);
        return code.ToString();
    }
}