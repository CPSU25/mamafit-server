using Newtonsoft.Json;

namespace MamaFit.BusinessObjects.DTO.GhtkDto.Address;

public class GhtkPickAddress
{
    [JsonProperty("pick_address_id")]
    public string PickAddressId { get; set; }
    [JsonProperty("address")]
    public string Address { get; set; }
    [JsonProperty("pick_tel")]
    public string PickTel { get; set; }
    [JsonProperty("pick_name")]
    public string PickName { get; set; }
}