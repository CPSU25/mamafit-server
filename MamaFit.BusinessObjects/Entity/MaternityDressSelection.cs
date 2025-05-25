using MamaFit.BusinessObjects.Base;

namespace MamaFit.BusinessObjects.Entity;

public class MaternityDressSelection : BaseEntity
{
    public string? ComponentOptionId { get; set; }
    public string? MaternityDressCustomizationId { get; set; }
    public string? Description { get; set; }

    //Navigation property
    public virtual ComponentOption? ComponentOption { get; set; }
    public virtual MaternityDressCustomization? MaternityDressCustomization { get; set; }
}