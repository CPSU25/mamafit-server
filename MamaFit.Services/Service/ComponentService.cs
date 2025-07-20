using AutoMapper;
using MamaFit.BusinessObjects.DTO.ComponentDto;
using MamaFit.BusinessObjects.Entity;
using MamaFit.BusinessObjects.Enum;
using MamaFit.Repositories.Implement;
using MamaFit.Repositories.Infrastructure;
using MamaFit.Services.ExternalService.Redis;
using MamaFit.Services.Interface;
using Microsoft.AspNetCore.Http;
namespace MamaFit.Services.Service
{
    public class ComponentService : IComponentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IMapper _mapper;
        private readonly IValidationService _validation;
        private readonly ICacheService _cache;

        public ComponentService(IUnitOfWork unitOfWork, IHttpContextAccessor contextAccessor, 
            IMapper mapper, IValidationService validation, ICacheService cache)
        {
            _unitOfWork = unitOfWork;
            _contextAccessor = contextAccessor;
            _mapper = mapper;
            _validation = validation;
            _cache = cache;
        }

        public async Task CreateAsync(ComponentRequestDto requestDto)
        {
            await _validation.ValidateAndThrowAsync(requestDto);

            var newComponent = _mapper.Map<Component>(requestDto);
            newComponent.GlobalStatus = GlobalStatus.INACTIVE;

            await _unitOfWork.ComponentRepository.InsertAsync(newComponent);
            await _unitOfWork.SaveChangesAsync();
            
            await _cache.IncreaseVersionAsync("components");
        }

        public async Task DeleteAsync(string id)
        {
            var component = await _unitOfWork.ComponentRepository.GetByIdAsync(id);

            if (component == null || component.IsDeleted)
                throw new ErrorException(StatusCodes.Status404NotFound, ApiCodes.NOT_FOUND, "Component not found!");

            if (component.Options.Any())
                throw new ErrorException(StatusCodes.Status400BadRequest, ApiCodes.BAD_REQUEST, "Cannot delete this component as policy restrict");

            await _unitOfWork.ComponentRepository.SoftDeleteAsync(id);
            await _unitOfWork.SaveChangesAsync();
            
            await _cache.IncreaseVersionAsync("components");
            await _cache.RemoveAsync($"component:{id}");
        }

        public async Task<PaginatedList<ComponentResponseDto>> GetAllAsync(int index, int pageSize, string? search, string? sortBy)
        {
            int version = await _cache.GetVersionAsync("components");
            string cacheKey = $"components:v{version}:{index}:{pageSize}:{search ?? ""}:{sortBy ?? ""}";
            
            var cached = await _cache.GetAsync<PaginatedList<ComponentResponseDto>>(cacheKey);
            if (cached != null)
                return cached;
            
            var componentList = await _unitOfWork.ComponentRepository.GetAllAsync(index, pageSize, search, sortBy);

            var responseList = componentList.Items.Select(item => _mapper.Map<ComponentResponseDto>(item)).ToList();

            var paginatedResponse = new PaginatedList<ComponentResponseDto>(
                responseList,
                componentList.TotalCount,
                componentList.PageNumber,
                componentList.PageSize
            );

            await _cache.SetAsync(cacheKey, paginatedResponse, TimeSpan.FromMinutes(15));
            return paginatedResponse;
        }

        public async Task<ComponentGetByIdResponseDto> GetByIdAsync(string id)
        {
            string cacheKey = $"component:{id}";
            var cached = await _cache.GetAsync<ComponentGetByIdResponseDto>(cacheKey);
            if (cached != null)
                return cached;
            
            var component = await _unitOfWork.ComponentRepository.GetById(id);

            if (component == null)
                throw new ErrorException(StatusCodes.Status404NotFound, ApiCodes.NOT_FOUND, "Component not found!");

            var result = _mapper.Map<ComponentGetByIdResponseDto>(component);
            await _cache.SetAsync(cacheKey, result, TimeSpan.FromMinutes(15));
            return result;
        }

        public async Task<List<ComponentGetByIdResponseDto>> GetComponentHavePresetByStyleId(string styleId)
        {
            string cacheKey = $"components:preset:{styleId}";
            var cached = await _cache.GetAsync<List<ComponentGetByIdResponseDto>>(cacheKey);
            if (cached != null)
                return cached;
            
            var style = await _unitOfWork.StyleRepository.GetByIdNotDeletedAsync(styleId);
            _validation.CheckNotFound(style, $"Style with ID {styleId} not found.");

            var components = await _unitOfWork.ComponentRepository.GetComponentHavePresetByStyleId(styleId);
            _validation.CheckNotFound(components, $"No components found for style with ID {styleId}.");
            
            var result = components.Select(c => _mapper.Map<ComponentGetByIdResponseDto>(c)).ToList();
            await _cache.SetAsync(cacheKey, result, TimeSpan.FromMinutes(15));
            return result;
        }

        public async Task UpdateAsync(string id, ComponentRequestDto requestDto)
        {
            await _validation.ValidateAndThrowAsync(requestDto);
            var component = await _unitOfWork.ComponentRepository.GetByIdAsync(id);

            if (component == null)
                throw new ErrorException(StatusCodes.Status404NotFound, ApiCodes.NOT_FOUND, "Component not found!");

            _mapper.Map(requestDto, component);
            component.UpdatedAt = DateTime.UtcNow;
            component.UpdatedBy = GetCurrentUserName();

            await _unitOfWork.ComponentRepository.UpdateAsync(component);
            await _unitOfWork.SaveChangesAsync();
            
            await _cache.IncreaseVersionAsync("components");
            await _cache.RemoveAsync($"component:{id}");
        }

        private string GetCurrentUserName()
        {
            return _contextAccessor.HttpContext?.User?.FindFirst("name")?.Value ?? "System";
        }
    }
}