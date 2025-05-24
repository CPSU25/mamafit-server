using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MamaFit.BusinessObjects.Base;

namespace MamaFit.BusinessObjects.Entity
{
    public class ApplicationUserToken : BaseEntity
    {
        public string UserId { get; set; }
        public ApplicationUser? User { get; set; }
        public string? Token { get; set; }
        public DateTime? ExpiredAt { get; set; }
        public bool? IsRevoked { get; set; }
    }
}
