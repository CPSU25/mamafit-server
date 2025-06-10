using AutoMapper;
using MamaFit.BusinessObjects.DTO.ComponentDto;
using MamaFit.BusinessObjects.Entity;
using MamaFit.Repositories.Infrastructure;
using MamaFit.Repositories.Interface;
using MamaFit.Services.Interface;
using Microsoft.AspNetCore.Http;

namespace MamaFit.Services.Service
{
    public class ComponentService : IComponentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IMapper _mapper;

        public ComponentService(IUnitOfWork unitOfWork, IHttpContextAccessor contextAccessor, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _contextAccessor = contextAccessor;
            _mapper = mapper;
        }

        public async Task CreateAsync(ComponentRequestDto requestDto)
        {
            var style = await _unitOfWork.StyleRepository.GetByIdAsync(requestDto.StyleId);
            if (style == null)
                throw new ErrorException(StatusCodes.Status404NotFound, "Style not found!");

            var newComponent = new Component
            {
                Name = requestDto.Name,
                Description = requestDto.Description,
                Images = requestDto.Images,
                StyleId = requestDto.StyleId,
                Style = style,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                CreatedBy = GetCurrentUserName()
            };

            await _unitOfWork.ComponentRepository.InsertAsync(newComponent);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteAsync(string id)
        {
            var component = await _unitOfWork.ComponentRepository.GetByIdAsync(id);

            if (component == null)
                throw new ErrorException(StatusCodes.Status404NotFound, "Component not found!");

            await _unitOfWork.ComponentRepository.SoftDeleteAsync(id);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<PaginatedList<ComponentResponseDto>> GetAllAsync(int index, int pageSize, string? search, string? sortBy)
        {
            var componentList = await _unitOfWork.ComponentRepository.GetAllAsync(index, pageSize, search, sortBy);

            // Map từng phần tử trong danh sách Items
            var responseList = componentList.Items.Select(item => _mapper.Map<ComponentResponseDto>(item)).ToList();

            // Tạo PaginatedList mới với các đối tượng đã map
            var paginatedResponse = new PaginatedList<ComponentResponseDto>(
                responseList,
                componentList.TotalCount,
                componentList.PageNumber,
                componentList.PageSize
            );

            return paginatedResponse;
        }

        public async Task<ComponentResponseDto> GetByIdAsync(string id)
        {
            var component = _unitOfWork.ComponentRepository.GetById(id);

            if (component == null)
                throw new ErrorException(StatusCodes.Status404NotFound, "Component not found!");

            return _mapper.Map<ComponentResponseDto>(component);
        }

        public async Task UpdateAsync(string id, ComponentRequestDto requestDto)
        {

            var component = await _unitOfWork.ComponentRepository.GetByIdAsync(id);

            if (component == null)
                throw new ErrorException(StatusCodes.Status404NotFound, "Component not found!");

            _mapper.Map(requestDto, component);
            component.UpdatedAt = DateTime.UtcNow;
            component.UpdatedBy = GetCurrentUserName();

            await _unitOfWork.ComponentRepository.UpdateAsync(component);
            await _unitOfWork.SaveChangesAsync();
        }

        private string GetCurrentUserName()
        {
            return _contextAccessor.HttpContext?.User?.FindFirst("name")?.Value ?? "System";
        }
    }
}
