using AutoMapper;
using MamaFit.BusinessObjects.DTO.OrderDto;
using MamaFit.BusinessObjects.DTO.OrderItemDto;
using MamaFit.BusinessObjects.DTO.WarrantyRequestItemDto;
using MamaFit.Repositories.Infrastructure;
using MamaFit.Repositories.Interface;
using MamaFit.Services.Interface;

namespace MamaFit.Services.Service;

public class WarrantyRequestItemService : IWarrantyRequestItemService
{
    private readonly IWarrantyRequestItemRepository _repository;
    private readonly IValidationService _validation;
    private readonly IMapper _mapper;
    
    public WarrantyRequestItemService(
        IWarrantyRequestItemRepository repository,
        IValidationService validation,
        IMapper mapper)
    {
        _repository = repository;
        _validation = validation;
        _mapper = mapper;
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