using AutoMapper;
using MamaFit.BusinessObjects.DTO.MaternityDressCustomizationDto;
using MamaFit.BusinessObjects.Entity;
using MamaFit.BusinessObjects.Enum;
using MamaFit.Repositories.Implement;
using MamaFit.Repositories.Infrastructure;
using MamaFit.Services.Interface;
using Microsoft.AspNetCore.Http;

namespace MamaFit.Services.Service
{
    public class MaternityDressCustomizationService : IMaternityDressCustomizationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IMapper _mapper;
        private readonly IValidationService _validationService;

        public MaternityDressCustomizationService(IUnitOfWork unitOfWork, IHttpContextAccessor contextAccessor, IMapper mapper, IValidationService validationService)
        {
            _unitOfWork = unitOfWork;
            _contextAccessor = contextAccessor;
            _mapper = mapper;
            _validationService = validationService;
        }

        private string GetCurrentUserId()
        {
            return _contextAccessor.HttpContext?.User?.FindFirst("userId")?.Value ?? string.Empty;
        }

        public async Task CreateCustom(CustomCreateRequestDto request)
        {
            var user = await _unitOfWork.UserRepository.GetByIdAsync(GetCurrentUserId());
            _validationService.CheckNotFound(user, $"User with id:{GetCurrentUserId} not found");

            var componentOptions = new List<ComponentOption>();

            foreach (var option in componentOptions)
            {
                var item = _unitOfWork.ComponentOptionRepository.GetByIdNotDeletedAsync(option.ComponentId!);
                _validationService.CheckNotFound(item, $"Component option with id:{option.ComponentId} not found");

                componentOptions.Add(_mapper.Map<ComponentOption>(item));
            }

            var custom = _mapper.Map<MaternityDressCustomization>(request);
            custom.UserId = GetCurrentUserId();
            custom.MaternityDressSelections = componentOptions.Select(option => new MaternityDressSelection
            {
                ComponentOptionId = option.Id,
                ComponentOption = option,
                Description = option.Description,
                CreatedBy = user!.UserName,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
            }).ToList();

            await _unitOfWork.MaternityDressCustomizationRepository.InsertAsync(custom);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteCustom(string id)
        {
            var custom = await _unitOfWork.MaternityDressCustomizationRepository.GetByIdNotDeletedAsync(id);
            _validationService.CheckNotFound(custom, $"Customization with id:{id} not found");

            await _unitOfWork.MaternityDressCustomizationRepository.SoftDeleteAsync(id);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<PaginatedList<CustomResponseDto>> GetAll(int index, int pageSize, string? search, EntitySortBy? sortBy)
        {
            var customlist = await _unitOfWork.MaternityDressCustomizationRepository.GetAll(index, pageSize, search, sortBy);

            var responseList = customlist.Items.Select(item => _mapper.Map<CustomResponseDto>(item)).ToList();

            var paginatedResponse = new PaginatedList<CustomResponseDto>(
                responseList,
                customlist.TotalCount,
                customlist.PageNumber,
                customlist.PageSize
            );

            return paginatedResponse;
        }

        public async Task<CustomResponseDto> GetById(string id)
        {
            var custom = await _unitOfWork.MaternityDressCustomizationRepository.GetDetailById(id);
            _validationService.CheckNotFound(custom, $"Customization with id:{id} not found");

            return _mapper.Map<CustomResponseDto>(custom);
        }

        public async Task UpdateCustom(string id, CustomUpdateRequestDto request)
        {
            var custom = await _unitOfWork.MaternityDressCustomizationRepository.GetDetailById(id);
            _validationService.CheckNotFound(custom, $"Customization with id:{id} not found");

            _mapper.Map(custom, request);
            await _unitOfWork.MaternityDressCustomizationRepository.UpdateAsync(custom!);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
