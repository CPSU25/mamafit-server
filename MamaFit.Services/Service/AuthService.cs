using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using AutoMapper;
using MamaFit.BusinessObjects.DTO.AuthDto;
using MamaFit.BusinessObjects.DTO.Token;
using MamaFit.BusinessObjects.Entity;
using MamaFit.Repositories.Infrastructure;
using MamaFit.Repositories.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace MamaFit.Services.Service;

public class AuthService
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
        ApplicationUser? user = await _unitOfWork.GetRepository<ApplicationUser>().Entities.FirstOrDefaultAsync(p => p.UserName == model.Username || p.UserEmail == model.Username);
        if (user.IsDeleted)
            throw new ErrorException(StatusCodes.Status403Forbidden, "Tài khoản của bạn đã bị khóa hoặc xóa.");
        if (!user.IsVerify)
            throw new ErrorException(StatusCodes.Status401Unauthorized, "Tài khoản chưa xác thực email/số điện thoại.");
        if (user == null)
        {
            throw new ErrorException(StatusCodes.Status401Unauthorized, "Bạn chưa có tài khoản! Vui lòng đăng ký!");
        }

        if (!VerifyPassword(model.Password, user.HashPassword, user.Salt))
        {
            throw new ErrorException(StatusCodes.Status401Unauthorized, "Mật khẩu không đúng!");
        }
        
        var roleRepo = _unitOfWork.GetRepository<ApplicationUserRole>();
        string role = (await roleRepo.FindAsync(x => x.Id == user.RoleId))?.RoleName ?? "unknown";
        var token = GenerateTokens(user, role);
        return new TokenResponseDto()
        {
            AccessToken = token.AccessToken,
            RefreshToken = token.RefreshToken,
        };
    }

    private bool VerifyPassword(string password, string storedHash, string storedSalt)
    {
        var saltBytes = Convert.FromBase64String(storedSalt);
        var hashBytes = new Rfc2898DeriveBytes(password, saltBytes, 10000, HashAlgorithmName.SHA256).GetBytes(32);
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
            new Claim(ClaimTypes.Name, user.UserName),
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
