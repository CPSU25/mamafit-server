namespace MamaFit.BusinessObjects.DTO.GhtkDto.SubmitOrder;

public class GhtkRecipentDto
{
    public string CustomerPhone { get; set; } //sdt nguoi nhan
    public string CustomerName { get; set; } // ten nguoi nhan
    public string CustomerAddress { get; set; } // dia chi nguoi nhan
    public string Province { get; set; } // tinh thanh nguoi nhan
    public string District { get; set; }  // quan huyen nguoi nhan
    public string Ward { get; set; } // phuong xa nguoi nhan
}