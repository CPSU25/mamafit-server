using MamaFit.BusinessObjects.DTO.NotificationDto;
using MamaFit.Repositories.Infrastructure;

namespace MamaFit.Services.Interface;

public interface INotificationService
{
    
    Task<PaginatedList<NotificationResponseDto>> GetAllNotificationsAsync(int index, int pageSize,
        string? search);
    Task<NotificationResponseDto> GetNotificationByIdAsync(string id);
    Task SendAndSaveNotificationAsync(NotificationRequestDto model);
}