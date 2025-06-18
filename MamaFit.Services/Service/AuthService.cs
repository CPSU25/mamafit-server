using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using AutoMapper;
using MamaFit.BusinessObjects.DTO.AuthDto;
using MamaFit.BusinessObjects.DTO.OTPDto;
using MamaFit.BusinessObjects.DTO.TokenDto;
using MamaFit.BusinessObjects.DTO.UserDto;
using MamaFit.BusinessObjects.Entity;
using MamaFit.BusinessObjects.Enum;
using MamaFit.Repositories.Helper;
using MamaFit.Repositories.Infrastructure;
using MamaFit.Repositories.Interface;
using MamaFit.Services.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace MamaFit.Services.Service;

public class AuthService : IAuthService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IEmailSenderSevice _emailSenderService;
    private readonly IConfiguration _configuration;
    private readonly IMapper _mapper;
    private readonly IValidationService _validation;

    public AuthService(IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor, IConfiguration configuration,
        IMapper mapper, IEmailSenderSevice emailSenderService, IValidationService validation)
    {
        _unitOfWork = unitOfWork;
        _httpContextAccessor = httpContextAccessor;
        _configuration = configuration;
        _mapper = mapper;
        _emailSenderService = emailSenderService;
        _validation = validation;
    }

    public async Task<PermissionResponseDto> GetCurrentUserAsync()
    {
        var userId = _httpContextAccessor.HttpContext?.User.FindFirstValue("userId");
        if (string.IsNullOrWhiteSpace(userId))
            throw new ErrorException(StatusCodes.Status401Unauthorized, "Unauthorized access!");

        var user = await _unitOfWork.UserRepository.GetByIdAsync(userId);

        if (user == null)
            throw new ErrorException(StatusCodes.Status404NotFound,
                ApiCodes.NOT_FOUND, "User not found!");

        return _mapper.Map<PermissionResponseDto>(user);
    }

    public async Task LogoutAsync(LogoutRequestDto model)
    {
        var userId = _httpContextAccessor.HttpContext?.User.FindFirstValue("userId");
        if (string.IsNullOrWhiteSpace(userId))
            throw new ErrorException(StatusCodes.Status401Unauthorized, "Unauthorized access!");

        if (!string.IsNullOrWhiteSpace(model.RefreshToken))
        {
            await _unitOfWork.TokenRepository.DeleteTokenAsync(userId, model.RefreshToken, TokenType.REFRESH_TOKEN);
        }

        if (!string.IsNullOrWhiteSpace(model.NotificationToken))
        {
            await _unitOfWork.TokenRepository.DeleteTokenAsync(userId, model.NotificationToken,
                TokenType.NOTIFICATION_TOKEN);
        }

        await _unitOfWork.SaveChangesAsync();
    }

    public async Task<TokenResponseDto> SignInAsync(LoginRequestDto model)
    {
        ApplicationUser? user = null;

        string loginKey = model.Identifier?.Trim();
        if (string.IsNullOrWhiteSpace(loginKey))
            throw new ErrorException(StatusCodes.Status400BadRequest,
                ApiCodes.BAD_REQUEST,
                "You must enter an email or username!");

        if (loginKey.Contains("@"))
        {
            string email = loginKey.ToLower();
            user = await _unitOfWork.UserRepository.GetByEmailAsync(email);
        }
        else
        {
            user = await _unitOfWork.UserRepository.GetByUsernameAsync(loginKey);
        }

        if (user == null)
            throw new ErrorException(StatusCodes.Status401Unauthorized,
                ApiCodes.UNAUTHORIZED,
                $"No account found with the provided information: {loginKey}");

        if (user.IsDeleted)
            throw new ErrorException(StatusCodes.Status403Forbidden,
                ApiCodes.FORBIDDEN, "Your account has been locked or deleted.");
        if (!user.IsVerify)
            throw new ErrorException(StatusCodes.Status401Unauthorized, ApiCodes.UNAUTHORIZED,
                "The account has not been verified via email or phone.");

        if (!VerifyPassword(model.Password, user.HashPassword, user.Salt))
            throw new ErrorException(StatusCodes.Status401Unauthorized,
                ApiCodes.UNAUTHORIZED, "Incorrect password!");

        var role = await _unitOfWork.RoleRepository.GetByIdAsync(user.RoleId);

        if (role == null)
        {
            throw new ErrorException(StatusCodes.Status404NotFound,
                ApiCodes.NOT_FOUND,
                $"No role found for this account! RoleId: {user.RoleId}");
        }

        string roleName = role.RoleName;
        var token = GenerateTokens(user, roleName);

        await HandleTokenAsync(user.Id, token.RefreshToken, TokenType.REFRESH_TOKEN);
        if (!string.IsNullOrWhiteSpace(model.NotificationToken))
        {
            await HandleTokenAsync(user.Id, model.NotificationToken, TokenType.NOTIFICATION_TOKEN);
        }

        return new TokenResponseDto
        {
            AccessToken = token.AccessToken,
            RefreshToken = token.RefreshToken,
        };
    }

    public async Task<TokenResponseDto> RefreshTokenAsync(RefreshTokenRequestDto model)
    {
        var userToken = await _unitOfWork.TokenRepository.GetTokenAsync(model.RefreshToken, TokenType.REFRESH_TOKEN);

        if (userToken == null)
            throw new ErrorException(StatusCodes.Status401Unauthorized, ApiCodes.UNAUTHORIZED,
                "Invalid refresh token!");

        if (userToken.ExpiredAt < DateTime.UtcNow)
            throw new ErrorException(StatusCodes.Status401Unauthorized, ApiCodes.TOKEN_EXPIRED,
                "Refresh token has expired!");

        var user = await _unitOfWork.UserRepository.GetByIdAsync(userToken.UserId);
        if (user == null || user.IsDeleted == true)
            throw new ErrorException(StatusCodes.Status404NotFound,
                ApiCodes.NOT_FOUND, "User not found or account has been deleted.");

        var role = await _unitOfWork.RoleRepository.GetByIdAsync(user.RoleId);
        string roleName = role.RoleName;

        var token = GenerateTokens(user, roleName);
        await HandleTokenAsync(user.Id, model.RefreshToken, TokenType.REFRESH_TOKEN);
        return new TokenResponseDto()
        {
            AccessToken = token.AccessToken,
            RefreshToken = token.RefreshToken
        };
    }

    public async Task VerifyOtpAsync(VerifyOtpRequestDto model)
    {
        var user = await _unitOfWork.UserRepository.GetByEmailAsync(model.Email);

        if (user == null)
            throw new ErrorException(StatusCodes.Status404NotFound,
                ApiCodes.NOT_FOUND, "User not found with the provided email.");

        string otpInputHash = HashHelper.HashOtp(model.Code);

        var otp = await _unitOfWork.OTPRepository.GetOTPAsync(user.Id, otpInputHash, OTPType.REGISTER);
        if (otp == null)
            throw new ErrorException(StatusCodes.Status400BadRequest,
                ApiCodes.BAD_REQUEST, "Invalid or expired OTP code.");

        user.IsVerify = true;
        await _unitOfWork.UserRepository.UpdateUserAsync(user);
        await _unitOfWork.OTPRepository.DeleteOTPAsync(otp);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task ResendOtpAsync(SendOTPRequestDto model)
    {
        var user = await _unitOfWork.UserRepository.GetByEmailPhoneAsync(model.Email, model.PhoneNumber);
        if (user == null)
            throw new ErrorException(StatusCodes.Status404NotFound,
                ApiCodes.NOT_FOUND, "User not found!");
        if (user.IsVerify)
            throw new ErrorException(StatusCodes.Status400BadRequest,
                ApiCodes.BAD_REQUEST, "User is already verified!");

        string otpCode = GenerateOtpCode();
        var otp = new OTP
        {
            UserId = user.Id,
            Code = otpCode,
            ExpiredAt = DateTime.UtcNow.AddMinutes(1),
            OTPType = OTPType.REGISTER
        };
        await _unitOfWork.OTPRepository.CreateOTPAsync(otp);
        await _unitOfWork.SaveChangesAsync();
        await SendOtpEmailAsync(model.Email, otpCode);
    }


    public async Task SendRegisterOtpAsync(SendOTPRequestDto model)
    {
        await _validation.ValidateAndThrowAsync(model);
        var isExist = await _unitOfWork.UserRepository.GetByEmailPhoneAsync(model.Email, model.PhoneNumber);
        if (isExist == null)
        {
            var user = _mapper.Map<ApplicationUser>(model);
            user.UserEmail = model.Email;
            user.PhoneNumber = model.PhoneNumber;
            user.IsVerify = false;
            await _unitOfWork.UserRepository.CreateUserAsync(user);
            await _unitOfWork.SaveChangesAsync();

            string otpCode = GenerateOtpCode();
            string otpHash = HashHelper.HashOtp(otpCode);
            var otp = new OTP
            {
                UserId = user.Id,
                Code = otpHash,
                ExpiredAt = DateTime.UtcNow.AddMinutes(5),
                OTPType = OTPType.REGISTER
            };
            await _unitOfWork.OTPRepository.CreateOTPAsync(otp);
            await _unitOfWork.SaveChangesAsync();

            await SendOtpEmailAsync(model.Email, otpCode);
        }
        else
        {
            if ((isExist.PhoneNumber == model.PhoneNumber || isExist.UserEmail == model.Email) && isExist.IsVerify)
            {
                throw new ErrorException(StatusCodes.Status400BadRequest,
                    ApiCodes.BAD_REQUEST, "Email or phone number has already been registered!");
            }

            var isEmailUsed = await _unitOfWork.UserRepository.GetByEmailAsync(model.Email);
            if (isEmailUsed != null && isEmailUsed.IsVerify)
            {
                throw new ErrorException(StatusCodes.Status400BadRequest,
                    ApiCodes.BAD_REQUEST, "Email has already been registered by another user!");
            }

            var isPhoneUsed = await _unitOfWork.UserRepository.GetByPhoneNumberAsync(model.PhoneNumber);
            if (isPhoneUsed != null && isPhoneUsed.IsVerify)
            {
                throw new ErrorException(StatusCodes.Status400BadRequest,
                    ApiCodes.BAD_REQUEST, "Phone number has already been registered by another user!");
            }

            var oldOtps = await _unitOfWork.OTPRepository.GetOTPAsync(isExist.Id, null, OTPType.REGISTER);
            await _unitOfWork.OTPRepository.DeleteOTPAsync(oldOtps);
            await _unitOfWork.SaveChangesAsync();

            string otpCode = GenerateOtpCode();
            string otpHash = HashHelper.HashOtp(otpCode);
            var otp = new OTP
            {
                UserId = isExist.Id,
                Code = otpHash,
                ExpiredAt = DateTime.UtcNow.AddMinutes(5),
                OTPType = OTPType.REGISTER
            };
            await _unitOfWork.OTPRepository.CreateOTPAsync(otp);
            await _unitOfWork.SaveChangesAsync();

            await SendOtpEmailAsync(model.Email, otpCode);
        }
    }

    public async Task CompleteRegisterAsync(RegisterUserRequestDto model)
    {
        await _validation.ValidateAndThrowAsync(model);
        var user = await _unitOfWork.UserRepository.GetByEmailPhoneAsync(model.Email, model.PhoneNumber);

        if (user == null)
            throw new ErrorException(StatusCodes.Status400BadRequest,
                ApiCodes.BAD_REQUEST, "Invalid email or phone number. Please check your information and try again.");
        if (user.IsVerify == false)
            throw new ErrorException(StatusCodes.Status400BadRequest,
                ApiCodes.BAD_REQUEST, "User is not verified! Please verify your account first.");

        var salt = HashHelper.GenerateSalt();
        var hashPassword = HashHelper.HashPassword(model.Password, salt);

        user.HashPassword = hashPassword;
        user.Salt = salt;

        var userRole = await _unitOfWork.RoleRepository.GetByNameAsync("User");
        if (userRole != null)
            user.RoleId = userRole.Id;

        await _unitOfWork.UserRepository.UpdateUserAsync(user);
        await _unitOfWork.SaveChangesAsync();
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
            <div class=""message"">Hello,<br/>Use the code below to complete your registration.<br/>This code will expire in <b>5 minutes</b>.</div>
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

    private bool VerifyPassword(string password, string storedHash, string storedSalt)
    {
        var saltBytes = Convert.FromBase64String(storedSalt);
        var hashBytes = new Rfc2898DeriveBytes(password, saltBytes, 20000, HashAlgorithmName.SHA256).GetBytes(32);
        return Convert.ToBase64String(hashBytes) == storedHash;
    }

    private TokenResponseDto GenerateTokens(ApplicationUser user, string role)
    {
        DateTime now = DateTime.UtcNow;
        var accessTokenExpirationMinutes = int.Parse(_configuration["JWT:AccessTokenExpirationMinutes"]);
        var refreshTokenExpirationDays = int.Parse(_configuration["JWT:RefreshTokenExpirationDays"]);

        var claims = new List<Claim>
        {
            new Claim("userId", user.Id.ToString()),
            new Claim("username", user.UserName ?? string.Empty),
            new Claim("name", user.FullName ?? string.Empty),
            new Claim("email", user.UserEmail ?? string.Empty),
            new Claim("role", role),
        };

        var keyString = _configuration["JWT:SecretKey"];
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(keyString));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

        var accessToken = new JwtSecurityToken(
            issuer: _configuration["JWT:Issuer"],
            audience: _configuration["JWT:Audience"],
            claims: claims,
            expires: now.AddMinutes(accessTokenExpirationMinutes),
            signingCredentials: creds
        );

        var accessTokenString = new JwtSecurityTokenHandler().WriteToken(accessToken);

        var refreshTokenString = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));

        return new TokenResponseDto()
        {
            AccessToken = accessTokenString,
            RefreshToken = refreshTokenString,
        };
    }

    public GoogleJwtPayload DecodePayload(string jwtToken)
    {
        var parts = jwtToken.Split('.');
        if (parts.Length != 3)
            throw new ArgumentException("Invalid JWT token");

        var payload = parts[1];
        var json = Encoding.UTF8.GetString(Convert.FromBase64String(PadBase64(payload)));
        return JsonSerializer.Deserialize<GoogleJwtPayload>(json);
    }

    private string PadBase64(string base64)
    {
        return base64.PadRight(base64.Length + (4 - base64.Length % 4) % 4, '=').Replace('-', '+').Replace('_', '/');
    }

    public async Task<TokenResponseDto> SignInWithGoogleJwtAsync(string jwtToken, string? notificationToken = null)
    {
        var payload = DecodePayload(jwtToken);

        var user = await _unitOfWork.UserRepository.GetByIdAsync(payload.sub);

        var userByEmail = await _unitOfWork.UserRepository.GetByEmailAsync(payload.email.ToLower());
        if (userByEmail != null && userByEmail.IsVerify && userByEmail.CreatedBy != "GoogleOAuth")
        {
            throw new ErrorException(StatusCodes.Status409Conflict, ApiCodes.CONFLICT,
                "Email has been aldready registered!");
        }

        if (user == null)
        {
            // Tạo user mới
            user = new ApplicationUser
            {
                Id = payload.sub,
                UserName = payload.name,
                FullName = payload.name,
                UserEmail = payload.email,
                HashPassword = null,
                Salt = null,
                ProfilePicture = payload.picture,
                DateOfBirth = null,
                PhoneNumber = null,
                IsVerify = payload.email_verified,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = "GoogleOAuth"
            };

            var defaultRole = await _unitOfWork.RoleRepository.GetByNameAsync("User");
            if (defaultRole != null)
                user.RoleId = defaultRole.Id;

            await _unitOfWork.UserRepository.InsertWithoutAuditAsync(user);
        }
        else
        {
            if (user.ProfilePicture != payload.picture)
            {
                user.ProfilePicture = payload.picture;
                user.UpdatedBy = "GoogleOAuth";
                user.UpdatedAt = DateTime.UtcNow;
                await _unitOfWork.UserRepository.UpdateWithoutAuditAsync(user);
            }
        }

        await _unitOfWork.SaveChangesAsync();

        if (string.IsNullOrWhiteSpace(user.RoleId))
            throw new ErrorException(StatusCodes.Status409Conflict,
                ApiCodes.CONFLICT,
                "The user's RoleId is null or empty. Please check the account data!");

        var userRole = await _unitOfWork.RoleRepository.GetByIdAsync(user.RoleId);

        if (userRole == null)
            throw new ErrorException(StatusCodes.Status404NotFound,
                ApiCodes.NOT_FOUND,
                $"No role found for this account! RoleId: {user.RoleId}");

        string? role = userRole.RoleName;

        var token = GenerateTokens(user, role);

        await HandleTokenAsync(user.Id, token.RefreshToken, TokenType.REFRESH_TOKEN);
        if (!string.IsNullOrWhiteSpace(notificationToken))
        {
            await HandleTokenAsync(user.Id, notificationToken, TokenType.NOTIFICATION_TOKEN);
        }

        return new TokenResponseDto
        {
            AccessToken = token.AccessToken,
            RefreshToken = token.RefreshToken
        };
    }

    private async Task HandleTokenAsync(string userId, string token, TokenType tokenType)
    {
        var existingToken = await _unitOfWork.TokenRepository.GetTokenByUserIdAsync(userId, tokenType);
        if (existingToken != null)
        {
            await _unitOfWork.TokenRepository.DeleteAsync(existingToken);
            await _unitOfWork.SaveChangesAsync();
        }

        DateTime? expiredAt = null;
        if (tokenType == TokenType.REFRESH_TOKEN)
        {
            expiredAt = DateTime.UtcNow.AddDays(int.Parse(_configuration["JWT:RefreshTokenExpirationDays"] ??
                                                          string.Empty));
        }

        if (tokenType == TokenType.NOTIFICATION_TOKEN)
        {
            expiredAt = null;
        }

        var tokenEntity = new ApplicationUserToken
        {
            UserId = userId,
            Token = token,
            ExpiredAt = expiredAt,
            IsRevoked = false,
            TokenType = tokenType,
            CreatedAt = DateTime.UtcNow
        };

        await _unitOfWork.TokenRepository.CreateTokenAsync(tokenEntity);
        await _unitOfWork.SaveChangesAsync();
    }
}