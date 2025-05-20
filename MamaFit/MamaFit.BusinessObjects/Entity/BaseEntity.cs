using System.ComponentModel.DataAnnotations;

namespace MamaFit.BusinessObjects.Entity
{
    public abstract class BaseEntity
    {
        protected BaseEntity()
        {
            Id = Guid.NewGuid().ToString("N");
            CreatedAt = UpdatedAt = DateTime.Now;
        }

        [Key]
        public string Id { get; set; }
        public string? CreatedBy { get; set; }
        public string? UpdateBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public bool? IsDelete { get; set; } = false;
    }
}
