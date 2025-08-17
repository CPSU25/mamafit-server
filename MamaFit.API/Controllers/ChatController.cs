using MamaFit.Services.Hubs;
using MamaFit.BusinessObjects.DTO.ChatMessageDto;
using MamaFit.BusinessObjects.DTO.ChatRoomDto;
using MamaFit.BusinessObjects.Enum;
using MamaFit.Repositories.Infrastructure;
using MamaFit.Services.Hubs;
using MamaFit.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace MamaFit.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ChatController : ControllerBase
    {
        private readonly IChatService _chatService;
        private readonly IHubContext<ChatHub> _hubContext;
        private readonly ILogger<ChatController> _logger;

        public ChatController(IChatService chatService, IHubContext<ChatHub> hubContext, ILogger<ChatController> logger)
        {
            _chatService = chatService;
            _hubContext = hubContext;
            _logger = logger;
        }

        [HttpPost("messages")]
        public async Task<IActionResult> CreateMessage([FromBody] ChatMessageCreateDto messageDto)
        {
            try
            {
                var userId = User.FindFirst("userId")?.Value;
                if (messageDto.SenderId != userId)
                {
                    return Forbid("You can only send messages as yourself");
                }

                var response = await _chatService.CreateChatMessageAsync(messageDto);
                var message = await _chatService.GetChatMessageById(response.Id);

                await _hubContext.Clients.Group(message.ChatRoomId).SendAsync("ReceiveMessage", message);

                return Ok(new ResponseModel<ChatMessageResponseDto>(StatusCodes.Status200OK, ApiCodes.SUCCESS, message, "Message sent successfully"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating message");
                return BadRequest(new ResponseModel<string>(StatusCodes.Status400BadRequest, ApiCodes.BAD_REQUEST, null, "Failed to create message"));
            }
        }

        [HttpGet("rooms/{roomId}/messages")]
        public async Task<IActionResult> GetChatHistory(string roomId, [FromQuery] int index = 1, [FromQuery] int pageSize = 20)
        {
            try
            {
                var chatRoom = await _chatService.GetChatRoomById(roomId);
                if (chatRoom == null)
                {
                    return NotFound(new ResponseModel<string>(StatusCodes.Status404NotFound, ApiCodes.NOT_FOUND, null, "Chat room not found"));
                }

                var messages = await _chatService.GetChatHistoryAsync(roomId, index, pageSize);
                return Ok(new ResponseModel<List<ChatMessageResponseDto>>(StatusCodes.Status200OK, ApiCodes.SUCCESS, messages, "Get chat history successfully"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error getting chat history for room {roomId}");
                return BadRequest(new ResponseModel<string>(StatusCodes.Status400BadRequest, ApiCodes.BAD_REQUEST, null, "Failed to get chat history"));
            }
        }

        [HttpPost("rooms")]
        public async Task<IActionResult> CreateChatRoom([FromBody] CreateChatRoomDto createDto)
        {
            try
            {
                var currentUserId = User.FindFirst("userId")?.Value;
                if (createDto.UserId1 != currentUserId && createDto.UserId2 != currentUserId)
                {
                    return Forbid("You can only create rooms that include yourself");
                }

                var room = await _chatService.CreateChatRoomAsync(createDto.UserId1, createDto.UserId2);

                if (createDto.UserId2 != currentUserId)
                {
                    await _hubContext.Clients.User(createDto.UserId2)
                        .SendAsync("InvitedToRoom", room.Id);
                }

                return Ok(new ResponseModel<string>(StatusCodes.Status200OK, ApiCodes.SUCCESS, room.Id, "Chat room created successfully"));
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseModel<string>(StatusCodes.Status400BadRequest, ApiCodes.BAD_REQUEST, null, "Failed to create chat room"));
            }
        }

        [HttpGet("users/{userId}/rooms")]
        public async Task<IActionResult> GetUserChatRooms(string userId)
        {
            try
            {
                var currentUserId = User.FindFirst("userId")?.Value;
                if (userId != currentUserId)
                {
                    return Forbid("You can only access your own chat rooms");
                }

                var rooms = await _chatService.GetUserChatRoom(userId);
                return Ok(new ResponseModel<List<ChatRoomResponseDto>>(StatusCodes.Status200OK, ApiCodes.SUCCESS, rooms, "Get all rooms successfully"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error getting chat rooms for user {userId}");
                return BadRequest(new ResponseModel<string>(StatusCodes.Status400BadRequest, ApiCodes.BAD_REQUEST, null, "Failed to get chat rooms"));
            }
        }

        [HttpGet("my-rooms")]
        public async Task<IActionResult> GetMyRooms()
        {
            try
            {
                var userId = User.FindFirst("userId")?.Value;
                var rooms = await _chatService.GetUserChatRoom(userId);
                return Ok(new ResponseModel<List<ChatRoomResponseDto>>(StatusCodes.Status200OK, ApiCodes.SUCCESS, rooms, "Get all rooms successfully"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting current user's chat rooms");
                return BadRequest(new ResponseModel<string>(StatusCodes.Status400BadRequest, ApiCodes.BAD_REQUEST, null, "Failed to get chat rooms"));
            }
        }

        [HttpGet("rooms/{roomId}")]
        public async Task<IActionResult> GetChatRoom(string roomId)
        {
            try
            {
                var chatRoom = await _chatService.GetChatRoomById(roomId);
                if (chatRoom == null)
                {
                    return NotFound(new ResponseModel<string>(StatusCodes.Status404NotFound, ApiCodes.NOT_FOUND, null, "Chat room not found"));
                }

                return Ok(new ResponseModel<ChatRoomResponseDto>(StatusCodes.Status200OK, ApiCodes.SUCCESS, chatRoom, "Get room successfully"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error getting chat room {roomId}");
                return BadRequest(new ResponseModel<string>(StatusCodes.Status400BadRequest, ApiCodes.BAD_REQUEST, null, "Failed to get chat room"));
            }
        }

        [HttpGet("messages/{messageId}")]
        public async Task<IActionResult> GetMessage(string messageId)
        {
            try
            {
                var message = await _chatService.GetChatMessageById(messageId);
                if (message == null)
                {
                    return NotFound(new ResponseModel<string>(StatusCodes.Status404NotFound, ApiCodes.NOT_FOUND, null, "Message not found"));
                }

                return Ok(new ResponseModel<ChatMessageResponseDto>(StatusCodes.Status200OK, ApiCodes.SUCCESS, message, "Get message successfully"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error getting message {messageId}");
                return BadRequest(new ResponseModel<string>(StatusCodes.Status400BadRequest, ApiCodes.BAD_REQUEST, null, "Failed to get message"));
            }
        }

        [HttpGet("health")]
        public IActionResult HealthCheck()
        {
            var health = new
            {
                status = "healthy",
                timestamp = DateTime.UtcNow,
                user = User.FindFirst("userId")?.Value
            };

            return Ok(new ResponseModel<object>(StatusCodes.Status200OK, ApiCodes.SUCCESS, health, "Health check success"));
        }

        [HttpGet("rooms/{roomId}/online-users")]
        public async Task<IActionResult> GetOnlineUsers(string roomId)
        {
            try
            {
                return Ok(new ResponseModel<object>(StatusCodes.Status200OK, ApiCodes.SUCCESS, new
                {
                    roomId,
                    onlineUsers = new List<string>(),
                    message = "Online users tracking via SignalR"
                }, "Get online users successfully"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error getting online users for room {roomId}");
                return BadRequest(new ResponseModel<string>(StatusCodes.Status400BadRequest, ApiCodes.BAD_REQUEST, null, "Failed to get online users"));
            }
        }


        [HttpPost("rooms/{roomId}/system-message")]
        public async Task<IActionResult> SendSystemMessage(string roomId, [FromBody] SystemMessageDto systemMessage)
        {
            try
            {
                var messageDto = new ChatMessageCreateDto
                {
                    ChatRoomId = roomId,
                    SenderId = "system",
                    Message = systemMessage.Content,
                    Type = MessageType.Text,
                };

                var response = await _chatService.CreateChatMessageAsync(messageDto);
                var message = await _chatService.GetChatMessageById(response.Id);

                await _hubContext.Clients.Group($"room_{roomId}").SendAsync("ReceiveMessage", message);

                return Ok(new ResponseModel<ChatMessageResponseDto>(StatusCodes.Status200OK, ApiCodes.SUCCESS, message, "System message sent successfully"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error sending system message to room {roomId}");
                return BadRequest(new ResponseModel<string>(StatusCodes.Status400BadRequest, ApiCodes.BAD_REQUEST, null, "Failed to send system message"));
            }
        }

        [HttpPost("users/{userId}/notify")]
        public async Task<IActionResult> NotifyUser(string userId, [FromBody] NotificationDto notification)
        {
            try
            {
                await _hubContext.Clients.User(userId).SendAsync("ReceiveNotification", notification);

                return Ok(new ResponseModel<bool>(StatusCodes.Status200OK, ApiCodes.SUCCESS, true, "Notification sent successfully"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error sending notification to user {userId}");
                return BadRequest(new ResponseModel<string>(StatusCodes.Status400BadRequest, ApiCodes.BAD_REQUEST, null, "Failed to send notification"));
            }
        }
    }

    public class CreateChatRoomDto
    {
        public string UserId1 { get; set; }
        public string UserId2 { get; set; }
    }

    public class SystemMessageDto
    {
        public string Content { get; set; }
    }

    public class NotificationDto
    {
        public string Title { get; set; }
        public string Message { get; set; }
        public string Type { get; set; }
        public Dictionary<string, object> Data { get; set; } = new();
    }
}
