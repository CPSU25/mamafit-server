using MamaFit.BusinessObjects.Base;
using MamaFit.Repositories.Infrastructure;
using System.Linq.Expressions;

namespace MamaFit.Repositories.Interface
{
    public interface IGenericRepository<T> where T : BaseEntity
    {
        IEnumerable<T> Get(int index, int pageSize);
        //IQueryable<T> Entities { get; }
        Task<PaginatedList<T>> GetPaging(IQueryable<T> query, int index, int pageSize);
        T GetById(object id);
        void Insert(T obj);
        void InsertRange(List<T> obj);
        Task InsertCollection(ICollection<T> collection);
        void Update(T obj);
        void Delete(object id);
        Task<IEnumerable<T>> GetAsync(int index, int pageSize);
        Task<T> GetByIdAsync(object id);
        Task<T?> GetByIdNotDeletedAsync(object id);
        List<T> GetAll();
        Task<IEnumerable<T>> GetAllAsync();
        Task InsertAsync(T obj);
        Task InsertWithoutAuditAsync(T obj);
        Task UpdateAsync(T obj);
        Task DeleteAsync(object id);
        Task DeleteAsync(T entity);
        void DeleteRange(IEnumerable<T> entities);
        Task SoftDeleteAsync(object id);
        Task UpdateWithoutAuditAsync(T obj);
        Task<T> FindAsync(Expression<Func<T, bool>> predicate);
        Task<IQueryable<T>> FindAllAsync(Expression<Func<T, bool>> predicate);
        Task<IQueryable<T>> GetAllQueryableAsync();
        Task<List<T>> FindListAsync(Expression<Func<T, bool>> predicate);

    }
}
