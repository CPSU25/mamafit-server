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
    }
}
