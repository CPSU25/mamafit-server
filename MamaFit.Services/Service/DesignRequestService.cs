using AutoMapper;
using MamaFit.BusinessObjects.DTO.DesignRequestDto;
using MamaFit.BusinessObjects.Entity;
using MamaFit.Repositories.Infrastructure;
using MamaFit.Repositories.Interface;
using MamaFit.Services.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

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
            var userRepo = _unitOfWork.GetRepository<ApplicationUser>();
            var orderItemRepo = _unitOfWork.GetRepository<OrderItem>();
            var designRequestRepo = _unitOfWork.GetRepository<DesignRequest>();

            // Kiểm tra User tồn tại
            var user = await userRepo.GetByIdAsync(requestDto.UserId);
            if (user == null)
                throw new ErrorException(StatusCodes.Status404NotFound, ErrorCode.NotFound, "User is not available");

            // Kiểm tra OrderItem tồn tại
            var orderItem = await orderItemRepo.GetByIdAsync(requestDto.OrderItemId);
            if (orderItem == null)
                throw new ErrorException(StatusCodes.Status404NotFound, ErrorCode.NotFound, "Order item is not available");

            var newRequest = _mapper.Map<DesignRequest>(requestDto);
            newRequest.User = user;
            newRequest.OrderItem = orderItem;
            newRequest.CreatedAt = DateTime.UtcNow;
            newRequest.CreatedBy = GetCurrentUserName();

            await designRequestRepo.InsertAsync(newRequest);
            await designRequestRepo.SaveAsync();
        }

        public async Task<DesignResponseDto> GetByIdAsync(string id)
        {
            var designRequestRepo = _unitOfWork.GetRepository<DesignRequest>();
            var request = await designRequestRepo.Entities
                .Include(d => d.User)
                .Include(d => d.OrderItem)
                .FirstOrDefaultAsync(d => d.Id == id && !d.IsDeleted);

            if (request == null)
                throw new ErrorException(StatusCodes.Status404NotFound, ErrorCode.NotFound, "Design request is not available");

            return _mapper.Map<DesignResponseDto>(request);
        }

        public async Task<PaginatedList<DesignResponseDto>> GetAllAsync(int index, int pageSize, string? search, string? sortBy)
        {
            var designRequestRepo = _unitOfWork.GetRepository<DesignRequest>();
            var query = designRequestRepo.Entities
                .Include(d => d.User)
                .Include(d => d.OrderItem)
                .Where(d => !d.IsDeleted);

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(d => d.Description!.Contains(search));
            }

            query = sortBy?.ToLower() switch
            {
                "createdat_asc" => query.OrderBy(d => d.CreatedAt),
                "createdat_desc" => query.OrderByDescending(d => d.CreatedAt),
                _ => query.OrderByDescending(d => d.CreatedAt)
            };

            var pagedResult = await designRequestRepo.GetPagging(query, index, pageSize);

            var result = pagedResult.Items.Select(_mapper.Map<DesignResponseDto>).ToList();
            return new PaginatedList<DesignResponseDto>(
                result,
                pagedResult.TotalCount,
                pagedResult.PageNumber,
                pageSize
            );
        }

        public async Task DeleteAsync(string id)
        {
            var designRequestRepo = _unitOfWork.GetRepository<DesignRequest>();

            var request = await designRequestRepo.GetByIdAsync(id);
            if (request == null)
                throw new ErrorException(StatusCodes.Status404NotFound, ErrorCode.NotFound, "Design request is not available");

            await designRequestRepo.SoftDeleteAsync(id);
            await designRequestRepo.SaveAsync();
        }

        private string GetCurrentUserName()
        {
            return _contextAccessor.HttpContext?.User?.FindFirst("name")?.Value ?? "System";
        }
    }
}