using MamaFit.BusinessObjects.DTO.GhtkDto.Response;
using Newtonsoft.Json;

namespace MamaFit.BusinessObjects.DTO.GhtkDto.SubmitOrder;

public class GhtkOrderSubmitFailResponse : GhtkBaseResponse
{
    [JsonProperty("error")]
    public GhtkOrderError? Error { get; set; }
    [JsonProperty("log_id")]
    public string? LogId { get; set; }
}

public class GhtkOrderError
{
    [JsonProperty("code")]
    public string Code { get; set; }
    [JsonProperty("partner_id")]
    public string PartnerId { get; set; }
    [JsonProperty("ghtk_label")]
    public string GhtkLabel { get; set; }
    [JsonProperty("created")]
    public string Created { get; set; }
    [JsonProperty("status")]
    public int Status { get; set; }
}
