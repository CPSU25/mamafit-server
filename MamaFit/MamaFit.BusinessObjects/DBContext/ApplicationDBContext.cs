using MamaFit.BusinessObjects.Entity;
using Microsoft.EntityFrameworkCore;

namespace MamaFit.BusinessObjects.DBContext
{
    public class ApplicationDBContext : DbContext
    {
        public ApplicationDBContext() { }

        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options)
        {

        }

        public DbSet<ApplicationUser> User { get; set; }
        public DbSet<ApplicationUserToken> UserToken { get; set; }
        public DbSet<Appointment> Appointment { get; set; }
        public DbSet<Branch> Branch { get; set; }
        public DbSet<CartItem> CartItem { get; set; }
        public DbSet<Category> Category { get; set; }
        public DbSet<Component> Component { get; set; }
        public DbSet<ComponentOption> ComponentOption { get; set; }
        public DbSet<DesignOrder> DesignOrder { get; set; }
        public DbSet<DressCustomization> DressCustomization { get; set; }
        public DbSet<Feedback> Feedback { get; set; }
        public DbSet<Location> Location { get; set; }
        public DbSet<MaternityDress> MaternityDress { get; set; }
        public DbSet<MaternityDressDetail> MaternityDressDetail { get; set; }
        public DbSet<BranchMaternityDressDetail> BranchMaternityDressDetail { get; set; }
        public DbSet<MaternityDressInspection> MaternityDressInspection { get; set; }
        public DbSet<Measurement> Measurement { get; set; }
        public DbSet<MeasurementDiary> MeasurementDiary { get; set; }
        public DbSet<Notification> Notification { get; set; }
        public DbSet<Order> Order { get; set; }
        public DbSet<OrderItem> OrderItem { get; set; }
        public DbSet<OrderItemInspection> OrderItemInspection { get; set; }
        public DbSet<OrderItemProductionStage> OrderItemProductionStage { get; set; }
        public DbSet<Role> Role { get; set; }
        public DbSet<Style> Style { get; set; }
        public DbSet<WarrantyHistory> WarrantyHistory { get; set; }

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
                       .WithOne(o => o.User)
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
                       .WithOne(wh => wh.WarrantyOrderItem)
                       .HasForeignKey(wh => wh.WarrantyOrderItemId)
                       .OnDelete(DeleteBehavior.Cascade);

                options.HasMany(ot => ot.WarrantyRequests)
                       .WithOne(wh => wh.OriginalOrderItem)
                       .HasForeignKey(wh => wh.OriginalOrderItemId)
                       .OnDelete(DeleteBehavior.Cascade);
            });

        }
    }
}
