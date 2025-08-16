namespace MamaFit.BusinessObjects.DTO.CMSDto
{
    public class CmsFieldDto
    {
        public string? Name { get; set; }
        public decimal DesignRequestServiceFee { get; set; }
        public decimal DepositRate { get; set; }
        public int PresetVersions { get; set; }
        public int WarrantyTime { get; set; }
        public int AppointmentSlotInterval { get; set; }
        public int MaxAppointmentPerDay { get; set; }
        public int MaxAppointmentPerUser { get; set; }
        public int WarrantyPeriod { get; set; }
        public List<string>? Colors { get; set; }
        public List<string>? Sizes { get; set; }

        public List<string>? JobTitle { get; set; }
    }

}
