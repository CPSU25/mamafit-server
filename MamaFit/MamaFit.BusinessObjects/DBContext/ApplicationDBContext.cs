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
        public DbSet<WarrantyHistory> WarrantyHistories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ApplicationUser>(options =>
            {
                options.HasOne(u => u.Role)
                       .WithMany(r => r.Users)
                       .HasForeignKey(u => u.RoleId)
                       .OnDelete(DeleteBehavior.Cascade);

                options.HasMany(u => u.Appointments)
                       .WithOne(a => a.User)
                       .HasForeignKey(u => u.UserId)
                       .OnDelete(DeleteBehavior.Cascade);

                options.HasMany(u => u.Feedbacks)
                       .WithOne(f => f.User)
                       .HasForeignKey(f => f.UserId)
                       .OnDelete(DeleteBehavior.Cascade);

                options.HasMany(u => u.DesignOrders)
                       .WithOne(f => f.User)
                       .HasForeignKey(f => f.UserId)
                       .OnDelete(DeleteBehavior.Cascade);

                options.HasMany(u => u.Notifications)
                       .WithOne(n => n.Receiver)
                       .HasForeignKey(n => n.ReceiverId)
                       .OnDelete(DeleteBehavior.Cascade);

                options.HasMany(u => u.OTPs)
                       .WithOne(o => o.User)
                       .HasForeignKey(o => o.UserId)
                       .OnDelete(DeleteBehavior.Cascade);

                options.HasMany(u => u.MeasurementDiaries)
                       .WithOne(m => m.User)
                       .HasForeignKey(m => m.UserId)
                       .OnDelete(DeleteBehavior.Cascade);

                options.HasMany(u => u.Location)
                       .WithOne(l => l.User)
                       .HasForeignKey(l => l.UserId)
                       .OnDelete(DeleteBehavior.Cascade);

                options.HasMany(u => u.CartItems)
                       .WithOne(l => l.User)
                       .HasForeignKey(l => l.UserId)
                       .OnDelete(DeleteBehavior.Cascade);

                options.HasMany(u => u.Orders)
                       .WithOne(o => o.User)
                       .HasForeignKey(o => o.UserId)
                       .OnDelete(DeleteBehavior.Cascade);

                options.HasMany(u => u.DressCustomizations)
                       .WithOne(o => o.Users)
                       .HasForeignKey(o => o.UserId)
                       .OnDelete(DeleteBehavior.Cascade);

                options.HasMany(u => u.VoucherDiscounts)
                       .WithOne(o => o.User)
                       .HasForeignKey(o => o.UserId)
                       .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Order>(options =>
            {
                options.HasMany(o => o.OrderItems)
                       .WithOne(oi => oi.Order)
                       .HasForeignKey(oi => oi.OrderId)
                       .OnDelete(DeleteBehavior.Cascade);

                options.HasMany(o => o.Transactions)
                       .WithOne(oi => oi.Order)
                       .HasForeignKey(oi => oi.OrderId)
                       .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<OrderItem>(options =>
            {
                options.HasOne(ot => ot.MaternityDressDetail)
                       .WithMany(mdd => mdd.OrderItems)
                       .HasForeignKey(ot => ot.MaternityDressDetailId)
                       .OnDelete(DeleteBehavior.Cascade);

                options.HasMany(ot => ot.OrderItemProductionStages)
                       .WithOne(oips => oips.OrderItem)
                       .HasForeignKey(oips => oips.OrderItemId)
                       .OnDelete(DeleteBehavior.Cascade);

                options.HasMany(ot => ot.Feedbacks)
                       .WithOne(f => f.OrderItem)
                       .HasForeignKey(f => f.OrderItemId)
                       .OnDelete(DeleteBehavior.Cascade);

                options.HasMany(ot => ot.OrderItemInspections)
                       .WithOne(oii => oii.OrderItem)
                       .HasForeignKey(oii => oii.OrderItemId)
                       .OnDelete(DeleteBehavior.Cascade);


                options.HasMany(ot => ot.WarrantyRequests)
                       .WithOne(wh => wh.OriginalOrderItem)
                       .HasForeignKey(wh => wh.OriginalOrderItemId)
                       .OnDelete(DeleteBehavior.Cascade);
            });

        }
    }
}
