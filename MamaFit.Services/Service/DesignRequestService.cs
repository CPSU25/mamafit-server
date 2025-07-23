using AutoMapper;
using MamaFit.BusinessObjects.DTO.DesignRequestDto;
using MamaFit.BusinessObjects.Entity;
using MamaFit.BusinessObjects.Enum;
using MamaFit.Repositories.Implement;
using MamaFit.Repositories.Infrastructure;
using MamaFit.Services.Interface;
using Microsoft.AspNetCore.Http;

namespace MamaFit.Services.Service
{
    public class DesignRequestService : IDesignRequestService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IMapper _mapper;

        public DesignRequestService(IUnitOfWork unitOfWork, IHttpContextAccessor contextAccessor, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _contextAccessor = contextAccessor;
            _mapper = mapper;
        }

        public async Task CreateAsync(DesignRequestCreateDto requestDto)
        {
            // Kiểm tra User tồn tại
            var user = await _unitOfWork.UserRepository.GetByIdAsync(requestDto.UserId);
            if (user == null)
                throw new ErrorException(StatusCodes.Status404NotFound, ApiCodes.NOT_FOUND, "User is not available");

            // Kiểm tra OrderItem tồn tại
            var orderItem = await _unitOfWork.OrderItemRepository.GetByIdAsync(requestDto.OrderItemId);
            if (orderItem == null)
                throw new ErrorException(StatusCodes.Status404NotFound, ApiCodes.NOT_FOUND, "Order item is not available");

            var newRequest = _mapper.Map<DesignRequest>(requestDto);
            newRequest.User = user;
            newRequest.OrderItem = orderItem;
            newRequest.CreatedAt = DateTime.UtcNow;
            newRequest.CreatedBy = GetCurrentUserName();

            await _unitOfWork.DesignRequestRepository.InsertAsync(newRequest);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<DesignResponseDto> GetByIdAsync(string id)
        {
            var request = await _unitOfWork.DesignRequestRepository.GetByIdAsync(id);

            if (request == null)
                throw new ErrorException(StatusCodes.Status404NotFound, ApiCodes.NOT_FOUND, "Design request is not available");

            return _mapper.Map<DesignResponseDto>(request);
        }

        public async Task<PaginatedList<DesignResponseDto>> GetAllAsync(int index, int pageSize, string? search, EntitySortBy? sortBy)
        {
            var designList = await _unitOfWork.DesignRequestRepository.GetAllAsync( index, pageSize, search, sortBy);

            // Map từng phần tử trong danh sách Items
            var responseList = designList.Items.Select(item => _mapper.Map<DesignResponseDto>(item)).ToList();

            // Tạo PaginatedList mới với các đối tượng đã map
            var paginatedResponse = new PaginatedList<DesignResponseDto>(
                responseList,
                designList.TotalCount,
                designList.PageNumber,
                designList.PageSize
            );

            return paginatedResponse;
        }

        public async Task DeleteAsync(string id)
        {
            var request = await _unitOfWork.DesignRequestRepository.GetByIdAsync(id);
            if (request == null)
                throw new ErrorException(StatusCodes.Status404NotFound, ApiCodes.NOT_FOUND, "Design request is not available");

            await _unitOfWork.DesignRequestRepository.SoftDeleteAsync(id);
            await _unitOfWork.SaveChangesAsync();
        }

        private string GetCurrentUserName()
        {
            return _contextAccessor.HttpContext?.User?.FindFirst("name")?.Value ?? "System";
        }
    }
}