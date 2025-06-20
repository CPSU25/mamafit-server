using Newtonsoft.Json;

namespace MamaFit.Services.ExternalService.ExpoNotification;

public class ExpoNotificationService : IExpoNotificationService
{
    private readonly HttpClient _client;

    public ExpoNotificationService(HttpClient client)
    {
        _client = client;
    }

    public async Task<HttpResponseMessage> SendPushAsync(string expoPushToken, string title, string body)
    {
        var message = new
        {
            to = expoPushToken,
            title = title,
            body = body,
            sound = "default",
        };

        var json = JsonConvert.SerializeObject(message);
        var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
        var response = await _client.PostAsync("https://exp.host/--/api/v2/push/send", content);
        
        return response;
    }
}