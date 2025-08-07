using MamaFit.BusinessObjects.Base;
using MamaFit.BusinessObjects.DTO.CartItemDto;
using MamaFit.BusinessObjects.Entity.ChatEntity;

namespace MamaFit.BusinessObjects.Entity
{
    public class ApplicationUser : BaseEntity
    {
        public string? UserName { get; set; }
        public string? UserEmail { get; set; }
        public string? HashPassword { get; set; }
        public string? Salt { get; set; }
        public string? FullName { get; set; }
        public string? ProfilePicture { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? PhoneNumber { get; set; }
        public bool IsVerify { get; set; } = false;
        public string? RoleId { get; set; }
        
        //Navigation properties
        public ApplicationUserRole? Role { get; set; }
        public List<Branch>? Branch { get; set; }
        public virtual ICollection<ApplicationUserToken>? Token { get; set; }
        public virtual ICollection<Address>? Addresses { get; set; } = new List<Address>();
        public virtual ICollection<MeasurementDiary>? MeasurementDiaries { get; set; } = new List<MeasurementDiary>();
        public virtual ICollection<Notification>? Notifications { get; set; } = new List<Notification>();
        public virtual ICollection<OTP>? OTPs { get; set; } = new List<OTP>();
        public virtual ICollection<Feedback>? Feedbacks { get; set; } = new List<Feedback>();
        public virtual ICollection<DesignRequest>? DesignRequests { get; set; } = new List<DesignRequest>();
        public virtual ICollection<Appointment>? Appointments { get; set; } = new List<Appointment>();
        public virtual ICollection<Order>? Orders { get; set; } = [];
        public virtual ICollection<VoucherDiscount>? VoucherDiscounts { get; set; } = [];
        public virtual ICollection<OrderItemTask>? OrderItemTasks { get; set; } = [];
        public virtual ICollection<Preset>? Presets { get; set; } = [];
        //Related chat navigation 
        public virtual ICollection<ChatRoom> CreatedChatRooms { get; set; } = new List<ChatRoom>();
        public virtual ICollection<ChatRoomMember> ChatRoomMemberships { get; set; } = new List<ChatRoomMember>();
        public virtual ICollection<ChatMessage> SentMessages { get; set; } = new List<ChatMessage>();
    }
}
