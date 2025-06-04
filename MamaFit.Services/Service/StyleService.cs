using AutoMapper;
using MamaFit.BusinessObjects.DTO.CategoryDto;
using MamaFit.BusinessObjects.DTO.StyleDto;
using MamaFit.BusinessObjects.Entity;
using MamaFit.Repositories.Infrastructure;
using MamaFit.Repositories.Interface;
using MamaFit.Services.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace MamaFit.Services.Service
{
    public class StyleService : IStyleService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IMapper _mapper;

        public StyleService(IUnitOfWork unitOfWork, IHttpContextAccessor contextAccessor, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _contextAccessor = contextAccessor;
            _mapper = mapper;
        }

        public async Task CreateAsync(StyleRequestDto requestDto)
        {
            var categoryRepo = _unitOfWork.GetRepository<Category>();// Repository category

            var category = await categoryRepo.GetByIdAsync(requestDto.CategoryId); // Tìm category
            if (category == null)
            {
                throw new ErrorException(StatusCodes.Status404NotFound, ErrorCode.NotFound, "Category is not available");
            }
            var styleRepo = _unitOfWork.GetRepository<Style>(); // Repo style

            var newStyle = _mapper.Map<Style>(requestDto); // Map Style
            newStyle.Category = category;
            newStyle.CreatedBy = GetCurrentUserName();
            newStyle.CreatedAt = DateTime.UtcNow;

            await styleRepo.InsertAsync(newStyle); // Tạo mới style
            await styleRepo.SaveAsync();
        }

        public async Task DeleteAsync(string id)
        {
            var styleRepo = _unitOfWork.GetRepository<Style>(); // Repo style

            var oldStyle = await styleRepo.GetByIdAsync(id);
            if (oldStyle == null)
            {
                throw new ErrorException(StatusCodes.Status404NotFound, ErrorCode.NotFound, "Style is not available");
            }
            await styleRepo.SoftDeleteAsync(id);
            await styleRepo.SaveAsync();
        }

        public async Task<PaginatedList<StyleResponseDto>> GetAllAsync(int index, int pageSize, string? search, string? sortBy)
        {
            var styleRepo = _unitOfWork.GetRepository<Style>(); // Repo của Category

            var query = styleRepo.Entities
                .Where(c => !c.IsDeleted);

            if (!string.IsNullOrWhiteSpace(search)) // Search
            {
                query = query.Where(u => u.Name.Contains(search));
            }

            query = sortBy?.ToLower() switch
            {
                "name_asc" => query.OrderBy(u => u.Name),
                "name_desc" => query.OrderByDescending(u => u.Name),
                "createdat_asc" => query.OrderBy(u => u.CreatedAt),
                "createdat_desc" => query.OrderByDescending(u => u.CreatedAt),
                _ => query.OrderByDescending(u => u.CreatedAt) // default
            };

            var pagedResult = await styleRepo.GetPagging(query, index, pageSize); // Paging

            var listStyle = pagedResult.Items
                .Select(_mapper.Map<StyleResponseDto>)
                .ToList();

            var responsePaginatedList = new PaginatedList<StyleResponseDto>(
                listStyle,
                pagedResult.TotalCount,
                pagedResult.PageNumber,
                pageSize
            );

            return responsePaginatedList;
        }

        public async Task<StyleResponseDto> GetByIdAsync(string id)
        {
            var styleRepo = _unitOfWork.GetRepository<Style>(); // Repo style

            var oldStyle = await styleRepo.GetByIdAsync(id);
            if (oldStyle == null)
            {
                throw new ErrorException(StatusCodes.Status404NotFound, ErrorCode.NotFound, "Style is not available");
            }
            return _mapper.Map<StyleResponseDto>(oldStyle);
        }

        public async Task UpdateAsync(string id, StyleRequestDto requestDto)
        {
            var styleRepo = _unitOfWork.GetRepository<Style>(); // Repo style

            var oldStyle = await styleRepo.GetByIdAsync(id);
            if (oldStyle == null)
            {
                throw new ErrorException(StatusCodes.Status404NotFound, ErrorCode.NotFound, "Style is not available");
            }

            _mapper.Map(requestDto, oldStyle); // Map request 
            oldStyle.UpdatedAt = DateTime.UtcNow;
            oldStyle.UpdatedBy = GetCurrentUserName();
            await styleRepo.UpdateAsync(oldStyle);
            await styleRepo.SaveAsync();
        }

        private string GetCurrentUserName()
        {
            return _contextAccessor.HttpContext?.User?.FindFirst("name")?.Value ?? "System";
        }
    }
}
