namespace MamaFit.BusinessObjects.Entity
{
    public sealed class Dress : BaseEntity
    {
        public string? DressName { get; set; }
        public string? Description { get; set; }
        public float Price { get; set; }
        public List<string>? Image { get; set; }
        public ICollection<DressCategory> Category { get; set; } = new List<DressCategory>();
        public ICollection<DressComponent> Component { get; set; } = new List<DressComponent>();
        public ICollection<Branch> Branch { get; set; } = new List<Branch>();
        public ICollection<Feedback> Feedback { get; set; } = new List<Feedback>();
    }
}
