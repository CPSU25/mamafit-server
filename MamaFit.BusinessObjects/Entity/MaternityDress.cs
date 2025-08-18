using MamaFit.BusinessObjects.Base;
using MamaFit.BusinessObjects.Enum;

namespace MamaFit.BusinessObjects.Entity
{
    public class MaternityDress : BaseEntity
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public List<string>? Images { get; set; } = new List<string>();
        public string? Slug { get; set; }
        public string? SKU { get; set; } 
        public float AverageRating { get; set; }
        public int TotalRating { get; set; }
        public int? Rating { get; set; }
        public string? StyleId { get; set; }
        public GlobalStatus GlobalStatus { get; set; } = GlobalStatus.INACTIVE;

        // Navigation properties
        public Style? Style { get; set; }
        public virtual ICollection<MaternityDressDetail>? Details { get; set; } = new List<MaternityDressDetail>();
    }
}