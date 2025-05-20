using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MamaFit.BusinessObjects.Entity
{
    public class ApplicationUserToken : BaseEntity
    {
        public string? Token { get; set; }
        public DateTime? ExpiredAt { get; set; }
        public bool? IsRevoked { get; set; }
    }
}
