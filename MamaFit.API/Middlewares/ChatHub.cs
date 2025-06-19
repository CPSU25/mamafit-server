using MamaFit.BusinessObjects.DTO.ChatMessageDto;
using MamaFit.Services.Interface;
using Microsoft.AspNetCore.SignalR;

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
            var userId = Context.User?.FindFirst("userId")?.Value;
            if (!string.IsNullOrEmpty(userId))
            {
                _userConnections[userId] = Context.ConnectionId;

                // Get user's chat rooms and join them
                var userRooms = await _chatService.GetUserChatRoom(userId);
                foreach (var room in userRooms)
                {
                    await Groups.AddToGroupAsync(Context.ConnectionId, $"room_{room.Id}");

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

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            var userId = Context.User?.FindFirst("userId")?.Value;
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
            var userId = Context.User?.FindFirst("userId")?.Value;
            if (string.IsNullOrEmpty(userId)) return;

            try
            {
                var chatRoom = await _chatService.GetChatRoomById(roomId);
                if (chatRoom == null) return;

                await Groups.AddToGroupAsync(Context.ConnectionId, roomId);

                if (!_roomConnections.ContainsKey(roomId))
                    _roomConnections[roomId] = new HashSet<string>();
                _roomConnections[roomId].Add(userId);

                await Clients.Group(roomId).SendAsync("UserJoinedRoom", userId, roomId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error joining room {roomId} for user {userId}");
            }
        }

        public async Task LeaveRoom(string roomId)
        {
            var userId = Context.User?.FindFirst("userId")?.Value;
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
            var userId = Context.User?.FindFirst("userId")?.Value;
            if (string.IsNullOrEmpty(userId) || messageDto.SenderId != userId)
            {
                await Clients.Caller.SendAsync("Error", "Unauthorized");
                return;
            }

            try
            {
                var response = await _chatService.CreateChatMessageAsync(messageDto);

                await Clients.Group(messageDto.ChatRoomId).SendAsync("ReceiveMessage", response);

                if (_typingUsers.ContainsKey(messageDto.ChatRoomId))
                {
                    _typingUsers[messageDto.ChatRoomId].Remove(userId);
                    await Clients.Group(messageDto.ChatRoomId).SendAsync("UserStoppedTyping", userId);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error sending message for user {userId}");
                await Clients.Caller.SendAsync("Error", "Failed to send message");
            }
        }

        public async Task MarkAsRead(string messageId, string chatRoomId)
        {
            var userId = Context.User?.FindFirst("userId")?.Value;
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
            var senderId = Context.User?.FindFirst("userId")?.Value;
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
    }
}