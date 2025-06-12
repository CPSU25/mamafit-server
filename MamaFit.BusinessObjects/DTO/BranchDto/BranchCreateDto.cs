namespace MamaFit.BusinessObjects.DTO.BranchDto
{
    public class BranchCreateDto
    {
        public string? BranchManagerId { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? OpeningHour { get; set; }
        public List<string> Images { get; set; } = new List<string>();
        public string? MapId { get; set; }
        public string? ShortName { get; set; }
        public string? LongName { get; set; }
        public float? Latitude { get; set; }
        public float? Longitude { get; set; }
    }
}
