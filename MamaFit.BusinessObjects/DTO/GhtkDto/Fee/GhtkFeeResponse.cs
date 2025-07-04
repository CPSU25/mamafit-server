using MamaFit.BusinessObjects.DTO.GhtkDto.Response;
using Newtonsoft.Json;

namespace MamaFit.BusinessObjects.DTO.GhtkDto.Fee;

public class GhtkFeeResponse : GhtkBaseResponse
{
    [JsonProperty("fee")]
    public GhtkFeeDetail Fee { get; set; }
}