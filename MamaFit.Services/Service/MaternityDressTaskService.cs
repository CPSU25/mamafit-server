using AutoMapper;
using MamaFit.BusinessObjects.DTO.MaternityDressTask;
using MamaFit.BusinessObjects.Entity;
using MamaFit.BusinessObjects.Enum;
using MamaFit.Repositories.Infrastructure;
using MamaFit.Repositories.Interface;
using MamaFit.Services.Interface;

namespace MamaFit.Services.Service
{
    public class MaternityDressTaskService : IMaternityDressTaskService
    {
        private readonly IMaternityDressTaskRepository _maternityDressTaskRepository;
        private readonly IMapper _mapper;

        public MaternityDressTaskService(IMapper mapper, IMaternityDressTaskRepository maternityDressTaskRepository)
        {
            _maternityDressTaskRepository = maternityDressTaskRepository;
            _mapper = mapper;
        }

        public async Task CreateAsync(MaternityDressTaskRequestDto request)
        {
            var task = _mapper.Map<MaternityDressTask>(request);
            await _maternityDressTaskRepository.InsertAsync(task);
        }

        public async Task DeleteAsync(string id)
        {
            var task = await _maternityDressTaskRepository.GetByIdAsync(id);
            if (task == null)
                throw new Exception($"Not found task with id:{id}");

            await _maternityDressTaskRepository.DeleteAsync(task);
        }

        public async Task<PaginatedList<MaternityDressTaskResponseDto>> GetAllAsync(int index, int pageSize, string search, EntitySortBy? sortBy)
        {
            var taskList = await _maternityDressTaskRepository.GetAllAsync(index, pageSize, search, sortBy);

            var responseList = _mapper.Map<PaginatedList<MaternityDressTaskResponseDto>>(taskList.Items);

            return responseList;
        }

        public async Task<MaternityDressTaskResponseDto> GetById(string? id)
        {
            var task = await _maternityDressTaskRepository.GetByIdAsync(id);
            if (task == null)
                throw new Exception($"Not found task with id:{id}");

            return _mapper.Map<MaternityDressTaskResponseDto>(task);
        }

        public async Task UpdateAsync(string id, MaternityDressTaskRequestDto request)
        {
            var task = await _maternityDressTaskRepository.GetByIdAsync(id);

            if (task == null)
                throw new Exception($"Not found task with id:{id}");

            _mapper.Map(request, task);

            await _maternityDressTaskRepository.UpdateAsync(task);
        }
    }
}
