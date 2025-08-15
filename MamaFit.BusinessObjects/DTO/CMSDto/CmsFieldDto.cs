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
        public int WarrantyPeriod { get; set; }
        public List<string>? Colors { get; set; }
        public Varriants? Varriant { get; set; }
        public string? JobTitle { get; set; }
    }

    public class Varriants
    {
        public List<string> Color { get; set; } = new List<string>();
        public List<string> Size { get; set; } = new List<string>();
    }
}
