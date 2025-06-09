using MamaFit.Repositories.Interface;
using MamaFit.BusinessObjects.DBContext;
using MamaFit.BusinessObjects.Base;
using MamaFit.Repositories.Repository;
using Microsoft.AspNetCore.Http;

namespace MamaFit.Repositories.Implement
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        private bool _disposed = false;
        public IUserRepository UserRepository { get; }
        public IRoleRepository RoleRepository { get; }
        public ITokenRepository TokenRepository { get; }
        public IOTPRepository OTPRepository { get; }

        public UnitOfWork(ApplicationDbContext context,
            IUserRepository userRepository,
            IRoleRepository roleRepository,
            ITokenRepository tokenRepository,
            IOTPRepository otpRepository)
        {
            _context = context;
            UserRepository = userRepository;
            RoleRepository = roleRepository;
            TokenRepository = tokenRepository;
            OTPRepository = otpRepository;
        }

        public int SaveChanges()
        {
            int result = -1;

            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    result = _context.SaveChanges();
                    transaction.Commit();
                }
                catch (Exception)
                {
                    result = -1;
                    transaction.Rollback();
                }
            }

            return result;
        }
        
        public async Task<int> SaveChangesAsync()
        {
            int result = -1;

            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    result = await _context.SaveChangesAsync();
                    await transaction.CommitAsync();
                }
                catch (Exception)
                {
                    //Log exception handling message
                    result = -1;
                    await transaction.RollbackAsync();
                }
            }

            return result;
        }


        public void BeginTransaction()
        {
            _context.Database.BeginTransaction();
        }

        public void CommitTransaction()
        {
            _context.Database.CommitTransaction();
        }

        public void RollBack()
        {
            _context.Database.RollbackTransaction();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }

            _disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}