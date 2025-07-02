using AutoMapper;
using MamaFit.BusinessObjects.DTO.WarrantyRequestDto;
using MamaFit.BusinessObjects.Enum;
using MamaFit.Repositories.Implement;
using MamaFit.Repositories.Infrastructure;
using MamaFit.Services.Interface;
using Microsoft.AspNetCore.Http;

namespace MamaFit.Services.Service
{
    public class WarrantyRequestService : IWarrantyRequestService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;
        private readonly IValidationService _validationService;

        public WarrantyRequestService(
            IUnitOfWork unitOfWork,
            IHttpContextAccessor httpContextAccessor,
            IMapper mapper,
            IValidationService validationService)
        {
            _unitOfWork = unitOfWork;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
            _validationService = validationService;
        }

        public Task CreateAsync(WarrantyRequestCreateDto warrantyRequestCreateDto)
        {
            throw new NotImplementedException();
        }

        public async Task DeleteAsync(string id)
        {
            var warrantyRequest = await _unitOfWork.WarrantyRequestRepository.GetByIdAsync(id);
            _validationService.CheckNotFound(warrantyRequest, $"Warranty request with id:{id} not found");

            await _unitOfWork.WarrantyRequestRepository.SoftDeleteAsync(warrantyRequest);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<PaginatedList<WarrantyRequestGetAllDto>> GetAllWarrantyRequestAsync(int index, int pageSize, string? search, EntitySortBy? sortBy)
        {
            var warrantyRequests = await _unitOfWork.WarrantyRequestRepository.GetAllWarrantyRequestAsync(index, pageSize, search, sortBy);

            var result = _mapper.Map<List<WarrantyRequestGetAllDto>>(warrantyRequests.Items);
            return new PaginatedList<WarrantyRequestGetAllDto>(
                result,
                warrantyRequests.TotalCount,
                warrantyRequests.PageNumber,
                pageSize);
        }

        public async Task<WarrantyRequestGetByIdDto> GetWarrantyRequestByIdAsync(string id)
        {
            var result = await _unitOfWork.WarrantyRequestRepository.GetWarrantyRequestByIdAsync(id);
            _validationService.CheckNotFound(result, $"Warranty request with id:{id} not found");

            return _mapper.Map<WarrantyRequestGetByIdDto>(result);
        }

        public async Task UpdateAsync(string id, WarrantyRequestUpdateDto warrantyRequestUpdateDto)
        {
            var warrantyRequest = await _unitOfWork.WarrantyRequestRepository.GetByIdAsync(id);
            _validationService.CheckNotFound(warrantyRequest, $"Warranty request with id:{id} not found");

            _mapper.Map(warrantyRequestUpdateDto, warrantyRequest);
            await _unitOfWork.WarrantyRequestRepository.UpdateAsync(warrantyRequest);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
