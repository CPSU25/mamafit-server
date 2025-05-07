namespace MamaFit.BusinessObjects.Entity
{
    public sealed class VoucherDiscount : BaseEntity
    {
        public string? VoucherName { get; set; }
        public string? Code { get; set; }
        public float? DiscountAmount { get; set; }
        public string? Description { get; set; }
        public float? ConditionFee { get; set; } = 0;
        public bool? isActive { get; set; }
    }
}
