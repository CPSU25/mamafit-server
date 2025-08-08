using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using MamaFit.Services.Hubs;
using MamaFit.Services.ExternalService.SignalR;
using System.Security.Claims;

namespace MamaFit.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SignalRTestController : ControllerBase
    {
        private readonly IHubContext<UnifiedHub> _hubContext;
        private readonly IUserConnectionManager _connectionManager;
        private readonly ILogger<SignalRTestController> _logger;

        public SignalRTestController(
            IHubContext<UnifiedHub> hubContext,
            IUserConnectionManager connectionManager,
            ILogger<SignalRTestController> logger)
        {
            _hubContext = hubContext;
            _connectionManager = connectionManager;
            _logger = logger;
        }

        [HttpGet("test-auth")]
        [Authorize]
        public IActionResult TestAuthentication()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var userName = User.FindFirst(ClaimTypes.Name)?.Value;
            var role = User.FindFirst(ClaimTypes.Role)?.Value;

            return Ok(new
            {
                Message = "Authentication successful",
                UserId = userId,
                UserName = userName,
                Role = role,
                Claims = User.Claims.Select(c => new { c.Type, c.Value }).ToList()
            });
        }

        [HttpGet("connections")]
        [Authorize]
        public async Task<IActionResult> GetConnections()
        {
            try
            {
                var onlineUsers = await _connectionManager.GetOnlineUsersAsync();
                var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var currentUserConnections = currentUserId != null 
                    ? await _connectionManager.GetUserConnectionsAsync(currentUserId)
                    : new List<string>();

                return Ok(new
                {
                    TotalOnlineUsers = onlineUsers.Count,
                    OnlineUsers = onlineUsers,
                    CurrentUserId = currentUserId,
                    CurrentUserConnections = currentUserConnections
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting connections");
                return StatusCode(500, new { Error = ex.Message });
            }
        }

        [HttpPost("test-broadcast")]
        [Authorize]
        public async Task<IActionResult> TestBroadcast([FromBody] string message)
        {
            try
            {
                await _hubContext.Clients.All.SendAsync("TestMessage", new
                {
                    Message = message,
                    Timestamp = DateTime.UtcNow,
                    From = User.FindFirst(ClaimTypes.Name)?.Value ?? "Unknown"
                });

                return Ok(new { Message = "Broadcast sent successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error broadcasting message");
                return StatusCode(500, new { Error = ex.Message });
            }
        }

        [HttpPost("test-user-message/{targetUserId}")]
        [Authorize]
        public async Task<IActionResult> TestUserMessage(string targetUserId, [FromBody] string message)
        {
            try
            {
                var targetConnections = await _connectionManager.GetUserConnectionsAsync(targetUserId);
                
                if (!targetConnections.Any())
                {
                    return NotFound(new { Message = "Target user is not connected" });
                }

                await _hubContext.Clients.Clients(targetConnections).SendAsync("TestMessage", new
                {
                    Message = message,
                    Timestamp = DateTime.UtcNow,
                    From = User.FindFirst(ClaimTypes.Name)?.Value ?? "Unknown",
                    Type = "DirectMessage"
                });

                return Ok(new { Message = $"Message sent to user {targetUserId} ({targetConnections.Count} connections)" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending user message");
                return StatusCode(500, new { Error = ex.Message });
            }
        }

        [HttpGet("connection-info")]
        public IActionResult GetConnectionInfo()
        {
            return Ok(new
            {
                HubEndpoint = "/unifiedHub",
                AuthenticationRequired = true,
                Instructions = new
                {
                    Step1 = "First get JWT token from /api/Auth/login",
                    Step2 = "Connect to SignalR with URL: ws://localhost:port/unifiedHub?access_token=YOUR_JWT_TOKEN",
                    Step3 = "Listen for events: 'OrderStatusChanged', 'TaskStatusChanged', 'PaymentStatusChanged', 'NotificationReceived', 'ChatMessageReceived'",
                    TestEndpoints = new
                    {
                        TestAuth = "GET /api/SignalRTest/test-auth (with Bearer token)",
                        GetConnections = "GET /api/SignalRTest/connections (with Bearer token)",
                        TestBroadcast = "POST /api/SignalRTest/test-broadcast (with Bearer token)",
                        TestUserMessage = "POST /api/SignalRTest/test-user-message/{userId} (with Bearer token)"
                    }
                }
            });
        }
    }
}