using AutoMapper;
using MamaFit.BusinessObjects.DTO.ComponentOptionDto;
using MamaFit.BusinessObjects.Entity;
using MamaFit.Repositories.Infrastructure;
using MamaFit.Repositories.Interface;
using MamaFit.Services.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace MamaFit.Services.Service
{
    public class ComponentOptionService : IComponentOptionService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IMapper _mapper;

        public ComponentOptionService(IUnitOfWork unitOfWork, IHttpContextAccessor contextAccessor, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _contextAccessor = contextAccessor;
            _mapper = mapper;
        }

        public async Task CreateAsync(ComponentOptionRequestDto requestDto)
        {
            var componentRepo = _unitOfWork.GetRepository<Component>();
            var component = await componentRepo.GetByIdAsync(requestDto.ComponentId);
            if (component == null)
            {
                throw new ErrorException(StatusCodes.Status404NotFound, ErrorCode.NotFound, "Component is not available");
            }

            var componentOptionRepo = _unitOfWork.GetRepository<ComponentOption>();
            var newOption = _mapper.Map<ComponentOption>(requestDto);

            newOption.Component = component;
            newOption.CreatedAt = DateTime.UtcNow;
            newOption.CreatedBy = GetCurrentUserName();

            await componentOptionRepo.InsertAsync(newOption);
            await componentOptionRepo.SaveAsync();
        }

        public async Task DeleteAsync(string id)
        {
            var componentOptionRepo = _unitOfWork.GetRepository<ComponentOption>();
            var oldOption = await componentOptionRepo.GetByIdAsync(id);

            if (oldOption == null)
            {
                throw new ErrorException(StatusCodes.Status404NotFound, ErrorCode.NotFound, "Component Option not found");
            }

            await componentOptionRepo.SoftDeleteAsync(id);
            await componentOptionRepo.SaveAsync();
        }

        public async Task<PaginatedList<ComponentOptionResponseDto>> GetAllAsync(int index, int pageSize, string? search, string? sortBy)
        {
            var componentOptionRepo = _unitOfWork.GetRepository<ComponentOption>();
            var query = componentOptionRepo.Entities
                .Include(o => o.Component)
                .Where(o => !o.IsDeleted);

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(o => o.Name.Contains(search));
            }

            query = sortBy?.ToLower() switch
            {
                "name_asc" => query.OrderBy(o => o.Name),
                "name_desc" => query.OrderByDescending(o => o.Name),
                "createdat_asc" => query.OrderBy(o => o.CreatedAt),
                "createdat_desc" => query.OrderByDescending(o => o.CreatedAt),
                _ => query.OrderByDescending(o => o.CreatedAt),
            };

            var pagedResult = await componentOptionRepo.GetPagging(query, index, pageSize);

            var list = pagedResult.Items
                .Select(_mapper.Map<ComponentOptionResponseDto>)
                .ToList();

            return new PaginatedList<ComponentOptionResponseDto>(
                list,
                pagedResult.TotalCount,
                pagedResult.PageNumber,
                pageSize
            );
        }

        public async Task<ComponentOptionResponseDto> GetByIdAsync(string id)
        {
            var componentOptionRepo = _unitOfWork.GetRepository<ComponentOption>();
            var option = await componentOptionRepo.GetByIdAsync(id);

            if (option == null)
            {
                throw new ErrorException(StatusCodes.Status404NotFound, ErrorCode.NotFound, "Component Option not found");
            }

            return _mapper.Map<ComponentOptionResponseDto>(option);
        }

        public async Task UpdateAsync(string id, ComponentOptionRequestDto requestDto)
        {
            var componentOptionRepo = _unitOfWork.GetRepository<ComponentOption>();
            var oldOption = await componentOptionRepo.GetByIdAsync(id);

            if (oldOption == null)
            {
                throw new ErrorException(StatusCodes.Status404NotFound, ErrorCode.NotFound, "Component Option not found");
            }

            _mapper.Map(requestDto, oldOption);
            oldOption.UpdatedAt = DateTime.UtcNow;
            oldOption.UpdatedBy = GetCurrentUserName();

            await componentOptionRepo.UpdateAsync(oldOption);
            await componentOptionRepo.SaveAsync();
        }

        private string GetCurrentUserName()
        {
            return _contextAccessor.HttpContext?.User?.FindFirst("name")?.Value ?? "System";
        }
    }
}
