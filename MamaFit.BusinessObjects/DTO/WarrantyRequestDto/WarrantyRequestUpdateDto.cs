using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MamaFit.BusinessObjects.DTO.WarrantyRequestDto
{
    public class WarrantyRequestUpdateDto : WarrantyBaseDto
    {
        public bool? IsFactoryError { get; set; } = null;
        public string? RejectedReason { get; set; }
        public float? Fee { get; set; }
    }
}
