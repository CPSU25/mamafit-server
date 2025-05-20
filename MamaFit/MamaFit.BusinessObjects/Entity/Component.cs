using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MamaFit.BusinessObjects.Entity
{
    public class Component
    {
        public string? StyleId { get; set; }
        public Style? Style { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public List<string>? Images { get; set; } = [];
        public virtual ICollection<ComponentOption> Option { get; set; } = [];
    }
}
