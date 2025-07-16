namespace MamaFit.BusinessObjects.Enum;

public enum OrderItemTaskStatus
{
    PENDING, // Chờ xử lý
    IN_PROGRESS, // Đang thực hiện
    DONE, // Đã hoàn thành
    PASS, // Đã thông qua
    FAIL, // Không thông qua
}