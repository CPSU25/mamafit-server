using AutoMapper;
using MamaFit.BusinessObjects.DTO.NotificationDto;
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

        public async Task<string> CreateAsync(WarrantyRequestCreateDto dto)
        {
            var config = await _configService.GetConfig();
            var validOrderItems = new List<OrderItem>();
            var orderItemSKUs = new List<string>();
            var notReceivedItems = new List<string>();
            foreach (var itemDto in dto.Items)
            {
                var orderItem = await _unitOfWork.OrderItemRepository.GetByIdNotDeletedAsync(itemDto.OrderItemId);
                _validationService.CheckNotFound(orderItem, $"Order item {itemDto.OrderItemId} not found!");

                var order = await _unitOfWork.OrderRepository.GetByIdNotDeletedAsync(orderItem.OrderId!);
                _validationService.CheckNotFound(order, $"Order of item {orderItem.Id} not found!");

                // Check thanh toán đủ hoặc đặt cọc đủ
                if (order.PaymentStatus != PaymentStatus.PAID_FULL && order.PaymentStatus != PaymentStatus.PAID_DEPOSIT_COMPLETED)
                    throw new ErrorException(StatusCodes.Status400BadRequest, ApiCodes.BAD_REQUEST,
                        $"Order {order.Code} must be paid before requesting warranty!");

                // Check đã nhận hàng
                if (order.ReceivedAt == null)
                    notReceivedItems.Add(order.Code);

                // Check hết hạn bảo hành
                var daysSinceReceived = (DateTime.UtcNow - order.ReceivedAt!.Value).TotalDays;
                if (daysSinceReceived > config.Fields?.WarrantyPeriod)
                    throw new ErrorException(StatusCodes.Status400BadRequest, ApiCodes.BAD_REQUEST,
                        $"Order item {orderItem.Id} is out of warranty (more than {config.Fields.WarrantyPeriod} days since received)!");

                // Check số lượt bảo hành đã dùng cho item này
                var warrantyCount = await _warrantyRequestItemRepository.CountWarrantyRequestItemsAsync(orderItem.Id);
                if (warrantyCount >= config.Fields?.WarrantyTime)
                    throw new ErrorException(StatusCodes.Status400BadRequest, ApiCodes.BAD_REQUEST,
                        $"You have reached the maximum warranty requests ({config.Fields?.WarrantyTime}) for item {orderItem.Id}!");

                string? sku = null;
                if (orderItem.Preset != null)
                    sku = orderItem.Preset.SKU;
                else if (orderItem.MaternityDressDetail != null)
                    sku = orderItem.MaternityDressDetail.SKU;

                if (!string.IsNullOrEmpty(sku))
                    orderItemSKUs.Add(sku);
                
                validOrderItems.Add(orderItem);
            }
            
            if (notReceivedItems.Count > 0)
                throw new ErrorException(StatusCodes.Status400BadRequest, ApiCodes.BAD_REQUEST,
                    $"Order(s) {string.Join(", ", notReceivedItems)} must be received before requesting warranty!");
            //Tao đơn hàng bảo hành
            var warrantyOrder = new Order
            {
                UserId = dto.UserId,
                AddressId = dto.AddressId,
                MeasurementId = dto.MeasurementId,
                BranchId = dto.BranchId,
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
                    ParentOrderItemId = orderItem.Id,
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
                };
                await _warrantyRequestItemRepository.InsertAsync(requestItem);
            }

            await _unitOfWork.SaveChangesAsync();

            // Update tổng phí
            // warrantyRequest.TotalFee = totalFee;
            //await _unitOfWork.WarrantyRequestRepository.UpdateAsync(warrantyRequest);
            
            await _notificationService.SendAndSaveNotificationAsync(new NotificationRequestDto
            {
                ReceiverId = dto.UserId,
                NotificationTitle = "Yêu cầu bảo hành mới",
                NotificationContent = $"Bạn đã tạo thành công bảo hành cho các sản phẩm với SKU: {string.Join(", ", orderItemSKUs)}.",
                ActionUrl = $"/warranty-requests/{warrantyRequest.Id}",
                Metadata = new Dictionary<string, string>
                {
                    { "warrantyRequestId", warrantyRequest.Id },
                    { "orderItemIds", string.Join(",", validOrderItems.Select(x => x.Id)) },
                    { "orderId", warrantyOrder.Id }
                }
            });
            return warrantyRequest.Id;
        }

        // public async Task<GetDetailDto> GetWarrantyRequestByOrderItemIdAsync(string orderItemId)
        // {
        //     var warrantyRequest = await _unitOfWork.WarrantyRequestRepository.GetWarrantyRequestByOrderItemIdAsync(orderItemId);
        //     _validationService.CheckNotFound(warrantyRequest, $"Warranty request for order item {orderItemId} not found");
        //
        //     return _mapper.Map<GetDetailDto>(warrantyRequest);
        // }

        public async Task DeleteAsync(string id)
        {
            var warrantyRequest = await _unitOfWork.WarrantyRequestRepository.GetByIdAsync(id);
            _validationService.CheckNotFound(warrantyRequest, $"Warranty request with id:{id} not found");

            await _unitOfWork.WarrantyRequestRepository.SoftDeleteAsync(warrantyRequest);
            await _unitOfWork.SaveChangesAsync();
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

        // public async Task<WarrantyRequestGetByIdDto> GetWarrantyRequestByIdAsync(string id)
        // {
        //     var result = await _unitOfWork.WarrantyRequestRepository.GetWarrantyRequestByIdAsync(id);
        //     _validationService.CheckNotFound(result, $"Warranty request with id:{id} not found");
        //
        //     return _mapper.Map<WarrantyRequestGetByIdDto>(result);
        // }

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