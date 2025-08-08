using AutoMapper;
using MamaFit.BusinessObjects.DTO.NotificationDto;
using MamaFit.BusinessObjects.DTO.RealtimeDto;
using MamaFit.BusinessObjects.Entity;
using MamaFit.BusinessObjects.Enum;
using MamaFit.Repositories.Helper;
using MamaFit.Repositories.Implement;
using MamaFit.Repositories.Infrastructure;
using MamaFit.Services.ExternalService.ExpoNotification;
using MamaFit.Services.ExternalService.SignalR;
using MamaFit.Services.Hubs;
using MamaFit.Services.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using Microsoft.VisualBasic;

namespace MamaFit.Services.Service;

public class NotificationService : INotificationService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IValidationService _validation;
    private readonly IExpoNotificationService _expoNotificationService;
    private readonly IHubContext<UnifiedHub> _notificationHubContext; // Changed to UnifiedHub
    private readonly IUserConnectionManager _userConnectionManager;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IRealtimeEventService _realtimeEventService;

    public NotificationService(IUnitOfWork unitOfWork, IMapper mapper, IValidationService validation,
        IExpoNotificationService expoNotificationService, IHubContext<UnifiedHub> notificationHubContext, // Changed to UnifiedHub
        IUserConnectionManager userConnectionManager, IHttpContextAccessor httpContextAccessor,
        IRealtimeEventService realtimeEventService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _validation = validation;
        _expoNotificationService = expoNotificationService;
        _notificationHubContext = notificationHubContext;
        _userConnectionManager = userConnectionManager;
        _httpContextAccessor = httpContextAccessor;
        _realtimeEventService = realtimeEventService;
    }

    public async Task<PaginatedList<NotificationResponseDto>> GetNotificationsByAccessTokenAsync(string accessToken,
        int index = 1, int pageSize = 10,  string? search = null, NotificationType? type = null, EntitySortBy? sortBy = null)
    {
        var userId = JwtTokenHelper.ExtractUserId(accessToken);

        var notifications = await _unitOfWork.NotificationRepository.GetAllByTokenAsync(userId, index, pageSize, search, type, sortBy);
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

    public async Task<PaginatedList<NotificationResponseDto>> GetAllNotificationsAsync(int index = 1, int pageSize = 10,
        string? search = null, NotificationType? type = null, EntitySortBy? sortBy = null)
    {
        var notifications = await _unitOfWork.NotificationRepository.GetAllAsync(index, pageSize, search, type, sortBy);
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

    public async Task MarkNotificationIsRead()
    {
        var currentUserId = GetCurrentUserId();
        _validation.CheckNotFound(currentUserId, "Please sign in");

        var notificationList = await _unitOfWork.NotificationRepository.GetAllByUserId(currentUserId);
        foreach(var notification in notificationList)
        {
            notification.IsRead = true;
            await _unitOfWork.NotificationRepository.UpdateAsync(notification);
        }

        await _unitOfWork.SaveChangesAsync();
    }

    public async Task SendAndSaveNotificationAsync(NotificationRequestDto model)
    {
        await _validation.ValidateAndThrowAsync(model);
        var user = await _unitOfWork.UserRepository.GetByIdAsync(model.ReceiverId);
        _validation.CheckNotFound(user, "User not found");

        var notification = _mapper.Map<Notification>(model);
        notification.IsRead = false;

        await _unitOfWork.NotificationRepository.InsertAsync(notification);
        await _unitOfWork.SaveChangesAsync();

        var connections = await _userConnectionManager.GetUserConnectionsAsync(model.ReceiverId);
        var notificationDto = _mapper.Map<NotificationResponseDto>(notification);
        
        // Publish realtime notification event
        await _realtimeEventService.PublishNotificationCreatedAsync(new NotificationEventDto
        {
            EventType = RealtimeEventTypes.NOTIFICATION_CREATED,
            EntityId = notification.Id,
            EntityType = RealtimeEntityTypes.NOTIFICATION,
            Data = notificationDto,
            TargetUserId = model.ReceiverId,
            NotificationId = notification.Id,
            Title = model.NotificationTitle,
            Content = model.NotificationContent,
            NotificationType = model.Type ?? NotificationType.ORDER_PROGRESS,
            ActionUrl = model.ActionUrl,
            Metadata = model.Metadata?.ToDictionary(x => x.Key, x => (object)x.Value) ?? new Dictionary<string, object>()
        });
        
        if (connections != null && connections.Count > 0)
        {
            // Send to individual user connections
            foreach (var connId in connections)
            {
                await _notificationHubContext.Clients.Client(connId)
                    .SendAsync("ReceiveNotification", notificationDto);
            }
            
            // Also send to notifications_all group for backward compatibility
            await _notificationHubContext.Clients.Group("notifications_all")
                .SendAsync("ReceiveNotification", notificationDto);
                
            // Send to user's personal group
            var userGroup = $"user_{model.ReceiverId}";
            await _notificationHubContext.Clients.Group(userGroup)
                .SendAsync("ReceiveNotification", notificationDto);
        }
        else
        {
            var token = await _unitOfWork.TokenRepository.GetNotificationTokensAsync(model.ReceiverId);

            if (token != null && !string.IsNullOrEmpty(token.Token))
            {
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
    }

    private string GetCurrentUserId()
    {
        return _httpContextAccessor?.HttpContext?.User?.FindFirst("userId")?.Value;
    }
}