namespace MamaFit.BusinessObjects.Enum
{
    public enum OrderStatus
    {
        CREATED, // default
        CONFIRMED, //thanh toán thành công
        IN_DESIGN, // Designer đang làm việc với khách hàng
        IN_PRODUCTION, // nhà máy đang làm
        AWAITING_PAID_REST, //pass QC và đang đợi customer trả phần còn lại
        AWAITING_DELIVERY, // DOI LEN DON HANG
        IN_QC, // nhà máy đã làm xong và đang kiểm tra chất lượng
        IN_WARRANTY, // đang bảo hành / sửa
        PACKAGING, // đóng gói
        DELIVERING,
        COMPLETED,
        WARRANTY_CHECK, // đang đợi staff xem có phải lỗi từ nhà máy
        CANCELLED,
        RETURNED,// bảo hành hoặc trả hàng
        EXPIRED // nếu đơn hàng có status là Designig mà quá 4 ngày sau khi khởi tạo thì tự động chuyển
    }
}