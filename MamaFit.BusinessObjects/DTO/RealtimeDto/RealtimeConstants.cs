namespace MamaFit.BusinessObjects.DTO.RealtimeDto;

public static class RealtimeEventTypes
{
    // Order Events
    public const string ORDER_STATUS_CHANGED = "order.status.changed";
    public const string ORDER_CREATED = "order.created";
    public const string ORDER_PAYMENT_UPDATED = "order.payment.updated";
    public const string ORDER_CANCELLED = "order.cancelled";
    public const string ORDER_COMPLETED = "order.completed";
    
    // Task Events  
    public const string TASK_STATUS_CHANGED = "task.status.changed";
    public const string TASK_ASSIGNED = "task.assigned";
    public const string TASK_PROGRESS_UPDATED = "task.progress.updated";
    public const string MILESTONE_COMPLETED = "milestone.completed";
    
    // Payment Events
    public const string PAYMENT_RECEIVED = "payment.received";
    public const string PAYMENT_FAILED = "payment.failed";
    public const string PAYMENT_REFUNDED = "payment.refunded";
    
    // Notification Events
    public const string NOTIFICATION_CREATED = "notification.created";
    public const string NOTIFICATION_READ = "notification.read";
    
    // Chat Events
    public const string CHAT_MESSAGE_SENT = "chat.message.sent";
    public const string CHAT_ROOM_CREATED = "chat.room.created";
    public const string CHAT_USER_JOINED = "chat.user.joined";
    
    // Warranty Events
    public const string WARRANTY_REQUEST_CREATED = "warranty.request.created";
    public const string WARRANTY_STATUS_UPDATED = "warranty.status.updated";
    
    // System Events
    public const string SYSTEM_MAINTENANCE = "system.maintenance";
    public const string SYSTEM_ANNOUNCEMENT = "system.announcement";
}

public static class RealtimeEntityTypes
{
    public const string ORDER = "Order";
    public const string ORDER_ITEM = "OrderItem";
    public const string ORDER_ITEM_TASK = "OrderItemTask";
    public const string TRANSACTION = "Transaction";
    public const string NOTIFICATION = "Notification";
    public const string CHAT_MESSAGE = "ChatMessage";
    public const string CHAT_ROOM = "ChatRoom";
    public const string WARRANTY_REQUEST = "WarrantyRequest";
    public const string USER = "User";
}

public static class RealtimeGroups
{
    public const string ALL_USERS = "AllUsers";
    public const string ADMIN_USERS = "AdminUsers";
    public const string STAFF_USERS = "StaffUsers";
    public const string CUSTOMER_USERS = "CustomerUsers";
    public const string DESIGNER_USERS = "DesignerUsers";
    public const string MANAGER_USERS = "ManagerUsers";
    
    public static string GetOrderGroup(string orderId) => $"Order_{orderId}";
    public static string GetUserGroup(string userId) => $"User_{userId}";
    public static string GetChatRoomGroup(string chatRoomId) => $"ChatRoom_{chatRoomId}";
    public static string GetBranchGroup(string branchId) => $"Branch_{branchId}";
}