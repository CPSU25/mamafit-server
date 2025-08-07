using AutoMapper;
using MamaFit.BusinessObjects.DTO.CartItemDto;
using MamaFit.BusinessObjects.DTO.MaternityDressDetailDto;
using MamaFit.BusinessObjects.DTO.PresetDto;
using MamaFit.BusinessObjects.Enum;
using MamaFit.Repositories.Implement;
using MamaFit.Repositories.Infrastructure;
using MamaFit.Services.ExternalService.Redis;
using MamaFit.Services.Interface;
using Microsoft.AspNetCore.Http;

namespace MamaFit.Services.Service;

public class CartItemService : ICartItemService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IValidationService _validation;
    private readonly ICacheService _cacheService;
    private readonly IHttpContextAccessor _contextAccessor;

    public CartItemService(IUnitOfWork unitOfWork, IMapper mapper, IValidationService validation, ICacheService cacheService, IHttpContextAccessor contextAccessor)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _validation = validation;
        _cacheService = cacheService;
        _contextAccessor = contextAccessor;
    }

    public async Task<List<CartItemResponseDto>> GetAllAsync()
    {
        var userId = GetCurrentUserId();
        var user = await _unitOfWork.UserRepository.GetByIdAsync(userId);
        _validation.CheckNotFound(user, "Need to login!!");

        var cacheKey = $"cart:user:{userId}";

        // Lấy tất cả cart items từ Hash
        var cartItemsHash = await _cacheService.GetAllHashAsync<CartItem>(cacheKey);

        // Chỉ lấy các field là cart items (bỏ qua metadata như created_at, updated_at)
        var cartItems = cartItemsHash
            .Where(x => x.Key.StartsWith("item:") && x.Value != null)
            .Select(x => x.Value!)
            .ToList();

        return _mapper.Map<List<CartItemResponseDto>>(cartItems);
    }

    public async Task<List<CartItemResponseDto>> CreateAsync(CartItemRequestDto model)
    {
        var userId = GetCurrentUserId();
        var user = await _unitOfWork.UserRepository.GetByIdAsync(userId);
        _validation.CheckNotFound(user, "Need to login!!");

        var cacheKey = $"cart:user:{userId}";
        var itemKey = $"item:{model.ItemId}";
        CartItem cartItem = null;
        // Kiểm tra item đã có trong cart chưa
        var existingItem = await _cacheService.GetHashAsync<CartItem>(cacheKey, itemKey);

        if (existingItem == null)
        {
            // Validate item tồn tại
            if (model.Type == ItemType.READY_TO_BUY)
            {
                var item = await _unitOfWork.MaternityDressDetailRepository.GetByIdAsync(model.ItemId);
                _validation.CheckNotFound(item, $"Maternity dress with id: {model.ItemId} not found");

                if (item.Quantity < model.Quantity)
                    throw new ErrorException(StatusCodes.Status400BadRequest, ApiCodes.BAD_REQUEST, "Quantity must be not over than in storage!");

                cartItem = new CartItem
                {
                    ItemId = model.ItemId,
                    Quantity = model.Quantity,
                    Type = model.Type,
                    MaternityDressDetail = _mapper.Map<MaternityDressDetailResponseDto>(item)
                };
            }
            else if (model.Type == ItemType.PRESET)
            {
                var item = await _unitOfWork.PresetRepository.GetByIdAsync(model.ItemId);
                _validation.CheckNotFound(item, $"Preset with id: {model.ItemId} not found");
                cartItem = new CartItem
                {
                    ItemId = model.ItemId,
                    Quantity = model.Quantity,
                    Type = model.Type,
                    Preset = _mapper.Map<PresetGetAllResponseDto>(item)
                };
            }

            // Lưu cart item mới
            await _cacheService.SetHashAsync(cacheKey, itemKey, cartItem, TimeSpan.FromDays(7));
        }
        else
        {
            // Cập nhật quantity cho item đã có
            if (model.Type == ItemType.READY_TO_BUY)
            {
                var item = await _unitOfWork.MaternityDressDetailRepository.GetByIdAsync(model.ItemId);
                if (item != null && item.Quantity < (existingItem.Quantity + model.Quantity))
                    throw new ErrorException(StatusCodes.Status400BadRequest, ApiCodes.BAD_REQUEST, "Quantity must be not over than in storage!");
            }
            else if (model.Type == ItemType.PRESET)
            {
                var item = await _unitOfWork.PresetRepository.GetByIdAsync(model.ItemId);
                _validation.CheckNotFound(item, $"Preset with id: {model.ItemId} not found");
            }
            existingItem.Quantity += model.Quantity;

            // Cập nhật cart item
            await _cacheService.SetHashAsync(cacheKey, itemKey, existingItem, TimeSpan.FromDays(7));
        }

        var cartItemsHash = await _cacheService.GetAllHashAsync<CartItem>(cacheKey);

        // Chỉ lấy các field là cart items (bỏ qua metadata như created_at, updated_at)
        var cartItems = cartItemsHash
            .Where(x => x.Key.StartsWith("item:") && x.Value != null)
            .Select(x => x.Value!)
            .ToList();

        return _mapper.Map<List<CartItemResponseDto>>(cartItems);
    }

    public async Task<List<CartItemResponseDto>> UpdateAsync(CartItemRequestDto request)
    {
        var userId = GetCurrentUserId();
        var user = await _unitOfWork.UserRepository.GetByIdAsync(userId);
        _validation.CheckNotFound(user, "Need to login!!");

        var cacheKey = $"cart:user:{userId}";
        var itemKey = $"item:{request.ItemId}";

        // Lấy cart item hiện tại
        var existingItem = await _cacheService.GetHashAsync<CartItem>(cacheKey, itemKey);
        _validation.CheckNotFound(existingItem, $"Cart item with id: {request.ItemId} not found");

        // Validate quantity nếu là READY_TO_BUY
        if (existingItem.Type == ItemType.READY_TO_BUY)
        {
            var item = await _unitOfWork.MaternityDressDetailRepository.GetByIdAsync(request.ItemId);
            if (item != null && item.Quantity < request.Quantity)
                throw new ErrorException(StatusCodes.Status400BadRequest, ApiCodes.BAD_REQUEST, "Quantity must be not over than in storage!");
        }

        // Cập nhật quantity
        existingItem.Quantity = request.Quantity;

        // Lưu lại
        await _cacheService.SetHashAsync(cacheKey, itemKey, existingItem, TimeSpan.FromDays(7));

        // Lấy tất cả cart items từ Hash
        var cartItemsHash = await _cacheService.GetAllHashAsync<CartItem>(cacheKey);

        // Chỉ lấy các field là cart items (bỏ qua metadata như created_at, updated_at)
        var cartItems = cartItemsHash
            .Where(x => x.Key.StartsWith("item:") && x.Value != null)
            .Select(x => x.Value!)
            .ToList();

        return _mapper.Map<List<CartItemResponseDto>>(cartItems);
    }

    public async Task<bool> DeleteAsync(string itemId)
    {
        var userId = GetCurrentUserId();
        var user = await _unitOfWork.UserRepository.GetByIdAsync(userId);
        _validation.CheckNotFound(user, "Need to login!!");

        var cacheKey = $"cart:user:{userId}";
        var itemKey = $"item:{itemId}";

        // Kiểm tra item có tồn tại không
        var existingItem = await _cacheService.GetHashAsync<CartItem>(cacheKey, itemKey);
        if (existingItem == null)
            return false;

        // Xóa item khỏi hash
        var deleted = await _cacheService.DeleteHashFieldAsync(cacheKey, itemKey);

        return deleted;
    }

    // Thêm methods hỗ trợ
    public async Task<bool> ClearCartAsync()
    {
        var userId = GetCurrentUserId();
        var cacheKey = $"cart:user:{userId}";

        await _cacheService.RemoveKeyAsync(cacheKey);
        return true;
    }

    private string GetCurrentUserId()
    {
        return _contextAccessor.HttpContext?.User?.FindFirst("userId")?.Value ?? "System";
    }

}