namespace MamaFit.BusinessObjects.Entity
{
    public class ComponentOption : BaseEntity
    {
        public string? ComponentId { get; set; }
        public Component? Component { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public List<string>? Images { get; set; } = [];
    }
}
