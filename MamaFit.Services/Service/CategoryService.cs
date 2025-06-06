using AutoMapper;
using MamaFit.BusinessObjects.DTO.CategoryDto;
using MamaFit.BusinessObjects.Entity;
using MamaFit.Repositories.Infrastructure;
using MamaFit.Repositories.Interface;
using MamaFit.Services.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace MamaFit.Services.Service
{
    public class CategoryService : ICategoryService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IMapper _mapper;

        public CategoryService(IUnitOfWork unitOfWork, IHttpContextAccessor contextAccessor, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _contextAccessor = contextAccessor;
            _mapper = mapper;
        }

        public async Task CreateAsync(CategoryRequestDto requestDto)
        {
            var categoryRepo = _unitOfWork.GetRepository<Category>(); // Repo của Category

            var newCategory = new Category // Tạo mới Category + List<Style>
            {
                Name = requestDto.Name,
                Description = requestDto.Description,
                Images = requestDto.Images,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = GetCurrentUserName()
            };

            await categoryRepo.InsertAsync(newCategory); // Insert + Save changes
            await categoryRepo.SaveAsync();
        }

        public async Task DeleteAsync(string id)
        {
            var categoryRepo = _unitOfWork.GetRepository<Category>(); // Repo của Category

            var oldCategory = await GetByIdAsync(id); // Tìm category

            if (oldCategory == null)
                throw new ErrorException(StatusCodes.Status404NotFound,
                ErrorCode.NotFound, "Category not found!"); // Nếu không có

            await categoryRepo.SoftDeleteAsync(oldCategory);
            await categoryRepo.SaveAsync();
        }

        public async Task<PaginatedList<CategoryResponseDto>> GetAllAsync(int index, int pageSize, string? search, string? sortBy)
        {
            var categoryRepo = _unitOfWork.GetRepository<Category>(); // Repo của Category

            var query = categoryRepo.Entities
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

            var pagedResult = await categoryRepo.GetPagging(query, index, pageSize); // Paging

            var listCategory = pagedResult.Items
                .Select(_mapper.Map<CategoryResponseDto>)
                .ToList();

            var responsePaginatedList = new PaginatedList<CategoryResponseDto>(
                listCategory,
                pagedResult.TotalCount,
                pagedResult.PageNumber,
                pageSize
            );

            return responsePaginatedList;
        }

        public async Task<CategoryResponseDto> GetByIdAsync(string id)
        {
            var categoryRepo = _unitOfWork.GetRepository<Category>(); // Repo của Category

            var oldCategory = await categoryRepo.Entities
                .Include(c => c.Styles)
                .Where(c => !c.IsDeleted)
                .FirstOrDefaultAsync(c => c.Id.Equals(id)); // Tìm Category

            if (oldCategory == null)
                throw new ErrorException(StatusCodes.Status404NotFound,
                ErrorCode.NotFound, "Category not found!"); // Nếu không có

            return _mapper.Map<CategoryResponseDto>(oldCategory);
        }

        public async Task UpdateAsync(string id, CategoryRequestDto requestDto)
        {
            var categoryRepo = _unitOfWork.GetRepository<Category>(); // Repo của Category

            var oldCategory = await categoryRepo.Entities
                .Where(c => !c.IsDeleted)
                .FirstOrDefaultAsync(c => c.Id.Equals(id)); // Tìm Category

            if (oldCategory == null)
                throw new ErrorException(StatusCodes.Status404NotFound,
                ErrorCode.NotFound, "MaternityDress not found!"); // Nếu không có

            _mapper.Map(requestDto, oldCategory); // Auto mapper Dto => category
            oldCategory.UpdatedAt = DateTime.UtcNow;
            oldCategory.UpdatedBy = GetCurrentUserName();

            await categoryRepo.UpdateAsync(oldCategory);    //Update + Save changes
            await categoryRepo.SaveAsync();
        }

        private string GetCurrentUserName()
        {
            return _contextAccessor.HttpContext?.User?.FindFirst("name")?.Value ?? "System";
        }
    }
}
