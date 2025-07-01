using AutoMapper;
using MamaFit.BusinessObjects.DTO.GhtkDto.Response;
using MamaFit.Repositories.Helper;
using MamaFit.Repositories.Implement;
using MamaFit.Services.Interface;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace MamaFit.Services.ExternalService.Ghtk;

public class GhtkService : IGhtkService
{
    private readonly IOrderService _orderService;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidationService _validationService;
    private readonly IMapper _mapper;
    
    public GhtkService(IOrderService orderService, 
        IUnitOfWork unitOfWork,
        IValidationService validationService,
        IMapper mapper,
        IHttpClientFactory httpClientFactory)
    {
        _orderService = orderService;
        _unitOfWork = unitOfWork;
        _validationService = validationService;
        _mapper = mapper;
        _httpClientFactory = httpClientFactory;
    }
    
    public async Task<GhtkBaseResponse?> AuthenticateGhtkAsync()
    {
        var httpClient = _httpClientFactory.CreateClient("GhtkClient");
        var response = await httpClient.GetAsync("/services/authenticated");
        var content = await response.Content.ReadAsStringAsync();

        var baseResp = JsonConvert.DeserializeObject<GhtkBaseResponse>(content);
        if (baseResp.Success)
            return JsonConvert.DeserializeObject<GhtkSuccessResponse>(content);
        return JsonConvert.DeserializeObject<GhtkFailResponse>(content);
    }

}