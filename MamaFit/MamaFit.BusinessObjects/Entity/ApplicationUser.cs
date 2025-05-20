﻿namespace MamaFit.BusinessObjects.Entity
{
    public class ApplicationUser : BaseEntity
    {
        public string UserName { get; set; }
        public string UserEmail { get; set; }
        public string Password { get; set; }
        public string? FullName { get; set; }
        public string? ProfilePicture { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? PhoneNumber { get; set; }
        public bool IsVerify { get; set; } = false;
        public string? RoleId { get; set; }
        public Role? Role { get; set; }
        public string? TokenId { get; set; }
        public ApplicationUserToken? Token { get; set; }
        public virtual ICollection<Location> Location { get; set; } = new List<Location>();
        public virtual ICollection<MeasurementDiary> MeasurementDiaries { get; set; } = new List<MeasurementDiary>();
        public virtual ICollection<Notification> Notifications { get; set; } = new List<Notification>();
        public virtual ICollection<OTP> OTPs { get; set; } = new List<OTP>();
        public virtual ICollection<Feedback> Feedbacks { get; set; } = new List<Feedback>();
        public virtual ICollection<DesignOrder> DesignOrders { get; set; } = new List<DesignOrder>();
        public virtual ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
        public virtual ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();
        public virtual ICollection<DressCustomization> DressCustomizations { get; set; } = [];
        public virtual ICollection<Order> Orders { get; set; } = [];
        public virtual ICollection<VoucherDiscount> VoucherDiscounts { get; set; } = [];
    }
}
