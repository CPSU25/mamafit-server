namespace MamaFit.BusinessObjects.Entity
{
    public class DressCustomization
    {
        public string? UserId { get; set; }
        public ApplicationUser? User { get; set; }
        public string? Description { get; set; }
        public string? Status { get; set; }
        public virtual ICollection<ComponentOption> ComponentOptions { get; set; } = [];
    }
}
