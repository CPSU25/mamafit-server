using MamaFit.BusinessObjects.DTO.GhtkDto.Response;
using Newtonsoft.Json;

namespace MamaFit.BusinessObjects.DTO.GhtkDto.SubmitOrder;

public class GhtkOrderSubmitSuccessResponse : GhtkBaseResponse
{
    [JsonProperty("order")]
    public GhtkOrderData? Order { get; set; }
}

public class GhtkOrderData
{
    [JsonProperty("partner_id")]
    public string PartnerId { get; set; }
    [JsonProperty("label")]
    public string Label { get; set; }
    [JsonProperty("area")]
    public string Area { get; set; }
    [JsonProperty("fee")]
    public string Fee { get; set; }
    [JsonProperty("insurance_fee")]
    public string InsuranceFee { get; set; }
    [JsonProperty("tracking_id")]
    public long TrackingId { get; set; }
    [JsonProperty("estimated_pick_time")]
    public string EstimatedPickTime { get; set; }
    [JsonProperty("estimated_deliver_time")]
    public string EstimatedDeliverTime { get; set; }
    [JsonProperty("products")]
    public List<OrderProductDto> Products { get; set; }
    [JsonProperty("status_id")]
    public int StatusId { get; set; }
}
