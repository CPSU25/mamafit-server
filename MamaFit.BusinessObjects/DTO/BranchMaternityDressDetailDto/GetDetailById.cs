using MamaFit.BusinessObjects.DTO.BranchDto;
using MamaFit.BusinessObjects.DTO.MaternityDressDetailDto;

namespace MamaFit.BusinessObjects.DTO.BranchMaternityDressDetailDto;

public class GetDetailById
{
    public int? Quantity { get; set; }
    public MaternityDressDetailResponseDto? MaternityDressDetail { get; set; }
    public BranchResponseDto? Branch { get; set; }
}