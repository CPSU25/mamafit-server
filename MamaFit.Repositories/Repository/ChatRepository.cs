using MamaFit.BusinessObjects.DBContext;
using MamaFit.BusinessObjects.Entity.ChatEntity;
using MamaFit.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace MamaFit.Repositories.Repository
{
    public class ChatRepository : IChatRepository
    {
        private readonly ApplicationDbContext _context;

        public ChatRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task CreateChatMessageAsync(ChatMessage requestDto)
        {
            await _context.ChatMessages.AddAsync(requestDto);
            await _context.SaveChangesAsync();
            await _context.Entry(requestDto)
                .Reference(m => m.Sender)
                .LoadAsync();
        }

        public async Task CreateChatRoomAsync(string userId1, string userId2)
        {
            var user1 = await _context.Users.FirstOrDefaultAsync(u => u.Id.Equals(userId1));
            var user2 = await _context.Users.FirstOrDefaultAsync(u => u.Id.Equals(userId2));

            var chatroom = new ChatRoom
            {
                Name = $"{user1?.FullName} && {user2?.FullName}",
                CreatedBy = "System",
                CreatedAt = DateTime.UtcNow,
            };
            await _context.ChatRooms.AddAsync(chatroom);

            await _context.ChatRoomMembers.AddRangeAsync(
                new ChatRoomMember
                {
                    ChatRoomId = chatroom.Id,
                    UserId = userId1,
                    User = user1!,
                    ChatRoom = chatroom
                },
                new ChatRoomMember
                {
                    ChatRoomId = chatroom.Id,
                    UserId = userId2,
                    User = user2!,
                    ChatRoom = chatroom
                });


            await _context.SaveChangesAsync();
        }

        public async Task<List<ChatMessage>> GetChatHistoryAsync(string chatRoomId, int index, int pageSize)
        {
            var chatMessages = await _context.ChatMessages
                .Where(m => m.ChatRoomId == chatRoomId && !m.IsDeleted)
                .Include(m => m.Sender)
                .OrderByDescending(m => m.CreatedAt)
                .Skip((index - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return chatMessages;
        }

        public async Task<ChatMessage> GetChatMessageById(string messageId)
        {
            var message =  await _context.ChatMessages
                .Include(m => m.Sender)
                .FirstOrDefaultAsync(m => m.Id.Equals(messageId));
            return message;
        }

        public async Task<ChatRoom> GetChatRoomById(string chatRoomId)
        {
            var chatroom = await _context.ChatRooms
                .Include(r => r.Messages)
                .Include(r => r.Members)
                .ThenInclude(m => m.User)
                .FirstOrDefaultAsync(r => r.Id == chatRoomId);
            return chatroom!;
        }

        public async Task<List<ChatRoom>> GetUserChatRoom(string userId)
        {
            return await _context.ChatRooms
                .Include(r => r.Messages)
                .Include(r => r.Members)
                .ThenInclude(m => m.User)
                .Where(c => c.Members.Select(m => m.UserId).Contains(userId))
                .ToListAsync();
        }
    }
}
