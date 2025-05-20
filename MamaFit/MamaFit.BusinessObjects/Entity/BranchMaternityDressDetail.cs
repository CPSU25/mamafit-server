using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MamaFit.BusinessObjects.Entity
{
    public class BranchMaternityDressDetail
    {
        public string? MaternityDressDetailId { get; set; }
        public MaternityDressDetail? MaternityDressDetail { get; set; }
        public string? BranchId { get; set; }
        public Branch? Branch { get; set; }
        public int? Quantity { get; set; }
    }
}
