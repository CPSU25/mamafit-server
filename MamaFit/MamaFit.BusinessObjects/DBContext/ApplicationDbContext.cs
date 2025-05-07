using MamaFit.BusinessObjects.Entity;
using Microsoft.EntityFrameworkCore;

namespace MamaFit.BusinessObjects.DBContext
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<ApplicationUser> Users { get; set; }
        public DbSet<Appointment> Appointment { get; set; }
        public DbSet<Branch> Branch { get; set; }
        public DbSet<Dress> Dress { get; set; }
        public DbSet<DressCategory> DressCategory { get; set; }
        public DbSet<DressComponent> DressComponent { get; set; }
        public DbSet<DressComponentOption> DressComponentOption { get; set; }
        public DbSet<Feedback> Feedback { get; set; }
        public DbSet<MeasurementDiary> MeasurementDiary { get; set; }
        public DbSet<Order> Order { get; set; }
        public DbSet<OrderDetail> OrderDetail { get; set; }
        public DbSet<Profile> Profile { get; set; }
        public DbSet<TaskOrder> TaskOrder { get; set; }
        public DbSet<VoucherDiscount> VoucherDiscount { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<ApplicationUser>(entity =>
            {
                entity.HasMany(u => u.Profile)
                      .WithOne(p => p.User)
                      .HasForeignKey(p => p.UserId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasMany(u => u.Order)
                      .WithOne(o => o.User)
                      .HasForeignKey(o => o.UserId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasMany(u => u.Feedback)
                      .WithOne(f => f.User)
                      .HasForeignKey(f => f.UserId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasMany(u => u.TaskOrder)
                      .WithOne(to => to.Designer)
                      .HasForeignKey(to => to.DesignerId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            builder.Entity<Branch>(entity =>
            {
                entity.HasMany(b => b.Appointment)
                      .WithOne(a => a.Branch)
                      .HasForeignKey(a => a.BranchId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            builder.Entity<Dress>(entity =>
            {
                entity.HasMany(d => d.Feedback)
                      .WithOne(f => f.Dress)
                      .HasForeignKey(f => f.DressId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            builder.Entity<MeasurementDiary>(entity =>
            {
                entity.HasOne(md => md.Profile)
                      .WithMany(p => p.Diary)
                      .HasForeignKey(md => md.ProfileId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            builder.Entity<Order>(entity =>
            {
                entity.HasMany(o => o.Details)
                      .WithOne(o => o.Order)
                      .HasForeignKey(o => o.OrderId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

        }
    }
}
