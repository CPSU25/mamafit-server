using MamaFit.Repositories.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MamaFit.Repositories.Interface
{
    public interface IGenericRepository<T> where T : class
    {
        IEnumerable<T> Get(int index, int pageSize);
        IQueryable<T> Entities { get; }
        Task<PaginatedList<T>> GetPagging(IQueryable<T> query, int index, int pageSize);
        T GetById(object id);
        void Insert(T obj);
        void InsertRange(List<T> obj);
        Task InsertCollection(ICollection<T> collection);

        void Update(T obj);
        void Delete(object id);
        void Save();
        Task<IEnumerable<T>> GetAsync(int index, int pageSize);
        Task<T> GetByIdAsync(object id);
        List<T> GetAll();
        Task<IEnumerable<T>> GetAllAsync();

        Task InsertAsync(T obj);
        Task UpdateAsync(T obj);
        Task DeleteAsync(object id);
        Task SaveAsync();
        Task<T> FindAsync(Expression<Func<T, bool>> predicate);
        Task<IQueryable<T>> FindAllAsync(Expression<Func<T, bool>> predicate);
        Task<IQueryable<T>> GetAllQueryableAsync();
        Task<List<T>> FindListAsync(Expression<Func<T, bool>> predicate);

    }
}
