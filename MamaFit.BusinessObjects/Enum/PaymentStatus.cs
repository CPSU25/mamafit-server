namespace MamaFit.BusinessObjects.Enum
{
    public enum PaymentStatus
    {
        PENDING,
        PAID_FULL, // thanh toán 100%% thành công
        PAID_DEPOSIT, // thanh toán cọc thành công
        PAID_DEPOSIT_COMPLETED,  //thanh toán phần còn lại thành công
        FAILED,
        CANCELED,
        EXPIRED
    }
}
