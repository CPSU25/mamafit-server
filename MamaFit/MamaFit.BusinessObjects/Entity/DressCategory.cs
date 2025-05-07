namespace MamaFit.BusinessObjects.Entity
{
    public sealed class DressCategory : BaseEntity
    {
        public string? CategoryName { get; set; }
        public string? Description { get; set; }
        public string? Image { get; set; }
        public ICollection<Dress> Dress { get; set; } = new List<Dress>();
    }
}
