namespace MamaFit.BusinessObjects.Entity
{
    public sealed class Profile : BaseEntity
    {
        public Guid UserId { get; set; }
        public ApplicationUser? User { get; set; }
        public string? ProfileName { get; set; }
        public string? ProfileDescription { get; set; }
        public ICollection<MeasurementDiary> Diary { get; set; } = new List<MeasurementDiary>();
    }
}
