using MamaFit.BusinessObjects.DTO.GhtkDto;
using MamaFit.BusinessObjects.DTO.GhtkDto.Fee;
using MamaFit.BusinessObjects.DTO.GhtkDto.GhtkAddressLevel4Response;
using MamaFit.BusinessObjects.DTO.GhtkDto.Response;
using MamaFit.BusinessObjects.DTO.GhtkDto.TrackOrder;

namespace MamaFit.Services.ExternalService.Ghtk;

public interface IGhtkService
{
    Task<GhtkBaseResponse?> AuthenticateGhtkAsync();
    Task<GhtkBaseResponse?> SubmitOrderExpressAsync(string orderId, GhtkRecipentDto recipent);
    Task<GhtkTrackOrderResponse?> GetOrderStatusAsync(string trackingOrderCode);
    Task<GhtkBaseResponse?> CancelOrderAsync(string trackingOrderCode);
    Task<GhtkAddressLevel4Response?> GetAddressLevel4Async(string province, string district, string wardStreet,
        string? address);
    Task<GhtkListPickAddressResponse?> GetListPickAddressAsync();
    Task<GhtkBaseResponse?> GetFeeAsync(GhtkFeeRequestDto dto);
}