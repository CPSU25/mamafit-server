using MamaFit.BusinessObjects.Entity;
using MamaFit.BusinessObjects.Enum;
using MamaFit.Repositories.Implement;
using MamaFit.Repositories.Infrastructure;

namespace MamaFit.Repositories.Interface
{
    public interface IPresetRepository : IGenericRepository<Preset>
    {
        public Task<PaginatedList<Preset>> GetAll(int index, int pageSize, string? search, EntitySortBy? sortBy);
        public Task<Preset> GetDetailById(string id);
        public Task<Preset> GetDefaultPresetByStyleId(string styleId);
        public Task<List<Preset>> GetAllPresetByComponentOptionId(List<string> componentOptionId);
        public Task<PaginatedList<Preset>> GetMostSelledPreset(int index, int pageSize, DateTime? startDate, DateTime? endDate, OrderStatus? filterBy);
        public Task<List<Preset>> GetPresetByDesignRequestId(string designRequestId);
    }
}
