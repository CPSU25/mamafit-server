using MamaFit.BusinessObjects.DTO.GhtkDto.Address;

namespace MamaFit.BusinessObjects.DTO.CMSDto
{
    public class CmsServiceBaseDto
    {
        public string? Name { get; set; }
        public decimal DesignRequestServiceFee { get; set; }
        public int DepositRate { get; set; }
        //public CmsPickAddress? GhtkPickAddress { get; set; }
    }
}
