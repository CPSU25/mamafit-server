using AutoMapper;
using MamaFit.BusinessObjects.DTO.GhtkDto.SubmitOrder;
using MamaFit.BusinessObjects.DTO.NotificationDto;
using MamaFit.BusinessObjects.DTO.OrderDto;
using MamaFit.BusinessObjects.DTO.OrderItemDto;
using MamaFit.BusinessObjects.DTO.WarrantyRequestDto;
using MamaFit.BusinessObjects.Entity;
using MamaFit.BusinessObjects.Enum;
using MamaFit.Repositories.Helper;
using MamaFit.Repositories.Implement;
using MamaFit.Repositories.Infrastructure;
using MamaFit.Repositories.Interface;
using MamaFit.Services.ExternalService.Ghtk;
using MamaFit.Services.ExternalService.Redis;
using MamaFit.Services.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace MamaFit.Services.Service
{
    public class WarrantyRequestService : IWarrantyRequestService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IValidationService _validationService;
        private readonly INotificationService _notificationService;
        private readonly IConfigService _configService;
        private readonly IOrderItemService _orderItemService;
        private readonly IWarrantyRequestItemRepository _warrantyRequestItemRepository;
        private readonly IGhtkService _ghtkService;
        private readonly GhtkSettings _ghtkSettings;
        private readonly ICacheService _cacheService;

        public WarrantyRequestService(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IValidationService validationService,
            INotificationService notificationService,
            IConfigService configService,
            IOrderItemService orderItemService,
            IWarrantyRequestItemRepository warrantyRequestItemRepository,
            IGhtkService ghtkService,
            IOptions<GhtkSettings> ghtkSettings,
            ICacheService cacheService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _validationService = validationService;
            _notificationService = notificationService;
            _configService = configService;
            _orderItemService = orderItemService;
            _warrantyRequestItemRepository = warrantyRequestItemRepository;
            _ghtkService = ghtkService;
            _ghtkSettings = ghtkSettings.Value;
            _cacheService = cacheService;
        }


        public async Task<string> CreateAsync(WarrantyRequestCreateDto dto, string accessToken)
        {
            var userId = JwtTokenHelper.ExtractUserId(accessToken);
            var config = await _configService.GetConfig();
            var validOrderItems = new List<OrderItem>();
            var orderItemSKUs = new List<string>();
            var warrantyRounds = new Dictionary<string, int>();

            if (!string.IsNullOrEmpty(dto.AddressId))
            {
                var address = await _unitOfWork.AddressRepository.GetByIdNotDeletedAsync(dto.AddressId);
                _validationService.CheckNotFound(address, $"Address with id {dto.AddressId} not found!");
            }

            if (!string.IsNullOrEmpty(dto.BranchId))
            {
                var branch = await _unitOfWork.BranchRepository.GetByIdNotDeletedAsync(dto.BranchId);
                _validationService.CheckNotFound(branch, $"Branch with id {dto.BranchId} not found!");
            }

            RequestType requestType = RequestType.FREE;
            foreach (var itemDto in dto.Items)
            {
                var orderItem = await _unitOfWork.OrderItemRepository.GetByIdNotDeletedAsync(itemDto.OrderItemId);
                _validationService.CheckNotFound(orderItem, $"Order item {itemDto.OrderItemId} not found!");

                var order = await _unitOfWork.OrderRepository.GetByIdNotDeletedAsync(orderItem.OrderId!);
                _validationService.CheckNotFound(order, $"Order of item {orderItem.Id} not found!");

                if (order.PaymentStatus == PaymentStatus.PENDING || order.PaymentStatus == PaymentStatus.PAID_DEPOSIT)
                    throw new ErrorException(StatusCodes.Status400BadRequest, ApiCodes.BAD_REQUEST,
                        $"Order {order.Code} must be paid before requesting warranty!");
                if (order.ReceivedAt == null)
                    throw new ErrorException(StatusCodes.Status400BadRequest, ApiCodes.BAD_REQUEST,
                        $"Order {order.Code} must be received before requesting warranty!");

                var daysSinceReceived = (DateTime.UtcNow - order.ReceivedAt!.Value).TotalDays;
                var warrantyCount =
                    await _warrantyRequestItemRepository.CountWarrantyRequestItemsAsync(itemDto.OrderItemId);
                warrantyRounds[orderItem.Id] = warrantyCount + 1;
                if (daysSinceReceived > config.Fields?.WarrantyPeriod || warrantyCount >= config.Fields?.WarrantyTime)
                    requestType = RequestType.FEE;

                string? skuOk = orderItem.Preset?.SKU ?? orderItem.MaternityDressDetail?.SKU;
                if (!string.IsNullOrEmpty(skuOk))
                    orderItemSKUs.Add(skuOk);

                validOrderItems.Add(orderItem);
                orderItem.WarrantyDate = DateTime.UtcNow;
                await _unitOfWork.OrderItemRepository.UpdateAsync(orderItem);
            }

            var warrantyOrder = new Order
            {
                UserId = userId,
                AddressId = !string.IsNullOrEmpty(dto.AddressId) ? dto.AddressId : null,
                BranchId = !string.IsNullOrEmpty(dto.BranchId) ? dto.BranchId : null,
                IsOnline = false,
                Type = OrderType.WARRANTY,
                Code = GenerateOrderCode(),
                Status = OrderStatus.CONFIRMED,
                PaymentStatus = PaymentStatus.WARRANTY,
                PaymentType = PaymentType.FULL,
                PaymentMethod = dto.PaymentMethod,
                DeliveryMethod = dto.DeliveryMethod,
            };
            await _unitOfWork.OrderRepository.InsertAsync(warrantyOrder);

            var warrantyOrderItemIdMap = new Dictionary<string, string>();
            foreach (var orderItem in validOrderItems)
            {
                var warrantyOrderItem = new OrderItem
                {
                    OrderId = warrantyOrder.Id,
                    ParentOrderItemId = orderItem.ParentOrderItemId ?? orderItem.Id,
                    MaternityDressDetailId = orderItem.MaternityDressDetailId,
                    Preset = orderItem.Preset,
                    PresetId = orderItem.PresetId,
                    ItemType = ItemType.WARRANTY,
                    Price = orderItem.Price,
                    Quantity = 1,
                };
                await _unitOfWork.OrderItemRepository.InsertAsync(warrantyOrderItem);
                warrantyOrderItemIdMap[orderItem.Id] = warrantyOrderItem.Id;
            }

            var warrantyRequest = new WarrantyRequest
            {
                SKU = CodeHelper.GenerateCode('W'),
                Status = WarrantyRequestStatus.PENDING,
                RequestType = requestType,
            };

            await _unitOfWork.WarrantyRequestRepository.InsertAsync(warrantyRequest);

            foreach (var itemDto in dto.Items)
            {
                var requestItem = new WarrantyRequestItem
                {
                    WarrantyRequestId = warrantyRequest.Id,
                    OrderItemId = warrantyOrderItemIdMap[itemDto.OrderItemId],
                    Status = WarrantyRequestItemStatus.PENDING,
                    Description = itemDto.Description,
                    Images = itemDto.Images ?? new List<string>(),
                    Videos = itemDto.Videos ?? new List<string>(),
                    WarrantyRound = warrantyRounds[itemDto.OrderItemId]
                };
                await _warrantyRequestItemRepository.InsertAsync(requestItem);
            }

            await _unitOfWork.SaveChangesAsync();

            await _notificationService.SendAndSaveNotificationAsync(new NotificationRequestDto
            {
                ReceiverId = userId,
                NotificationTitle = "Yêu cầu bảo hành mới",
                NotificationContent =
                    $"Bạn đã tạo thành công bảo hành cho các sản phẩm với SKU: {string.Join(", ", orderItemSKUs)}.",
                ActionUrl = $"/warranty-requests/{warrantyRequest.Id}",
                Metadata = new Dictionary<string, string>
                {
                    { "warrantyRequestId", warrantyRequest.Id },
                    { "orderItemIds", string.Join(",", validOrderItems.Select(x => x.Id)) },
                    { "orderId", warrantyOrder.Id }
                }
            });
            return warrantyOrder.Id;
        }

        public async Task<string> CreateRequestInBranch(WarrantyBranchRequestDto dto, string accessToken)
        {
            var userId = JwtTokenHelper.ExtractUserId(accessToken);
            var config = await _configService.GetConfig();
            var validOrderItems = new List<OrderItem>();
            var orderItemSKUs = new List<string>();
            var warrantyRounds = new Dictionary<string, int>();

            var user = await _unitOfWork.UserRepository.GetByIdAsync(userId);
            _validationService.CheckNotFound(user, $"User with id {userId} not found!");

            var branch = user.Branch;
            _validationService.CheckBadRequest(branch == null, "Manager is not assigned to any branch!");

            string branchId = branch.Id;

            RequestType requestType = RequestType.FREE;
            foreach (var itemDto in dto.Items)
            {
                var orderItem = await _unitOfWork.OrderItemRepository.GetByIdNotDeletedAsync(itemDto.OrderItemId);
                _validationService.CheckNotFound(orderItem, $"Order item {itemDto.OrderItemId} not found!");

                var order = await _unitOfWork.OrderRepository.GetByIdNotDeletedAsync(orderItem.OrderId!);
                _validationService.CheckNotFound(order, $"Order of item {orderItem.Id} not found!");

                if (order.PaymentStatus == PaymentStatus.PENDING || order.PaymentStatus == PaymentStatus.PAID_DEPOSIT)
                    throw new ErrorException(StatusCodes.Status400BadRequest, ApiCodes.BAD_REQUEST,
                        $"Order {order.Code} must be paid before requesting warranty!");
                if (order.ReceivedAt == null)
                    throw new ErrorException(StatusCodes.Status400BadRequest, ApiCodes.BAD_REQUEST,
                        $"Order {order.Code} must be received before requesting warranty!");

                var daysSinceReceived = (DateTime.UtcNow - order.ReceivedAt!.Value).TotalDays;
                var warrantyCount =
                    await _warrantyRequestItemRepository.CountWarrantyRequestItemsAsync(itemDto.OrderItemId);
                warrantyRounds[orderItem.Id] = warrantyCount + 1;
                if (daysSinceReceived > config.Fields?.WarrantyPeriod || warrantyCount >= config.Fields?.WarrantyTime)
                    requestType = RequestType.FEE;

                string? skuOk = orderItem.Preset?.SKU ?? orderItem.MaternityDressDetail?.SKU;
                if (!string.IsNullOrEmpty(skuOk))
                    orderItemSKUs.Add(skuOk);

                validOrderItems.Add(orderItem);
                orderItem.WarrantyDate = DateTime.UtcNow;
                await _unitOfWork.OrderItemRepository.UpdateAsync(orderItem);
            }

            var warrantyOrder = new Order
            {
                UserId = userId,
                BranchId = branchId,
                IsOnline = false,
                Type = OrderType.WARRANTY,
                Code = GenerateOrderCode(),
                Status = OrderStatus.CONFIRMED,
                PaymentStatus = PaymentStatus.WARRANTY,
                PaymentType = PaymentType.FULL,
                PaymentMethod = dto.PaymentMethod,
                DeliveryMethod = DeliveryMethod.PICK_UP,
            };
            await _unitOfWork.OrderRepository.InsertAsync(warrantyOrder);

            var warrantyOrderItemIdMap = new Dictionary<string, string>();
            foreach (var orderItem in validOrderItems)
            {
                var warrantyOrderItem = new OrderItem
                {
                    OrderId = warrantyOrder.Id,
                    ParentOrderItemId = orderItem.ParentOrderItemId ?? orderItem.Id,
                    MaternityDressDetailId = orderItem.MaternityDressDetailId,
                    Preset = orderItem.Preset,
                    PresetId = orderItem.PresetId,
                    ItemType = ItemType.WARRANTY,
                    Price = orderItem.Price,
                    Quantity = 1,
                };
                await _unitOfWork.OrderItemRepository.InsertAsync(warrantyOrderItem);
                warrantyOrderItemIdMap[orderItem.Id] = warrantyOrderItem.Id;
            }

            var warrantyRequest = new WarrantyRequest
            {
                SKU = CodeHelper.GenerateCode('W'),
                Status = WarrantyRequestStatus.PENDING,
                RequestType = requestType,
            };

            await _unitOfWork.WarrantyRequestRepository.InsertAsync(warrantyRequest);

            foreach (var itemDto in dto.Items)
            {
                var requestItem = new WarrantyRequestItem
                {
                    WarrantyRequestId = warrantyRequest.Id,
                    OrderItemId = warrantyOrderItemIdMap[itemDto.OrderItemId],
                    Status = WarrantyRequestItemStatus.PENDING,
                    Description = itemDto.Description,
                    Images = itemDto.Images ?? new List<string>(),
                    Videos = itemDto.Videos ?? new List<string>(),
                    WarrantyRound = warrantyRounds[itemDto.OrderItemId]
                };
                await _warrantyRequestItemRepository.InsertAsync(requestItem);
            }

            await _unitOfWork.SaveChangesAsync();

            await _notificationService.SendAndSaveNotificationAsync(new NotificationRequestDto
            {
                ReceiverId = userId,
                NotificationTitle = "Yêu cầu bảo hành mới",
                NotificationContent =
                    $"Bạn đã tạo thành công bảo hành cho các sản phẩm với SKU: {string.Join(", ", orderItemSKUs)}.",
                ActionUrl = $"/warranty-requests/{warrantyRequest.Id}",
                Metadata = new Dictionary<string, string>
                {
                    { "warrantyRequestId", warrantyRequest.Id },
                    { "orderItemIds", string.Join(",", validOrderItems.Select(x => x.Id)) },
                    { "orderId", warrantyOrder.Id }
                }
            });

            return warrantyOrder.Id;
        }

        public async Task DeleteAsync(string id)
        {
            var warrantyRequest = await _unitOfWork.WarrantyRequestRepository.GetByIdAsync(id);
            _validationService.CheckNotFound(warrantyRequest, $"Warranty request with id:{id} not found");

            await _unitOfWork.WarrantyRequestRepository.SoftDeleteAsync(warrantyRequest);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<WarrantyDetailResponseDto> DetailsByIdAsync(string orderId)
        {
            var order = await _unitOfWork.OrderRepository.GetWithItemsAndDressDetails(orderId);
            _validationService.CheckNotFound(order, $"Order with id {orderId} not found");

            // Lấy WarrantyRequest từ order hiện tại
            var warrantyRequest = order.OrderItems
                .SelectMany(oi => oi.WarrantyRequestItems ?? Enumerable.Empty<WarrantyRequestItem>())
                .Select(wri => wri.WarrantyRequest)
                .FirstOrDefault(wr => wr != null);

            var originalOrders = order.OrderItems
                .Where(x => x.ParentOrderItemId != null && x.ParentOrderItem?.Order != null)
                .GroupBy(x => x.ParentOrderItem.Order)
                .Select(group => new OrderWarrantyOnlyCode
                {
                    Id = group.Key.Id,
                    Code = group.Key.Code,
                    ReceivedAt = group.Key.ReceivedAt ?? DateTime.MinValue,
                    OrderItems = group.Select(x => _mapper.Map<OrderItemGetByIdResponseDto>(x)).ToList()
                })
                .ToList();

            return new WarrantyDetailResponseDto
            {
                WarrantyRequest = warrantyRequest != null
                    ? _mapper.Map<WarrantyRequestGetAllDto>(warrantyRequest)
                    : null,
                OriginalOrders = originalOrders
            };
        }

        public async Task<PaginatedList<WarrantyRequestGetAllDto>> GetAllWarrantyRequestAsync(int index, int pageSize,
            string? search, EntitySortBy? sortBy)
        {
            var warrantyRequests =
                await _unitOfWork.WarrantyRequestRepository.GetAllWarrantyRequestAsync(index, pageSize, search, sortBy);

            var result = _mapper.Map<List<WarrantyRequestGetAllDto>>(warrantyRequests.Items);
            return new PaginatedList<WarrantyRequestGetAllDto>(
                result,
                warrantyRequests.TotalCount,
                warrantyRequests.PageNumber,
                pageSize);
        }

        public async Task<WarrantyGetByIdResponseDto> GetWarrantyRequestByIdAsync(string id)
        {
            var result = await _unitOfWork.WarrantyRequestRepository.GetDetailById(id);
            _validationService.CheckNotFound(result, $"Warranty request with id:{id} not found");

            return _mapper.Map<WarrantyGetByIdResponseDto>(result);
        }

        private string GenerateOrderCode()
        {
            string prefix = "O";
            string randomPart = new Random().Next(10000, 99999).ToString();
            return $"{prefix}{randomPart}";
        }


        public async Task<WarrantyDecisionResponseDto> DecideAsync(
            string warrantyRequestId, WarrantyDecisionRequestDto dto)
        {
            var wr = await _unitOfWork.WarrantyRequestRepository.GetDetailById(warrantyRequestId);
            _validationService.CheckNotFound(wr, $"Warranty request {warrantyRequestId} not found");

            if (!string.IsNullOrWhiteSpace(dto.NoteInternal))
            {
                wr.NoteInternal = dto.NoteInternal;
            }

            if (dto.Items == null || dto.Items.Count == 0)
                throw new ErrorException(StatusCodes.Status400BadRequest, ApiCodes.INVALID_INPUT,
                    "Items must not be empty");

            var wrOrderItemIds = wr.WarrantyRequestItems
                .Select(x => x.OrderItemId)
                .ToHashSet(StringComparer.OrdinalIgnoreCase);

            var dupIds = dto.Items
                .GroupBy(x => x.OrderItemId, StringComparer.OrdinalIgnoreCase)
                .Where(g => g.Count() > 1)
                .Select(g => g.Key)
                .ToList();
            if (dupIds.Count > 0)
                throw new ErrorException(StatusCodes.Status400BadRequest, ApiCodes.INVALID_INPUT,
                    $"Duplicate OrderItemId in request: {string.Join(", ", dupIds)}");

            var badIds = dto.Items
                .Select(x => x.OrderItemId)
                .Where(id => !wrOrderItemIds.Contains(id))
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToList();
            if (badIds.Count > 0)
                throw new ErrorException(StatusCodes.Status400BadRequest, ApiCodes.INVALID_INPUT,
                    $"OrderItemId does not belong to warranty request {warrantyRequestId}: {string.Join(", ", badIds)}");

            bool isFeeRequest = wr.RequestType == RequestType.FEE;
            var feeByOrderId = new Dictionary<string, decimal>(StringComparer.OrdinalIgnoreCase);
            var shipByOrderId = new Dictionary<string, decimal>(StringComparer.OrdinalIgnoreCase);
            foreach (var it in dto.Items)
            {
                if (it.Status != WarrantyRequestItemStatus.APPROVED &&
                    it.Status != WarrantyRequestItemStatus.REJECTED)
                {
                    throw new ErrorException(StatusCodes.Status400BadRequest, ApiCodes.INVALID_INPUT,
                        "Status must be APPROVED or REJECTED");
                }

                if (it.Status == WarrantyRequestItemStatus.REJECTED)
                {
                    if (string.IsNullOrWhiteSpace(it.RejectedReason))
                        throw new ErrorException(StatusCodes.Status400BadRequest, ApiCodes.INVALID_INPUT,
                            "RejectedReason is required when status is REJECTED");

                    if (it.DestinationType != default || it.Fee.HasValue || it.ShippingFee.HasValue ||
                        it.EstimateTime.HasValue)
                    {
                        throw new ErrorException(StatusCodes.Status400BadRequest, ApiCodes.INVALID_INPUT,
                            "Only RejectedReason is allowed when status is REJECTED");
                    }
                }
                else
                {
                    if (!string.IsNullOrWhiteSpace(it.RejectedReason))
                        throw new ErrorException(StatusCodes.Status400BadRequest, ApiCodes.INVALID_INPUT,
                            "RejectedReason must be empty when status is APPROVED");
                    if (!isFeeRequest && it.ShippingFee.HasValue && it.Fee.HasValue)
                        throw new ErrorException(StatusCodes.Status400BadRequest, ApiCodes.INVALID_INPUT,
                            "ShippingFee and Fee cannot be set together when RequestType is not FEE");
                }
            }

            var anyApprove = false;
            var anyReject = false;
            var decisions = dto.Items.ToDictionary(
                x => x.OrderItemId,
                x => x,
                StringComparer.OrdinalIgnoreCase
            );

            var approveGroups = wr.WarrantyRequestItems
                .Where(wri => decisions.ContainsKey(wri.OrderItemId) &&
                              wri.Status == WarrantyRequestItemStatus.APPROVED)
                .GroupBy(wri =>
                {
                    var oi = _unitOfWork.OrderItemRepository.GetByIdNotDeletedAsync(wri.OrderItemId).Result;
                    var orderId = oi.OrderId;
                    var destKey = wri.DestinationType == DestinationType.FACTORY ? "FACTORY" : "BRANCH";
                    return $"{orderId}|{destKey}";
                })
                .ToDictionary(g => g.Key, g => g.ToList());

            var responseItems = new List<WarrantyDecisionResponseItemDto>();

            foreach (var wri in wr.WarrantyRequestItems)
            {
                if (!decisions.TryGetValue(wri.OrderItemId, out var d)) continue;

                if (d.Status == WarrantyRequestItemStatus.REJECTED)
                {
                    anyReject = true;
                    wri.Status = WarrantyRequestItemStatus.REJECTED;
                    wri.RejectedReason = d.RejectedReason;
                    wri.DestinationType = default;
                    wri.DestinationBranchId = null;
                    wri.Fee = null;
                    wri.EstimateTime = null;

                    responseItems.Add(new WarrantyDecisionResponseItemDto
                    {
                        OrderItemId = wri.OrderItemId,
                        Status = WarrantyRequestItemStatus.REJECTED,
                        TrackingCode = null
                    });
                    continue;
                }

                anyApprove = true;
                wri.Status = WarrantyRequestItemStatus.APPROVED;
                wri.DestinationType = d.DestinationType;
                wri.Fee = d.Fee;
                wri.EstimateTime = d.EstimateTime;
                wri.RejectedReason = null;

                var oi = await _unitOfWork.OrderItemRepository.GetByIdNotDeletedAsync(wri.OrderItemId);
                _validationService.CheckNotFound(oi, $"OrderItem {wri.OrderItemId} not found");
                var orderId = oi.OrderId!;
                if (isFeeRequest)
                {
                    if (!d.Fee.HasValue || d.Fee <= 0)
                    {
                        throw new ErrorException(StatusCodes.Status400BadRequest, ApiCodes.INVALID_INPUT,
                            "Fee is required and must be > 0 when RequestType = FEE");
                    }

                    feeByOrderId[oi.OrderId!] =
                        (feeByOrderId.TryGetValue(oi.OrderId!, out var s) ? s : 0) + d.Fee!.Value;

                    if (d.ShippingFee.HasValue)
                    {
                        if (shipByOrderId.TryGetValue(orderId, out var existed) && existed != d.ShippingFee.Value)
                            throw new ErrorException(StatusCodes.Status400BadRequest, ApiCodes.INVALID_INPUT,
                                "ShippingFee must be the same for all items of the same order");
                        shipByOrderId[orderId] = d.ShippingFee.Value;
                    }

                    responseItems.Add(new WarrantyDecisionResponseItemDto
                    {
                        OrderItemId = wri.OrderItemId,
                        Status = WarrantyRequestItemStatus.APPROVED,
                        TrackingCode = null
                    });
                    continue; // KHÔNG gom nhóm GHTK ở luồng FEE
                }

                var oiForKey = await _unitOfWork.OrderItemRepository.GetByIdNotDeletedAsync(wri.OrderItemId);
                _validationService.CheckNotFound(oiForKey, $"OrderItem {wri.OrderItemId} not found");
                var orderKey = oiForKey.OrderId!;
                var destKey = d.DestinationType == DestinationType.FACTORY
                    ? "FACTORY"
                    : "BRANCH";

                var key = $"{orderKey}|{destKey}";

                if (!approveGroups.TryGetValue(key, out var list))
                    approveGroups[key] = list = new List<WarrantyRequestItem>();
                list.Add(wri);
            }

            if (isFeeRequest)
            {
                wr.TotalFee = wr.WarrantyRequestItems
                    .Where(x => x.Status == WarrantyRequestItemStatus.APPROVED)
                    .Sum(x => x.Fee ?? 0);

                // YÊU CẦU: mỗi order có phí bảo hành phải có ShippingFee
                foreach (var orderId in feeByOrderId.Keys)
                {
                    if (!shipByOrderId.TryGetValue(orderId, out var _))
                        throw new ErrorException(StatusCodes.Status400BadRequest, ApiCodes.INVALID_INPUT,
                            $"ShippingFee is required for warranty fee order {orderId}");
                }

                // Cập nhật tiền & trạng thái cho từng Order bảo hành liên quan
                foreach (var (orderId, feeTotal) in feeByOrderId)
                {
                    var orderEntity = await _unitOfWork.OrderRepository.GetByIdNotDeletedAsync(orderId);
                    _validationService.CheckNotFound(orderEntity, "Order not found");

                    var ship = shipByOrderId[orderId]; // đã validate tồn tại
                    orderEntity.SubTotalAmount = feeTotal; 
                    orderEntity.ShippingFee = ship;
                    orderEntity.TotalAmount = feeTotal + ship;
                    orderEntity.Status = OrderStatus.AWAITING_PAID_WARRANTY;

                    await _unitOfWork.OrderRepository.UpdateAsync(orderEntity);
                }

                // Cập nhật trạng thái tổng của WarrantyRequest
                if (anyApprove && anyReject) wr.Status = WarrantyRequestStatus.PARTIALLY_REJECTED;
                else if (anyApprove) wr.Status = WarrantyRequestStatus.APPROVED;
                else wr.Status = WarrantyRequestStatus.REJECTED;

                await _unitOfWork.SaveChangesAsync();

                return new WarrantyDecisionResponseDto
                {
                    RequestStatus = wr.Status ?? WarrantyRequestStatus.PENDING,
                    Items = responseItems
                };
            }

            await _unitOfWork.SaveChangesAsync();

            var clearedOrders = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            // Xử lý từng nhóm items đã APPROVED
            foreach (var (key, group) in approveGroups)
            {
                var firstWri = group.First();
                var firstOi = await _unitOfWork.OrderItemRepository.GetByIdNotDeletedAsync(firstWri.OrderItemId);
                _validationService.CheckNotFound(firstOi, $"OrderItem {firstWri.OrderItemId} not found");

                var orderEntity = await _unitOfWork.OrderRepository.GetWithItemsAndDressDetails(firstOi.OrderId!);
                _validationService.CheckNotFound(orderEntity, "Order not found");

                // Chỉ gán milestone cho các items về FACTORY
                var approvedOrderItemIds = group
                    .Where(wri => wri.DestinationType == DestinationType.FACTORY
                                  && (wri.Status == WarrantyRequestItemStatus.IN_TRANSIT || wri.Status == WarrantyRequestItemStatus.APPROVED))
                    .Select(wri => wri.OrderItemId)
                    .ToHashSet(StringComparer.OrdinalIgnoreCase);

                await AssignTasksForOrder(orderEntity, approvedOrderItemIds);

                // Chỉ xử lý tạo đơn GHTK cho các items về FACTORY
                if (firstWri.DestinationType == DestinationType.FACTORY)
                {
                    var (senderAddr, senderProvince, senderDistrict, senderWard) = ResolveAddress(orderEntity);

                    // Thông tin nhận hàng luôn là factory
                    var orderInfo = new GhtkOrderExpressInfo
                    {
                        Id = $"{orderEntity.Code}-{CodeHelper.GenerateCode('W')}",
                        PickAddressId = null,
                        PickName = orderEntity.User.FullName,
                        PickAddress = senderAddr,
                        PickProvince = senderProvince,
                        PickDistrict = senderDistrict,
                        PickTel = orderEntity.User.PhoneNumber,
                        Name = _ghtkSettings.PickName,
                        Tel = _ghtkSettings.PickTel,
                        Address = _ghtkSettings.PickAddress ?? "",
                        Province = _ghtkSettings.PickProvince ?? "",
                        District = _ghtkSettings.PickDistrict ?? "",
                        Ward = _ghtkSettings.PickWard ?? "",
                        Value = await SumValueAsync(group)
                    };

                    // Chỉ lấy products của các items về FACTORY
                    var products = group
                        .Where(wri => wri.DestinationType == DestinationType.FACTORY)
                        .Select(wri =>
                        {
                            var oi = _unitOfWork.OrderItemRepository.GetByIdNotDeletedAsync(wri.OrderItemId).Result;
                            _validationService.CheckNotFound(oi, $"OrderItem {wri.OrderItemId} not found");
                            return MapToGhtkProduct(oi);
                        })
                        .ToList();

                    var (tracking, createResp, cancelResp) =
                        await _ghtkService.SubmitAndCancelExpressForWarrantyAsync(products, orderInfo);

                    if (!string.IsNullOrWhiteSpace(tracking))
                    {
                        if (!clearedOrders.Contains(orderEntity.Id))
                        {
                            orderEntity.TrackingOrderCode = null;
                            orderEntity.Status = OrderStatus.PICKUP_IN_PROGRESS;
                            clearedOrders.Add(orderEntity.Id);
                        }

                        orderEntity.TrackingOrderCode = string.IsNullOrWhiteSpace(orderEntity.TrackingOrderCode)
                            ? tracking
                            : $"{orderEntity.TrackingOrderCode},{tracking}";

                        await _unitOfWork.OrderRepository.UpdateAsync(orderEntity);
                    }

                    foreach (var wri in group.Where(wri => wri.DestinationType == DestinationType.FACTORY))
                    {
                        wri.TrackingCode = tracking;
                        wri.Status = WarrantyRequestItemStatus.IN_TRANSIT;

                        responseItems.Add(new WarrantyDecisionResponseItemDto
                        {
                            OrderItemId = wri.OrderItemId,
                            Status = WarrantyRequestItemStatus.IN_TRANSIT,
                            TrackingCode = tracking,
                            GhtkCreateMessage = createResp?.Success == true
                                ? "Tạo đơn thành công"
                                : "Tạo đơn thất bại",
                            GhtkCancelMessage = cancelResp?.Success == true
                                ? "Hủy đơn thành công"
                                : "Hủy đơn thất bại",
                            GhtkCreateResponse = createResp,
                            GhtkCancelResponse = cancelResp
                        });
                    }
                }
                else // BRANCH - chỉ cập nhật status
                {
                    foreach (var wri in group)
                    {
                        responseItems.Add(new WarrantyDecisionResponseItemDto
                        {
                            OrderItemId = wri.OrderItemId,
                            Status = WarrantyRequestItemStatus.APPROVED,
                            TrackingCode = null
                        });
                    }
                }

                await _unitOfWork.SaveChangesAsync();
            }

            // Cập nhật trạng thái tổng của WarrantyRequest
            if (anyApprove && anyReject)
                wr.Status = WarrantyRequestStatus.PARTIALLY_REJECTED;
            else if (anyApprove)
                wr.Status = WarrantyRequestStatus.APPROVED;
            else
                wr.Status = WarrantyRequestStatus.REJECTED;

            await _unitOfWork.SaveChangesAsync();

            return new WarrantyDecisionResponseDto
            {
                RequestStatus = wr.Status ?? WarrantyRequestStatus.PENDING,
                Items = responseItems
            };
        }

        public async Task<WarrantyDecisionResponseDto> ShipPaidWarrantyAsync(string warrantyRequestId)
        {
            var wr = await _unitOfWork.WarrantyRequestRepository.GetDetailById(warrantyRequestId);
            _validationService.CheckNotFound(wr, $"Warranty request {warrantyRequestId} not found");

            // Lấy Order bảo hành từ 1 item bất kỳ
            var firstItem = wr.WarrantyRequestItems.FirstOrDefault();
            _validationService.CheckBadRequest(firstItem == null, "No warranty items");
            var firstOi = await _unitOfWork.OrderItemRepository.GetByIdNotDeletedAsync(firstItem!.OrderItemId);
            var order = await _unitOfWork.OrderRepository.GetWithItemsAndDressDetails(firstOi.OrderId!);
            _validationService.CheckNotFound(order, "Order not found");

            // Đảm bảo đã thanh toán phí BH
            if (order.PaymentStatus != PaymentStatus.PAID_FULL)
                throw new ErrorException(StatusCodes.Status400BadRequest, ApiCodes.BAD_REQUEST,
                    "Warranty fee must be PAID_FULL before shipping");

            // Chỉ ship những item APPROVED, về FACTORY
            var toShip = wr.WarrantyRequestItems
                .Where(x => x.Status == WarrantyRequestItemStatus.APPROVED
                            && x.DestinationType == DestinationType.FACTORY)
                .ToList();
            if (toShip.Count == 0)
                return new WarrantyDecisionResponseDto
                {
                    RequestStatus = wr.Status ?? WarrantyRequestStatus.PENDING,
                    Items = new()
                };

            var (senderAddr, senderProvince, senderDistrict, senderWard) = ResolveAddress(order);
            var orderInfo = new GhtkOrderExpressInfo
            {
                Id = $"{order.Code}-{CodeHelper.GenerateCode('W')}",
                PickAddressId = null,
                PickName = order.User.FullName,
                PickAddress = senderAddr,
                PickProvince = senderProvince,
                PickDistrict = senderDistrict,
                PickTel = order.User.PhoneNumber,
                Name = _ghtkSettings.PickName,
                Tel = _ghtkSettings.PickTel,
                Address = _ghtkSettings.PickAddress ?? "",
                Province = _ghtkSettings.PickProvince ?? "",
                District = _ghtkSettings.PickDistrict ?? "",
                Ward = _ghtkSettings.PickWard ?? "",
                Value = await SumValueAsync(toShip)
            };

            var products = new List<GhtkProductDto>();
            foreach (var wri in toShip)
            {
                var oi = await _unitOfWork.OrderItemRepository.GetByIdNotDeletedAsync(wri.OrderItemId);
                products.Add(MapToGhtkProduct(oi));
            }

            var (tracking, createResp, cancelResp) =
                await _ghtkService.SubmitAndCancelExpressForWarrantyAsync(products,
                    orderInfo);

            if (!string.IsNullOrWhiteSpace(tracking))
            {
                // Optional: nếu muốn reset trước khi append giống nhánh FREE theo từng order group:
                // order.TrackingOrderCode = null;

                order.TrackingOrderCode = string.IsNullOrWhiteSpace(order.TrackingOrderCode)
                    ? tracking
                    : $"{order.TrackingOrderCode},{tracking}";
                order.Status = OrderStatus.PICKUP_IN_PROGRESS;
                await _unitOfWork.OrderRepository.UpdateAsync(order);
            }

            foreach (var wri in toShip)
            {
                wri.TrackingCode = tracking;
                wri.Status = WarrantyRequestItemStatus.IN_TRANSIT;
            }

            await _unitOfWork.SaveChangesAsync();

            return new WarrantyDecisionResponseDto
            {
                RequestStatus = wr.Status ?? WarrantyRequestStatus.APPROVED,
                Items = toShip.Select(x => new WarrantyDecisionResponseItemDto
                {
                    OrderItemId = x.OrderItemId,
                    Status = WarrantyRequestItemStatus.IN_TRANSIT,
                    TrackingCode = x.TrackingCode,
                    GhtkCreateMessage = createResp?.Success == true ? "Tạo đơn thành công" : "Tạo đơn thất bại",
                    GhtkCancelMessage = cancelResp?.Success == true ? "Hủy đơn thành công" : "Hủy đơn thất bại",
                    GhtkCreateResponse = createResp,
                    GhtkCancelResponse = cancelResp
                }).ToList()
            };
        }

        public async Task AssignWarrantyTasksAfterPaidAsync(Order order)
        {
            var feeRequests = await _unitOfWork.WarrantyRequestRepository
                .GetFeeWarrantyRequestsByOrderIdAsync(order.Id);

            if (feeRequests.Count == 0) return;

            // Gom các OrderItemId cần gán task (APPROVED + FACTORY)
            var allowedIds = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            foreach (var wr in feeRequests)
            {
                foreach (var wri in wr.WarrantyRequestItems)
                {
                    if (wri.Status == WarrantyRequestItemStatus.APPROVED &&
                        wri.DestinationType == DestinationType.FACTORY)
                    {
                        allowedIds.Add(wri.OrderItemId);
                    }
                }
            }

            if (allowedIds.Count == 0) return;

            // Lấy lại order đầy đủ để có Items + DressDetail cho AssignTasksForOrder
            var fullOrder = await _unitOfWork.OrderRepository.GetWithItemsAndDressDetails(order.Id);
            _validationService.CheckNotFound(fullOrder, "Order not found");

            await AssignTasksForOrder(fullOrder, allowedIds);
        }
        
        private async Task AssignTasksForOrder(Order order, HashSet<string> allowedOrderItemIds)
        {
            var milestoneList = await _unitOfWork.MilestoneRepository.GetAllWithInclude();

            var assignRequests = order.OrderItems
                .Where(orderItem => allowedOrderItemIds.Contains(orderItem.Id))
                .Select(orderItem =>
                {
                    var matchingMilestones = milestoneList
                        .Where(m => m.ApplyFor.Contains((ItemType)orderItem.ItemType!))
                        .Select(m => m.Id)
                        .ToList();

                    if (orderItem.OrderItemAddOnOptions != null && orderItem.OrderItemAddOnOptions.Any())
                    {
                        var addOnMilestones = milestoneList
                            .Where(m => m.ApplyFor.Contains(ItemType.ADD_ON))
                            .Select(m => m.Id);
                        matchingMilestones.AddRange(addOnMilestones);
                    }

                    return new AssignTaskToOrderItemRequestDto
                    {
                        OrderItemId = orderItem.Id,
                        MilestoneIds = matchingMilestones
                    };
                }).ToList();

            foreach (var assignRequest in assignRequests)
            {
                await _orderItemService.AssignTaskToOrderItemAsync(assignRequest);
            }

            await _cacheService.RemoveByPrefixAsync("MilestoneAchiveOrderItemResponseDto_");
        }

        private static GhtkProductDto MapToGhtkProduct(OrderItem oi)
        {
            if (oi.MaternityDressDetailId != null)
                return new GhtkProductDto
                {
                    Name = oi.MaternityDressDetail?.Name ?? "Đầm bầu",
                    Weight = oi.MaternityDressDetail?.Weight ?? 200,
                    Quantity = 1
                };
            if (oi.PresetId != null)
                return new GhtkProductDto
                { Name = oi.Preset?.Name ?? "Preset thiết kế", Weight = oi.Preset?.Weight ?? 200, Quantity = 1 };
            throw new ErrorException(StatusCodes.Status400BadRequest, ApiCodes.INVALID_INPUT,
                "Order item must have either MaternityDressDetail or Preset");
        }

        private static (string addr, string province, string district, string ward) ResolveAddress(Order order)
        {
            if (order.AddressId != null && order.Address != null)
                return (order.Address.Street ?? "", order.Address.Province ?? "", order.Address.District ?? "",
                    order.Address.Ward ?? "");
            throw new ErrorException(StatusCodes.Status400BadRequest, ApiCodes.INVALID_INPUT,
                "Order must have address for delivery");
        }

        private async Task<decimal> SumValueAsync(List<WarrantyRequestItem> items)
        {
            decimal sum = 0;
            foreach (var wri in items)
            {
                var oi = await _unitOfWork.OrderItemRepository.GetByIdNotDeletedAsync(wri.OrderItemId);
                var price = oi.MaternityDressDetail?.Price ?? oi.Preset?.Price ?? 0;
                sum += price;
            }

            return sum;
        }
    }
}