using MamaFit.BusinessObjects.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace MamaFit.BusinessObjects.DBContext
{
    public class ApplicationDBContext : DbContext
    {
        public ApplicationDBContext() { }

        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options)
        {

        }

        public DbSet<ApplicationUser> Users { get; set; }
        public DbSet<ApplicationUserToken> UserTokens { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<Branch> Branches { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Component> Components { get; set; }
        public DbSet<ComponentOption> ComponentOptions { get; set; }
        public DbSet<DesignOrder> DesignOrders { get; set; }
        public DbSet<MaternityDressCustomization> DressCustomizations { get; set; }
        public DbSet<Feedback> Feedbacks { get; set; }
        public DbSet<Location> Locations { get; set; }
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
        public DbSet<Role> Roles { get; set; }
        public DbSet<Style> Styles { get; set; }
        public DbSet<VoucherBatch> VoucherBatchs { get; set; }
        public DbSet<WarrantyHistory> WarrantyHistories { get; set; }
        public DbSet<WarrantyRequest> WarrantyRequests { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

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

                options.HasMany(u => u.DesignOrders)
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

                options.HasMany(u => u.Location)
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

                options.HasOne(u => u.Branch)
                       .WithOne(b => b.BranchManager)
                       .HasForeignKey<ApplicationUser>(u => u.BranchId)
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

                options.HasOne(ot => ot.DesignOrder)
                       .WithOne(od => od.OrderItem)
                       .HasForeignKey<OrderItem>(ot => ot.DesignOrderId)
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

            modelBuilder.Entity<Branch>(options =>
            {
                options.HasMany(b => b.BranchMaternityDressDetail)
                       .WithOne(bmdd => bmdd.Branch)
                       .HasForeignKey(bmdd => bmdd.BranchId)
                       .OnDelete(DeleteBehavior.NoAction);


                options.HasOne(b => b.Location)
                       .WithOne(l => l.Branch)
                       .HasForeignKey<Branch>(b => b.LocationId)
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
            //sqlserver
            /*            modelBuilder.Entity<Category>(options =>
                        {
                            options.Property(c => c.ImagesJson)
                                   .HasColumnType("nvarchar(max)");
                        });*/
        }
    }
}
