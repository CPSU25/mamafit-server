using MamaFit.BusinessObjects.Base;

namespace MamaFit.BusinessObjects.Entity
{
    public class MeasurementDiary : BaseEntity
    {
        public string? UserId { get; set; }
        public string? Name { get; set; }
        public int? Age { get; set; }
        public float? Height { get; set; }
        public float? Weight { get; set; }
        public float? Bust { get; set; }
        public float? Waist { get; set; }
        public float? Hip { get; set; }
        public DateTime? FirstDateOfLastPeriod { get; set; }
        public int? AverageMenstrualCycle { get; set; }
        public int? NumberOfPregnancy { get; set; }
        public DateTime? UltrasoundDate { get; set; }
        public int WeeksFromUltrasound { get; set; }
        public DateTime? PregnancyStartDate
        {
            get
            {
                if (UltrasoundDate.HasValue && WeeksFromUltrasound > 0)
                {
                    var ultrasoundDate = UltrasoundDate.Value;
                    return ultrasoundDate.AddDays(-7 * WeeksFromUltrasound);
                }
                if (FirstDateOfLastPeriod.HasValue)
                {
                    int cycleLength = AverageMenstrualCycle.HasValue ? AverageMenstrualCycle.Value : 28;
                    return FirstDateOfLastPeriod.Value.AddDays(cycleLength - 14); 
                }
                return null;
            }
        }

        // Nagivation property
        public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
        public ApplicationUser? User { get; set; }
        public virtual ICollection<Measurement> Measurements { get; set; } = new List<Measurement>();
    }
}
