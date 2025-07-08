using AutoMapper;
using MamaFit.BusinessObjects.DTO.GhtkDto;
using MamaFit.BusinessObjects.DTO.GhtkDto.Fee;
using MamaFit.BusinessObjects.DTO.GhtkDto.GhtkAddressLevel4Response;
using MamaFit.BusinessObjects.DTO.GhtkDto.Response;
using MamaFit.BusinessObjects.DTO.GhtkDto.SubmitOrder;
using MamaFit.BusinessObjects.DTO.GhtkDto.TrackOrder;
using MamaFit.Repositories.Helper;
using MamaFit.Repositories.Implement;
using MamaFit.Repositories.Infrastructure;
using MamaFit.Services.Interface;
using Microsoft.AspNetCore.Http;
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
    private readonly GhtkSettings _ghtkSettings;

    public GhtkService(IOrderService orderService,
        IUnitOfWork unitOfWork,
        IValidationService validationService,
        IMapper mapper,
        IHttpClientFactory httpClientFactory,
        IOptions<GhtkSettings> ghtkSettings)
    {
        _orderService = orderService;
        _unitOfWork = unitOfWork;
        _validationService = validationService;
        _mapper = mapper;
        _httpClientFactory = httpClientFactory;
        _ghtkSettings = ghtkSettings.Value;
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

        var ghtkProducts = order.OrderItems.Select(item =>
        {
            if (item.MaternityDressDetailId != null)
            {
                return new GhtkProductDto
                {
                    Name = item.MaternityDressDetail?.Name ?? "Đầm bầu",
                    Weight = item.MaternityDressDetail?.Weight ?? 0,
                    Quantity = item.Quantity
                };
            }
            if (item.PresetId != null)
            {
                return new GhtkProductDto
                {
                    Name = item.Preset?.Name ?? "Preset thiết kế",
                    Weight = item.Preset?.Weight ?? 0,
                    Quantity = item.Quantity
                };
            }
            throw new ErrorException(
                StatusCodes.Status400BadRequest,
                ApiCodes.INVALID_INPUT,
                "Order item must have either MaternityDressDetail or Preset");
        }).ToList();

        var mappedProducts = order.OrderItems.Select(oi => {
            if (oi.MaternityDressDetailId != null)
            {
                return new OrderProductDto
                {
                    Name = oi.MaternityDressDetail?.Name ?? "Đầm bầu",
                    Weight = oi.MaternityDressDetail?.Weight ?? 0,
                    Quantity = oi.Quantity,
                    Price = oi.MaternityDressDetail?.Price ?? 0
                };
            } 
            if (oi.PresetId != null)
            {
                return new OrderProductDto
                {
                    Name = oi.Preset?.Name ?? "Preset thiết kế",
                    Weight = oi.Preset?.Weight ?? 0,
                    Quantity = oi.Quantity,
                    Price = oi.Preset?.Price ?? 0
                };
            }
            throw new ErrorException(
                StatusCodes.Status400BadRequest,
                ApiCodes.INVALID_INPUT,
                "Order item must have either MaternityDressDetail or Preset");
        }).ToList();

        _validationService.CheckBadRequest(ghtkProducts.Count == 0, "Order must have at least one item");
        var value = order.OrderItems.Sum(item =>
            ((item.MaternityDressDetail?.Price) ?? (item.Preset?.Price) ?? 0) * item.Quantity
        );
        var ghtkOrder = new GhtkOrderExpressInfo
        {
            Id = order.Code,
            PickAddressId = _ghtkSettings.PickAddressId,
            PickName = _ghtkSettings.PickName,
            PickAddress = _ghtkSettings.PickAddress,
            PickProvince = _ghtkSettings.PickProvince,
            PickDistrict = _ghtkSettings.PickDistrict,
            PickTel = _ghtkSettings.PickTel,
            Tel = recipent.CustomerPhone,
            Name = recipent.CustomerName,
            Address = recipent.CustomerAddress,
            Province = recipent.Province,
            District = recipent.District,
            Ward = recipent.Ward,
            Note = recipent.Note ?? "Giao hàng MamaFit",
            Value = value!,
            Transport = recipent.Transport,
            DeliveryOption = recipent.DeliveryOption,
            IsFreeship = recipent.IsFreeship
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

    public async Task<GhtkBaseResponse?> GetFeeAsync(GhtkFeeRequestDto dto)
    {
        var httpClient = _httpClientFactory.CreateClient("GhtkClient");

        string pickProvince = _ghtkSettings.PickProvince;
        string pickDistrict = _ghtkSettings.PickDistrict;
        string pickAddressId = _ghtkSettings.PickAddressId;

        var query = $"province={Uri.EscapeDataString(dto.Province)}" +
                    $"&district={Uri.EscapeDataString(dto.District)}" +
                    $"&weight={dto.Weight}" +
                    $"&deliver_option=none" +
                    $"&pick_province={Uri.EscapeDataString(pickProvince)}" +
                    $"&pick_district={Uri.EscapeDataString(pickDistrict)}" +
                    $"&pick_address_id={Uri.EscapeDataString(pickAddressId)}";
        if (!string.IsNullOrEmpty(dto.DeliveryOption))
            query += $"&delivery_option={Uri.EscapeDataString(dto.DeliveryOption)}";
        if (!string.IsNullOrEmpty(dto.Transport))
            query += $"&transport={Uri.EscapeDataString(dto.Transport)}";
        var url = $"/services/shipment/fee?{query}";
        var response = await httpClient.GetAsync(url);
        var content = await response.Content.ReadAsStringAsync();
        var baseResponse = JsonConvert.DeserializeObject<GhtkBaseResponse>(content);
        if (baseResponse.Success)
            return JsonConvert.DeserializeObject<GhtkFeeResponse>(content);
        return JsonConvert.DeserializeObject<GhtkFailResponse>(content);
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

    public async Task<GhtkAddressLevel4Response?> GetAddressLevel4Async(string province, string district,
        string wardStreet, string? address = null)
    {
        var httpClient = _httpClientFactory.CreateClient("GhtkClient");
        var query =
            $"province={Uri.EscapeDataString(province)}&district={Uri.EscapeDataString(district)}&ward_street={Uri.EscapeDataString(wardStreet)}";
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