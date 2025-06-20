﻿using MamaFit.Repositories.Interface;

namespace MamaFit.Repositories.Implement
{
    public interface IUnitOfWork
    {
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
        public IMeasurementRepository MeasurementRepository { get; }
        public IMeasurementDiaryRepository MeasurementDiaryRepository { get; }
        public IChatRepository ChatRepository { get; }
        public IVoucherBatchRepository VoucherBatchRepository { get; }
        public IVoucherDiscountRepository VoucherDiscountRepository { get; }
        public IOrderRepository OrderRepository { get; }
        public IMaternityDressTaskRepository MaternityDressTaskRepository { get; }
        public IMilestoneRepository MilestoneRepository { get; }
        int SaveChanges();
        Task<int> SaveChangesAsync();
        void BeginTransaction();
        void CommitTransaction();
        void RollBack();
    }
}
