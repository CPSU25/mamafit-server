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

        public Task<PaginatedList<MaternityDressTaskResponseDto>> GetAllAsync(int index, int pageSize, string search, EntitySortBy? sortBy)
        {
            throw new NotImplementedException();
        }

        public Task<MaternityDressTaskResponseDto> GetById(string? id)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(string id, MaternityDressTaskRequestDto request)
        {
            throw new NotImplementedException();
        }
    }
}
