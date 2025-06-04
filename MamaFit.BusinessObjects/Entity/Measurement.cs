using MamaFit.BusinessObjects.Base;

namespace MamaFit.BusinessObjects.Entity
{
    public class Measurement : BaseEntity
    {
        public string? MeasurementDiaryId { get; set; }
        public float Height { get; set; }
        public float Weight { get; set; }
        public float Neck { get; set; }
        public float Coat { get; set; }
        public float ChestAround { get; set; }
        public float Stomach { get; set; }
        public float ShoulderWidth { get; set; }
        public float Hip { get; set; }
        
        // Navigation properties
        public MeasurementDiary? MeasurementDiary { get; set; }
    }
}
