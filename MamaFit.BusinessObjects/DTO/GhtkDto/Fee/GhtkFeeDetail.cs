using Newtonsoft.Json;

namespace MamaFit.BusinessObjects.DTO.GhtkDto.Fee;

public class GhtkFeeDetail
{
    [JsonProperty("name")]
    public string Name { get; set; }

    [JsonProperty("fee")]
    public int Fee { get; set; }

    [JsonProperty("insurance_fee")]
    public int InsuranceFee { get; set; }

    [JsonProperty("delivery_type")]
    public string DeliveryType { get; set; }

    [JsonProperty("a")]
    public int? A { get; set; }
    
    [JsonProperty("dt")]
    public string Dt { get; set; }

    [JsonProperty("extFees")]
    public List<GhtkExtFee> ExtFees { get; set; }

    [JsonProperty("delivery")]
    public bool Delivery { get; set; }
}