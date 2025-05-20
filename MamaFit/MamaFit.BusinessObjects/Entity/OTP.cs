using MamaFit.BusinessObjects.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MamaFit.BusinessObjects.Entity
{
    public class OTP : BaseEntity
    {
        public string? UserId { get; set; }
        public ApplicationUser? User { get; set; }
        public string? Code { get; set; }
        public DateTime ExpiredAt { get; set; }
        public OTPType OTPType { get; set; }
    }
}
