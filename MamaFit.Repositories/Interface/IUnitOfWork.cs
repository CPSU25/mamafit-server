using MamaFit.BusinessObjects.Base;

namespace MamaFit.Repositories.Interface
{
    public interface IUnitOfWork
    {
        public IUserRepository UserRepository { get; }
        public IRoleRepository RoleRepository { get; }
        public ITokenRepository TokenRepository { get; }
        public IOTPRepository OTPRepository { get; }
        int SaveChanges();
        Task<int> SaveChangesAsync();
        void BeginTransaction();
        void CommitTransaction();
        void RollBack();
    }
}
