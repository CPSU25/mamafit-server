using MamaFit.BusinessObjects.Base;

namespace MamaFit.BusinessObjects.Entity.AI;

public class AIConversation : BaseEntity
{
    public string UserId { get; set; }
    public string ConversationData { get; set; } // JSON
    public string Language { get; set; }
    public DateTime StartedAt { get; set; }
    public DateTime LastMessageAt { get; set; }
    public bool IsActive { get; set; }
        
    // Navigation
    public ApplicationUser User { get; set; }
}