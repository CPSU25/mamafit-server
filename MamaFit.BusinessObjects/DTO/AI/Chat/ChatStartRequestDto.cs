namespace MamaFit.BusinessObjects.DTO.AI.Chat;

public class ChatStartRequestDto
{
    public string Language { get; set; } = "vi";
}

public class ChatStartResponseDto
{
    public string ConversationId { get; set; }
    public string WelcomeMessage { get; set; }
}

public class ChatMessageRequestDto
{
    public string Message { get; set; }
}

public class ChatMessageResponseDto
{
    public string Response { get; set; }
    public DateTime Timestamp { get; set; }
}

public class HealthAdviceRequestDto
{
    public int CurrentWeek { get; set; }
    public string Question { get; set; }
    public float? CurrentWeight { get; set; }
    public string Language { get; set; } = "vi";
}