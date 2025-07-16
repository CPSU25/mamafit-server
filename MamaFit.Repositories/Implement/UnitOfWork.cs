using MamaFit.BusinessObjects.DBContext;
using MamaFit.Repositories.Interface;

namespace MamaFit.Repositories.Implement
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        private bool _disposed;
        public IUserRepository UserRepository { get; }
        public IAddressRepository AddressRepository { get; } 
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
        public IMeasurementRepository MeasurementRepository { get; }
        public IMeasurementDiaryRepository MeasurementDiaryRepository { get; }
        public IChatRepository ChatRepository { get; }
        public IVoucherBatchRepository VoucherBatchRepository { get; }
        public IVoucherDiscountRepository VoucherDiscountRepository { get; }
        public IOrderRepository OrderRepository { get; }
        public IMaternityDressTaskRepository MaternityDressTaskRepository { get; }
        public IMilestoneRepository MilestoneRepository { get; }
        public INotificationRepository NotificationRepository { get; }
        public IWarrantyHistoryRepository WarrantyHistoryRepository { get; }
        public IWarrantyRequestRepository WarrantyRequestRepository { get; }
        public ITransactionRepository TransactionRepository { get; }
        public IFeedbackRepository FeedbackRepository { get; }
        public ICartItemRepository CartItemRepository { get; }
        public IPresetRepository PresetRepository { get; }
        public IAddOnRepository AddOnRepository { get; }

        public IOrderItemTaskRepository OrderItemTaskRepository { get; }
        public IAddOnOptionRepository AddOnOptionRepository { get; }

        public UnitOfWork(ApplicationDbContext context,
            IUserRepository userRepository,
            IAddressRepository addressRepository,
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
            IOrderItemRepository orderItemRepository,
            IMeasurementRepository measurementRepository,
            IMeasurementDiaryRepository measurementDiaryRepository,
            IChatRepository chatRepository,
            IVoucherBatchRepository voucherBatchRepository,
            IVoucherDiscountRepository voucherDiscountRepository,
            IOrderRepository orderRepository,
            IMaternityDressTaskRepository maternityDressTaskRepository,
            IMilestoneRepository milestoneRepository,
            INotificationRepository notificationRepository,
            IWarrantyHistoryRepository warrantyHistoryRepository,
            ITransactionRepository transactionRepository,
            IFeedbackRepository feedbackRepository,
            ICartItemRepository cartItemRepository,
            IPresetRepository presetRepository,
            IWarrantyRequestRepository warrantyRequestRepository,
            IAddOnRepository addOnRepository,
            IOrderItemTaskRepository orderItemTaskRepository,
            IAddOnOptionRepository addOnOptionRepository)
        {
            _context = context;
            UserRepository = userRepository;
            AddressRepository = addressRepository;
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
            MeasurementRepository = measurementRepository;
            MeasurementDiaryRepository = measurementDiaryRepository;
            ChatRepository = chatRepository;
            VoucherBatchRepository = voucherBatchRepository;
            VoucherDiscountRepository = voucherDiscountRepository;
            OrderRepository = orderRepository;
            MaternityDressTaskRepository = maternityDressTaskRepository;
            MilestoneRepository = milestoneRepository;
            NotificationRepository = notificationRepository;
            WarrantyHistoryRepository = warrantyHistoryRepository;
            TransactionRepository = transactionRepository;
            FeedbackRepository = feedbackRepository;
            CartItemRepository = cartItemRepository;
            PresetRepository = presetRepository;
            WarrantyRequestRepository = warrantyRequestRepository;
            AddOnRepository = addOnRepository;
            OrderItemTaskRepository = orderItemTaskRepository;
            AddOnOptionRepository = addOnOptionRepository;
        }

        public int SaveChanges()
        {
            int result;

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
            int result;

            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    result = await _context.SaveChangesAsync();
                    await transaction.CommitAsync();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error during SaveChangesAsync: {ex.Message}");
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