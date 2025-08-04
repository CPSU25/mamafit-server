using AutoMapper;
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
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;
        private readonly IValidationService _validationService;

        public WarrantyRequestService(
            IUnitOfWork unitOfWork,
            IHttpContextAccessor httpContextAccessor,
            IMapper mapper,
            IValidationService validationService)
        {
            _unitOfWork = unitOfWork;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
            _validationService = validationService;
        }

        public async Task CreateAsync(WarrantyRequestCreateDto warrantyRequestCreateDto)
        {
            var oldOrderItem =
                await _unitOfWork.OrderItemRepository.GetByIdNotDeletedAsync(warrantyRequestCreateDto.WarrantyOrderItemId!);
            _validationService.CheckNotFound(oldOrderItem, "Order item don't exist!");
            
            var oldOrder = await _unitOfWork.OrderRepository.GetByIdNotDeletedAsync(oldOrderItem.OrderId);
            _validationService.CheckNotFound(oldOrder, "Order don't exist!");
            
            var warrantyRequestCount =
                await _unitOfWork.WarrantyRequestRepository.CountWarrantyForOrderItemAsync(oldOrderItem.Id);
            var newOrder = new Order
            {
                UserId = oldOrder.UserId,
                AddressId = oldOrder.AddressId,
                MeasurementId = oldOrder.MeasurementId,
                BranchId = oldOrder.BranchId,
                Type = OrderType.WARRANTY,
                Code = GenerateOrderCode(), 
                Status = OrderStatus.CONFIRMED,
                PaymentStatus = PaymentStatus.WARRANTY,
                PaymentType = PaymentType.FULL,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                CreatedBy = "System"
            };
            await _unitOfWork.OrderRepository.InsertAsync(newOrder);
            
            var newOrderItem = new OrderItem
            {
                OrderId = newOrder.Id,
                ParentOrderItemId = oldOrderItem.Id,
                MaternityDressDetailId = oldOrderItem.MaternityDressDetailId,
                PresetId = oldOrderItem.PresetId,
                ItemType = ItemType.WARRANTY,
                Price = oldOrderItem.Price, // hoặc = 0 nếu miễn phí bảo hành
                Quantity = 1,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                CreatedBy = "System"
            };
            await _unitOfWork.OrderItemRepository.InsertAsync(newOrderItem);
            
            var warrantyRequest = new WarrantyRequest
            {
                WarrantyOrderItemId = oldOrderItem.Id,
                Images = warrantyRequestCreateDto.Images,
                Description = warrantyRequestCreateDto.Description,
                Status = WarrantyRequestStatus.SUBMITTED,
                WarrantyRound = warrantyRequestCount + 1,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
            };
            await _unitOfWork.WarrantyRequestRepository.InsertAsync(warrantyRequest);
            await _unitOfWork.SaveChangesAsync();
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
    }
}