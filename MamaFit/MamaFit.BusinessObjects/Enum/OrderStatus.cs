namespace MamaFit.BusinessObjects.Enum
{
    public enum OrderStatus
    {
        CREATED, // default
        DESIGNING, // Designer đang làm việc với khách hàng
        CONFIRMED, // chờ nhà máy xác nhận
        PROCESSING, // nhà máy đang làm + QC
        PACKAGING, // đóng gói
        DELIVERING,
        DELIVERED,
        CANCELLED,
        RETURNED,// bảo hành hoặc trả hàng
        EXPIRED // nếu đơn hàng có status là Designig mà quá 4 ngày sau khi khởi tạo thì tự động chuyển
    }
}
