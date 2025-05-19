namespace MamaFit.BusinessObjects.Entity
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
        public virtual ICollection<MeasurementDiary> MeasurementDiary { get; set; } = new List<MeasurementDiary>();
        public virtual ICollection<Notification> Notification { get; set; } = new List<Notification>();
        public virtual ICollection<OTP>OTP { get; set; } = new List<OTP>();
    }
}
