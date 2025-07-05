using AutoMapper;
using MamaFit.BusinessObjects.DTO.MaternityDressTask;
using MamaFit.BusinessObjects.Entity;
using MamaFit.BusinessObjects.Enum;
using MamaFit.Repositories.Implement;
using MamaFit.Repositories.Infrastructure;
using MamaFit.Services.Interface;

namespace MamaFit.Services.Service
{
    public class MaternityDressTaskService : IMaternityDressTaskService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IValidationService _validationService;

        public MaternityDressTaskService(IMapper mapper, IUnitOfWork unitOfWork, IValidationService validationService)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _validationService = validationService;
        }

        public async Task CreateAsync(MaternityDressTaskRequestDto request)
        {
            var milestone = await _unitOfWork.MilestoneRepository.GetByIdAsync(request.MilestoneId!);
            _validationService.CheckNotFound(milestone, $"Milestone with id:{request.MilestoneId} not found");

            var task = _mapper.Map<MaternityDressTask>(request);
            task.Milestone = milestone;
            await _unitOfWork.MaternityDressTaskRepository.InsertAsync(task);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteAsync(string id)
        {
            var task = await _unitOfWork.MaternityDressTaskRepository.GetByIdAsync(id);
            if (task == null)
                throw new Exception($"Not found task with id:{id}");

            await _unitOfWork.MaternityDressTaskRepository.DeleteAsync(task);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<PaginatedList<MaternityDressTaskResponseDto>> GetAllAsync(int index, int pageSize, string search, EntitySortBy? sortBy)
        {
            var taskList = await _unitOfWork.MaternityDressTaskRepository.GetAllAsync(index, pageSize, search, sortBy);

            var responseList = taskList.Items.Select(item => _mapper.Map<MaternityDressTaskResponseDto>(item)).ToList();

            var paginatedResponse = new PaginatedList<MaternityDressTaskResponseDto>(
                responseList,
                taskList.TotalCount,
                taskList.PageNumber,
                taskList.PageSize
            );

            return paginatedResponse;
        }

        public async Task<MaternityDressTaskResponseDto> GetById(string? id)
        {
            var task = await _unitOfWork.MaternityDressTaskRepository.GetByIdAsync(id);
            if (task == null)
                throw new Exception($"Not found task with id:{id}");

            return _mapper.Map<MaternityDressTaskResponseDto>(task);
        }

        public async Task UpdateAsync(string id, MaternityDressTaskRequestDto request)
        {
            var task = await _unitOfWork.MaternityDressTaskRepository.GetByIdAsync(id);

            if (task == null)
                throw new Exception($"Not found task with id:{id}");

            _mapper.Map(request, task);

            await _unitOfWork.MaternityDressTaskRepository.UpdateAsync(task);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
