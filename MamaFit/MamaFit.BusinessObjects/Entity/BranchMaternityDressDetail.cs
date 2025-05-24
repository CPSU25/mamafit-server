﻿namespace MamaFit.BusinessObjects.Entity
{
    public class BranchMaternityDressDetail
    {
        public string? MaternityDressDetailId { get; set; }
        public string? BranchId { get; set; }
        public int? Quantity { get; set; }
        
        //Navigation property
        public MaternityDressDetail? MaternityDressDetail { get; set; }
        public Branch? Branch { get; set; }
    }
}
