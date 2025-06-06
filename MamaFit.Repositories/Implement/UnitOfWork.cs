using MamaFit.Repositories.Interface;
using MamaFit.BusinessObjects.DBContext;
using MamaFit.BusinessObjects.Base;
using Microsoft.AspNetCore.Http;

namespace MamaFit.Repositories.Implement
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly Dictionary<Type, object> _repositories = new();
        private readonly IHttpContextAccessor _httpContextAccessor;
        private bool _disposed;

        public UnitOfWork(ApplicationDbContext dbContext, IHttpContextAccessor httpContextAccessor)
        {
            _dbContext = dbContext;
            _httpContextAccessor = httpContextAccessor;
        }

        public IGenericRepository<T> GetRepository<T>() where T : BaseEntity
        {
            if (_repositories.ContainsKey(typeof(T)))
            {
                return (IGenericRepository<T>)_repositories[typeof(T)];
            }

            var repositoryInstance = new GenericRepository<T>(_dbContext, _httpContextAccessor);
            _repositories.Add(typeof(T), repositoryInstance);
            return repositoryInstance;
        }
        public async Task SaveAsync()
        {
            await _dbContext.SaveChangesAsync();
        }
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _dbContext.Dispose();
                }
            }
            _disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void BeginTransaction()
        {
            _dbContext.Database.BeginTransaction();
        }

        public void CommitTransaction()
        {
            _dbContext.Database.CommitTransaction();
        }

        public void RollBack()
        {
            _dbContext.Database.RollbackTransaction();
        }
    }
}
