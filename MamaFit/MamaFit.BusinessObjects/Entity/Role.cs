namespace MamaFit.BusinessObjects.Entity
{
    public class Role : BaseEntity
    {
        public string? RoleName { get; set; }
        public virtual ICollection<ApplicationUser> Users { get; set; } = new List<ApplicationUser>();
    }
}
