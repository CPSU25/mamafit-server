using MamaFit.Repositories.Interface;
using MamaFit.BusinessObjects.DBContext;
using MamaFit.BusinessObjects.Base;

namespace MamaFit.Repositories.Implement
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly Dictionary<Type, object> _repositories = new();
        private bool disposed = false;

        public UnitOfWork(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IGenericRepository<T> GetRepository<T>() where T : BaseEntity
        {
            if (_repositories.ContainsKey(typeof(T)))
            {
                return (IGenericRepository<T>)_repositories[typeof(T)];
            }

            var repositoryInstance = new GenericRepository<T>(_dbContext);
            _repositories.Add(typeof(T), repositoryInstance);
            return repositoryInstance;
        }
        public async Task SaveAsync()
        {
            await _dbContext.SaveChangesAsync();
        }
        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    _dbContext.Dispose();
                }
            }
            disposed = true;
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
