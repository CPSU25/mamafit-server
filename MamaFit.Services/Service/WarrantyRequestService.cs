using AutoMapper;
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

                if (daysSinceReceived > config.Fields?.WarrantyPeriod || warrantyCount >= config.Fields?.WarrantyTime)
                    requestType = RequestType.FEE;

                string? skuOk = orderItem.Preset?.SKU ?? orderItem.MaternityDressDetail?.SKU;
                if (!string.IsNullOrEmpty(skuOk))
                    orderItemSKUs.Add(skuOk);

                if (!string.IsNullOrEmpty(skuOk))
                    orderItemSKUs.Add(skuOk);
                
                validOrderItems.Add(orderItem);
                orderItem.WarrantyDate = DateTime.UtcNow;
                warrantyRounds[orderItem.Id] = warrantyCount + 1;
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

        public async Task<List<WarrantyDetailResponseDto>> DetailsByIdAsync(string orderId)
        {
            var result = new List<WarrantyDetailResponseDto>();

            var order = await _unitOfWork.OrderRepository.GetWithItemsAndDressDetails(orderId);
            _validationService.CheckNotFound(order, $"Order with id {orderId} not found");

            // Chỉ lấy những OrderItem có ParentOrderItem
            var itemsWithParent = order.OrderItems
                .Where(x => x.ParentOrderItemId != null && x.ParentOrderItem != null && x.ParentOrderItem.Order != null)
                .ToList();

            // Nhóm theo đơn hàng gốc (ParentOrderItem.Order)
            var groupedByOriginalOrder = itemsWithParent
                .GroupBy(item => item.ParentOrderItem.Order)
                .ToList();

            foreach (var group in groupedByOriginalOrder)
            {
                var originalOrder = group.Key;

                var warrantyDto = new WarrantyDetailResponseDto
                {
                    OriginalOrder = new OrderWarrantyOnlyCode
                    {
                        Id = originalOrder.Id,
                        Code = originalOrder.Code,
                        ReceivedAt = (DateTime)originalOrder.ReceivedAt,
                        OrderItems = group
                        .Select(x => _mapper.Map<OrderItemGetByIdResponseDto>(x))
                        .ToList()
                    }

                };

                result.Add(warrantyDto);
            }

            return result;
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
    }
}