using MamaFit.BusinessObjects.Entity;
using MamaFit.BusinessObjects.Enum;
using Microsoft.EntityFrameworkCore;

namespace MamaFit.BusinessObjects.Data
{
    public static class SeedData
    {
        public static void Seed(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ApplicationUserRole>().HasData(
                new ApplicationUserRole
                {
                    Id = "b8d237b8b6f849988d60c6c3c1d0a943",
                    RoleName = "User"
                },
                new ApplicationUserRole
                {
                    Id = "bf081015e17a41b8b1cae65b1b17cfdb",
                    RoleName = "BranchManager"
                },
                new ApplicationUserRole
                {
                    Id = "c9118b99c0ad486dbb18560a916b630c",
                    RoleName = "BranchStaff"
                },
                new ApplicationUserRole
                {
                    Id = "e5b0f987fbf44608b7a6a2d0e313b3b2",
                    RoleName = "Designer"
                },
                new ApplicationUserRole
                {
                    Id = "a3cb88edaf2b4718a9986010c5b9c1d7",
                    RoleName = "Manager"
                },
                new ApplicationUserRole
                {
                    Id = "5ed8cfa9b62d433c88ab097b6d2baccd",
                    RoleName = "Staff"
                },
                new ApplicationUserRole
                {
                    Id = "2e7b5a97e42e4e84a08ffbe0bc05d2ea",
                    RoleName = "Admin"
                }
            );

            modelBuilder.Entity<ApplicationUser>().HasData(
                new ApplicationUser
                {
                    Id = "1a3bcd12345678901234567890123456",
                    UserName = "admin",
                    UserEmail = "admin@mamafit.com",
                    HashPassword = "u+7jaa75MJns9ST/6FSw4aQU+zSF/iNldvzpJxBFzwk=",
                    Salt = "R0kox04KXBTPFFlKjfxIzNeMzlbH1rGO/YJDtl0N894=",
                    FullName = "Admin",
                    IsVerify = true,
                    RoleId = "2e7b5a97e42e4e84a08ffbe0bc05d2ea",
                },
                new ApplicationUser
                {
                    Id = "ce5235c40924fd5b0792732d3fb1b6f",
                    UserName = "staff",
                    UserEmail = "staff@mamafit.com",
                    HashPassword = "u+7jaa75MJns9ST/6FSw4aQU+zSF/iNldvzpJxBFzwk=",
                    Salt = "R0kox04KXBTPFFlKjfxIzNeMzlbH1rGO/YJDtl0N894=",
                    FullName = "Staff",
                    IsVerify = true,
                    RoleId = "5ed8cfa9b62d433c88ab097b6d2baccd",
                },
                new ApplicationUser
                {
                    Id = "4c9804ecc1d645de96fcfc906cc43d6c",
                    UserName = "manager",
                    UserEmail = "manager@mamafit.com",
                    HashPassword = "u+7jaa75MJns9ST/6FSw4aQU+zSF/iNldvzpJxBFzwk=",
                    Salt = "R0kox04KXBTPFFlKjfxIzNeMzlbH1rGO/YJDtl0N894=",
                    FullName = "Manager",
                    IsVerify = true,
                    RoleId = "a3cb88edaf2b4718a9986010c5b9c1d7",
                },
                new ApplicationUser
                {
                    Id = "08ee8586464b43dd9a4507add95b281c",
                    UserName = "designer",
                    UserEmail = "designer@mamafit.com",
                    HashPassword = "u+7jaa75MJns9ST/6FSw4aQU+zSF/iNldvzpJxBFzwk=",
                    Salt = "R0kox04KXBTPFFlKjfxIzNeMzlbH1rGO/YJDtl0N894=",
                    FullName = "Designer",
                    IsVerify = true,
                    RoleId = "e5b0f987fbf44608b7a6a2d0e313b3b2",
                },
                new ApplicationUser
                {
                    Id = "eb019fbe31e6449b9b92c89b5c893b03",
                    UserName = "branchstaff",
                    UserEmail = "branchstaff@mamafit.com",
                    HashPassword = "u+7jaa75MJns9ST/6FSw4aQU+zSF/iNldvzpJxBFzwk=",
                    Salt = "R0kox04KXBTPFFlKjfxIzNeMzlbH1rGO/YJDtl0N894=",
                    FullName = "Branch Staff",
                    IsVerify = true,
                    RoleId = "c9118b99c0ad486dbb18560a916b630c",
                },
                new ApplicationUser
                {
                    Id = "29d72211a9f7480c9812d61ee17c92b9",
                    UserName = "branchmanager",
                    UserEmail = "branchmanager@mamafit.com",
                    HashPassword = "u+7jaa75MJns9ST/6FSw4aQU+zSF/iNldvzpJxBFzwk=",
                    Salt = "R0kox04KXBTPFFlKjfxIzNeMzlbH1rGO/YJDtl0N894=",
                    FullName = "Branch Manager",
                    IsVerify = true,
                    RoleId = "bf081015e17a41b8b1cae65b1b17cfdb",
                },
                new ApplicationUser
                {
                    Id = "f49aa51bbd304e77933e24bbed65b165",
                    UserName = "user",
                    UserEmail = "user@mamafit.com",
                    HashPassword = "u+7jaa75MJns9ST/6FSw4aQU+zSF/iNldvzpJxBFzwk=",
                    Salt = "R0kox04KXBTPFFlKjfxIzNeMzlbH1rGO/YJDtl0N894=",
                    FullName = "User",
                    IsVerify = true,
                    RoleId = "b8d237b8b6f849988d60c6c3c1d0a943",
                }
            );

            #region Seed Category, Style, and Component
            var now = DateTime.UtcNow;
            var createdBy = "System";

            // ===== CATEGORIES =====
            var categories = new List<Category>
{
    new Category
    {
        Id = Guid.NewGuid().ToString("N"),
        Name = "Beach/Resort",
        Description = "Trang phục phù hợp khi đi biển hoặc nghỉ dưỡng.",
        Images = new() { "https://example.com/images/beach.jpg" },
        CreatedBy = createdBy, UpdatedBy = createdBy, CreatedAt = now, UpdatedAt = now
    },
    new Category
    {
        Id = Guid.NewGuid().ToString("N"),
        Name = "Casual/Everyday",
        Description = "Phong cách thường ngày, thoải mái.",
        Images = new() { "https://example.com/images/casual.jpg" },
        CreatedBy = createdBy, UpdatedBy = createdBy, CreatedAt = now, UpdatedAt = now
    },
    new Category
    {
        Id = Guid.NewGuid().ToString("N"),
        Name = "Formal/Event",
        Description = "Phù hợp với các sự kiện trang trọng.",
        Images = new() { "https://example.com/images/formal.jpg" },
        CreatedBy = createdBy, UpdatedBy = createdBy, CreatedAt = now, UpdatedAt = now
    },
    new Category
    {
        Id = Guid.NewGuid().ToString("N"),
        Name = "Party/Special",
        Description = "Dành cho buổi tiệc hoặc dịp đặc biệt.",
        Images = new() { "https://example.com/images/party.jpg" },
        CreatedBy = createdBy, UpdatedBy = createdBy, CreatedAt = now, UpdatedAt = now
    },
    new Category
    {
        Id = Guid.NewGuid().ToString("N"),
        Name = "Work/Office",
        Description = "Thiết kế trang nhã dành cho công sở.",
        Images = new() { "https://example.com/images/work.jpg" },
        CreatedBy = createdBy, UpdatedBy = createdBy, CreatedAt = now, UpdatedAt = now
    }
};

            // ===== STYLES =====
            var styles = new List<Style>
{
    new Style
    {
        Id = Guid.NewGuid().ToString("N"),
        Name = "Maxi",
        Description = "Đầm dài maxi, thích hợp đi biển hoặc dạo phố.",
        Images = new() { "https://example.com/images/maxi.jpg" },
        IsCustom = false,
        CategoryId = categories.First(c => c.Name == "Beach/Resort").Id,
        CreatedBy = createdBy, UpdatedBy = createdBy, CreatedAt = now, UpdatedAt = now
    }
};

            // ===== COMPONENTS =====
            var components = new List<Component>
{
    new Component { Id = Guid.NewGuid().ToString("N"), Name = "Neckline", Description = "Kiểu dáng cổ áo.", Images = new() { "https://example.com/images/neckline.jpg" }, CreatedBy = createdBy, UpdatedBy = createdBy, CreatedAt = now, UpdatedAt = now },
    new Component { Id = Guid.NewGuid().ToString("N"), Name = "Sleeves", Description = "Kiểu tay áo.", Images = new() { "https://example.com/images/sleeves.jpg" }, CreatedBy = createdBy, UpdatedBy = createdBy, CreatedAt = now, UpdatedAt = now },
    new Component { Id = Guid.NewGuid().ToString("N"), Name = "Waist", Description = "Kiểu eo.", Images = new() { "https://example.com/images/waist.jpg" }, CreatedBy = createdBy, UpdatedBy = createdBy, CreatedAt = now, UpdatedAt = now },
    new Component { Id = Guid.NewGuid().ToString("N"), Name = "Hem", Description = "Đường viền gấu váy.", Images = new() { "https://example.com/images/hem.jpg" }, CreatedBy = createdBy, UpdatedBy = createdBy, CreatedAt = now, UpdatedAt = now },
    new Component { Id = Guid.NewGuid().ToString("N"), Name = "Color", Description = "Màu sắc.", Images = new() { "https://example.com/images/color.jpg" }, CreatedBy = createdBy, UpdatedBy = createdBy, CreatedAt = now, UpdatedAt = now },
    new Component { Id = Guid.NewGuid().ToString("N"), Name = "Fabric", Description = "Chất liệu vải.", Images = new() { "https://example.com/images/fabric.jpg" }, CreatedBy = createdBy, UpdatedBy = createdBy, CreatedAt = now, UpdatedAt = now }
};

            // ===== COMPONENT OPTIONS =====
            var options = new List<ComponentOption>
{
    new ComponentOption { Id = Guid.NewGuid().ToString("N"), Name = "Round Neck", ComponentId = components[0].Id, Price = 20000, Description = "Cổ tròn cơ bản, thanh lịch.", Images = new() { "https://example.com/images/roundneck.jpg" }, ComponentOptionType = ComponentOptionType.APPROVED, CreatedBy = createdBy, UpdatedBy = createdBy, CreatedAt = now, UpdatedAt = now },
    new ComponentOption { Id = Guid.NewGuid().ToString("N"), Name = "V Neck", ComponentId = components[0].Id, Price = 25000, Description = "Cổ chữ V tạo cảm giác cổ dài hơn.", Images = new() { "https://example.com/images/vneck.jpg" }, ComponentOptionType = ComponentOptionType.APPROVED, CreatedBy = createdBy, UpdatedBy = createdBy, CreatedAt = now, UpdatedAt = now },

    new ComponentOption { Id = Guid.NewGuid().ToString("N"), Name = "No Sleeves", ComponentId = components[1].Id, Price = 10000, Description = "Không tay, thoáng mát.", Images = new() { "https://example.com/images/nosleeves.jpg" }, ComponentOptionType = ComponentOptionType.APPROVED, CreatedBy = createdBy, UpdatedBy = createdBy, CreatedAt = now, UpdatedAt = now },
    new ComponentOption { Id = Guid.NewGuid().ToString("N"), Name = "Spaghetti Strap", ComponentId = components[1].Id, Price = 12000, Description = "Dây mảnh, nữ tính.", Images = new() { "https://example.com/images/spaghetti.jpg" }, ComponentOptionType = ComponentOptionType.APPROVED, CreatedBy = createdBy, UpdatedBy = createdBy, CreatedAt = now, UpdatedAt = now },
    new ComponentOption { Id = Guid.NewGuid().ToString("N"), Name = "Flattered Short Sleeves", ComponentId = components[1].Id, Price = 15000, Description = "Tay ngắn bay bổng, thoải mái.", Images = new() { "https://example.com/images/shortsleeves.jpg" }, ComponentOptionType = ComponentOptionType.APPROVED, CreatedBy = createdBy, UpdatedBy = createdBy, CreatedAt = now, UpdatedAt = now },

    new ComponentOption { Id = Guid.NewGuid().ToString("N"), Name = "Normal Waist", ComponentId = components[2].Id, Price = 18000, Description = "Thiết kế eo trung bình truyền thống.", Images = new() { "https://example.com/images/normalwaist.jpg" }, ComponentOptionType = ComponentOptionType.APPROVED, CreatedBy = createdBy, UpdatedBy = createdBy, CreatedAt = now, UpdatedAt = now },
    new ComponentOption { Id = Guid.NewGuid().ToString("N"), Name = "Empire Waist", ComponentId = components[2].Id, Price = 20000, Description = "Eo cao, phù hợp cho phong cách nữ tính.", Images = new() { "https://example.com/images/empirewaist.jpg" }, ComponentOptionType = ComponentOptionType.APPROVED, CreatedBy = createdBy, UpdatedBy = createdBy, CreatedAt = now, UpdatedAt = now },
    new ComponentOption { Id = Guid.NewGuid().ToString("N"), Name = "Bow Tie Waist", ComponentId = components[2].Id, Price = 22000, Description = "Nơ thắt eo tạo điểm nhấn đáng yêu.", Images = new() { "https://example.com/images/bowtie.jpg" }, ComponentOptionType = ComponentOptionType.APPROVED, CreatedBy = createdBy, UpdatedBy = createdBy, CreatedAt = now, UpdatedAt = now },

    new ComponentOption { Id = Guid.NewGuid().ToString("N"), Name = "Single Layer Hem", ComponentId = components[3].Id, Price = 12000, Description = "Gấu váy đơn giản, thanh lịch.", Images = new() { "https://example.com/images/singlelayer.jpg" }, ComponentOptionType = ComponentOptionType.APPROVED, CreatedBy = createdBy, UpdatedBy = createdBy, CreatedAt = now, UpdatedAt = now },
    new ComponentOption { Id = Guid.NewGuid().ToString("N"), Name = "High-Low Hem", ComponentId = components[3].Id, Price = 16000, Description = "Trước ngắn sau dài tạo nét lạ mắt.", Images = new() { "https://example.com/images/highlow.jpg" }, ComponentOptionType = ComponentOptionType.APPROVED, CreatedBy = createdBy, UpdatedBy = createdBy, CreatedAt = now, UpdatedAt = now },

    new ComponentOption { Id = Guid.NewGuid().ToString("N"), Name = "Red", ComponentId = components[4].Id, Price = 10000, Description = "Màu đỏ nổi bật, quyến rũ.", Images = new() { "https://example.com/images/red.jpg" }, ComponentOptionType = ComponentOptionType.APPROVED, CreatedBy = createdBy, UpdatedBy = createdBy, CreatedAt = now, UpdatedAt = now },
    new ComponentOption { Id = Guid.NewGuid().ToString("N"), Name = "Navy Blue", ComponentId = components[4].Id, Price = 10000, Description = "Xanh navy lịch sự, thanh nhã.", Images = new() { "https://example.com/images/navyblue.jpg" }, ComponentOptionType = ComponentOptionType.APPROVED, CreatedBy = createdBy, UpdatedBy = createdBy, CreatedAt = now, UpdatedAt = now },
    new ComponentOption { Id = Guid.NewGuid().ToString("N"), Name = "Emerald Green", ComponentId = components[4].Id, Price = 10000, Description = "Xanh ngọc quý phái, nổi bật.", Images = new() { "https://example.com/images/emerald.jpg" }, ComponentOptionType = ComponentOptionType.APPROVED, CreatedBy = createdBy, UpdatedBy = createdBy, CreatedAt = now, UpdatedAt = now },

    new ComponentOption { Id = Guid.NewGuid().ToString("N"), Name = "Cotton", ComponentId = components[5].Id, Price = 9000, Description = "Vải cotton thoáng mát, thấm hút tốt.", Images = new() { "https://example.com/images/cotton.jpg" }, ComponentOptionType = ComponentOptionType.APPROVED, CreatedBy = createdBy, UpdatedBy = createdBy, CreatedAt = now, UpdatedAt = now }
};

            #endregion

            modelBuilder.Entity<Category>().HasData(categories);
            modelBuilder.Entity<Style>().HasData(styles);
            modelBuilder.Entity<Component>().HasData(components);
            modelBuilder.Entity<ComponentOption>().HasData(options);
        }
    }
}