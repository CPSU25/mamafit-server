﻿using MamaFit.BusinessObjects.Base;

namespace MamaFit.BusinessObjects.Entity
{
    public class ApplicationUser : BaseEntity
    {
        public string? BranchId { get; set; }
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
        public Branch? Branch { get; set; }
        public Branch? BranchAsManager { get; set; }
        public virtual ICollection<ApplicationUserToken>? Token { get; set; }
        public virtual ICollection<Address>? Addresses { get; set; } = new List<Address>();
        public virtual ICollection<MeasurementDiary>? MeasurementDiaries { get; set; } = new List<MeasurementDiary>();
        public virtual ICollection<Notification>? Notifications { get; set; } = new List<Notification>();
        public virtual ICollection<OTP>? OTPs { get; set; } = new List<OTP>();
        public virtual ICollection<Feedback>? Feedbacks { get; set; } = new List<Feedback>();
        public virtual ICollection<DesignRequest>? DesignRequests { get; set; } = new List<DesignRequest>();
        public virtual ICollection<Appointment>? Appointments { get; set; } = new List<Appointment>();
        public virtual ICollection<CartItem>? CartItems { get; set; } = new List<CartItem>();
        public virtual ICollection<MaternityDressCustomization>? DressCustomizations { get; set; } = [];
        public virtual ICollection<Order>? Orders { get; set; } = [];
        public virtual ICollection<VoucherDiscount>? VoucherDiscounts { get; set; } = [];
        public virtual ICollection<OrderItemTask>? OrderItemTasks { get; set; } = [];
    }
}
