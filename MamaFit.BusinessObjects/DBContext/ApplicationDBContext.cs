using MamaFit.BusinessObjects.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace MamaFit.BusinessObjects.DBContext
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext()
        {
        }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        
        public DbSet<OTP> OTPs { get; set; }
        public DbSet<ApplicationUser> Users { get; set; }
        public DbSet<ApplicationUserToken> UserTokens { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<Branch> Branches { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Component> Components { get; set; }
        public DbSet<ComponentOption> ComponentOptions { get; set; }
        public DbSet<DesignRequest> DesignRequests { get; set; }
        public DbSet<MaternityDressCustomization> DressCustomizations { get; set; }
        public DbSet<Feedback> Feedbacks { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<MaternityDress> MaternityDresses { get; set; }
        public DbSet<MaternityDressDetail> MaternityDressDetails { get; set; }
        public DbSet<BranchMaternityDressDetail> BranchMaternityDressDetails { get; set; }
        public DbSet<MaternityDressInspection> MaternityDressInspections { get; set; }
        public DbSet<Measurement> Measurements { get; set; }
        public DbSet<MeasurementDiary> MeasurementDiaries { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<OrderItemInspection> OrderItemInspections { get; set; }
        public DbSet<OrderItemProductionStage> OrderItemProductionStages { get; set; }
        public DbSet<ApplicationUserRole> Roles { get; set; }
        public DbSet<Style> Styles { get; set; }
        public DbSet<VoucherBatch> VoucherBatchs { get; set; }
        public DbSet<WarrantyHistory> WarrantyHistories { get; set; }
        public DbSet<WarrantyRequest> WarrantyRequests { get; set; }
        public DbSet<MaternityDressTask> MaternityDressTasks { get; set; }
        public DbSet<MilestoneTask> MilestoneTasks { get; set; }
        public DbSet<MaternityDressService> MaternityDressServices { get; set; }
        public DbSet<MaternityDressServiceOption> MaternityDressServiceOptions { get; set; }
        public DbSet<OrderItemService> OrderItemServices { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            #region Confiure Table Names
            modelBuilder.Entity<OTP>().ToTable("OTP");
            modelBuilder.Entity<ApplicationUser>().ToTable("User");
            modelBuilder.Entity<ApplicationUserToken>().ToTable("UserToken");
            modelBuilder.Entity<Appointment>().ToTable("Appointment");
            modelBuilder.Entity<Branch>().ToTable("Branch");
            modelBuilder.Entity<CartItem>().ToTable("CartItem");
            modelBuilder.Entity<Category>().ToTable("Category");
            modelBuilder.Entity<Component>().ToTable("Component");
            modelBuilder.Entity<ComponentOption>().ToTable("ComponentOption");
            modelBuilder.Entity<DesignRequest>().ToTable("DesignRequest");
            modelBuilder.Entity<MaternityDressCustomization>().ToTable("DressCustomization");
            modelBuilder.Entity<Feedback>().ToTable("Feedback");
            modelBuilder.Entity<Address>().ToTable("Address");
            modelBuilder.Entity<MaternityDress>().ToTable("MaternityDress");
            modelBuilder.Entity<MaternityDressDetail>().ToTable("MaternityDressDetail");
            modelBuilder.Entity<BranchMaternityDressDetail>().ToTable("BranchMaternityDressDetail");
            modelBuilder.Entity<MaternityDressInspection>().ToTable("MaternityDressInspection");
            modelBuilder.Entity<Measurement>().ToTable("Measurement");
            modelBuilder.Entity<MeasurementDiary>().ToTable("MeasurementDiary");
            modelBuilder.Entity<Notification>().ToTable("Notification");
            modelBuilder.Entity<Order>().ToTable("Order");
            modelBuilder.Entity<OrderItem>().ToTable("OrderItem");
            modelBuilder.Entity<OrderItemInspection>().ToTable("OrderItemInspection");
            modelBuilder.Entity<OrderItemProductionStage>().ToTable("OrderItemProductionStage");
            modelBuilder.Entity<ApplicationUserRole>().ToTable("Role");
            modelBuilder.Entity<Style>().ToTable("Style");
            modelBuilder.Entity<VoucherBatch>().ToTable("VoucherBatch");
            modelBuilder.Entity<WarrantyHistory>().ToTable("WarrantyHistory");
            modelBuilder.Entity<WarrantyRequest>().ToTable("WarrantyRequest");
            modelBuilder.Entity<MaternityDressTask>().ToTable("MaternityDressTask");
            modelBuilder.Entity<MilestoneTask>().ToTable("MilestoneTask");
            modelBuilder.Entity<MaternityDressService>().ToTable("MaternityDressService");
            modelBuilder.Entity<MaternityDressServiceOption>().ToTable("MaternityDressServiceOption");
            modelBuilder.Entity<OrderItemService>().ToTable("OrderItemService");

            #endregion

            #region Configure FluentApi

            modelBuilder.Entity<ApplicationUser>(options =>
            {
                options.HasOne(u => u.Role)
                    .WithMany(r => r.Users)
                    .HasForeignKey(u => u.RoleId)
                    .OnDelete(DeleteBehavior.NoAction);

                options.HasMany(u => u.Appointments)
                    .WithOne(a => a.User)
                    .HasForeignKey(u => u.UserId)
                    .OnDelete(DeleteBehavior.NoAction);

                options.HasMany(u => u.Feedbacks)
                    .WithOne(f => f.User)
                    .HasForeignKey(f => f.UserId)
                    .OnDelete(DeleteBehavior.NoAction);

                options.HasMany(u => u.DesignRequests)
                    .WithOne(f => f.User)
                    .HasForeignKey(f => f.UserId)
                    .OnDelete(DeleteBehavior.NoAction);

                options.HasMany(u => u.Notifications)
                    .WithOne(n => n.Receiver)
                    .HasForeignKey(n => n.ReceiverId)
                    .OnDelete(DeleteBehavior.NoAction);

                options.HasMany(u => u.OTPs)
                    .WithOne(o => o.User)
                    .HasForeignKey(o => o.UserId)
                    .OnDelete(DeleteBehavior.NoAction);

                options.HasMany(u => u.MeasurementDiaries)
                    .WithOne(m => m.User)
                    .HasForeignKey(m => m.UserId)
                    .OnDelete(DeleteBehavior.NoAction);

                options.HasMany(u => u.Addresses)
                    .WithOne(l => l.User)
                    .HasForeignKey(l => l.UserId)
                    .OnDelete(DeleteBehavior.NoAction);

                options.HasMany(u => u.CartItems)
                    .WithOne(l => l.User)
                    .HasForeignKey(l => l.UserId)
                    .OnDelete(DeleteBehavior.NoAction);

                options.HasMany(u => u.Orders)
                    .WithOne(o => o.User)
                    .HasForeignKey(o => o.UserId)
                    .OnDelete(DeleteBehavior.NoAction);

                options.HasMany(u => u.DressCustomizations)
                    .WithOne(o => o.Users)
                    .HasForeignKey(o => o.UserId)
                    .OnDelete(DeleteBehavior.NoAction);

                options.HasMany(u => u.VoucherDiscounts)
                    .WithOne(o => o.User)
                    .HasForeignKey(o => o.UserId)
                    .OnDelete(DeleteBehavior.NoAction);

                options.HasMany(u => u.Token)
                    .WithOne(l => l.User)
                    .HasForeignKey(l => l.UserId)
                    .OnDelete(DeleteBehavior.NoAction);
                
            });

            modelBuilder.Entity<Order>(options =>
            {
                options.HasMany(o => o.OrderItems)
                    .WithOne(oi => oi.Order)
                    .HasForeignKey(oi => oi.OrderId)
                    .OnDelete(DeleteBehavior.NoAction);

                options.HasMany(o => o.Transactions)
                    .WithOne(oi => oi.Order)
                    .HasForeignKey(oi => oi.OrderId)
                    .OnDelete(DeleteBehavior.NoAction);
            });

            modelBuilder.Entity<OrderItem>(options =>
            {
                options.HasOne(ot => ot.MaternityDressDetail)
                    .WithMany(mdd => mdd.OrderItems)
                    .HasForeignKey(ot => ot.MaternityDressDetailId)
                    .OnDelete(DeleteBehavior.NoAction);

                options.HasMany(ot => ot.OrderItemProductionStages)
                    .WithOne(oips => oips.OrderItem)
                    .HasForeignKey(oips => oips.OrderItemId)
                    .OnDelete(DeleteBehavior.NoAction);

                options.HasMany(ot => ot.Feedbacks)
                    .WithOne(f => f.OrderItem)
                    .HasForeignKey(f => f.OrderItemId)
                    .OnDelete(DeleteBehavior.NoAction);

                options.HasOne(ot => ot.DesignRequest)
                    .WithOne(od => od.OrderItem)
                    .HasForeignKey<OrderItem>(ot => ot.DesignRequestId)
                    .OnDelete(DeleteBehavior.NoAction);

                options.HasOne(ot => ot.MaternityDressCustomization)
                    .WithOne(mdc => mdc.OrderItems)
                    .HasForeignKey<OrderItem>(ot => ot.MaternityDressCustomizationId)
                    .OnDelete(DeleteBehavior.NoAction);

                options.HasMany(ot => ot.OrderItemInspections)
                    .WithOne(oii => oii.OrderItem)
                    .HasForeignKey(oii => oii.OrderItemId)
                    .OnDelete(DeleteBehavior.NoAction);

                options.HasMany(ot => ot.WarrantyRequests)
                    .WithOne(wh => wh.OriginalOrderItem)
                    .HasForeignKey(wh => wh.OriginalOrderItemId)
                    .OnDelete(DeleteBehavior.NoAction);
            });

            modelBuilder.Entity<OrderItemProductionStage>(options =>
            {
                options.HasKey(oips => new { oips.OrderItemId, oips.ProductionStageId });
            });

            modelBuilder.Entity<ProductionStage>(options =>
            {
                options.HasMany(ps => ps.OrderItemProductionStages)
                    .WithOne(oips => oips.ProductionStage)
                    .HasForeignKey(oips => oips.ProductionStageId)
                    .OnDelete(DeleteBehavior.NoAction);
            });

            modelBuilder.Entity<BranchMaternityDressDetail>(options =>
            {
                options.HasKey(bmdd => new { bmdd.BranchId, bmdd.MaternityDressDetailId });
            });

            modelBuilder.Entity<OrderItemService>(options =>
            {
                options.HasKey(ois => new { ois.MaternityDressServiceId, ois.OrderItemId });
            });
            
            modelBuilder.Entity<Branch>(options =>
            {
                options.HasMany(b => b.BranchMaternityDressDetail)
                    .WithOne(bmdd => bmdd.Branch)
                    .HasForeignKey(bmdd => bmdd.BranchId)
                    .OnDelete(DeleteBehavior.NoAction);
                
            });

            modelBuilder.Entity<MaternityDressDetail>(options =>
            {
                options.HasMany(md => md.BranchMaternityDressDetails)
                    .WithOne(bmdd => bmdd.MaternityDressDetail)
                    .HasForeignKey(bmdd => bmdd.MaternityDressDetailId)
                    .OnDelete(DeleteBehavior.NoAction);
            });

            modelBuilder.Entity<VoucherBatch>(options =>
            {
                options.HasMany(vb => vb.VoucherDiscounts)
                    .WithOne(vd => vd.VoucherBatch)
                    .HasForeignKey(vb => vb.VoucherBatchId)
                    .OnDelete(DeleteBehavior.NoAction);
            });

            modelBuilder.Entity<MaternityDressCustomization>(options =>
            {
                options.HasOne(mdc => mdc.MaternityDressTask)
                    .WithOne(mdt => mdt.MaternityDressCustomization)
                    .HasForeignKey<MaternityDressTask>(mdt => mdt.MaternityDressCustomizationId)
                    .OnDelete(DeleteBehavior.NoAction);
            });

            modelBuilder.Entity<DesignRequest>(options =>
            {
                options.HasOne(dr => dr.MaternityDressTask)
                    .WithOne(mdt => mdt.DesignRequest)
                    .HasForeignKey<MaternityDressTask>(mdt => mdt.DesignRequestId)
                    .OnDelete(DeleteBehavior.NoAction);
            });
            #endregion
        }
    }
}