namespace MamaFit.Services.ExternalService.ExpoNotification;

public interface IExpoNotificationService
{
    Task<HttpResponseMessage> SendPushAsync(string expoPushToken, string title, string body);
}