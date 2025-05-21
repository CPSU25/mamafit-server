using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using MamaFit.BusinessObjects.Base;

namespace MamaFit.BusinessObjects.Entity
{
    public class Category : BaseEntity
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        //public List<string> Images { get; set; } = [];
        //sqlserver
        public string ImagesJson { get; set; } = "[]"; // Mặc định là mảng rỗng

        // Property [NotMapped] để làm việc với List<string> trong code
        [NotMapped]
        public List<string> Images
        {
            get => JsonSerializer.Deserialize<List<string>>(ImagesJson) ?? new List<string>();
            set => ImagesJson = JsonSerializer.Serialize(value);
        }
        
        //Navigation property
        public virtual ICollection<Style> Styles { get; set; } = [];
    }
}
