using MamaFit.Repositories.Infrastructure;
using MamaFit.Repositories.Interface;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using MamaFit.BusinessObjects.DBContext;
using MamaFit.BusinessObjects.Base;
using Microsoft.AspNetCore.Http;

namespace MamaFit.Repositories.Implement
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        protected readonly ApplicationDbContext _context;
        protected readonly DbSet<T> _dbSet;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public GenericRepository(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _dbSet = _context.Set<T>();
            _httpContextAccessor = httpContextAccessor;
        }
        
        private string GetCurrentUserName()
        {
            return _httpContextAccessor.HttpContext?.User?.FindFirst("name")?.Value ?? "System";
        }
        
        //public IQueryable<T> Entities => _context.Set<T>();

        public void Delete(object id)
        {
            T entity = _dbSet.Find(id);
            if (entity != null)
            {
                _dbSet.Remove(entity);
            }
        }

        public async Task DeleteAsync(object id)
        {
            T entity = await _dbSet.FindAsync(id) ?? throw new Exception();
            if (entity != null)
            {
                _dbSet.Remove(entity);
            }
        }
        
        public Task DeleteAsync(T entity)
        {
            _dbSet.Remove(entity);
            return Task.CompletedTask;
        }

        public async Task SoftDeleteAsync(object id)
        {
            var entity = await _dbSet.FindAsync(id) ?? throw new Exception("Entity not found");

            entity.IsDeleted = true;
            entity.UpdatedAt = DateTime.UtcNow;
            entity.UpdatedBy = GetCurrentUserName();
            
            await UpdateAsync(entity);
        }

        public void DeleteRange(IEnumerable<T> entities)
        {
            _dbSet.RemoveRange(entities);
        }

        public async Task<IQueryable<T>> FindAllAsync(Expression<Func<T, bool>> predicate)
        {
            return await Task.FromResult(_dbSet.Where(predicate));
        }

        public async Task<T> FindAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbSet.FirstOrDefaultAsync(predicate);
        }

        public async Task<List<T>> FindListAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbSet.Where(predicate).ToListAsync();
        }

        public IEnumerable<T> Get(int index, int pageSize)
        {
            return _dbSet.Skip(index * pageSize).Take(pageSize).ToList();
        }

        public List<T> GetAll()
        {
            return _context.Set<T>().ToList();
        }

        public async Task<IQueryable<T>> GetAllQueryableAsync()
        {
            return await Task.FromResult(_dbSet.AsQueryable());
        }

        public async Task<IEnumerable<T>> GetAsync(int index, int pageSize)
        {
            return await _dbSet.Skip(index * pageSize).Take(pageSize).ToListAsync();
        }

        public T GetById(object id)
        {
            return _dbSet.Find(id);
        }

        public async Task<T> GetByIdAsync(object id)
        {
            return await _dbSet.FindAsync(id);
        }
        
        public async Task<T> GetByIdNotDeletedAsync(object id)
        {
            var entity = await _dbSet.FindAsync(id);
            return (entity != null && !entity.IsDeleted) ? entity : null;
        }

        public async Task<PaginatedList<T>> GetPaging(IQueryable<T> query, int index, int pageSize)
        {
            return await query.GetPaginatedList(index, pageSize);
        }

        public void Insert(T obj)
        {
            _dbSet.Add(obj);
        }

        public async Task InsertAsync(T obj)
        {
            obj.CreatedAt = DateTime.UtcNow;
            obj.CreatedBy = GetCurrentUserName();
            await _dbSet.AddAsync(obj);
        }
        
        public async Task InsertWithoutAuditAsync(T obj)
        {
            await _dbSet.AddAsync(obj);
        }

        public void InsertRange(List<T> obj)
        {
            _dbSet.AddRange(obj);
        }
        public async Task InsertCollection(ICollection<T> collection)
        {
            await _dbSet.AddRangeAsync(collection);
        }
        
        public void Update(T obj)
        {
            _context.Entry(obj).State = EntityState.Modified;
        }

        public async Task UpdateWithoutAuditAsync(T obj)
        {
            _dbSet.Attach(obj);
            _context.Entry(obj).State = EntityState.Modified;
        }
        public async Task UpdateAsync(T obj)
        {
            obj.UpdatedAt = DateTime.UtcNow;
            obj.UpdatedBy = GetCurrentUserName();
            _dbSet.Attach(obj);
            _context.Entry(obj).State = EntityState.Modified;
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }
    }
}
