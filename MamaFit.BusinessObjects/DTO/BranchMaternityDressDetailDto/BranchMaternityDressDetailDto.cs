using MamaFit.BusinessObjects.DTO.BranchDto;
using MamaFit.BusinessObjects.DTO.MaternityDressDetailDto;

namespace MamaFit.BusinessObjects.DTO.BranchMaternityDressDetailDto;

public class BranchMaternityDressDetailDto
{
    public string? MaternityDressDetailId { get; set; }
    public string? BranchId { get; set; }
    public int? Quantity { get; set; }
}