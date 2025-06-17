using System.Text.Json;
using MamaFit.Repositories.Helper;
using MamaFit.Repositories.Infrastructure;
using MamaFit.Services.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using RestSharp;
using RestSharp.Authenticators;

namespace MamaFit.Services.Service;

public class EmailSenderService : IEmailSenderSevice
{
    private readonly EmailSettings _emailSettings;

    public EmailSenderService(IOptions<EmailSettings> emailSettings)
    {
        _emailSettings = emailSettings.Value;
    }

    public async Task SendEmailAsync(string email, string subject, string message)
    {
        var options = new RestClientOptions(_emailSettings.ApiBaseUri)
        {
            Authenticator = new HttpBasicAuthenticator("api", _emailSettings.ApiKey)
        };
        
        var client = new RestClient(options);
        
        var request = new RestRequest($"/v3/{_emailSettings.Domain}/messages", Method.Post);
        request.AddParameter("from", _emailSettings.FromEmail);
        request.AddParameter("to", email);
        request.AddParameter("subject", subject);
        request.AddParameter("html", message);
        
        var response = await client.ExecuteAsync(request);
        
        if (!response.IsSuccessful)
        {
            if (response.Content != null)
            {
                string errorMsg = response.Content;

                // Parse lấy trường "message" (nếu content là JSON)
                try
                {
                    using var doc = JsonDocument.Parse(response.Content);
                    if (doc.RootElement.TryGetProperty("message", out var messageElement))
                    {
                        errorMsg = messageElement.GetString()!;
                    }
                }
                catch
                {
                    // Nếu không parse được JSON, giữ nguyên errorMsg
                }

                if (errorMsg != null)
                    throw new ErrorException(StatusCodes.Status500InternalServerError, ApiCodes.INTERNAL_SERVER_ERROR,
                        errorMsg);
            }
        }

    }
}