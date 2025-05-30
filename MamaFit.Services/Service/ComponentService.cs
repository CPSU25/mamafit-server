using AutoMapper;
using MamaFit.BusinessObjects.DTO.ComponentDto;
using MamaFit.BusinessObjects.Entity;
using MamaFit.Repositories.Infrastructure;
using MamaFit.Repositories.Interface;
using MamaFit.Services.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

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
            var styleRepo = _unitOfWork.GetRepository<Style>();
            var style = await styleRepo.GetByIdAsync(requestDto.StyleId);
            if (style == null)
                throw new ErrorException(StatusCodes.Status404NotFound, "Style not found!");

            var componentRepo = _unitOfWork.GetRepository<Component>(); // Tạo repository 

            var newComponent = new Component
            {
                Name = requestDto.Name,
                Description = requestDto.Description,
                Images = requestDto.Images,
                StyleId = requestDto.StyleId,
                Option = requestDto.Options.Select(o => new ComponentOption
                {
                    Name = o.Name,
                    Description = o.Description,
                    Images = o.Images,
                    ComponentOptionType = o.ComponentOptionType,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    CreatedBy = GetCurrentUserName()
                }).ToList(),
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                CreatedBy = GetCurrentUserName()
            };

            await componentRepo.InsertAsync(newComponent);
            await componentRepo.SaveAsync();
        }

        public async Task DeleteAsync(string id)
        {
            var componentRepo = _unitOfWork.GetRepository<Component>();
            var component = await componentRepo.Entities
                .Include(c => c.Option)
                .FirstOrDefaultAsync(c => c.Id == id && !c.IsDeleted);

            if (component == null)
                throw new ErrorException(StatusCodes.Status404NotFound, "Component not found!");

            await componentRepo.SoftDeleteAsync(component);
            await componentRepo.SaveAsync();
        }

        public async Task<PaginatedList<ComponentResponseDto>> GetAllAsync(int index, int pageSize, string? search, string? sortBy)
        {
            var componentRepo = _unitOfWork.GetRepository<Component>();

            var query = componentRepo.Entities
                .Include(c => c.Option)
                .Include(c => c.Style)
                .Where(c => !c.IsDeleted);

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(c => c.Name.Contains(search));
            }

            query = sortBy?.ToLower() switch
            {
                "name_asc" => query.OrderBy(c => c.Name),
                "name_desc" => query.OrderByDescending(c => c.Name),
                "createdat_asc" => query.OrderBy(c => c.CreatedAt),
                "createdat_desc" => query.OrderByDescending(c => c.CreatedAt),
                _ => query.OrderByDescending(c => c.CreatedAt)
            };

            var pagedResult = await componentRepo.GetPagging(query, index, pageSize);

            var listComponent = pagedResult.Items
                .Select(_mapper.Map<ComponentResponseDto>)
                .ToList();

            return new PaginatedList<ComponentResponseDto>(
                listComponent,
                pagedResult.TotalCount,
                pagedResult.PageNumber,
                pageSize
            );
        }

        public async Task<ComponentResponseDto> GetByIdAsync(string id)
        {
            var componentRepo = _unitOfWork.GetRepository<Component>();
            var component = await componentRepo.Entities
                .Include(c => c.Option)
                .Include(c => c.Style)
                .Where(c => !c.IsDeleted)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (component == null)
                throw new ErrorException(StatusCodes.Status404NotFound, "Component not found!");

            return _mapper.Map<ComponentResponseDto>(component);
        }

        public async Task UpdateAsync(string id, ComponentRequestDto requestDto)
        {
            var componentRepo = _unitOfWork.GetRepository<Component>();
            var optionRepo = _unitOfWork.GetRepository<ComponentOption>();

            var component = await componentRepo.Entities
                .Include(c => c.Option)
                .FirstOrDefaultAsync(c => c.Id == id && !c.IsDeleted);

            if (component == null)
                throw new ErrorException(StatusCodes.Status404NotFound, "Component not found!");

            optionRepo.DeleteRange(component.Option); // Xóa option cũ

            _mapper.Map(requestDto, component);
            component.UpdatedAt = DateTime.UtcNow;
            component.UpdatedBy = GetCurrentUserName();
            component.Option = requestDto.Options.Select(o => _mapper.Map<ComponentOption>(o)).ToList();

            await componentRepo.UpdateAsync(component);
            await componentRepo.SaveAsync();
        }

        private string GetCurrentUserName()
        {
            return _contextAccessor.HttpContext?.User?.FindFirst("name")?.Value ?? "System";
        }
    }
}
