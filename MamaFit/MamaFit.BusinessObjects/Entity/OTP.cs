﻿using MamaFit.BusinessObjects.Enum;
using MamaFit.BusinessObjects.Base;

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
