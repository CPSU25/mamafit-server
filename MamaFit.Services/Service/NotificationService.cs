using AutoMapper;
using MamaFit.BusinessObjects.DTO.NotificationDto;
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
    private readonly IHubContext<NotificationHub> _notificationHubContext;
    private readonly IUserConnectionManager _userConnectionManager;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public NotificationService(IUnitOfWork unitOfWork, IMapper mapper, IValidationService validation,
        IExpoNotificationService expoNotificationService, IHubContext<NotificationHub> notificationHubContext,
        IUserConnectionManager userConnectionManager, IHttpContextAccessor httpContextAccessor)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _validation = validation;
        _expoNotificationService = expoNotificationService;
        _notificationHubContext = notificationHubContext;
        _userConnectionManager = userConnectionManager;
        _httpContextAccessor = httpContextAccessor;
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

        await SendRealTimeNotificationAsync(notification);
    }

    public async Task SendAndSaveNotificationToMultipleAsync(NotificationMultipleRequestDto model)
    {
        await _validation.ValidateAndThrowAsync(model);
        
        if (model.ReceiverIds == null || !model.ReceiverIds.Any())
        {
            throw new ErrorException(StatusCodes.Status400BadRequest, ApiCodes.BAD_REQUEST, "ReceiverIds cannot be empty");
        }

        // Validate all users exist
        var users = new List<ApplicationUser>();
        foreach (var receiverId in model.ReceiverIds)
        {
            var user = await _unitOfWork.UserRepository.GetByIdAsync(receiverId);
            _validation.CheckNotFound(user, $"User with ID {receiverId} not found");
            if (user != null)
                users.Add(user);
        }

        // Create notifications for each user
        var notifications = new List<Notification>();
        foreach (var receiverId in model.ReceiverIds)
        {
            var notification = new Notification
            {
                Id = Guid.NewGuid().ToString(),
                ReceiverId = receiverId,
                NotificationTitle = model.NotificationTitle,
                NotificationContent = model.NotificationContent,
                Type = model.Type,
                ActionUrl = model.ActionUrl,
                Metadata = model.Metadata != null ? System.Text.Json.JsonSerializer.Serialize(model.Metadata) : null,
                IsRead = false,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                IsDeleted = false
            };
            
            notifications.Add(notification);
            await _unitOfWork.NotificationRepository.InsertAsync(notification);
        }
        
        await _unitOfWork.SaveChangesAsync();

        // Send real-time notifications
        foreach (var notification in notifications)
        {
            await SendRealTimeNotificationAsync(notification);
        }
    }

    public async Task SendAndSaveNotificationByRoleAsync(NotificationByRoleRequestDto model)
    {
        await _validation.ValidateAndThrowAsync(model);
        
        if (model.RoleIds == null || !model.RoleIds.Any())
        {
            throw new ErrorException(StatusCodes.Status400BadRequest, ApiCodes.BAD_REQUEST, "RoleIds cannot be empty");
        }

        // Validate all roles exist
        var roles = new List<ApplicationUserRole>();
        foreach (var roleId in model.RoleIds)
        {
            var role = await _unitOfWork.RoleRepository.GetByIdAsync(roleId);
            _validation.CheckNotFound(role, $"Role with ID {roleId} not found");
            if (role != null)
                roles.Add(role);
        }

        // Get all users with specified roles
        var userIds = new List<string>();
        foreach (var roleId in model.RoleIds)
        {
            var usersInRole = await _unitOfWork.UserRepository.GetUsersByRoleIdAsync(roleId, model.OnlyActiveUsers);
            userIds.AddRange(usersInRole.Select(u => u.Id));
        }

        // Remove duplicates
        userIds = userIds.Distinct().ToList();

        if (!userIds.Any())
        {
            throw new ErrorException(StatusCodes.Status400BadRequest, ApiCodes.BAD_REQUEST, "No users found with the specified roles");
        }

        // Create notifications for each user
        var notifications = new List<Notification>();
        foreach (var userId in userIds)
        {
            var notification = new Notification
            {
                Id = Guid.NewGuid().ToString(),
                ReceiverId = userId,
                NotificationTitle = model.NotificationTitle,
                NotificationContent = model.NotificationContent,
                Type = model.Type,
                ActionUrl = model.ActionUrl,
                Metadata = model.Metadata != null ? System.Text.Json.JsonSerializer.Serialize(model.Metadata) : null,
                IsRead = false,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                IsDeleted = false
            };
            
            notifications.Add(notification);
            await _unitOfWork.NotificationRepository.InsertAsync(notification);
        }
        
        await _unitOfWork.SaveChangesAsync();

        // Send real-time notifications
        foreach (var notification in notifications)
        {
            await SendRealTimeNotificationAsync(notification);
        }
    }

    private async Task SendRealTimeNotificationAsync(Notification notification)
    {
        if (string.IsNullOrEmpty(notification.ReceiverId))
            return;

        var connections = await _userConnectionManager.GetUserConnectionsAsync(notification.ReceiverId);
        var notificationDto = _mapper.Map<NotificationResponseDto>(notification);
        
        if (connections != null && connections.Count > 0)
        {
            foreach (var connId in connections)
            {
                await _notificationHubContext.Clients.Client(connId)
                    .SendAsync("ReceiveNotification", notificationDto);
            }
        }
        else
        {
            var token = await _unitOfWork.TokenRepository.GetNotificationTokensAsync(notification.ReceiverId);

            if (token != null && !string.IsNullOrEmpty(token.Token))
            {
                var title = notification.NotificationTitle ?? "Notification";
                var content = notification.NotificationContent ?? "";
                
                var expoResponse = await _expoNotificationService.SendPushAsync(
                    token.Token,
                    title,
                    content
                );

                if (!expoResponse.IsSuccessStatusCode)
                {
                    // Log error but don't throw to avoid breaking batch operation
                    Console.WriteLine($"Failed to send push notification to user {notification.ReceiverId}: {expoResponse.StatusCode}");
                }
            }
        }
    }

    private string GetCurrentUserId()
    {
        return _httpContextAccessor?.HttpContext?.User?.FindFirst("userId")?.Value ?? string.Empty;
    }
}