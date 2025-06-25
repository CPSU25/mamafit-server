using MamaFit.BusinessObjects.DTO.SepayDto;

namespace MamaFit.Services.ExternalService.Sepay;

public interface ISepayService
{
    Task ProcessPaymentWebhookAsync(SepayWebhookPayload payload);
    Task<SepayQrResponse> CreatePaymentQrAsync(string orderId, string callbackUrl);
}