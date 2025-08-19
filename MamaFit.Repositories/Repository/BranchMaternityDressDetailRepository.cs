using MamaFit.BusinessObjects.DBContext;
using MamaFit.BusinessObjects.Entity;
using MamaFit.Repositories.Infrastructure;
using MamaFit.Repositories.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace MamaFit.Repositories.Repository;

public class BranchMaternityDressDetailRepository : IBranchMaternityDressDetailRepository
{
    private readonly ApplicationDbContext _context;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly DbSet<BranchMaternityDressDetail> _dbSet;
    
    public BranchMaternityDressDetailRepository(ApplicationDbContext context, IHttpContextAccessor accessor)
    {
        _context = context;
        _httpContextAccessor = accessor;
        _dbSet = _context.Set<BranchMaternityDressDetail>();
    }

    public async Task<PaginatedList<BranchMaternityDressDetail>> GetAllAsync(int index, int pageSize, string userId, string? search)
    {
        var branchId = await _context.Branches.Where(
            b => b.BranchManagerId == userId)
            .Select(b => b.Id)
            .FirstOrDefaultAsync();
        
        var query = _dbSet
            .Where(b => b.BranchId == branchId)
            .Include(b => b.Branch)
            .Include(b => b.MaternityDressDetail)
            .OrderBy(b => b.MaternityDressDetail.CreatedAt)
            .AsQueryable();
        
        if (!string.IsNullOrEmpty(search))
        {
            query = query.Where(b => b.Branch.Name.Contains(search) || 
                                     b.MaternityDressDetail.Name.Contains(search));
        }
        
        return await query.GetPaginatedList(index, pageSize);
    }
    
    public async Task<BranchMaternityDressDetail?> GetDetailByIdAsync(string userId, string branchId, string dressDetailId)
    {
        if (string.IsNullOrEmpty(branchId) || string.IsNullOrEmpty(dressDetailId))
            return null;
        
        return await _dbSet
            .Include(b => b.Branch)
            .Include(b => b.MaternityDressDetail)
            .FirstOrDefaultAsync(b => b.BranchId == branchId && b.MaternityDressDetailId == dressDetailId);
    }
    
    public async Task InsertAsync(BranchMaternityDressDetail entity)
    {
        _dbSet.Add(entity);
        await _context.SaveChangesAsync();
    }
    
    public async Task UpdateAsync(BranchMaternityDressDetail entity)
    {
        _dbSet.Update(entity);
        await _context.SaveChangesAsync();
    }
    
    public async Task<BranchMaternityDressDetail?> GetByIdAsync(string branchId, string dressDetailId)
    {
        if (string.IsNullOrEmpty(branchId) || string.IsNullOrEmpty(dressDetailId))
            return null;

        return await _dbSet
            .Include(b => b.Branch)
            .Include(b => b.MaternityDressDetail)
            .FirstOrDefaultAsync(b => b.BranchId == branchId && b.MaternityDressDetailId == dressDetailId);
    }
    
    public async Task DeleteAsync(string branchId, string dressDetailId)
    {
        var entity = await GetByIdAsync(branchId, dressDetailId);
        if (entity != null)
        {
            _dbSet.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }
}