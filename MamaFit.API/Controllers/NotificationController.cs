using MamaFit.BusinessObjects.DTO.NotificationDto;
using MamaFit.Repositories.Infrastructure;
using MamaFit.Services.Interface;
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
        [FromQuery] string? search = null)
    {
        var result = await _notificationService.GetAllNotificationsAsync(index, pageSize, search);
        return Ok(new ResponseModel<PaginatedList<NotificationResponseDto>>(
            StatusCodes.Status200OK,
            ApiCodes.SUCCESS,
            result,
            "Get all notifications successfully!"
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
}