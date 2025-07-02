using MamaFit.BusinessObjects.DbContext;
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
        
        public async Task<ChatRoom?> GetRoomBetweenUsersAsync(string userId1, string userId2)
        {
            var roomIds = await _context.ChatRoomMembers
                .Where(m => m.UserId == userId1 || m.UserId == userId2)
                .GroupBy(m => m.ChatRoomId)
                .Where(g => g.Count() == 2)
                .Select(g => g.Key)
                .ToListAsync();

            return await _context.ChatRooms
                .FirstOrDefaultAsync(r => roomIds.Contains(r.Id));
        }

        public async Task<ChatRoom> CreateChatRoomAsync(string userId1, string userId2)
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

            var members = new List<ChatRoomMember>
            {
                new ChatRoomMember { UserId = userId1, ChatRoom = chatroom },
                new ChatRoomMember { UserId = userId2, ChatRoom = chatroom }
            };
            await _context.ChatRoomMembers.AddRangeAsync(members);

            await _context.SaveChangesAsync();

            // Trả về phòng có kèm member
            return await _context.ChatRooms
                       .Include(r => r.Members)
                       .FirstOrDefaultAsync(r => r.Id == chatroom.Id)
                   ?? chatroom;
        }

        public async Task<List<ChatMessage>> GetChatHistoryAsync(string chatRoomId, int page, int pageSize)
        {
            int skip = (page - 1) * pageSize;
            
            var chatMessages = await _context.ChatMessages
                .Where(m => m.ChatRoomId == chatRoomId && !m.IsDeleted)
                .Include(m => m.Sender)
                .OrderByDescending(m => m.CreatedAt) 
                .Skip(skip)
                .Take(pageSize)
                .ToListAsync();

            chatMessages.Reverse();
            
            return chatMessages;
        }

        public async Task<ChatMessage?> GetChatMessageById(string messageId)
        {
            var message = await _context.ChatMessages
                .Include(m => m.Sender)
                .FirstOrDefaultAsync(m => m.Id.Equals(messageId) && !m.IsDeleted);
            return message;
        }

        public async Task<ChatRoom> GetChatRoomById(string chatRoomId)
        {
            var chatroom = await _context.ChatRooms
                .Include(r => r.Messages)
                .Include(r => r.Members)
                .ThenInclude(m => m.User)
                .FirstOrDefaultAsync(r => r.Id == chatRoomId && !r.IsDeleted);
            return chatroom!;
        }

        public async Task<List<ChatRoom>> GetUserChatRoom(string userId)
        {
            return await _context.ChatRooms
                .Include(r => r.Messages)
                .Include(r => r.Members)
                .ThenInclude(m => m.User)
                .Where(c => c.Members.Select(m => m.UserId).Contains(userId) && !c.IsDeleted)
                .ToListAsync();
        }

        public async Task UpdateMessageAsync(ChatMessage message)
        {
            _context.Entry(message).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
    }
}
