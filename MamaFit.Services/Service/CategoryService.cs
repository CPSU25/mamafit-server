using AutoMapper;
using MamaFit.BusinessObjects.DTO.CategoryDto;
using MamaFit.BusinessObjects.Entity;
using MamaFit.Repositories.Infrastructure;
using MamaFit.Repositories.Interface;
using MamaFit.Services.Interface;
using Microsoft.AspNetCore.Http;

namespace MamaFit.Services.Service
{
    public class CategoryService : ICategoryService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _contextAccessor;

        public CategoryService(IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor contextAccessor)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _contextAccessor = contextAccessor;
        }

        private string GetCurrentUserName()
        {
            return _contextAccessor.HttpContext?.User?.FindFirst("name")?.Value ?? "System";
        }

        public async Task CreateAsync(CategoryRequestDto requestDto)
        {
            var newCategory = new Category
            {
                Name = requestDto.Name,
                Description = requestDto.Description,
                Images = requestDto.Images,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = GetCurrentUserName()
            };

            await _unitOfWork.CategoryRepository.InsertAsync(newCategory);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteAsync(string id)
        {

            var oldCategory = await _unitOfWork.CategoryRepository.GetByIdAsync(id);

            if (oldCategory == null || oldCategory.IsDeleted)
                throw new ErrorException(StatusCodes.Status404NotFound, ErrorCode.NotFound, "Category not found!");
            if(oldCategory.Styles != null)
                throw new ErrorException(StatusCodes.Status400BadRequest, ErrorCode.BadRequest, "Cannot delete this category as policy restrict");

            await _unitOfWork.CategoryRepository.SoftDeleteAsync(id);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<PaginatedList<CategoryResponseDto>> GetAllAsync(int index, int pageSize, string? search, string? sortBy)
        {
            var categoryList = await _unitOfWork.CategoryRepository.GetAllAsync(index, pageSize, search, sortBy);

            // Map từng phần tử trong danh sách Items
            var responseList = categoryList.Items.Select(item => _mapper.Map<CategoryResponseDto>(item)).ToList();

            // Tạo PaginatedList mới với các đối tượng đã map
            var paginatedResponse = new PaginatedList<CategoryResponseDto>(
                responseList,
                categoryList.TotalCount,
                categoryList.PageNumber,
                categoryList.PageSize
            );

            return paginatedResponse;
        }

        public async Task<CategoryGetByIdResponse> GetByIdAsync(string id)
        {

            var oldCategory = await _unitOfWork.CategoryRepository.GetById(id);

            if (oldCategory == null || oldCategory.IsDeleted)
                throw new ErrorException(StatusCodes.Status404NotFound, ErrorCode.NotFound, "Category not found!");

            return _mapper.Map<CategoryGetByIdResponse>(oldCategory);
        }

        public async Task UpdateAsync(string id, CategoryRequestDto requestDto)
        {
            var oldCategory = await _unitOfWork.CategoryRepository.GetByIdAsync(id);

            if (oldCategory == null || oldCategory.IsDeleted)
                throw new ErrorException(StatusCodes.Status404NotFound, ErrorCode.NotFound, "Category not found!");

            _mapper.Map(requestDto, oldCategory);
            oldCategory.UpdatedAt = DateTime.UtcNow;
            oldCategory.UpdatedBy = GetCurrentUserName();

            await _unitOfWork.CategoryRepository.UpdateAsync(oldCategory);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
