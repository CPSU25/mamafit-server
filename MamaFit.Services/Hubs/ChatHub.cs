using MamaFit.BusinessObjects.DTO.ChatMessageDto;
using MamaFit.Services.Interface;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;
using MamaFit.Services.ExternalService.SignalR;

namespace MamaFit.Services.Hubs
{
    public class ChatHub : Hub
    {
        private readonly IChatService _chatService;
        private readonly IUserConnectionManager _userConnectionManager;

        public ChatHub(IChatService chatService, IUserConnectionManager connectionManager)
        {
            _chatService = chatService;
            _userConnectionManager = connectionManager;
        }

        public override async Task OnConnectedAsync()
        {
            var userId = GetCurrentUserId();
            if (!string.IsNullOrEmpty(userId))
            {
                await _userConnectionManager.AddUserConnectionAsync(userId, Context.ConnectionId);

                var userRooms = await _chatService.GetUserChatRoom(userId);
                foreach (var room in userRooms)
                {
                    await Groups.AddToGroupAsync(Context.ConnectionId, room.Id);
                    await _userConnectionManager.AddUserToRoomAsync(room.Id, userId);
                }

                await Clients.Others.SendAsync("UserOnline", userId);
            }

            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            var userId = GetCurrentUserId();
            if (!string.IsNullOrEmpty(userId))
            {
                await _userConnectionManager.RemoveUserConnectionAsync(userId, Context.ConnectionId);

                var userRooms = await _chatService.GetUserChatRoom(userId);
                foreach (var room in userRooms)
                {
                    await _userConnectionManager.RemoveUserFromRoomAsync(room.Id, userId);
                }

                await Clients.Others.SendAsync("UserOffline", userId);
            }

            await base.OnDisconnectedAsync(exception);
        }

        public async Task LoadRoom()
        {
            var userId = GetCurrentUserId();
            if (string.IsNullOrEmpty(userId))
            {
                await Clients.Caller.SendAsync("Error", "Unauthorized: Invalid user");
                return;
            }

            var rooms = await _chatService.GetUserChatRoom(userId);
            if (rooms == null || !rooms.Any())
            {
                await Clients.Caller.SendAsync("NoRooms", "You have no chat rooms.");
                return;
            }

            await Clients.Caller.SendAsync("LoadRoom", rooms);
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
            await _userConnectionManager.AddUserToRoomAsync(roomId, userId);
            await Clients.Caller.SendAsync("JoinedRoom", roomId);
        }

        public async Task LeaveRoom(string roomId)
        {
            var userId = GetCurrentUserId();
            if (string.IsNullOrEmpty(userId)) return;

            await Groups.RemoveFromGroupAsync(Context.ConnectionId, roomId);
            await _userConnectionManager.RemoveUserFromRoomAsync(roomId, userId);

            await Clients.Group(roomId).SendAsync("UserLeftRoom", userId, roomId);
        }

        public async Task SendMessage(ChatMessageCreateDto messageDto)
        {
            var userId = GetCurrentUserId();

            if (string.IsNullOrEmpty(userId))
            {
                await Clients.Caller.SendAsync("Error", "Unauthorized: User not authenticated");
                return;
            }

            if (string.IsNullOrEmpty(messageDto.ChatRoomId))
            {
                await Clients.Caller.SendAsync("Error", "Invalid room ID");
                return;
            }

            if (string.IsNullOrEmpty(messageDto.Message?.Trim()))
            {
                await Clients.Caller.SendAsync("Error", "Message cannot be empty");
                return;
            }

            try
            {
                messageDto.SenderId = userId;
                var response = await _chatService.CreateChatMessageAsync(messageDto);

                if (response == null || string.IsNullOrEmpty(response.Id))
                {
                    await Clients.Caller.SendAsync("Error", "Failed to create or retrieve message");
                    return;
                }

                await Clients.Group(messageDto.ChatRoomId).SendAsync("ReceiveMessage", new
                {
                    id = response.Id,
                    message = response.Message,
                    senderId = response.SenderId,
                    senderName = response.SenderName,
                    senderAvatar = response.SenderAvatar,
                    chatRoomId = response.ChatRoomId,
                    type = response.Type,
                    messageTimestamp = response.MessageTimestamp.ToString("O")
                });

                await Clients.Caller.SendAsync("MessageSent", new
                {
                    tempId = userId,
                    messageId = response.Id,
                    status = "success"
                });
            }
            catch (Exception ex)
            {
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
            await _chatService.MarkMessageAsReadAsync(messageId, userId, chatRoomId);

            await Clients.Caller.SendAsync("Error", "Failed to mark message as read");
        }


        public async Task GetOnlineUsers(string roomId)
        {
            var onlineUsers = await _userConnectionManager.GetRoomUsersAsync(roomId);
            await Clients.Caller.SendAsync("OnlineUsers", roomId, onlineUsers);
        }


        public async Task SendPrivateMessage(string targetUserId, string message)
        {
            var senderId = GetCurrentUserId();
            var targetConnections = await _userConnectionManager.GetUserConnectionsAsync(targetUserId);
            foreach (var connectionId in targetConnections)
            {
                await Clients.Client(connectionId).SendAsync("ReceivePrivateMessage", senderId, message);
            }
        }

        private string? GetCurrentUserId()
        {
            var userId = Context.User?.FindFirst("userId")?.Value ??
                         Context.User?.FindFirst("sub")?.Value ??
                         Context.User?.FindFirst("nameid")?.Value ??
                         Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return userId;
        }
    }
}