using AutoMapper;
using MamaFit.BusinessObjects.DTO.NotificationDto;
using MamaFit.BusinessObjects.DTO.OrderDto;
using MamaFit.BusinessObjects.Entity;
using MamaFit.BusinessObjects.Enum;
using MamaFit.Repositories.Implement;
using MamaFit.Repositories.Infrastructure;
using MamaFit.Repositories.Interface;
using MamaFit.Services.Interface;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace MamaFit.Services.Service;

public class OrderService : IOrderService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IValidationService _validation;
    private readonly INotificationService _notificationService;

    public OrderService(IUnitOfWork unitOfWork, IMapper mapper, IValidationService validation, INotificationService notificationService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _validation = validation;
        _notificationService = notificationService;
    }

    public async Task UpdateOrderStatusAsync(string id, OrderStatus orderStatus, PaymentStatus paymentStatus)
    {
        var order = await _unitOfWork.OrderRepository.GetByIdNotDeletedAsync(id);
        _validation.CheckNotFound(order, "Order not found");
        order.PaymentStatus = paymentStatus;
        order.Status = orderStatus;
        await _unitOfWork.OrderRepository.UpdateAsync(order);
        await _unitOfWork.SaveChangesAsync();
        
        var notification = new NotificationRequestDto()
        {
            ReceiverId = order.UserId,
            NotificationTitle = "Order Status Updated",
            NotificationContent = $"Your order with code {order.Code} has been updated to {orderStatus.ToString().ToLowerInvariant()}.",
            Type = NotificationType.ORDER_PROGRESS,
            ActionUrl = $"/order/{order.Id}",
            Metadata = JsonConvert.SerializeObject(new { orderId = order.Id, orderCode = order.Code }),
        };
        
        await _notificationService.SendAndSaveNotificationAsync(notification);
    }
    public async Task<PaginatedList<OrderResponseDto>> GetAllAsync(int index, int pageSize, DateTime? startDate, DateTime? endDate)
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

    public async Task<OrderResponseDto> GetOrderByIdAsync(string id)
    {
        var order = await _unitOfWork.OrderRepository.GetByIdNotDeletedAsync(id);
        _validation.CheckNotFound(order, "Order not found");
        return _mapper.Map<OrderResponseDto>(order);
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
        var notification = new NotificationRequestDto()
        {
            ReceiverId = model.UserId,
            NotificationTitle = "New Order Created",
            NotificationContent = $"Order with code {order.Code} has been created.",
            Type = NotificationType.ORDER_PROGRESS,
            ActionUrl = $"/order/{order.Id}",
            Metadata = JsonConvert.SerializeObject(new { orderId = order.Id, orderCode = order.Code }),
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

    public async Task CreateReadyToBuyOrderAsync(OrderReadyToBuyRequestDto request)
    {
        var user = await _unitOfWork.UserRepository.GetByIdAsync(request.UserId!);
        _validation.CheckNotFound(user, $"User with id: {request.UserId} not found");

        var voucher = await _unitOfWork.VoucherDiscountRepository.GetByIdAsync(request.VoucherDiscountId);

        var dressDetails = new List<MaternityDressDetail>();

        foreach (var item in request.OrderItems)
        {
            var dress = await _unitOfWork.MaternityDressDetailRepository.GetByIdAsync(item.MaternityDressDetailId!);
            if (dress == null)
            {
                throw new ErrorException(StatusCodes.Status404NotFound, ApiCodes.NOT_FOUND, $"Dress with id:{item.MaternityDressDetailId} not found!");
            }

            if (dress.Quantity < item.Quantity)
            {
                throw new ErrorException(StatusCodes.Status400BadRequest, ApiCodes.BAD_REQUEST, $"Not enough stock for dress with id: {item.MaternityDressDetailId}");
            }

            dress.Quantity -= item.Quantity;
            await _unitOfWork.MaternityDressDetailRepository.UpdateAsync(dress);

            dressDetails.Add(_mapper.Map<MaternityDressDetail>(dress));
        }

        var subTotalAmount = request.OrderItems.Sum(item =>
        {
            var dress = dressDetails.FirstOrDefault(d => d.Id == item.MaternityDressDetailId);
            return dress != null ? dress.Price * item.Quantity : 0;
        });
        var shippingFee = request.DeliveryMethod == DeliveryMethod.DELIVERY ? 30000 : 0;

        var order = _mapper.Map<Order>(request);

        order.User = user!;
        order.VoucherDiscount = voucher;
        order.Type = OrderType.NORMAL;
        order.Code = GenerateOrderCode();
        order.Status = OrderStatus.CREATED;
        order.ShippingFee = shippingFee;
        order.SubTotalAmount = subTotalAmount;
        order.TotalAmount = (subTotalAmount - ((voucher?.VoucherBatch?.DiscountPercentValue ?? 0) * subTotalAmount) + shippingFee);
        order.PaymentStatus = PaymentStatus.PENDING;
        order.PaymentType = PaymentType.FULL;
        order.OrderItems = dressDetails.Select(d => new OrderItem
        {
            MaternityDressDetailId = d.Id,
            ItemType = ItemType.READY_TO_BUY,
            Price = d.Price,
            Quantity = request.OrderItems.First(i => i.MaternityDressDetailId == d.Id).Quantity
        }).ToList();

        if (request.DeliveryMethod == DeliveryMethod.DELIVERY && request.IsOnline)
        {
            var address = await _unitOfWork.AddressRepository.GetByIdAsync(request.AddressId!);
            _validation.CheckNotFound(address, $"Address with id: {request.AddressId} not found");

            order.Address = new OrderAddress
            {
                MapId = address.MapId,
                ShortName = address.ShortName,
                LongName = address.LongName,
                Latitude = address.Latitude,
                Longitude = address.Longitude,
            };
            await _unitOfWork.OrderRepository.InsertAsync(order);
            await _unitOfWork.SaveChangesAsync();
            return;
        }

        if (request.DeliveryMethod == DeliveryMethod.PICK_UP)
        {
            var branch = await _unitOfWork.BranchRepository.GetByIdAsync(request.BranchId!);
            _validation.CheckNotFound(branch, $"Branch with id: {request.BranchId} not found");
            order.Branch = branch;
            order.Address = new OrderAddress
            {
                MapId = branch.Address!.MapId,
                ShortName = branch.Address.ShortName,
                LongName = branch.Address.LongName,
                Latitude = branch.Address.Latitude,
                Longitude = branch.Address.Longitude,
            };
        }
        await _unitOfWork.OrderRepository.InsertAsync(order);
        await _unitOfWork.SaveChangesAsync();
    }

    private string GenerateOrderCode()
    {
        string prefix = "ORD";
        string datePart = DateTime.UtcNow.ToString("yyyyMMdd");
        string randomPart = new Random().Next(1000, 9999).ToString();
        return $"{prefix}-{datePart}-{randomPart}";
    }
}