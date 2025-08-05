namespace MamaFit.BusinessObjects.DTO.CMSDto
{
    public class CmsFieldDto
    {
        public string? Name { get; set; }
        public decimal DesignRequestServiceFee { get; set; }
        public float DepositRate { get; set; }
        public int PresetVersions { get; set; }
        public int WarrantyTime { get; set; }
        public int AppointmentSlotInterval { get; set; }
        public int MaxAppointmentPerDay { get; set; }
        public int MaxAppointmentPerUser { get; set; }
        //public CmsPickAddress? GhtkPickAddress { get; set; }
        public int WarrantyPeriod { get; set; }
    }
}
