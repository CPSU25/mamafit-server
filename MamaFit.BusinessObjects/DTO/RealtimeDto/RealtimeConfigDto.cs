namespace MamaFit.BusinessObjects.DTO.RealtimeDto;

public class RealtimeConfigDto
{
    public bool EnableOrderStatusUpdates { get; set; } = true;
    public bool EnableTaskStatusUpdates { get; set; } = true;
    public bool EnablePaymentStatusUpdates { get; set; } = true;
    public bool EnableNotificationUpdates { get; set; } = true;
    public bool EnableChatUpdates { get; set; } = true;
    public bool EnableWarrantyStatusUpdates { get; set; } = true;
    public bool EnableInventoryUpdates { get; set; } = false;
    public bool EnableSystemMaintenanceUpdates { get; set; } = false;
    
    // Advanced configurations
    public int MaxRetryCount { get; set; } = 3;
    public int RetryDelayMs { get; set; } = 1000;
    public int EventTimeoutMs { get; set; } = 30000;
    public bool LogFailedEvents { get; set; } = true;
}