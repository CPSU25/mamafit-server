using MamaFit.BusinessObjects.DTO.ChatMessageDto;
using MamaFit.Services.Interface;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;
using MamaFit.Services.ExternalService.SignalR;
using Microsoft.AspNetCore.Http;

namespace MamaFit.Services.Hubs
{
    public class ChatHub : Hub
    {
        private readonly IChatService _chatService;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IUserConnectionManager _userConnectionManager;

        public ChatHub(IChatService chatService, IUserConnectionManager connectionManager, IHttpContextAccessor contextAccessor)
        {
            _chatService = chatService;
            _userConnectionManager = connectionManager;
            _contextAccessor = contextAccessor;
        }

        public override async Task OnConnectedAsync()
        {
            var userId = GetCurrentUserId();
            if (string.IsNullOrEmpty(userId))
            {
                await base.OnConnectedAsync();
                return;
            }

            // Đăng ký kết nối user - connectionId vào Redis (UserConnectionManager)
            await _userConnectionManager.AddUserConnectionAsync(userId, Context.ConnectionId);

            // Tự động join các room user đang là thành viên
            var userRooms = await _chatService.GetUserChatRoom(userId);
            foreach (var room in userRooms)
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, room.Id);
                await _userConnectionManager.AddUserToRoomAsync(room.Id, userId);
            }

            // Chỉ gửi thông báo online nếu đây là kết nối đầu tiên (user vừa online thực sự)
            if (await _userConnectionManager.GetUserConnectionsAsync(userId) is { Count: 1 })
            {
                await Clients.Others.SendAsync("UserOnline", userId);
            }

            await GetListOnlineUser();
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            var userId = GetCurrentUserId();
            if (!string.IsNullOrEmpty(userId))
            {
                await _userConnectionManager.RemoveUserConnectionAsync(userId, Context.ConnectionId);

                // Xoá user khỏi các room (tuỳ nghiệp vụ, có thể bỏ qua nếu muốn user chỉ out khỏi room khi leave)
                var userRooms = await _chatService.GetUserChatRoom(userId);
                foreach (var room in userRooms)
                {
                    await _userConnectionManager.RemoveUserFromRoomAsync(room.Id, userId);
                }

                // Chỉ gửi offline khi user thực sự không còn kết nối nào (tức thật sự offline)
                if (!await _userConnectionManager.IsUserOnlineAsync(userId))
                {
                    await Clients.Others.SendAsync("UserOffline", userId);
                }
            }

            await base.OnDisconnectedAsync(exception);
        }

        /// <summary>
        /// Client chủ động load các room mà mình là thành viên
        /// </summary>
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

        /// <summary>
        /// Join vào room (group SignalR) và cập nhật trạng thái vào RoomUsers Redis
        /// </summary>
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

        /// <summary>
        /// Rời khỏi room (group SignalR) và cập nhật trạng thái vào RoomUsers Redis
        /// </summary>
        public async Task LeaveRoom(string roomId)
        {
            var userId = GetCurrentUserId();
            if (string.IsNullOrEmpty(userId)) return;

            await Groups.RemoveFromGroupAsync(Context.ConnectionId, roomId);
            await _userConnectionManager.RemoveUserFromRoomAsync(roomId, userId);

            await Clients.Group(roomId).SendAsync("LeftRoom", userId, roomId);
        }

        /// <summary>
        /// Gửi tin nhắn đến 1 room, đồng bộ DB và gửi real-time
        /// </summary>
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

                // Gửi tới tất cả client trong room
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

                // Thông báo gửi thành công cho client vừa gửi
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


        /// <summary>
        /// Lấy danh sách user online trong room (lọc đúng người đang online)
        /// </summary>
        public async Task GetOnlineUsers(string roomId)
        {
            var userIds = await _userConnectionManager.GetRoomUsersAsync(roomId);
            var onlineUsers = new List<string>();
            foreach (var userId in userIds)
            {
                if (await _userConnectionManager.IsUserOnlineAsync(userId))
                {
                    onlineUsers.Add(userId);
                }
            }
            await Clients.Caller.SendAsync("OnlineUsers", roomId, onlineUsers);
        }

        public async Task GetListOnlineUser()
        {
            var onlineUsers = await _userConnectionManager.GetOnlineUsersAsync();
            if (onlineUsers == null || !onlineUsers.Any())
            {
                await Clients.Caller.SendAsync("NoOnlineUsers", "No users are currently online.");
                return;
            }

            await Clients.Caller.SendAsync("ListOnlineUsers", onlineUsers);
        }
        /// <summary>
        /// Gửi tin nhắn riêng tư cho user khác
        /// </summary>
        public async Task SendPrivateMessage(string targetUserId, string message)
        {
            var senderId = GetCurrentUserId();
            if (string.IsNullOrEmpty(senderId) || string.IsNullOrEmpty(targetUserId) || string.IsNullOrEmpty(message?.Trim()))
            {
                await Clients.Caller.SendAsync("Error", "Invalid sender, target or message.");
                return;
            }

            var targetConnections = await _userConnectionManager.GetUserConnectionsAsync(targetUserId);
            if (targetConnections.Count == 0)
            {
                await Clients.Caller.SendAsync("Error", "Target user is offline.");
                return;
            }
            foreach (var connectionId in targetConnections)
            {
                await Clients.Client(connectionId).SendAsync("ReceivePrivateMessage", senderId, message);
            }
        }

        /// <summary>
        /// Helper lấy userId từ Claims (ưu tiên các trường phổ biến)
        /// </summary>
        private string? GetCurrentUserId()
        {
            return Context.User?.FindFirst("userId")?.Value
                ?? _contextAccessor.HttpContext.Request.Cookies.TryGetValue("userId", out var value).ToString();
        }
    }
}
