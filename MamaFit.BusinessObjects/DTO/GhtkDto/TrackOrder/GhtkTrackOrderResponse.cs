using MamaFit.BusinessObjects.DTO.GhtkDto.Response;
using Newtonsoft.Json;

namespace MamaFit.BusinessObjects.DTO.GhtkDto.TrackOrder;

public class GhtkTrackOrderResponse : GhtkBaseResponse
{
    [JsonProperty("order")]
    public GhtkTrackOrderInfo? Order { get; set; }
}

public class GhtkTrackOrderInfo
{
    [JsonProperty("label_id")]
    public string LabelId { get; set; }
    [JsonProperty("partner_id")]
    public string PartnerId { get; set; }
    [JsonProperty("status")]
    public string Status { get; set; }
    [JsonProperty("status_text")]
    public string StatusText { get; set; }
    [JsonProperty("created")]
    public string Created { get; set; }
    [JsonProperty("modified")]
    public string Modified { get; set; }
    [JsonProperty("message")]
    public string OrderMessage { get; set; } 
    [JsonProperty("pick_date")]
    public string PickDate { get; set; }
    [JsonProperty("deliver_date")]
    public string DeliverDate { get; set; }
    [JsonProperty("customer_fullname")]
    public string CustomerFullname { get; set; }
    [JsonProperty("customer_tel")]
    public string CustomerTel { get; set; }
    [JsonProperty("address")]
    public string Address { get; set; }
    [JsonProperty("storage_day")]
    public int? StorageDay { get; set; }
    [JsonProperty("ship_money")]
    public int? ShipMoney { get; set; }
    [JsonProperty("insurance")]
    public int? Insurance { get; set; }
    [JsonProperty("value")]
    public int? Value { get; set; }
    [JsonProperty("weight")]
    public int? Weight { get; set; }
    [JsonProperty("pick_money")]
    public int? PickMoney { get; set; }
    [JsonProperty("is_freeship")]
    public int? IsFreeShip { get; set; }
}