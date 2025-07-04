using AutoMapper;
using MamaFit.BusinessObjects.DTO.GhtkDto;
using MamaFit.BusinessObjects.DTO.GhtkDto.Fee;
using MamaFit.BusinessObjects.DTO.GhtkDto.GhtkAddressLevel4Response;
using MamaFit.BusinessObjects.DTO.GhtkDto.Response;
using MamaFit.BusinessObjects.DTO.GhtkDto.SubmitOrder;
using MamaFit.BusinessObjects.DTO.GhtkDto.TrackOrder;
using MamaFit.Repositories.Implement;
using MamaFit.Services.Interface;
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

    public async Task<GhtkBaseResponse?> SubmitOrderExpressAsync(string orderId, GhtkRecipentDto recipent)
    {
        var order = await _unitOfWork.OrderRepository.GetWithItemsAndDressDetails(orderId);
        _validationService.CheckNotFound(order, "Order not found");
        
        var ghtkProducts = order.OrderItems.Select(item => new GhtkProductDto
        {
            Name = item.MaternityDressDetail?.Name ?? "Đầm bầu",
            Weight = item.MaternityDressDetail?.Weight != null ? (int)(item.MaternityDressDetail.Weight) : 0 ,
            Quantity = item.Quantity
        }).ToList();
        
        var mappedProducts = order.OrderItems.Select(oi => new OrderProductDto {
            Name = oi.MaternityDressDetail?.Name,
            Weight = oi.MaternityDressDetail?.Weight,
            Quantity = oi.Quantity,
            Price = oi.MaternityDressDetail?.Price
        }).ToList();
        
        _validationService.CheckBadRequest(ghtkProducts.Count == 0, "Order must have at least one item");
        var value = order.OrderItems.Sum(item => (item.MaternityDressDetail?.Price!) * item.Quantity);
        var ghtkOrder = new GhtkOrderExpressInfo
        {
            Id = order.Code,
            PickAddressId = "8006198",
            PickName = "MamaFit Shop",
            PickAddress = "Trường Đại học FPT",
            PickProvince = "TP. Hồ Chí Minh",
            PickDistrict = "Thủ Đức",
            PickTel = "0377137787",
            Tel = recipent.CustomerPhone,
            Name = recipent.CustomerName,
            Address = recipent.CustomerAddress,
            Province = recipent.Province,
            District = recipent.District,
            Ward = recipent.Ward,
            Note = recipent.Note ?? "Giao hàng MamaFit",
            Value = value!,
            IsFreeship = recipent.IsFreeship,
            Transport = "road"
        };

        var ghtkRequest = new GhtkOrderExpressRequest
        {
            Products = ghtkProducts,
            Order = ghtkOrder
        };

        var httpClient = _httpClientFactory.CreateClient("GhtkClient");
        var json = JsonConvert.SerializeObject(ghtkRequest);
        var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
        var response = await httpClient.PostAsync("/services/shipment/order", content);
        var responseBody = await response.Content.ReadAsStringAsync();

        var baseResp = JsonConvert.DeserializeObject<GhtkBaseResponse>(responseBody);
        if (baseResp.Success)
        {
            var successResp = JsonConvert.DeserializeObject<GhtkOrderSubmitSuccessResponse>(responseBody);
            
            if (successResp?.Order != null)
                successResp.Order.Products = mappedProducts;
            return successResp;
        }
        return JsonConvert.DeserializeObject<GhtkOrderSubmitFailResponse>(responseBody);
    }
    
    public async Task<GhtkFeeResponse?> GetFeeAsync(GhtkFeeRequestDto dto)
    {
        var httpClient = _httpClientFactory.CreateClient("GhtkClient");

        var query = $"province={Uri.EscapeDataString(dto.Province)}" +
                    $"&district={Uri.EscapeDataString(dto.District)}" +
                    $"&weight={dto.Weight}" +
                    $"&deliver_option={Uri.EscapeDataString(dto.DeliverOption)}";

        if (!string.IsNullOrEmpty(dto.Address)) query += $"&address={Uri.EscapeDataString(dto.Address)}";
        if (!string.IsNullOrEmpty(dto.PickProvince)) query += $"&pick_province={Uri.EscapeDataString(dto.PickProvince)}";
        if (!string.IsNullOrEmpty(dto.PickDistrict)) query += $"&pick_district={Uri.EscapeDataString(dto.PickDistrict)}";
        if (!string.IsNullOrEmpty(dto.PickAddressId)) query += $"&pick_address_id={Uri.EscapeDataString(dto.PickAddressId)}";
        if (!string.IsNullOrEmpty(dto.PickAddress)) query += $"&pick_address={Uri.EscapeDataString(dto.PickAddress)}";
        if (!string.IsNullOrEmpty(dto.PickWard)) query += $"&pick_ward={Uri.EscapeDataString(dto.PickWard)}";
        if (!string.IsNullOrEmpty(dto.PickStreet)) query += $"&pick_street={Uri.EscapeDataString(dto.PickStreet)}";
        if (!string.IsNullOrEmpty(dto.Ward)) query += $"&ward={Uri.EscapeDataString(dto.Ward)}";
        if (!string.IsNullOrEmpty(dto.Street)) query += $"&street={Uri.EscapeDataString(dto.Street)}";
        if (dto.Value.HasValue) query += $"&value={dto.Value.Value}";
        if (!string.IsNullOrEmpty(dto.Transport)) query += $"&transport={Uri.EscapeDataString(dto.Transport)}";
        if (dto.Tags != null && dto.Tags.Length > 0) query += $"&tags={Uri.EscapeDataString(string.Join(",", dto.Tags))}";

        var url = $"/services/shipment/fee?{query}";
        var response = await httpClient.GetAsync(url);
        var content = await response.Content.ReadAsStringAsync();

        return JsonConvert.DeserializeObject<GhtkFeeResponse>(content);
    }

    public async Task<GhtkTrackOrderResponse?> GetOrderStatusAsync(string trackingOrderCode)
    {
        var httpClient = _httpClientFactory.CreateClient("GhtkClient");
        var url = $"/services/shipment/v2/{trackingOrderCode}";
        var response = await httpClient.GetAsync(url);
        var content = await response.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<GhtkTrackOrderResponse>(content);
    }
    
    public async Task<GhtkBaseResponse?> CancelOrderAsync(string trackingOrderCode)
    {
        var httpClient = _httpClientFactory.CreateClient("GhtkClient");
        var url = $"/services/shipment/cancel/{trackingOrderCode}";
        var response = await httpClient.PostAsync(url, null);
        var content = await response.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<GhtkBaseResponse>(content);
    }
    
    public async Task<GhtkAddressLevel4Response?> GetAddressLevel4Async(string province, string district, string wardStreet, string? address = null)
    {
        var httpClient = _httpClientFactory.CreateClient("GhtkClient");
        var query = $"province={Uri.EscapeDataString(province)}&district={Uri.EscapeDataString(district)}&ward_street={Uri.EscapeDataString(wardStreet)}";
        if (!string.IsNullOrEmpty(address))
            query += $"&address={Uri.EscapeDataString(address)}";
        var url = $"/services/address/getAddressLevel4?{query}";
        var response = await httpClient.GetAsync(url);
        var content = await response.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<GhtkAddressLevel4Response>(content);
    }

    public async Task<GhtkListPickAddressResponse?> GetListPickAddressAsync()
    {
        var httpClient = _httpClientFactory.CreateClient("GhtkClient");
        var url = "/services/shipment/list_pick_add";
        var response = await httpClient.GetAsync(url);
        var content = await response.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<GhtkListPickAddressResponse>(content);
    }

}