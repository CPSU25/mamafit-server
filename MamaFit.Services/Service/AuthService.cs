using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using AutoMapper;
using MamaFit.BusinessObjects.DTO.AuthDto;
using MamaFit.BusinessObjects.DTO.OTPDto;
using MamaFit.BusinessObjects.DTO.Token;
using MamaFit.BusinessObjects.DTO.UserDto;
using MamaFit.BusinessObjects.Entity;
using MamaFit.BusinessObjects.Enum;
using MamaFit.Repositories.Infrastructure;
using MamaFit.Repositories.Interface;
using MamaFit.Services.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace MamaFit.Services.Service;

public class AuthService : IAuthService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IConfiguration _configuration;
    private readonly IMapper _mapper;

    public AuthService(IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor, IConfiguration configuration,
        IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _httpContextAccessor = httpContextAccessor;
        _configuration = configuration;
        _mapper = mapper;
    }

    public async Task<UserReponseDto> GetCurrentUserAsync()
    {
        var userId = _httpContextAccessor.HttpContext?.User.FindFirstValue("userId");
        if (string.IsNullOrWhiteSpace(userId))
            throw new ErrorException(StatusCodes.Status401Unauthorized, "Unauthorized access!");

        var userRepo = _unitOfWork.GetRepository<ApplicationUser>();
        var user = await userRepo.Entities
            .Include(u => u.Role)
            .FirstOrDefaultAsync(u => u.Id.Equals(userId));

        if (user == null)
            throw new ErrorException(StatusCodes.Status404NotFound,
                ErrorCode.NotFound, "User not found!");

        return _mapper.Map<UserReponseDto>(user);
    }

    public async Task LogoutAsync(LogoutRequestDto model)
    {
        var userId = _httpContextAccessor.HttpContext?.User.FindFirstValue("userId");
        if (string.IsNullOrWhiteSpace(userId))
            throw new ErrorException(StatusCodes.Status401Unauthorized, "Unauthorized access!");

        var userTokenRepo = _unitOfWork.GetRepository<ApplicationUserToken>();
        //Delete refresh token
        if (!string.IsNullOrWhiteSpace(model.RefreshToken))
        {
            var refreshToken = await userTokenRepo.Entities
                .FirstOrDefaultAsync(t =>
                    t.UserId == userId && t.Token == model.RefreshToken && t.TokenType == TokenType.REFRESH_TOKEN);
            if (refreshToken != null)
                await userTokenRepo.DeleteAsync(refreshToken.Id);
        }

        //Delete notification token
        if (!string.IsNullOrWhiteSpace(model.NotificationToken))
        {
            var notificationToken = await userTokenRepo.Entities
                .FirstOrDefaultAsync(t =>
                    t.UserId == userId && t.Token == model.NotificationToken && t.TokenType == TokenType.NOTIFICATION_TOKEN);
            if (notificationToken != null)
                await userTokenRepo.DeleteAsync(notificationToken.Id);
        }
        await _unitOfWork.SaveAsync();
    }

    public async Task<TokenResponseDto> SignInAsync(LoginRequestDto model)
    {
        if (model == null)
            throw new ErrorException(StatusCodes.Status400BadRequest, ErrorCode.BadRequest, "Invalid login data!");

        var userRepo = _unitOfWork.GetRepository<ApplicationUser>();
        ApplicationUser? user = null;

        string loginKey = model.Identifier?.Trim();
        if (string.IsNullOrWhiteSpace(loginKey))
            throw new ErrorException(StatusCodes.Status400BadRequest,
                ErrorCode.BadRequest,
                "You must enter an email or username!");

        if (loginKey.Contains("@"))
        {
            string email = loginKey.ToLower();
            user = await userRepo.Entities.FirstOrDefaultAsync(p =>
                p.UserEmail != null && p.UserEmail.ToLower() == email
            );
        }
        else
        {
            user = await userRepo.Entities.FirstOrDefaultAsync(p => p.UserName == loginKey);
        }

        if (user == null)
            throw new ErrorException(StatusCodes.Status401Unauthorized,
                ErrorCode.UnAuthorized,
                $"No account found with the provided information: {loginKey}");

        if (user.IsDeleted)
            throw new ErrorException(StatusCodes.Status403Forbidden,
                ErrorCode.Forbidden, "Your account has been locked or deleted.");
        if (!user.IsVerify)
            throw new ErrorException(StatusCodes.Status401Unauthorized, ErrorCode.UnAuthorized,
                "The account has not been verified via email or phone.");

        if (!VerifyPassword(model.Password, user.HashPassword, user.Salt))
            throw new ErrorException(StatusCodes.Status401Unauthorized,
                ErrorCode.UnAuthorized, "Incorrect password!");

        if (string.IsNullOrWhiteSpace(user.RoleId))
        {
            throw new ErrorException(StatusCodes.Status409Conflict,
                ErrorCode.Conflicted,
                "The user's RoleId is null or empty. Please check the account data!");
        }

        var roleRepo = _unitOfWork.GetRepository<ApplicationUserRole>();
        var userRole = await roleRepo.Entities.FirstOrDefaultAsync(x => x.Id == user.RoleId);

        if (userRole == null)
        {
            throw new ErrorException(StatusCodes.Status404NotFound,
                ErrorCode.NotFound,
                $"No role found for this account! RoleId: {user.RoleId}");
        }

        string role = userRole.RoleName;
        var token = GenerateTokens(user, role);
        var userTokenRepo = _unitOfWork.GetRepository<ApplicationUserToken>();

        // 1. Save refresh token
        var refreshTokenEntity = new ApplicationUserToken
        {
            UserId = user.Id,
            Token = token.RefreshToken,
            ExpiredAt = DateTime.UtcNow.AddDays(int.Parse(_configuration["JWT:RefreshTokenExpirationDays"])),
            IsRevoked = false,
            TokenType = TokenType.REFRESH_TOKEN
        };
        await userTokenRepo.InsertAsync(refreshTokenEntity);

        // 2. Save notification token if provided
        if (!string.IsNullOrWhiteSpace(model.NotificationToken))
        {
            var oldTokens = userTokenRepo.Entities
                .Where(x => x.UserId == user.Id
                            && x.TokenType == TokenType.NOTIFICATION_TOKEN
                            && x.Token != model.NotificationToken)
                .ToList();

            foreach (var oldToken in oldTokens)
            {
                await userTokenRepo.DeleteAsync(oldToken.Id);
            }

            var existNotiToken = await userTokenRepo.Entities
                .FirstOrDefaultAsync(x =>
                    x.UserId == user.Id &&
                    x.Token == model.NotificationToken &&
                    x.TokenType == TokenType.NOTIFICATION_TOKEN);

            if (existNotiToken == null)
            {
                var notiTokenEntity = new ApplicationUserToken
                {
                    UserId = user.Id,
                    Token = model.NotificationToken,
                    ExpiredAt = null,
                    IsRevoked = false,
                    TokenType = TokenType.NOTIFICATION_TOKEN,
                    CreatedAt = DateTime.UtcNow
                };
                await userTokenRepo.InsertAsync(notiTokenEntity);
            }
        }

        await _unitOfWork.SaveAsync();

        return new TokenResponseDto
        {
            AccessToken = token.AccessToken,
            RefreshToken = token.RefreshToken,
        };
    }

    public async Task<TokenResponseDto> RefreshTokenAsync(RefreshTokenRequestDto model)
    {
        var tokenRepo = _unitOfWork.GetRepository<ApplicationUserToken>();
        var userToken = await tokenRepo.Entities
            .FirstOrDefaultAsync(t =>
                t.Token == model.RefreshToken
                && t.TokenType == TokenType.REFRESH_TOKEN
                && t.IsRevoked == false);

        if (userToken == null)
            throw new ErrorException(StatusCodes.Status401Unauthorized, ErrorCode.BadRequest,
                "Invalid refresh token!");

        if (userToken.ExpiredAt < DateTime.UtcNow)
            throw new ErrorException(StatusCodes.Status401Unauthorized, ErrorCode.TokenExpired,
                "Refresh token has expired!");

        var userRepo = _unitOfWork.GetRepository<ApplicationUser>();
        var user = await userRepo.GetByIdAsync(userToken.UserId);
        if (user == null || user.IsDeleted == true)
            throw new ErrorException(StatusCodes.Status404NotFound,
                ErrorCode.NotFound, "User not found or account has been deleted.");

        var roleRepo = _unitOfWork.GetRepository<ApplicationUserRole>();
        string role = (await roleRepo.FindAsync(x => x.Id == user.RoleId)).RoleName ?? "unknown";

        var token = GenerateTokens(user, role);

        userToken.IsRevoked = true;
        await tokenRepo.UpdateAsync(userToken);

        var refreshTokenEntity = new ApplicationUserToken
        {
            UserId = user.Id,
            Token = token.RefreshToken,
            ExpiredAt = DateTime.UtcNow.AddDays(int.Parse(_configuration["JWT:RefreshTokenExpirationDays"])),
            IsRevoked = false,
            TokenType = TokenType.REFRESH_TOKEN
        };
        await tokenRepo.InsertAsync(refreshTokenEntity);
        await _unitOfWork.SaveAsync();

        return new TokenResponseDto()
        {
            AccessToken = token.AccessToken,
            RefreshToken = token.RefreshToken
        };
    }

    public async Task VerifyOtpAsync(VerifyOtpRequestDto model)
    {
        var userRepo = _unitOfWork.GetRepository<ApplicationUser>();
        var user = await userRepo.Entities.FirstOrDefaultAsync(x => x.UserEmail == model.Email);

        if (user == null)
            throw new ErrorException(StatusCodes.Status404NotFound,
                ErrorCode.NotFound, "User not found with the provided email.");

        var otpRepo = _unitOfWork.GetRepository<OTP>();
        var otp = await otpRepo.Entities
            .Where(x => x.UserId == user.Id
                        && x.Code == model.Code
                        && x.OTPType == model.OTPType
                        && x.ExpiredAt >= DateTime.UtcNow)
            .OrderByDescending(x => x.ExpiredAt)
            .FirstOrDefaultAsync();

        if (otp == null)
            throw new ErrorException(StatusCodes.Status400BadRequest,
                ErrorCode.BadRequest, "Invalid or expired OTP code.");

        user.IsVerify = true;
        await userRepo.UpdateAsync(user);

        otpRepo.Delete(otp.Id);
        await _unitOfWork.SaveAsync();
    }

    private bool VerifyPassword(string password, string storedHash, string storedSalt)
    {
        var saltBytes = Convert.FromBase64String(storedSalt);
        var hashBytes = new Rfc2898DeriveBytes(password, saltBytes, 20000, HashAlgorithmName.SHA256).GetBytes(32);
        return Convert.ToBase64String(hashBytes) == storedHash;
    }

    public TokenResponseDto GenerateTokens(ApplicationUser user, string role)
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

        var userRepo = _unitOfWork.GetRepository<ApplicationUser>();
        var user = await userRepo.Entities.FirstOrDefaultAsync(u => u.Id == payload.sub);

        var userByEmail = await userRepo.Entities.FirstOrDefaultAsync(u => u.UserEmail == payload.email && u.CreatedBy.Equals("System"));

        if (userByEmail != null)
        {
            throw new ErrorException(StatusCodes.Status409Conflict, ErrorCode.Conflicted, "Email has been aldready registered!");
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

            var roleRepo = _unitOfWork.GetRepository<ApplicationUserRole>();
            var defaultRole = await roleRepo.Entities.FirstOrDefaultAsync(r => r.RoleName == "User");
            if (defaultRole != null)
                user.RoleId = defaultRole.Id;

            await userRepo.InsertAsync(user);
        }
        else
        {
            // Cập nhật profile picture nếu khác
            if (user.ProfilePicture != payload.picture)
            {
                user.ProfilePicture = payload.picture;
                user.UpdatedAt = DateTime.UtcNow;
                user.UpdatedBy = "GoogleOAuth";
                await userRepo.UpdateAsync(user);
            }
        }

        await _unitOfWork.SaveAsync();

        if (string.IsNullOrWhiteSpace(user.RoleId))
            throw new ErrorException(StatusCodes.Status409Conflict,
                ErrorCode.Conflicted,
                "The user's RoleId is null or empty. Please check the account data!");

        var roleRepoCheck = _unitOfWork.GetRepository<ApplicationUserRole>();
        var userRole = await roleRepoCheck.Entities.FirstOrDefaultAsync(x => x.Id == user.RoleId);

        if (userRole == null)
            throw new ErrorException(StatusCodes.Status404NotFound,
                ErrorCode.NotFound,
                $"No role found for this account! RoleId: {user.RoleId}");

        string role = userRole.RoleName;

        var token = GenerateTokens(user, role);

        var userTokenRepo = _unitOfWork.GetRepository<ApplicationUserToken>();

        // Lưu refresh token
        var refreshTokenEntity = new ApplicationUserToken
        {
            UserId = user.Id,
            Token = token.RefreshToken,
            ExpiredAt = DateTime.UtcNow.AddDays(int.Parse(_configuration["JWT:RefreshTokenExpirationDays"])),
            IsRevoked = false,
            TokenType = TokenType.REFRESH_TOKEN
        };
        await userTokenRepo.InsertAsync(refreshTokenEntity);

        // Xử lý notification token nếu có
        if (!string.IsNullOrWhiteSpace(notificationToken))
        {
            var oldTokens = await userTokenRepo.Entities
                .Where(x => x.UserId == user.Id
                            && x.TokenType == TokenType.NOTIFICATION_TOKEN
                            && x.Token != notificationToken)
                .ToListAsync();

            foreach (var oldToken in oldTokens)
            {
                await userTokenRepo.DeleteAsync(oldToken.Id);
            }

            var existNotiToken = await userTokenRepo.Entities
                .FirstOrDefaultAsync(x =>
                    x.UserId == user.Id &&
                    x.Token == notificationToken &&
                    x.TokenType == TokenType.NOTIFICATION_TOKEN);

            if (existNotiToken == null)
            {
                var notiTokenEntity = new ApplicationUserToken
                {
                    UserId = user.Id,
                    Token = notificationToken,
                    ExpiredAt = null,
                    IsRevoked = false,
                    TokenType = TokenType.NOTIFICATION_TOKEN,
                    CreatedAt = DateTime.UtcNow
                };
                await userTokenRepo.InsertAsync(notiTokenEntity);
            }
        }

        await _unitOfWork.SaveAsync();

        return new TokenResponseDto
        {
            AccessToken = token.AccessToken,
            RefreshToken = token.RefreshToken
        };
    }

}