using MamaFit.BusinessObjects.DTO.GhtkDto.Response;
using Newtonsoft.Json;

namespace MamaFit.BusinessObjects.DTO.GhtkDto.GhtkAddressLevel4Response;

public class GhtkListPickAddressResponse : GhtkBaseResponse
{
    [JsonProperty("data")]
    public List<GhtkPickAddress> Data { get; set; }
}