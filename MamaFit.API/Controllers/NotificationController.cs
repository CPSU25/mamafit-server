using MamaFit.BusinessObjects.DTO.NotificationDto;
using MamaFit.BusinessObjects.Enum;
using MamaFit.Repositories.Infrastructure;
using MamaFit.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MamaFit.API.Controllers;

[ApiController]
[Route("api/notification")]
public class NotificationController : ControllerBase
{
    private readonly INotificationService _notificationService;
    public NotificationController(INotificationService notificationService)
    {
        _notificationService = notificationService;
    }
    
    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] int index = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string? search = null,
        [FromQuery] NotificationType ? type = null,
        [FromQuery] EntitySortBy? sortBy = null)
    {
        var result = await _notificationService.GetAllNotificationsAsync(index, pageSize, search, type, sortBy);
        return Ok(new ResponseModel<PaginatedList<NotificationResponseDto>>(
            StatusCodes.Status200OK,
            ApiCodes.SUCCESS,
            result,
            "Get all notifications successfully!"
        ));
    }
    
    [Authorize]
    [HttpGet("by-token")]
    public async Task<IActionResult> GetByAccessToken(
        [FromHeader(Name = "Authorization")] string accessToken,
        [FromQuery] int index = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string? search = null,
        [FromQuery] NotificationType ? type = null,
        [FromQuery] EntitySortBy? sortBy = null)
    {
        var result = await _notificationService.GetNotificationsByAccessTokenAsync(accessToken, index, pageSize, search, type, sortBy);
        return Ok(new ResponseModel<PaginatedList<NotificationResponseDto>>(
            StatusCodes.Status200OK,
            ApiCodes.SUCCESS,
            result,
            "Get notifications by access token successfully!"
        ));
    }

    [Authorize]
    [HttpPut("marks-all-as-read")]
    public async Task<IActionResult> MarkNotificationAsRead()
    {
        await _notificationService.MarkNotificationIsRead();
        return Ok(new ResponseModel<string>(
            StatusCodes.Status200OK,
            ApiCodes.SUCCESS,
            null,
            "Mark all notifications as read successfully!"
        ));
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById([FromRoute] string id)
    {
        var result = await _notificationService.GetNotificationByIdAsync(id);
        return Ok(new ResponseModel<NotificationResponseDto>(
            StatusCodes.Status200OK,
            ApiCodes.SUCCESS,
            result,
            "Get notification successfully!"
        ));
    }
    
    [HttpPost]
    public async Task<IActionResult> SendAndSaveNotification([FromBody] NotificationRequestDto model)
    {
        await _notificationService.SendAndSaveNotificationAsync(model);
        return Ok(new ResponseModel<string>(
            StatusCodes.Status200OK,
            ApiCodes.SUCCESS,
            null,
            "Send and save notification successfully!"
        ));
    }
    
    [HttpPost("send-multiple")]
    public async Task<IActionResult> SendAndSaveNotificationToMultiple([FromBody] NotificationMultipleRequestDto model)
    {
        await _notificationService.SendAndSaveNotificationToMultipleAsync(model);
        return Ok(new ResponseModel<string>(
            StatusCodes.Status200OK,
            ApiCodes.SUCCESS,
            null,
            "Send and save notifications to multiple users successfully!"
        ));
    }
    
    [HttpPost("send-by-role")]
    public async Task<IActionResult> SendAndSaveNotificationByRole([FromBody] NotificationByRoleRequestDto model)
    {
        await _notificationService.SendAndSaveNotificationByRoleAsync(model);
        return Ok(new ResponseModel<string>(
            StatusCodes.Status200OK,
            ApiCodes.SUCCESS,
            null,
            "Send and save notifications by role successfully!"
        ));
    }
}