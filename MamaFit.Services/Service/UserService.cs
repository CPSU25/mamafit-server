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
    private readonly ICloudinaryService _cloudinaryService;

    public UserService(IUnitOfWork unitOfWork, IMapper mapper, ICloudinaryService cloudinaryService)
    {
        _cloudinaryService = cloudinaryService;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
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
                ErrorCode.NotFound, "User not found!");

        return _mapper.Map<UserReponseDto>(user);
    }

    public async Task<UserReponseDto> UpdateUserAsync(string userId, UpdateUserRequestDto model)
    {
        var userRepo = await _unitOfWork.UserRepository.GetByIdAsync(userId);
        if (userRepo == null)
            throw new ErrorException(StatusCodes.Status404NotFound,
                ErrorCode.NotFound, "User not found!");

        var user = _mapper.Map<ApplicationUser>(model);


        await _unitOfWork.UserRepository.UpdateUserAsync(user);
        await _unitOfWork.SaveChangesAsync();

        return _mapper.Map<UserReponseDto>(user);
    }

    public async Task DeleteUserAsync(string userId)
    {
        var user = await _unitOfWork.UserRepository.GetByIdAsync(userId);

        if (user == null)
            throw new ErrorException(StatusCodes.Status404NotFound,
                ErrorCode.NotFound, "User not found!");

        await _unitOfWork.UserRepository.DeleteUserAsync(user);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task<PhotoUploadResult> UpdateUserProfilePictureAsync(UploadImageDto model)
    {
        var user = await _unitOfWork.UserRepository.GetByIdAsync(model.Id);
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
            throw new ErrorException(StatusCodes.Status400BadRequest, ErrorCode.BadRequest, uploadResult.ErrorMessage);

        user.ProfilePicture = uploadResult.Url;
        await _unitOfWork.UserRepository.UpdateUserAsync(user);
        await _unitOfWork.SaveChangesAsync();
        return uploadResult;
    }

}