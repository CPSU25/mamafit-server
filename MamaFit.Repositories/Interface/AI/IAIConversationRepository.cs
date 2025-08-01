using MamaFit.BusinessObjects.Entity.AI;
using MamaFit.Repositories.Implement;

namespace MamaFit.Repositories.Interface.AI;

public interface IAIConversationRepository : IGenericRepository<AIConversation>
{
    Task<AIConversation> GetActiveConversationAsync(string userId);
    Task<List<AIConversation>> GetUserConversationsAsync(string userId, int limit = 5);
    Task DeactivateUserConversationsAsync(string userId);
}