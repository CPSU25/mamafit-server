using AutoMapper;
using MamaFit.BusinessObjects.DTO.OrderDto;
using MamaFit.BusinessObjects.DTO.WarrantyRequestItemDto;
using MamaFit.Repositories.Implement;
using MamaFit.Repositories.Infrastructure;
using MamaFit.Repositories.Interface;
using MamaFit.Services.Interface;

namespace MamaFit.Services.Service;

public class WarrantyRequestItemService : IWarrantyRequestItemService
{
    private readonly IWarrantyRequestItemRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidationService _validation;
    private readonly IMapper _mapper;

    public WarrantyRequestItemService(
        IWarrantyRequestItemRepository repository,
        IValidationService validation,
        IMapper mapper,
        IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _validation = validation;
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }

    public async Task<PaginatedList<WarrantyRequestItemGetAllDto>> GetAllAsync(int index, int pageSize, string? search)
    {
        var items = await _repository.GetAllAsync(index, pageSize, search);
        var responseItems = items.Items
            .Select(item => _mapper.Map<WarrantyRequestItemGetAllDto>(item))
            .ToList();

        return new PaginatedList<WarrantyRequestItemGetAllDto>(
            responseItems,
            items.TotalCount,
            items.PageNumber,
            pageSize
        );
    }

    public async Task<List<WarrantyRequestItemDetailListDto>> GetAllDetailsByOrderItemIdAsync(string orderItemId)
    {
        var itemList = await _repository.GetAllRelatedByOrderItemAsync(orderItemId);
        _validation.CheckNotFound(itemList, $"Order item with id: {orderItemId} has no related order !!");

        var order = await _unitOfWork.OrderRepository.GetByOrderItemId(orderItemId);
        _validation.CheckNotFound(order, "Not found order origin ");

        var result = itemList.Select(x => _mapper.Map<WarrantyRequestItemDetailListDto>(x)).ToList();
        result.Add(new WarrantyRequestItemDetailListDto
        {
            Order = _mapper.Map<OrderGetByIdResponseDto>(order),
            WarrantyRequestItems = null
        });

        return result;
    }

    public async Task<WarrantyRequestItemDetailDto> GetDetailsByOrderItemIdAsync(string orderItemId)
    {
        var item = await _repository.GetByOrderItemIdAsync(orderItemId);
        _validation.CheckNotFound(item, $"No WarrantyRequestItem found with OrderItemId: {orderItemId}");

        return new WarrantyRequestItemDetailDto
        {
            WarrantyRequestItems = _mapper.Map<WarrantyRequestItemGetAllDto>(item),
            ParentOrder = item.OrderItem.ParentOrderItem?.Order != null
                ? _mapper.Map<OrderGetByIdResponseDto>(item.OrderItem.ParentOrderItem.Order)
                : null
        };
    }
}