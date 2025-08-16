using System.Security.Claims;
using AutoMapper;
using MamaFit.BusinessObjects.DTO.UploadImageDto;
using MamaFit.BusinessObjects.DTO.UserDto;
using MamaFit.BusinessObjects.Entity;
using MamaFit.Repositories.Helper;
using MamaFit.Repositories.Implement;
using MamaFit.Repositories.Infrastructure;
using MamaFit.Services.ExternalService.CloudinaryService;
using MamaFit.Services.Interface;
using Microsoft.AspNetCore.Http;

namespace MamaFit.Services.Service;

public class UserService : IUserService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ICloudinaryService _cloudinaryService;
    private readonly IValidationService _validation;

    public UserService(IUnitOfWork unitOfWork, IMapper mapper, ICloudinaryService cloudinaryService,
        IValidationService validation)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _cloudinaryService = cloudinaryService;
        _validation = validation;
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

    public async Task<UserReponseDto> GetUserByIdAsync(string userId)
    {
        var user = await _unitOfWork.UserRepository.GetByIdAsync(userId);
        if (user == null)
            throw new ErrorException(StatusCodes.Status404NotFound,
                ApiCodes.NOT_FOUND, "User not found!");

        return _mapper.Map<UserReponseDto>(user);
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
    
        var role = await _unitOfWork.RoleRepository.GetByIdAsync(model.RoleId);
        _validation.CheckNotFound(role, "Role not found!");
    
        _mapper.Map(model, user);
    
        await _unitOfWork.UserRepository.UpdateUserAsync(user);
        await _unitOfWork.SaveChangesAsync();

        return _mapper.Map<UserReponseDto>(user);
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

}