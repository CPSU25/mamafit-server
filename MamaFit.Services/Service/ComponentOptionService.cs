using AutoMapper;
using MamaFit.BusinessObjects.DTO.ComponentOptionDto;
using MamaFit.BusinessObjects.Entity;
using MamaFit.Repositories.Infrastructure;
using MamaFit.Repositories.Interface;
using MamaFit.Services.Interface;
using Microsoft.AspNetCore.Http;

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
            var component = await _unitOfWork.ComponentRepository.GetByIdAsync(requestDto.ComponentId);
            if (component == null || component.IsDeleted)
                throw new ErrorException(StatusCodes.Status404NotFound, ErrorCode.NotFound, "Component is not available");

            var newOption = _mapper.Map<ComponentOption>(requestDto);

            newOption.Component = component;
            newOption.CreatedAt = DateTime.UtcNow;
            newOption.CreatedBy = GetCurrentUserName();

            await _unitOfWork.ComponentOptionRepository.InsertAsync(newOption);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteAsync(string id)
        {
            var oldOption = await _unitOfWork.ComponentOptionRepository.GetByIdAsync(id);

            if (oldOption == null || oldOption.IsDeleted)
            {
                throw new ErrorException(StatusCodes.Status404NotFound, ErrorCode.NotFound, "Component Option not found");
            }

            await _unitOfWork.ComponentOptionRepository.SoftDeleteAsync(id);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<PaginatedList<ComponentOptionResponseDto>> GetAllAsync(int index, int pageSize, string? search, string? sortBy)
        {
            var optionList = await _unitOfWork.ComponentOptionRepository.GetAllAsync(index, pageSize, search, sortBy);

            // Map từng phần tử trong danh sách Items
            var responseList = optionList.Items.Select(item => _mapper.Map<ComponentOptionResponseDto>(item)).ToList();

            // Tạo PaginatedList mới với các đối tượng đã map
            var paginatedResponse = new PaginatedList<ComponentOptionResponseDto>(
                responseList,
                optionList.TotalCount,
                optionList.PageNumber,
                optionList.PageSize
            );

            return paginatedResponse;
        }

        public async Task<ComponentOptionResponseDto> GetByIdAsync(string id)
        {
            var option = await _unitOfWork.ComponentOptionRepository.GetByIdAsync(id);

            if (option == null || option.IsDeleted)
            {
                throw new ErrorException(StatusCodes.Status404NotFound, ErrorCode.NotFound, "Component Option not found");
            }

            return _mapper.Map<ComponentOptionResponseDto>(option);
        }

        public async Task UpdateAsync(string id, ComponentOptionRequestDto requestDto)
        {
            var oldOption = await _unitOfWork.ComponentOptionRepository.GetByIdAsync(id);

            if (oldOption == null)
            {
                throw new ErrorException(StatusCodes.Status404NotFound, ErrorCode.NotFound, "Component Option not found");
            }

            _mapper.Map(requestDto, oldOption);
            oldOption.UpdatedAt = DateTime.UtcNow;
            oldOption.UpdatedBy = GetCurrentUserName();

            await _unitOfWork.ComponentOptionRepository.UpdateAsync(oldOption);
            await _unitOfWork.SaveChangesAsync();
        }

        private string GetCurrentUserName()
        {
            return _contextAccessor.HttpContext?.User?.FindFirst("name")?.Value ?? "System";
        }
    }
}
