using AutoMapper;
using Contentful.Core;
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

    public OrderService(IUnitOfWork unitOfWork, IMapper mapper, IValidationService validation,
        INotificationService notificationService, IHttpContextAccessor contextAccessor, HttpClient httpClient,
        IConfiguration configuration, ICacheService cacheService, IConfigService configService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _validation = validation;
        _contextAccessor = contextAccessor;
        _notificationService = notificationService;

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
        var userId = GetCurrentUserId(); // lấy user từ JWT
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

    public async Task UpdateOrderStatusAsync(string id, OrderStatus orderStatus, PaymentStatus paymentStatus)
    {
        var order = await _unitOfWork.OrderRepository.GetByIdNotDeletedAsync(id);
        _validation.CheckNotFound(order, "Order not found");
        
        if (order.Status == OrderStatus.COMPLETED && 
             (order.PaymentStatus == PaymentStatus.PAID_FULL || order.PaymentStatus == PaymentStatus.PAID_DEPOSIT_COMPLETED))
        {
            throw new ErrorException(StatusCodes.Status400BadRequest, ApiCodes.BAD_REQUEST, "Order already completed and paid.");
        }
        
        order.Status = orderStatus;
        order.PaymentStatus = paymentStatus;
        
        await _unitOfWork.OrderRepository.UpdateAsync(order);
        await _unitOfWork.SaveChangesAsync();
        
        string notificationContent = paymentStatus switch
        {
            PaymentStatus.PAID_DEPOSIT =>
                $"Your deposit for order {order.Code} has been received. Order is now {orderStatus.ToString().ToLowerInvariant()}.",
            PaymentStatus.PAID_DEPOSIT_COMPLETED =>
                $"Final payment for order {order.Code} confirmed. Order is now {orderStatus.ToString().ToLowerInvariant()}.",
            PaymentStatus.PAID_FULL =>
                $"Full payment for order {order.Code} confirmed. Order is now {orderStatus.ToString().ToLowerInvariant()}.",
            _ =>
                $"Order {order.Code} status updated to {orderStatus.ToString().ToLowerInvariant()}."
        };

        await _notificationService.SendAndSaveNotificationAsync(new NotificationRequestDto
        {
            ReceiverId = order.UserId,
            NotificationTitle = "Order Status Updated",
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
            NotificationTitle = "New Order Created",
            NotificationContent = $"Order with code {order.Code} has been created.",
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

        var user = await _unitOfWork.UserRepository.GetByIdAsync(request.UserId!);
        _validation.CheckNotFound(user, $"User with id: {request.UserId} not found");

        VoucherDiscount? voucher = null;
        MeasurementDiary? measurement = null;

        if (request.VoucherDiscountId != null)
        {
            voucher = await _unitOfWork.VoucherDiscountRepository.GetByIdAsync(request.VoucherDiscountId);
            _validation.CheckNotFound(voucher, $"Voucher with id: {request.VoucherDiscountId} not found");
        }

        if (request.MeasurementDiaryId != null)
        {
            measurement = await _unitOfWork.MeasurementDiaryRepository.GetByIdAsync(request.MeasurementDiaryId);
            _validation.CheckNotFound(measurement,
                $"Measurement diary with id: {request.MeasurementDiaryId} not found");
        }

        var dressDetails = new List<MaternityDressDetail>();

        foreach (var item in request.OrderItems)
        {
            var dress = await _unitOfWork.MaternityDressDetailRepository.GetByIdAsync(item.MaternityDressDetailId!);
            if (dress == null)
            {
                throw new ErrorException(StatusCodes.Status404NotFound, ApiCodes.NOT_FOUND,
                    $"Dress with id:{item.MaternityDressDetailId} not found!");
            }

            if (dress.Quantity < item.Quantity)
            {
                throw new ErrorException(StatusCodes.Status400BadRequest, ApiCodes.BAD_REQUEST,
                    $"Not enough stock for dress with id: {item.MaternityDressDetailId}");
            }

            dress.Quantity -= item.Quantity;
            await _unitOfWork.MaternityDressDetailRepository.UpdateAsync(dress);

            dressDetails.Add(_mapper.Map<MaternityDressDetail>(dress));
        }

        var config = await _configService.GetConfig();

        var subTotalAmount = request.OrderItems.Sum(item =>
        {
            var dress = dressDetails.FirstOrDefault(d => d.Id == item.MaternityDressDetailId);
            return dress != null ? dress.Price * item.Quantity : 0;
        });

        decimal? discountValue = 0;

        if (voucher != null && voucher.VoucherBatch != null)
        {
            if (voucher.VoucherBatch.DiscountType == DiscountType.PERCENTAGE)
            {
                discountValue = (voucher.VoucherBatch.DiscountValue / 100) * subTotalAmount;
            }
            else if (voucher.VoucherBatch.DiscountType == DiscountType.FIXED)
            {
                discountValue = voucher.VoucherBatch.DiscountValue;
            }

            voucher.Status = VoucherStatus.USED;
            await _unitOfWork.VoucherDiscountRepository.UpdateAsync(voucher);
        }

        // Tính merchandise subtotal sau khi trừ discount
        var merchandiseAfterDiscount = subTotalAmount - discountValue;
        var totalAmount = merchandiseAfterDiscount + request.ShippingFee;

        var order = _mapper.Map<Order>(request);
        order.User = user!;
        order.VoucherDiscount = voucher;
        order.Type = OrderType.NORMAL;
        order.Code = GenerateOrderCode();
        order.Status = OrderStatus.CREATED;
        order.MeasurementDiary = measurement;
        order.SubTotalAmount = subTotalAmount; // Merchandise subtotal gốc
        order.DiscountSubtotal = discountValue;
        order.ShippingFee = request.ShippingFee;
        order.TotalAmount = totalAmount;
        order.PaymentStatus = PaymentStatus.PENDING;

        // Tính toán deposit fields nếu payment type là DEPOSIT
        if (request.PaymentType == PaymentType.DEPOSIT)
        {
            order.PaymentType = PaymentType.DEPOSIT;
            // Chia đôi số tiền merchandise sau khi đã trừ discount
            order.DepositSubtotal = merchandiseAfterDiscount / 2; // 50% của merchandise sau discount
            order.RemainingBalance = merchandiseAfterDiscount - order.DepositSubtotal;
        }
        else
        {
            order.PaymentType = PaymentType.FULL;
            order.DepositSubtotal = null;
            order.RemainingBalance = null;
        }

        order.OrderItems = dressDetails.Select(d => new OrderItem
        {
            MaternityDressDetailId = d.Id,
            ItemType = ItemType.READY_TO_BUY,
            Price = d.Price,
            Quantity = request.OrderItems.First(i => i.MaternityDressDetailId == d.Id).Quantity,
            WarrantyNumber = config.Fields.WarrantyTime,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            CreatedBy = "System"
        }).ToList();

        if (request.DeliveryMethod == DeliveryMethod.DELIVERY)
        {
            if (request.AddressId == null)
                throw new ErrorException(StatusCodes.Status400BadRequest, ApiCodes.BAD_REQUEST,
                    "Address ID is required for delivery orders.");

            var address = await _unitOfWork.AddressRepository.GetByIdAsync(request.AddressId);
            _validation.CheckNotFound(address, $"Address with id: {request.AddressId} not found");

            order.Address = address;
            await _unitOfWork.OrderRepository.InsertAsync(order);
            await _unitOfWork.SaveChangesAsync();
            return order.Id;
        }

        if (request.DeliveryMethod == DeliveryMethod.PICK_UP)
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

    private string GenerateOrderCode()
    {
        string prefix = "O";
        string randomPart = new Random().Next(10000, 99999).ToString();
        return $"{prefix}{randomPart}";
    }

    private string GetCurrentUserId()
    {
        return _contextAccessor.HttpContext?.User?.FindFirst("userId")?.Value ?? string.Empty;
    }

    public async Task<string> CreateDesignRequestOrderAsync(OrderDesignRequestDto request)
    {
        await _validation.ValidateAndThrowAsync(request);

        var userId = GetCurrentUserId();
        var user = await _unitOfWork.UserRepository.GetByIdAsync(userId);
        _validation.CheckNotFound(user, $"User with id: {userId} not found");

        decimal? designFee = 0;

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
            Type = OrderType.NORMAL,
            Code = GenerateOrderCode(),
            Status = OrderStatus.CREATED,
            IsOnline = true,
            TotalAmount = designFee,
            SubTotalAmount = designFee,
            PaymentStatus = PaymentStatus.PENDING,
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
                    WarrantyNumber = config!.Fields!.WarrantyTime,
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
        var preset = await _unitOfWork.PresetRepository.GetDetailById(request.PresetId!);
        _validation.CheckNotFound(preset, $"Preset with id: {request.PresetId} not found");

        var userId = GetCurrentUserId();
        var user = await _unitOfWork.UserRepository.GetByIdAsync(userId);
        _validation.CheckNotFound(user, $"User with id: {userId} not found");

        VoucherDiscount? voucher = null;
        MeasurementDiary? measurement = null;
        List<OrderItemAddOnOption>? addOnOptions = new List<OrderItemAddOnOption>();

        if (request.VoucherDiscountId != null)
        {
            voucher = await _unitOfWork.VoucherDiscountRepository
                .GetVoucherDiscountWithBatch(request.VoucherDiscountId);
            _validation.CheckNotFound(voucher, $"Voucher with id: {request.VoucherDiscountId} not found");
        }

        if (request.MeasurementDiaryId != null)
        {
            measurement = await _unitOfWork.MeasurementDiaryRepository.GetByIdAsync(request.MeasurementDiaryId);
            _validation.CheckNotFound(measurement,
                $"Measurement diary with id: {request.MeasurementDiaryId} not found");
        }

        if (request.Options is not null)
        {
            foreach (var option in request.Options)
            {
                var addOn = await _unitOfWork.AddOnOptionRepository.GetByIdAsync(option.AddOnOptionId!);
                _validation.CheckNotFound(addOn, $"Add-on option with id: {option.AddOnOptionId} not found");
                addOnOptions.Add(new OrderItemAddOnOption
                {
                    AddOnOptionId = addOn.Id,
                    AddOnOption = addOn,
                    Value = option.Value
                });
            }
        }

        var addOnListTotalPrice = addOnOptions.Sum(x => x.AddOnOption!.Price);
        var subTotalAmount = preset!.Price;
        decimal? discountValue = 0;

        if (voucher != null && voucher.VoucherBatch != null)
        {
            if (voucher.VoucherBatch.DiscountType == DiscountType.PERCENTAGE)
            {
                discountValue = (decimal)voucher.VoucherBatch.DiscountValue! / 100 * subTotalAmount;
            }
            else if (voucher.VoucherBatch.DiscountType == DiscountType.FIXED)
            {
                discountValue = voucher.VoucherBatch.DiscountValue;
            }

            if (discountValue > subTotalAmount)
            {
                discountValue = subTotalAmount;
            }
            
            voucher.Status = VoucherStatus.USED;
            await _unitOfWork.VoucherDiscountRepository.UpdateAsync(voucher);
        }

        // Tính merchandise subtotal sau khi trừ discount
        var merchandiseAfterDiscount = subTotalAmount - discountValue;
        var totalAmount = merchandiseAfterDiscount + request.ShippingFee + addOnListTotalPrice;
        var depositSubtotal = merchandiseAfterDiscount / 2;

        var config = await _configService.GetConfig();

        var order = _mapper.Map<Order>(request);
        order.User = user!;
        order.VoucherDiscount = voucher;
        order.Type = OrderType.NORMAL;
        order.VoucherDiscountId = request.VoucherDiscountId ?? null;
        order.Code = GenerateOrderCode();
        order.Status = OrderStatus.CREATED;
        order.MeasurementDiary = measurement;
        order.SubTotalAmount = subTotalAmount; // Merchandise subtotal gốc
        order.DiscountSubtotal = discountValue;
        order.ShippingFee = request.ShippingFee;
        order.TotalAmount = totalAmount;
        order.PaymentStatus = PaymentStatus.PENDING;
        order.ServiceAmount = addOnListTotalPrice;

        // Tính toán deposit fields nếu payment type là DEPOSIT
        if (request.PaymentType == PaymentType.DEPOSIT)
        {
            order.PaymentType = PaymentType.DEPOSIT;
            // Chia đôi số tiền merchandise sau khi đã trừ discount
            order.DepositSubtotal = depositSubtotal; // 50% của merchandise sau discount
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

        order.OrderItems = new List<OrderItem>()
        {
            new OrderItem
            {
                Preset = preset,
                ItemType = ItemType.PRESET,
                OrderItemAddOnOptions = addOnOptions.Select(x => new OrderItemAddOnOption
                {
                    AddOnOptionId = x.AddOnOptionId,
                    AddOnOption = x.AddOnOption,
                    Value = x.Value,
                }).ToList(),
                // Price = preset.ComponentOptionPresets.Sum(co => co.ComponentOption!.Price),
                Price = preset.Price,
                WarrantyNumber = config.Fields!.WarrantyTime,
                Quantity = 1,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                CreatedBy = user!.UserName
            }
        };

        if (request.DeliveryMethod == DeliveryMethod.DELIVERY)
        {
            if (request.AddressId == null)
                throw new ErrorException(StatusCodes.Status400BadRequest, ApiCodes.BAD_REQUEST,
                    "Address ID is required for delivery orders.");

            var address = await _unitOfWork.AddressRepository.GetByIdAsync(request.AddressId);
            _validation.CheckNotFound(address, $"Address with id: {request.AddressId} not found");

            order.Address = address;
            await _unitOfWork.OrderRepository.InsertAsync(order);
            await _unitOfWork.SaveChangesAsync();
            return order.Id;
        }

        if (request.DeliveryMethod == DeliveryMethod.PICK_UP)
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
            int count = user.Orders.Count(x => x.Status == status);
            var statusCount = new MyOrderStatusCount
            {
                OrderStatus = status,
                OrderNumber = count
            };

            myOrderStatusCounts.Add(statusCount);
        }

        return myOrderStatusCounts;
    }
}