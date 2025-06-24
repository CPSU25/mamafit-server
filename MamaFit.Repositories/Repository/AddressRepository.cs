﻿using MamaFit.BusinessObjects.DbContext;
using MamaFit.BusinessObjects.Entity;
using MamaFit.Repositories.Implement;
using MamaFit.Repositories.Infrastructure;
using MamaFit.Repositories.Interface;
using Microsoft.AspNetCore.Http;

namespace MamaFit.Repositories.Repository
{
    public class AddressRepository : GenericRepository<Address>, IAddressRepository
    {
        public AddressRepository(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor) : base(context, httpContextAccessor)
        {
        }
        
        public async Task<PaginatedList<Address>> GetAllAsync(int index, int pageSize)
        {
            var query = _dbSet.Where(x => !x.IsDeleted);
            return await query.GetPaginatedList(index, pageSize);
        }
    }
}
