using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using AutoMapper;
using MamaFit.BusinessObjects.DTO.AuthDto;
using MamaFit.BusinessObjects.DTO.OTPDto;
using MamaFit.BusinessObjects.DTO.Token;
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
    private IUnitOfWork _unitOfWork;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IConfiguration _configuration;
    private readonly IMapper _mapper;

    public AuthService(IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor, IConfiguration configuration, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _httpContextAccessor = httpContextAccessor;
        _configuration = configuration;
        var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
        _configuration = builder.Build();
        _mapper = mapper;
    }
    
    public async Task<string> GetCurrentUserIdAsync()
    {
        var userId = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId))
        {
            throw new UnauthorizedAccessException("User is not authenticated.");
        }
        return userId;
    }

    public async Task<TokenResponseDto> SignInAsync(LoginRequestDto model)
{
    if (model == null)
        throw new ErrorException(StatusCodes.Status400BadRequest, "Dữ liệu đăng nhập không hợp lệ!");

    var userRepo = _unitOfWork.GetRepository<ApplicationUser>();
    ApplicationUser? user = null;
    string loginKey = null;

    if (!string.IsNullOrWhiteSpace(model.Username))
    {
        loginKey = model.Username;
        user = await userRepo.Entities.FirstOrDefaultAsync(p => p.UserName == model.Username);
    }
    else if (!string.IsNullOrWhiteSpace(model.Email))
    {
        loginKey = model.Email.Trim().ToLower();
        user = await userRepo.Entities.FirstOrDefaultAsync(
            p => p.UserEmail != null && p.UserEmail.ToLower() == loginKey
        );
    }
    else
    {
        throw new ErrorException(StatusCodes.Status400BadRequest, "Bạn phải nhập Email hoặc Username!");
    }

    if (user == null)
        throw new ErrorException(StatusCodes.Status401Unauthorized, $"Không tìm thấy tài khoản với thông tin: {loginKey}");

    if (user.IsDeleted)
        throw new ErrorException(StatusCodes.Status403Forbidden, "Tài khoản của bạn đã bị khóa hoặc xóa.");
    if (!user.IsVerify)
        throw new ErrorException(StatusCodes.Status401Unauthorized, "Tài khoản chưa xác thực email/số điện thoại.");

    if (!VerifyPassword(model.Password, user.HashPassword, user.Salt))
        throw new ErrorException(StatusCodes.Status401Unauthorized, "Mật khẩu không đúng!");

    // ---- DEBUG RoleId trước khi query
    if (string.IsNullOrWhiteSpace(user.RoleId))
    {
        throw new ErrorException(StatusCodes.Status500InternalServerError,
            "RoleId của user bị null/trống. Vui lòng kiểm tra lại dữ liệu tài khoản!");
    }

    var roleRepo = _unitOfWork.GetRepository<ApplicationUserRole>();
    var userRole = await roleRepo.Entities.FirstOrDefaultAsync(x => x.Id == user.RoleId);

    if (userRole == null)
    {
        throw new ErrorException(StatusCodes.Status500InternalServerError,
            $"Không tìm thấy role cho tài khoản này! RoleId: {user.RoleId}");
    }

    string role = userRole.RoleName;
    var token = GenerateTokens(user, role);

    var refreshTokenEntity = new ApplicationUserToken
    {
        UserId = user.Id,
        Token = token.RefreshToken,
        ExpiredAt = DateTime.UtcNow.AddDays(int.Parse(_configuration["JWT:RefreshTokenExpirationDays"])),
        IsRevoked = false,
        TokenType = TokenType.REFRESH_TOKEN
    };

    var userTokenRepo = _unitOfWork.GetRepository<ApplicationUserToken>();
    await userTokenRepo.InsertAsync(refreshTokenEntity);
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
        throw new ErrorException(StatusCodes.Status401Unauthorized, "Refresh token không hợp lệ!");
    
    if (userToken.ExpiredAt < DateTime.UtcNow)
        throw new ErrorException(StatusCodes.Status401Unauthorized, "Refresh token đã hết hạn!");
    
    var userRepo = _unitOfWork.GetRepository<ApplicationUser>();
    var user = await userRepo.GetByIdAsync(userToken.UserId);
    if (user == null || user.IsDeleted == true)
        throw new ErrorException(StatusCodes.Status401Unauthorized, "Tài khoản không hợp lệ!");
    
    var roleRepo = _unitOfWork.GetRepository<ApplicationUserRole>();
    string role = (await roleRepo.FindAsync(x => x.Id == user.RoleId))?.RoleName ?? "unknown";
    
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
            throw new ErrorException(StatusCodes.Status404NotFound, "Không tìm thấy người dùng với email này!");
        
        var otpRepo = _unitOfWork.GetRepository<OTP>();
        var otp = await otpRepo.Entities
            .Where(x => x.UserId == user.Id
                        && x.Code == model.Code
                        && x.OTPType == model.OTPType
                        && x.ExpiredAt >= DateTime.UtcNow)
            .OrderByDescending(x => x.ExpiredAt)
            .FirstOrDefaultAsync();

        if (otp == null)
            throw new ErrorException(StatusCodes.Status400BadRequest, "OTP không hợp lệ hoặc đã hết hạn.");
        
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
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.UserName ?? string.Empty),
            new Claim(ClaimTypes.Email, user.UserEmail ?? string.Empty),
            new Claim(ClaimTypes.Role, role),
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
        
        // _refreshTokenService.SaveRefreshToken(user.Id, refreshTokenString, now.AddDays(refreshTokenExpirationDays));

        return new TokenResponseDto()
        {
            AccessToken = accessTokenString,
            RefreshToken = refreshTokenString,
        };
    }
}
