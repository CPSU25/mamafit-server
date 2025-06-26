using AutoMapper;
using MamaFit.BusinessObjects.DTO.StyleDto;
using MamaFit.BusinessObjects.Entity;
using MamaFit.BusinessObjects.Enum;
using MamaFit.Repositories.Implement;
using MamaFit.Repositories.Infrastructure;
using MamaFit.Services.Interface;
using Microsoft.AspNetCore.Http;

namespace MamaFit.Services.Service
{
    public class StyleService : IStyleService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IMapper _mapper;
        private readonly IValidationService _validationService;

        public StyleService(IUnitOfWork unitOfWork, IHttpContextAccessor contextAccessor, IMapper mapper, IValidationService validationService)
        {
            _unitOfWork = unitOfWork;
            _contextAccessor = contextAccessor;
            _mapper = mapper;
            _validationService = validationService;
        }

        public async Task AssignComponentToStyle(string styleId, List<string> componentIds)
        {
            var style = await _unitOfWork.StyleRepository.GetByIdAsync(styleId);
            _validationService.CheckNotFound(style, $"Style with ID {styleId} not found.");

            var componentList = new List<Component>();

            foreach (var componentId in componentIds)
            {
                var component = await _unitOfWork.ComponentRepository.GetByIdAsync(componentId);
                _validationService.CheckNotFound(component, $"Component with ID {componentId} not found.");

                componentList.Add(component);
                component.GlobalStatus = GlobalStatus.ACTIVE;
                await _unitOfWork.ComponentRepository.UpdateAsync(component);
            }

            style.Components = componentList;
            style.GlobalStatus = GlobalStatus.ACTIVE;
            await _unitOfWork.StyleRepository.UpdateAsync(style);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task CreateAsync(StyleRequestDto requestDto)
        {
            if (requestDto.CategoryId != null)
            {
                var category = await _unitOfWork.CategoryRepository.GetByIdAsync(requestDto.CategoryId); // Tìm category
                if (category == null)
                    throw new ErrorException(StatusCodes.Status404NotFound, ApiCodes.NOT_FOUND, "Category is not available");

                category.Status = GlobalStatus.ACTIVE;
                await _unitOfWork.CategoryRepository.UpdateAsync(category);

                var newStyle = _mapper.Map<Style>(requestDto); // Map Style
                newStyle.Category = category;
                newStyle.GlobalStatus = GlobalStatus.INACTIVE;
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

            var styleList = await _unitOfWork.StyleRepository.GetAllByCategoryAsync(categoryId, index, pageSize, search, sortBy);
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

        public async Task<StyleGetByIdResponseDto> GetByIdAsync(string id)
        {
            var oldStyle = await _unitOfWork.StyleRepository.GetDetailById(id);
            if (oldStyle == null)
                throw new ErrorException(StatusCodes.Status404NotFound, ApiCodes.NOT_FOUND, "Style is not available");

            return _mapper.Map<StyleGetByIdResponseDto>(oldStyle);
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
