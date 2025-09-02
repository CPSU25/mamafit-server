using AutoMapper;
using MamaFit.BusinessObjects.DTO.UploadImageDto;
using MamaFit.BusinessObjects.DTO.UserDto;
using MamaFit.Repositories.Helper;
using MamaFit.Repositories.Implement;
using MamaFit.Repositories.Infrastructure;
using MamaFit.Services.ExternalService.CloudinaryService;
using MamaFit.Services.Interface;
using Microsoft.AspNetCore.Http;
using System.Security.Cryptography;

namespace MamaFit.Services.Service;

public class UserService : IUserService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ICloudinaryService _cloudinaryService;
    private readonly IValidationService _validation;
    private readonly IHttpContextAccessor _context;

    public UserService(IUnitOfWork unitOfWork, IMapper mapper, ICloudinaryService cloudinaryService,
        IValidationService validation, IHttpContextAccessor context)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _cloudinaryService = cloudinaryService;
        _validation = validation;
        _context = context;
    }

    public async Task<PaginatedList<UserReponseDto>> GetAllUsersAsync(
        int index = 1, int pageSize = 10, string? nameSearch = null, string? roleName = null)
    {
        var users = await _unitOfWork.UserRepository.GetUsersAsync(index, pageSize, nameSearch, roleName);

        var responseItems = users.Items
            .Select(_mapper.Map<UserReponseDto>)
            .ToList();

        var responsePaginatedList = new PaginatedList<UserReponseDto>(
            responseItems,
            users.TotalCount,
            users.PageNumber,
            pageSize
        );

        return responsePaginatedList;
    }

    private bool VerifyPassword(string password, string storedHash, string storedSalt)
    {
        var saltBytes = Convert.FromBase64String(storedSalt);
        var hashBytes = new Rfc2898DeriveBytes(password, saltBytes, 20000, HashAlgorithmName.SHA256).GetBytes(32);
        return Convert.ToBase64String(hashBytes) == storedHash;
    }

    public async Task<UserReponseDto> GetUserByIdAsync(string userId)
    {
        var user = await _unitOfWork.UserRepository.GetByIdAsync(userId);
        if (user == null)
            throw new ErrorException(StatusCodes.Status404NotFound,
                ApiCodes.NOT_FOUND, "User not found!");

        return _mapper.Map<UserReponseDto>(user);
    }

    public async Task<UserReponseDto> UpdateUserAsync(UpdateProfileRequestDto model)
    {
        await _validation.ValidateAndThrowAsync(model);
        var currentUserId = GetCurrentUserId();

        var user = await _unitOfWork.UserRepository.GetByIdNotDeletedAsync(currentUserId);
        _validation.CheckNotFound(user, "User not found!");

        if (user.UserName != model.UserName)
        {
            var usernameExists = await _unitOfWork.UserRepository.IsEntityExistsAsync(x =>
                x.UserName == model.UserName && x.Id != currentUserId);
            _validation.CheckBadRequest(usernameExists, "Username already exists!");
        }

        if (!string.IsNullOrEmpty(model.NewPassword))
        {
            if (!VerifyPassword(model.OldPassword, user.HashPassword, user.Salt))
                throw new ErrorException(StatusCodes.Status401Unauthorized,
                    ApiCodes.UNAUTHORIZED, "Incorrect password!");

            user.Salt = HashHelper.GenerateSalt();
            user.HashPassword = HashHelper.HashPassword(model.NewPassword, user.Salt);
        }

        _mapper.Map(model, user);

        await _unitOfWork.UserRepository.UpdateUserAsync(user);
        await _unitOfWork.SaveChangesAsync();

        return _mapper.Map<UserReponseDto>(user);
    }

    private string GetCurrentUserId()
    {
        return _context.HttpContext.User.FindFirst("userId").Value;
    }

    public async Task DeleteUserAsync(string userId)
    {
        var user = await _unitOfWork.UserRepository.GetByIdAsync(userId);
        _validation.CheckNotFound(user, "User not found!");
        await _unitOfWork.UserRepository.DeleteUserAsync(user);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task<PhotoUploadResult> UpdateUserProfilePictureAsync(UploadImageDto model)
    {
        var user = await _unitOfWork.UserRepository.GetByIdAsync(model.Id);
        _validation.CheckNotFound(user, "User not found!");
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
            throw new ErrorException(StatusCodes.Status400BadRequest, ApiCodes.NOT_FOUND, uploadResult.ErrorMessage);

        user.ProfilePicture = uploadResult.Url;
        await _unitOfWork.UserRepository.UpdateUserAsync(user);
        await _unitOfWork.SaveChangesAsync();
        return uploadResult;
    }

    public async Task<List<UserReponseDto>> GetAllStaffAsync()
    {
        var listUser = await _unitOfWork.UserRepository.GetAllUserAsync();

        var response = listUser.Where(x => x.Role?.RoleName == "Staff" || x.Role?.RoleName == "Designer").ToList();

        return _mapper.Map<List<UserReponseDto>>(response);
    }

    public async Task<UserReponseDto> UpdateUserAsync(string userId, UpdateUserRequestDto model)
    {
        await _validation.ValidateAndThrowAsync(model);

        var user = await _unitOfWork.UserRepository.GetByIdNotDeletedAsync(userId);
        _validation.CheckNotFound(user, "User not found!");

        if (user.UserName != model.UserName)
        {
            var usernameExists = await _unitOfWork.UserRepository.IsEntityExistsAsync(x =>
                x.UserName == model.UserName && x.Id != userId);
            _validation.CheckBadRequest(usernameExists, "Username already exists!");
        }

        _mapper.Map(model, user);
        user.Salt = HashHelper.GenerateSalt();
        user.HashPassword = HashHelper.HashPassword(model.Password, user.Salt);

        await _unitOfWork.UserRepository.UpdateUserAsync(user);
        await _unitOfWork.SaveChangesAsync();

        return _mapper.Map<UserReponseDto>(user);
    }
}