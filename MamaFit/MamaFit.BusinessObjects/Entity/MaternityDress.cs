namespace MamaFit.BusinessObjects.Entity
{
    public class MaternityDress : BaseEntity
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public List<string> Images { get; set; } = new List<string>();
        public string? Slug { get; set; }
        public virtual ICollection<MaternityDressDetail> Details { get; set; } = new List<MaternityDressDetail>();
        public float AverageRating { get; set; }
        public int TotalRatings { get; set; }
        public Dictionary<int, int> RatingDistribution { get; set; } = new();
        public string? StyleId { get; set; }
        public Style? Style { get; set; }
    }
}
