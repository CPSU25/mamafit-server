using MamaFit.BusinessObjects.Enum;

namespace MamaFit.BusinessObjects.Entity
{
    public sealed class TaskOrder : BaseEntity
    {
        public Guid DesignerId { get; set; }
        public ApplicationUser? Designer { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public Guid? OrderId { get; set; }
        public Order? Order { get; set; }
        public bool? isUrgent { get; set; } = false;
        public TaskOrderStatus? Status { get; set; }
    }
}
