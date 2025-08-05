using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MamaFit.BusinessObjects.DTO.WarrantyRequestDto
{
    public class WarrantyRequestGetAllDto : WarrantyBaseDto
    {
        public string? Id { get; set; }
        public int WarrantyRound { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string? UpdatedBy { get; set; }
        public float? Fee { get; set; }
    }
}
