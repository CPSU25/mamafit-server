using MamaFit.BusinessObjects.Entity;
using MamaFit.Repositories.Implement;
using MamaFit.Repositories.Infrastructure;

namespace MamaFit.Repositories.Interface
{
    public interface IAddressRepository : IGenericRepository<Address>
    {
        Task<PaginatedList<Address>> GetAllAsync(int index, int pageSize);
        Task<List<Address>> GetByUserId(string userId);
    }
}
