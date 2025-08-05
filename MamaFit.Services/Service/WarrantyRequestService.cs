using AutoMapper;
using MamaFit.BusinessObjects.DTO.NotificationDto;
using MamaFit.BusinessObjects.DTO.WarrantyRequestDto;
using MamaFit.BusinessObjects.Entity;
using MamaFit.BusinessObjects.Enum;
using MamaFit.Repositories.Implement;
using MamaFit.Repositories.Infrastructure;
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

        public WarrantyRequestService(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IValidationService validationService,
            INotificationService notificationService,
            IConfigService configService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _validationService = validationService;
            _notificationService = notificationService;
            _configService = configService;
        }

        public async Task<string> CreateAsync(WarrantyRequestCreateDto warrantyRequestCreateDto)
        {
            var rootOrderItem = await GetRootOrderItemAsync(warrantyRequestCreateDto.WarrantyOrderItemId!);
            _validationService.CheckNotFound(rootOrderItem, "Root order item don't exist!");

            var rootOrder = await _unitOfWork.OrderRepository.GetByIdNotDeletedAsync(rootOrderItem.OrderId!);
            _validationService.CheckNotFound(rootOrder, "Order don't exist!");

            if (rootOrder!.PaymentStatus != PaymentStatus.PAID_FULL &&
                rootOrder.PaymentStatus != PaymentStatus.PAID_DEPOSIT_COMPLETED)
                throw new ErrorException(StatusCodes.Status400BadRequest, ApiCodes.BAD_REQUEST,
                    "Order must be paid before requesting warranty!");

            var config = await _configService.GetConfig();

            var warrantyRequestCount =
                await _unitOfWork.WarrantyRequestRepository.CountWarrantyForOrderItemAsync(rootOrderItem.Id);

            if (warrantyRequestCount >= config.Fields?.WarrantyTime)
                throw new ErrorException(StatusCodes.Status400BadRequest, ApiCodes.BAD_REQUEST,
                    $"You have reached the maximum warranty requests ({config.Fields?.WarrantyTime}) for this item!");

            if (rootOrder.ReceivedAt == null)
                throw new ErrorException(StatusCodes.Status400BadRequest, ApiCodes.BAD_REQUEST,
                    "Order must be received before requesting warranty!");
            var daysSinceReceived = (DateTime.UtcNow - rootOrder.ReceivedAt.Value).TotalDays;
            if (daysSinceReceived > config.Fields?.WarrantyPeriod)
                throw new ErrorException(StatusCodes.Status400BadRequest, ApiCodes.BAD_REQUEST,
                    $"The product is out of warranty (more than {config.Fields.WarrantyPeriod} days since received)!");
            var newOrder = new Order
            {
                UserId = rootOrder.UserId,
                AddressId = rootOrder.AddressId,
                MeasurementId = rootOrder.MeasurementId,
                BranchId = rootOrder.BranchId,
                Type = OrderType.WARRANTY,
                Code = GenerateOrderCode(),
                Status = OrderStatus.CONFIRMED,
                PaymentStatus = PaymentStatus.WARRANTY,
                PaymentType = PaymentType.FULL,
            };
            await _unitOfWork.OrderRepository.InsertAsync(newOrder);

            var newOrderItem = new OrderItem
            {
                OrderId = newOrder.Id,
                ParentOrderItemId = rootOrderItem.Id,
                MaternityDressDetailId = rootOrderItem.MaternityDressDetailId,
                PresetId = rootOrderItem.PresetId,
                ItemType = ItemType.WARRANTY,
                Price = rootOrderItem.Price, // hoặc = 0 nếu miễn phí bảo hành
                Quantity = 1,
            };
            await _unitOfWork.OrderItemRepository.InsertAsync(newOrderItem);

            var warrantyRequest = new WarrantyRequest
            {
                WarrantyOrderItemId = rootOrderItem.Id,
                Images = warrantyRequestCreateDto.Images,
                Description = warrantyRequestCreateDto.Description,
                Status = WarrantyRequestStatus.SUBMITTED,
                WarrantyRound = warrantyRequestCount + 1,
            };
            await _unitOfWork.WarrantyRequestRepository.InsertAsync(warrantyRequest);
            await _unitOfWork.SaveChangesAsync();
            await _notificationService.SendAndSaveNotificationAsync(new NotificationRequestDto
            {
                ReceiverId = rootOrder.UserId,
                NotificationTitle = "New Warranty Request",
                NotificationContent =
                    $"Your warranty request for order item {rootOrderItem.Preset?.Name ?? rootOrderItem.MaternityDressDetail?.Name} has been created successfully.",
                ActionUrl = $"/warranty-requests/{warrantyRequest.Id}",
                Metadata = new Dictionary<string, string>
                {
                    { "warrantyRequestId", warrantyRequest.Id },
                    { "orderItemId", rootOrderItem.Id },
                    { "orderId", newOrder.Id }
                }
            });
            return newOrder.Id;
        }
        
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

        public async Task<WarrantyRequestGetByIdDto> GetWarrantyRequestByIdAsync(string id)
        {
            var result = await _unitOfWork.WarrantyRequestRepository.GetWarrantyRequestByIdAsync(id);
            _validationService.CheckNotFound(result, $"Warranty request with id:{id} not found");

            return _mapper.Map<WarrantyRequestGetByIdDto>(result);
        }

        public async Task UpdateAsync(string id, WarrantyRequestUpdateDto warrantyRequestUpdateDto)
        {
            var warrantyRequest = await _unitOfWork.WarrantyRequestRepository.GetByIdAsync(id);
            _validationService.CheckNotFound(warrantyRequest, $"Warranty request with id:{id} not found");

            _mapper.Map(warrantyRequestUpdateDto, warrantyRequest);
            await _unitOfWork.WarrantyRequestRepository.UpdateAsync(warrantyRequest);
            await _unitOfWork.SaveChangesAsync();
        }

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
    }
}