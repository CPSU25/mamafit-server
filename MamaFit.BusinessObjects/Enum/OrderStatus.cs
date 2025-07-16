namespace MamaFit.BusinessObjects.Enum
{
    public enum OrderStatus
    {
        CREATED, // default
        PAID_FULL, // đã thanh toán đầy đủ
        PAID_DEPOSIT, // đã thanh toán đặt cọc
        IN_DESIGN, // Designer đang làm việc với khách hàng
        IN_PRODUCTION, // nhà máy đang làm
        IN_QC, // nhà máy đã làm xong và đang kiểm tra chất lượng
        PACKAGING, // đóng gói
        DELIVERING,
        COMPLETED,
        CANCELLED,
        RETURNED,// bảo hành hoặc trả hàng
        EXPIRED // nếu đơn hàng có status là Designig mà quá 4 ngày sau khi khởi tạo thì tự động chuyển
    }
}
