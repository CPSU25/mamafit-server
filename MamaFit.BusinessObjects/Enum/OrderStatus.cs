namespace MamaFit.BusinessObjects.Enum
{
    public enum OrderStatus
    {
        CREATED, // default
        CONFIRMED, //thanh toán thành công
        IN_PROGRESS, // đang thiết kế đối với design request, đang sản xuất, đang thêm addon, qc, sửa qc failed, đang bảo hành, đang check bảo hành
        AWAITING_PAID_REST, // đợi thanh toán phần còn lại 
        PACKAGING, // đóng gói và lên đơn
        DELIVERING, // lên đơn thành công
        COMPLETED, // khách confirm nhận hàng
        CANCELLED, // hủy đơn hàng
        RETURNED,// trả hàng
        
        //status for warranty order
        AWAITING_PAID_WARRANTY, 
        COMPLETED_WARRANTY
    }
}