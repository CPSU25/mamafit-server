using MamaFit.BusinessObjects.Entity;
using MamaFit.BusinessObjects.Enum;
using MamaFit.Repositories.Implement;
using MamaFit.Repositories.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MamaFit.Repositories.Interface
{
    public interface IMaternityDressCustomizationRepository : IGenericRepository<MaternityDressCustomization>
    {
        Task<PaginatedList<MaternityDressCustomization>> GetAll(int index, int pageSize, string? search, EntitySortBy? sortBy);
        Task<MaternityDressCustomization?> GetDetailById(string id);
    }
}
