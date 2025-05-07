namespace MamaFit.BusinessObjects.Entity
{
    public sealed class ApplicationUser
    {
        public Guid Id { get; set; }
        public string? Username { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public bool? isVerify { get; set; } = false;
        public string? Role { get; set; }
        public string? Address { get; set; }
        public string? Image { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string? VerificationToken { get; set; }
        public ICollection<Profile> Profile { get; set; } = new List<Profile>();
        public ICollection<VoucherDiscount>? Voucher { get; set; }
        public ICollection<Order>? Order { get; set; } = new List<Order>();
        public ICollection<Feedback>? Feedback { get; set; } = new List<Feedback>();
        public ICollection<TaskOrder>? TaskOrder { get; set; }
    }
}
