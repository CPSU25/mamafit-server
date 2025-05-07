namespace MamaFit.BusinessObjects.Entity
{
    public sealed class DressComponent : BaseEntity
    {
        public string? ComponentName { get; set; }
        public string? Description { get; set; }
        public string? ComponentType { get; set; }
        public string? Image { get; set; }
        public float Price { get; set; }
        public ICollection<DressComponentOption> Option { get; set; } = new List<DressComponentOption>();
    }
}
