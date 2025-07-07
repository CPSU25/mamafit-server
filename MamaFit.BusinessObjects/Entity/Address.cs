﻿using MamaFit.BusinessObjects.Enum;
using MamaFit.BusinessObjects.Base;

namespace MamaFit.BusinessObjects.Entity
{
    public class Address : BaseEntity
    {
        public string? UserId { get; set; } 
        public string? MapId { get; set; }
        public string? Province { get; set; }
        public string? District { get; set; }
        public string? Ward { get; set; }
        public string? Street { get; set; }
        public float? Latitude { get; set; }
        public float? Longitude { get; set; }
        public bool IsDefault { get; set; } = false;

        // Navigation properties
        public ApplicationUser? User { get; set; }
    }
}
