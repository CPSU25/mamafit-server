using MamaFit.BusinessObjects.DTO.NotificationDto;
using MamaFit.BusinessObjects.Enum;
using MamaFit.Repositories.Infrastructure;

namespace MamaFit.Services.Interface;

public interface INotificationService
{
    Task<PaginatedList<NotificationResponseDto>> GetNotificationsByAccessTokenAsync(string accessToken,
        int index, int pageSize, string? search, NotificationType? type, EntitySortBy? sortBy);
    Task<PaginatedList<NotificationResponseDto>> GetAllNotificationsAsync(int index, int pageSize,
        string? search, NotificationType? type, EntitySortBy? sortBy);
    Task<NotificationResponseDto> GetNotificationByIdAsync(string id);
    Task SendAndSaveNotificationAsync(NotificationRequestDto model);
}