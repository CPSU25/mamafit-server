using MamaFit.Repositories.Interface;
using MamaFit.BusinessObjects.DBContext;

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
        public IAppointmentRepository AppointmentRepository { get; }
        public ICategoryRepository CategoryRepository { get; }
        public IStyleRepository StyleRepository { get; }
        public IComponentRepository ComponentRepository { get; }
        public IComponentOptionRepository ComponentOptionRepository { get; }
        public IBranchRepository BranchRepository { get; }
        public IDesignRequestRepository DesignRequestRepository { get; }
        public IMaternityDressRepository MaternityDressRepository { get; }
        public IMaternityDressDetailRepository MaternityDressDetailRepository { get; }
        public IOrderItemRepository OrderItemRepository { get; }

        public UnitOfWork(ApplicationDbContext context,
            IUserRepository userRepository,
            IRoleRepository roleRepository,
            ITokenRepository tokenRepository,
            IOTPRepository otpRepository,
            IAppointmentRepository appointmentRepository,
            ICategoryRepository categoryRepository,
            IStyleRepository styleRepository,
            IComponentRepository componentRepository,
            IComponentOptionRepository componentOptionRepository,
            IBranchRepository branchRepository,
            IDesignRequestRepository designRequestRepository,
            IMaternityDressRepository maternityDressRepository,
            IMaternityDressDetailRepository maternityDressDetailRepository,
            IOrderItemRepository orderItemRepository)
        {
            _context = context;
            UserRepository = userRepository;
            RoleRepository = roleRepository;
            TokenRepository = tokenRepository;
            OTPRepository = otpRepository;
            AppointmentRepository = appointmentRepository;
            CategoryRepository = categoryRepository;
            StyleRepository = styleRepository;
            ComponentRepository = componentRepository;
            ComponentOptionRepository = componentOptionRepository;
            BranchRepository = branchRepository;
            DesignRequestRepository = designRequestRepository;
            MaternityDressDetailRepository = maternityDressDetailRepository;
            MaternityDressRepository = maternityDressRepository;
            OrderItemRepository = orderItemRepository;
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