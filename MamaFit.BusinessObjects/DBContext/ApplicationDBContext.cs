using MamaFit.BusinessObjects.Data;
using MamaFit.BusinessObjects.Entity;
using MamaFit.BusinessObjects.Entity.ChatEntity;
using Microsoft.EntityFrameworkCore;

namespace MamaFit.BusinessObjects.DbContext
{
    public class ApplicationDbContext : Microsoft.EntityFrameworkCore.DbContext
    {
        public ApplicationDbContext()
        {
        }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        //DbSet Chat
        public DbSet<ChatMessage> ChatMessages { get; set; }
        public DbSet<ChatRoom> ChatRooms { get; set; }
        public DbSet<ChatRoomMember> ChatRoomMembers { get; set; }
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
        public DbSet<Measurement> Measurements { get; set; }
        public DbSet<MeasurementDiary> MeasurementDiaries { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<OrderItemTask> OrderItemsTasks { get; set; }
        public DbSet<ApplicationUserRole> Roles { get; set; }
        public DbSet<Style> Styles { get; set; }
        public DbSet<VoucherBatch> VoucherBatchs { get; set; }
        public DbSet<WarrantyHistory> WarrantyHistories { get; set; }
        public DbSet<WarrantyRequest> WarrantyRequests { get; set; }
        public DbSet<MaternityDressTask> MaternityDressTasks { get; set; }
        public DbSet<Milestone> Milestones { get; set; }
        public DbSet<MaternityDressService> MaternityDressServices { get; set; }
        public DbSet<MaternityDressServiceOption> MaternityDressServiceOptions { get; set; }
        public DbSet<OrderItemServiceOption> OrderItemServiceOptions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            #region Confiure Table Names

            modelBuilder.Entity<ChatMessage>().ToTable("ChatMessage");
            modelBuilder.Entity<ChatRoom>().ToTable("ChatRoom");
            modelBuilder.Entity<ChatRoomMember>().ToTable("ChatRoomMember");
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
            modelBuilder.Entity<MaternityDressCustomization>().ToTable("MaternityDressCustomization");
            modelBuilder.Entity<Feedback>().ToTable("Feedback");
            modelBuilder.Entity<Address>().ToTable("Address");
            modelBuilder.Entity<MaternityDress>().ToTable("MaternityDress");
            modelBuilder.Entity<MaternityDressDetail>().ToTable("MaternityDressDetail");
            modelBuilder.Entity<BranchMaternityDressDetail>().ToTable("BranchMaternityDressDetail");
            modelBuilder.Entity<Measurement>().ToTable("Measurement");
            modelBuilder.Entity<MeasurementDiary>().ToTable("MeasurementDiary");
            modelBuilder.Entity<Notification>().ToTable("Notification");
            modelBuilder.Entity<Order>().ToTable("Order");
            modelBuilder.Entity<OrderItem>().ToTable("OrderItem");
            modelBuilder.Entity<ApplicationUserRole>().ToTable("Role");
            modelBuilder.Entity<Style>().ToTable("Style");
            modelBuilder.Entity<VoucherBatch>().ToTable("VoucherBatch");
            modelBuilder.Entity<WarrantyHistory>().ToTable("WarrantyHistory");
            modelBuilder.Entity<WarrantyRequest>().ToTable("WarrantyRequest");
            modelBuilder.Entity<MaternityDressTask>().ToTable("MaternityDressTask");
            modelBuilder.Entity<Milestone>().ToTable("Milestone");
            modelBuilder.Entity<MaternityDressService>().ToTable("MaternityDressService");
            modelBuilder.Entity<MaternityDressServiceOption>().ToTable("MaternityDressServiceOption");
            modelBuilder.Entity<OrderItemServiceOption>().ToTable("OrderItemServiceOption");

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
                    .WithOne(o => o.User)
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

                options.HasMany(u => u.Branch)
                    .WithOne(b => b.BranchManager)
                    .HasForeignKey(b => b.BranchManagerId)
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

                options.HasMany(ot => ot.Feedbacks)
                    .WithOne(f => f.OrderItem)
                    .HasForeignKey(f => f.OrderItemId)
                    .OnDelete(DeleteBehavior.NoAction);

                options.HasOne(ot => ot.DesignRequest)
                    .WithOne(od => od.OrderItem)
                    .HasForeignKey<DesignRequest>(ot => ot.OrderItemId)
                    .OnDelete(DeleteBehavior.NoAction);

                options.HasOne(ot => ot.MaternityDressCustomization)
                    .WithOne(mdc => mdc.OrderItem)
                    .HasForeignKey<MaternityDressCustomization>(ot => ot.OrderItemId)
                    .OnDelete(DeleteBehavior.NoAction);

                options.HasMany(ot => ot.WarrantyRequests)
                    .WithOne(wh => wh.OriginalOrderItem)
                    .HasForeignKey(wh => wh.OriginalOrderItemId)
                    .OnDelete(DeleteBehavior.NoAction);
            });

            modelBuilder.Entity<BranchMaternityDressDetail>(options =>
            {
                options.HasKey(bmdd => new { bmdd.BranchId, bmdd.MaternityDressDetailId });
            });

            modelBuilder.Entity<OrderItemTask>(options =>
            {
                options.HasKey(oit => new
                {
                    oit.UserId,
                    oit.OrderItemId,
                    oit.MilestoneId
                });
            });

            modelBuilder.Entity<OrderItemServiceOption>(options =>
            {
                options.HasKey(ois => new { ois.MaternityDressServiceOptionId, ois.OrderItemId });
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

            modelBuilder.Entity<ChatRoom>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Description).HasMaxLength(500);
            });

            modelBuilder.Entity<ChatMessage>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Message).IsRequired().HasMaxLength(1000);
                entity.HasIndex(e => e.CreatedAt);
                entity.HasIndex(e => new { e.ChatRoomId, e.CreatedAt });
                entity.HasIndex(e => e.SenderId);

                entity.HasOne(m => m.Sender)
                      .WithMany(u => u.SentMessages)
                      .HasForeignKey(m => m.SenderId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(m => m.ChatRoom)
                      .WithMany(r => r.Messages)
                      .HasForeignKey(m => m.ChatRoomId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<ChatRoomMember>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.ChatRoomId });
                entity.HasIndex(e => e.UserId);
                entity.HasIndex(e => e.ChatRoomId);

                entity.HasOne(m => m.User)
                      .WithMany(u => u.ChatRoomMemberships)
                      .HasForeignKey(m => m.UserId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(m => m.ChatRoom)
                      .WithMany(r => r.Members)
                      .HasForeignKey(m => m.ChatRoomId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            SeedData.Seed(modelBuilder);

            #endregion
        }
    }
}