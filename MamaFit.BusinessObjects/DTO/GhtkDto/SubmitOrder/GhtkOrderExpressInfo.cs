using Newtonsoft.Json;

namespace MamaFit.BusinessObjects.DTO.GhtkDto;

public class GhtkOrderExpressInfo
{
    [JsonProperty("id")]
    public string? Id { get; set; }
    [JsonProperty("pick_address_id")]
    public string PickAddressId { get; set; }
    [JsonProperty("pick_name")]
    public string PickName { get; set; }
    [JsonProperty("pick_address")]
    public string PickAddress { get; set; }
    [JsonProperty("pick_province")]
    public string PickProvince { get; set; }
    [JsonProperty("pick_district")]
    public string PickDistrict { get; set; }
    [JsonProperty("pick_tel")]
    public string PickTel { get; set; }
    [JsonProperty("tel")]
    public string Tel { get; set; }
    [JsonProperty("name")]
    public string Name { get; set; }
    [JsonProperty("address")]
    public string Address { get; set; }
    [JsonProperty("province")]
    public string Province { get; set; }
    [JsonProperty("district")]
    public string District { get; set; }
    [JsonProperty("ward")]
    public string Ward { get; set; }
    [JsonProperty("hamlet")]
    public string Hamlet { get; set; } = "Khác";
    [JsonProperty("weight_option")]
    public string WeightOption { get; set; } = "gram";
    [JsonProperty("is_freeship")]
    public string IsFreeship { get; set; }
    [JsonProperty("pick_money")]
    public decimal PickMoney { get; set; }
    [JsonProperty("note")]
    public string Note { get; set; }
    [JsonProperty("value")]
    public decimal Value { get; set; }
    [JsonProperty("transport")]
    public string Transport { get; set; } = "road";
}