using AutoMapper;
using MamaFit.BusinessObjects.DTO.MilestoneDto;
using MamaFit.BusinessObjects.Entity;
using MamaFit.BusinessObjects.Enum;
using MamaFit.Repositories.Implement;
using MamaFit.Repositories.Infrastructure;
using MamaFit.Services.Interface;

namespace MamaFit.Services.Service
{
    public class MilestoneService : IMilestoneService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IValidationService _validationService;

        public MilestoneService(IMapper mapper, IValidationService validationService, IUnitOfWork unitOfWork)
        {

            _mapper = mapper;
            _validationService = validationService;
            _unitOfWork = unitOfWork;
        }

        public async Task CreateAsync(MilestoneRequestDto request)
        {
            await _validationService.ValidateAndThrowAsync(request);
            var milestone = _mapper.Map<Milestone>(request);
            await _unitOfWork.MilestoneRepository.InsertAsync(milestone);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteAsync(string id)
        {
            var milestone = await _unitOfWork.MilestoneRepository.GetByIdNotDeletedAsync(id);
            _validationService.CheckNotFound(milestone, $"Milestone with id:{id} is not found");

            await _unitOfWork.MilestoneRepository.SoftDeleteAsync(id);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<PaginatedList<MilestoneResponseDto>> GetAllAsync(int index, int pageSize, string? search, EntitySortBy? sortBy)
        {
            var milestoneList = await _unitOfWork.MilestoneRepository.GetAllAsync(index, pageSize, search, sortBy);

            var responseList = milestoneList.Items.Select(item => _mapper.Map<MilestoneResponseDto>(item)).ToList();

            var paginatedResponse = new PaginatedList<MilestoneResponseDto>(
                responseList,
                milestoneList.TotalCount,
                milestoneList.PageNumber,
                milestoneList.PageSize
            );
            return paginatedResponse;
        }

        public async Task<MilestoneGetByIdResponseDto> GetByIdAsync(string? id)
        {
            var milestone = await _unitOfWork.MilestoneRepository.GetByIdDetailAsync(id);
            _validationService.CheckNotFound(milestone, $"Milestone with id:{id} is not found");

            return _mapper.Map<MilestoneGetByIdResponseDto>(milestone);
        }

        public async Task UpdateAsync(string id, MilestoneRequestDto request)
        {
            var milestone = await _unitOfWork.MilestoneRepository.GetByIdNotDeletedAsync(id);
            _validationService.CheckNotFound(milestone, $"Milestone with id:{id} is not found");

            _mapper.Map(milestone, request);
            await _unitOfWork.MilestoneRepository.UpdateAsync(milestone!);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
