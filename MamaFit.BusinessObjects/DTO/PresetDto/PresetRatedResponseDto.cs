namespace MamaFit.BusinessObjects.DTO.PresetDto
{
    public class PresetRatedResponseDto : PresetGetAllResponseDto
    {
        public int FeedbackCount { get; set; }
        public float AverageRate { get; set; }
    }
}
