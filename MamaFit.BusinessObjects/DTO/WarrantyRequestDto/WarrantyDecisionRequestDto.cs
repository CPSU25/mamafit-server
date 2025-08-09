namespace MamaFit.BusinessObjects.DTO.WarrantyRequestDto;

public class WarrantyDecisionRequestDto
{
    public string? NoteInternal { get; set; } 
    public List<WarrantyDecisionItemDto> Items { get; set; } = new();
}