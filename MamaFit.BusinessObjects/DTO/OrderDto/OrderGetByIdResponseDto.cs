using MamaFit.BusinessObjects.DTO.AddressDto;
using MamaFit.BusinessObjects.DTO.BranchDto;
using MamaFit.BusinessObjects.DTO.VoucherDiscountDto;

namespace MamaFit.BusinessObjects.DTO.OrderDto
{
    public class OrderGetByIdResponseDto : OrderResponseDto
    {
        public AddressResponseDto? Address { get; set; }
        public BranchResponseDto? Branch { get; set; }
        public VoucherDiscountResponseDto? VoucherDiscount { get; set; }
    }
}
