using MamaFit.BusinessObjects.DTO.ChatMessageDto;
using MamaFit.Services.Interface;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace MamaFit.API.Middlewares
{
    public class ChatHub : Hub
    {
        private readonly IChatService _chatService;
        private readonly ILogger<ChatHub> _logger;

        // Static dictionaries to track connections
        private static readonly Dictionary<string, string> _userConnections = new();
        private static readonly Dictionary<string, HashSet<string>> _roomConnections = new();
        private static readonly Dictionary<string, Dictionary<string, bool>> _typingUsers = new();

        public ChatHub(IChatService chatService, ILogger<ChatHub> logger)
        {
            _chatService = chatService;
            _logger = logger;
        }

        public override async Task OnConnectedAsync()
        {
            var userId = GetCurrentUserId();
            if (!string.IsNullOrEmpty(userId))
            {
                _userConnections[userId] = Context.ConnectionId;

                // Get user's chat rooms and join them
                var userRooms = await _chatService.GetUserChatRoom(userId);
                foreach (var room in userRooms)
                {
                    await Groups.AddToGroupAsync(Context.ConnectionId, room.Id);

                    // Track room connections
                    if (!_roomConnections.ContainsKey(room.Id))
                        _roomConnections[room.Id] = new HashSet<string>();
                    _roomConnections[room.Id].Add(userId);
                }

                // Notify others that user is online
                await Clients.Others.SendAsync("UserOnline", userId);
                _logger.LogInformation($"User {userId} connected with ConnectionId {Context.ConnectionId}");
            }

            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            var userId = GetCurrentUserId();
            if (!string.IsNullOrEmpty(userId))
            {
                _userConnections.Remove(userId);

                // Remove user from all room connections
                foreach (var roomId in _roomConnections.Keys.ToList())
                {
                    _roomConnections[roomId].Remove(userId);
                    if (!_roomConnections[roomId].Any())
                        _roomConnections.Remove(roomId);

                    // Clear typing status
                    if (_typingUsers.ContainsKey(roomId))
                    {
                        _typingUsers[roomId].Remove(userId);
                        if (!_typingUsers[roomId].Any())
                            _typingUsers.Remove(roomId);
                    }
                }

                // Notify others that user is offline
                await Clients.Others.SendAsync("UserOffline", userId);
                _logger.LogInformation($"User {userId} disconnected");
            }

            await base.OnDisconnectedAsync(exception);
        }

        public async Task JoinRoom(string roomId)
        {
            var userId = GetCurrentUserId();

            if (string.IsNullOrEmpty(userId))
            {
                await Clients.Caller.SendAsync("Error", "Invalid user or room");
                return;
            }

            await Groups.AddToGroupAsync(Context.ConnectionId, roomId);
            _logger.LogInformation($"User {userId} joined room {roomId}");

            // Confirm join
            await Clients.Caller.SendAsync("JoinedRoom", roomId);
        }

        

        public async Task LeaveRoom(string roomId)
        {
            var userId = GetCurrentUserId();
            if (string.IsNullOrEmpty(userId)) return;

            await Groups.RemoveFromGroupAsync(Context.ConnectionId, roomId);

            if (_roomConnections.ContainsKey(roomId))
            {
                _roomConnections[roomId].Remove(userId);
                if (!_roomConnections[roomId].Any())
                    _roomConnections.Remove(roomId);
            }

            if (_typingUsers.ContainsKey(roomId))
            {
                _typingUsers[roomId].Remove(userId);
                await Clients.Group(roomId).SendAsync("UserStoppedTyping", userId);
            }

            await Clients.Group(roomId).SendAsync("UserLeftRoom", userId, roomId);
        }

        public async Task SendMessage(ChatMessageCreateDto messageDto)
        {
            var userId = GetCurrentUserId(); 

            _logger.LogInformation($"SendMessage called by user {userId} in room {messageDto.ChatRoomId}");

            // Validate user authentication
            if (string.IsNullOrEmpty(userId))
            {
                _logger.LogWarning("SendMessage: User not authenticated");
                await Clients.Caller.SendAsync("Error", "Unauthorized: User not authenticated");
                return;
            }

            // Validate input
            if (string.IsNullOrEmpty(messageDto.ChatRoomId))
            {
                _logger.LogWarning($"SendMessage: Invalid ChatRoomId for user {userId}");
                await Clients.Caller.SendAsync("Error", "Invalid room ID");
                return;
            }

            if (string.IsNullOrEmpty(messageDto.Message?.Trim()))
            {
                _logger.LogWarning($"SendMessage: Empty message from user {userId}");
                await Clients.Caller.SendAsync("Error", "Message cannot be empty");
                return;
            }

            try
            {
                // Set the SenderId from authenticated user (security measure)
                messageDto.SenderId = userId;

                _logger.LogInformation(
                    $"Creating message: User={userId}, Room={messageDto.ChatRoomId}, Message='{messageDto.Message}'");

                // Create message in database
                var response = await _chatService.CreateChatMessageAsync(messageDto);

                if (response == null)
                {
                    _logger.LogError($"Failed to create message in database for user {userId}");
                    await Clients.Caller.SendAsync("Error", "Failed to save message");
                    return;
                }

                _logger.LogInformation($"Message created with ID: {response.Id}");

                // Get the complete message object for broadcasting
                var message = await _chatService.GetChatMessageById(response.Id);

                if (message == null)
                {
                    _logger.LogError($"Failed to retrieve created message {response.Id}");
                    await Clients.Caller.SendAsync("Error", "Failed to retrieve message");
                    return;
                }

                _logger.LogInformation(
                    $"Broadcasting ReceiveMessage to room {messageDto.ChatRoomId}: MessageId={message.Id}, SenderId={message.SenderId}, SenderName={message.SenderName}");

                // Broadcast message to all users in the room (including sender for confirmation)
                await Clients.Group(messageDto.ChatRoomId).SendAsync("ReceiveMessage", new
                {
                    id = message.Id,
                    message = message.Message,
                    senderId = message.SenderId,
                    senderName = message.SenderName,
                    senderAvatar = message.SenderAvatar,
                    chatRoomId = message.ChatRoomId,
                    type = message.Type,
                    messageTimestamp = message.MessageTimestamp.ToString("O") // For backward compatibility
                });

                _logger.LogInformation(
                    $"✅ Message {message.Id} successfully broadcasted to room {messageDto.ChatRoomId}");

                // Optional: Send confirmation to sender only
                await Clients.Caller.SendAsync("MessageSent", new
                {
                    tempId = messageDto.SenderId, // If you're using temp IDs on client
                    messageId = message.Id,
                    status = "success"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    $"❌ Error sending message for user {userId} in room {messageDto.ChatRoomId}: {ex.Message}");
                await Clients.Caller.SendAsync("Error", $"Failed to send message: {ex.Message}");
            }
        }

        public async Task MarkAsRead(string messageId, string chatRoomId)
        {
            var userId = GetCurrentUserId();
            if (string.IsNullOrEmpty(userId))
            {
                await Clients.Caller.SendAsync("Error", "Unauthorized");
                return;
            }

            try
            {
                await _chatService.MarkMessageAsReadAsync(messageId, userId, chatRoomId);
                _logger.LogInformation($"Message {messageId} marked as read by user {userId} in room {chatRoomId}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error marking message {messageId} as read for user {userId}");
                await Clients.Caller.SendAsync("Error", "Failed to mark message as read");
            }
        }

        public async Task UpdateTypingStatus(string roomId, bool isTyping)
        {
            var userId = Context.User?.FindFirst("userId")?.Value;
            if (string.IsNullOrEmpty(userId)) return;

            try
            {
                if (!_typingUsers.ContainsKey(roomId))
                    _typingUsers[roomId] = new Dictionary<string, bool>();

                if (isTyping)
                {
                    _typingUsers[roomId][userId] = true;
                    await Clients.GroupExcept(roomId, Context.ConnectionId)
                        .SendAsync("UserStartedTyping", userId);
                }
                else
                {
                    _typingUsers[roomId].Remove(userId);
                    await Clients.GroupExcept(roomId, Context.ConnectionId)
                        .SendAsync("UserStoppedTyping", userId);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error updating typing status for user {userId} in room {roomId}");
            }
        }

        public async Task GetOnlineUsers(string roomId)
        {
            try
            {
                if (_roomConnections.ContainsKey(roomId))
                {
                    var onlineUsers = _roomConnections[roomId].ToList();
                    await Clients.Caller.SendAsync("OnlineUsers", roomId, onlineUsers);
                }
                else
                {
                    await Clients.Caller.SendAsync("OnlineUsers", roomId, new List<string>());
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error getting online users for room {roomId}");
            }
        }

        public async Task GetTypingUsers(string roomId)
        {
            try
            {
                if (_typingUsers.ContainsKey(roomId))
                {
                    var typingUsers = _typingUsers[roomId].Where(kv => kv.Value).Select(kv => kv.Key).ToList();
                    await Clients.Caller.SendAsync("TypingUsers", roomId, typingUsers);
                }
                else
                {
                    await Clients.Caller.SendAsync("TypingUsers", roomId, new List<string>());
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error getting typing users for room {roomId}");
            }
        }

        public async Task SendPrivateMessage(string targetUserId, string message)
        {
            var senderId = GetCurrentUserId();
            if (string.IsNullOrEmpty(senderId)) return;

            try
            {
                if (_userConnections.ContainsKey(targetUserId))
                {
                    await Clients.Client(_userConnections[targetUserId])
                        .SendAsync("ReceivePrivateMessage", senderId, message);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error sending private message from {senderId} to {targetUserId}");
            }
        }

        // Debug method to check available claims
        
        private string? GetCurrentUserId()
        {
            // Try multiple claim types to ensure compatibility
            var userId = Context.User?.FindFirst("userId")?.Value ??
                        Context.User?.FindFirst("sub")?.Value ??
                        Context.User?.FindFirst("nameid")?.Value ??
                        Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            
            _logger.LogInformation($"GetCurrentUserId: Retrieved userId={userId} for ConnectionId={Context.ConnectionId}");
            return userId;
        }
    }
}