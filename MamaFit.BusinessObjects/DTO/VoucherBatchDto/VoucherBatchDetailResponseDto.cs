using MamaFit.BusinessObjects.DTO.VoucherDiscountDto;

namespace MamaFit.BusinessObjects.DTO.VoucherBatchDto
{
    public class VoucherBatchDetailResponseDto : VoucherBatchResponseDto
    {
        public List<VoucherDiscountResponseDto>? Details { get; set; }
    }
}
