using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using MamaFit.BusinessObjects.Base;
using MamaFit.BusinessObjects.Enum;

namespace MamaFit.BusinessObjects.Entity
{
    public class Category : BaseEntity
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public List<string> Images { get; set; } = [];
        public GlobalStatus Status { get; set; } = GlobalStatus.ACTIVE;
        //Navigation property
        public virtual ICollection<Style>? Styles { get; set; } = [];
    }
}
