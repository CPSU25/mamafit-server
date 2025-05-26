using MamaFit.BusinessObjects.Base;
using MamaFit.BusinessObjects.Enum;

namespace MamaFit.BusinessObjects.Entity
{
    public class ApplicationUserToken : BaseEntity
    {
        public string UserId { get; set; }
        public ApplicationUser? User { get; set; }
        public string? Token { get; set; }
        public DateTime? ExpiredAt { get; set; }
        public bool? IsRevoked { get; set; }
        public TokenType? TokenType { get; set; }
    }
}
