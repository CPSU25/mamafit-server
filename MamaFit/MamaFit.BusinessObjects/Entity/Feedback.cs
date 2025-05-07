namespace MamaFit.BusinessObjects.Entity
{
    public sealed class Feedback : BaseEntity
    {
        public Guid UserId { get; set; }
        public ApplicationUser? User { get; set; }
        public Guid DressId { get; set; }
        public Dress? Dress { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public float? Rated { get; set; }
    }
}
