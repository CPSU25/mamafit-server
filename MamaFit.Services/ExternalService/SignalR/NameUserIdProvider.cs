using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace MamaFit.Services.ExternalService.SignalR
{
    public class NameUserIdProvider : IUserIdProvider
    {
        public string? GetUserId(HubConnectionContext connection)
        {
            // Match the same logic used in ChatHub.GetCurrentUserId()
            return connection.User?.FindFirst("userId")?.Value
                ?? connection.User?.FindFirst("sub")?.Value
                ?? connection.User?.FindFirst("nameid")?.Value
                ?? connection.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        }
    }
}
