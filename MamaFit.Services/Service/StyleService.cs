using AutoMapper;
using MamaFit.BusinessObjects.DTO.StyleDto;
using MamaFit.BusinessObjects.Entity;
using MamaFit.Repositories.Infrastructure;
using MamaFit.Repositories.Interface;
using MamaFit.Services.Interface;
using Microsoft.AspNetCore.Http;

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
            if (requestDto.CategoryId != null)
            {
                var category = await _unitOfWork.CategoryRepository.GetByIdAsync(requestDto.CategoryId); // Tìm category
                if (category == null)
                    throw new ErrorException(StatusCodes.Status404NotFound, ApiCodes.NOT_FOUND, "Category is not available");

                var newStyle = _mapper.Map<Style>(requestDto); // Map Style
                newStyle.Category = category;
                newStyle.CreatedBy = GetCurrentUserName();
                newStyle.CreatedAt = DateTime.UtcNow;

                await _unitOfWork.StyleRepository.InsertAsync(newStyle); // Tạo mới style
            }

            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteAsync(string id)
        {
            var oldStyle = await _unitOfWork.StyleRepository.GetByIdAsync(id);
            if (oldStyle == null)
            {
                throw new ErrorException(StatusCodes.Status404NotFound, ApiCodes.NOT_FOUND, "Style is not available");
            }
            await _unitOfWork.StyleRepository.SoftDeleteAsync(id);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<PaginatedList<StyleResponseDto>> GetAllAsync(int index, int pageSize, string? search, string? sortBy)
        {
            var styleList = await _unitOfWork.StyleRepository.GetAllAsync(index, pageSize, search, sortBy);

            // Map từng phần tử trong danh sách Items
            var responseList = styleList.Items.Select(item => _mapper.Map<StyleResponseDto>(item)).ToList();

            // Tạo PaginatedList mới với các đối tượng đã map
            var paginatedResponse = new PaginatedList<StyleResponseDto>(
                responseList,
                styleList.TotalCount,
                styleList.PageNumber,
                styleList.PageSize
            );

            return paginatedResponse;
        }

        public async Task<PaginatedList<StyleResponseDto>> GetAllByCategoryAsync(string categoryId, int index, int pageSize, string? search, string? sortBy)
        {

            var categpory = await _unitOfWork.CategoryRepository.GetByIdAsync(categoryId);
            if (categpory == null)
                throw new ErrorException(StatusCodes.Status404NotFound, ApiCodes.NOT_FOUND, "Category is not available");

            var styleList = await _unitOfWork.StyleRepository.GetAllByCategoryAsync(categoryId,index, pageSize, search, sortBy);
            if (styleList == null)
                throw new ErrorException(StatusCodes.Status404NotFound, ApiCodes.NOT_FOUND, "No record found");

            // Map từng phần tử trong danh sách Items
            var responseList = styleList.Items.Select(item => _mapper.Map<StyleResponseDto>(item)).ToList();

            // Tạo PaginatedList mới với các đối tượng đã map
            var paginatedResponse = new PaginatedList<StyleResponseDto>(
                responseList,
                styleList.TotalCount,
                styleList.PageNumber,
                styleList.PageSize
            );

            return paginatedResponse;
        }

        public async Task<StyleResponseDto> GetByIdAsync(string id)
        {
            var oldStyle = await _unitOfWork.StyleRepository.GetByIdAsync(id);
            if (oldStyle == null)
                throw new ErrorException(StatusCodes.Status404NotFound, ApiCodes.NOT_FOUND, "Style is not available");

            return _mapper.Map<StyleResponseDto>(oldStyle);
        }

        public async Task UpdateAsync(string id, StyleRequestDto requestDto)
        {
            var oldStyle = await _unitOfWork.StyleRepository.GetByIdAsync(id);
            if (oldStyle == null)
            {
                throw new ErrorException(StatusCodes.Status404NotFound, ApiCodes.NOT_FOUND, "Style is not available");
            }

            _mapper.Map(requestDto, oldStyle); // Map request 
            oldStyle.UpdatedAt = DateTime.UtcNow;
            oldStyle.UpdatedBy = GetCurrentUserName();
            await _unitOfWork.StyleRepository.UpdateAsync(oldStyle);
            await _unitOfWork.SaveChangesAsync();
        }

        private string GetCurrentUserName()
        {
            return _contextAccessor.HttpContext?.User.FindFirst("name")?.Value ?? "System";
        }
    }
}
