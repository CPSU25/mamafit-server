using AutoMapper;
using MamaFit.BusinessObjects.DTO.MaternityDressTaskDto;
using MamaFit.BusinessObjects.DTO.MilestoneDto;
using MamaFit.BusinessObjects.Entity;
using MamaFit.BusinessObjects.Enum;
using MamaFit.Repositories.Implement;
using MamaFit.Repositories.Infrastructure;
using MamaFit.Services.ExternalService.Redis;
using MamaFit.Services.Interface;

namespace MamaFit.Services.Service
{
    public class MilestoneService : IMilestoneService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IValidationService _validationService;
        private readonly ICacheService _cacheService;

        public MilestoneService(IMapper mapper, IValidationService validationService, IUnitOfWork unitOfWork,
            ICacheService cacheService)
        {
            _mapper = mapper;
            _validationService = validationService;
            _unitOfWork = unitOfWork;
            _cacheService = cacheService;
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

        public async Task<PaginatedList<MilestoneResponseDto>> GetAllAsync(int index, int pageSize, string? search,
            EntitySortBy? sortBy)
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

        public async Task<List<MilestoneAchiveOrderItemResponseDto>> GetMilestoneByOrderItemId(string orderItemId)
        {
            var cacheKey = $"MilestoneAchiveOrderItemResponseDto_{orderItemId}";
            var cachedData = await _cacheService.GetAsync<List<MilestoneAchiveOrderItemResponseDto>>(cacheKey);
            if (cachedData != null)
            {
                return cachedData;
            }

            var milestoneList = await _unitOfWork.MilestoneRepository.GetByOrderItemId(orderItemId);
            _validationService.CheckNotFound(milestoneList, $"Milestone with order item id:{orderItemId} is not found");

            var milestoneAchiveList = new List<MilestoneAchiveOrderItemResponseDto>();

            foreach (var milestone in milestoneList)
            {
                var totalTaskCount = milestone.MaternityDressTasks.Count;
                var doneTaskCount = milestone.MaternityDressTasks
                    .Count(x => x.OrderItemTasks.Any(t =>
                        t.OrderItemId == orderItemId && t.Status == OrderItemTaskStatus.DONE));

                float progress = totalTaskCount == 0 ? 0 : (float)doneTaskCount / totalTaskCount * 100;

                var currentTask = milestone.MaternityDressTasks
                    .Where(m => m.OrderItemTasks.Any(o =>
                        o.OrderItemId == orderItemId &&
                        (o.Status == OrderItemTaskStatus.IN_PROGRESS || o.Status == OrderItemTaskStatus.PENDING)))
                    .OrderBy(m => m.SequenceOrder)
                    .FirstOrDefault();

                var achiveOrderItem = new MilestoneAchiveOrderItemResponseDto
                {
                    MilestoneId = milestone.Id,
                    MilestoneName = milestone.Name,
                    Progress = progress,
                    IsDone = progress >= 100,
                    CurrentTask = currentTask != null
                        ? new MaternityDressTaskForMilestoneAchiveResponseDto
                        {
                            Id = currentTask.Id,
                            Name = currentTask.Name,
                        }
                        : null
                };

                milestoneAchiveList.Add(achiveOrderItem);
            }

            await _cacheService.SetAsync(cacheKey, milestoneAchiveList);
            return milestoneAchiveList;
        }

        public async Task UpdateAsync(string id, MilestoneRequestDto request)
        {
            var milestone = await _unitOfWork.MilestoneRepository.GetByIdNotDeletedAsync(id);
            _validationService.CheckNotFound(milestone, $"Milestone with id:{id} is not found");

            _mapper.Map(request, milestone);
            await _unitOfWork.MilestoneRepository.UpdateAsync(milestone!);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}