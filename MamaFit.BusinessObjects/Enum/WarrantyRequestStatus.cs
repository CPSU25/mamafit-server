namespace MamaFit.BusinessObjects.Enum
{
    public enum WarrantyRequestStatus
    {
        PENDING,
        REPAIRING, 
        COMPLETED, 
        REJECTED, 
        APROVED, // Tất cả các item đều được chấp nhận
        PARTIALLY_REJECTED, // Một số item được chấp nhận, một số bị từ chối
        CANCELLED 
    }
}
