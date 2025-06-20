using AutoMapper;
using MamaFit.BusinessObjects.DTO.NotificationDto;
using MamaFit.BusinessObjects.Entity;
using MamaFit.BusinessObjects.Enum;
using MamaFit.Repositories.Implement;
using MamaFit.Repositories.Infrastructure;
using MamaFit.Services.ExternalService.ExpoNotification;
using MamaFit.Services.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.VisualBasic;

namespace MamaFit.Services.Service;

public class NotificationService : INotificationService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IValidationService _validation;
    private readonly IExpoNotificationService _expoNotificationService;

    public NotificationService(IUnitOfWork unitOfWork, IMapper mapper, IValidationService validation,
        IExpoNotificationService expoNotificationService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _validation = validation;
        _expoNotificationService = expoNotificationService;
    }

    public async Task<PaginatedList<NotificationResponseDto>> GetAllNotificationsAsync(int index = 1, int pageSize = 10,
        string? search = null)
    {
        var notifications = await _unitOfWork.NotificationRepository.GetAllAsync(index, pageSize, search);
        var responseItems = notifications.Items
            .Select(notification => _mapper.Map<NotificationResponseDto>(notification))
            .ToList();
        return new PaginatedList<NotificationResponseDto>(
            responseItems,
            notifications.TotalCount,
            notifications.PageNumber,
            pageSize
        );
    }

    public async Task<NotificationResponseDto?> GetNotificationByIdAsync(string id)
    {
        var notification = await _unitOfWork.NotificationRepository.GetByIdNotDeletedAsync(id);
        _validation.CheckNotFound(notification, "Notification not found");
        return _mapper.Map<NotificationResponseDto>(notification);
    }

    public async Task SendAndSaveNotificationAsync(NotificationRequestDto model)
    {
        await _validation.ValidateAndThrowAsync(model);
        var user = await _unitOfWork.UserRepository.GetByIdAsync(model.ReceiverId);
        _validation.CheckNotFound(user, "User not found");

        var token = await _unitOfWork.TokenRepository.GetNotificationTokensAsync(model.ReceiverId);
        _validation.CheckNotFound(token, "No notification tokens found for the user");
        var notification = _mapper.Map<Notification>(model);
        notification.IsRead = false;

        await _unitOfWork.NotificationRepository.InsertAsync(notification);
        await _unitOfWork.SaveChangesAsync();

        var expoResponse = await _expoNotificationService.SendPushAsync(
            token.Token,
            model.NotificationTitle,
            model.NotificationContent
        );

        if (!expoResponse.IsSuccessStatusCode)
        {
            throw new ErrorException(StatusCodes.Status500InternalServerError, ApiCodes.INTERNAL_SERVER_ERROR,
                "Failed to send notification via Expo");
        }
    }
}