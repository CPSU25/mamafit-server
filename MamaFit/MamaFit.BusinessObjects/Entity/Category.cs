namespace MamaFit.BusinessObjects.Entity
{
    public class Category : BaseEntity
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public List<string> Images { get; set; } = [];
        public virtual ICollection<Style> Styles { get; set; } = [];
    }
}
