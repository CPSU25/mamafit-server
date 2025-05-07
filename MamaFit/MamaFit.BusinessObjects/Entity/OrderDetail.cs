namespace MamaFit.BusinessObjects.Entity
{
    public sealed class OrderDetail : BaseEntity
    {
        public Guid OrderId { get; set; }
        public Order? Order { get; set; }
        public Guid DressId { get; set; }
        public Dress? Dress { get; set; }
        public int Quantity { get; set; }
    }
}
