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

        public WarrantyRequestService(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IValidationService validationService,
            INotificationService notificationService,
            IConfigService configService,
            IOrderItemService orderItemService,
            IWarrantyRequestItemRepository warrantyRequestItemRepository,
            IGhtkService ghtkService,
            IOptions<GhtkSettings> ghtkSettings)
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

            //Tao đơn hàng bảo hành
            var warrantyOrder = new Order
            {
                UserId = userId,
                AddressId = !string.IsNullOrEmpty(dto.AddressId) ? dto.AddressId : null,
                BranchId = !string.IsNullOrEmpty(dto.BranchId) ? dto.BranchId : null,
                Type = OrderType.WARRANTY,
                Code = GenerateOrderCode(),
                Status = OrderStatus.CONFIRMED,
                PaymentStatus = PaymentStatus.WARRANTY,
                PaymentType = PaymentType.FULL,
                PaymentMethod = dto.PaymentMethod,
                DeliveryMethod = dto.DeliveryMethod,
            };
            await _unitOfWork.OrderRepository.InsertAsync(warrantyOrder);

            //Tạo các OrderItem bảo hành tương ứng với các OrderItem đã chọn
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

            //Tạo WarrantyRequest tổng
            var warrantyRequest = new WarrantyRequest
            {
                SKU = CodeHelper.GenerateCode('W'),
                Status = WarrantyRequestStatus.PENDING,
                RequestType = requestType,
            };

            await _unitOfWork.WarrantyRequestRepository.InsertAsync(warrantyRequest);

            // Tạo các WarrantyRequestItem
            foreach (var itemDto in dto.Items)
            {
                var requestItem = new WarrantyRequestItem
                {
                    WarrantyRequestId = warrantyRequest.Id,
                    OrderItemId = warrantyOrderItemIdMap[itemDto.OrderItemId],
                    Status = WarrantyRequestItemStatus.PENDING,
                    Description = itemDto.Description,
                    Images = itemDto.Images ?? new List<string>(),
                    WarrantyRound = warrantyRounds[itemDto.OrderItemId]
                };
                await _warrantyRequestItemRepository.InsertAsync(requestItem);
            }

            await _unitOfWork.SaveChangesAsync();

            // Update tổng phí
            // warrantyRequest.TotalFee = totalFee;
            //await _unitOfWork.WarrantyRequestRepository.UpdateAsync(warrantyRequest);

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

        // public async Task UpdateAsync(string id, WarrantyRequestUpdateDto warrantyRequestUpdateDto)
        // {
        //     var warrantyRequest = await _unitOfWork.WarrantyRequestRepository.GetByIdAsync(id);
        //     _validationService.CheckNotFound(warrantyRequest, $"Warranty request with id:{id} not found");
        //
        //     _mapper.Map(warrantyRequestUpdateDto, warrantyRequest);
        //     await _unitOfWork.WarrantyRequestRepository.UpdateAsync(warrantyRequest);
        //     await _unitOfWork.SaveChangesAsync();
        // }

        private string GenerateOrderCode()
        {
            string prefix = "O";
            string randomPart = new Random().Next(10000, 99999).ToString();
            return $"{prefix}{randomPart}";
        }

        private async Task<OrderItem> GetRootOrderItemAsync(string itemId)
        {
            var item = await _unitOfWork.OrderItemRepository.GetByIdNotDeletedAsync(itemId);
            while (!string.IsNullOrEmpty(item!.ParentOrderItemId))
            {
                item = await _unitOfWork.OrderItemRepository.GetByIdNotDeletedAsync(item.ParentOrderItemId);
            }

            return item;
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

                    if (it.DestinationType != default || !string.IsNullOrEmpty(it.DestinationBranchId)
                                                      || it.Fee.HasValue || it.EstimateTime.HasValue)
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

                    if (it.DestinationType == DestinationType.BRANCH &&
                        string.IsNullOrEmpty(it.DestinationBranchId))
                    {
                        throw new ErrorException(StatusCodes.Status400BadRequest, ApiCodes.INVALID_INPUT,
                            "DestinationBranchId is required when DestinationType is BRANCH");
                    }
                }
            }

            // var decisions = dto.Items.ToDictionary(x => x.OrderItemId);
            var anyApprove = false;
            var anyReject = false;
            var decisions = dto.Items.ToDictionary(
                x => x.OrderItemId,
                x => x,
                StringComparer.OrdinalIgnoreCase
            );
            var approveGroups = wr.WarrantyRequestItems
                .Where(wri =>
                    decisions.ContainsKey(wri.OrderItemId) && wri.Status == WarrantyRequestItemStatus.APPROVED)
                .GroupBy(wri =>
                {
                    var oi = _unitOfWork.OrderItemRepository.GetByIdNotDeletedAsync(wri.OrderItemId).Result;
                    var orderId = oi.OrderId;
                    var destKey = wri.DestinationType == DestinationType.FACTORY
                        ? "FACTORY"
                        : $"BRANCH:{wri.DestinationBranchId}";
                    return $"{orderId}|{destKey}";
                })
                .ToDictionary(g => g.Key, g => g.ToList());
            var responseItems = new List<WarrantyDecisionResponseItemDto>();
            
            foreach (var wri in wr.WarrantyRequestItems)
            {
                if (!decisions.TryGetValue(wri.OrderItemId, out var d)) continue;
                if (wri.Status == WarrantyRequestItemStatus.REJECTED)
                {
                    if (d.Status == WarrantyRequestItemStatus.REJECTED)
                    {
                        throw new ErrorException(
                            StatusCodes.Status409Conflict,
                            ApiCodes.CONFLICT,
                            $"OrderItem {wri.OrderItemId} has been REJECTED and cannot be changed.");
                    }
                }

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
                wri.DestinationBranchId = d.DestinationType == DestinationType.BRANCH ? d.DestinationBranchId : null;
                wri.Fee = d.Fee;
                wri.EstimateTime = d.EstimateTime;
                wri.RejectedReason = null;

                var oiForKey = await _unitOfWork.OrderItemRepository.GetByIdNotDeletedAsync(wri.OrderItemId);
                _validationService.CheckNotFound(oiForKey, $"OrderItem {wri.OrderItemId} not found");
                var orderKey = oiForKey.OrderId!;
                var destKey = d.DestinationType == DestinationType.FACTORY
                    ? "FACTORY"
                    : $"BRANCH:{wri.DestinationBranchId}";
                var key = $"{orderKey}|{destKey}";

                if (!approveGroups.TryGetValue(key, out var list))
                    approveGroups[key] = list = new();
                list.Add(wri);
            }

            await _unitOfWork.SaveChangesAsync();

            var clearedOrders = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            // 2) Lên đơn theo nhóm APPROVE
            foreach (var (key, group) in approveGroups)
            {
                // Lấy order từ item đầu nhóm
                var firstWri = group.First();
                var firstOi = await _unitOfWork.OrderItemRepository.GetByIdNotDeletedAsync(firstWri.OrderItemId);
                _validationService.CheckNotFound(firstOi, $"OrderItem {firstWri.OrderItemId} not found");

                var orderEntity = await _unitOfWork.OrderRepository.GetWithItemsAndDressDetails(firstOi.OrderId!);
                _validationService.CheckNotFound(orderEntity, "Order not found");

                var (senderAddr, senderProvince, senderDistrict, senderWard) = ResolveAddress(orderEntity);

                // Receiver theo destination của nhóm (lấy từ firstWri)
                string recvName, recvAddress, recvProvince, recvDistrict, recvWard;
                var recvTel = _ghtkSettings.PickTel;
                if (firstWri.DestinationType == DestinationType.FACTORY)
                {
                    recvName = _ghtkSettings.PickName;
                    recvAddress = _ghtkSettings.PickAddress ?? "";
                    recvProvince = _ghtkSettings.PickProvince ?? "";
                    recvDistrict = _ghtkSettings.PickDistrict ?? "";
                    recvWard = _ghtkSettings.PickWard ?? "";
                }
                else
                {
                    var branchId = firstWri.DestinationBranchId!;
                    var branch = await _unitOfWork.BranchRepository.GetByIdNotDeletedAsync(branchId);
                    _validationService.CheckNotFound(branch, $"DestinationBranchId {branchId} not found");
                    recvName = branch.Name ?? "Chi nhánh";
                    recvTel = branch.BranchManager?.PhoneNumber ?? recvTel;
                    recvAddress = branch.Street ?? "";
                    recvProvince = branch.Province ?? "";
                    recvDistrict = branch.District ?? "";
                    recvWard = branch.Ward ?? "";
                }

                // Build toàn bộ products cho NHÓM
                var products = new List<GhtkProductDto>();
                foreach (var wri in group)
                {
                    var oi = await _unitOfWork.OrderItemRepository.GetByIdNotDeletedAsync(wri.OrderItemId);
                    _validationService.CheckNotFound(oi, $"OrderItem {wri.OrderItemId} not found");
                    products.Add(MapToGhtkProduct(oi));
                }

                var value = await SumValueAsync(group);
                var random = CodeHelper.GenerateCode('W');
                var orderInfo = new GhtkOrderExpressInfo
                {
                    Id = $"{orderEntity.Code}-{random}",
                    PickAddressId = null,
                    PickName = orderEntity.User.FullName ,
                    PickAddress = senderAddr,
                    PickProvince = senderProvince,
                    PickDistrict = senderDistrict,
                    // PickWard = senderWard, // mở nếu GHTK yêu cầu
                    PickTel = orderEntity.User.PhoneNumber,
                    Name = recvName,
                    Tel = recvTel,
                    Address = recvAddress,
                    Province = recvProvince,
                    District = recvDistrict,
                    Ward = recvWard,
                    Value = value
                };

                var (tracking, createResp, cancelResp) = await _ghtkService.SubmitAndCancelExpressForWarrantyAsync(products, orderInfo);
                
                bool? createOk = createResp?.Success;
                bool? cancelOk = cancelResp?.Success;
                string? createMsg = createResp == null ? null
                    : (createResp.Success ? "Tạo đơn thành công" : "Tạo đơn thất bại");
                string? cancelMsg = cancelResp == null ? null
                    : (cancelResp.Success ? "Hủy đơn thành công" : "Hủy đơn thất bại");
                // Cập nhật tracking vào ORDER (CSV nếu test trên data cũ đã có)
                if (!string.IsNullOrWhiteSpace(tracking))
                {
                    // Clear 1 lần cho order này trong request hiện tại
                    if (!clearedOrders.Contains(orderEntity.Id))
                    {
                        orderEntity.TrackingOrderCode = null;  
                        clearedOrders.Add(orderEntity.Id);
                    }

                    if (string.IsNullOrWhiteSpace(orderEntity.TrackingOrderCode))
                    {
                        orderEntity.TrackingOrderCode = tracking;
                    }
                    else
                    {
                        // nếu cùng Order có thêm 1 shipment khác (khác destination) trong CHÍNH request này
                        orderEntity.TrackingOrderCode += $",{tracking}";
                    }

                    await _unitOfWork.OrderRepository.UpdateAsync(orderEntity);
                }

                foreach (var wri in group)
                {
                    wri.TrackingCode = tracking;
                    wri.Status = WarrantyRequestItemStatus.IN_TRANSIT;

                    responseItems.Add(new WarrantyDecisionResponseItemDto
                    {
                        OrderItemId = wri.OrderItemId,
                        Status = WarrantyRequestItemStatus.IN_TRANSIT,
                        TrackingCode = tracking,
                        GhtkCreateMessage = createMsg,
                        GhtkCancelMessage = cancelMsg,
                        GhtkCreateResponse = createResp,
                        GhtkCancelResponse = cancelResp
                    });
                }

                await _unitOfWork.SaveChangesAsync();
            }

            // Trạng thái tổng của WR (đồng bộ với luồng đã lên đơn)
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


        private static GhtkProductDto MapToGhtkProduct(OrderItem oi)
        {
            if (oi.MaternityDressDetailId != null)
                return new GhtkProductDto
                {
                    Name = oi.MaternityDressDetail?.Name ?? "Đầm bầu", Weight = oi.MaternityDressDetail?.Weight ?? 200,
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
            if (order.BranchId != null && order.Branch != null)
                return (order.Branch.Street ?? "", order.Branch.Province ?? "", order.Branch.District ?? "",
                    order.Branch.Ward ?? "");
            throw new ErrorException(StatusCodes.Status400BadRequest, ApiCodes.INVALID_INPUT,
                "Order must have either branch or address for delivery");
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