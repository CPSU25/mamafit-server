namespace MamaFit.BusinessObjects.Entity
{
    public sealed class DressComponentOption : BaseEntity
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public List<string>? Image { get; set; }
    }
}
