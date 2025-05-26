using MamaFit.BusinessObjects.Base;

namespace MamaFit.BusinessObjects.Entity
{
    public class MeasurementDiary : BaseEntity
    {
        public string? UserId { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; } 
        public int NumberOfPregnancy { get; set; }
        
        // Nagivation property
        public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
        public ApplicationUser? User { get; set; }
        public virtual ICollection<Measurement> Measurements { get; set; } = new List<Measurement>();
    }
}
