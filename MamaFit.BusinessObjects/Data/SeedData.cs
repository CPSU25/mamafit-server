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
        Name = "Đầm bầu công sở",
        Description = "Thiết kế dành cho môi trường làm việc, lịch sự và thoải mái.",
        Images = new() { "https://example.com/images/workwear.jpg" },
        CreatedBy = createdBy, UpdatedBy = createdBy, CreatedAt = now, UpdatedAt = now
    },
    new Category
    {
        Id = Guid.NewGuid().ToString("N"),
        Name = "Đầm bầu dạo phố",
        Description = "Phong cách trẻ trung, năng động cho các buổi đi chơi.",
        Images = new() { "https://example.com/images/streetwear.jpg" },
        CreatedBy = createdBy, UpdatedBy = createdBy, CreatedAt = now, UpdatedAt = now
    }
};

            // ===== STYLES =====
            var styles = new List<Style>
{
    new Style
    {
        Id = Guid.NewGuid().ToString("N"),
        Name = "Đầm A-line",
        Description = "Thiết kế ôm phần ngực và xòe dần xuống dưới.",
        Images = new() { "https://example.com/images/aline.jpg" },
        IsCustom = false,
        CategoryId = categories[0].Id,
        CreatedBy = createdBy, UpdatedBy = createdBy, CreatedAt = now, UpdatedAt = now
    },
    new Style
    {
        Id = Guid.NewGuid().ToString("N"),
        Name = "Đầm xếp ly nhẹ",
        Description = "Tạo điểm nhấn nhẹ nhàng với đường xếp ly phía bụng.",
        Images = new() { "https://example.com/images/pleated.jpg" },
        IsCustom = true,
        CategoryId = categories[1].Id,
        CreatedBy = createdBy, UpdatedBy = createdBy, CreatedAt = now, UpdatedAt = now
    },
    new Style
    {
        Id = Guid.NewGuid().ToString("N"),
        Name = "Đầm hai dây thun eo",
        Description = "Phong cách thoải mái với dây vai mảnh và thun co giãn ở eo.",
        Images = new() { "https://example.com/images/spaghettistrap.jpg" },
        IsCustom = false,
        CategoryId = categories[1].Id,
        CreatedBy = createdBy, UpdatedBy = createdBy, CreatedAt = now, UpdatedAt = now
    }
};

            // ===== COMPONENTS =====
            var components = new List<Component>
{
    new Component
    {
        Id = Guid.NewGuid().ToString("N"),
        Name = "Cổ áo",
        Description = "Lựa chọn kiểu cổ áo phù hợp với phong cách và hoàn cảnh.",
        Images = new() { "https://example.com/images/collar.jpg" },
        CreatedBy = createdBy, UpdatedBy = createdBy, CreatedAt = now, UpdatedAt = now
    },
    new Component
    {
        Id = Guid.NewGuid().ToString("N"),
        Name = "Chất liệu vải",
        Description = "Ảnh hưởng đến độ thoáng khí và cảm giác thoải mái.",
        Images = new() { "https://example.com/images/fabric.jpg" },
        CreatedBy = createdBy, UpdatedBy = createdBy, CreatedAt = now, UpdatedAt = now
    },
    new Component
    {
        Id = Guid.NewGuid().ToString("N"),
        Name = "Chiều dài váy",
        Description = "Điều chỉnh độ dài theo sở thích và dịp sử dụng.",
        Images = new() { "https://example.com/images/length.jpg" },
        CreatedBy = createdBy, UpdatedBy = createdBy, CreatedAt = now, UpdatedAt = now
    }
};

            // ===== COMPONENT OPTIONS =====
            var options = new List<ComponentOption>
{
    new ComponentOption
    {
        Id = Guid.NewGuid().ToString("N"),
        Name = "Cổ tròn mềm",
        ComponentId = components[0].Id,
        Price = 30000,
        Description = "Phù hợp cho phong cách nhẹ nhàng.",
        Images = new() { "https://example.com/images/roundsoft.jpg" },
        Tag = new Tag { ParentTag = ["Cổ áo"], ChildTag = ["Tròn"] },
        ComponentOptionType = ComponentOptionType.APPROVED,
        CreatedBy = createdBy, UpdatedBy = createdBy, CreatedAt = now, UpdatedAt = now
    },
    new ComponentOption
    {
        Id = Guid.NewGuid().ToString("N"),
        Name = "Cổ V gợi cảm",
        ComponentId = components[0].Id,
        Price = 50000,
        Description = "Tạo điểm nhấn cổ áo quyến rũ.",
        Images = new() { "https://example.com/images/vneck.jpg" },
        Tag = new Tag { ParentTag = ["Cổ áo"], ChildTag = ["Cổ V"] },
        ComponentOptionType = ComponentOptionType.APPROVAL_PENDING,
        CreatedBy = createdBy, UpdatedBy = createdBy, CreatedAt = now, UpdatedAt = now
    },
    new ComponentOption
    {
        Id = Guid.NewGuid().ToString("N"),
        Name = "Cotton mềm cao cấp",
        ComponentId = components[1].Id,
        Price = 70000,
        Description = "Vải cotton dày dặn, co giãn tốt.",
        Images = new() { "https://example.com/images/premiumcotton.jpg" },
        Tag = new Tag { ParentTag = ["Vải"], ChildTag = ["Cotton", "Mùa hè"] },
        ComponentOptionType = ComponentOptionType.QUOTATION_PENDING,
        CreatedBy = createdBy, UpdatedBy = createdBy, CreatedAt = now, UpdatedAt = now
    },
    new ComponentOption
    {
        Id = Guid.NewGuid().ToString("N"),
        Name = "Linen thoáng khí",
        ComponentId = components[1].Id,
        Price = 60000,
        Description = "Lý tưởng cho thời tiết nóng ẩm.",
        Images = new() { "https://example.com/images/linen.jpg" },
        Tag = new Tag { ParentTag = ["Vải"], ChildTag = ["Linen"] },
        ComponentOptionType = ComponentOptionType.APPROVED,
        CreatedBy = createdBy, UpdatedBy = createdBy, CreatedAt = now, UpdatedAt = now
    },
    new ComponentOption
    {
        Id = Guid.NewGuid().ToString("N"),
        Name = "Dài qua gối",
        ComponentId = components[2].Id,
        Price = 80000,
        Description = "Mang lại vẻ trang nhã và kín đáo.",
        Images = new() { "https://example.com/images/belowknee.jpg" },
        Tag = new Tag { ParentTag = ["Chiều dài"], ChildTag = ["Qua gối"] },
        ComponentOptionType = ComponentOptionType.APPROVAL_PENDING,
        CreatedBy = createdBy, UpdatedBy = createdBy, CreatedAt = now, UpdatedAt = now
    },
    new ComponentOption
    {
        Id = Guid.NewGuid().ToString("N"),
        Name = "Ngắn trên gối",
        ComponentId = components[2].Id,
        Price = 40000,
        Description = "Trẻ trung, năng động, phù hợp đi chơi.",
        Images = new() { "https://example.com/images/aboveknee.jpg" },
        Tag = new Tag { ParentTag = ["Chiều dài"], ChildTag = ["Trên gối"] },
        ComponentOptionType = ComponentOptionType.APPROVAL_PENDING,
        CreatedBy = createdBy, UpdatedBy = createdBy, CreatedAt = now, UpdatedAt = now
    }
};

            #endregion

        }
    }
}