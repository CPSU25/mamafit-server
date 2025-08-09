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
using MamaFit.Services.Interface;
using Microsoft.AspNetCore.Http;

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

        public WarrantyRequestService(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IValidationService validationService,
            INotificationService notificationService,
            IConfigService configService,
            IOrderItemService orderItemService,
            IWarrantyRequestItemRepository warrantyRequestItemRepository)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _validationService = validationService;
            _notificationService = notificationService;
            _configService = configService;
            _orderItemService = orderItemService;
            _warrantyRequestItemRepository = warrantyRequestItemRepository;
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

        // private async Task<OrderItem> GetRootOrderItemAsync(string itemId)
        // {
        //     var item = await _unitOfWork.OrderItemRepository.GetByIdNotDeletedAsync(itemId);
        //     while (!string.IsNullOrEmpty(item!.ParentOrderItemId))
        //     {
        //         item = await _unitOfWork.OrderItemRepository.GetByIdNotDeletedAsync(item.ParentOrderItemId);
        //     }
        //
        //     return item;
        // }

        // public async Task<WarrantyDecisionResponseDto> DecideAsync(string warrantyRequestId,
        //     WarrantyDecisionRequestDto dto)
        // {
        //     var wr = await _unitOfWork.WarrantyRequestRepository.GetDetailById(warrantyRequestId);
        //     _validationService.CheckNotFound(wr, $"Warranty request {warrantyRequestId} not found");
        //
        //     if (!string.IsNullOrWhiteSpace(dto.NoteInternal))
        //     {
        //         wr.NoteInternal = dto.NoteInternal;
        //         await _unitOfWork.WarrantyRequestRepository.UpdateAsync(wr);
        //     }
        //
        //     var decisions = dto.Items.ToDictionary(x => x.OrderItemId);
        //     var approveGroups = new Dictionary<string, List<WarrantyRequestItem>>();
        //     var anyApprove = false;
        //     var anyReject = false;
        //
        //     // 1) Cập nhật từng item
        //     foreach (var wri in wr.WarrantyRequestItems)
        //     {
        //         if (!decisions.TryGetValue(wri.OrderItemId, out var d)) continue;
        //
        //         if (d.Status == WarrantyRequestItemStatus.REJECTED)
        //         {
        //             anyReject = true;
        //             wri.Status = WarrantyRequestItemStatus.REJECTED;
        //             wri.RejectedReason = d.RejectReason;
        //             wri.DestinationType = d.DestinationType;
        //             wri.DestinationBranchId =
        //                 d.DestinationType == DestinationType.BRANCH ? d.DestinationBranchId : null;
        //             wri.Fee = d.Fee;
        //             wri.EstimateTime = d.EstimateTime;
        //             await _unitOfWork.WarrantyRequestItemRepository.UpdateAsync(wri);
        //             continue;
        //         }
        //
        //         // APPROVED
        //         anyApprove = true;
        //         if (d.DestinationType == DestinationType.BRANCH && string.IsNullOrEmpty(d.DestinationBranchId))
        //             throw new ErrorException(StatusCodes.Status400BadRequest, ApiCodes.INVALID_INPUT,
        //                 "DestinationBranchId is required for BRANCH");
        //
        //         wri.Status = WarrantyRequestItemStatus.APPROVED; // tạm, sẽ set IN_TRANSIT sau khi tạo đơn
        //         wri.DestinationType = d.DestinationType;
        //         wri.DestinationBranchId = d.DestinationType == DestinationType.BRANCH ? d.DestinationBranchId : null;
        //         wri.Fee = d.Fee;
        //         wri.EstimateTime = d.EstimateTime;
        //         await _unitOfWork.WarrantyRequestItemRepository.UpdateAsync(wri);
        //
        //         var key = d.DestinationType == DestinationType.FACTORY
        //             ? "FACTORY"
        //             : $"BRANCH:{wri.DestinationBranchId}";
        //         if (!approveGroups.TryGetValue(key, out var list))
        //             approveGroups[key] = list = new();
        //         list.Add(wri);
        //     }
        //
        //     await _unitOfWork.SaveChangesAsync();
        //
        //     // 2) Lên đơn theo nhóm APPROVE
        //     var responseItems = new List<WarrantyDecisionResponseItemDto>();
        //
        //     foreach (var (key, group) in approveGroups)
        //     {
        //         // 2.1 Build products từ các OrderItem bảo hành
        //         var products = new List<GhtkProductDto>();
        //         foreach (var wri in group)
        //         {
        //             var oi = await _unitOfWork.OrderItemRepository.GetByIdNotDeletedAsync(wri.OrderItemId);
        //             _validationService.CheckNotFound(oi, $"OrderItem {wri.OrderItemId} not found");
        //
        //             products.Add(MapToGhtkProduct(oi)); // cùng kiểu map như SubmitOrderExpressAsync
        //         }
        //
        //         // 2.2 Lấy order gốc (để lấy địa chỉ KH làm Sender)
        //         var sampleOi = await _unitOfWork.OrderItemRepository.GetByIdNotDeletedAsync(group.First().OrderItemId);
        //         var rootOi = sampleOi;
        //         if (!string.IsNullOrEmpty(sampleOi.ParentOrderItemId))
        //             rootOi = await _unitOfWork.OrderItemRepository.GetByIdNotDeletedAsync(sampleOi.ParentOrderItemId);
        //         var originalOrder = await _unitOfWork.OrderRepository.GetWithItemsAndDressDetails(rootOi.OrderId!);
        //         _validationService.CheckNotFound(originalOrder, "Original order not found");
        //
        //         // Sender = KH: address từ Order.Address hoặc Order.Branch (giống luồng GHTK hiện tại)
        //         var (senderAddr, senderProvince, senderDistrict, senderWard) = ResolveAddress(originalOrder);
        //         var senderName = originalOrder.User?.FullName ?? "Khách hàng";
        //         var senderTel = originalOrder.User?.PhoneNumber ?? "";
        //
        //         // Receiver
        //         string recvName, recvTel, recvAddress, recvProvince, recvDistrict, recvWard;
        //         if (group.First().DestinationType == DestinationType.FACTORY)
        //         {
        //             // dùng cấu hình shop từ appsettings (pick*)
        //             // Lưu ý: các giá trị này có sẵn trong GhtkService qua _ghtkSettings
        //             // -> truyền trực tiếp vào orderInfo (Receiver)
        //             recvName = _configService.GetConfig().Result.Shipping?.PickName ?? "Shop";
        //             recvTel = _configService.GetConfig().Result.Shipping?.PickTel ?? "";
        //             recvAddress = _configService.GetConfig().Result.Shipping?.PickAddress ?? "";
        //             recvProvince = _configService.GetConfig().Result.Shipping?.PickProvince ?? "";
        //             recvDistrict = _configService.GetConfig().Result.Shipping?.PickDistrict ?? "";
        //             recvWard = _configService.GetConfig().Result.Shipping?.PickWard ?? "";
        //         }
        //         else
        //         {
        //             var branchId = group.First().DestinationBranchId!;
        //             var branch = await _unitOfWork.BranchRepository.GetByIdNotDeletedAsync(branchId);
        //             _validationService.CheckNotFound(branch, $"Branch {branchId} not found");
        //             recvName = branch.Name ?? "Chi nhánh";
        //             recvTel = branch.PhoneNumber ?? "";
        //             recvAddress = branch.Street ?? "";
        //             recvProvince = branch.Province ?? "";
        //             recvDistrict = branch.District ?? "";
        //             recvWard = branch.Ward ?? "";
        //         }
        //
        //         var value = await SumValueAsync(group); // tổng định giá
        //
        //         var orderInfo = new GhtkOrderExpressInfo
        //         {
        //             Id = $"W-{wr.SKU}-{Guid.NewGuid().ToString()[..8]}",
        //
        //             // Pick = Sender (KH)
        //             PickAddressId = null,
        //             PickName = senderName,
        //             PickAddress = senderAddr,
        //             PickProvince = senderProvince,
        //             PickDistrict = senderDistrict,
        //             PickTel = senderTel,
        //
        //             // Receiver
        //             Name = recvName,
        //             Tel = recvTel,
        //             Address = recvAddress,
        //             Province = recvProvince,
        //             District = recvDistrict,
        //             Ward = recvWard,
        //
        //             Value = value
        //         };
        //
        //         var ghtkResp = await _ghtkService.SubmitExpressForWarrantyAsync(products, orderInfo);
        //         string? tracking = null;
        //         if (ghtkResp is GhtkOrderSubmitSuccessResponse ok && ok.Order != null)
        //             tracking = ok.Order.Label ?? ok.Order.TrackingId?.ToString();
        //
        //         foreach (var wri in group)
        //         {
        //             wri.TrackingCode = tracking;
        //             wri.Status = WarrantyRequestItemStatus.IN_TRANSIT;
        //             await _warrantyRequestItemRepository.UpdateAsync(wri);
        //
        //             responseItems.Add(new WarrantyDecisionResponseItemDto
        //             {
        //                 OrderItemId = wri.OrderItemId,
        //                 Status = WarrantyRequestItemStatus.IN_TRANSIT,
        //                 TrackingCode = tracking
        //             });
        //         }
        //
        //         await _unitOfWork.SaveChangesAsync();
        //     }
        //
        //     // 3) Trạng thái tổng của WR
        //     if (anyApprove && anyReject) wr.Status = WarrantyRequestStatus.PARTIALLY_REJECTED;
        //     else if (anyApprove) wr.Status = WarrantyRequestStatus.IN_TRANSIT;
        //     else wr.Status = WarrantyRequestStatus.REJECTED;
        //
        //     await _unitOfWork.WarrantyRequestRepository.UpdateAsync(wr);
        //     await _unitOfWork.SaveChangesAsync();
        //
        //     return new WarrantyDecisionResponseDto
        //     {
        //         RequestStatus = wr.Status ?? WarrantyRequestStatus.PENDING,
        //         Items = responseItems
        //     };
        // }


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
                var price = (oi.MaternityDressDetail?.Price) ?? (oi.Preset?.Price) ?? 0;
                sum += price;
            }

            return sum;
        }
    }
}