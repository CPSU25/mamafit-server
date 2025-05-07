namespace MamaFit.BusinessObjects.Entity
{
    public sealed class MeasurementDiary : BaseEntity
    {
        public Guid ProfileId { get; set; }
        public Profile? Profile { get; set; }
        public float BustCircumference { get; set; }
        public float UnderBustCircumference { get; set; }
        public float BellyCircumference { get; set; }
        public float HipCircumference { get; set; }
        public float Height { get; set; }
        public float ShoulderWidth { get; set; }
    }
}
