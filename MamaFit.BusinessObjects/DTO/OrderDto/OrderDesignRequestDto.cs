using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MamaFit.BusinessObjects.DTO.OrderDto
{
    public class OrderDesignRequestDto
    {
        public List<string>? Images { get; set; }
        public string? Description { get; set; }
    }
}
