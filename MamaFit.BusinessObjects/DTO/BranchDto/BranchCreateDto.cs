namespace MamaFit.BusinessObjects.DTO.BranchDto
{
    public class BranchCreateDto
    {
        public string? BranchManagerId { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public TimeOnly? OpeningHour { get; set; }
        public TimeOnly? ClosingHour { get; set; }
        public List<string> Images { get; set; } = new List<string>();
        public string? MapId { get; set; }
        public string? Province { get; set; }
        public string? District { get; set; }
        public string? Ward { get; set; }
        public string? Street { get; set; }
        public float? Latitude { get; set; }
        public float? Longitude { get; set; }
    }
}
