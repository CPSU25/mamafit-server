using MamaFit.BusinessObjects.DTO.GhtkDto.Response;

namespace MamaFit.Services.ExternalService.Ghtk;

public interface IGhtkService
{
    Task<GhtkBaseResponse> AuthenticateGhtkAsync();
}