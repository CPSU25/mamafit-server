using System.Globalization;
using System.Text;
using AutoMapper;
using Contentful.Core;
using Contentful.Core.Models.Management;
using MamaFit.BusinessObjects.DTO.CartItemDto;
using MamaFit.BusinessObjects.DTO.CMSDto;
using MamaFit.BusinessObjects.DTO.NotificationDto;
using MamaFit.BusinessObjects.DTO.OrderDto;
using MamaFit.BusinessObjects.DTO.OrderItemDto;
using MamaFit.BusinessObjects.Entity;
using MamaFit.BusinessObjects.Enum;
using MamaFit.Repositories.Helper;
using MamaFit.Repositories.Implement;
using MamaFit.Repositories.Infrastructure;
using MamaFit.Services.ExternalService.Redis;
using MamaFit.Services.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace MamaFit.Services.Service;

public class OrderService : IOrderService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IValidationService _validation;
    private readonly IHttpContextAccessor _contextAccessor;
    private readonly INotificationService _notificationService;
    private readonly HttpClient _httpClient;
    private readonly ContentfulClient _contentfulClient;
    private readonly IConfigurationSection _contentfulSettings;
    private readonly IConfiguration _configuration;
    private readonly ICacheService _cacheService;
    private readonly IConfigService _configService;
    private readonly IEmailSenderSevice _emailSenderService;

    public OrderService(IUnitOfWork unitOfWork, IMapper mapper, IValidationService validation,
        INotificationService notificationService, IHttpContextAccessor contextAccessor, HttpClient httpClient,
        IConfiguration configuration, ICacheService cacheService, IConfigService configService,
        IEmailSenderSevice emailSenderService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _validation = validation;
        _contextAccessor = contextAccessor;
        _notificationService = notificationService;
        _emailSenderService = emailSenderService;
        _httpClient = httpClient;
        _configuration = configuration;
        _contentfulSettings = configuration.GetSection("Contentful");
        var spaceId = _contentfulSettings!.GetSection("SpaceId").Value;
        var contentKey = _contentfulSettings!.GetSection("ContentDeliveryKey").Value;
        var entryId = _contentfulSettings!.GetSection("EntryId").Value;
        _contentfulClient = new ContentfulClient(httpClient, contentKey, null, spaceId, false);
        _cacheService = cacheService;
        _configService = configService;
    }

    public async Task<List<OrderResponseDto>> GetOrdersForAssignedStaffAsync()
    {
        var userId = GetCurrentUserId();
        var orders = await _unitOfWork.OrderRepository.GetOrdersByAssignedStaffAsync(userId);

        return orders.Select(order =>
        {
            var orderDto = _mapper.Map<OrderResponseDto>(order);
            orderDto.Items = order.OrderItems.Select(oi => _mapper.Map<OrderItemResponseDto>(oi)).ToList();
            return orderDto;
        }).ToList();
    }

    public async Task<OrderResponseDto> GetBySkuAndCodeAsync(string code, string? sku = null)
    {
        var order = await _unitOfWork.OrderRepository.GetBySkuAndCodeAsync(code, sku);
        _validation.CheckNotFound(order, "Order not found");

        var dto = _mapper.Map<OrderResponseDto>(order);

        if (dto.Items != null && order?.OrderItems != null)
        {
            var filteredOrderItems = order.OrderItems.AsEnumerable();

            if (!string.IsNullOrEmpty(sku))
            {
                filteredOrderItems = order.OrderItems.Where(item =>
                    (item.Preset?.SKU != null && item.Preset.SKU.Equals(sku, StringComparison.OrdinalIgnoreCase)) ||
                    (item.MaternityDressDetail?.SKU != null &&
                     item.MaternityDressDetail.SKU.Equals(sku, StringComparison.OrdinalIgnoreCase))
                );
            }

            var filteredOrderItemsList = filteredOrderItems.ToList();

            // Nếu có SKU nhưng không tìm thấy OrderItem nào khớp
            if (!string.IsNullOrEmpty(sku) && !filteredOrderItemsList.Any())
            {
                throw new ErrorException(StatusCodes.Status404NotFound, ApiCodes.NOT_FOUND,
                    $"No order item found with SKU: {sku} in order {code}");
            }

            // Cập nhật lại Items trong DTO để chỉ chứa filtered items
            dto.Items = filteredOrderItemsList.Select(item => _mapper.Map<OrderItemResponseDto>(item)).ToList();

            // Lấy tất cả original order item IDs từ filtered items
            var orderItemIds = filteredOrderItemsList.Select(x => x.Id).ToList();

            // Lấy warranty rounds cho tất cả original order items
            var warrantyRounds = await _unitOfWork.WarrantyRequestItemRepository
                .GetWarrantyRoundsByOrderItemIdsAsync(orderItemIds);

            foreach (var itemDto in dto.Items)
            {
                var entityItem = filteredOrderItemsList.FirstOrDefault(x => x.Id == itemDto.Id);

                if (entityItem != null)
                {
                    // Xác định original order item ID
                    string originalOrderItemId = entityItem.ParentOrderItemId ?? entityItem.Id;

                    // Lấy warranty round cao nhất cho original order item này
                    if (warrantyRounds.ContainsKey(originalOrderItemId))
                    {
                        itemDto.WarrantyRound = warrantyRounds[originalOrderItemId];
                    }
                    else
                    {
                        itemDto.WarrantyRound = 0;
                    }
                }
                else
                {
                    itemDto.WarrantyRound = 0;
                }
            }
        }

        return dto;
    }


    public async Task<List<OrderResponseDto>> GetOrdersForBranchManagerAsync()
    {
        var userId = GetCurrentUserId();
        var orders = await _unitOfWork.OrderRepository.GetOrdersByBranchManagerAsync(userId);

        return orders.Select(order =>
        {
            var orderDto = _mapper.Map<OrderResponseDto>(order);
            orderDto.Items = order.OrderItems.Select(oi => _mapper.Map<OrderItemResponseDto>(oi)).ToList();
            return orderDto;
        }).ToList();
    }

    public async Task<List<OrderResponseDto>> GetOrdersForDesignerAsync()
    {
        var userId = GetCurrentUserId();
        var orders = await _unitOfWork.OrderRepository.GetOrdersByDesignerAsync(userId);

        return orders.Select(order =>
        {
            var orderDto = _mapper.Map<OrderResponseDto>(order);
            orderDto.Items = order.OrderItems.Select(oi => _mapper.Map<OrderItemResponseDto>(oi)).ToList();
            return orderDto;
        }).ToList();
    }

    public async Task<PaginatedList<OrderResponseDto>> GetByTokenAsync(string accessToken, int index = 1,
        int pageSize = 10, string? search = null, OrderStatus? status = null)
    {
        var userId = JwtTokenHelper.ExtractUserId(accessToken);
        var orders = await _unitOfWork.OrderRepository.GetByTokenAsync(index, pageSize, userId, search, status);
        var responseItems = orders.Items
            .Select(order => _mapper.Map<OrderResponseDto>(order))
            .ToList();
        return new PaginatedList<OrderResponseDto>(
            responseItems,
            orders.TotalCount,
            orders.PageNumber,
            pageSize
        );
    }

    public async Task<List<OrderResponseDto>> GetOrderToFeedback()
    {
        var userId = GetCurrentUserId();
        var myOrder = await _unitOfWork.OrderRepository.GetByTokenAsync(userId);
        myOrder = myOrder
            .Where(x => x.OrderItems.All(x => x.Feedbacks.Count() <= 0) && x.Status == OrderStatus.COMPLETED).ToList();

        return myOrder.Select(x => _mapper.Map<OrderResponseDto>(x)).ToList();
    }

    public async Task UpdateOrderStatusAsync(string id, OrderStatus orderStatus, PaymentStatus paymentStatus)
    {
        var order = await _unitOfWork.OrderRepository.GetByIdNotDeletedAsync(id);
        _validation.CheckNotFound(order, "Order not found");

        if (order.Status == OrderStatus.COMPLETED &&
            (order.PaymentStatus == PaymentStatus.PAID_FULL ||
             order.PaymentStatus == PaymentStatus.PAID_DEPOSIT_COMPLETED))
        {
            throw new ErrorException(StatusCodes.Status400BadRequest, ApiCodes.BAD_REQUEST,
                "Order already completed and paid.");
        }

        order.Status = orderStatus;
        order.PaymentStatus = paymentStatus;

        await _unitOfWork.OrderRepository.UpdateAsync(order);
        await _unitOfWork.SaveChangesAsync();

        string notificationContent = paymentStatus switch
        {
            PaymentStatus.PAID_DEPOSIT =>
                $"Tiền cọc cho đơn hàng {order.Code} đã được xác nhận. Đơn hàng hiện đang ở trạng thái {orderStatus.ToString().ToLowerInvariant()}.",
            PaymentStatus.PAID_DEPOSIT_COMPLETED =>
                $"Đơn hàng {order.Code} đã được thanh toán đầy đủ. Đơn hàng hiện đang ở trạng thái {orderStatus.ToString().ToLowerInvariant()}.",
            PaymentStatus.PAID_FULL =>
                $"Thanh toán đầy đủ cho đơn hàng {order.Code} đã được xác nhận. Đơn hàng hiện đang ở trạng thái {orderStatus.ToString().ToLowerInvariant()}.",
            _ =>
                $"Trạng thái đơn hàng {order.Code} đã được cập nhật thành {orderStatus.ToString().ToLowerInvariant()}.",
        };

        await _notificationService.SendAndSaveNotificationToMultipleAsync(new NotificationMultipleRequestDto
        {
            ReceiverIds =
            [
                order.UserId,
            ],
            NotificationTitle = "Câp nhật trạng thái đơn hàng",
            NotificationContent = notificationContent,
            Type = NotificationType.ORDER_PROGRESS,
            ActionUrl = $"/order/{order.Id}",
            Metadata = new Dictionary<string, string>
            {
                { "orderId", order.Id },
                { "paymentStatus", paymentStatus.ToString() },
                { "orderStatus", orderStatus.ToString() }
            }
        });
    }

    public async Task<PaginatedList<OrderResponseDto>> GetAllAsync(int index, int pageSize, DateTime? startDate,
        DateTime? endDate)
    {
        var orders = await _unitOfWork.OrderRepository.GetAllAsync(index, pageSize, startDate, endDate);
        var responseItems = orders.Items
            .Select(order => _mapper.Map<OrderResponseDto>(order))
            .ToList();
        return new PaginatedList<OrderResponseDto>(
            responseItems,
            orders.TotalCount,
            orders.PageNumber,
            pageSize
        );
    }

    public async Task<OrderGetByIdResponseDto> GetOrderByIdAsync(string id)
    {
        var order = await _unitOfWork.OrderRepository.GetByIdWithItems(id);
        _validation.CheckNotFound(order, "Order not found");
        return _mapper.Map<OrderGetByIdResponseDto>(order);
    }

    public async Task<OrderResponseDto> CreateOrderAsync(OrderRequestDto model)
    {
        await _validation.ValidateAndThrowAsync(model);
        var user = await _unitOfWork.UserRepository.GetByIdAsync(model.UserId);
        _validation.CheckNotFound(user, "User not found");
        var order = _mapper.Map<Order>(model);
        order.Code = GenerateOrderCode();
        await _unitOfWork.OrderRepository.InsertAsync(order);
        await _unitOfWork.SaveChangesAsync();
        var notification = new NotificationRequestDto
        {
            ReceiverId = model.UserId,
            NotificationTitle = "Tạo đơn hàng thành công",
            NotificationContent = $"Đơn hàng {order.Code} đã được tạo thành công.",
            Type = NotificationType.ORDER_PROGRESS,
            ActionUrl = $"/order/{order.Id}",
            Metadata = new Dictionary<string, string>
            {
                { "orderId", order.Id },
                { "orderCode", order.Code },
                { "status", order.Status.ToString() }
            }
        };

        await _notificationService.SendAndSaveNotificationAsync(notification);
        return _mapper.Map<OrderResponseDto>(order);
    }

    public async Task<OrderResponseDto> UpdateOrderAsync(string id, OrderRequestDto model)
    {
        await _validation.ValidateAndThrowAsync(model);
        var order = await _unitOfWork.OrderRepository.GetByIdNotDeletedAsync(id);
        _validation.CheckNotFound(order, "Order not found");
        var user = await _unitOfWork.UserRepository.GetByIdAsync(model.UserId);
        _validation.CheckNotFound(user, "User not found");
        order = _mapper.Map(model, order);
        await _unitOfWork.OrderRepository.UpdateAsync(order);
        await _unitOfWork.SaveChangesAsync();
        return _mapper.Map<OrderResponseDto>(order);
    }

    public async Task<bool> DeleteOrderAsync(string id)
    {
        var order = await _unitOfWork.OrderRepository.GetByIdNotDeletedAsync(id);
        _validation.CheckNotFound(order, "Order not found");
        await _unitOfWork.OrderRepository.SoftDeleteAsync(id);
        await _unitOfWork.SaveChangesAsync();
        return true;
    }

    public async Task<string> CreateReadyToBuyOrderAsync(OrderReadyToBuyRequestDto request)
    {
        await _validation.ValidateAndThrowAsync(request);

        var userId = GetCurrentUserId();
        var user = await _unitOfWork.UserRepository.GetByIdAsync(userId);
        _validation.CheckNotFound(user, $"User with id: {userId} not found");

        var cacheKey = $"cart:user:{userId}";
        var isCartExist = await _cacheService.KeyExistsAsync(cacheKey);
        if (!isCartExist)
            throw new ErrorException(StatusCodes.Status404NotFound, ApiCodes.NOT_FOUND,
                $"User with id: {userId} has no cart item");

        VoucherDiscount? voucher = null;
        Measurement? measurement = null;

        if (request.VoucherDiscountId != null)
        {
            voucher = await _unitOfWork.VoucherDiscountRepository
                .GetVoucherDiscountWithBatch(request.VoucherDiscountId);
            _validation.CheckNotFound(voucher, $"Voucher with id: {request.VoucherDiscountId} not found");
        }

        if (request.MeasurementId != null)
        {
            measurement = await _unitOfWork.MeasurementRepository.GetByIdAsync(request.MeasurementId);
            _validation.CheckNotFound(measurement, $"Measurement diary with id: {request.MeasurementId} not found");
        }

        var dressDetails = new List<MaternityDressDetail>();
        var orderItems = new List<OrderItem>();
        decimal addOnListTotalPrice = 0;
        decimal subTotalAmount = 0;

        foreach (var ri in request.OrderItems)
        {
            var itemKey = $"item:{ri.MaternityDressDetailId}";
            var cartItem = await _cacheService.GetHashAsync<CartItem>(cacheKey, itemKey);
            _validation.CheckNotFound(cartItem,
                $"Maternity dress with id: {ri.MaternityDressDetailId} not found in cart");

            var dress = await _unitOfWork.MaternityDressDetailRepository
                .GetByIdAsync(cartItem!.MaternityDressDetail!.Id!);
            _validation.CheckNotFound(dress, $"Maternity dress with id: {ri.MaternityDressDetailId} not found");

            if (dress.Quantity < ri.Quantity)
                throw new ErrorException(StatusCodes.Status400BadRequest, ApiCodes.BAD_REQUEST,
                    $"Not enough stock for dress with id: {ri.MaternityDressDetailId}");

            dress.Quantity -= ri.Quantity;
            await _unitOfWork.MaternityDressDetailRepository.UpdateAsync(dress);
            dressDetails.Add(dress);

            var orderItemAddOns = new List<OrderItemAddOnOption>();
            if (ri.Options is not null)
            {
                foreach (var opt in ri.Options)
                {
                    var addOn = await _unitOfWork.AddOnOptionRepository.GetByIdAsync(opt.AddOnOptionId!);
                    _validation.CheckNotFound(addOn, $"Add-on option with id: {opt.AddOnOptionId} not found");

                    orderItemAddOns.Add(new OrderItemAddOnOption
                    {
                        AddOnOptionId = addOn.Id,
                        AddOnOption = addOn,
                        Value = opt.Value
                    });

                    addOnListTotalPrice += addOn.Price * ri.Quantity;
                }
            }

            orderItems.Add(new OrderItem
            {
                MaternityDressDetailId = dress.Id,
                ItemType = ItemType.READY_TO_BUY,
                Price = dress.Price,
                Quantity = ri.Quantity,
                OrderItemAddOnOptions = orderItemAddOns,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                CreatedBy = user!.UserName
            });

            subTotalAmount += dress.Price * ri.Quantity;
        }

        decimal? discountValue = 0;
        if (voucher != null && voucher.VoucherBatch != null)
        {
            discountValue = CalculateVoucherDiscount(voucher.VoucherBatch, subTotalAmount);

            if (discountValue > 0)
            {
                voucher.Status = VoucherStatus.USED;
                await _unitOfWork.VoucherDiscountRepository.UpdateAsync(voucher);
            }
        }

        var merchandiseAfterDiscount = subTotalAmount - discountValue;
        var totalAmount = merchandiseAfterDiscount + request.ShippingFee + addOnListTotalPrice;
        var depositSubtotal = merchandiseAfterDiscount / 2;

        var order = _mapper.Map<Order>(request);
        order.VoucherDiscountId = request.VoucherDiscountId;
        order.User = user!;
        order.VoucherDiscount = voucher;
        order.Type = OrderType.NORMAL;
        order.Code = GenerateOrderCode();
        order.Status = OrderStatus.CREATED;
        order.Measurement = measurement;
        order.SubTotalAmount = subTotalAmount;
        order.DiscountSubtotal = discountValue;
        order.ShippingFee = request.ShippingFee;
        order.TotalAmount = totalAmount;
        order.PaymentStatus = PaymentStatus.PENDING;
        order.ServiceAmount = addOnListTotalPrice;
        order.OrderItems = orderItems;

        if (request.PaymentType == PaymentType.DEPOSIT)
        {
            order.PaymentType = PaymentType.DEPOSIT;
            order.DepositSubtotal = depositSubtotal;
            order.RemainingBalance = merchandiseAfterDiscount - order.DepositSubtotal;
            order.TotalPaid = depositSubtotal + addOnListTotalPrice + request.ShippingFee;
        }
        else
        {
            order.PaymentType = PaymentType.FULL;
            order.DepositSubtotal = null;
            order.RemainingBalance = null;
            order.TotalPaid = totalAmount;
        }

        if (request.DeliveryMethod == DeliveryMethod.DELIVERY)
        {
            if (request.AddressId == null)
                throw new ErrorException(StatusCodes.Status400BadRequest, ApiCodes.BAD_REQUEST,
                    "Address ID is required for delivery orders.");

            var address = await _unitOfWork.AddressRepository.GetByIdAsync(request.AddressId);
            _validation.CheckNotFound(address, $"Address with id: {request.AddressId} not found");
            order.Address = address;
        }
        else if (request.DeliveryMethod == DeliveryMethod.PICK_UP)
        {
            if (request.BranchId == null)
                throw new ErrorException(StatusCodes.Status400BadRequest, ApiCodes.BAD_REQUEST,
                    "Branch ID is required for pick-up orders.");

            var branch = await _unitOfWork.BranchRepository.GetByIdAsync(request.BranchId);
            _validation.CheckNotFound(branch, $"Branch with id: {request.BranchId} not found");
            order.Branch = branch;
        }

        await _unitOfWork.OrderRepository.InsertAsync(order);
        await _unitOfWork.SaveChangesAsync();

        foreach (var ri in request.OrderItems)
        {
            var itemRemoveKey = $"item:{ri.MaternityDressDetailId}";
            await _cacheService.DeleteHashFieldAsync(cacheKey, itemRemoveKey);
        }

        return order.Id;
    }


    private static readonly Random _random = new Random();

    private string GenerateOrderCode()
    {
        string prefix = "ORD";
        string randomPart = _random.Next(1000000, 9999999).ToString();
        return $"{prefix}{randomPart}";
    }


    private string GetCurrentUserId()
    {
        return _contextAccessor.HttpContext?.User?.FindFirst("userId")?.Value ?? string.Empty;
    }

    public async Task<string> CreateDesignRequestOrderAsync(OrderDesignRequestDto request)
    {
        var userId = GetCurrentUserId();
        var user = await _unitOfWork.UserRepository.GetByIdAsync(userId);
        _validation.CheckNotFound(user, $"User with id: {userId} not found");

        decimal? designFee;

        var response = await _cacheService.GetAsync<CmsServiceBaseDto>("cms:service:base");

        if (response?.Fields == null)
        {
            var entryId = _contentfulSettings!.GetSection("EntryId").Value;
            var contentfulResponse = await _contentfulClient.GetEntry<CmsFieldDto>(entryId);
            var request1 = new CmsServiceBaseDto
            {
                Fields = contentfulResponse
            };
            await _cacheService.SetAsync("cms:service:base", request1);

            response = request1;
        }

        designFee = response?.Fields.DesignRequestServiceFee;
        if (designFee == 0)
        {
            throw new ErrorException(StatusCodes.Status500InternalServerError, ApiCodes.INTERNAL_SERVER_ERROR,
                "Design request service fee not found in CMS.");
        }

        var config = await _configService.GetConfig();

        var order = new Order
        {
            UserId = userId,
            User = user!,
            Type = OrderType.DESIGN,
            Code = GenerateOrderCode(),
            Status = OrderStatus.CREATED,
            IsOnline = true,
            TotalAmount = designFee,
            SubTotalAmount = designFee,
            PaymentStatus = PaymentStatus.PENDING,
            PaymentMethod = PaymentMethod.ONLINE_BANKING,
            PaymentType = PaymentType.FULL,
            OrderItems = new List<OrderItem>
            {
                new OrderItem
                {
                    DesignRequest = new DesignRequest
                    {
                        Images = request.Images ?? new List<string>(),
                        Description = request.Description ?? string.Empty,
                        User = user,
                        CreatedAt = DateTime.UtcNow,
                        CreatedBy = user!.UserName,
                        UpdatedAt = DateTime.UtcNow
                    },
                    ItemType = ItemType.DESIGN_REQUEST,
                    Price = (decimal)designFee!,
                    Quantity = 1,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    CreatedBy = user!.UserName
                }
            }
        };

        await _unitOfWork.OrderRepository.InsertAsync(order);
        await _unitOfWork.SaveChangesAsync();
        return order.Id;
    }

    public async Task<string> CreatePresetOrderAsync(OrderPresetCreateRequestDto request)
    {
        var userId = GetCurrentUserId();
        var user = await _unitOfWork.UserRepository.GetByIdAsync(userId);
        _validation.CheckNotFound(user, $"User with id: {userId} not found");

        var cacheKey = $"cart:user:{userId}";
        var isCartExist = await _cacheService.KeyExistsAsync(cacheKey);

        if (!isCartExist)
            throw new ErrorException(StatusCodes.Status404NotFound, ApiCodes.NOT_FOUND,
                $"User with id: {userId} has no cart item");

        VoucherDiscount? voucher = null;
        Measurement? measurement = null;

        if (request.VoucherDiscountId != null)
        {
            voucher = await _unitOfWork.VoucherDiscountRepository
                .GetVoucherDiscountWithBatch(request.VoucherDiscountId);
            _validation.CheckNotFound(voucher, $"Voucher with id: {request.VoucherDiscountId} not found");
        }

        if (request.MeasurementId != null)
        {
            measurement = await _unitOfWork.MeasurementRepository.GetByIdAsync(request.MeasurementId);
            _validation.CheckNotFound(measurement,
                $"Measurement diary with id: {request.MeasurementId} not found");
        }

        var orderItems = new List<OrderItem>();
        decimal addOnListTotalPrice = 0;
        decimal subTotalAmount = 0;

        // Tạo OrderItem theo preset và add-on tương ứng
        foreach (var presetReq in request.Presets)
        {
            var itemKey = $"item:{presetReq.Id}";
            var cartItem = await _cacheService.GetHashAsync<CartItem>(cacheKey, itemKey);
            _validation.CheckNotFound(cartItem, $"Preset with id: {presetReq.Id} not found in cart");
            var preset = await _unitOfWork.PresetRepository.GetDetailById(cartItem.Preset.Id);
            _validation.CheckNotFound(preset, $"Preset with id: {cartItem.Preset.Id} not found in database");

            var orderItemAddOns = new List<OrderItemAddOnOption>();

            if (presetReq.Options is not null)
            {
                foreach (var option in presetReq.Options)
                {
                    var addOn = await _unitOfWork.AddOnOptionRepository.GetByIdAsync(option.AddOnOptionId!);
                    _validation.CheckNotFound(addOn, $"Add-on option with id: {option.AddOnOptionId} not found");

                    orderItemAddOns.Add(new OrderItemAddOnOption
                    {
                        AddOnOptionId = addOn.Id,
                        AddOnOption = addOn,
                        Value = option.Value
                    });

                    addOnListTotalPrice += addOn.Price * presetReq.Quantity;
                }
            }

            orderItems.Add(new OrderItem
            {
                Preset = preset,
                PresetId = preset.Id,
                ItemType = ItemType.PRESET,
                Price = preset.Price,
                Quantity = presetReq.Quantity,
                OrderItemAddOnOptions = orderItemAddOns,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                CreatedBy = user!.UserName
            });

            subTotalAmount += preset.Price * presetReq.Quantity;
            await _cacheService.DeleteHashFieldAsync(cacheKey, itemKey);
        }

        // Tính discount nếu có voucher
        decimal? discountValue = 0;
        if (voucher?.VoucherBatch != null)
        {
            discountValue = CalculateVoucherDiscount(voucher.VoucherBatch, subTotalAmount);

            if (discountValue > 0)
            {
                voucher.Status = VoucherStatus.USED;
                await _unitOfWork.VoucherDiscountRepository.UpdateAsync(voucher);
            }
        }

        var merchandiseAfterDiscount = subTotalAmount - discountValue;
        var totalAmount = merchandiseAfterDiscount + request.ShippingFee + addOnListTotalPrice;
        var depositSubtotal = merchandiseAfterDiscount / 2;

        var config = await _configService.GetConfig();

        // Map sang Order entity
        var order = _mapper.Map<Order>(request);
        order.User = user!;
        order.VoucherDiscount = voucher;
        order.Type = OrderType.NORMAL;
        order.VoucherDiscountId = request.VoucherDiscountId ?? null;
        order.Code = GenerateOrderCode();
        order.Status = OrderStatus.CREATED;
        order.Measurement = measurement;
        order.SubTotalAmount = subTotalAmount; // Merchandise subtotal gốc
        order.DiscountSubtotal = discountValue;
        order.ShippingFee = request.ShippingFee;
        order.TotalAmount = totalAmount;
        order.PaymentStatus = PaymentStatus.PENDING;
        order.ServiceAmount = addOnListTotalPrice;
        order.OrderItems = orderItems;

        if (request.PaymentType == PaymentType.DEPOSIT)
        {
            order.PaymentType = PaymentType.DEPOSIT;
            order.DepositSubtotal = depositSubtotal;
            order.RemainingBalance = merchandiseAfterDiscount - order.DepositSubtotal;
            order.TotalPaid = depositSubtotal + addOnListTotalPrice + request.ShippingFee;
        }
        else
        {
            order.PaymentType = PaymentType.FULL;
            order.DepositSubtotal = null;
            order.RemainingBalance = null;
            order.TotalPaid = totalAmount;
        }

        // Xử lý địa chỉ giao hàng hoặc chi nhánh pick-up
        if (request.DeliveryMethod == DeliveryMethod.DELIVERY)
        {
            if (request.AddressId == null)
                throw new ErrorException(StatusCodes.Status400BadRequest, ApiCodes.BAD_REQUEST,
                    "Address ID is required for delivery orders.");

            var address = await _unitOfWork.AddressRepository.GetByIdAsync(request.AddressId);
            _validation.CheckNotFound(address, $"Address with id: {request.AddressId} not found");
            order.Address = address;
        }
        else if (request.DeliveryMethod == DeliveryMethod.PICK_UP)
        {
            if (request.BranchId == null)
                throw new ErrorException(StatusCodes.Status400BadRequest, ApiCodes.BAD_REQUEST,
                    "Branch ID is required for pick-up orders.");

            var branch = await _unitOfWork.BranchRepository.GetByIdAsync(request.BranchId);
            _validation.CheckNotFound(branch, $"Branch with id: {request.BranchId} not found");
            order.Branch = branch;
        }

        await _unitOfWork.OrderRepository.InsertAsync(order);
        await _unitOfWork.SaveChangesAsync();
        return order.Id;
    }

    public async Task UpdateReceivedOrderAsync(string id)
    {
        var order = await _unitOfWork.OrderRepository.GetByIdNotDeletedAsync(id);
        _validation.CheckNotFound(order, "Order not found");

        if (order.Status != OrderStatus.DELIVERING
            && order.Status != OrderStatus.RECEIVED_AT_BRANCH)
        {
            throw new ErrorException(StatusCodes.Status400BadRequest, ApiCodes.BAD_REQUEST,
                "Order is not in a state that can be received.");
        }

        order.Status = OrderStatus.COMPLETED;
        order.ReceivedAt = DateTime.UtcNow;

        await _unitOfWork.OrderRepository.UpdateAsync(order);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task UpdateCancelledOrderAsync(string id, string? cancelReason = null)
    {
        var order = await _unitOfWork.OrderRepository.GetByIdNotDeletedAsync(id);
        _validation.CheckNotFound(order, "Order not found");

        if (order.Status != OrderStatus.CREATED)
        {
            throw new ErrorException(StatusCodes.Status400BadRequest, ApiCodes.BAD_REQUEST,
                "Order is not in a state that can be cancelled.");
        }

        order.CanceledReason = cancelReason;
        order.Status = OrderStatus.CANCELLED;

        await _unitOfWork.OrderRepository.UpdateAsync(order);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task WebhookForContentfulWhenUpdateData(CmsServiceBaseDto request)
    {
        await _cacheService.SetAsync("cms:service:base", request, TimeSpan.FromDays(30));
        await _cacheService.RemoveByPrefixAsync("appointment_slots");
    }

    public async Task<List<MyOrderStatusCount>> GetMyOrderStatusCounts()
    {
        var userId = GetCurrentUserId();
        var user = await _unitOfWork.UserRepository.GetByIdAsync(userId);
        _validation.CheckNotFound(user, $"User with id: {userId} not found");

        List<MyOrderStatusCount>? myOrderStatusCounts = new List<MyOrderStatusCount>();

        foreach (OrderStatus status in Enum.GetValues(typeof(OrderStatus)))
        {
            int count = user.Orders.Count(x => x.Status == status && x.IsDeleted == false);
            var statusCount = new MyOrderStatusCount
            {
                OrderStatus = status,
                OrderNumber = count
            };

            myOrderStatusCounts.Add(statusCount);
        }

        return myOrderStatusCounts;
    }

    public async Task OrderReceivedAtUpdateAsync(string orderId)
    {
        var order = await _unitOfWork.OrderRepository.GetByIdNotDeletedAsync(orderId);
        order.ReceivedAt = DateTime.UtcNow;

        await _unitOfWork.OrderRepository.UpdateAsync(order);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task<List<OrderGetByIdResponseDto>> GetForWarranty()
    {
        var userId = GetCurrentUserId();
        var user = await _unitOfWork.UserRepository.GetByIdAsync(userId);
        _validation.CheckNotFound(user, " Please sign in");
        var config = await _configService.GetConfig();
        var result = await _unitOfWork.OrderRepository.GetOrderForRequest(userId);

        return _mapper.Map<List<OrderGetByIdResponseDto>>(result);
    }

    public async Task<List<OrderGetByIdResponseDto>> GetAllByDesignRequestId(string designRequestId)
    {
        var orderList = await _unitOfWork.OrderRepository.GetAllOrderByDesignRequestId(designRequestId);
        _validation.CheckNotFound(orderList, $"Design request with Id: {designRequestId} has no order");

        var response = orderList.Select(x => _mapper.Map<OrderGetByIdResponseDto>(x)).ToList();
        return response;
    }

    public async Task<OrderGetByIdResponseDto> GetOrderByIdForFeedbackAsync(string id)
    {
        var order = await _unitOfWork.OrderRepository.GetByIdWithItems(id);

        if (order.Type == OrderType.WARRANTY)
            order.OrderItems = order.OrderItems.Where(x =>
                x.WarrantyRequestItems.Any(x =>
                    x.Status != WarrantyRequestItemStatus.REJECTED)).ToList();

        return _mapper.Map<OrderGetByIdResponseDto>(order);
    }

    private static decimal CalculateVoucherDiscount(VoucherBatch batch, decimal subTotalAmount)
    {
        if (batch == null) return 0m;

        var minOrder = batch.MinimumOrderValue.HasValue ? batch.MinimumOrderValue.Value : 0m;
        if (subTotalAmount <= 0m || subTotalAmount < minOrder) return 0m;

        decimal rawDiscount = 0m;
        if (batch.DiscountType == DiscountType.PERCENTAGE)
        {
            var percent = (batch.DiscountValue ?? 0) / 100m;
            if (percent <= 0m) return 0m;
            rawDiscount = subTotalAmount * percent;
        }
        else if (batch.DiscountType == DiscountType.FIXED)
        {
            rawDiscount = batch.DiscountValue ?? 0;
        }

        if (rawDiscount <= 0m) return 0m;

        if (batch.MaximumDiscountValue.HasValue)
        {
            var cap = batch.MaximumDiscountValue.Value;
            if (cap >= 0m) rawDiscount = Math.Min(rawDiscount, cap);
        }

        // Không vượt quá subtotal
        return Math.Min(rawDiscount, subTotalAmount);
    }

    public async Task ReceivedAtBranchAsync(string orderId)
    {
        var order = await _unitOfWork.OrderRepository.GetByIdNotDeletedAsync(orderId);
        _validation.CheckNotFound(order, $"Order with id: {orderId} not found");
        var branch = await _unitOfWork.BranchRepository.GetByIdAsync(order.BranchId);

        if (branch == null)
        {
            throw new ErrorException(StatusCodes.Status400BadRequest, ApiCodes.BAD_REQUEST,
                $"Order {orderId} does not have an associated branch");
        }

        if (string.IsNullOrEmpty(branch.Name))
        {
            throw new ErrorException(StatusCodes.Status400BadRequest, ApiCodes.BAD_REQUEST,
                $"Branch associated with order {orderId} does not have a name");
        }

        if (string.IsNullOrEmpty(order.Branch.BranchManagerId))
        {
            throw new ErrorException(StatusCodes.Status400BadRequest, ApiCodes.BAD_REQUEST,
                $"Branch {order.Branch.Name} does not have a branch manager assigned");
        }

        order.Status = OrderStatus.RECEIVED_AT_BRANCH;
        order.ReceivedAtBranch = DateTime.UtcNow;

        // 6. Tạo notification với safe access
        var notification = new NotificationMultipleRequestDto
        {
            NotificationTitle = "Order has been received at branch",
            NotificationContent = $"Đơn hàng đã được nhận tại chi nhánh {branch.Name}",
            Type = NotificationType.ORDER_PROGRESS,
            ReceiverIds = new List<string>
            {
                order.UserId,
                branch.BranchManagerId,
                "4c9804ecc1d645de96fcfc906cc43d6c", // Sample receiver ID (could be a customer service or admin)
                "1a3bcd12345678901234567890123456" // Sample receiver ID (admin or other relevant)
            },
            ActionUrl = $"/order/{order.Id}",
            Metadata = new Dictionary<string, string>
            {
                { "orderId", order.Id },
                { "branchId", order.Branch.Id },
                { "status", OrderStatus.RECEIVED_AT_BRANCH.ToString() }
            }
        };

        // Send notification
        await _notificationService.SendAndSaveNotificationToMultipleAsync(notification);

        // 7. Send email to customer about the order status update
        await SendOrderReceivedAtBranchEmailAsync(order);

        // 8. Update order status and save changes to the database
        await _unitOfWork.OrderRepository.UpdateAsync(order);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task CompleteOrderAtBranch(string orderId)
    {
        var order = await _unitOfWork.OrderRepository.GetByIdNotDeletedAsync(orderId);
        _validation.CheckNotFound(order, $"Order with id: {orderId} not found");
        var branch = await _unitOfWork.BranchRepository.GetByIdAsync(order.BranchId);
        if (branch == null)
        {
            throw new ErrorException(StatusCodes.Status400BadRequest, ApiCodes.BAD_REQUEST,
                $"Order {orderId} does not have an associated branch");
        }

        if (string.IsNullOrEmpty(branch.Name))
        {
            throw new ErrorException(StatusCodes.Status400BadRequest, ApiCodes.BAD_REQUEST,
                $"Branch associated with order {orderId} does not have a name");
        }

        if (string.IsNullOrEmpty(order.Branch.BranchManagerId))
        {
            throw new ErrorException(StatusCodes.Status400BadRequest, ApiCodes.BAD_REQUEST,
                $"Branch {order.Branch.Name} does not have a branch manager assigned");
        }

        order.Status = OrderStatus.COMPLETED;

        // 6. Tạo notification với safe access
        var notification = new NotificationMultipleRequestDto
        {
            NotificationTitle = $"Đơn hàng {order.Code} đã được hoàn thành",
            NotificationContent = $"Đơn hàng đã được hoàn thành tại chi nhánh {branch.Name}",
            Type = NotificationType.ORDER_PROGRESS,
            ReceiverIds = new List<string>
            {
                order.UserId,
                branch.BranchManagerId,
                "4c9804ecc1d645de96fcfc906cc43d6c",
                "1a3bcd12345678901234567890123456"
            },
            ActionUrl = $"/order/{order.Id}",
            Metadata = new Dictionary<string, string>
            {
                { "orderId", order.Id },
                { "branchId", order.Branch.Id },
                { "status", OrderStatus.COMPLETED.ToString() }
            }
        };

        await _notificationService.SendAndSaveNotificationToMultipleAsync(notification);
        await _unitOfWork.OrderRepository.UpdateAsync(order);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task SendOrderReceivedAtBranchEmailAsync(Order order)
    {
        var email = order.User.UserEmail;
        if (string.IsNullOrWhiteSpace(email))
            throw new ErrorException(StatusCodes.Status400BadRequest, ApiCodes.BAD_REQUEST,
                "User email is not set, cannot send email");

        var subject = $"[MamaFit] Đơn hàng {order.Code} đã sẵn sàng tại chi nhánh";

        // Custom message for order received at branch
        var html = BuildOrderReceivedAtBranchHtml(order);

        await _emailSenderService.SendEmailAsync(email, subject, html);
    }

    private static string BuildOrderReceivedAtBranchHtml(Order order)
    {
        var vn = new CultureInfo("vi-VN");

        var itemsHtml = new StringBuilder();
        if (order?.OrderItems != null)
        {
            foreach (var it in order.OrderItems)
            {
                var name =
                    it.Preset?.Name ??
                    it.MaternityDressDetail?.Name ??
                    (it.DesignRequest != null ? "Yêu cầu thiết kế" : "Váy bầu");
                itemsHtml.Append($@"
            <tr>
                <td style=""padding:8px 0"">{name}</td>
                <td style=""padding:8px 0; text-align:center"">{it.Quantity}</td>
                <td style=""padding:8px 0; text-align:right"">{(it.Price).ToString("c0", vn)}</td>
            </tr>");
            }
        }

        var preheader = $"Thông báo về trạng thái đơn hàng {order?.Code}";

        return $@"
    <!DOCTYPE html>
    <html lang=""vi"">
    <head>
    <meta charset=""UTF-8"">
    <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
    <title>Thông báo đơn hàng nhận tại chi nhánh</title>
    <style>
    body {{ font-family: Arial, Helvetica, sans-serif; background:#f7f7f7; margin:0; padding:0; }}
    .container {{ max-width: 600px; margin:40px auto; background:#fff; border-radius:8px; box-shadow:0 2px 10px rgba(0,0,0,0.05); padding:24px; }}
    .brand {{ font-size:22px; font-weight:bold; color:#2266cc; text-align:center; margin-bottom:6px; }}
    .sub {{ text-align:center; color:#666; margin-bottom:16px; }}
    .section-title {{ font-size:16px; font-weight:bold; margin:18px 0 8px; }}
    .table {{ width:100%; border-collapse:collapse; }}
    .table th, .table td {{ border-bottom:1px solid #eee; padding:8px 0; font-size:14px; }}
    .right {{ text-align:right; }}
    .footer {{ margin-top:24px; font-size:12px; color:#888; text-align:center; }}
    .badge {{ display:inline-block; padding:6px 10px; background:#e8f3ff; color:#2266cc; border-radius:999px; font-size:12px; }}
    .total-row td {{ font-weight:bold; }}
    </style>
    </head>
    <body>
    <span style=""display:none!important;"">{preheader}</span>
    <div class=""container"">
        <div class=""brand"">MamaFit</div>
        <div class=""sub""><span class=""badge"">Đơn hàng đã sẵn sàng</span></div>

        <div class=""section-title"">Thông tin đơn hàng</div>
        <table class=""table"">
            <tr><td>Mã đơn</td><td class=""right"">{order?.Code}</td></tr>
            <tr><td>Trạng thái</td><td class=""right"">Sẵn sàng tại chi nhánh</td></tr>
            <tr><td>Chi nhánh</td><td class=""right"">{order.Branch?.Name}</td></tr>
        </table>

        <div class=""section-title"">Chi tiết sản phẩm</div>
        <table class=""table"">
            <thead>
                <tr><th style=""text-align:left"">Sản phẩm</th><th>Số lượng</th><th class=""right"">Đơn giá</th></tr>
            </thead>
            <tbody>
                {itemsHtml}
            </tbody>
            <tfoot>
                <tr><td colspan=""2"" class=""right"">Tạm tính</td><td class=""right"">{order.SubTotalAmount?.ToString("c0", vn) ?? "0"}</td></tr>
            </tfoot>
        </table>

        <div class=""footer"">
            Nếu có sai sót, vui lòng phản hồi email này hoặc liên hệ MamaFit để được hỗ trợ.<br/>
            &copy; {DateTime.Now.Year} MamaFit. All rights reserved.
        </div>
    </div>
    </body>
    </html>";
    }
}