using System.Security.Claims;
using AutoMapper;
using CloudinaryDotNet.Actions;
using MamaFit.BusinessObjects.DTO.UploadImageDto;
using MamaFit.BusinessObjects.DTO.UserDto;
using MamaFit.BusinessObjects.Entity;
using MamaFit.BusinessObjects.Enum;
using MamaFit.Repositories.Helper;
using MamaFit.Repositories.Infrastructure;
using MamaFit.Repositories.Interface;
using MamaFit.Services.ExternalService;
using MamaFit.Services.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace MamaFit.Services.Service;

public class UserService : IUserService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IEmailSenderSevice _emailSenderService;
    private readonly ICloudinaryService _cloudinaryService;

    public UserService(IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor, IEmailSenderSevice emailSenderService, ICloudinaryService cloudinaryService)
    {
        _cloudinaryService = cloudinaryService;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _httpContextAccessor = httpContextAccessor;
        _emailSenderService = emailSenderService;
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
            ExpiredAt = DateTime.UtcNow.AddMinutes(1),
            OTPType = OTPType.REGISTER
        };
        await otpRepo.InsertAsync(otp);
        await _unitOfWork.SaveAsync();
        await SendOtpEmailAsync(model.Email, otpCode);
    }

    public async Task<PhotoUploadResult> UpdateUserProfilePictureAsync(UploadImageDto model)
    {
        var userRepo = _unitOfWork.GetRepository<ApplicationUser>();
        var user = await userRepo.Entities.FirstOrDefaultAsync(u => u.Id == model.Id);

        if (user == null)
            throw new ErrorException(StatusCodes.Status404NotFound, ErrorCode.NotFound, "User not found.");
        
        if (!string.IsNullOrEmpty(user.ProfilePicture))
        {
            var publicId = _cloudinaryService.GetCloudinaryPublicIdFromUrl(user.ProfilePicture);
            if (!string.IsNullOrEmpty(publicId))
            {
                await _cloudinaryService.DeletePhotoAsync(publicId);
            }
        }
        
        var uploadResult = await _cloudinaryService.AddPhotoAsync(model.NewImage);
        
        if (!uploadResult.IsSuccess)
            throw new ErrorException(StatusCodes.Status400BadRequest, ErrorCode.BadRequest, uploadResult.ErrorMessage ?? "Failed to upload image.");
        
        user.ProfilePicture = uploadResult.Url;
        user.UpdatedAt = DateTime.UtcNow;
        user.UpdatedBy = GetCurrentUserName();
        await userRepo.UpdateAsync(user);
        await _unitOfWork.SaveAsync();

        return uploadResult;
    }
    public async Task SendRegisterOtpAsync(SendOTPRequestDto model)
    {
        var userRepo = _unitOfWork.GetRepository<ApplicationUser>();
        
        bool isExist = await userRepo.Entities.AnyAsync(x => x.UserEmail == model.Email && x.PhoneNumber == model.PhoneNumber && x.IsVerify == true);
        if (isExist)
            throw new ErrorException(StatusCodes.Status400BadRequest,
                ErrorCode.BadRequest, "Email or phone number has already been registered!");
        bool isEmailUsed = await userRepo.Entities.AnyAsync(x => x.UserEmail == model.Email && x.IsVerify == true);
        if (isEmailUsed)
            throw new ErrorException(StatusCodes.Status400BadRequest,
                ErrorCode.BadRequest, "Email has already been used for registration!");
        bool isPhoneUsed = await userRepo.Entities.AnyAsync(x => x.PhoneNumber == model.PhoneNumber && x.IsVerify == true);
        if (isPhoneUsed)
            throw new ErrorException(StatusCodes.Status400BadRequest,
                ErrorCode.BadRequest, "Phone number has already been used for registration!");
        
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
                CreatedBy = GetCurrentUserName()
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
        string otpHash = HashHelper.HashOtp(otpCode);
        var otp = new OTP
        {
            UserId = user.Id,
            Code = otpHash,
            ExpiredAt = DateTime.UtcNow.AddMinutes(1),
            OTPType = OTPType.REGISTER
        };
        await otpRepo.InsertAsync(otp);
        await _unitOfWork.SaveAsync();
        
            await SendOtpEmailAsync(model.Email, otpCode);
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

        var salt = HashHelper.GenerateSalt();
        var hashPassword = HashHelper.HashPassword(model.Password, salt);

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
    
    public async Task SendOtpEmailAsync(string email, string otpCode)
{
    string subject = "Your OTP Code - MamaFit";
    string preheader = "Use this OTP code to complete your registration. This code is valid for 60 seconds.";

    string content = $@"
    <!DOCTYPE html>
    <html lang=""en"">
    <head>
        <meta charset=""UTF-8"">
        <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
        <meta http-equiv=""X-UA-Compatible"" content=""IE=edge"">
        <title>Your OTP Code</title>
        <style>
            body {{ font-family: Arial, Helvetica, sans-serif; background: #f7f7f7; margin:0; padding:0; }}
            .container {{ max-width: 480px; margin: 40px auto; background: #fff; border-radius: 8px; box-shadow: 0 2px 10px rgba(0,0,0,0.05); padding: 32px 24px; }}
            .brand {{ font-size: 24px; font-weight: bold; color: #2266cc; margin-bottom: 20px; text-align:center; }}
            .title {{ font-size: 20px; font-weight: bold; margin-bottom: 8px; color: #333; text-align:center; }}
            .otp {{ font-size: 28px; font-weight: bold; color: #e91e63; letter-spacing: 6px; margin: 24px 0; text-align:center; }}
            .message {{ font-size: 16px; color: #333; margin-bottom: 18px; text-align:center; }}
            .footer {{ margin-top: 36px; font-size: 13px; color: #888; text-align:center; }}
        </style>
    </head>
    <body>
        <span style=""display:none!important;"">{preheader}</span>
        <div class=""container"">
            <div class=""brand"">MamaFit</div>
            <div class=""title"">Your One-Time Password (OTP)</div>
            <div class=""message"">Hello,<br/>Use the code below to complete your registration.<br/>This code will expire in <b>60 seconds</b>.</div>
            <div class=""otp"">{otpCode}</div>
            <div class=""footer"">
                If you did not request this, please ignore this email.<br>
                &copy; {DateTime.Now.Year} MamaFit. All rights reserved.
            </div>
        </div>
    </body>
    </html>";

    await _emailSenderService.SendEmailAsync(email, subject, content);
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