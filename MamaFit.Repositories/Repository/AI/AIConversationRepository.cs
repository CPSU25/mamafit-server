using MamaFit.BusinessObjects.DBContext;
using MamaFit.BusinessObjects.Entity.AI;
using MamaFit.Repositories.Implement;
using MamaFit.Repositories.Interface.AI;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace MamaFit.Repositories.Repository.AI;

public class AIConversationRepository : GenericRepository<AIConversation>, IAIConversationRepository
{
    public AIConversationRepository(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor) 
        : base(context, httpContextAccessor)
    {
    }
    
    public async Task<AIConversation?> GetActiveConversationAsync(string userId)
    {
        return await _dbSet
            .Where(c => c.UserId == userId && !c.IsDeleted && c.IsActive)
            .OrderByDescending(c => c.CreatedAt)
            .FirstOrDefaultAsync();
    }
    
    public async Task<List<AIConversation>> GetUserConversationsAsync(string userId, int limit = 5)
    {
        return await _dbSet
            .Where(c => c.UserId == userId && !c.IsDeleted)
            .OrderByDescending(c => c.CreatedAt)
            .Take(limit)
            .ToListAsync();
    }
    
    public async Task DeactivateUserConversationsAsync(string userId)
    {
        var conversations = await _dbSet
            .Where(c => c.UserId == userId && !c.IsDeleted && c.IsActive)
            .ToListAsync();

        foreach (var conversation in conversations)
        {
            conversation.IsActive = false;
            conversation.UpdatedAt = DateTime.UtcNow;
        }

        _context.UpdateRange(conversations);
        await _context.SaveChangesAsync();
    }
}