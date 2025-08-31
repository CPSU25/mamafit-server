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
                    Id = "aw9aa51bbd304e77933e24bbed65cd19",
                    UserName = "branchquan9",
                    UserEmail = "branchquan9@mamafit.com",
                    HashPassword = "u+7jaa75MJns9ST/6FSw4aQU+zSF/iNldvzpJxBFzwk=",
                    Salt = "R0kox04KXBTPFFlKjfxIzNeMzlbH1rGO/YJDtl0N894=",
                    PhoneNumber = "0909648632",
                    JobTitle = "Quản lý cửa hàng",
                    FullName = "Hữu Danh",
                    IsVerify = true,
                    RoleId = "bf081015e17a41b8b1cae65b1b17cfdb",
                },
                new ApplicationUser
                {
                    Id = "08ee8586464b43dd9a4507add95b281c",
                    UserName = "designer",
                    UserEmail = "designer@mamafit.com",
                    HashPassword = "u+7jaa75MJns9ST/6FSw4aQU+zSF/iNldvzpJxBFzwk=",
                    Salt = "R0kox04KXBTPFFlKjfxIzNeMzlbH1rGO/YJDtl0N894=",
                    PhoneNumber = "0909130392",
                    JobTitle = "Nhân viên thiết kế",
                    FullName = "Designer",
                    IsVerify = true,
                    RoleId = "e5b0f987fbf44608b7a6a2d0e313b3b2",
                },
                new ApplicationUser
                {
                    Id = "1a3bcd12345678901234567890123456",
                    UserName = "admin",
                    UserEmail = "admin@mamafit.com",
                    HashPassword = "u+7jaa75MJns9ST/6FSw4aQU+zSF/iNldvzpJxBFzwk=",
                    Salt = "R0kox04KXBTPFFlKjfxIzNeMzlbH1rGO/YJDtl0N894=",
                    PhoneNumber = "0909932698",
                    JobTitle = "Quản lý hệ thống",
                    FullName = "Admin",
                    IsVerify = true,
                    RoleId = "2e7b5a97e42e4e84a08ffbe0bc05d2ea",
                },
                new ApplicationUser
                {
                    Id = "29d72211a9f7480c9812d61ee17c92b9",
                    UserName = "branchquan1",
                    UserEmail = "branchquan1@mamafit.com",
                    HashPassword = "u+7jaa75MJns9ST/6FSw4aQU+zSF/iNldvzpJxBFzwk=",
                    Salt = "R0kox04KXBTPFFlKjfxIzNeMzlbH1rGO/YJDtl0N894=",
                    PhoneNumber = "0909446338",
                    JobTitle = "Quản lý cửa hàng",
                    FullName = "Phương Nam",
                    IsVerify = true,
                    RoleId = "bf081015e17a41b8b1cae65b1b17cfdb",
                },
                new ApplicationUser
                {
                    Id = "4c9804ecc1d645de96fcfc906cc43d6c",
                    UserName = "manager",
                    UserEmail = "manager@mamafit.com",
                    HashPassword = "u+7jaa75MJns9ST/6FSw4aQU+zSF/iNldvzpJxBFzwk=",
                    Salt = "R0kox04KXBTPFFlKjfxIzNeMzlbH1rGO/YJDtl0N894=",
                    PhoneNumber = "0909178846",
                    JobTitle = "Quản lý nhà xưởng",
                    FullName = "Manager",
                    IsVerify = true,
                    RoleId = "a3cb88edaf2b4718a9986010c5b9c1d7",
                },
                new ApplicationUser
                {
                    Id = "ce5235c40924fd5b0792732d3fb1b6f",
                    UserName = "vananh",
                    UserEmail = "vananh@mamafit.com",
                    HashPassword = "u+7jaa75MJns9ST/6FSw4aQU+zSF/iNldvzpJxBFzwk=",
                    Salt = "R0kox04KXBTPFFlKjfxIzNeMzlbH1rGO/YJDtl0N894=",
                    PhoneNumber = "0909219060",
                    FullName = "Van Anh",
                    JobTitle = "Nhân viên vận hành",
                    IsVerify = true,
                    RoleId = "5ed8cfa9b62d433c88ab097b6d2baccd",
                },
                new ApplicationUser
                {
                    Id = "5f6855ec48a84ad066b5bee371aa67fe",
                    UserName = "thanhtuan",
                    UserEmail = "thanhtuan@mamafit.com",
                    HashPassword = "u+7jaa75MJns9ST/6FSw4aQU+zSF/iNldvzpJxBFzwk=",
                    Salt = "R0kox04KXBTPFFlKjfxIzNeMzlbH1rGO/YJDtl0N894=",
                    PhoneNumber = "0909213321",
                    FullName = "Thanh Tuan",
                    JobTitle = "Nhân viên vận hành",
                    IsVerify = true,
                    RoleId = "5ed8cfa9b62d433c88ab097b6d2baccd",
                },
                new ApplicationUser
                {
                    Id = "617b57c35a46c0efbb263506d9321843",
                    UserName = "duccuong",
                    UserEmail = "duccuong@mamafit.com",
                    HashPassword = "u+7jaa75MJns9ST/6FSw4aQU+zSF/iNldvzpJxBFzwk=",
                    Salt = "R0kox04KXBTPFFlKjfxIzNeMzlbH1rGO/YJDtl0N894=",
                    PhoneNumber = "0909213321",
                    FullName = "Đức Cường",
                    JobTitle = "Nhân viên QC",
                    IsVerify = true,
                    RoleId = "5ed8cfa9b62d433c88ab097b6d2baccd",
                },
                new ApplicationUser
                {
                    Id = "f49aa51bbd304e77933e24bbed65b165",
                    UserName = "user",
                    UserEmail = "user@mamafit.com",
                    HashPassword = "u+7jaa75MJns9ST/6FSw4aQU+zSF/iNldvzpJxBFzwk=",
                    Salt = "R0kox04KXBTPFFlKjfxIzNeMzlbH1rGO/YJDtl0N894=",
                    PhoneNumber = "0909648736",
                    FullName = "User",
                    JobTitle = "",
                    IsVerify = true,
                    RoleId = "b8d237b8b6f849988d60c6c3c1d0a943",
                },
                new ApplicationUser
                {
                    Id = "po103fbe31e6449b9b92c89b5c23oa1x2",
                    UserName = "branchquan7",
                    UserEmail = "branchquan7@mamafit.com",
                    HashPassword = "u+7jaa75MJns9ST/6FSw4aQU+zSF/iNldvzpJxBFzwk=",
                    Salt = "R0kox04KXBTPFFlKjfxIzNeMzlbH1rGO/YJDtl0N894=",
                    PhoneNumber = "0909648721",
                    FullName = "Đức Anh",
                    JobTitle = "Quản lý cửa hàng",
                    IsVerify = true,
                    RoleId = "bf081015e17a41b8b1cae65b1b17cfdb",
                },
                new ApplicationUser
                {
                    Id = "ms1948fbai10380snalep19041nxbap1009",
                    UserName = "branchhanoi",
                    UserEmail = "branchhanoi@mamafit.com",
                    HashPassword = "u+7jaa75MJns9ST/6FSw4aQU+zSF/iNldvzpJxBFzwk=",
                    Salt = "R0kox04KXBTPFFlKjfxIzNeMzlbH1rGO/YJDtl0N894=",
                    PhoneNumber = "0909648722",
                    FullName = "Lương Thiện",
                    JobTitle = "Quản lý cửa hàng",
                    IsVerify = true,
                    RoleId = "bf081015e17a41b8b1cae65b1b17cfdb",
                },
                new ApplicationUser
                {
                    Id = "nnas1039jxoaopepmxsaoqjxba188290nci",
                    UserName = "branchbinhduong",
                    UserEmail = "branchbinhduong@mamafit.com",
                    HashPassword = "u+7jaa75MJns9ST/6FSw4aQU+zSF/iNldvzpJxBFzwk=",
                    Salt = "R0kox04KXBTPFFlKjfxIzNeMzlbH1rGO/YJDtl0N894=",
                    PhoneNumber = "0909648723",
                    FullName = "Van Tai",
                    JobTitle = "Quản lý cửa hàng",
                    IsVerify = true,
                    RoleId = "bf081015e17a41b8b1cae65b1b17cfdb",
                }
            );

            #region Seed Category, Style, and Component

            var now = DateTime.UtcNow;

            // ===== CATEGORIES =====
            modelBuilder.Entity<Category>().HasData(
                new Category
                {
                    Id = "04a3b452cfd841919b4aed099c28d709",
                    Name = "Work/Office",
                    Description = "Thiết kế trang nhã dành cho công sở.",
                    Images =
                    [
                        "https://firebasestorage.googleapis.com/v0/b/mamafit-e0138.firebasestorage.app/o/components%2Fwork-office.png?alt=media&token=8e85150b-0d2d-4855-ac2c-93a6e78c5fe8"
                    ],
                    Status = 0,
                    CreatedBy = "System",
                    UpdatedBy = "System",
                    CreatedAt = new DateTime(2025, 8, 4, 14, 5, 3, DateTimeKind.Utc),
                    UpdatedAt = new DateTime(2025, 8, 4, 14, 5, 3, DateTimeKind.Utc),
                    IsDeleted = false
                },
                new Category
                {
                    Id = "1946de6edbc24354bdd82b7c2c2c4cb2",
                    Name = "Party/Event",
                    Description = "Dành cho buổi tiệc hoặc sự kiện.",
                    Images =
                    [
                        "https://firebasestorage.googleapis.com/v0/b/mamafit-e0138.firebasestorage.app/o/components%2Fparty-special.png?alt=media&token=f4b44822-d3a5-4b54-879b-927fa841090e"
                    ],
                    Status = 0,
                    CreatedBy = "System",
                    UpdatedBy = "System",
                    CreatedAt = new DateTime(2025, 8, 4, 14, 5, 3, DateTimeKind.Utc),
                    UpdatedAt = new DateTime(2025, 8, 4, 14, 5, 3, DateTimeKind.Utc),
                    IsDeleted = false
                },
                new Category
                {
                    Id = "4f8c9fd3fb4548af8ca11c6521aa3b33",
                    Name = "Casual/Everyday",
                    Description = "Phong cách thường ngày, thoải mái.",
                    Images =
                    [
                        "https://firebasestorage.googleapis.com/v0/b/mamafit-e0138.firebasestorage.app/o/components%2Fcasual-everyday.png?alt=media&token=8a0ad31a-7de5-4cb0-843a-d1c0cb60227b"
                    ],
                    Status = 0,
                    CreatedBy = "System",
                    UpdatedBy = "System",
                    CreatedAt = new DateTime(2025, 8, 4, 14, 5, 3, DateTimeKind.Utc),
                    UpdatedAt = new DateTime(2025, 8, 4, 14, 5, 3, DateTimeKind.Utc),
                    IsDeleted = false
                },
                new Category
                {
                    Id = "a7a75e41c1a64b4498a81f4b76029a5a",
                    Name = "Beach/Resort",
                    Description = "Trang phục phù hợp khi đi biển hoặc nghỉ dưỡng.",
                    Images =
                    [
                        "https://firebasestorage.googleapis.com/v0/b/mamafit-e0138.firebasestorage.app/o/components%2Fbeach-resort.png?alt=media&token=72ffcbc5-2092-4e82-8603-afcad8a080c5"
                    ],
                    Status = 0,
                    CreatedBy = "System",
                    UpdatedBy = "System",
                    CreatedAt = new DateTime(2025, 8, 4, 14, 5, 3, DateTimeKind.Utc),
                    UpdatedAt = new DateTime(2025, 8, 4, 14, 5, 3, DateTimeKind.Utc),
                    IsDeleted = false
                }
            );

            // ===== STYLES =====
            modelBuilder.Entity<Style>().HasData(
                new Style
                {
                    Id = "11363158846e4478b326bf58a7ca9d21",
                    CategoryId = "04a3b452cfd841919b4aed099c28d709",
                    Name = "Shirt Dress",
                    IsCustom = true,
                    Description =
                        "Váy sơ mi dáng suông hoặc thắt nhẹ ở eo, thanh lịch và thoải mái khi đi làm, dễ phối với blazer hoặc cardigan.",
                    Images =
                    [
                        "https://firebasestorage.googleapis.com/v0/b/mamafit-e0138.firebasestorage.app/o/components%2Fsketch%2Fshirt-sketch.png?alt=media&token=4afb0550-e2ed-41e1-b3a6-48b1378f36ec"
                    ],
                    GlobalStatus = 0,
                    CreatedBy = "System",
                    UpdatedBy = "System",
                    CreatedAt = new DateTime(2025, 8, 20, 4, 3, 32, DateTimeKind.Utc),
                    UpdatedAt = new DateTime(2025, 8, 20, 4, 3, 32, DateTimeKind.Utc),
                    IsDeleted = false
                },
                new Style
                {
                    Id = "bd6ae7259f6d42fbac946b3319a92406",
                    CategoryId = "1946de6edbc24354bdd82b7c2c2c4cb2",
                    Name = "Halter Dress",
                    IsCustom = true,
                    Description =
                        "Váy cổ yếm buộc sau gáy, khoe vai và lưng tinh tế, hiện đại và quyến rũ, hợp tiệc tối.",
                    Images =
                    [
                    ],
                    GlobalStatus = 0,
                    CreatedBy = "System",
                    UpdatedBy = "System",
                    CreatedAt = new DateTime(2025, 8, 20, 4, 3, 32, DateTimeKind.Utc),
                    UpdatedAt = new DateTime(2025, 8, 20, 4, 3, 32, DateTimeKind.Utc),
                    IsDeleted = false
                },
                new Style
                {
                    Id = "f5d9785312d34110a0aaed96272b630a",
                    CategoryId = "1946de6edbc24354bdd82b7c2c2c4cb2",
                    Name = "Off-Shoulder",
                    IsCustom = true,
                    Description =
                        "Váy trễ vai nhẹ nhàng, tôn cổ và bờ vai, vừa nữ tính vừa sang trọng, lý tưởng cho dịp đặc biệt.",
                    Images =
                    [
                    ],
                    GlobalStatus = 0,
                    CreatedBy = "System",
                    UpdatedBy = "System",
                    CreatedAt = new DateTime(2025, 8, 20, 4, 3, 32, DateTimeKind.Utc),
                    UpdatedAt = new DateTime(2025, 8, 20, 4, 3, 32, DateTimeKind.Utc),
                    IsDeleted = false
                },
                new Style
                {
                    Id = "037b41302a4c453f8cef135afceb3956",
                    CategoryId = "a7a75e41c1a64b4498a81f4b76029a5a",
                    Name = "Maxi",
                    IsCustom = true,
                    Description =
                        "Đầm dài maxi, thích hợp đi biển hoặc dạo phố.",
                    Images =
                    [
                        "https://firebasestorage.googleapis.com/v0/b/mamafit-e0138.firebasestorage.app/o/components%2Fsketch%2Fmaxi-sketch.png?alt=media&token=00662628-1f51-4e04-9afc-ec6bfdd64ae9"
                    ],
                    GlobalStatus = 0,
                    CreatedBy = "System",
                    UpdatedBy = "System",
                    CreatedAt = new DateTime(2025, 8, 4, 14, 5, 3, DateTimeKind.Utc),
                    UpdatedAt = new DateTime(2025, 8, 4, 14, 5, 3, DateTimeKind.Utc),
                    IsDeleted = false
                },
                new Style
                {
                    Id = "5ed8aceab2d34bd8801e8c6a10dfc0d0",
                    CategoryId = "4f8c9fd3fb4548af8ca11c6521aa3b33",
                    Name = "T-Shirt Dress",
                    IsCustom = true,
                    Description =
                        "Váy dáng áo thun rộng rãi, thoải mái, năng động, dễ mặc hằng ngày.",
                    Images =
                    [
                        "https://firebasestorage.googleapis.com/v0/b/mamafit-e0138.firebasestorage.app/o/components%2Fsketch%2Ft-shirt-sketch.png?alt=media&token=a99e3be5-1508-4614-83c1-fe4af79fdc82"
                    ],
                    GlobalStatus = 0,
                    CreatedBy = "System",
                    UpdatedBy = "System",
                    CreatedAt = new DateTime(2025, 8, 20, 4, 3, 32, DateTimeKind.Utc),
                    UpdatedAt = new DateTime(2025, 8, 20, 4, 3, 32, DateTimeKind.Utc),
                    IsDeleted = false
                },
                new Style
                {
                    Id = "b10d38a099674087b125cf0dfe099f5a",
                    CategoryId = "4f8c9fd3fb4548af8ca11c6521aa3b33",
                    Name = "Nurse Dress",
                    IsCustom = true,
                    Description =
                        "Thiết kế tiện lợi với hàng nút hoặc khóa kéo trước, phù hợp cho mẹ bầu và sau sinh cần cho con bú.",
                    Images =
                    [
                        "https://firebasestorage.googleapis.com/v0/b/mamafit-e0138.firebasestorage.app/o/components%2Fsketch%2Fnurse-sketch.png?alt=media&token=3cf25db4-e5ab-436c-8643-721616a4ceba"
                    ],
                    GlobalStatus = 0,
                    CreatedBy = "System",
                    UpdatedBy = "System",
                    CreatedAt = new DateTime(2025, 8, 20, 4, 3, 32, DateTimeKind.Utc),
                    UpdatedAt = new DateTime(2025, 8, 20, 4, 3, 32, DateTimeKind.Utc),
                    IsDeleted = false
                }
            );

            // ===== COMPONENTS =====
            var components = new List<Component>
            {
                new Component
                {
                    Id = "0ec808c3046b4d368962c65eb771dea0",
                    Name = "Color",
                    Description = "Màu sắc.",
                    Images = new List<string> { "https://example.com/images/color.jpg" },
                    CreatedBy = "System", UpdatedBy = "System", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow
                },
                new Component
                {
                    Id = "3b83e5e47b444880aa9495b58b38a2fd",
                    Name = "Sleeves",
                    Description = "Kiểu tay áo.",
                    Images = new List<string> { "https://example.com/images/sleeves.jpg" },
                    CreatedBy = "System", UpdatedBy = "System", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow
                },
                new Component
                {
                    Id = "3e9f3d9beeff47f1977a6713cffe781e",
                    Name = "Waist",
                    Description = "Kiểu eo.",
                    Images = new List<string> { "https://example.com/images/waist.jpg" },
                    CreatedBy = "System", UpdatedBy = "System", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow
                },
                new Component
                {
                    Id = "b66ab857cd93410599bd14b79ae37147",
                    Name = "Neckline",
                    Description = "Kiểu dáng cổ áo.",
                    Images = new List<string> { "https://example.com/images/neckline.jpg" },
                    CreatedBy = "System", UpdatedBy = "System", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow
                },
                new Component
                {
                    Id = "c615996abfe24d3885ccea361a199dba",
                    Name = "Fabric",
                    Description = "Chất liệu vải.",
                    Images = new List<string> { "https://example.com/images/fabric.jpg" },
                    CreatedBy = "System", UpdatedBy = "System", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow
                },
                new Component
                {
                    Id = "f00fcaf79a86427c995daa1fd0915d8e",
                    Name = "Hem",
                    Description = "Đường viền gấu váy.",
                    Images = new List<string> { "https://example.com/images/hem.jpg" },
                    CreatedBy = "System", UpdatedBy = "System", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow
                }
            };

            modelBuilder.Entity<Component>().HasData(components);

            #region ComponentOption Seed
            modelBuilder.Entity<ComponentOption>().HasData(
                new ComponentOption
                {
                    Id = "06e6899bd6544dd3b9020a50789d9421",
                    ComponentId = "3e9f3d9beeff47f1977a6713cffe781e",
                    Name = "Normal",
                    Price = 0.0m,
                    Description = "Thiết kế eo trung bình truyền thống.",
                    Images = ["https://firebasestorage.googleapis.com/v0/b/mamafit-e0138.firebasestorage.app/o/components%2Fnormal-waist.png?alt=media&token=6b608665-6975-4993-abd7-c57106c913f8"],
                    GlobalStatus = 0,
                    CreatedBy = "System",
                    UpdatedBy = "System",
                    CreatedAt = DateTime.Parse("2025-08-04 21:05:03.000 +0700").ToUniversalTime(),
                    UpdatedAt = DateTime.Parse("2025-08-04 21:05:03.000 +0700").ToUniversalTime(),
                    IsDeleted = false,
                },
    new ComponentOption
    {
        Id = "15fecd17aa014779853f30532d260f47",
        ComponentId = "f00fcaf79a86427c995daa1fd0915d8e",
        Name = "Asymmetrical",
        Price = 0.0m,
        Description = "Gấu lệch, tạo dáng hiện đại.",
        Images = ["https://firebasestorage.googleapis.com/v0/b/mamafit-e0138.firebasestorage.app/o/components%2Fasymmetrical-hem.png?alt=media&token=6267ed98-6b96-41bf-b0eb-c0b1574b8b23"],
        GlobalStatus = 0,
        CreatedBy = "Admin",
        UpdatedBy = "System",
        CreatedAt = DateTime.Parse("2025-08-29 11:57:50.000 +0700").ToUniversalTime(),
        UpdatedAt = DateTime.Parse("2025-08-29 11:57:50.000 +0700").ToUniversalTime(),
        IsDeleted = false,
    },
    new ComponentOption
    {
        Id = "31c853d3dde94e68ab57903cf8a80798",
        ComponentId = "c615996abfe24d3885ccea361a199dba",
        Name = "Cotton",
        Price = 0.0m,
        Description = "Vải cotton thoáng mát, thấm hút tốt.",
        Images = ["https://firebasestorage.googleapis.com/v0/b/mamafit-e0138.firebasestorage.app/o/components%2Fcotton.png?alt=media&token=bdd471d7-c695-4c05-8770-ffb8eeb0f03e"],
        GlobalStatus = 0,
        CreatedBy = "System",
        UpdatedBy = "System",
        CreatedAt = DateTime.Parse("2025-08-04 21:05:03.000 +0700").ToUniversalTime(),
        UpdatedAt = DateTime.Parse("2025-08-04 21:05:03.000 +0700").ToUniversalTime(),
        IsDeleted = false,
    },
    new ComponentOption
    {
        Id = "3ca4d35ad1b341ed835ab225f9c2faa0",
        ComponentId = "3e9f3d9beeff47f1977a6713cffe781e",
        Name = "Peplum",
        Price = 0.0m,
        Description = "Eo xếp bèo nhẹ, tạo điểm nhấn và che bụng.",
        Images = ["https://firebasestorage.googleapis.com/v0/b/mamafit-e0138.firebasestorage.app/o/components%2Fpeplum-waist.png?alt=media&token=c4d57153-75e2-4bbd-be17-25de6e8a49b7"],
        GlobalStatus = 0,
        CreatedBy = "Admin",
        UpdatedBy = "System",
        CreatedAt = DateTime.Parse("2025-08-29 11:52:42.000 +0700").ToUniversalTime(),
        UpdatedAt = DateTime.Parse("2025-08-29 11:52:42.000 +0700").ToUniversalTime(),
        IsDeleted = false,
    },
    new ComponentOption
    {
        Id = "44ecb280dd8f4137af58024e0780e994",
        ComponentId = "b66ab857cd93410599bd14b79ae37147",
        Name = "Boat Neck",
        Price = 0.0m,
        Description = "Cổ ngang, hơi rộng, ôm sát vai.",
        Images = ["https://firebasestorage.googleapis.com/v0/b/mamafit-e0138.firebasestorage.app/o/components%2Fboat-neck.png?alt=media&token=3f2e448b-e252-4567-841f-b88531f378fc"],
        GlobalStatus = 0,
        CreatedBy = "Admin",
        UpdatedBy = "System",
        CreatedAt = DateTime.Parse("2025-08-29 11:53:50.000 +0700").ToUniversalTime(),
        UpdatedAt = DateTime.Parse("2025-08-29 11:53:50.000 +0700").ToUniversalTime(),
        IsDeleted = false,
    },
    new ComponentOption
    {
        Id = "4ed7cdc105ab44de81d333d63db0a222",
        ComponentId = "3b83e5e47b444880aa9495b58b38a2fd",
        Name = "No Sleeves",
        Price = 0.0m,
        Description = "Không tay, thoáng mát.",
        Images = ["https://firebasestorage.googleapis.com/v0/b/mamafit-e0138.firebasestorage.app/o/components%2Fno-sleeves.png?alt=media&token=222c148c-22b0-407d-acf9-03b91a793123"],
        GlobalStatus = 0,
        CreatedBy = "System",
        UpdatedBy = "System",
        CreatedAt = DateTime.Parse("2025-08-04 21:05:03.000 +0700").ToUniversalTime(),
        UpdatedAt = DateTime.Parse("2025-08-04 21:05:03.000 +0700").ToUniversalTime(),
        IsDeleted = false,
    },
    new ComponentOption
    {
        Id = "66645e2f8fa94d228d87450cc4a74706",
        ComponentId = "0ec808c3046b4d368962c65eb771dea0",
        Name = "Green",
        Price = 0.0m,
        Description = "Xanh ngọc quý phái, nổi bật.",
        Images = ["https://firebasestorage.googleapis.com/v0/b/mamafit-e0138.firebasestorage.app/o/components%2Femerald-green.png?alt=media&token=d63ad4cc-0037-4ca5-b71b-4d8eb394b6ba"],
        GlobalStatus = 0,
        CreatedBy = "System",
        UpdatedBy = "System",
        CreatedAt = DateTime.Parse("2025-08-04 21:05:03.000 +0700").ToUniversalTime(),
        UpdatedAt = DateTime.Parse("2025-08-04 21:05:03.000 +0700").ToUniversalTime(),
        IsDeleted = false,
    },
    new ComponentOption
    {
        Id = "6b81101144404b9ab306235861eedc12",
        ComponentId = "f00fcaf79a86427c995daa1fd0915d8e",
        Name = "Straight",
        Price = 0.0m,
        Description = "Đường gấu thẳng, đơn giản.",
        Images = ["https://firebasestorage.googleapis.com/v0/b/mamafit-e0138.firebasestorage.app/o/components%2Fstraight-hem.png?alt=media&token=4ead53a9-08f5-4362-872e-5f3b0272d220"],
        GlobalStatus = 0,
        CreatedBy = "Admin",
        UpdatedBy = "System",
        CreatedAt = DateTime.Parse("2025-08-29 11:59:34.000 +0700").ToUniversalTime(),
        UpdatedAt = DateTime.Parse("2025-08-29 11:59:34.000 +0700").ToUniversalTime(),
        IsDeleted = false,
    },
    new ComponentOption
    {
        Id = "6eb66f2427eb47a1a1fffc0dd3f89f6e",
        ComponentId = "3b83e5e47b444880aa9495b58b38a2fd",
        Name = "Short Sleeves",
        Price = 0.0m,
        Description = "Tay ngắn, gọn gàng.",
        Images = ["https://firebasestorage.googleapis.com/v0/b/mamafit-e0138.firebasestorage.app/o/components%2Fshort-sleeves.png?alt=media&token=04468aff-6280-41fc-bac7-203bb65ef698"],
        GlobalStatus = 0,
        CreatedBy = "Admin",
        UpdatedBy = "System",
        CreatedAt = DateTime.Parse("2025-08-29 11:44:53.000 +0700").ToUniversalTime(),
        UpdatedAt = DateTime.Parse("2025-08-29 11:44:53.000 +0700").ToUniversalTime(),
        IsDeleted = false,
    },
    new ComponentOption
    {
        Id = "70b69ec5ea074c0f8bf904b20b9c7884",
        ComponentId = "f00fcaf79a86427c995daa1fd0915d8e",
        Name = "High-Low",
        Price = 0.0m,
        Description = "Trước ngắn sau dài tạo nét lạ mắt.",
        Images = ["https://firebasestorage.googleapis.com/v0/b/mamafit-e0138.firebasestorage.app/o/components%2Fhigh-low-hem.png?alt=media&token=258a7314-654c-4a9a-8af7-90a5b23597e3"],
        GlobalStatus = 0,
        CreatedBy = "System",
        UpdatedBy = "System",
        CreatedAt = DateTime.Parse("2025-08-04 21:05:03.000 +0700").ToUniversalTime(),
        UpdatedAt = DateTime.Parse("2025-08-04 21:05:03.000 +0700").ToUniversalTime(),
        IsDeleted = false,
    },
    new ComponentOption
    {
        Id = "7186b0e8de2348fdbde87105c0f9abe3",
        ComponentId = "3e9f3d9beeff47f1977a6713cffe781e",
        Name = "Wrap",
        Price = 0.0m,
        Description = "Eo quấn chéo, buộc dây, dễ điều chỉnh và tiện lợi cho mẹ bầu.",
        Images = ["https://firebasestorage.googleapis.com/v0/b/mamafit-e0138.firebasestorage.app/o/components%2Fwrap-waist.png?alt=media&token=3153efa1-17f4-4f8d-8e51-e287ca43ab26"],
        GlobalStatus = 0,
        CreatedBy = "Admin",
        UpdatedBy = "System",
        CreatedAt = DateTime.Parse("2025-08-29 20:26:53.000 +0700").ToUniversalTime(),
        UpdatedAt = DateTime.Parse("2025-08-29 20:26:53.000 +0700").ToUniversalTime(),
        IsDeleted = false,
    },
    new ComponentOption
    {
        Id = "78b78938aefe4bc6b927ed3722bc5859",
        ComponentId = "3b83e5e47b444880aa9495b58b38a2fd",
        Name = "Strap",
        Price = 0.0m,
        Description = "Dây mảnh, nữ tính.",
        Images = ["https://firebasestorage.googleapis.com/v0/b/mamafit-e0138.firebasestorage.app/o/components%2Fspaghetti-strap.png?alt=media&token=db6fb73c-11e8-4b72-8a64-3f87c82eb34c"],
        GlobalStatus = 0,
        CreatedBy = "System",
        UpdatedBy = "System",
        CreatedAt = DateTime.Parse("2025-08-04 21:05:03.000 +0700").ToUniversalTime(),
        UpdatedAt = DateTime.Parse("2025-08-04 21:05:03.000 +0700").ToUniversalTime(),
        IsDeleted = false,
    },
    new ComponentOption
    {
        Id = "8158b8e5ef0340bdaf830fa8eb2e650d",
        ComponentId = "3e9f3d9beeff47f1977a6713cffe781e",
        Name = "Empire Waist",
        Price = 0.0m,
        Description = "Eo cao, phù hợp cho phong cách nữ tính.",
        Images = ["https://firebasestorage.googleapis.com/v0/b/mamafit-e0138.firebasestorage.app/o/components%2Fempire-waist.png?alt=media&token=292233b5-38d3-4b46-b543-6747d187a702"],
        GlobalStatus = 0,
        CreatedBy = "System",
        UpdatedBy = "System",
        CreatedAt = DateTime.Parse("2025-08-04 21:05:03.000 +0700").ToUniversalTime(),
        UpdatedAt = DateTime.Parse("2025-08-04 21:05:03.000 +0700").ToUniversalTime(),
        IsDeleted = false,
    },
    new ComponentOption
    {
        Id = "818f875df04a49eebe9c474bba4247b9",
        ComponentId = "b66ab857cd93410599bd14b79ae37147",
        Name = "V Neck",
        Price = 0.0m,
        Description = "Cổ chữ V tạo cảm giác cổ dài hơn.",
        Images = ["https://firebasestorage.googleapis.com/v0/b/mamafit-e0138.firebasestorage.app/o/components%2Fv-neck.png?alt=media&token=5418fe58-896e-4559-86ae-e2809c519963"],
        GlobalStatus = 0,
        CreatedBy = "System",
        UpdatedBy = "System",
        CreatedAt = DateTime.Parse("2025-08-04 21:05:03.000 +0700").ToUniversalTime(),
        UpdatedAt = DateTime.Parse("2025-08-04 21:05:03.000 +0700").ToUniversalTime(),
        IsDeleted = false,
    },
    new ComponentOption
    {
        Id = "81acc249e2664c72908014a28e1e72ef",
        ComponentId = "3e9f3d9beeff47f1977a6713cffe781e",
        Name = "Bow Tie",
        Price = 0.0m,
        Description = "Nơ thắt eo tạo điểm nhấn đáng yêu.",
        Images = ["https://firebasestorage.googleapis.com/v0/b/mamafit-e0138.firebasestorage.app/o/components%2Fbow-tie-waist.png?alt=media&token=43522a3e-848a-49d1-8e5a-a6610e619c21"],
        GlobalStatus = 0,
        CreatedBy = "System",
        UpdatedBy = "System",
        CreatedAt = DateTime.Parse("2025-08-04 21:05:03.000 +0700").ToUniversalTime(),
        UpdatedAt = DateTime.Parse("2025-08-04 21:05:03.000 +0700").ToUniversalTime(),
        IsDeleted = false,
    },
    new ComponentOption
    {
        Id = "8cff38afa8464ae191ba152b93438266",
        ComponentId = "f00fcaf79a86427c995daa1fd0915d8e",
        Name = "Single Layer",
        Price = 0.0m,
        Description = "Gấu váy đơn giản, thanh lịch.",
        Images = ["https://firebasestorage.googleapis.com/v0/b/mamafit-e0138.firebasestorage.app/o/components%2Fsingle-layer-hem.png?alt=media&token=bfd6b737-ca41-4a9d-9257-b35320fd63b4"],
        GlobalStatus = 0,
        CreatedBy = "System",
        UpdatedBy = "System",
        CreatedAt = DateTime.Parse("2025-08-04 21:05:03.000 +0700").ToUniversalTime(),
        UpdatedAt = DateTime.Parse("2025-08-04 21:05:03.000 +0700").ToUniversalTime(),
        IsDeleted = false,
    },
    new ComponentOption
    {
        Id = "92bdbbdbfd3b4dcf8256b68a66dfa35b",
        ComponentId = "b66ab857cd93410599bd14b79ae37147",
        Name = "Round",
        Price = 0.0m,
        Description = "Cổ tròn cơ bản, thanh lịch.",
        Images = ["https://firebasestorage.googleapis.com/v0/b/mamafit-e0138.firebasestorage.app/o/components%2Fround-neck.png?alt=media&token=1f8e06b6-1aa5-49fc-a05a-e88e29593710"],
        GlobalStatus = 0,
        CreatedBy = "System",
        UpdatedBy = "System",
        CreatedAt = DateTime.Parse("2025-08-04 21:05:03.000 +0700").ToUniversalTime(),
        UpdatedAt = DateTime.Parse("2025-08-04 21:05:03.000 +0700").ToUniversalTime(),
        IsDeleted = false,
    },
    new ComponentOption
    {
        Id = "9cc2f01b2fb34f21884b1365acd9bb48",
        ComponentId = "c615996abfe24d3885ccea361a199dba",
        Name = "Jersey Knit",
        Price = 0.0m,
        Description = "Vải thun co giãn, mềm mại, dễ vận động.",
        Images = ["https://firebasestorage.googleapis.com/v0/b/mamafit-e0138.firebasestorage.app/o/components%2Fjersey-knit.png?alt=media&token=d16f44cc-528e-49e5-a985-61a63c058d77"],
        GlobalStatus = 0,
        CreatedBy = "Admin",
        UpdatedBy = "System",
        CreatedAt = DateTime.Parse("2025-08-29 11:55:25.000 +0700").ToUniversalTime(),
        UpdatedAt = DateTime.Parse("2025-08-29 11:55:25.000 +0700").ToUniversalTime(),
        IsDeleted = false,
    },
    new ComponentOption
    {
        Id = "ac73834982664ae09fc86ba4359e4aa8",
        ComponentId = "c615996abfe24d3885ccea361a199dba",
        Name = "Linen",
        Price = 0.0m,
        Description = "Vải thoáng mát, nhẹ, thích hợp mùa hè.",
        Images = ["https://firebasestorage.googleapis.com/v0/b/mamafit-e0138.firebasestorage.app/o/components%2Flinen.png?alt=media&token=39c033ba-8e36-46d3-8bdf-da5fc16a63f7"],
        GlobalStatus = 0,
        CreatedBy = "Admin",
        UpdatedBy = "System",
        CreatedAt = DateTime.Parse("2025-08-29 11:55:15.000 +0700").ToUniversalTime(),
        UpdatedAt = DateTime.Parse("2025-08-29 11:55:15.000 +0700").ToUniversalTime(),
        IsDeleted = false,
    },
    new ComponentOption
    {
        Id = "c454b97a93f341e78b127d5425acc464",
        ComponentId = "0ec808c3046b4d368962c65eb771dea0",
        Name = "Red",
        Price = 0.0m,
        Description = "Màu đỏ nổi bật, quyến rũ.",
        Images = ["https://firebasestorage.googleapis.com/v0/b/mamafit-e0138.firebasestorage.app/o/components%2Fred.png?alt=media&token=b520d1e1-69b9-48bd-aa84-d3824897c0b8"],
        GlobalStatus = 0,
        CreatedBy = "System",
        UpdatedBy = "System",
        CreatedAt = DateTime.Parse("2025-08-04 21:05:03.000 +0700").ToUniversalTime(),
        UpdatedAt = DateTime.Parse("2025-08-04 21:05:03.000 +0700").ToUniversalTime(),
        IsDeleted = false,
    },
    new ComponentOption
    {
        Id = "d63a7d0c9e5d4e258285ded1974bf9cf",
        ComponentId = "0ec808c3046b4d368962c65eb771dea0",
        Name = "Black",
        Price = 0.0m,
        Description = "Đen cơ bản, thanh lịch.",
        Images = ["https://firebasestorage.googleapis.com/v0/b/mamafit-e0138.firebasestorage.app/o/components%2Fblack.png?alt=media&token=08e541fa-5ced-449c-bf7a-63da6ea78e2c"],
        GlobalStatus = 0,
        CreatedBy = "Admin",
        UpdatedBy = "System",
        CreatedAt = DateTime.Parse("2025-08-29 11:46:38.000 +0700").ToUniversalTime(),
        UpdatedAt = DateTime.Parse("2025-08-29 11:46:38.000 +0700").ToUniversalTime(),
        IsDeleted = false,
    },
    new ComponentOption
    {
        Id = "e2c03da33b12458694a1b01c3ec66474",
        ComponentId = "0ec808c3046b4d368962c65eb771dea0",
        Name = "Navy",
        Price = 0.0m,
        Description = "Xanh navy lịch sự, thanh nhã.",
        Images = ["https://firebasestorage.googleapis.com/v0/b/mamafit-e0138.firebasestorage.app/o/components%2Fnavy-blue.png?alt=media&token=94e2c88c-a38c-4294-8b35-027b255e26ca"],
        GlobalStatus = 0,
        CreatedBy = "System",
        UpdatedBy = "System",
        CreatedAt = DateTime.Parse("2025-08-04 21:05:03.000 +0700").ToUniversalTime(),
        UpdatedAt = DateTime.Parse("2025-08-04 21:05:03.000 +0700").ToUniversalTime(),
        IsDeleted = false,
    },
    new ComponentOption
    {
        Id = "fecd26fe3b6b4e88ba97ebb406fa6df2",
        ComponentId = "3b83e5e47b444880aa9495b58b38a2fd",
        Name = "Flattered",
        Price = 0.0m,
        Description = "Tay ngắn bay bổng, thoải mái.",
        Images = ["https://firebasestorage.googleapis.com/v0/b/mamafit-e0138.firebasestorage.app/o/components%2Fflattered-sleeves.png?alt=media&token=93a07830-1514-4a65-80ca-599c67cd7334"],
        GlobalStatus = 0,
        CreatedBy = "System",
        UpdatedBy = "System",
        CreatedAt = DateTime.Parse("2025-08-04 21:05:03.000 +0700").ToUniversalTime(),
        UpdatedAt = DateTime.Parse("2025-08-04 21:05:03.000 +0700").ToUniversalTime(),
        IsDeleted = false,
    },
    new ComponentOption
    {
        Id = "e639e41ce412489cbd74fbf43428344d",
        ComponentId = "b66ab857cd93410599bd14b79ae37147",
        Name = "Shirt Collar",
        Price = 0.0m,
        Description = "Cổ áo sơ mi thanh lịch, tinh tế.",
        Images = ["https://firebasestorage.googleapis.com/v0/b/mamafit-e0138.firebasestorage.app/o/components%2Fshirt-collar.png?alt=media&token=bd88f9a8-61bb-4aad-85b3-97d5e0bb37c6"],
        GlobalStatus = 0,
        CreatedBy = "Admin",
        UpdatedBy = "System",
        CreatedAt = DateTime.Parse("2025-08-30 22:47:06.920 +0700").ToUniversalTime(),
        UpdatedAt = DateTime.Parse("2025-08-30 22:47:06.919 +0700").ToUniversalTime(),
        IsDeleted = false,
    }
);
            #endregion

            #region Preset Seed
            modelBuilder.Entity<Preset>().HasData(
                            new Preset
                            {
                                Id = "11230115583c4994bfaa3925951d3817",
                                Name = "Name",
                                UserId = "f49aa51bbd304e77933e24bbed65b165",
                                StyleId = "037b41302a4c453f8cef135afceb3956",
                                SKU = "P00005",
                                Images = ["https://firebasestorage.googleapis.com/v0/b/mamafit-e0138.firebasestorage.app/o/presets%2Fmaxi%2Fmaxi-19.png?alt=media&token=053f4feb-7e75-4645-b8aa-e0e0b1fae0b3"],
                                IsDefault = false,
                                Weight = 200,
                                Price = 2000.0m,
                                Type = 0,
                                CreatedBy = "User",
                                UpdatedBy = "System",
                                CreatedAt = DateTime.Parse("2025-08-30 12:56:25.590 +0700").ToUniversalTime(),
                                UpdatedAt = DateTime.Parse("2025-08-30 12:56:25.590 +0700").ToUniversalTime(),
                                IsDeleted = false,
                            },
                new Preset
                {
                    Id = "1453c4e8983c4a3397deecaccf1230c1",
                    Name = "Name",
                    UserId = "4c9804ecc1d645de96fcfc906cc43d6c",
                    StyleId = "5ed8aceab2d34bd8801e8c6a10dfc0d0",
                    SKU = "P27814",
                    Images = ["https://firebasestorage.googleapis.com/v0/b/mamafit-e0138.firebasestorage.app/o/images%2F5_1756533760956_6yighu.png?alt=media&token=5b241827-624f-434c-9399-9556893e98e8"],
                    IsDefault = false,
                    Price = 2000.0m,
                    Type = 0,
                    CreatedBy = "Manager",
                    CreatedAt = DateTime.Parse("2025-08-30 13:03:05.443 +0700").ToUniversalTime(),
                    UpdatedAt = DateTime.Parse("2025-08-30 13:03:05.427 +0700").ToUniversalTime(),
                    IsDeleted = false,
                },
                new Preset
                {
                    Id = "1acd7ac56e5c4d15895f03cc2d6e7723",
                    Name = "Name",
                    UserId = "f49aa51bbd304e77933e24bbed65b165",
                    StyleId = "037b41302a4c453f8cef135afceb3956",
                    SKU = "P00027",
                    Images = ["https://firebasestorage.googleapis.com/v0/b/mamafit-e0138.firebasestorage.app/o/presets%2Fmaxi%2Fmaxi-31.png?alt=media&token=97b8fadf-6bf1-4ab7-995a-5126f7fbfc65"],
                    IsDefault = false,
                    Weight = 200,
                    Price = 2000.0m,
                    Type = 0,
                    CreatedBy = "User",
                    UpdatedBy = "System",
                    CreatedAt = DateTime.Parse("2025-08-30 12:56:25.590 +0700").ToUniversalTime(),
                    UpdatedAt = DateTime.Parse("2025-08-30 12:56:25.590 +0700").ToUniversalTime(),
                    IsDeleted = false,
                },
                new Preset
                {
                    Id = "1f55382a1ffa42be9f046634d90b9f97",
                    Name = "Name",
                    UserId = "4c9804ecc1d645de96fcfc906cc43d6c",
                    StyleId = "b10d38a099674087b125cf0dfe099f5a",
                    SKU = "P08902",
                    Images = ["https://firebasestorage.googleapis.com/v0/b/mamafit-e0138.firebasestorage.app/o/images%2F2_1756534037761_mo4ug4.png?alt=media&token=47295ccb-b880-4518-aaad-e1b940afca2a"],
                    IsDefault = false,
                    Price = 2000.0m,
                    Type = 0,
                    CreatedBy = "Manager",
                    CreatedAt = DateTime.Parse("2025-08-30 13:07:42.563 +0700").ToUniversalTime(),
                    UpdatedAt = DateTime.Parse("2025-08-30 13:07:42.549 +0700").ToUniversalTime(),
                    IsDeleted = false,
                },
                new Preset
                {
                    Id = "21b2065adaf44984b32cd6b9eb75c2f7",
                    Name = "Name",
                    UserId = "4c9804ecc1d645de96fcfc906cc43d6c",
                    StyleId = "b10d38a099674087b125cf0dfe099f5a",
                    SKU = "P76993",
                    Images = ["https://firebasestorage.googleapis.com/v0/b/mamafit-e0138.firebasestorage.app/o/images%2F5_1756534347472_dby3ui.png?alt=media&token=7025a677-e2ba-4530-9c5b-1c4e3b4386de"],
                    IsDefault = false,
                    Price = 2000.0m,
                    Type = 0,
                    CreatedBy = "Manager",
                    CreatedAt = DateTime.Parse("2025-08-30 13:12:38.904 +0700").ToUniversalTime(),
                    UpdatedAt = DateTime.Parse("2025-08-30 13:12:38.895 +0700").ToUniversalTime(),
                    IsDeleted = false,
                },
                new Preset
                {
                    Id = "220bff408b7e465eb4652da38241c7a2",
                    Name = "Name",
                    UserId = "4c9804ecc1d645de96fcfc906cc43d6c",
                    StyleId = "b10d38a099674087b125cf0dfe099f5a",
                    SKU = "P47695",
                    Images = ["https://firebasestorage.googleapis.com/v0/b/mamafit-e0138.firebasestorage.app/o/images%2F1_1756533995037_rmpl8f.png?alt=media&token=921d7c58-edc6-4126-80e3-da79b82e4e4e"],
                    IsDefault = true,
                    Price = 2000.0m,
                    Type = 0,
                    CreatedBy = "Manager",
                    CreatedAt = DateTime.Parse("2025-08-30 13:07:04.474 +0700").ToUniversalTime(),
                    UpdatedAt = DateTime.Parse("2025-08-30 13:07:04.457 +0700").ToUniversalTime(),
                    IsDeleted = false,
                },
                new Preset
                {
                    Id = "2b9900f612414d2f80531a48275927aa",
                    Name = "Name",
                    UserId = "4c9804ecc1d645de96fcfc906cc43d6c",
                    StyleId = "5ed8aceab2d34bd8801e8c6a10dfc0d0",
                    SKU = "P97701",
                    Images = ["https://firebasestorage.googleapis.com/v0/b/mamafit-e0138.firebasestorage.app/o/images%2F2_1756533623846_6edofr.png?alt=media&token=67e03656-ec4e-4674-ae0a-099e419c3355"],
                    IsDefault = false,
                    Price = 2000.0m,
                    Type = 0,
                    CreatedBy = "Manager",
                    CreatedAt = DateTime.Parse("2025-08-30 13:00:46.623 +0700").ToUniversalTime(),
                    UpdatedAt = DateTime.Parse("2025-08-30 13:00:46.605 +0700").ToUniversalTime(),
                    IsDeleted = false,
                },
                new Preset
                {
                    Id = "33c8177630a54906b92c0b8c03c57450",
                    Name = "Name",
                    UserId = "f49aa51bbd304e77933e24bbed65b165",
                    StyleId = "037b41302a4c453f8cef135afceb3956",
                    SKU = "P00003",
                    Images = ["https://firebasestorage.googleapis.com/v0/b/mamafit-e0138.firebasestorage.app/o/presets%2Fmaxi%2Fmaxi-18.png?alt=media&token=fe931377-e708-4586-b897-4bdef49a0d5f"],
                    IsDefault = false,
                    Weight = 200,
                    Price = 2000.0m,
                    Type = 0,
                    CreatedBy = "User",
                    UpdatedBy = "System",
                    CreatedAt = DateTime.Parse("2025-08-30 12:56:25.590 +0700").ToUniversalTime(),
                    UpdatedAt = DateTime.Parse("2025-08-30 12:56:25.590 +0700").ToUniversalTime(),
                    IsDeleted = false,
                },
                new Preset
                {
                    Id = "3583422ceef647d28a14b82680058ff0",
                    Name = "Name",
                    UserId = "f49aa51bbd304e77933e24bbed65b165",
                    StyleId = "037b41302a4c453f8cef135afceb3956",
                    SKU = "P00009",
                    Images = ["https://firebasestorage.googleapis.com/v0/b/mamafit-e0138.firebasestorage.app/o/presets%2Fmaxi%2Fmaxi-13.png?alt=media&token=fdf74485-b0af-4c32-b7d3-cc78beb9dcd7"],
                    IsDefault = false,
                    Weight = 200,
                    Price = 2000.0m,
                    Type = 0,
                    CreatedBy = "User",
                    UpdatedBy = "System",
                    CreatedAt = DateTime.Parse("2025-08-30 12:56:25.590 +0700").ToUniversalTime(),
                    UpdatedAt = DateTime.Parse("2025-08-30 12:56:25.590 +0700").ToUniversalTime(),
                    IsDeleted = false,
                },
                new Preset
                {
                    Id = "380e3d771eac4488ad94b09c9e77ccd2",
                    Name = "Name",
                    UserId = "f49aa51bbd304e77933e24bbed65b165",
                    StyleId = "037b41302a4c453f8cef135afceb3956",
                    SKU = "P00026",
                    Images = ["https://firebasestorage.googleapis.com/v0/b/mamafit-e0138.firebasestorage.app/o/presets%2Fmaxi%2Fmaxi-32.png?alt=media&token=d9268430-d725-4813-a70f-260ee49cd1c5"],
                    IsDefault = false,
                    Weight = 200,
                    Price = 2000.0m,
                    Type = 0,
                    CreatedBy = "User",
                    UpdatedBy = "System",
                    CreatedAt = DateTime.Parse("2025-08-30 12:56:25.590 +0700").ToUniversalTime(),
                    UpdatedAt = DateTime.Parse("2025-08-30 12:56:25.590 +0700").ToUniversalTime(),
                    IsDeleted = false,
                },
                new Preset
                {
                    Id = "3d0b5e4af6114f659e6c4457bae78782",
                    Name = "Name",
                    UserId = "4c9804ecc1d645de96fcfc906cc43d6c",
                    StyleId = "b10d38a099674087b125cf0dfe099f5a",
                    SKU = "P23653",
                    Images = ["https://firebasestorage.googleapis.com/v0/b/mamafit-e0138.firebasestorage.app/o/images%2F9_1756534458495_tj8z04.png?alt=media&token=9baf6664-59ad-4ffb-b7d5-a2255b45a070"],
                    IsDefault = false,
                    Price = 2000.0m,
                    Type = 0,
                    CreatedBy = "Manager",
                    CreatedAt = DateTime.Parse("2025-08-30 13:14:57.621 +0700").ToUniversalTime(),
                    UpdatedAt = DateTime.Parse("2025-08-30 13:14:57.607 +0700").ToUniversalTime(),
                    IsDeleted = false,
                },
                new Preset
                {
                    Id = "41e4b32655a44dca862141df8099f646",
                    Name = "Name",
                    UserId = "f49aa51bbd304e77933e24bbed65b165",
                    StyleId = "037b41302a4c453f8cef135afceb3956",
                    SKU = "P00006",
                    Images = ["https://firebasestorage.googleapis.com/v0/b/mamafit-e0138.firebasestorage.app/o/presets%2Fmaxi%2Fmaxi-16.png?alt=media&token=1d05c179-85bd-444e-9056-be2509352f8c"],
                    IsDefault = false,
                    Weight = 200,
                    Price = 2000.0m,
                    Type = 0,
                    CreatedBy = "User",
                    UpdatedBy = "System",
                    CreatedAt = DateTime.Parse("2025-08-30 12:56:25.590 +0700").ToUniversalTime(),
                    UpdatedAt = DateTime.Parse("2025-08-30 12:56:25.590 +0700").ToUniversalTime(),
                    IsDeleted = false,
                },
                new Preset
                {
                    Id = "440dfdc24b064c9babc883547439de24",
                    Name = "Name",
                    UserId = "4c9804ecc1d645de96fcfc906cc43d6c",
                    StyleId = "5ed8aceab2d34bd8801e8c6a10dfc0d0",
                    SKU = "P37612",
                    Images = ["https://firebasestorage.googleapis.com/v0/b/mamafit-e0138.firebasestorage.app/o/images%2F6_1756533821543_mr824m.png?alt=media&token=3f027290-4b53-41f0-a67f-3d4e113cb9ce"],
                    IsDefault = false,
                    Price = 2000.0m,
                    Type = 0,
                    CreatedBy = "Manager",
                    CreatedAt = DateTime.Parse("2025-08-30 13:03:55.690 +0700").ToUniversalTime(),
                    UpdatedAt = DateTime.Parse("2025-08-30 13:03:55.672 +0700").ToUniversalTime(),
                    IsDeleted = false,
                },
                new Preset
                {
                    Id = "45f43125d239423893e337915ccaaea3",
                    Name = "Name",
                    UserId = "f49aa51bbd304e77933e24bbed65b165",
                    StyleId = "037b41302a4c453f8cef135afceb3956",
                    SKU = "P00017",
                    Images = ["https://firebasestorage.googleapis.com/v0/b/mamafit-e0138.firebasestorage.app/o/presets%2Fmaxi%2Fmaxi-22.png?alt=media&token=0d80783e-3267-4743-a2b1-f931a48716fd"],
                    IsDefault = false,
                    Weight = 200,
                    Price = 2000.0m,
                    Type = 0,
                    CreatedBy = "User",
                    UpdatedBy = "System",
                    CreatedAt = DateTime.Parse("2025-08-30 12:56:25.590 +0700").ToUniversalTime(),
                    UpdatedAt = DateTime.Parse("2025-08-30 12:56:25.590 +0700").ToUniversalTime(),
                    IsDeleted = false,
                },
                new Preset
                {
                    Id = "49e11d4698f643dca14364eed4d9c239",
                    Name = "Name",
                    UserId = "f49aa51bbd304e77933e24bbed65b165",
                    StyleId = "037b41302a4c453f8cef135afceb3956",
                    SKU = "P00012",
                    Images = ["https://firebasestorage.googleapis.com/v0/b/mamafit-e0138.firebasestorage.app/o/presets%2Fmaxi%2Fmaxi-28.png?alt=media&token=6276c8c2-679d-48fc-a4ec-214ce17fe3b9"],
                    IsDefault = false,
                    Weight = 200,
                    Price = 2000.0m,
                    Type = 0,
                    CreatedBy = "User",
                    UpdatedBy = "System",
                    CreatedAt = DateTime.Parse("2025-08-30 12:56:25.590 +0700").ToUniversalTime(),
                    UpdatedAt = DateTime.Parse("2025-08-30 12:56:25.590 +0700").ToUniversalTime(),
                    IsDeleted = false,
                },
                new Preset
                {
                    Id = "4f4d8b91705f4a9bb95ecf18b57b0306",
                    Name = "Name",
                    UserId = "f49aa51bbd304e77933e24bbed65b165",
                    StyleId = "037b41302a4c453f8cef135afceb3956",
                    SKU = "P00021",
                    Images = ["https://firebasestorage.googleapis.com/v0/b/mamafit-e0138.firebasestorage.app/o/presets%2Fmaxi%2Fmaxi-2.png?alt=media&token=f6cf6663-58a2-4b43-9c8a-46796b1dd8e1"],
                    IsDefault = false,
                    Weight = 200,
                    Price = 2000.0m,
                    Type = 0,
                    CreatedBy = "User",
                    UpdatedBy = "System",
                    CreatedAt = DateTime.Parse("2025-08-30 12:56:25.590 +0700").ToUniversalTime(),
                    UpdatedAt = DateTime.Parse("2025-08-30 12:56:25.590 +0700").ToUniversalTime(),
                    IsDeleted = false,
                },
                new Preset
                {
                    Id = "50959c7c201e45a9b5255558ac9dfcf7",
                    Name = "Name",
                    UserId = "f49aa51bbd304e77933e24bbed65b165",
                    StyleId = "037b41302a4c453f8cef135afceb3956",
                    SKU = "P00028",
                    Images = ["https://firebasestorage.googleapis.com/v0/b/mamafit-e0138.firebasestorage.app/o/presets%2Fmaxi%2Fmaxi-30.png?alt=media&token=373eb84b-cd7b-4e81-840d-516781ae9d72"],
                    IsDefault = false,
                    Weight = 200,
                    Price = 2000.0m,
                    Type = 0,
                    CreatedBy = "User",
                    UpdatedBy = "System",
                    CreatedAt = DateTime.Parse("2025-08-30 12:56:25.590 +0700").ToUniversalTime(),
                    UpdatedAt = DateTime.Parse("2025-08-30 12:56:25.590 +0700").ToUniversalTime(),
                    IsDeleted = false,
                },
                new Preset
                {
                    Id = "58d759c12387490fb3a97c4633d6f1bb",
                    Name = "Name",
                    UserId = "f49aa51bbd304e77933e24bbed65b165",
                    StyleId = "037b41302a4c453f8cef135afceb3956",
                    SKU = "P00020",
                    Images = ["https://firebasestorage.googleapis.com/v0/b/mamafit-e0138.firebasestorage.app/o/presets%2Fmaxi%2Fmaxi-10.png?alt=media&token=8fb01fe1-9ff2-4d7a-bf84-e8e1093f0f9b"],
                    IsDefault = false,
                    Weight = 200,
                    Price = 2000.0m,
                    Type = 0,
                    CreatedBy = "User",
                    UpdatedBy = "System",
                    CreatedAt = DateTime.Parse("2025-08-30 12:56:25.590 +0700").ToUniversalTime(),
                    UpdatedAt = DateTime.Parse("2025-08-30 12:56:25.590 +0700").ToUniversalTime(),
                    IsDeleted = false,
                },
                new Preset
                {
                    Id = "5a5081ce720141188c09336e4a936ef9",
                    Name = "Name",
                    UserId = "f49aa51bbd304e77933e24bbed65b165",
                    StyleId = "037b41302a4c453f8cef135afceb3956",
                    SKU = "P00029",
                    Images = ["https://firebasestorage.googleapis.com/v0/b/mamafit-e0138.firebasestorage.app/o/presets%2Fmaxi%2Fmaxi-3.png?alt=media&token=149e97e6-d906-4345-bfcb-dfc3d619a73b"],
                    IsDefault = false,
                    Weight = 200,
                    Price = 2000.0m,
                    Type = 0,
                    CreatedBy = "User",
                    UpdatedBy = "System",
                    CreatedAt = DateTime.Parse("2025-08-30 12:56:25.590 +0700").ToUniversalTime(),
                    UpdatedAt = DateTime.Parse("2025-08-30 12:56:25.590 +0700").ToUniversalTime(),
                    IsDeleted = false,
                },
                new Preset
                {
                    Id = "5ef79dda17e64808afa6287c07c8bac9",
                    Name = "Name",
                    UserId = "f49aa51bbd304e77933e24bbed65b165",
                    StyleId = "037b41302a4c453f8cef135afceb3956",
                    SKU = "P00008",
                    Images = ["https://firebasestorage.googleapis.com/v0/b/mamafit-e0138.firebasestorage.app/o/presets%2Fmaxi%2Fmaxi-14.png?alt=media&token=77ec4929-09f7-427c-9714-9f287ac89f4c"],
                    IsDefault = false,
                    Weight = 200,
                    Price = 2000.0m,
                    Type = 0,
                    CreatedBy = "User",
                    UpdatedBy = "System",
                    CreatedAt = DateTime.Parse("2025-08-30 12:56:25.590 +0700").ToUniversalTime(),
                    UpdatedAt = DateTime.Parse("2025-08-30 12:56:25.590 +0700").ToUniversalTime(),
                    IsDeleted = false,
                },
                new Preset
                {
                    Id = "68ee38a6554f407abd932d492719a834",
                    Name = "Name",
                    UserId = "f49aa51bbd304e77933e24bbed65b165",
                    StyleId = "037b41302a4c453f8cef135afceb3956",
                    SKU = "P00034",
                    Images = ["https://firebasestorage.googleapis.com/v0/b/mamafit-e0138.firebasestorage.app/o/presets%2Fmaxi%2Fmaxi-6.png?alt=media&token=28833b6c-ff85-4eb2-8f85-65e9ea4d8d23"],
                    IsDefault = false,
                    Weight = 200,
                    Price = 2000.0m,
                    Type = 0,
                    CreatedBy = "User",
                    UpdatedBy = "System",
                    CreatedAt = DateTime.Parse("2025-08-30 12:56:25.590 +0700").ToUniversalTime(),
                    UpdatedAt = DateTime.Parse("2025-08-30 12:56:25.590 +0700").ToUniversalTime(),
                    IsDeleted = false,
                },
                new Preset
                {
                    Id = "68fca8f086454898ae81b1ea29890b60",
                    Name = "Name",
                    UserId = "f49aa51bbd304e77933e24bbed65b165",
                    StyleId = "037b41302a4c453f8cef135afceb3956",
                    SKU = "P00036",
                    Images = ["https://firebasestorage.googleapis.com/v0/b/mamafit-e0138.firebasestorage.app/o/presets%2Fmaxi%2Fmaxi-4.png?alt=media&token=ea8426c2-8bac-4b0d-bff3-e7e2536ac351"],
                    IsDefault = false,
                    Weight = 200,
                    Price = 2000.0m,
                    Type = 0,
                    CreatedBy = "User",
                    UpdatedBy = "System",
                    CreatedAt = DateTime.Parse("2025-08-30 12:56:25.590 +0700").ToUniversalTime(),
                    UpdatedAt = DateTime.Parse("2025-08-30 12:56:25.590 +0700").ToUniversalTime(),
                    IsDeleted = false,
                },
                new Preset
                {
                    Id = "694d8135f4ca4238956e9f47460b5dff",
                    Name = "Name",
                    UserId = "f49aa51bbd304e77933e24bbed65b165",
                    StyleId = "037b41302a4c453f8cef135afceb3956",
                    SKU = "P00022",
                    Images = ["https://firebasestorage.googleapis.com/v0/b/mamafit-e0138.firebasestorage.app/o/presets%2Fmaxi%2Fmaxi-36.png?alt=media&token=dfa8808c-c87d-4ae0-be6b-1108343320ac"],
                    IsDefault = false,
                    Weight = 200,
                    Price = 2000.0m,
                    Type = 0,
                    CreatedBy = "User",
                    UpdatedBy = "System",
                    CreatedAt = DateTime.Parse("2025-08-30 12:56:25.590 +0700").ToUniversalTime(),
                    UpdatedAt = DateTime.Parse("2025-08-30 12:56:25.590 +0700").ToUniversalTime(),
                    IsDeleted = false,
                },
                new Preset
                {
                    Id = "6ab0daed7fe54e9fa791bfcb6bf1a043",
                    Name = "Name",
                    UserId = "f49aa51bbd304e77933e24bbed65b165",
                    StyleId = "037b41302a4c453f8cef135afceb3956",
                    SKU = "P00010",
                    Images = ["https://firebasestorage.googleapis.com/v0/b/mamafit-e0138.firebasestorage.app/o/presets%2Fmaxi%2Fmaxi-12.png?alt=media&token=72e10ea4-1d5c-4f50-b614-b8c56eed7cfb"],
                    IsDefault = false,
                    Weight = 200,
                    Price = 2000.0m,
                    Type = 0,
                    CreatedBy = "User",
                    UpdatedBy = "System",
                    CreatedAt = DateTime.Parse("2025-08-30 12:56:25.590 +0700").ToUniversalTime(),
                    UpdatedAt = DateTime.Parse("2025-08-30 12:56:25.590 +0700").ToUniversalTime(),
                    IsDeleted = false,
                },
                new Preset
                {
                    Id = "6fd08876dd804c01983236cb5277b406",
                    Name = "Name",
                    UserId = "f49aa51bbd304e77933e24bbed65b165",
                    StyleId = "037b41302a4c453f8cef135afceb3956",
                    SKU = "P00013",
                    Images = ["https://firebasestorage.googleapis.com/v0/b/mamafit-e0138.firebasestorage.app/o/presets%2Fmaxi%2Fmaxi-27.png?alt=media&token=c80d1ad8-5a45-4d8d-8e77-558ec415f728"],
                    IsDefault = false,
                    Weight = 200,
                    Price = 2000.0m,
                    Type = 0,
                    CreatedBy = "User",
                    UpdatedBy = "System",
                    CreatedAt = DateTime.Parse("2025-08-30 12:56:25.590 +0700").ToUniversalTime(),
                    UpdatedAt = DateTime.Parse("2025-08-30 12:56:25.590 +0700").ToUniversalTime(),
                    IsDeleted = false,
                },
                new Preset
                {
                    Id = "7abc9feb7c1e47778ec32b24aac95985",
                    Name = "Name",
                    UserId = "f49aa51bbd304e77933e24bbed65b165",
                    StyleId = "037b41302a4c453f8cef135afceb3956",
                    SKU = "P00025",
                    Images = ["https://firebasestorage.googleapis.com/v0/b/mamafit-e0138.firebasestorage.app/o/presets%2Fmaxi%2Fmaxi-33.png?alt=media&token=5f0263e2-84b2-4260-9c25-ca2b12411fc2"],
                    IsDefault = false,
                    Weight = 200,
                    Price = 2000.0m,
                    Type = 0,
                    CreatedBy = "User",
                    UpdatedBy = "System",
                    CreatedAt = DateTime.Parse("2025-08-30 12:56:25.590 +0700").ToUniversalTime(),
                    UpdatedAt = DateTime.Parse("2025-08-30 12:56:25.590 +0700").ToUniversalTime(),
                    IsDeleted = false,
                },
                new Preset
                {
                    Id = "7ae7818fdf09466f819808beed30e707",
                    Name = "Name",
                    UserId = "f49aa51bbd304e77933e24bbed65b165",
                    StyleId = "037b41302a4c453f8cef135afceb3956",
                    SKU = "P00002",
                    Images = ["https://firebasestorage.googleapis.com/v0/b/mamafit-e0138.firebasestorage.app/o/presets%2Fmaxi%2Fmaxi-11.png?alt=media&token=14bc30f9-fa14-41ae-8bfa-cc4b94acda1b"],
                    IsDefault = false,
                    Weight = 200,
                    Price = 2000.0m,
                    Type = 0,
                    CreatedBy = "User",
                    UpdatedBy = "System",
                    CreatedAt = DateTime.Parse("2025-08-30 12:56:25.590 +0700").ToUniversalTime(),
                    UpdatedAt = DateTime.Parse("2025-08-30 12:56:25.590 +0700").ToUniversalTime(),
                    IsDeleted = false,
                },
                new Preset
                {
                    Id = "7c2f591168f144d199a8d9e174addc45",
                    Name = "Name",
                    UserId = "f49aa51bbd304e77933e24bbed65b165",
                    StyleId = "037b41302a4c453f8cef135afceb3956",
                    SKU = "P00018",
                    Images = ["https://firebasestorage.googleapis.com/v0/b/mamafit-e0138.firebasestorage.app/o/presets%2Fmaxi%2Fmaxi-21.png?alt=media&token=13248af4-08bf-454c-9b30-6c741dbef82b"],
                    IsDefault = false,
                    Weight = 200,
                    Price = 2000.0m,
                    Type = 0,
                    CreatedBy = "User",
                    UpdatedBy = "System",
                    CreatedAt = DateTime.Parse("2025-08-30 12:56:25.590 +0700").ToUniversalTime(),
                    UpdatedAt = DateTime.Parse("2025-08-30 12:56:25.590 +0700").ToUniversalTime(),
                    IsDeleted = false,
                },
                new Preset
                {
                    Id = "7faa6eb739d548bf9f9ce7d3de978d3d",
                    Name = "Name",
                    UserId = "f49aa51bbd304e77933e24bbed65b165",
                    StyleId = "037b41302a4c453f8cef135afceb3956",
                    SKU = "P00031",
                    Images = ["https://firebasestorage.googleapis.com/v0/b/mamafit-e0138.firebasestorage.app/o/presets%2Fmaxi%2Fmaxi-8.png?alt=media&token=ccad2c07-0022-4956-9591-8ddcbda3a31a"],
                    IsDefault = false,
                    Weight = 200,
                    Price = 2000.0m,
                    Type = 0,
                    CreatedBy = "User",
                    UpdatedBy = "System",
                    CreatedAt = DateTime.Parse("2025-08-30 12:56:25.590 +0700").ToUniversalTime(),
                    UpdatedAt = DateTime.Parse("2025-08-30 12:56:25.590 +0700").ToUniversalTime(),
                    IsDeleted = false,
                },
                new Preset
                {
                    Id = "8311c9434d354d689a5049c5041026ea",
                    Name = "Name",
                    UserId = "4c9804ecc1d645de96fcfc906cc43d6c",
                    StyleId = "b10d38a099674087b125cf0dfe099f5a",
                    SKU = "P44678",
                    Images = ["https://firebasestorage.googleapis.com/v0/b/mamafit-e0138.firebasestorage.app/o/images%2F3_1756534093513_hy9hc3.png?alt=media&token=e02d24e8-fd1c-4de2-83df-95eb1e36a556"],
                    IsDefault = false,
                    Price = 2000.0m,
                    Type = 0,
                    CreatedBy = "Manager",
                    CreatedAt = DateTime.Parse("2025-08-30 13:08:45.368 +0700").ToUniversalTime(),
                    UpdatedAt = DateTime.Parse("2025-08-30 13:08:45.358 +0700").ToUniversalTime(),
                    IsDeleted = false,
                },
                new Preset
                {
                    Id = "88d4e4ed5bbe49fc81059e4f7bf45de3",
                    Name = "Name",
                    UserId = "4c9804ecc1d645de96fcfc906cc43d6c",
                    StyleId = "5ed8aceab2d34bd8801e8c6a10dfc0d0",
                    SKU = "P27141",
                    Images = ["https://firebasestorage.googleapis.com/v0/b/mamafit-e0138.firebasestorage.app/o/images%2F8_1756533902237_0nejhm.png?alt=media&token=a95e6e0c-f2be-4475-9b8c-1a1ee87571d2"],
                    IsDefault = false,
                    Price = 2000.0m,
                    Type = 0,
                    CreatedBy = "Manager",
                    CreatedAt = DateTime.Parse("2025-08-30 13:05:17.573 +0700").ToUniversalTime(),
                    UpdatedAt = DateTime.Parse("2025-08-30 13:05:17.563 +0700").ToUniversalTime(),
                    IsDeleted = false,
                },
                new Preset
                {
                    Id = "9d84ba5aa4404a6b8c3daa1648ef947a",
                    Name = "Name",
                    UserId = "4c9804ecc1d645de96fcfc906cc43d6c",
                    StyleId = "b10d38a099674087b125cf0dfe099f5a",
                    SKU = "P14659",
                    Images = ["https://firebasestorage.googleapis.com/v0/b/mamafit-e0138.firebasestorage.app/o/images%2F8_1756534426322_zfev71.png?alt=media&token=9216b266-05ed-45cf-97eb-75bfe4db2cb3"],
                    IsDefault = false,
                    Price = 2000.0m,
                    Type = 0,
                    CreatedBy = "Manager",
                    CreatedAt = DateTime.Parse("2025-08-30 13:14:07.572 +0700").ToUniversalTime(),
                    UpdatedAt = DateTime.Parse("2025-08-30 13:14:07.557 +0700").ToUniversalTime(),
                    IsDeleted = false,
                },
                new Preset
                {
                    Id = "a80f79e565d240af9d77d2550c6e2933",
                    Name = "Name",
                    UserId = "f49aa51bbd304e77933e24bbed65b165",
                    StyleId = "037b41302a4c453f8cef135afceb3956",
                    SKU = "P00015",
                    Images = ["https://firebasestorage.googleapis.com/v0/b/mamafit-e0138.firebasestorage.app/o/presets%2Fmaxi%2Fmaxi-24.png?alt=media&token=b0ceddd4-b37a-4be9-951a-e403ae5b9433"],
                    IsDefault = false,
                    Weight = 200,
                    Price = 2000.0m,
                    Type = 0,
                    CreatedBy = "User",
                    UpdatedBy = "System",
                    CreatedAt = DateTime.Parse("2025-08-30 12:56:25.590 +0700").ToUniversalTime(),
                    UpdatedAt = DateTime.Parse("2025-08-30 12:56:25.590 +0700").ToUniversalTime(),
                    IsDeleted = false,
                },
                new Preset
                {
                    Id = "a90e932d3ff14830b3c4c95dd42e9ff8",
                    Name = "Name",
                    UserId = "4c9804ecc1d645de96fcfc906cc43d6c",
                    StyleId = "b10d38a099674087b125cf0dfe099f5a",
                    SKU = "P94059",
                    Images = ["https://firebasestorage.googleapis.com/v0/b/mamafit-e0138.firebasestorage.app/o/images%2F10_1756534507244_zgbdk3.png?alt=media&token=96858234-8f94-469d-a570-0ba1d55d2c5d"],
                    IsDefault = false,
                    Price = 2000.0m,
                    Type = 0,
                    CreatedBy = "Manager",
                    CreatedAt = DateTime.Parse("2025-08-30 13:15:27.076 +0700").ToUniversalTime(),
                    UpdatedAt = DateTime.Parse("2025-08-30 13:15:27.066 +0700").ToUniversalTime(),
                    IsDeleted = false,
                },
                new Preset
                {
                    Id = "ac1b2b6941c04598ae5a50d48efd8232",
                    Name = "Name",
                    UserId = "f49aa51bbd304e77933e24bbed65b165",
                    StyleId = "037b41302a4c453f8cef135afceb3956",
                    SKU = "P00019",
                    Images = ["https://firebasestorage.googleapis.com/v0/b/mamafit-e0138.firebasestorage.app/o/presets%2Fmaxi%2Fmaxi-20.png?alt=media&token=2294c5c4-403d-4b33-afec-78f20bf02566"],
                    IsDefault = false,
                    Weight = 200,
                    Price = 2000.0m,
                    Type = 0,
                    CreatedBy = "User",
                    UpdatedBy = "System",
                    CreatedAt = DateTime.Parse("2025-08-30 12:56:25.590 +0700").ToUniversalTime(),
                    UpdatedAt = DateTime.Parse("2025-08-30 12:56:25.590 +0700").ToUniversalTime(),
                    IsDeleted = false,
                },
                new Preset
                {
                    Id = "af643fe22b414985b6896d0532875528",
                    Name = "Name",
                    UserId = "4c9804ecc1d645de96fcfc906cc43d6c",
                    StyleId = "b10d38a099674087b125cf0dfe099f5a",
                    SKU = "P40728",
                    Images = ["https://firebasestorage.googleapis.com/v0/b/mamafit-e0138.firebasestorage.app/o/images%2F11_1756534531134_5nd356.png?alt=media&token=c63bab87-3613-4164-ad00-8919403786b8"],
                    IsDefault = false,
                    Price = 2000.0m,
                    Type = 0,
                    CreatedBy = "Manager",
                    CreatedAt = DateTime.Parse("2025-08-30 13:15:46.054 +0700").ToUniversalTime(),
                    UpdatedAt = DateTime.Parse("2025-08-30 13:15:46.041 +0700").ToUniversalTime(),
                    IsDeleted = false,
                },
                new Preset
                {
                    Id = "b0daa08ae6064686a59c0835bf58145c",
                    Name = "Name",
                    UserId = "f49aa51bbd304e77933e24bbed65b165",
                    StyleId = "037b41302a4c453f8cef135afceb3956",
                    SKU = "P00014",
                    Images = ["https://firebasestorage.googleapis.com/v0/b/mamafit-e0138.firebasestorage.app/o/presets%2Fmaxi%2Fmaxi-26.png?alt=media&token=d3653bec-f9bc-4558-bc54-81d6a19c4ce2"],
                    IsDefault = false,
                    Weight = 200,
                    Price = 2000.0m,
                    Type = 0,
                    CreatedBy = "User",
                    UpdatedBy = "System",
                    CreatedAt = DateTime.Parse("2025-08-30 12:56:25.590 +0700").ToUniversalTime(),
                    UpdatedAt = DateTime.Parse("2025-08-30 12:56:25.590 +0700").ToUniversalTime(),
                    IsDeleted = false,
                },
                new Preset
                {
                    Id = "b9c4df5bcd4240f98a86ba7fd55faea6",
                    Name = "Name",
                    UserId = "4c9804ecc1d645de96fcfc906cc43d6c",
                    StyleId = "5ed8aceab2d34bd8801e8c6a10dfc0d0",
                    SKU = "P30539",
                    Images = ["https://firebasestorage.googleapis.com/v0/b/mamafit-e0138.firebasestorage.app/o/images%2F4_1756533692233_qcpddf.png?alt=media&token=744b1ee0-dd1a-4048-b424-6d74d904db52"],
                    IsDefault = false,
                    Price = 2000.0m,
                    Type = 0,
                    CreatedBy = "Manager",
                    CreatedAt = DateTime.Parse("2025-08-30 13:02:28.135 +0700").ToUniversalTime(),
                    UpdatedAt = DateTime.Parse("2025-08-30 13:02:28.121 +0700").ToUniversalTime(),
                    IsDeleted = false,
                },
                new Preset
                {
                    Id = "bac18c4350314174b9f4cbe73ca0b24d",
                    Name = "Name",
                    UserId = "f49aa51bbd304e77933e24bbed65b165",
                    StyleId = "037b41302a4c453f8cef135afceb3956",
                    SKU = "P00035",
                    Images = ["https://firebasestorage.googleapis.com/v0/b/mamafit-e0138.firebasestorage.app/o/presets%2Fmaxi%2Fmaxi-5.png?alt=media&token=778c61b7-68ff-4224-afd8-223d9295c99c"],
                    IsDefault = false,
                    Weight = 200,
                    Price = 2000.0m,
                    Type = 0,
                    CreatedBy = "User",
                    UpdatedBy = "System",
                    CreatedAt = DateTime.Parse("2025-08-30 12:56:25.590 +0700").ToUniversalTime(),
                    UpdatedAt = DateTime.Parse("2025-08-30 12:56:25.590 +0700").ToUniversalTime(),
                    IsDeleted = false,
                },
                new Preset
                {
                    Id = "bcdcf89795b2484f96a3a12b25383a12",
                    Name = "Name",
                    UserId = "f49aa51bbd304e77933e24bbed65b165",
                    StyleId = "037b41302a4c453f8cef135afceb3956",
                    SKU = "P00007",
                    Images = ["https://firebasestorage.googleapis.com/v0/b/mamafit-e0138.firebasestorage.app/o/presets%2Fmaxi%2Fmaxi-15.png?alt=media&token=a4a6b9e1-b20a-4939-bf9f-524f0c5911ae"],
                    IsDefault = false,
                    Weight = 200,
                    Price = 2000.0m,
                    Type = 0,
                    CreatedBy = "User",
                    UpdatedBy = "System",
                    CreatedAt = DateTime.Parse("2025-08-30 12:56:25.590 +0700").ToUniversalTime(),
                    UpdatedAt = DateTime.Parse("2025-08-30 12:56:25.590 +0700").ToUniversalTime(),
                    IsDeleted = false,
                },
                new Preset
                {
                    Id = "be7b594cc1274fb5b9c4fa5571ed8952",
                    Name = "Name",
                    UserId = "f49aa51bbd304e77933e24bbed65b165",
                    StyleId = "037b41302a4c453f8cef135afceb3956",
                    SKU = "P00016",
                    Images = ["https://firebasestorage.googleapis.com/v0/b/mamafit-e0138.firebasestorage.app/o/presets%2Fmaxi%2Fmaxi-23.png?alt=media&token=03440f28-4ec7-4fd7-8cd5-8a269106a726"],
                    IsDefault = false,
                    Weight = 200,
                    Price = 2000.0m,
                    Type = 0,
                    CreatedBy = "User",
                    UpdatedBy = "System",
                    CreatedAt = DateTime.Parse("2025-08-30 12:56:25.590 +0700").ToUniversalTime(),
                    UpdatedAt = DateTime.Parse("2025-08-30 12:56:25.590 +0700").ToUniversalTime(),
                    IsDeleted = false,
                },
                new Preset
                {
                    Id = "c3797a5e9c9a42dba09b0001c8cdaed6",
                    Name = "Name",
                    UserId = "4c9804ecc1d645de96fcfc906cc43d6c",
                    StyleId = "b10d38a099674087b125cf0dfe099f5a",
                    SKU = "P36965",
                    Images = ["https://firebasestorage.googleapis.com/v0/b/mamafit-e0138.firebasestorage.app/o/images%2F7_1756534384235_g4rq8h.png?alt=media&token=57af6647-4ac2-43f1-ab9c-48d80f19eea8"],
                    IsDefault = false,
                    Price = 2000.0m,
                    Type = 0,
                    CreatedBy = "Manager",
                    CreatedAt = DateTime.Parse("2025-08-30 13:13:42.971 +0700").ToUniversalTime(),
                    UpdatedAt = DateTime.Parse("2025-08-30 13:13:42.959 +0700").ToUniversalTime(),
                    IsDeleted = false,
                },
                new Preset
                {
                    Id = "c9348ab0418743d08ee786e31083d76b",
                    Name = "Name",
                    UserId = "f49aa51bbd304e77933e24bbed65b165",
                    StyleId = "037b41302a4c453f8cef135afceb3956",
                    SKU = "P00004",
                    Images = ["https://firebasestorage.googleapis.com/v0/b/mamafit-e0138.firebasestorage.app/o/presets%2Fmaxi%2Fmaxi-17.png?alt=media&token=f90d9acc-504a-496b-8c4d-eb35b2a05fb0"],
                    IsDefault = false,
                    Weight = 200,
                    Price = 2000.0m,
                    Type = 0,
                    CreatedBy = "User",
                    UpdatedBy = "System",
                    CreatedAt = DateTime.Parse("2025-08-30 12:56:25.590 +0700").ToUniversalTime(),
                    UpdatedAt = DateTime.Parse("2025-08-30 12:56:25.590 +0700").ToUniversalTime(),
                    IsDeleted = false,
                },
                new Preset
                {
                    Id = "caa36b99b068411aaa62b0f4234c1656",
                    Name = "Name",
                    UserId = "f49aa51bbd304e77933e24bbed65b165",
                    StyleId = "037b41302a4c453f8cef135afceb3956",
                    SKU = "P00032",
                    Images = ["https://firebasestorage.googleapis.com/v0/b/mamafit-e0138.firebasestorage.app/o/presets%2Fmaxi%2Fmaxi-7.png?alt=media&token=29058fd5-ec1f-4199-94ff-72c77584b562"],
                    IsDefault = false,
                    Weight = 200,
                    Price = 2000.0m,
                    Type = 0,
                    CreatedBy = "User",
                    UpdatedBy = "System",
                    CreatedAt = DateTime.Parse("2025-08-30 12:56:25.590 +0700").ToUniversalTime(),
                    UpdatedAt = DateTime.Parse("2025-08-30 12:56:25.590 +0700").ToUniversalTime(),
                    IsDeleted = false,
                },
                new Preset
                {
                    Id = "cc5b36affe2243f2b119fd65cb3434d5",
                    Name = "Name",
                    UserId = "f49aa51bbd304e77933e24bbed65b165",
                    StyleId = "037b41302a4c453f8cef135afceb3956",
                    SKU = "P00024",
                    Images = ["https://firebasestorage.googleapis.com/v0/b/mamafit-e0138.firebasestorage.app/o/presets%2Fmaxi%2Fmaxi-34.png?alt=media&token=076bb0d1-0496-4353-adb2-1be1fce6e8bb"],
                    IsDefault = false,
                    Weight = 200,
                    Price = 2000.0m,
                    Type = 0,
                    CreatedBy = "User",
                    UpdatedBy = "System",
                    CreatedAt = DateTime.Parse("2025-08-30 12:56:25.590 +0700").ToUniversalTime(),
                    UpdatedAt = DateTime.Parse("2025-08-30 12:56:25.590 +0700").ToUniversalTime(),
                    IsDeleted = false,
                },
                new Preset
                {
                    Id = "ccc662f99d7a41d7aa1505e3a2cb8950",
                    Name = "Name",
                    UserId = "f49aa51bbd304e77933e24bbed65b165",
                    StyleId = "037b41302a4c453f8cef135afceb3956",
                    SKU = "P00011",
                    Images = ["https://firebasestorage.googleapis.com/v0/b/mamafit-e0138.firebasestorage.app/o/presets%2Fmaxi%2Fmaxi-29.png?alt=media&token=25bc32d6-d2b7-4881-8ead-38dbad9096b6"],
                    IsDefault = false,
                    Weight = 200,
                    Price = 2000.0m,
                    Type = 0,
                    CreatedBy = "User",
                    UpdatedBy = "System",
                    CreatedAt = DateTime.Parse("2025-08-30 12:56:25.590 +0700").ToUniversalTime(),
                    UpdatedAt = DateTime.Parse("2025-08-30 12:56:25.590 +0700").ToUniversalTime(),
                    IsDeleted = false,
                },
                new Preset
                {
                    Id = "cfdabdeb4fd748849e6a635ec6418dd2",
                    Name = "Name",
                    UserId = "4c9804ecc1d645de96fcfc906cc43d6c",
                    StyleId = "5ed8aceab2d34bd8801e8c6a10dfc0d0",
                    SKU = "P12312",
                    Images = ["https://firebasestorage.googleapis.com/v0/b/mamafit-e0138.firebasestorage.app/o/images%2F3_1756533654085_mvf12n.png?alt=media&token=2485dbac-18bb-44b2-af37-e1d8ffa5bb59"],
                    IsDefault = false,
                    Price = 2000.0m,
                    Type = 0,
                    CreatedBy = "Manager",
                    CreatedAt = DateTime.Parse("2025-08-30 13:01:24.126 +0700").ToUniversalTime(),
                    UpdatedAt = DateTime.Parse("2025-08-30 13:01:24.111 +0700").ToUniversalTime(),
                    IsDeleted = false,
                },
                new Preset
                {
                    Id = "d1d9616eb7904f189659505eca16cd43",
                    Name = "Name",
                    UserId = "4c9804ecc1d645de96fcfc906cc43d6c",
                    StyleId = "5ed8aceab2d34bd8801e8c6a10dfc0d0",
                    SKU = "P05656",
                    Images = ["https://firebasestorage.googleapis.com/v0/b/mamafit-e0138.firebasestorage.app/o/images%2F1_1756533543886_ahm0dk.png?alt=media&token=e73093c0-402a-4b85-9389-d1e91a61fc4b"],
                    IsDefault = true,
                    Price = 2000.0m,
                    Type = 0,
                    CreatedBy = "Manager",
                    CreatedAt = DateTime.Parse("2025-08-30 12:59:26.373 +0700").ToUniversalTime(),
                    UpdatedAt = DateTime.Parse("2025-08-30 12:59:26.361 +0700").ToUniversalTime(),
                    IsDeleted = false,
                },
                new Preset
                {
                    Id = "d208c36d9fa640c48eb05f6dd19963e2",
                    Name = "Name",
                    UserId = "4c9804ecc1d645de96fcfc906cc43d6c",
                    StyleId = "b10d38a099674087b125cf0dfe099f5a",
                    SKU = "P22298",
                    Images = ["https://firebasestorage.googleapis.com/v0/b/mamafit-e0138.firebasestorage.app/o/images%2F4_1756534137369_gxxhyl.png?alt=media&token=35280be5-5e61-4e95-81c6-e2b8d243dcaf"],
                    IsDefault = false,
                    Price = 2000.0m,
                    Type = 0,
                    CreatedBy = "Manager",
                    CreatedAt = DateTime.Parse("2025-08-30 13:09:20.050 +0700").ToUniversalTime(),
                    UpdatedAt = DateTime.Parse("2025-08-30 13:09:20.035 +0700").ToUniversalTime(),
                    IsDeleted = false,
                },
                new Preset
                {
                    Id = "dd737fac7e8b4b0cb353b6d8ac16cb51",
                    Name = "Name",
                    UserId = "f49aa51bbd304e77933e24bbed65b165",
                    StyleId = "037b41302a4c453f8cef135afceb3956",
                    SKU = "P00014",
                    Images = ["https://firebasestorage.googleapis.com/v0/b/mamafit-e0138.firebasestorage.app/o/presets%2Fmaxi%2Fmaxi-25.png?alt=media&token=f0a6e9b6-05cf-4e31-b1f3-9ecdbdcde3d2"],
                    IsDefault = false,
                    Weight = 200,
                    Price = 2000.0m,
                    Type = 0,
                    CreatedBy = "User",
                    UpdatedBy = "System",
                    CreatedAt = DateTime.Parse("2025-08-30 12:56:25.590 +0700").ToUniversalTime(),
                    UpdatedAt = DateTime.Parse("2025-08-30 12:56:25.590 +0700").ToUniversalTime(),
                    IsDeleted = false,
                },
                new Preset
                {
                    Id = "e3485b7dc853448588da3dde04c4e3eb",
                    Name = "Name",
                    UserId = "4c9804ecc1d645de96fcfc906cc43d6c",
                    StyleId = "b10d38a099674087b125cf0dfe099f5a",
                    SKU = "P44253",
                    Images = ["https://firebasestorage.googleapis.com/v0/b/mamafit-e0138.firebasestorage.app/o/images%2F6_1756534362326_l8sazc.png?alt=media&token=c9256b37-db4d-45aa-a3fe-3f1d5efe954d"],
                    IsDefault = false,
                    Price = 2000.0m,
                    Type = 0,
                    CreatedBy = "Manager",
                    CreatedAt = DateTime.Parse("2025-08-30 13:13:00.832 +0700").ToUniversalTime(),
                    UpdatedAt = DateTime.Parse("2025-08-30 13:13:00.823 +0700").ToUniversalTime(),
                    IsDeleted = false,
                },
                new Preset
                {
                    Id = "f1beda0c907a40dfaa73738cd6313d03",
                    Name = "Name",
                    UserId = "4c9804ecc1d645de96fcfc906cc43d6c",
                    StyleId = "5ed8aceab2d34bd8801e8c6a10dfc0d0",
                    SKU = "P25932",
                    Images = ["https://firebasestorage.googleapis.com/v0/b/mamafit-e0138.firebasestorage.app/o/images%2F7_1756533844208_jjmei1.png?alt=media&token=a2facff1-8085-486c-bc89-df1da42bed17"],
                    IsDefault = false,
                    Price = 2000.0m,
                    Type = 0,
                    CreatedBy = "Manager",
                    CreatedAt = DateTime.Parse("2025-08-30 13:04:31.466 +0700").ToUniversalTime(),
                    UpdatedAt = DateTime.Parse("2025-08-30 13:04:31.453 +0700").ToUniversalTime(),
                    IsDeleted = false,
                },
                new Preset
                {
                    Id = "f1ec1932c78e4a5a8035df1864fb32a4",
                    Name = "Name",
                    UserId = "f49aa51bbd304e77933e24bbed65b165",
                    StyleId = "037b41302a4c453f8cef135afceb3956",
                    SKU = "P00023",
                    Images = ["https://firebasestorage.googleapis.com/v0/b/mamafit-e0138.firebasestorage.app/o/presets%2Fmaxi%2Fmaxi-35.png?alt=media&token=1a81d76f-506a-4b60-9c03-05fa9aab1483"],
                    IsDefault = false,
                    Weight = 200,
                    Price = 2000.0m,
                    Type = 0,
                    CreatedBy = "User",
                    UpdatedBy = "System",
                    CreatedAt = DateTime.Parse("2025-08-30 12:56:25.590 +0700").ToUniversalTime(),
                    UpdatedAt = DateTime.Parse("2025-08-30 12:56:25.590 +0700").ToUniversalTime(),
                    IsDeleted = false,
                },
                new Preset
                {
                    Id = "fe7043a37995453db2d03741e81da9df",
                    Name = "Name",
                    UserId = "f49aa51bbd304e77933e24bbed65b165",
                    StyleId = "037b41302a4c453f8cef135afceb3956",
                    SKU = "P00030",
                    Images = ["https://firebasestorage.googleapis.com/v0/b/mamafit-e0138.firebasestorage.app/o/presets%2Fmaxi%2Fmaxi-9.png?alt=media&token=221bb4f5-1a84-452a-9a40-48cc8f50afdc"],
                    IsDefault = false,
                    Weight = 200,
                    Price = 2000.0m,
                    Type = 0,
                    CreatedBy = "User",
                    UpdatedBy = "System",
                    CreatedAt = DateTime.Parse("2025-08-30 12:56:25.590 +0700").ToUniversalTime(),
                    UpdatedAt = DateTime.Parse("2025-08-30 12:56:25.590 +0700").ToUniversalTime(),
                    IsDeleted = false,
                },
                new Preset
                {
                    Id = "fe88b53dce4c4938a7785fb9e3db1f0a",
                    Name = "Name",
                    UserId = "f49aa51bbd304e77933e24bbed65b165",
                    StyleId = "037b41302a4c453f8cef135afceb3956",
                    SKU = "P00001",
                    Images = ["https://firebasestorage.googleapis.com/v0/b/mamafit-e0138.firebasestorage.app/o/presets%2Fmaxi%2Fmaxi.png?alt=media&token=da59cd6d-df28-4605-a464-88951676e0fd"],
                    IsDefault = true,
                    Weight = 200,
                    Price = 2000.0m,
                    Type = 0,
                    CreatedBy = "User",
                    UpdatedBy = "System",
                    CreatedAt = DateTime.Parse("2025-08-30 12:56:25.590 +0700").ToUniversalTime(),
                    UpdatedAt = DateTime.Parse("2025-08-30 12:56:25.590 +0700").ToUniversalTime(),
                    IsDeleted = false,
                },
                new Preset
                {
                    Id = "ff13d99b031a4a93b571c0a0dfdf5770",
                    Name = "Name",
                    UserId = "4c9804ecc1d645de96fcfc906cc43d6c",
                    StyleId = "b10d38a099674087b125cf0dfe099f5a",
                    SKU = "P89985",
                    Images = ["https://firebasestorage.googleapis.com/v0/b/mamafit-e0138.firebasestorage.app/o/images%2F12_1756534565372_psoeuj.png?alt=media&token=efee87be-b611-4645-a91a-c934ed00c9ee"],
                    IsDefault = false,
                    Price = 2000.0m,
                    Type = 0,
                    CreatedBy = "Manager",
                    CreatedAt = DateTime.Parse("2025-08-30 13:16:20.119 +0700").ToUniversalTime(),
                    UpdatedAt = DateTime.Parse("2025-08-30 13:16:20.106 +0700").ToUniversalTime(),
                    IsDeleted = false,
                },
                new Preset
                {
                    Id = "edaba02c35264bfcbddd4d80ee293da6",
                    Name = "Name",
                    UserId = "4c9804ecc1d645de96fcfc906cc43d6c",
                    StyleId = "11363158846e4478b326bf58a7ca9d21",
                    SKU = "P30666",
                    Images = ["https://firebasestorage.googleapis.com/v0/b/mamafit-e0138.firebasestorage.app/o/images%2F1_1756568980028_xb2nkv.png?alt=media&token=4d45a38c-a88b-4556-98f4-9349edd382f5"],
                    IsDefault = true,
                    Price = 2000.0m,
                    Type = 0,
                    CreatedBy = "Manager",
                    CreatedAt = DateTime.Parse("2025-08-30 22:49:56.614 +0700").ToUniversalTime(),
                    UpdatedAt = DateTime.Parse("2025-08-30 22:49:56.585 +0700").ToUniversalTime(),
                    IsDeleted = false,
                },
                new Preset
                {
                    Id = "bb4fa5ef967845ed9df5ff26d2dd1c4c",
                    Name = "Name",
                    UserId = "4c9804ecc1d645de96fcfc906cc43d6c",
                    StyleId = "11363158846e4478b326bf58a7ca9d21",
                    SKU = "P56619",
                    Images = ["https://firebasestorage.googleapis.com/v0/b/mamafit-e0138.firebasestorage.app/o/images%2F2_1756569003946_l22m8e.png?alt=media&token=eaaa11aa-dfbb-4908-8438-ce66cff84342"],
                    IsDefault = false,
                    Price = 2000.0m,
                    Type = 0,
                    CreatedBy = "Manager",
                    CreatedAt = DateTime.Parse("2025-08-30 22:50:17.339 +0700").ToUniversalTime(),
                    UpdatedAt = DateTime.Parse("2025-08-30 22:50:17.318 +0700").ToUniversalTime(),
                    IsDeleted = false,
                },
                new Preset
                {
                    Id = "54963650e77e411daa0afe6cf4e0bd87",
                    Name = "Name",
                    UserId = "4c9804ecc1d645de96fcfc906cc43d6c",
                    StyleId = "11363158846e4478b326bf58a7ca9d21",
                    SKU = "P84279",
                    Images = ["https://firebasestorage.googleapis.com/v0/b/mamafit-e0138.firebasestorage.app/o/images%2F3_1756569024889_a9kaaw.png?alt=media&token=752ee4ab-ba75-4a31-bb05-309359d0f295"],
                    IsDefault = false,
                    Price = 2000.0m,
                    Type = 0,
                    CreatedBy = "Manager",
                    CreatedAt = DateTime.Parse("2025-08-30 22:50:38.660 +0700").ToUniversalTime(),
                    UpdatedAt = DateTime.Parse("2025-08-30 22:50:38.645 +0700").ToUniversalTime(),
                    IsDeleted = false,
                },
                new Preset
                {
                    Id = "dd486595e71d444cb8f42d0c91f5a0e7",
                    Name = "Name",
                    UserId = "4c9804ecc1d645de96fcfc906cc43d6c",
                    StyleId = "11363158846e4478b326bf58a7ca9d21",
                    SKU = "P77304",
                    Images = ["https://firebasestorage.googleapis.com/v0/b/mamafit-e0138.firebasestorage.app/o/images%2F4_1756569070512_o2s30f.png?alt=media&token=3516a508-f6be-483b-9b8d-f2d9a6f1d7fc"],
                    IsDefault = false,
                    Price = 2000.0m,
                    Type = 0,
                    CreatedBy = "Manager",
                    CreatedAt = DateTime.Parse("2025-08-30 22:51:23.449 +0700").ToUniversalTime(),
                    UpdatedAt = DateTime.Parse("2025-08-30 22:51:23.432 +0700").ToUniversalTime(),
                    IsDeleted = false,
                },
                new Preset
                {
                    Id = "461e2cae39e14726b2cd1772cdb4c66e",
                    Name = "Name",
                    UserId = "4c9804ecc1d645de96fcfc906cc43d6c",
                    StyleId = "11363158846e4478b326bf58a7ca9d21",
                    SKU = "P27707",
                    Images = ["https://firebasestorage.googleapis.com/v0/b/mamafit-e0138.firebasestorage.app/o/images%2F5_1756569091241_b3i5lw.png?alt=media&token=90621bde-0b81-4edb-9ac3-5a9a730b1687"],
                    IsDefault = false,
                    Price = 2000.0m,
                    Type = 0,
                    CreatedBy = "Manager",
                    CreatedAt = DateTime.Parse("2025-08-30 22:51:44.681 +0700").ToUniversalTime(),
                    UpdatedAt = DateTime.Parse("2025-08-30 22:51:44.662 +0700").ToUniversalTime(),
                    IsDeleted = false,
                },
                new Preset
                {
                    Id = "7afdd951b8f948519388689b84373fba",
                    Name = "Name",
                    UserId = "4c9804ecc1d645de96fcfc906cc43d6c",
                    StyleId = "11363158846e4478b326bf58a7ca9d21",
                    SKU = "P16180",
                    Images = ["https://firebasestorage.googleapis.com/v0/b/mamafit-e0138.firebasestorage.app/o/images%2F6_1756569112965_kl3g47.png?alt=media&token=e65e0446-dd65-44d1-984d-8709a96bda91"],
                    IsDefault = false,
                    Price = 2000.0m,
                    Type = 0,
                    CreatedBy = "Manager",
                    CreatedAt = DateTime.Parse("2025-08-30 22:52:29.302 +0700").ToUniversalTime(),
                    UpdatedAt = DateTime.Parse("2025-08-30 22:52:29.289 +0700").ToUniversalTime(),
                    IsDeleted = false,
                },
                new Preset
                {
                    Id = "7f7a036811a14c9ba4058fa770b9a7e6",
                    Name = "Name",
                    UserId = "4c9804ecc1d645de96fcfc906cc43d6c",
                    StyleId = "11363158846e4478b326bf58a7ca9d21",
                    SKU = "P64333",
                    Images = ["https://firebasestorage.googleapis.com/v0/b/mamafit-e0138.firebasestorage.app/o/images%2F7_1756569155414_z0c8fl.png?alt=media&token=25dfd3d5-7870-4142-bcc7-4a21b9f0a02c"],
                    IsDefault = false,
                    Price = 2000.0m,
                    Type = 0,
                    CreatedBy = "Manager",
                    CreatedAt = DateTime.Parse("2025-08-30 22:53:46.396 +0700").ToUniversalTime(),
                    UpdatedAt = DateTime.Parse("2025-08-30 22:53:46.383 +0700").ToUniversalTime(),
                    IsDeleted = false,
                },
                new Preset
                {
                    Id = "e4de6c02c26a42a6bc1ba0b9ac158774",
                    Name = "Name",
                    UserId = "4c9804ecc1d645de96fcfc906cc43d6c",
                    StyleId = "11363158846e4478b326bf58a7ca9d21",
                    SKU = "P20124",
                    Images = ["https://firebasestorage.googleapis.com/v0/b/mamafit-e0138.firebasestorage.app/o/images%2F8_1756569236306_x05poe.png?alt=media&token=7d22aabb-0a1d-4b09-b8a7-0d386f0cd71c"],
                    IsDefault = false,
                    Price = 2000.0m,
                    Type = 0,
                    CreatedBy = "Manager",
                    CreatedAt = DateTime.Parse("2025-08-30 22:54:37.024 +0700").ToUniversalTime(),
                    UpdatedAt = DateTime.Parse("2025-08-30 22:54:37.009 +0700").ToUniversalTime(),
                    IsDeleted = false,
                }
            );
            #endregion

            #region ComponentOptionPreset Seed
            modelBuilder.Entity<ComponentOptionPreset>().HasData(
                new ComponentOptionPreset
                {
                    ComponentOptionsId = "06e6899bd6544dd3b9020a50789d9421",
                    PresetsId = "11230115583c4994bfaa3925951d3817",
                },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "31c853d3dde94e68ab57903cf8a80798",
        PresetsId = "11230115583c4994bfaa3925951d3817",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "4ed7cdc105ab44de81d333d63db0a222",
        PresetsId = "11230115583c4994bfaa3925951d3817",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "70b69ec5ea074c0f8bf904b20b9c7884",
        PresetsId = "11230115583c4994bfaa3925951d3817",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "92bdbbdbfd3b4dcf8256b68a66dfa35b",
        PresetsId = "11230115583c4994bfaa3925951d3817",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "c454b97a93f341e78b127d5425acc464",
        PresetsId = "11230115583c4994bfaa3925951d3817",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "06e6899bd6544dd3b9020a50789d9421",
        PresetsId = "1453c4e8983c4a3397deecaccf1230c1",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "31c853d3dde94e68ab57903cf8a80798",
        PresetsId = "1453c4e8983c4a3397deecaccf1230c1",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "66645e2f8fa94d228d87450cc4a74706",
        PresetsId = "1453c4e8983c4a3397deecaccf1230c1",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "6b81101144404b9ab306235861eedc12",
        PresetsId = "1453c4e8983c4a3397deecaccf1230c1",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "6eb66f2427eb47a1a1fffc0dd3f89f6e",
        PresetsId = "1453c4e8983c4a3397deecaccf1230c1",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "92bdbbdbfd3b4dcf8256b68a66dfa35b",
        PresetsId = "1453c4e8983c4a3397deecaccf1230c1",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "31c853d3dde94e68ab57903cf8a80798",
        PresetsId = "1acd7ac56e5c4d15895f03cc2d6e7723",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "70b69ec5ea074c0f8bf904b20b9c7884",
        PresetsId = "1acd7ac56e5c4d15895f03cc2d6e7723",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "78b78938aefe4bc6b927ed3722bc5859",
        PresetsId = "1acd7ac56e5c4d15895f03cc2d6e7723",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "8158b8e5ef0340bdaf830fa8eb2e650d",
        PresetsId = "1acd7ac56e5c4d15895f03cc2d6e7723",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "818f875df04a49eebe9c474bba4247b9",
        PresetsId = "1acd7ac56e5c4d15895f03cc2d6e7723",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "c454b97a93f341e78b127d5425acc464",
        PresetsId = "1acd7ac56e5c4d15895f03cc2d6e7723",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "31c853d3dde94e68ab57903cf8a80798",
        PresetsId = "1f55382a1ffa42be9f046634d90b9f97",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "4ed7cdc105ab44de81d333d63db0a222",
        PresetsId = "1f55382a1ffa42be9f046634d90b9f97",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "6b81101144404b9ab306235861eedc12",
        PresetsId = "1f55382a1ffa42be9f046634d90b9f97",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "7186b0e8de2348fdbde87105c0f9abe3",
        PresetsId = "1f55382a1ffa42be9f046634d90b9f97",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "818f875df04a49eebe9c474bba4247b9",
        PresetsId = "1f55382a1ffa42be9f046634d90b9f97",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "e2c03da33b12458694a1b01c3ec66474",
        PresetsId = "1f55382a1ffa42be9f046634d90b9f97",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "31c853d3dde94e68ab57903cf8a80798",
        PresetsId = "21b2065adaf44984b32cd6b9eb75c2f7",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "44ecb280dd8f4137af58024e0780e994",
        PresetsId = "21b2065adaf44984b32cd6b9eb75c2f7",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "4ed7cdc105ab44de81d333d63db0a222",
        PresetsId = "21b2065adaf44984b32cd6b9eb75c2f7",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "6b81101144404b9ab306235861eedc12",
        PresetsId = "21b2065adaf44984b32cd6b9eb75c2f7",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "7186b0e8de2348fdbde87105c0f9abe3",
        PresetsId = "21b2065adaf44984b32cd6b9eb75c2f7",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "e2c03da33b12458694a1b01c3ec66474",
        PresetsId = "21b2065adaf44984b32cd6b9eb75c2f7",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "31c853d3dde94e68ab57903cf8a80798",
        PresetsId = "220bff408b7e465eb4652da38241c7a2",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "6b81101144404b9ab306235861eedc12",
        PresetsId = "220bff408b7e465eb4652da38241c7a2",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "6eb66f2427eb47a1a1fffc0dd3f89f6e",
        PresetsId = "220bff408b7e465eb4652da38241c7a2",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "7186b0e8de2348fdbde87105c0f9abe3",
        PresetsId = "220bff408b7e465eb4652da38241c7a2",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "818f875df04a49eebe9c474bba4247b9",
        PresetsId = "220bff408b7e465eb4652da38241c7a2",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "e2c03da33b12458694a1b01c3ec66474",
        PresetsId = "220bff408b7e465eb4652da38241c7a2",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "06e6899bd6544dd3b9020a50789d9421",
        PresetsId = "2b9900f612414d2f80531a48275927aa",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "31c853d3dde94e68ab57903cf8a80798",
        PresetsId = "2b9900f612414d2f80531a48275927aa",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "4ed7cdc105ab44de81d333d63db0a222",
        PresetsId = "2b9900f612414d2f80531a48275927aa",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "6b81101144404b9ab306235861eedc12",
        PresetsId = "2b9900f612414d2f80531a48275927aa",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "92bdbbdbfd3b4dcf8256b68a66dfa35b",
        PresetsId = "2b9900f612414d2f80531a48275927aa",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "d63a7d0c9e5d4e258285ded1974bf9cf",
        PresetsId = "2b9900f612414d2f80531a48275927aa",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "31c853d3dde94e68ab57903cf8a80798",
        PresetsId = "33c8177630a54906b92c0b8c03c57450",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "70b69ec5ea074c0f8bf904b20b9c7884",
        PresetsId = "33c8177630a54906b92c0b8c03c57450",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "8158b8e5ef0340bdaf830fa8eb2e650d",
        PresetsId = "33c8177630a54906b92c0b8c03c57450",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "92bdbbdbfd3b4dcf8256b68a66dfa35b",
        PresetsId = "33c8177630a54906b92c0b8c03c57450",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "c454b97a93f341e78b127d5425acc464",
        PresetsId = "33c8177630a54906b92c0b8c03c57450",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "fecd26fe3b6b4e88ba97ebb406fa6df2",
        PresetsId = "33c8177630a54906b92c0b8c03c57450",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "06e6899bd6544dd3b9020a50789d9421",
        PresetsId = "3583422ceef647d28a14b82680058ff0",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "31c853d3dde94e68ab57903cf8a80798",
        PresetsId = "3583422ceef647d28a14b82680058ff0",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "78b78938aefe4bc6b927ed3722bc5859",
        PresetsId = "3583422ceef647d28a14b82680058ff0",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "8cff38afa8464ae191ba152b93438266",
        PresetsId = "3583422ceef647d28a14b82680058ff0",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "92bdbbdbfd3b4dcf8256b68a66dfa35b",
        PresetsId = "3583422ceef647d28a14b82680058ff0",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "c454b97a93f341e78b127d5425acc464",
        PresetsId = "3583422ceef647d28a14b82680058ff0",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "31c853d3dde94e68ab57903cf8a80798",
        PresetsId = "380e3d771eac4488ad94b09c9e77ccd2",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "70b69ec5ea074c0f8bf904b20b9c7884",
        PresetsId = "380e3d771eac4488ad94b09c9e77ccd2",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "78b78938aefe4bc6b927ed3722bc5859",
        PresetsId = "380e3d771eac4488ad94b09c9e77ccd2",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "818f875df04a49eebe9c474bba4247b9",
        PresetsId = "380e3d771eac4488ad94b09c9e77ccd2",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "81acc249e2664c72908014a28e1e72ef",
        PresetsId = "380e3d771eac4488ad94b09c9e77ccd2",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "c454b97a93f341e78b127d5425acc464",
        PresetsId = "380e3d771eac4488ad94b09c9e77ccd2",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "31c853d3dde94e68ab57903cf8a80798",
        PresetsId = "3d0b5e4af6114f659e6c4457bae78782",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "6b81101144404b9ab306235861eedc12",
        PresetsId = "3d0b5e4af6114f659e6c4457bae78782",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "7186b0e8de2348fdbde87105c0f9abe3",
        PresetsId = "3d0b5e4af6114f659e6c4457bae78782",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "818f875df04a49eebe9c474bba4247b9",
        PresetsId = "3d0b5e4af6114f659e6c4457bae78782",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "d63a7d0c9e5d4e258285ded1974bf9cf",
        PresetsId = "3d0b5e4af6114f659e6c4457bae78782",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "fecd26fe3b6b4e88ba97ebb406fa6df2",
        PresetsId = "3d0b5e4af6114f659e6c4457bae78782",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "06e6899bd6544dd3b9020a50789d9421",
        PresetsId = "41e4b32655a44dca862141df8099f646",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "31c853d3dde94e68ab57903cf8a80798",
        PresetsId = "41e4b32655a44dca862141df8099f646",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "4ed7cdc105ab44de81d333d63db0a222",
        PresetsId = "41e4b32655a44dca862141df8099f646",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "70b69ec5ea074c0f8bf904b20b9c7884",
        PresetsId = "41e4b32655a44dca862141df8099f646",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "818f875df04a49eebe9c474bba4247b9",
        PresetsId = "41e4b32655a44dca862141df8099f646",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "c454b97a93f341e78b127d5425acc464",
        PresetsId = "41e4b32655a44dca862141df8099f646",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "06e6899bd6544dd3b9020a50789d9421",
        PresetsId = "440dfdc24b064c9babc883547439de24",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "31c853d3dde94e68ab57903cf8a80798",
        PresetsId = "440dfdc24b064c9babc883547439de24",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "4ed7cdc105ab44de81d333d63db0a222",
        PresetsId = "440dfdc24b064c9babc883547439de24",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "66645e2f8fa94d228d87450cc4a74706",
        PresetsId = "440dfdc24b064c9babc883547439de24",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "6b81101144404b9ab306235861eedc12",
        PresetsId = "440dfdc24b064c9babc883547439de24",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "92bdbbdbfd3b4dcf8256b68a66dfa35b",
        PresetsId = "440dfdc24b064c9babc883547439de24",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "31c853d3dde94e68ab57903cf8a80798",
        PresetsId = "45f43125d239423893e337915ccaaea3",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "78b78938aefe4bc6b927ed3722bc5859",
        PresetsId = "45f43125d239423893e337915ccaaea3",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "8158b8e5ef0340bdaf830fa8eb2e650d",
        PresetsId = "45f43125d239423893e337915ccaaea3",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "8cff38afa8464ae191ba152b93438266",
        PresetsId = "45f43125d239423893e337915ccaaea3",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "92bdbbdbfd3b4dcf8256b68a66dfa35b",
        PresetsId = "45f43125d239423893e337915ccaaea3",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "c454b97a93f341e78b127d5425acc464",
        PresetsId = "45f43125d239423893e337915ccaaea3",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "31c853d3dde94e68ab57903cf8a80798",
        PresetsId = "49e11d4698f643dca14364eed4d9c239",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "78b78938aefe4bc6b927ed3722bc5859",
        PresetsId = "49e11d4698f643dca14364eed4d9c239",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "818f875df04a49eebe9c474bba4247b9",
        PresetsId = "49e11d4698f643dca14364eed4d9c239",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "81acc249e2664c72908014a28e1e72ef",
        PresetsId = "49e11d4698f643dca14364eed4d9c239",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "8cff38afa8464ae191ba152b93438266",
        PresetsId = "49e11d4698f643dca14364eed4d9c239",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "c454b97a93f341e78b127d5425acc464",
        PresetsId = "49e11d4698f643dca14364eed4d9c239",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "06e6899bd6544dd3b9020a50789d9421",
        PresetsId = "4f4d8b91705f4a9bb95ecf18b57b0306",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "31c853d3dde94e68ab57903cf8a80798",
        PresetsId = "4f4d8b91705f4a9bb95ecf18b57b0306",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "4ed7cdc105ab44de81d333d63db0a222",
        PresetsId = "4f4d8b91705f4a9bb95ecf18b57b0306",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "818f875df04a49eebe9c474bba4247b9",
        PresetsId = "4f4d8b91705f4a9bb95ecf18b57b0306",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "8cff38afa8464ae191ba152b93438266",
        PresetsId = "4f4d8b91705f4a9bb95ecf18b57b0306",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "c454b97a93f341e78b127d5425acc464",
        PresetsId = "4f4d8b91705f4a9bb95ecf18b57b0306",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "31c853d3dde94e68ab57903cf8a80798",
        PresetsId = "50959c7c201e45a9b5255558ac9dfcf7",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "70b69ec5ea074c0f8bf904b20b9c7884",
        PresetsId = "50959c7c201e45a9b5255558ac9dfcf7",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "818f875df04a49eebe9c474bba4247b9",
        PresetsId = "50959c7c201e45a9b5255558ac9dfcf7",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "81acc249e2664c72908014a28e1e72ef",
        PresetsId = "50959c7c201e45a9b5255558ac9dfcf7",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "c454b97a93f341e78b127d5425acc464",
        PresetsId = "50959c7c201e45a9b5255558ac9dfcf7",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "fecd26fe3b6b4e88ba97ebb406fa6df2",
        PresetsId = "50959c7c201e45a9b5255558ac9dfcf7",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "31c853d3dde94e68ab57903cf8a80798",
        PresetsId = "58d759c12387490fb3a97c4633d6f1bb",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "8158b8e5ef0340bdaf830fa8eb2e650d",
        PresetsId = "58d759c12387490fb3a97c4633d6f1bb",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "8cff38afa8464ae191ba152b93438266",
        PresetsId = "58d759c12387490fb3a97c4633d6f1bb",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "92bdbbdbfd3b4dcf8256b68a66dfa35b",
        PresetsId = "58d759c12387490fb3a97c4633d6f1bb",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "c454b97a93f341e78b127d5425acc464",
        PresetsId = "58d759c12387490fb3a97c4633d6f1bb",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "fecd26fe3b6b4e88ba97ebb406fa6df2",
        PresetsId = "58d759c12387490fb3a97c4633d6f1bb",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "31c853d3dde94e68ab57903cf8a80798",
        PresetsId = "5a5081ce720141188c09336e4a936ef9",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "4ed7cdc105ab44de81d333d63db0a222",
        PresetsId = "5a5081ce720141188c09336e4a936ef9",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "8158b8e5ef0340bdaf830fa8eb2e650d",
        PresetsId = "5a5081ce720141188c09336e4a936ef9",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "8cff38afa8464ae191ba152b93438266",
        PresetsId = "5a5081ce720141188c09336e4a936ef9",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "92bdbbdbfd3b4dcf8256b68a66dfa35b",
        PresetsId = "5a5081ce720141188c09336e4a936ef9",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "c454b97a93f341e78b127d5425acc464",
        PresetsId = "5a5081ce720141188c09336e4a936ef9",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "31c853d3dde94e68ab57903cf8a80798",
        PresetsId = "5ef79dda17e64808afa6287c07c8bac9",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "4ed7cdc105ab44de81d333d63db0a222",
        PresetsId = "5ef79dda17e64808afa6287c07c8bac9",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "70b69ec5ea074c0f8bf904b20b9c7884",
        PresetsId = "5ef79dda17e64808afa6287c07c8bac9",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "8158b8e5ef0340bdaf830fa8eb2e650d",
        PresetsId = "5ef79dda17e64808afa6287c07c8bac9",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "92bdbbdbfd3b4dcf8256b68a66dfa35b",
        PresetsId = "5ef79dda17e64808afa6287c07c8bac9",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "c454b97a93f341e78b127d5425acc464",
        PresetsId = "5ef79dda17e64808afa6287c07c8bac9",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "06e6899bd6544dd3b9020a50789d9421",
        PresetsId = "68ee38a6554f407abd932d492719a834",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "31c853d3dde94e68ab57903cf8a80798",
        PresetsId = "68ee38a6554f407abd932d492719a834",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "818f875df04a49eebe9c474bba4247b9",
        PresetsId = "68ee38a6554f407abd932d492719a834",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "8cff38afa8464ae191ba152b93438266",
        PresetsId = "68ee38a6554f407abd932d492719a834",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "c454b97a93f341e78b127d5425acc464",
        PresetsId = "68ee38a6554f407abd932d492719a834",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "fecd26fe3b6b4e88ba97ebb406fa6df2",
        PresetsId = "68ee38a6554f407abd932d492719a834",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "31c853d3dde94e68ab57903cf8a80798",
        PresetsId = "68fca8f086454898ae81b1ea29890b60",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "4ed7cdc105ab44de81d333d63db0a222",
        PresetsId = "68fca8f086454898ae81b1ea29890b60",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "81acc249e2664c72908014a28e1e72ef",
        PresetsId = "68fca8f086454898ae81b1ea29890b60",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "8cff38afa8464ae191ba152b93438266",
        PresetsId = "68fca8f086454898ae81b1ea29890b60",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "92bdbbdbfd3b4dcf8256b68a66dfa35b",
        PresetsId = "68fca8f086454898ae81b1ea29890b60",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "c454b97a93f341e78b127d5425acc464",
        PresetsId = "68fca8f086454898ae81b1ea29890b60",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "06e6899bd6544dd3b9020a50789d9421",
        PresetsId = "694d8135f4ca4238956e9f47460b5dff",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "31c853d3dde94e68ab57903cf8a80798",
        PresetsId = "694d8135f4ca4238956e9f47460b5dff",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "70b69ec5ea074c0f8bf904b20b9c7884",
        PresetsId = "694d8135f4ca4238956e9f47460b5dff",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "818f875df04a49eebe9c474bba4247b9",
        PresetsId = "694d8135f4ca4238956e9f47460b5dff",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "c454b97a93f341e78b127d5425acc464",
        PresetsId = "694d8135f4ca4238956e9f47460b5dff",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "fecd26fe3b6b4e88ba97ebb406fa6df2",
        PresetsId = "694d8135f4ca4238956e9f47460b5dff",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "31c853d3dde94e68ab57903cf8a80798",
        PresetsId = "6ab0daed7fe54e9fa791bfcb6bf1a043",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "81acc249e2664c72908014a28e1e72ef",
        PresetsId = "6ab0daed7fe54e9fa791bfcb6bf1a043",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "8cff38afa8464ae191ba152b93438266",
        PresetsId = "6ab0daed7fe54e9fa791bfcb6bf1a043",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "92bdbbdbfd3b4dcf8256b68a66dfa35b",
        PresetsId = "6ab0daed7fe54e9fa791bfcb6bf1a043",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "c454b97a93f341e78b127d5425acc464",
        PresetsId = "6ab0daed7fe54e9fa791bfcb6bf1a043",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "fecd26fe3b6b4e88ba97ebb406fa6df2",
        PresetsId = "6ab0daed7fe54e9fa791bfcb6bf1a043",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "31c853d3dde94e68ab57903cf8a80798",
        PresetsId = "6fd08876dd804c01983236cb5277b406",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "78b78938aefe4bc6b927ed3722bc5859",
        PresetsId = "6fd08876dd804c01983236cb5277b406",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "8158b8e5ef0340bdaf830fa8eb2e650d",
        PresetsId = "6fd08876dd804c01983236cb5277b406",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "818f875df04a49eebe9c474bba4247b9",
        PresetsId = "6fd08876dd804c01983236cb5277b406",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "8cff38afa8464ae191ba152b93438266",
        PresetsId = "6fd08876dd804c01983236cb5277b406",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "c454b97a93f341e78b127d5425acc464",
        PresetsId = "6fd08876dd804c01983236cb5277b406",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "06e6899bd6544dd3b9020a50789d9421",
        PresetsId = "7abc9feb7c1e47778ec32b24aac95985",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "31c853d3dde94e68ab57903cf8a80798",
        PresetsId = "7abc9feb7c1e47778ec32b24aac95985",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "70b69ec5ea074c0f8bf904b20b9c7884",
        PresetsId = "7abc9feb7c1e47778ec32b24aac95985",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "78b78938aefe4bc6b927ed3722bc5859",
        PresetsId = "7abc9feb7c1e47778ec32b24aac95985",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "818f875df04a49eebe9c474bba4247b9",
        PresetsId = "7abc9feb7c1e47778ec32b24aac95985",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "c454b97a93f341e78b127d5425acc464",
        PresetsId = "7abc9feb7c1e47778ec32b24aac95985",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "31c853d3dde94e68ab57903cf8a80798",
        PresetsId = "7ae7818fdf09466f819808beed30e707",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "818f875df04a49eebe9c474bba4247b9",
        PresetsId = "7ae7818fdf09466f819808beed30e707",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "81acc249e2664c72908014a28e1e72ef",
        PresetsId = "7ae7818fdf09466f819808beed30e707",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "8cff38afa8464ae191ba152b93438266",
        PresetsId = "7ae7818fdf09466f819808beed30e707",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "c454b97a93f341e78b127d5425acc464",
        PresetsId = "7ae7818fdf09466f819808beed30e707",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "fecd26fe3b6b4e88ba97ebb406fa6df2",
        PresetsId = "7ae7818fdf09466f819808beed30e707",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "31c853d3dde94e68ab57903cf8a80798",
        PresetsId = "7c2f591168f144d199a8d9e174addc45",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "70b69ec5ea074c0f8bf904b20b9c7884",
        PresetsId = "7c2f591168f144d199a8d9e174addc45",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "81acc249e2664c72908014a28e1e72ef",
        PresetsId = "7c2f591168f144d199a8d9e174addc45",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "92bdbbdbfd3b4dcf8256b68a66dfa35b",
        PresetsId = "7c2f591168f144d199a8d9e174addc45",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "c454b97a93f341e78b127d5425acc464",
        PresetsId = "7c2f591168f144d199a8d9e174addc45",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "fecd26fe3b6b4e88ba97ebb406fa6df2",
        PresetsId = "7c2f591168f144d199a8d9e174addc45",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "31c853d3dde94e68ab57903cf8a80798",
        PresetsId = "7faa6eb739d548bf9f9ce7d3de978d3d",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "8158b8e5ef0340bdaf830fa8eb2e650d",
        PresetsId = "7faa6eb739d548bf9f9ce7d3de978d3d",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "818f875df04a49eebe9c474bba4247b9",
        PresetsId = "7faa6eb739d548bf9f9ce7d3de978d3d",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "8cff38afa8464ae191ba152b93438266",
        PresetsId = "7faa6eb739d548bf9f9ce7d3de978d3d",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "c454b97a93f341e78b127d5425acc464",
        PresetsId = "7faa6eb739d548bf9f9ce7d3de978d3d",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "fecd26fe3b6b4e88ba97ebb406fa6df2",
        PresetsId = "7faa6eb739d548bf9f9ce7d3de978d3d",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "31c853d3dde94e68ab57903cf8a80798",
        PresetsId = "8311c9434d354d689a5049c5041026ea",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "6b81101144404b9ab306235861eedc12",
        PresetsId = "8311c9434d354d689a5049c5041026ea",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "7186b0e8de2348fdbde87105c0f9abe3",
        PresetsId = "8311c9434d354d689a5049c5041026ea",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "818f875df04a49eebe9c474bba4247b9",
        PresetsId = "8311c9434d354d689a5049c5041026ea",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "e2c03da33b12458694a1b01c3ec66474",
        PresetsId = "8311c9434d354d689a5049c5041026ea",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "fecd26fe3b6b4e88ba97ebb406fa6df2",
        PresetsId = "8311c9434d354d689a5049c5041026ea",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "31c853d3dde94e68ab57903cf8a80798",
        PresetsId = "88d4e4ed5bbe49fc81059e4f7bf45de3",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "4ed7cdc105ab44de81d333d63db0a222",
        PresetsId = "88d4e4ed5bbe49fc81059e4f7bf45de3",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "66645e2f8fa94d228d87450cc4a74706",
        PresetsId = "88d4e4ed5bbe49fc81059e4f7bf45de3",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "6b81101144404b9ab306235861eedc12",
        PresetsId = "88d4e4ed5bbe49fc81059e4f7bf45de3",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "8158b8e5ef0340bdaf830fa8eb2e650d",
        PresetsId = "88d4e4ed5bbe49fc81059e4f7bf45de3",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "92bdbbdbfd3b4dcf8256b68a66dfa35b",
        PresetsId = "88d4e4ed5bbe49fc81059e4f7bf45de3",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "31c853d3dde94e68ab57903cf8a80798",
        PresetsId = "9d84ba5aa4404a6b8c3daa1648ef947a",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "4ed7cdc105ab44de81d333d63db0a222",
        PresetsId = "9d84ba5aa4404a6b8c3daa1648ef947a",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "6b81101144404b9ab306235861eedc12",
        PresetsId = "9d84ba5aa4404a6b8c3daa1648ef947a",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "7186b0e8de2348fdbde87105c0f9abe3",
        PresetsId = "9d84ba5aa4404a6b8c3daa1648ef947a",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "818f875df04a49eebe9c474bba4247b9",
        PresetsId = "9d84ba5aa4404a6b8c3daa1648ef947a",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "d63a7d0c9e5d4e258285ded1974bf9cf",
        PresetsId = "9d84ba5aa4404a6b8c3daa1648ef947a",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "06e6899bd6544dd3b9020a50789d9421",
        PresetsId = "a80f79e565d240af9d77d2550c6e2933",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "31c853d3dde94e68ab57903cf8a80798",
        PresetsId = "a80f79e565d240af9d77d2550c6e2933",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "70b69ec5ea074c0f8bf904b20b9c7884",
        PresetsId = "a80f79e565d240af9d77d2550c6e2933",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "78b78938aefe4bc6b927ed3722bc5859",
        PresetsId = "a80f79e565d240af9d77d2550c6e2933",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "92bdbbdbfd3b4dcf8256b68a66dfa35b",
        PresetsId = "a80f79e565d240af9d77d2550c6e2933",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "c454b97a93f341e78b127d5425acc464",
        PresetsId = "a80f79e565d240af9d77d2550c6e2933",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "31c853d3dde94e68ab57903cf8a80798",
        PresetsId = "a90e932d3ff14830b3c4c95dd42e9ff8",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "44ecb280dd8f4137af58024e0780e994",
        PresetsId = "a90e932d3ff14830b3c4c95dd42e9ff8",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "6b81101144404b9ab306235861eedc12",
        PresetsId = "a90e932d3ff14830b3c4c95dd42e9ff8",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "6eb66f2427eb47a1a1fffc0dd3f89f6e",
        PresetsId = "a90e932d3ff14830b3c4c95dd42e9ff8",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "7186b0e8de2348fdbde87105c0f9abe3",
        PresetsId = "a90e932d3ff14830b3c4c95dd42e9ff8",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "d63a7d0c9e5d4e258285ded1974bf9cf",
        PresetsId = "a90e932d3ff14830b3c4c95dd42e9ff8",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "31c853d3dde94e68ab57903cf8a80798",
        PresetsId = "ac1b2b6941c04598ae5a50d48efd8232",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "4ed7cdc105ab44de81d333d63db0a222",
        PresetsId = "ac1b2b6941c04598ae5a50d48efd8232",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "70b69ec5ea074c0f8bf904b20b9c7884",
        PresetsId = "ac1b2b6941c04598ae5a50d48efd8232",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "81acc249e2664c72908014a28e1e72ef",
        PresetsId = "ac1b2b6941c04598ae5a50d48efd8232",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "92bdbbdbfd3b4dcf8256b68a66dfa35b",
        PresetsId = "ac1b2b6941c04598ae5a50d48efd8232",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "c454b97a93f341e78b127d5425acc464",
        PresetsId = "ac1b2b6941c04598ae5a50d48efd8232",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "31c853d3dde94e68ab57903cf8a80798",
        PresetsId = "af643fe22b414985b6896d0532875528",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "44ecb280dd8f4137af58024e0780e994",
        PresetsId = "af643fe22b414985b6896d0532875528",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "4ed7cdc105ab44de81d333d63db0a222",
        PresetsId = "af643fe22b414985b6896d0532875528",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "6b81101144404b9ab306235861eedc12",
        PresetsId = "af643fe22b414985b6896d0532875528",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "7186b0e8de2348fdbde87105c0f9abe3",
        PresetsId = "af643fe22b414985b6896d0532875528",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "d63a7d0c9e5d4e258285ded1974bf9cf",
        PresetsId = "af643fe22b414985b6896d0532875528",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "31c853d3dde94e68ab57903cf8a80798",
        PresetsId = "b0daa08ae6064686a59c0835bf58145c",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "70b69ec5ea074c0f8bf904b20b9c7884",
        PresetsId = "b0daa08ae6064686a59c0835bf58145c",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "78b78938aefe4bc6b927ed3722bc5859",
        PresetsId = "b0daa08ae6064686a59c0835bf58145c",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "81acc249e2664c72908014a28e1e72ef",
        PresetsId = "b0daa08ae6064686a59c0835bf58145c",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "92bdbbdbfd3b4dcf8256b68a66dfa35b",
        PresetsId = "b0daa08ae6064686a59c0835bf58145c",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "c454b97a93f341e78b127d5425acc464",
        PresetsId = "b0daa08ae6064686a59c0835bf58145c",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "31c853d3dde94e68ab57903cf8a80798",
        PresetsId = "b9c4df5bcd4240f98a86ba7fd55faea6",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "4ed7cdc105ab44de81d333d63db0a222",
        PresetsId = "b9c4df5bcd4240f98a86ba7fd55faea6",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "6b81101144404b9ab306235861eedc12",
        PresetsId = "b9c4df5bcd4240f98a86ba7fd55faea6",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "8158b8e5ef0340bdaf830fa8eb2e650d",
        PresetsId = "b9c4df5bcd4240f98a86ba7fd55faea6",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "92bdbbdbfd3b4dcf8256b68a66dfa35b",
        PresetsId = "b9c4df5bcd4240f98a86ba7fd55faea6",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "d63a7d0c9e5d4e258285ded1974bf9cf",
        PresetsId = "b9c4df5bcd4240f98a86ba7fd55faea6",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "31c853d3dde94e68ab57903cf8a80798",
        PresetsId = "bac18c4350314174b9f4cbe73ca0b24d",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "4ed7cdc105ab44de81d333d63db0a222",
        PresetsId = "bac18c4350314174b9f4cbe73ca0b24d",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "8158b8e5ef0340bdaf830fa8eb2e650d",
        PresetsId = "bac18c4350314174b9f4cbe73ca0b24d",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "818f875df04a49eebe9c474bba4247b9",
        PresetsId = "bac18c4350314174b9f4cbe73ca0b24d",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "8cff38afa8464ae191ba152b93438266",
        PresetsId = "bac18c4350314174b9f4cbe73ca0b24d",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "c454b97a93f341e78b127d5425acc464",
        PresetsId = "bac18c4350314174b9f4cbe73ca0b24d",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "06e6899bd6544dd3b9020a50789d9421",
        PresetsId = "bcdcf89795b2484f96a3a12b25383a12",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "31c853d3dde94e68ab57903cf8a80798",
        PresetsId = "bcdcf89795b2484f96a3a12b25383a12",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "78b78938aefe4bc6b927ed3722bc5859",
        PresetsId = "bcdcf89795b2484f96a3a12b25383a12",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "818f875df04a49eebe9c474bba4247b9",
        PresetsId = "bcdcf89795b2484f96a3a12b25383a12",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "8cff38afa8464ae191ba152b93438266",
        PresetsId = "bcdcf89795b2484f96a3a12b25383a12",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "c454b97a93f341e78b127d5425acc464",
        PresetsId = "bcdcf89795b2484f96a3a12b25383a12",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "31c853d3dde94e68ab57903cf8a80798",
        PresetsId = "be7b594cc1274fb5b9c4fa5571ed8952",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "78b78938aefe4bc6b927ed3722bc5859",
        PresetsId = "be7b594cc1274fb5b9c4fa5571ed8952",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "81acc249e2664c72908014a28e1e72ef",
        PresetsId = "be7b594cc1274fb5b9c4fa5571ed8952",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "8cff38afa8464ae191ba152b93438266",
        PresetsId = "be7b594cc1274fb5b9c4fa5571ed8952",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "92bdbbdbfd3b4dcf8256b68a66dfa35b",
        PresetsId = "be7b594cc1274fb5b9c4fa5571ed8952",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "c454b97a93f341e78b127d5425acc464",
        PresetsId = "be7b594cc1274fb5b9c4fa5571ed8952",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "31c853d3dde94e68ab57903cf8a80798",
        PresetsId = "c3797a5e9c9a42dba09b0001c8cdaed6",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "6b81101144404b9ab306235861eedc12",
        PresetsId = "c3797a5e9c9a42dba09b0001c8cdaed6",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "6eb66f2427eb47a1a1fffc0dd3f89f6e",
        PresetsId = "c3797a5e9c9a42dba09b0001c8cdaed6",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "7186b0e8de2348fdbde87105c0f9abe3",
        PresetsId = "c3797a5e9c9a42dba09b0001c8cdaed6",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "818f875df04a49eebe9c474bba4247b9",
        PresetsId = "c3797a5e9c9a42dba09b0001c8cdaed6",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "d63a7d0c9e5d4e258285ded1974bf9cf",
        PresetsId = "c3797a5e9c9a42dba09b0001c8cdaed6",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "06e6899bd6544dd3b9020a50789d9421",
        PresetsId = "c9348ab0418743d08ee786e31083d76b",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "31c853d3dde94e68ab57903cf8a80798",
        PresetsId = "c9348ab0418743d08ee786e31083d76b",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "70b69ec5ea074c0f8bf904b20b9c7884",
        PresetsId = "c9348ab0418743d08ee786e31083d76b",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "92bdbbdbfd3b4dcf8256b68a66dfa35b",
        PresetsId = "c9348ab0418743d08ee786e31083d76b",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "c454b97a93f341e78b127d5425acc464",
        PresetsId = "c9348ab0418743d08ee786e31083d76b",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "fecd26fe3b6b4e88ba97ebb406fa6df2",
        PresetsId = "c9348ab0418743d08ee786e31083d76b",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "31c853d3dde94e68ab57903cf8a80798",
        PresetsId = "caa36b99b068411aaa62b0f4234c1656",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "4ed7cdc105ab44de81d333d63db0a222",
        PresetsId = "caa36b99b068411aaa62b0f4234c1656",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "818f875df04a49eebe9c474bba4247b9",
        PresetsId = "caa36b99b068411aaa62b0f4234c1656",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "81acc249e2664c72908014a28e1e72ef",
        PresetsId = "caa36b99b068411aaa62b0f4234c1656",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "8cff38afa8464ae191ba152b93438266",
        PresetsId = "caa36b99b068411aaa62b0f4234c1656",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "c454b97a93f341e78b127d5425acc464",
        PresetsId = "caa36b99b068411aaa62b0f4234c1656",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "31c853d3dde94e68ab57903cf8a80798",
        PresetsId = "cc5b36affe2243f2b119fd65cb3434d5",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "4ed7cdc105ab44de81d333d63db0a222",
        PresetsId = "cc5b36affe2243f2b119fd65cb3434d5",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "70b69ec5ea074c0f8bf904b20b9c7884",
        PresetsId = "cc5b36affe2243f2b119fd65cb3434d5",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "8158b8e5ef0340bdaf830fa8eb2e650d",
        PresetsId = "cc5b36affe2243f2b119fd65cb3434d5",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "818f875df04a49eebe9c474bba4247b9",
        PresetsId = "cc5b36affe2243f2b119fd65cb3434d5",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "c454b97a93f341e78b127d5425acc464",
        PresetsId = "cc5b36affe2243f2b119fd65cb3434d5",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "31c853d3dde94e68ab57903cf8a80798",
        PresetsId = "ccc662f99d7a41d7aa1505e3a2cb8950",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "70b69ec5ea074c0f8bf904b20b9c7884",
        PresetsId = "ccc662f99d7a41d7aa1505e3a2cb8950",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "8158b8e5ef0340bdaf830fa8eb2e650d",
        PresetsId = "ccc662f99d7a41d7aa1505e3a2cb8950",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "818f875df04a49eebe9c474bba4247b9",
        PresetsId = "ccc662f99d7a41d7aa1505e3a2cb8950",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "c454b97a93f341e78b127d5425acc464",
        PresetsId = "ccc662f99d7a41d7aa1505e3a2cb8950",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "fecd26fe3b6b4e88ba97ebb406fa6df2",
        PresetsId = "ccc662f99d7a41d7aa1505e3a2cb8950",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "31c853d3dde94e68ab57903cf8a80798",
        PresetsId = "cfdabdeb4fd748849e6a635ec6418dd2",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "6b81101144404b9ab306235861eedc12",
        PresetsId = "cfdabdeb4fd748849e6a635ec6418dd2",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "6eb66f2427eb47a1a1fffc0dd3f89f6e",
        PresetsId = "cfdabdeb4fd748849e6a635ec6418dd2",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "8158b8e5ef0340bdaf830fa8eb2e650d",
        PresetsId = "cfdabdeb4fd748849e6a635ec6418dd2",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "92bdbbdbfd3b4dcf8256b68a66dfa35b",
        PresetsId = "cfdabdeb4fd748849e6a635ec6418dd2",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "d63a7d0c9e5d4e258285ded1974bf9cf",
        PresetsId = "cfdabdeb4fd748849e6a635ec6418dd2",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "06e6899bd6544dd3b9020a50789d9421",
        PresetsId = "d1d9616eb7904f189659505eca16cd43",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "31c853d3dde94e68ab57903cf8a80798",
        PresetsId = "d1d9616eb7904f189659505eca16cd43",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "6b81101144404b9ab306235861eedc12",
        PresetsId = "d1d9616eb7904f189659505eca16cd43",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "6eb66f2427eb47a1a1fffc0dd3f89f6e",
        PresetsId = "d1d9616eb7904f189659505eca16cd43",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "92bdbbdbfd3b4dcf8256b68a66dfa35b",
        PresetsId = "d1d9616eb7904f189659505eca16cd43",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "d63a7d0c9e5d4e258285ded1974bf9cf",
        PresetsId = "d1d9616eb7904f189659505eca16cd43",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "31c853d3dde94e68ab57903cf8a80798",
        PresetsId = "d208c36d9fa640c48eb05f6dd19963e2",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "44ecb280dd8f4137af58024e0780e994",
        PresetsId = "d208c36d9fa640c48eb05f6dd19963e2",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "6b81101144404b9ab306235861eedc12",
        PresetsId = "d208c36d9fa640c48eb05f6dd19963e2",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "6eb66f2427eb47a1a1fffc0dd3f89f6e",
        PresetsId = "d208c36d9fa640c48eb05f6dd19963e2",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "7186b0e8de2348fdbde87105c0f9abe3",
        PresetsId = "d208c36d9fa640c48eb05f6dd19963e2",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "e2c03da33b12458694a1b01c3ec66474",
        PresetsId = "d208c36d9fa640c48eb05f6dd19963e2",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "31c853d3dde94e68ab57903cf8a80798",
        PresetsId = "dd737fac7e8b4b0cb353b6d8ac16cb51",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "70b69ec5ea074c0f8bf904b20b9c7884",
        PresetsId = "dd737fac7e8b4b0cb353b6d8ac16cb51",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "78b78938aefe4bc6b927ed3722bc5859",
        PresetsId = "dd737fac7e8b4b0cb353b6d8ac16cb51",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "8158b8e5ef0340bdaf830fa8eb2e650d",
        PresetsId = "dd737fac7e8b4b0cb353b6d8ac16cb51",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "92bdbbdbfd3b4dcf8256b68a66dfa35b",
        PresetsId = "dd737fac7e8b4b0cb353b6d8ac16cb51",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "c454b97a93f341e78b127d5425acc464",
        PresetsId = "dd737fac7e8b4b0cb353b6d8ac16cb51",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "31c853d3dde94e68ab57903cf8a80798",
        PresetsId = "e3485b7dc853448588da3dde04c4e3eb",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "44ecb280dd8f4137af58024e0780e994",
        PresetsId = "e3485b7dc853448588da3dde04c4e3eb",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "6b81101144404b9ab306235861eedc12",
        PresetsId = "e3485b7dc853448588da3dde04c4e3eb",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "7186b0e8de2348fdbde87105c0f9abe3",
        PresetsId = "e3485b7dc853448588da3dde04c4e3eb",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "e2c03da33b12458694a1b01c3ec66474",
        PresetsId = "e3485b7dc853448588da3dde04c4e3eb",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "fecd26fe3b6b4e88ba97ebb406fa6df2",
        PresetsId = "e3485b7dc853448588da3dde04c4e3eb",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "31c853d3dde94e68ab57903cf8a80798",
        PresetsId = "f1beda0c907a40dfaa73738cd6313d03",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "66645e2f8fa94d228d87450cc4a74706",
        PresetsId = "f1beda0c907a40dfaa73738cd6313d03",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "6b81101144404b9ab306235861eedc12",
        PresetsId = "f1beda0c907a40dfaa73738cd6313d03",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "6eb66f2427eb47a1a1fffc0dd3f89f6e",
        PresetsId = "f1beda0c907a40dfaa73738cd6313d03",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "8158b8e5ef0340bdaf830fa8eb2e650d",
        PresetsId = "f1beda0c907a40dfaa73738cd6313d03",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "92bdbbdbfd3b4dcf8256b68a66dfa35b",
        PresetsId = "f1beda0c907a40dfaa73738cd6313d03",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "31c853d3dde94e68ab57903cf8a80798",
        PresetsId = "f1ec1932c78e4a5a8035df1864fb32a4",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "4ed7cdc105ab44de81d333d63db0a222",
        PresetsId = "f1ec1932c78e4a5a8035df1864fb32a4",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "70b69ec5ea074c0f8bf904b20b9c7884",
        PresetsId = "f1ec1932c78e4a5a8035df1864fb32a4",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "818f875df04a49eebe9c474bba4247b9",
        PresetsId = "f1ec1932c78e4a5a8035df1864fb32a4",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "81acc249e2664c72908014a28e1e72ef",
        PresetsId = "f1ec1932c78e4a5a8035df1864fb32a4",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "c454b97a93f341e78b127d5425acc464",
        PresetsId = "f1ec1932c78e4a5a8035df1864fb32a4",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "06e6899bd6544dd3b9020a50789d9421",
        PresetsId = "fe7043a37995453db2d03741e81da9df",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "31c853d3dde94e68ab57903cf8a80798",
        PresetsId = "fe7043a37995453db2d03741e81da9df",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "8cff38afa8464ae191ba152b93438266",
        PresetsId = "fe7043a37995453db2d03741e81da9df",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "92bdbbdbfd3b4dcf8256b68a66dfa35b",
        PresetsId = "fe7043a37995453db2d03741e81da9df",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "c454b97a93f341e78b127d5425acc464",
        PresetsId = "fe7043a37995453db2d03741e81da9df",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "fecd26fe3b6b4e88ba97ebb406fa6df2",
        PresetsId = "fe7043a37995453db2d03741e81da9df",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "06e6899bd6544dd3b9020a50789d9421",
        PresetsId = "fe88b53dce4c4938a7785fb9e3db1f0a",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "31c853d3dde94e68ab57903cf8a80798",
        PresetsId = "fe88b53dce4c4938a7785fb9e3db1f0a",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "4ed7cdc105ab44de81d333d63db0a222",
        PresetsId = "fe88b53dce4c4938a7785fb9e3db1f0a",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "8cff38afa8464ae191ba152b93438266",
        PresetsId = "fe88b53dce4c4938a7785fb9e3db1f0a",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "92bdbbdbfd3b4dcf8256b68a66dfa35b",
        PresetsId = "fe88b53dce4c4938a7785fb9e3db1f0a",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "c454b97a93f341e78b127d5425acc464",
        PresetsId = "fe88b53dce4c4938a7785fb9e3db1f0a",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "31c853d3dde94e68ab57903cf8a80798",
        PresetsId = "ff13d99b031a4a93b571c0a0dfdf5770",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "44ecb280dd8f4137af58024e0780e994",
        PresetsId = "ff13d99b031a4a93b571c0a0dfdf5770",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "6b81101144404b9ab306235861eedc12",
        PresetsId = "ff13d99b031a4a93b571c0a0dfdf5770",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "7186b0e8de2348fdbde87105c0f9abe3",
        PresetsId = "ff13d99b031a4a93b571c0a0dfdf5770",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "d63a7d0c9e5d4e258285ded1974bf9cf",
        PresetsId = "ff13d99b031a4a93b571c0a0dfdf5770",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "fecd26fe3b6b4e88ba97ebb406fa6df2",
        PresetsId = "ff13d99b031a4a93b571c0a0dfdf5770",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "6b81101144404b9ab306235861eedc12",
        PresetsId = "edaba02c35264bfcbddd4d80ee293da6",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "6eb66f2427eb47a1a1fffc0dd3f89f6e",
        PresetsId = "edaba02c35264bfcbddd4d80ee293da6",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "7186b0e8de2348fdbde87105c0f9abe3",
        PresetsId = "edaba02c35264bfcbddd4d80ee293da6",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "ac73834982664ae09fc86ba4359e4aa8",
        PresetsId = "edaba02c35264bfcbddd4d80ee293da6",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "c454b97a93f341e78b127d5425acc464",
        PresetsId = "edaba02c35264bfcbddd4d80ee293da6",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "e639e41ce412489cbd74fbf43428344d",
        PresetsId = "edaba02c35264bfcbddd4d80ee293da6",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "6b81101144404b9ab306235861eedc12",
        PresetsId = "bb4fa5ef967845ed9df5ff26d2dd1c4c",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "6eb66f2427eb47a1a1fffc0dd3f89f6e",
        PresetsId = "bb4fa5ef967845ed9df5ff26d2dd1c4c",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "81acc249e2664c72908014a28e1e72ef",
        PresetsId = "bb4fa5ef967845ed9df5ff26d2dd1c4c",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "ac73834982664ae09fc86ba4359e4aa8",
        PresetsId = "bb4fa5ef967845ed9df5ff26d2dd1c4c",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "c454b97a93f341e78b127d5425acc464",
        PresetsId = "bb4fa5ef967845ed9df5ff26d2dd1c4c",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "e639e41ce412489cbd74fbf43428344d",
        PresetsId = "bb4fa5ef967845ed9df5ff26d2dd1c4c",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "4ed7cdc105ab44de81d333d63db0a222",
        PresetsId = "54963650e77e411daa0afe6cf4e0bd87",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "6b81101144404b9ab306235861eedc12",
        PresetsId = "54963650e77e411daa0afe6cf4e0bd87",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "7186b0e8de2348fdbde87105c0f9abe3",
        PresetsId = "54963650e77e411daa0afe6cf4e0bd87",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "ac73834982664ae09fc86ba4359e4aa8",
        PresetsId = "54963650e77e411daa0afe6cf4e0bd87",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "c454b97a93f341e78b127d5425acc464",
        PresetsId = "54963650e77e411daa0afe6cf4e0bd87",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "e639e41ce412489cbd74fbf43428344d",
        PresetsId = "54963650e77e411daa0afe6cf4e0bd87",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "4ed7cdc105ab44de81d333d63db0a222",
        PresetsId = "dd486595e71d444cb8f42d0c91f5a0e7",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "6b81101144404b9ab306235861eedc12",
        PresetsId = "dd486595e71d444cb8f42d0c91f5a0e7",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "81acc249e2664c72908014a28e1e72ef",
        PresetsId = "dd486595e71d444cb8f42d0c91f5a0e7",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "ac73834982664ae09fc86ba4359e4aa8",
        PresetsId = "dd486595e71d444cb8f42d0c91f5a0e7",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "c454b97a93f341e78b127d5425acc464",
        PresetsId = "dd486595e71d444cb8f42d0c91f5a0e7",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "e639e41ce412489cbd74fbf43428344d",
        PresetsId = "dd486595e71d444cb8f42d0c91f5a0e7",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "66645e2f8fa94d228d87450cc4a74706",
        PresetsId = "461e2cae39e14726b2cd1772cdb4c66e",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "6b81101144404b9ab306235861eedc12",
        PresetsId = "461e2cae39e14726b2cd1772cdb4c66e",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "6eb66f2427eb47a1a1fffc0dd3f89f6e",
        PresetsId = "461e2cae39e14726b2cd1772cdb4c66e",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "7186b0e8de2348fdbde87105c0f9abe3",
        PresetsId = "461e2cae39e14726b2cd1772cdb4c66e",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "ac73834982664ae09fc86ba4359e4aa8",
        PresetsId = "461e2cae39e14726b2cd1772cdb4c66e",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "e639e41ce412489cbd74fbf43428344d",
        PresetsId = "461e2cae39e14726b2cd1772cdb4c66e",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "66645e2f8fa94d228d87450cc4a74706",
        PresetsId = "7afdd951b8f948519388689b84373fba",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "6b81101144404b9ab306235861eedc12",
        PresetsId = "7afdd951b8f948519388689b84373fba",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "6eb66f2427eb47a1a1fffc0dd3f89f6e",
        PresetsId = "7afdd951b8f948519388689b84373fba",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "81acc249e2664c72908014a28e1e72ef",
        PresetsId = "7afdd951b8f948519388689b84373fba",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "ac73834982664ae09fc86ba4359e4aa8",
        PresetsId = "7afdd951b8f948519388689b84373fba",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "e639e41ce412489cbd74fbf43428344d",
        PresetsId = "7afdd951b8f948519388689b84373fba",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "4ed7cdc105ab44de81d333d63db0a222",
        PresetsId = "7f7a036811a14c9ba4058fa770b9a7e6",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "66645e2f8fa94d228d87450cc4a74706",
        PresetsId = "7f7a036811a14c9ba4058fa770b9a7e6",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "6b81101144404b9ab306235861eedc12",
        PresetsId = "7f7a036811a14c9ba4058fa770b9a7e6",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "7186b0e8de2348fdbde87105c0f9abe3",
        PresetsId = "7f7a036811a14c9ba4058fa770b9a7e6",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "ac73834982664ae09fc86ba4359e4aa8",
        PresetsId = "7f7a036811a14c9ba4058fa770b9a7e6",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "e639e41ce412489cbd74fbf43428344d",
        PresetsId = "7f7a036811a14c9ba4058fa770b9a7e6",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "4ed7cdc105ab44de81d333d63db0a222",
        PresetsId = "e4de6c02c26a42a6bc1ba0b9ac158774",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "66645e2f8fa94d228d87450cc4a74706",
        PresetsId = "e4de6c02c26a42a6bc1ba0b9ac158774",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "6b81101144404b9ab306235861eedc12",
        PresetsId = "e4de6c02c26a42a6bc1ba0b9ac158774",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "81acc249e2664c72908014a28e1e72ef",
        PresetsId = "e4de6c02c26a42a6bc1ba0b9ac158774",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "ac73834982664ae09fc86ba4359e4aa8",
        PresetsId = "e4de6c02c26a42a6bc1ba0b9ac158774",
    },
    new ComponentOptionPreset
    {
        ComponentOptionsId = "e639e41ce412489cbd74fbf43428344d",
        PresetsId = "e4de6c02c26a42a6bc1ba0b9ac158774",
    }
);
            #endregion

            #endregion

            #region Seed Positions, Sizes, and AddOns

            modelBuilder.Entity<Position>().HasData(
                                        new Position
                                        {
                                            Id = "dddef2e0837e4461a3edf6f6fc4ca6a5",
                                            Name = "Right Sleeve Hem",
                                            Image = null,
                                            CreatedBy = "System",
                                            UpdatedBy = "System",
                                            CreatedAt = DateTime.UtcNow,
                                            UpdatedAt = DateTime.UtcNow,
                                            IsDeleted = false,
                                        },
                                        new Position
                                        {
                                            Id = "dda917d763ef470ba63a76a0c46b6dd3",
                                            Name = "Left Sleeve Hem",
                                            Image = null,
                                            CreatedBy = "System",
                                            UpdatedBy = "System",
                                            CreatedAt = DateTime.UtcNow,
                                            UpdatedAt = DateTime.UtcNow,
                                            IsDeleted = false,
                                        },
                                        new Position
                                        {
                                            Id = "1892d3778cf44b8eb791801be53d1950",
                                            Name = "Left Chest",
                                            Image = null,
                                            CreatedBy = "System",
                                            UpdatedBy = "System",
                                            CreatedAt = DateTime.UtcNow,
                                            UpdatedAt = DateTime.UtcNow,
                                            IsDeleted = false,
                                        },
                                        new Position
                                        {
                                            Id = "6351c905181f4959973341881dcbc3ac",
                                            Name = "Right Chest",
                                            Image = null,
                                            CreatedBy = "System",
                                            UpdatedBy = "System",
                                            CreatedAt = DateTime.UtcNow,
                                            UpdatedAt = DateTime.UtcNow,
                                            IsDeleted = false,
                                        },
                                        new Position
                                        {
                                            Id = "d1a6e54ea47d4b89beaa90421e819a2f",
                                            Name = "Center Chest",
                                            Image = null,
                                            CreatedBy = "System",
                                            UpdatedBy = "System",
                                            CreatedAt = DateTime.UtcNow,
                                            UpdatedAt = DateTime.UtcNow,
                                            IsDeleted = false,
                                        },
                                        new Position
                                        {
                                            Id = "18d2ffb3804c4a6d89db65961c26d687",
                                            Name = "Left Dress Side",
                                            Image = null,
                                            CreatedBy = "System",
                                            UpdatedBy = "System",
                                            CreatedAt = DateTime.UtcNow,
                                            UpdatedAt = DateTime.UtcNow,
                                            IsDeleted = false,
                                        },
                                        new Position
                                        {
                                            Id = "cf321914500e44e0ab21fffebff8f63b",
                                            Name = "Right Dress Side",
                                            Image = null,
                                            CreatedBy = "System",
                                            UpdatedBy = "System",
                                            CreatedAt = DateTime.UtcNow,
                                            UpdatedAt = DateTime.UtcNow,
                                            IsDeleted = false,
                                        },
                                        new Position
                                        {
                                            Id = "7ab17c28f8764667a8cdd92015d135a7",
                                            Name = "Full Neckline",
                                            Image = null,
                                            CreatedBy = "System",
                                            UpdatedBy = "System",
                                            CreatedAt = DateTime.UtcNow,
                                            UpdatedAt = DateTime.UtcNow,
                                            IsDeleted = false,
                                        },
                                        new Position
                                        {
                                            Id = "3a0440ea7695472f9324919567fbd548",
                                            Name = "Full Dress Hem",
                                            Image = null,
                                            CreatedBy = "System",
                                            UpdatedBy = "System",
                                            CreatedAt = DateTime.UtcNow,
                                            UpdatedAt = DateTime.UtcNow,
                                            IsDeleted = false,
                                        }
                                    );

            modelBuilder.Entity<Size>().HasData(
                new Size
                {
                    Id = "28f75887e2314369a96d17200e93e1df",
                    Name = "Small",
                    CreatedBy = "System",
                    UpdatedBy = "System",
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    IsDeleted = false,
                },
                new Size
                {
                    Id = "8d643f1e68294355aa0ba06fa333642c",
                    Name = "Medium",
                    CreatedBy = "System",
                    UpdatedBy = "System",
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    IsDeleted = false,
                },
                new Size
                {
                    Id = "b3c067654ccf41529dab7a39efa73177",
                    Name = "Large",
                    CreatedBy = "System",
                    UpdatedBy = "System",
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    IsDeleted = false,
                },
                new Size
                {
                    Id = "d500f6f2377f475b86fcaefe85ca172b",
                    Name = "Full Position",
                    CreatedBy = "System",
                    UpdatedBy = "System",
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    IsDeleted = false,
                }
            );

            modelBuilder.Entity<AddOn>().HasData(
                new AddOn
                {
                    Id = "c136acbd20b14414a8013a9b8b8ea5ab",
                    Name = "Rhinestone Attachment",
                    Description = "Decorate the dress with small, sparkling rhinestones",
                    CreatedBy = "System",
                    UpdatedBy = "System",
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    IsDeleted = false,
                },
                new AddOn
                {
                    Id = "a55a878403b54856bf6128a185a8eb4a",
                    Name = "Embroidery",
                    Description = "Embroider flowers, names, logos, or patterns for a handmade touch",
                    CreatedBy = "System",
                    UpdatedBy = "System",
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    IsDeleted = false,
                },
                new AddOn
                {
                    Id = "836ffd7371b64eb48b3679fd02ce530f",
                    Name = "Lace Applique",
                    Description = "Attach lace fabric to the dress for a feminine, elegant look",
                    CreatedBy = "System",
                    UpdatedBy = "System",
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    IsDeleted = false,
                },
                new AddOn
                {
                    Id = "b22e5a30c05147ddb90bba660269d844",
                    Name = "Bead Attachment",
                    Description = "Hand-attach beads for subtle decorative accents",
                    CreatedBy = "System",
                    UpdatedBy = "System",
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    IsDeleted = false,
                },
                new AddOn
                {
                    Id = "1d9f860375714d9a9d6f9dcbdb8f9294",
                    Name = "Personal Tag Attachment",
                    Description = "Attach a personalized tag, usually at the hem, for identification",
                    CreatedBy = "System",
                    UpdatedBy = "System",
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    IsDeleted = false,
                },
                new AddOn
                {
                    Id = "f9bacc3095014ca6913a596a7e705769",
                    Name = "Pattern Printing",
                    Description = "Print patterns, text, or images on the dress as requested",
                    CreatedBy = "System",
                    UpdatedBy = "System",
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    IsDeleted = false,
                }
            );

            modelBuilder.Entity<AddOnOption>().HasData(
                new AddOnOption
                {
                    Id = "a0fbfbb612d54e0f9ca48961c2fc7a6a",
                    AddOnId = "836ffd7371b64eb48b3679fd02ce530f",
                    PositionId = "dddef2e0837e4461a3edf6f6fc4ca6a5",
                    SizeId = "28f75887e2314369a96d17200e93e1df",
                    Name = "Geometric Lace",
                    Description = "Attach lace applique on right sleeve hem, small size",
                    Price = 950m,
                    ItemServiceType = (ItemServiceType)2,
                    CreatedBy = "System",
                    UpdatedBy = "System",
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    IsDeleted = false,
                },
                new AddOnOption
                {
                    Id = "f12ad2c32f4d49e78c36c631ea56fa2d",
                    AddOnId = "836ffd7371b64eb48b3679fd02ce530f",
                    PositionId = "dddef2e0837e4461a3edf6f6fc4ca6a5",
                    SizeId = "8d643f1e68294355aa0ba06fa333642c",
                    Name = "Vintage Lace",
                    Description = "Attach lace applique on right sleeve hem, medium size",
                    Price = 1300m,
                    ItemServiceType = (ItemServiceType)2,
                    CreatedBy = "System",
                    UpdatedBy = "System",
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    IsDeleted = false,
                },
                new AddOnOption
                {
                    Id = "c289f97217504ea4a0a16b7079cf5f2a",
                    AddOnId = "836ffd7371b64eb48b3679fd02ce530f",
                    PositionId = "dddef2e0837e4461a3edf6f6fc4ca6a5",
                    SizeId = "8d643f1e68294355aa0ba06fa333642c",
                    Name = "Floral Lace",
                    Description = "Attach lace applique on right sleeve hem, medium size",
                    Price = 1100m,
                    ItemServiceType = (ItemServiceType)2,
                    CreatedBy = "System",
                    UpdatedBy = "System",
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    IsDeleted = false,
                },
                new AddOnOption
                {
                    Id = "073522fa350645d5b394e6021c6221d5",
                    AddOnId = "836ffd7371b64eb48b3679fd02ce530f",
                    PositionId = "dddef2e0837e4461a3edf6f6fc4ca6a5",
                    SizeId = "28f75887e2314369a96d17200e93e1df",
                    Name = "Floral Lace",
                    Description = "Attach lace applique on right sleeve hem, small size",
                    Price = 900m,
                    ItemServiceType = (ItemServiceType)2,
                    CreatedBy = "System",
                    UpdatedBy = "System",
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    IsDeleted = false,
                },
                new AddOnOption
                {
                    Id = "61fd99be41564ecba801e7e19e2a73b7",
                    AddOnId = "836ffd7371b64eb48b3679fd02ce530f",
                    PositionId = "dddef2e0837e4461a3edf6f6fc4ca6a5",
                    SizeId = "8d643f1e68294355aa0ba06fa333642c",
                    Name = "Geometric Lace",
                    Description = "Attach lace applique on right sleeve hem, medium size",
                    Price = 1200m,
                    ItemServiceType = (ItemServiceType)2,
                    CreatedBy = "System",
                    UpdatedBy = "System",
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    IsDeleted = false,
                },
                new AddOnOption
                {
                    Id = "1baf9f4c82c24300b2d145bb7e5d89e1",
                    AddOnId = "836ffd7371b64eb48b3679fd02ce530f",
                    PositionId = "dddef2e0837e4461a3edf6f6fc4ca6a5",
                    SizeId = "28f75887e2314369a96d17200e93e1df",
                    Name = "Vintage Lace",
                    Description = "Attach lace applique on right sleeve hem, small size",
                    Price = 1000m,
                    ItemServiceType = (ItemServiceType)2,
                    CreatedBy = "System",
                    UpdatedBy = "System",
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    IsDeleted = false,
                },
                new AddOnOption
                {
                    Id = "92275cd4b83b450eb5bfcbb03aa0c979",
                    AddOnId = "f9bacc3095014ca6913a596a7e705769",
                    PositionId = "d1a6e54ea47d4b89beaa90421e819a2f",
                    SizeId = "b3c067654ccf41529dab7a39efa73177",
                    Name = "Custom Print",
                    Description = "Print custom image on center chest, large size",
                    Price = 1500m,
                    ItemServiceType = (ItemServiceType)1,
                    CreatedBy = "System",
                    UpdatedBy = "System",
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    IsDeleted = false,
                },
                new AddOnOption
                {
                    Id = "8b2f1ba36a874862981dc1a7eabb5c70",
                    AddOnId = "c136acbd20b14414a8013a9b8b8ea5ab",
                    PositionId = "7ab17c28f8764667a8cdd92015d135a7",
                    SizeId = "28f75887e2314369a96d17200e93e1df",
                    Name = "Rhinestone Neckline",
                    Description = "Attach rhinestones on neckline, small size",
                    Price = 1500m,
                    ItemServiceType = (ItemServiceType)2,
                    CreatedBy = "System",
                    UpdatedBy = "System",
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    IsDeleted = false,
                },
                new AddOnOption
                {
                    Id = "f30a0e0b33bc40f6a69cbaf5e20f978c",
                    AddOnId = "b22e5a30c05147ddb90bba660269d844",
                    PositionId = "dda917d763ef470ba63a76a0c46b6dd3",
                    SizeId = "28f75887e2314369a96d17200e93e1df",
                    Name = "Beaded Sleeve",
                    Description = "Attach beads on left sleeve hem, small size",
                    Price = 1000m,
                    ItemServiceType = (ItemServiceType)2,
                    CreatedBy = "System",
                    UpdatedBy = "System",
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    IsDeleted = false,
                },
                new AddOnOption
                {
                    Id = "3ec44795a3eb480e987ec3f0761c13fb",
                    AddOnId = "a55a878403b54856bf6128a185a8eb4a",
                    PositionId = "dddef2e0837e4461a3edf6f6fc4ca6a5",
                    SizeId = "28f75887e2314369a96d17200e93e1df",
                    Name = "Floral Embroidery",
                    Description = "Embroider floral pattern on right sleeve hem, small size",
                    Price = 700m,
                    ItemServiceType = (ItemServiceType)2,
                    CreatedBy = "System",
                    UpdatedBy = "System",
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    IsDeleted = false,
                },
                new AddOnOption
                {
                    Id = "d29e831fa7e642d18be3c882405e4416",
                    AddOnId = "a55a878403b54856bf6128a185a8eb4a",
                    PositionId = "1892d3778cf44b8eb791801be53d1950",
                    SizeId = "28f75887e2314369a96d17200e93e1df",
                    Name = "Name Embroidery",
                    Description = "Embroider a name on left chest, small size",
                    Price = 500m,
                    ItemServiceType = (ItemServiceType)0,
                    CreatedBy = "System",
                    UpdatedBy = "System",
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    IsDeleted = false,
                },
                new AddOnOption
                {
                    Id = "26486ca9b5ce4a6da7b2925ee6011c6c",
                    AddOnId = "a55a878403b54856bf6128a185a8eb4a",
                    PositionId = "1892d3778cf44b8eb791801be53d1950",
                    SizeId = "b3c067654ccf41529dab7a39efa73177",
                    Name = "Name Embroidery",
                    Description = "Embroider a name on left chest, large size",
                    Price = 800m,
                    ItemServiceType = (ItemServiceType)0,
                    CreatedBy = "System",
                    UpdatedBy = "System",
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    IsDeleted = false,
                },
                new AddOnOption
                {
                    Id = "0dd67bccf110430da89bccfa378b8fb1",
                    AddOnId = "f9bacc3095014ca6913a596a7e705769",
                    PositionId = "d1a6e54ea47d4b89beaa90421e819a2f",
                    SizeId = "28f75887e2314369a96d17200e93e1df",
                    Name = "Custom Print",
                    Description = "Print custom image on center chest, small size",
                    Price = 1000m,
                    ItemServiceType = (ItemServiceType)1,
                    CreatedBy = "System",
                    UpdatedBy = "System",
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    IsDeleted = false,
                },
                new AddOnOption
                {
                    Id = "0528ef2e60164a3aa4ee078e25e982b2",
                    AddOnId = "c136acbd20b14414a8013a9b8b8ea5ab",
                    PositionId = "7ab17c28f8764667a8cdd92015d135a7",
                    SizeId = "d500f6f2377f475b86fcaefe85ca172b",
                    Name = "Rhinestone Neckline",
                    Description = "Attach rhinestones on full neckline, full position",
                    Price = 2500m,
                    ItemServiceType = (ItemServiceType)2,
                    CreatedBy = "System",
                    UpdatedBy = "System",
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    IsDeleted = false,
                },
                new AddOnOption
                {
                    Id = "262bed0f79814ddbbc46b0e3e23fbd17",
                    AddOnId = "1d9f860375714d9a9d6f9dcbdb8f9294",
                    PositionId = "3a0440ea7695472f9324919567fbd548",
                    SizeId = "28f75887e2314369a96d17200e93e1df",
                    Name = "Personal Tag",
                    Description = "Attach name tag at dress hem, small size",
                    Price = 300m,
                    ItemServiceType = (ItemServiceType)0,
                    CreatedBy = "System",
                    UpdatedBy = "System",
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    IsDeleted = false,
                },
                new AddOnOption
                {
                    Id = "a5f643a359804e199d02d775afd26577",
                    AddOnId = "a55a878403b54856bf6128a185a8eb4a",
                    PositionId = "dddef2e0837e4461a3edf6f6fc4ca6a5",
                    SizeId = "b3c067654ccf41529dab7a39efa73177",
                    Name = "Floral Embroidery",
                    Description = "Embroider floral pattern on right sleeve hem, large size",
                    Price = 950m,
                    ItemServiceType = (ItemServiceType)2,
                    CreatedBy = "System",
                    UpdatedBy = "System",
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    IsDeleted = false,
                },
                new AddOnOption
                {
                    Id = "811a8841037a43e0adf5b0bfd7630bad",
                    AddOnId = "a55a878403b54856bf6128a185a8eb4a",
                    PositionId = "1892d3778cf44b8eb791801be53d1950",
                    SizeId = "8d643f1e68294355aa0ba06fa333642c",
                    Name = "Name Embroidery",
                    Description = "Embroider a name on left chest, medium size",
                    Price = 650m,
                    ItemServiceType = (ItemServiceType)0,
                    CreatedBy = "System",
                    UpdatedBy = "System",
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    IsDeleted = false,
                },
                new AddOnOption
                {
                    Id = "3c7f15f87fc34c3f9448d52a2e98f3ed",
                    AddOnId = "f9bacc3095014ca6913a596a7e705769",
                    PositionId = "d1a6e54ea47d4b89beaa90421e819a2f",
                    SizeId = "8d643f1e68294355aa0ba06fa333642c",
                    Name = "Custom Print",
                    Description = "Print custom image on center chest, medium size",
                    Price = 1200m,
                    ItemServiceType = (ItemServiceType)1,
                    CreatedBy = "System",
                    UpdatedBy = "System",
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    IsDeleted = false,
                },
                new AddOnOption
                {
                    Id = "df081a39cfb246f8ae4be96cbb633893",
                    AddOnId = "b22e5a30c05147ddb90bba660269d844",
                    PositionId = "dda917d763ef470ba63a76a0c46b6dd3",
                    SizeId = "b3c067654ccf41529dab7a39efa73177",
                    Name = "Beaded Sleeve",
                    Description = "Attach beads on left sleeve hem, large size",
                    Price = 1300m,
                    ItemServiceType = (ItemServiceType)2,
                    CreatedBy = "System",
                    UpdatedBy = "System",
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    IsDeleted = false,
                },
                new AddOnOption
                {
                    Id = "71655b636da34aa391d8691180ca763b",
                    AddOnId = "1d9f860375714d9a9d6f9dcbdb8f9294",
                    PositionId = "3a0440ea7695472f9324919567fbd548",
                    SizeId = "8d643f1e68294355aa0ba06fa333642c",
                    Name = "Personal Tag",
                    Description = "Attach name tag at dress hem, medium size",
                    Price = 650m,
                    ItemServiceType = (ItemServiceType)0,
                    CreatedBy = "System",
                    UpdatedBy = "System",
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    IsDeleted = false,
                },
                new AddOnOption
                {
                    Id = "47d124b869de44d8b9b384711e58fc4b  ",
                    AddOnId = "a55a878403b54856bf6128a185a8eb4a",
                    PositionId = "6351c905181f4959973341881dcbc3ac",
                    SizeId = "8d643f1e68294355aa0ba06fa333642c",
                    Name = "Name Embroidery",
                    Description = "Embroider a name on right chest, medium size",
                    Price = 650m,
                    ItemServiceType = (ItemServiceType)0,
                    CreatedBy = "System",
                    UpdatedBy = "System",
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    IsDeleted = false,
                },
                new AddOnOption
                {
                    Id = "e59d8d7e163a49c8b1a6ea0c62fbe2a2",
                    AddOnId = "a55a878403b54856bf6128a185a8eb4a",
                    PositionId = "dda917d763ef470ba63a76a0c46b6dd3",
                    SizeId = "28f75887e2314369a96d17200e93e1df",
                    Name = "Floral Embroidery",
                    Description = "Embroider floral pattern on left sleeve hem, small size",
                    Price = 700m,
                    ItemServiceType = (ItemServiceType)2,
                    CreatedBy = "System",
                    UpdatedBy = "System",
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    IsDeleted = false,
                },
                new AddOnOption
                {
                    Id = "6e8cd43b09344f78ae9d3b2fdba915fa",
                    AddOnId = "a55a878403b54856bf6128a185a8eb4a",
                    PositionId = "6351c905181f4959973341881dcbc3ac",
                    SizeId = "8d643f1e68294355aa0ba06fa333642c",
                    Name = "Logo Embroidery",
                    Description = "Embroider a logo on right chest, medium size",
                    Price = 650m,
                    ItemServiceType = (ItemServiceType)0,
                    CreatedBy = "System",
                    UpdatedBy = "System",
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    IsDeleted = false,
                },
                new AddOnOption
                {
                    Id = "b52cf28289f64637adf10eeb3e1046c7",
                    AddOnId = "a55a878403b54856bf6128a185a8eb4a",
                    PositionId = "d1a6e54ea47d4b89beaa90421e819a2f",
                    SizeId = "28f75887e2314369a96d17200e93e1df",
                    Name = "Logo Embroidery",
                    Description = "Embroider a logo on center chest, small size",
                    Price = 700m,
                    ItemServiceType = (ItemServiceType)2,
                    CreatedBy = "System",
                    UpdatedBy = "System",
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    IsDeleted = false,
                },
                new AddOnOption
                {
                    Id = "dda917d763ef470ba63a76a0c46b6dd3",
                    AddOnId = "a55a878403b54856bf6128a185a8eb4a",
                    PositionId = "1892d3778cf44b8eb791801be53d1950",
                    SizeId = "8d643f1e68294355aa0ba06fa333642c",
                    Name = "Name Embroidery",
                    Description = "Embroider a name on left chest, medium size",
                    Price = 650m,
                    ItemServiceType = (ItemServiceType)1,
                    CreatedBy = "System",
                    UpdatedBy = "System",
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    IsDeleted = false,
                },
                new AddOnOption
                {
                    Id = "1892d3778cf44b8eb791801be53d1950",
                    AddOnId = "a55a878403b54856bf6128a185a8eb4a",
                    PositionId = "1892d3778cf44b8eb791801be53d1950",
                    SizeId = "28f75887e2314369a96d17200e93e1df",
                    Name = "Name Embroidery",
                    Description = "Embroider a name on left chest, small size",
                    Price = 500m,
                    ItemServiceType = (ItemServiceType)1,
                    CreatedBy = "System",
                    UpdatedBy = "System",
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    IsDeleted = false,
                },
                new AddOnOption
                {
                    Id = "dddef2e0837e4461a3edf6f6fc4ca6a5",
                    AddOnId = "a55a878403b54856bf6128a185a8eb4a",
                    PositionId = "1892d3778cf44b8eb791801be53d1950",
                    SizeId = "b3c067654ccf41529dab7a39efa73177",
                    Name = "Name Embroidery",
                    Description = "Embroider a name on left chest, large size",
                    Price = 800m,
                    ItemServiceType = (ItemServiceType)1,
                    CreatedBy = "System",
                    UpdatedBy = "System",
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    IsDeleted = false,
                },
                new AddOnOption
                {
                    Id = "6351c905181f4959973341881dcbc3ac",
                    AddOnId = "a55a878403b54856bf6128a185a8eb4a",
                    PositionId = "6351c905181f4959973341881dcbc3ac",
                    SizeId = "8d643f1e68294355aa0ba06fa333642c",
                    Name = "Name Embroidery",
                    Description = "Embroider a name on right chest, medium size",
                    Price = 650m,
                    ItemServiceType = (ItemServiceType)1,
                    CreatedBy = "System",
                    UpdatedBy = "System",
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    IsDeleted = false,
                }
            );

            #endregion

            #region Seed Diary, Address, Voucher, Branch

            modelBuilder.Entity<MeasurementDiary>().HasData(
                new MeasurementDiary
                {
                    Id = "071b2ce694d84452a7899c9a97a863ec",
                    UserId = "f49aa51bbd304e77933e24bbed65b165",
                    Name = "Nguyễn Thị Lan",
                    Age = 27,
                    Height = 155.0f,
                    Weight = 50.0f,
                    Bust = 86.0f,
                    Waist = 74.0f,
                    Hip = 90.0f,
                    FirstDateOfLastPeriod = new DateTime(2025, 4, 26, 17, 0, 0, DateTimeKind.Utc),
                    AverageMenstrualCycle = 0,
                    NumberOfPregnancy = 1,
                    UltrasoundDate = null,
                    WeeksFromUltrasound = 0,
                    PregnancyStartDate = new DateTime(2025, 4, 12, 17, 0, 0, DateTimeKind.Utc),
                    IsActive = false,
                    CreatedBy = "User",
                    UpdatedBy = null,
                    CreatedAt = new DateTime(2025, 8, 3, 6, 23, 44, DateTimeKind.Utc),
                    UpdatedAt = new DateTime(2025, 8, 3, 6, 23, 44, DateTimeKind.Utc),
                    IsDeleted = false
                },
                new MeasurementDiary
                {
                    Id = "296b60c784c64ccfb8e7eb5354b97c52",
                    UserId = "f49aa51bbd304e77933e24bbed65b165",
                    Name = "Vũ Thị Thu Hương",
                    Age = 26,
                    Height = 154.0f,
                    Weight = 48.0f,
                    Bust = 82.0f,
                    Waist = 66.0f,
                    Hip = 87.0f,
                    FirstDateOfLastPeriod = new DateTime(2025, 6, 29, 17, 0, 0, DateTimeKind.Utc),
                    AverageMenstrualCycle = 28,
                    NumberOfPregnancy = 2,
                    UltrasoundDate = new DateTime(2025, 8, 1, 17, 0, 0, DateTimeKind.Utc),
                    WeeksFromUltrasound = 5,
                    PregnancyStartDate = new DateTime(2025, 6, 27, 17, 0, 0, DateTimeKind.Utc),
                    IsActive = true,
                    CreatedBy = "User",
                    UpdatedBy = null,
                    CreatedAt = new DateTime(2025, 8, 3, 6, 26, 33, DateTimeKind.Utc),
                    UpdatedAt = new DateTime(2025, 8, 3, 6, 26, 33, DateTimeKind.Utc),
                    IsDeleted = false
                }
            );

            modelBuilder.Entity<VoucherBatch>().HasData(
                new VoucherBatch
                {
                    Id = "0d150b442aaa45c992bd4aeb9160cdc8",
                    BatchName = "Khuyến mãi hè",
                    BatchCode = "KMHE2025",
                    Description = "Ưu đãi giảm giá cho sản phẩm mùa hè",
                    StartDate = new DateTime(2025, 07, 23, 12, 13, 29, DateTimeKind.Utc),
                    EndDate = new DateTime(2025, 08, 23, 12, 13, 29, DateTimeKind.Utc),
                    TotalQuantity = 1000,
                    RemainingQuantity = 999,
                    DiscountType = DiscountType.PERCENTAGE,
                    DiscountValue = 15,
                    MinimumOrderValue = 100000.0m,
                    MaximumDiscountValue = 50000.0m,
                    CreatedBy = "System",
                    UpdatedBy = null,
                    CreatedAt = new DateTime(2025, 07, 23, 12, 15, 20, DateTimeKind.Utc),
                    UpdatedAt = new DateTime(2025, 07, 23, 12, 15, 20, DateTimeKind.Utc),
                    IsDeleted = false,
                },
                new VoucherBatch
                {
                    Id = "51cbab1b74894a128a9a62db5e30d69f",
                    BatchName = "Mua sắm mùa tựu trường",
                    BatchCode = "TUTRUONG2025",
                    Description = "Giảm giá đặc biệt cho đồ dùng học sinh",
                    StartDate = new DateTime(2025, 07, 25, 07, 00, 00, DateTimeKind.Utc),
                    EndDate = new DateTime(2025, 08, 10, 07, 00, 00, DateTimeKind.Utc),
                    TotalQuantity = 500,
                    RemainingQuantity = 499,
                    DiscountType = DiscountType.FIXED,
                    DiscountValue = 10000,
                    MinimumOrderValue = 50000.0m,
                    MaximumDiscountValue = 10000.0m,
                    CreatedBy = "System",
                    UpdatedBy = null,
                    CreatedAt = new DateTime(2025, 07, 23, 12, 20, 20, DateTimeKind.Utc),
                    UpdatedAt = new DateTime(2025, 07, 23, 12, 20, 20, DateTimeKind.Utc),
                    IsDeleted = false,
                },
                new VoucherBatch
                {
                    Id = "47eefe5ed2bc4aac8cf93bd7cc94ea85",
                    BatchName = "Flash Sale 24 giờ",
                    BatchCode = "FLASH24",
                    Description = "Ưu đãi giới hạn trong 1 ngày",
                    StartDate = new DateTime(2025, 07, 23, 07, 00, 00, DateTimeKind.Utc),
                    EndDate = new DateTime(2025, 07, 24, 07, 00, 00, DateTimeKind.Utc),
                    TotalQuantity = 200,
                    RemainingQuantity = 199,
                    DiscountType = DiscountType.PERCENTAGE,
                    DiscountValue = 20,
                    MinimumOrderValue = 30000.0m,
                    MaximumDiscountValue = 25000.0m,
                    CreatedBy = "System",
                    UpdatedBy = null,
                    CreatedAt = new DateTime(2025, 07, 23, 12, 20, 32, DateTimeKind.Utc),
                    UpdatedAt = new DateTime(2025, 07, 23, 12, 20, 32, DateTimeKind.Utc),
                    IsDeleted = false,
                },
                new VoucherBatch
                {
                    Id = "99ba88fd0f9740e99bdcb7a94c5c7ff8",
                    BatchName = "Xả kho cuối mùa",
                    BatchCode = "XAKHO2025",
                    Description = "Giảm giá thanh lý hàng tồn kho",
                    StartDate = new DateTime(2025, 07, 26, 07, 00, 00, DateTimeKind.Utc),
                    EndDate = new DateTime(2025, 08, 26, 07, 00, 00, DateTimeKind.Utc),
                    TotalQuantity = 750,
                    RemainingQuantity = 749,
                    DiscountType = DiscountType.FIXED,
                    DiscountValue = 5000,
                    MinimumOrderValue = 0.0m,
                    MaximumDiscountValue = 5000.0m,
                    CreatedBy = "System",
                    UpdatedBy = null,
                    CreatedAt = new DateTime(2025, 07, 23, 12, 20, 40, DateTimeKind.Utc),
                    UpdatedAt = new DateTime(2025, 07, 23, 12, 20, 40, DateTimeKind.Utc),
                    IsDeleted = false,
                },
                new VoucherBatch
                {
                    Id = "8e54075eb5bc40f18d0b6b55584feda3",
                    BatchName = "Ưu đãi khách hàng thân thiết",
                    BatchCode = "THANTHIET2025",
                    Description = "Dành riêng cho hội viên thân thiết",
                    StartDate = new DateTime(2025, 07, 27, 07, 00, 00, DateTimeKind.Utc),
                    EndDate = new DateTime(2025, 08, 15, 07, 00, 00, DateTimeKind.Utc),
                    TotalQuantity = 300,
                    RemainingQuantity = 299,
                    DiscountType = DiscountType.PERCENTAGE,
                    DiscountValue = 25,
                    MinimumOrderValue = 150000.0m,
                    MaximumDiscountValue = 100000.0m,
                    CreatedBy = "System",
                    UpdatedBy = null,
                    CreatedAt = new DateTime(2025, 07, 23, 12, 20, 46, DateTimeKind.Utc),
                    UpdatedAt = new DateTime(2025, 07, 23, 12, 20, 46, DateTimeKind.Utc),
                    IsDeleted = false,
                }
            );

            modelBuilder.Entity<VoucherDiscount>().HasData(
                new VoucherDiscount
                {
                    Id = "a445f49f53904919ba8ab9b0bcdf9f9d",
                    UserId = "f49aa51bbd304e77933e24bbed65b165",
                    VoucherBatchId = "0d150b442aaa45c992bd4aeb9160cdc8",
                    Code = "KMHE2025-001",
                    Status = VoucherStatus.ACTIVE,
                    CreatedBy = "System",
                    UpdatedBy = null,
                    CreatedAt = new DateTime(2025, 07, 23, 12, 25, 18, DateTimeKind.Utc),
                    UpdatedAt = new DateTime(2025, 07, 23, 12, 25, 18, DateTimeKind.Utc),
                    IsDeleted = false,
                },
                new VoucherDiscount
                {
                    Id = "a9c92f441b0847d3bebcef74570b2ae6",
                    UserId = "f49aa51bbd304e77933e24bbed65b165",
                    VoucherBatchId = "51cbab1b74894a128a9a62db5e30d69f",
                    Code = "TUTRUONG2025-001",
                    Status = VoucherStatus.ACTIVE,
                    CreatedBy = "System",
                    UpdatedBy = null,
                    CreatedAt = new DateTime(2025, 07, 23, 12, 25, 26, DateTimeKind.Utc),
                    UpdatedAt = new DateTime(2025, 07, 23, 12, 25, 26, DateTimeKind.Utc),
                    IsDeleted = false,
                },
                new VoucherDiscount
                {
                    Id = "0bbfcbd438134f288841b5f4e865a9ad",
                    UserId = "f49aa51bbd304e77933e24bbed65b165",
                    VoucherBatchId = "47eefe5ed2bc4aac8cf93bd7cc94ea85",
                    Code = "FLASH24-001",
                    Status = VoucherStatus.ACTIVE,
                    CreatedBy = "System",
                    UpdatedBy = null,
                    CreatedAt = new DateTime(2025, 07, 23, 12, 25, 32, DateTimeKind.Utc),
                    UpdatedAt = new DateTime(2025, 07, 23, 12, 25, 32, DateTimeKind.Utc),
                    IsDeleted = false,
                },
                new VoucherDiscount
                {
                    Id = "0a707a6002ee409ca7d12028f7b7ccdf",
                    UserId = "f49aa51bbd304e77933e24bbed65b165",
                    VoucherBatchId = "99ba88fd0f9740e99bdcb7a94c5c7ff8",
                    Code = "XAKHO2025-001",
                    Status = VoucherStatus.ACTIVE,
                    CreatedBy = "System",
                    UpdatedBy = null,
                    CreatedAt = new DateTime(2025, 07, 23, 12, 25, 38, DateTimeKind.Utc),
                    UpdatedAt = new DateTime(2025, 07, 23, 12, 25, 38, DateTimeKind.Utc),
                    IsDeleted = false,
                },
                new VoucherDiscount
                {
                    Id = "dd3104e0d7b7451c946b93bb281441a2",
                    UserId = "f49aa51bbd304e77933e24bbed65b165",
                    VoucherBatchId = "8e54075eb5bc40f18d0b6b55584feda3",
                    Code = "THANTHIET2025-001",
                    Status = VoucherStatus.ACTIVE,
                    CreatedBy = "System",
                    UpdatedBy = null,
                    CreatedAt = new DateTime(2025, 07, 23, 12, 25, 46, DateTimeKind.Utc),
                    UpdatedAt = new DateTime(2025, 07, 23, 12, 25, 46, DateTimeKind.Utc),
                    IsDeleted = false,
                }
            );

            modelBuilder.Entity<Address>().HasData(
                new Address
                {
                    Id = "7aa93cf1dcea43e68da114d7c991a732",
                    UserId = "f49aa51bbd304e77933e24bbed65b165",
                    MapId =
                        "rGHobqy_rVN8V-EjqVDYaXe91lWjE3_1nq9TL69Sd0xO2tkirG-8T3iyZS-thm2aSb1LMaIydjs5_21tYrn-G5UnaMBg",
                    Province = "TP. Hồ Chí Minh",
                    District = "Quận 2",
                    Ward = "Phường Thảo Điền",
                    Street = "12 Đường Thảo Điền",
                    Latitude = 10.875121f,
                    Longitude = 106.80071f,
                    IsDefault = true,
                    CreatedBy = "System",
                    UpdatedBy = null,
                    CreatedAt = new DateTime(2025, 07, 23, 12, 07, 25, DateTimeKind.Utc),
                    UpdatedAt = new DateTime(2025, 07, 23, 12, 07, 25, DateTimeKind.Utc),
                    IsDeleted = false,
                },
                new Address
                {
                    Id = "f71162915b5846f8a470f05adc30e635",
                    UserId = "f49aa51bbd304e77933e24bbed65b165",
                    MapId =
                        "qGHuRFi-pARDYqQuX6KC62zUoh2iWOHabK5PNadiqF9zovo3o1m8W3WNdRC4T7SQdrNEVpNyd8sro3W8k2N78D2MZTKScpLbdrJEUJWakl93iFcEkvuCknWgXy-USBenS",
                    Province = "Hà Nội",
                    District = "Hai Bà Trưng",
                    Ward = "Lê Đại Hành",
                    Street = "Vincom Center Bà Triệu, 191 P. Bà Triệu",
                    Latitude = 21.010862f,
                    Longitude = 105.84956f,
                    IsDefault = false,
                    CreatedBy = "System",
                    UpdatedBy = null,
                    CreatedAt = new DateTime(2025, 07, 23, 12, 09, 23, DateTimeKind.Utc),
                    UpdatedAt = new DateTime(2025, 07, 23, 12, 09, 23, DateTimeKind.Utc),
                    IsDeleted = false,
                },
                new Address
                {
                    Id = "9b5d70c62d8940c2b933d55db1a64893",
                    UserId = "f49aa51bbd304e77933e24bbed65b165",
                    MapId =
                        "TErhHQFjiV1t_21NqqlGz9XmFIDydhW3HfKxQLKyd7k90rOEYrFe7M0quV16ubp6ZSr6vYkgyjc2ufK9UrXyF5tjZMxs",
                    Province = "Đồng Nai",
                    District = "Biên Hòa",
                    Ward = "Tân Mai",
                    Street = "UNIQLO Vincom Biên Hoà, 1096 Phạm Văn Thuận",
                    Latitude = 10.95753f,
                    Longitude = 106.84307f,
                    IsDefault = false,
                    CreatedBy = "System",
                    UpdatedBy = null,
                    CreatedAt = new DateTime(2025, 07, 23, 12, 10, 46, DateTimeKind.Utc),
                    UpdatedAt = new DateTime(2025, 07, 23, 12, 10, 46, DateTimeKind.Utc),
                    IsDeleted = false,
                }
            );

            modelBuilder.Entity<Measurement>().HasData(
                new Measurement
                {
                    Id = "82e18e44f0984ad08209e575f77f3ca0",
                    MeasurementDiaryId = "071b2ce694d84452a7899c9a97a863ec",
                    WeekOfPregnancy = 16,
                    Weight = 50.0f,
                    Neck = 31.0f,
                    Coat = 95.0f,
                    Bust = 86.0f,
                    ChestAround = 93.0f,
                    Stomach = 85.0f,
                    PantsWaist = 75.0f,
                    Thigh = 49.5f,
                    DressLength = 102.3f,
                    SleeveLength = 24.2f,
                    ShoulderWidth = 36.0f,
                    Waist = 74.0f,
                    LegLength = 74.4f,
                    Hip = 90.0f,
                    CreatedBy = "User",
                    UpdatedBy = null,
                    CreatedAt = new DateTime(2025, 8, 3, 6, 23, 44, DateTimeKind.Utc),
                    UpdatedAt = new DateTime(2025, 8, 3, 6, 23, 44, DateTimeKind.Utc),
                    IsDeleted = false
                },
                new Measurement
                {
                    Id = "c49743c2e5d746239bddd1ade3d76ae9",
                    MeasurementDiaryId = "296b60c784c64ccfb8e7eb5354b97c52",
                    WeekOfPregnancy = 5,
                    Weight = 48.0f,
                    Neck = 30.8f,
                    Coat = 87.0f,
                    Bust = 82.0f,
                    ChestAround = 85.0f,
                    Stomach = 71.0f,
                    PantsWaist = 61.0f,
                    Thigh = 46.0f,
                    DressLength = 101.6f,
                    SleeveLength = 24.1f,
                    ShoulderWidth = 35.8f,
                    Waist = 66.0f,
                    LegLength = 73.9f,
                    Hip = 87.0f,
                    CreatedBy = "User",
                    UpdatedBy = null,
                    CreatedAt = new DateTime(2025, 8, 3, 6, 26, 33, DateTimeKind.Utc),
                    UpdatedAt = new DateTime(2025, 8, 3, 6, 26, 33, DateTimeKind.Utc),
                    IsDeleted = false
                });

            modelBuilder.Entity<Branch>().HasData(
                new Branch
                {
                    Id = "f35d622bcccc448bb3e4d61205fe9771",
                    BranchManagerId = "29d72211a9f7480c9812d61ee17c92b9",
                    Name = "Chi Nhánh Quận 1",
                    Description = "Trụ sở chính, cung cấp đầy đủ các dịch vụ và hỗ trợ khách hàng 24/7.",
                    OpeningHour = new TimeOnly(09, 00, 00),
                    ClosingHour = new TimeOnly(18, 00, 00),
                    Images =
                    [
                        "https://firebasestorage.googleapis.com/v0/b/mamafit-e0138.firebasestorage.app/o/images%2Fdownload_1753254774304_m5fg15.jpeg?alt=media&token=02b0e1ee-3126-4c80-ad66-59aea3fdfd70"
                    ],
                    MapId =
                        "XTX4i62tylprNlWCpW2uynJsQW6yIY_EZQ8iropFoomEN3esvWO2cmkXQb-nfP_E-wpZk71HuuxrSHyunEWMQW0Vj6y6G4BjaE5alox8jOVpGkm-uhqcjGsxQaC0MG_fM",
                    Province = "TP. Hồ Chí Minh",
                    District = "Quận 1",
                    Ward = "Phường Bến Nghé",
                    Street = "123 Đường Lê Lợi",
                    Latitude = 10.772752f,
                    Longitude = 106.69986f,
                    CreatedBy = "System",
                    UpdatedBy = "System",
                    CreatedAt = new DateTime(2025, 07, 23, 14, 12, 59, DateTimeKind.Utc),
                    UpdatedAt = new DateTime(2025, 07, 23, 14, 12, 59, DateTimeKind.Utc),
                    IsDeleted = false,
                },
                new Branch
                {
                    Id = "9523754dfe4e4e9283011b328192272d",
                    BranchManagerId = "aw9aa51bbd304e77933e24bbed65cd19",
                    Name = "Chi Nhánh Quận 9",
                    Description =
                        "Chi nhánh phát triển khu vực phía Đông thành phố, thuận tiện cho các khu công nghệ cao.",
                    OpeningHour = new TimeOnly(08, 30, 00),
                    ClosingHour = new TimeOnly(17, 30, 00),
                    Images =
                    [
                        "https://firebasestorage.googleapis.com/v0/b/mamafit-e0138.firebasestorage.app/o/images%2Fdownload_1753254877456_iubawj.jpeg?alt=media&token=e343f866-8c6b-4fa3-b5fa-17026bed0922"
                    ],
                    MapId =
                        "abpzyvOobyVhaEHDtbmbClhBTYq_RZQIWLtF96RrhC1YiSf7l0SyS2mqTeG5eaoO4bbpnj71Brih0u0HouGumgG_OL0-5RIRKbKhe4Yh4iB6Pkk2Ivh6-NZi6RchtH4gx",
                    Province = "TP. Hồ Chí Minh",
                    District = "Quận 9",
                    Ward = "Phường Hiệp Phú",
                    Street = "50 Đường Lê Văn Việt",
                    Latitude = 10.845493f,
                    Longitude = 106.77894f,
                    CreatedBy = "System",
                    UpdatedBy = "System",
                    CreatedAt = new DateTime(2025, 07, 23, 14, 14, 51, DateTimeKind.Utc),
                    UpdatedAt = new DateTime(2025, 07, 23, 14, 14, 51, DateTimeKind.Utc),
                    IsDeleted = false,
                },
                new Branch
                {
                    Id = "1ca91da6e6874aac9f8a64a90a5779b2",
                    BranchManagerId = "po103fbe31e6449b9b92c89b5c23oa1x2",
                    Name = "Chi Nhánh Quận 7",
                    Description =
                        "Chi nhánh phục vụ khu vực Nam Sài Gòn, tập trung các khu đô thị mới, dân cư đông đúc.",
                    OpeningHour = new TimeOnly(09, 00, 00),
                    ClosingHour = new TimeOnly(13, 00, 00),
                    Images =
                    [
                        "https://firebasestorage.googleapis.com/v0/b/mamafit-e0138.firebasestorage.app/o/images%2Fdownload_1753254950766_es1xrv.jpeg?alt=media&token=c93a8c76-3112-44af-8c57-9445a41ec554"
                    ],
                    MapId =
                        "vRJ97gJfPeU95hjUgnwit3WGBWj-sb4PsqSY1vqF-sXWYIzEkrFXof6KGMRy11LlsYq9COapWetaMvjiztQlJx1IST76ef7qfUr9JXZ9vn_Z6hVoJqQmPn3itUiKfCOTf",
                    Province = "TP. Hồ Chí Minh",
                    District = "Quận 7",
                    Ward = "Phường Tân Phú",
                    Street = "25 Đường Nguyễn Lương Bằng",
                    Latitude = 10.715182f,
                    Longitude = 106.729774f,
                    CreatedBy = "System",
                    UpdatedBy = "System",
                    CreatedAt = new DateTime(2025, 07, 23, 14, 15, 55, DateTimeKind.Utc),
                    UpdatedAt = new DateTime(2025, 07, 23, 14, 15, 55, DateTimeKind.Utc),
                    IsDeleted = false,
                },
                new Branch
                {
                    Id = "896d6274c5a243758397d245dee2bbc9",
                    BranchManagerId = "ms1948fbai10380snalep19041nxbap1009",
                    Name = "Chi Nhánh Hà Nội",
                    Description = "Chi nhánh phục vụ khu vực phía Bắc, hỗ trợ khách hàng Hà Nội và vùng lân cận.",
                    OpeningHour = new TimeOnly(08, 30, 00),
                    ClosingHour = new TimeOnly(17, 30, 00),
                    Images =
                    [
                        "https://firebasestorage.googleapis.com/v0/b/mamafit-e0138.firebasestorage.app/o/images%2Fdownload_1753255046676_1dvpll.jpeg?alt=media&token=9ef2c48b-49dd-4206-b397-add539af29ba"
                    ],
                    MapId =
                        "Rka5w6k6WWv0zU42vEZ8RY-7anShuKUFf2lsQLqTpoppknNOuXqx22mpLRK4eIiPXKiTJLlCXttq7zptNu2qT8FzPJQ0",
                    Province = "Hà Nội",
                    District = "Quận Ba Đình",
                    Ward = "Phường Trúc Bạch",
                    Street = "456 Đường Hoàng Hoa Thám",
                    Latitude = 21.04182f,
                    Longitude = 105.83528f,
                    CreatedBy = "System",
                    UpdatedBy = "System",
                    CreatedAt = new DateTime(2025, 07, 23, 14, 17, 54, DateTimeKind.Utc),
                    UpdatedAt = new DateTime(2025, 07, 23, 14, 17, 54, DateTimeKind.Utc),
                    IsDeleted = false,
                },
                new Branch
                {
                    Id = "b536f146c8f04bbc9949fc70a31fda8f",
                    BranchManagerId = "nnas1039jxoaopepmxsaoqjxba188290nci",
                    Name = "Chi Nhánh Bình Dương ",
                    Description = "Cơ sở phục vụ các khu công nghiệp và khu vực lân cận.",
                    OpeningHour = new TimeOnly(08, 30, 00),
                    ClosingHour = new TimeOnly(17, 30, 00),
                    Images =
                    [
                        "https://firebasestorage.googleapis.com/v0/b/mamafit-e0138.firebasestorage.app/o/images%2Fdownload_1753255136226_9b8nry.jpeg?alt=media&token=ec3eea0a-2200-4dd1-80f2-4b2bb4944350"
                    ],
                    MapId =
                        "IKRTfzB9UZqa3EN9s3u43GSUVzekeJnSc7s0GZxTtJ9kuGEhq515mWSUeiCtS5bMdSoOsBIZRmvx8q1cmtg6KnmewnwwjDbTpfrpMhVhqmvN_gF8MkgyKrH2oVyeaDeHa",
                    Province = "Bình Dương",
                    District = "Thị xã Thuận An",
                    Ward = "Phường Lái Thiêu",
                    Street = "90 Đường Quốc lộ 13",
                    Latitude = 10.892301f,
                    Longitude = 106.70494f,
                    CreatedBy = "System",
                    UpdatedBy = "System",
                    CreatedAt = new DateTime(2025, 07, 23, 14, 19, 01, DateTimeKind.Utc),
                    UpdatedAt = new DateTime(2025, 07, 23, 14, 19, 01, DateTimeKind.Utc),
                    IsDeleted = false,
                }
            );

            #endregion

            #region Seed Milestone, Task

            modelBuilder.Entity<Milestone>().HasData(
                new Milestone
                {
                    Id = "08c1f3fd1f71460e9a4afefc14fa01cb",
                    Name = "In Warranty",
                    Description = "Warranty repair or replacement",
                    ApplyFor = [ItemType.WARRANTY],
                    SequenceOrder = 4,
                    CreatedBy = "Admin",
                    UpdatedBy = "Admin",
                    CreatedAt = new DateTime(2025, 7, 23, 14, 17, 54, DateTimeKind.Utc),
                    UpdatedAt = new DateTime(2025, 7, 23, 14, 17, 54, DateTimeKind.Utc),
                    IsDeleted = false
                },
                new Milestone
                {
                    Id = "130a30e16f764ec7b29c1461f932d10a",
                    Name = "Preset Production",
                    Description = "Production process for preset orders",
                    ApplyFor = [ItemType.PRESET],
                    SequenceOrder = 1,
                    CreatedBy = "Admin",
                    UpdatedBy = "Admin",
                    CreatedAt = new DateTime(2025, 7, 23, 14, 17, 54, DateTimeKind.Utc),
                    UpdatedAt = new DateTime(2025, 7, 23, 14, 17, 54, DateTimeKind.Utc),
                    IsDeleted = false
                },
                new Milestone
                {
                    Id = "3bc69c7f00d44442bc3dd095637b4172",
                    Name = "Quality Check",
                    Description = "Quality control checks for all order types",
                    ApplyFor = [ItemType.PRESET, ItemType.READY_TO_BUY],
                    SequenceOrder = 5,
                    CreatedBy = "Admin",
                    UpdatedBy = "Admin",
                    CreatedAt = new DateTime(2025, 7, 23, 14, 17, 54, DateTimeKind.Utc),
                    UpdatedAt = new DateTime(2025, 7, 23, 14, 17, 54, DateTimeKind.Utc),
                    IsDeleted = false
                },
                new Milestone
                {
                    Id = "d677453018e04408ba1d0ddec81e5ffd",
                    Name = "Design",
                    Description = "Design process for custom design requests",
                    ApplyFor = [ItemType.DESIGN_REQUEST],
                    SequenceOrder = 1,
                    CreatedBy = "Admin",
                    UpdatedBy = "Admin",
                    CreatedAt = new DateTime(2025, 7, 23, 14, 17, 54, DateTimeKind.Utc),
                    UpdatedAt = new DateTime(2025, 7, 23, 14, 17, 54, DateTimeKind.Utc),
                    IsDeleted = false
                },
                new Milestone
                {
                    Id = "e1235c985155490d9f701a5e35d490ed",
                    Name = "Add On",
                    Description = "Additional customization or services",
                    ApplyFor = [ItemType.ADD_ON],
                    SequenceOrder = 2,
                    CreatedBy = "Admin",
                    UpdatedBy = "Admin",
                    CreatedAt = new DateTime(2025, 7, 23, 14, 17, 54, DateTimeKind.Utc),
                    UpdatedAt = new DateTime(2025, 7, 23, 14, 17, 54, DateTimeKind.Utc),
                    IsDeleted = false
                },
                new Milestone
                {
                    Id = "e1d5cb20d3e5460b9067113cf4615fa7",
                    Name = "Quality Check Warranty",
                    Description = "Quality control checks for warranty preset",
                    ApplyFor = [ItemType.WARRANTY],
                    SequenceOrder = 5,
                    CreatedBy = "Admin",
                    UpdatedBy = "Admin",
                    CreatedAt = new DateTime(2025, 7, 23, 14, 17, 54, DateTimeKind.Utc),
                    UpdatedAt = new DateTime(2025, 7, 23, 14, 17, 54, DateTimeKind.Utc),
                    IsDeleted = false
                },
                new Milestone
                {
                    Id = "afa0cfb3844a4bf99e79b40a009b8f58",
                    Name = "Packing",
                    Description = "Final packing for customer",
                    ApplyFor = [ItemType.PRESET, ItemType.READY_TO_BUY, ItemType.WARRANTY],
                    SequenceOrder = 7,
                    CreatedBy = "Admin",
                    UpdatedBy = "Admin",
                    CreatedAt = new DateTime(2025, 7, 23, 14, 17, 54, DateTimeKind.Utc),
                    UpdatedAt = new DateTime(2025, 7, 23, 14, 17, 54, DateTimeKind.Utc),
                    IsDeleted = false
                },
                new Milestone
                {
                    Id = "a82b6d4f0c194fa5b92348e7d86c4a10",
                    Name = "Quality Check Failed",
                    Description = "Did not meet quality standards, needs rework.",
                    ApplyFor = [ItemType.QC_FAIL],
                    SequenceOrder = 6,
                    CreatedBy = "Admin",
                    UpdatedBy = "Admin",
                    CreatedAt = new DateTime(2025, 7, 23, 14, 17, 54, DateTimeKind.Utc),
                    UpdatedAt = new DateTime(2025, 7, 23, 14, 17, 54, DateTimeKind.Utc),
                    IsDeleted = false
                });

            modelBuilder.Entity<MaternityDressTask>().HasData(
                new MaternityDressTask
                {
                    Id = "13a6b12c7ae643809c13ee2863e86b10",
                    MilestoneId = "afa0cfb3844a4bf99e79b40a009b8f58",
                    Name = "Prepare Shipping",
                    Description = "Create shipping label and arrange delivery",
                    SequenceOrder = 2,
                    EstimateTimeSpan = 30,
                    CreatedBy = "System",
                    UpdatedBy = "System",
                    CreatedAt = new DateTime(2025, 7, 23, 14, 17, 54, DateTimeKind.Utc),
                    UpdatedAt = new DateTime(2025, 7, 23, 14, 17, 54, DateTimeKind.Utc),
                    IsDeleted = false
                },
                new MaternityDressTask
                {
                    Id = "23de35d6fb194c398d09ce00ae01eeea",
                    MilestoneId = "130a30e16f764ec7b29c1461f932d10a",
                    Name = "Attach Details",
                    Description = "Add buttons, zippers, and finishing touches",
                    SequenceOrder = 5,
                    EstimateTimeSpan = 30,
                    CreatedBy = "Admin",
                    UpdatedBy = "Admin",
                    CreatedAt = new DateTime(2025, 7, 23, 14, 17, 54, DateTimeKind.Utc),
                    UpdatedAt = new DateTime(2025, 7, 23, 14, 17, 54, DateTimeKind.Utc),
                    IsDeleted = false
                },
                new MaternityDressTask
                {
                    Id = "26049f6a1a964dc5b659e4991eb475c5",
                    MilestoneId = "d677453018e04408ba1d0ddec81e5ffd",
                    Name = "Create preset with design request",
                    Description = "Create preset for customer",
                    SequenceOrder = 1,
                    EstimateTimeSpan = 30,
                    CreatedBy = "System",
                    UpdatedBy = "System",
                    CreatedAt = new DateTime(2025, 7, 23, 14, 17, 54, DateTimeKind.Utc),
                    UpdatedAt = new DateTime(2025, 7, 23, 14, 17, 54, DateTimeKind.Utc),
                    IsDeleted = false
                },
                new MaternityDressTask
                {
                    Id = "26ab09cb782a4e8db409f39079ac24f1",
                    MilestoneId = "130a30e16f764ec7b29c1461f932d10a",
                    Name = "Prepare Materials",
                    Description = "Gather required fabrics and materials for preset design",
                    SequenceOrder = 2,
                    EstimateTimeSpan = 30,
                    CreatedBy = "Admin",
                    UpdatedBy = "Admin",
                    CreatedAt = new DateTime(2025, 7, 23, 14, 17, 54, DateTimeKind.Utc),
                    UpdatedAt = new DateTime(2025, 7, 23, 14, 17, 54, DateTimeKind.Utc),
                    IsDeleted = false
                },
                new MaternityDressTask
                {
                    Id = "32e620585a454e7bbe7bf9314e7577ce",
                    MilestoneId = "e1235c985155490d9f701a5e35d490ed",
                    Name = "Apply Customizations",
                    Description = "Execute additional features and embroidery",
                    SequenceOrder = 1,
                    EstimateTimeSpan = 30,
                    CreatedBy = "System",
                    UpdatedBy = "System",
                    CreatedAt = new DateTime(2025, 7, 23, 14, 17, 54, DateTimeKind.Utc),
                    UpdatedAt = new DateTime(2025, 7, 23, 14, 17, 54, DateTimeKind.Utc),
                    IsDeleted = false
                },
                new MaternityDressTask
                {
                    Id = "4d5854acc4c34ff294d0cc8e29f558e8",
                    MilestoneId = "3bc69c7f00d44442bc3dd095637b4172",
                    Name = "Visual Inspection",
                    Description = "Check overall appearance and construction quality",
                    SequenceOrder = 1,
                    EstimateTimeSpan = 30,
                    CreatedBy = "System",
                    UpdatedBy = "System",
                    CreatedAt = new DateTime(2025, 7, 23, 14, 17, 54, DateTimeKind.Utc),
                    UpdatedAt = new DateTime(2025, 7, 23, 14, 17, 54, DateTimeKind.Utc),
                    IsDeleted = false
                },
                new MaternityDressTask
                {
                    Id = "524ae555a1644d37a8b66dd26724c792",
                    MilestoneId = "130a30e16f764ec7b29c1461f932d10a",
                    Name = "Cut Fabric",
                    Description = "Cut fabric pieces according to preset pattern",
                    SequenceOrder = 3,
                    EstimateTimeSpan = 30,
                    CreatedBy = "Admin",
                    UpdatedBy = "Admin",
                    CreatedAt = new DateTime(2025, 7, 23, 14, 17, 54, DateTimeKind.Utc),
                    UpdatedAt = new DateTime(2025, 7, 23, 14, 17, 54, DateTimeKind.Utc),
                    IsDeleted = false
                },
                new MaternityDressTask
                {
                    Id = "667a4387ded44595b8385f31ff3010d0",
                    MilestoneId = "3bc69c7f00d44442bc3dd095637b4172",
                    Name = "Seam Quality Check",
                    Description = "Inspect all seams for strength and finishing",
                    SequenceOrder = 2,
                    EstimateTimeSpan = 30,
                    CreatedBy = "System",
                    UpdatedBy = "System",
                    CreatedAt = new DateTime(2025, 7, 23, 14, 17, 54, DateTimeKind.Utc),
                    UpdatedAt = new DateTime(2025, 7, 23, 14, 17, 54, DateTimeKind.Utc),
                    IsDeleted = false
                },
                new MaternityDressTask
                {
                    Id = "6bc65fa7010a4adf93cd52135cc0aa4e",
                    MilestoneId = "130a30e16f764ec7b29c1461f932d10a",
                    Name = "Initial Fitting Check",
                    Description = "Basic fitting verification",
                    SequenceOrder = 6,
                    EstimateTimeSpan = 30,
                    CreatedBy = "Admin",
                    UpdatedBy = "Admin",
                    CreatedAt = new DateTime(2025, 7, 23, 14, 17, 54, DateTimeKind.Utc),
                    UpdatedAt = new DateTime(2025, 7, 23, 14, 17, 54, DateTimeKind.Utc),
                    IsDeleted = false
                },
                new MaternityDressTask
                {
                    Id = "6cbafb0310074107892958b2325df8d0",
                    MilestoneId = "e1d5cb20d3e5460b9067113cf4615fa7",
                    Name = "Inspect Repair Work",
                    Description = "Check quality of repaired areas",
                    SequenceOrder = 1,
                    EstimateTimeSpan = 30,
                    CreatedBy = "System",
                    UpdatedBy = "System",
                    CreatedAt = new DateTime(2025, 7, 23, 14, 17, 54, DateTimeKind.Utc),
                    UpdatedAt = new DateTime(2025, 7, 23, 14, 17, 54, DateTimeKind.Utc),
                    IsDeleted = false
                },
                new MaternityDressTask
                {
                    Id = "8694d2af4cef4b2a869d69b2c11ca16e",
                    MilestoneId = "3bc69c7f00d44442bc3dd095637b4172",
                    Name = "Measurement Verification",
                    Description = "Verify garment measurements against specifications",
                    SequenceOrder = 3,
                    EstimateTimeSpan = 30,
                    CreatedBy = "System",
                    UpdatedBy = "System",
                    CreatedAt = new DateTime(2025, 7, 23, 14, 17, 54, DateTimeKind.Utc),
                    UpdatedAt = new DateTime(2025, 7, 23, 14, 17, 54, DateTimeKind.Utc),
                    IsDeleted = false
                },
                new MaternityDressTask
                {
                    Id = "b085a91ce1f9400ba1f8bfc973f2c568",
                    MilestoneId = "130a30e16f764ec7b29c1461f932d10a",
                    Name = "Sew Garment",
                    Description = "Complete main sewing process",
                    SequenceOrder = 4,
                    EstimateTimeSpan = 30,
                    CreatedBy = "Admin",
                    UpdatedBy = "Admin",
                    CreatedAt = new DateTime(2025, 7, 23, 14, 17, 54, DateTimeKind.Utc),
                    UpdatedAt = new DateTime(2025, 7, 23, 14, 17, 54, DateTimeKind.Utc),
                    IsDeleted = false
                },
                new MaternityDressTask
                {
                    Id = "b13f33bdf3fa48059226c45033349224",
                    MilestoneId = "08c1f3fd1f71460e9a4afefc14fa01cb",
                    Name = "Prepare Repair",
                    Description = "Set up repair workspace and materials",
                    SequenceOrder = 1,
                    EstimateTimeSpan = 30,
                    CreatedBy = "System",
                    UpdatedBy = "System",
                    CreatedAt = new DateTime(2025, 7, 23, 14, 17, 54, DateTimeKind.Utc),
                    UpdatedAt = new DateTime(2025, 7, 23, 14, 17, 54, DateTimeKind.Utc),
                    IsDeleted = false
                },
                new MaternityDressTask
                {
                    Id = "bfef208943f5429abaf5ad7265df20c3",
                    MilestoneId = "3bc69c7f00d44442bc3dd095637b4172",
                    Name = "Functional Testing",
                    Description = "Test zippers, buttons, and other functional elements",
                    SequenceOrder = 3,
                    EstimateTimeSpan = 30,
                    CreatedBy = "System",
                    UpdatedBy = "System",
                    CreatedAt = new DateTime(2025, 7, 23, 14, 17, 54, DateTimeKind.Utc),
                    UpdatedAt = new DateTime(2025, 7, 23, 14, 17, 54, DateTimeKind.Utc),
                    IsDeleted = false
                },
                new MaternityDressTask
                {
                    Id = "cca010d95a774c578cd3910aced360e2",
                    MilestoneId = "08c1f3fd1f71460e9a4afefc14fa01cb",
                    Name = "Execute Repair",
                    Description = "Perform repair or replacement work",
                    SequenceOrder = 2,
                    EstimateTimeSpan = 30,
                    CreatedBy = "System",
                    UpdatedBy = "System",
                    CreatedAt = new DateTime(2025, 7, 23, 14, 17, 54, DateTimeKind.Utc),
                    UpdatedAt = new DateTime(2025, 7, 23, 14, 17, 54, DateTimeKind.Utc),
                    IsDeleted = false
                },
                new MaternityDressTask
                {
                    Id = "e067e4747bdd40d4a665d79b91285bdf",
                    MilestoneId = "130a30e16f764ec7b29c1461f932d10a",
                    Name = "Check Order Details",
                    Description = "Review preset order specifications and measurements",
                    SequenceOrder = 1,
                    EstimateTimeSpan = 30,
                    CreatedBy = "Admin",
                    UpdatedBy = "Admin",
                    CreatedAt = new DateTime(2025, 7, 23, 14, 17, 54, DateTimeKind.Utc),
                    UpdatedAt = new DateTime(2025, 7, 23, 14, 17, 54, DateTimeKind.Utc),
                    IsDeleted = false
                },
                new MaternityDressTask
                {
                    Id = "eaf8da4628664f4083af4bf73639b2ea",
                    MilestoneId = "3bc69c7f00d44442bc3dd095637b4172",
                    Name = "Fabric Quality Check",
                    Description = "Check for fabric defects or issues",
                    SequenceOrder = 4,
                    EstimateTimeSpan = 30,
                    CreatedBy = "System",
                    UpdatedBy = "System",
                    CreatedAt = new DateTime(2025, 7, 23, 14, 17, 54, DateTimeKind.Utc),
                    UpdatedAt = new DateTime(2025, 7, 23, 14, 17, 54, DateTimeKind.Utc),
                    IsDeleted = false
                },
                new MaternityDressTask
                {
                    Id = "f4f2ebc128514d3a923b82451c575967",
                    MilestoneId = "afa0cfb3844a4bf99e79b40a009b8f58",
                    Name = "Package Product",
                    Description = "Pack in branded packaging with care instructions",
                    SequenceOrder = 1,
                    EstimateTimeSpan = 30,
                    CreatedBy = "System",
                    UpdatedBy = "System",
                    CreatedAt = new DateTime(2025, 7, 23, 14, 17, 54, DateTimeKind.Utc),
                    UpdatedAt = new DateTime(2025, 7, 23, 14, 17, 54, DateTimeKind.Utc),
                    IsDeleted = false
                },
                new MaternityDressTask
                {
                    Id = "c5d12a4f339a4bc69e2e873ad67512fe",
                    MilestoneId = "a82b6d4f0c194fa5b92348e7d86c4a10",
                    Name = "Quality Review Correction",
                    Description = "Address issues identified in the quality check",
                    SequenceOrder = 1,
                    EstimateTimeSpan = 30,
                    CreatedBy = "Admin",
                    UpdatedBy = "Admin",
                    CreatedAt = new DateTime(2025, 7, 23, 14, 17, 54, DateTimeKind.Utc),
                    UpdatedAt = new DateTime(2025, 7, 23, 14, 17, 54, DateTimeKind.Utc),
                    IsDeleted = false
                });

            #endregion

            #region Seed Dress, Detail

            // Dress seed data
            modelBuilder.Entity<MaternityDress>().HasData(
                new MaternityDress
                {
                    Id = "e610ee9fb2494165a6c0bf196f1b2eba",
                    Name = "Đầm Bầu Thun Gân Cơ Bản Dáng Suông Tay Ngắn Thời Trang Thoải Mái Hằng Ngày",
                    Description =
                        "<p>Đầm bầu dáng suông đơn giản, thiết kế tay ngắn dễ mặc. Kiểu dáng basic, thoải mái cho mẹ bầu trong nhiều hoàn cảnh: đi làm, đi chơi hay ở nhà. Chất liệu thun gân co giãn giúp ôm vừa vặn cơ thể mà vẫn tạo sự thoáng mát, nhẹ nhàng.</p><p><strong>Chất Liệu</strong></p><ul><li><p>Vải thun gân cao cấp</p></li><li><p>Co giãn 4 chiều, mềm mại</p></li><li><p>Thoáng khí, thấm hút tốt</p></li></ul><p><strong>Cách Chăm Sóc</strong></p><ul><li><p>Giặt máy ở chế độ nhẹ hoặc giặt tay</p></li><li><p>Không dùng thuốc tẩy mạnh</p></li><li><p>Phơi nơi thoáng mát, tránh ánh nắng trực tiếp</p></li><li><p>Ủi ở nhiệt độ thấp</p></li></ul><p></p>",
                    Images = new List<string>
                    {
                        "https://firebasestorage.googleapis.com/v0/b/mamafit-e0138.firebasestorage.app/o/images%2F1_1755664026966_gku80f.webp?alt=media&token=523c6ebc-18d7-4f60-ba10-aa5038ab51ab",
                        "https://firebasestorage.googleapis.com/v0/b/mamafit-e0138.firebasestorage.app/o/images%2F2_1755664026967_aen00h.webp?alt=media&token=3c252172-b875-4ce7-a804-2d73bf95598f",
                        "https://firebasestorage.googleapis.com/v0/b/mamafit-e0138.firebasestorage.app/o/images%2F3_1755664026967_3x26tj.webp?alt=media&token=3c479c27-654b-4098-bc61-59c0c2e2bba9"
                    },
                    Slug = "dam-bau-thun-gan-co-ban-dang-suong-tay-ngan-thoi-trang-thoai-mai-hang-ngay",
                    SKU = "MD582",
                    AverageRating = 0.0f,
                    TotalRating = 0,
                    Rating = null,
                    StyleId = "5ed8aceab2d34bd8801e8c6a10dfc0d0",
                    GlobalStatus = 0,
                    CreatedBy = "Admin",
                    UpdatedBy = "Admin",
                    CreatedAt = new DateTime(2025, 8, 20, 4, 27, 15, 363, DateTimeKind.Utc),
                    UpdatedAt = new DateTime(2025, 8, 20, 4, 30, 24, 171, DateTimeKind.Utc),
                    IsDeleted = false
                },
                new MaternityDress
                {
                    Id = "c5773c8eedba465fb0cfea9f8048cbca",
                    Name = "Đầm Bầu Sơ Mi Họa Tiết Hoa Thắt Eo Dáng Suông Thanh Lịch",
                    Description =
                        "<p>Đầm bầu sơ mi họa tiết hoa tươi trẻ, thiết kế thắt dây eo tạo điểm nhấn giúp tôn dáng và che bụng bầu gọn gàng. Dáng suông thoải mái, dễ mặc trong nhiều hoàn cảnh: đi làm, đi chơi hay dự tiệc nhẹ. Phong cách nữ tính, thanh lịch nhưng vẫn mang lại sự thoải mái cho mẹ bầu.</p><h3><strong>Chất Liệu</strong></h3><ul><li><p>Vải cotton pha rayon/voan in hoa</p></li><li><p>Mềm mại, thoáng khí, ít nhăn</p></li><li><p>Thắt dây eo linh hoạt điều chỉnh</p></li></ul><h3><strong>Cách Chăm Sóc</strong></h3><ul><li><p>Giặt máy chế độ nhẹ hoặc giặt tay</p></li><li><p>Không dùng thuốc tẩy mạnh</p></li><li><p>Phơi khô tự nhiên, tránh ánh nắng gắt</p></li><li><p>Ủi nhẹ ở nhiệt độ thấp, ưu tiên hơi nước</p></li></ul><p></p>",
                    Images = new List<string>
                    {
                        "https://firebasestorage.googleapis.com/v0/b/mamafit-e0138.firebasestorage.app/o/images%2F26_1755666733667_9tuldf.webp?alt=media&token=9903703c-9c9f-4e61-ad85-f09b864b6118",
                        "https://firebasestorage.googleapis.com/v0/b/mamafit-e0138.firebasestorage.app/o/images%2F27_1755666733668_bk0k77.webp?alt=media&token=d0701c86-3b85-49a8-99a0-556a27e88e79"
                    },
                    Slug = "dam-bau-so-mi-hoa-tiet-hoa-that-eo-dang-suong-thanh-lich",
                    SKU = "MD585",
                    AverageRating = 0.0f,
                    TotalRating = 0,
                    Rating = null,
                    StyleId = "11363158846e4478b326bf58a7ca9d21",
                    GlobalStatus = 0,
                    CreatedBy = "Admin",
                    UpdatedBy = "Admin",
                    CreatedAt = new DateTime(2025, 8, 20, 5, 12, 18, 14, DateTimeKind.Utc),
                    UpdatedAt = new DateTime(2025, 8, 20, 5, 13, 6, 486, DateTimeKind.Utc),
                    IsDeleted = false
                },
                new MaternityDress
                {
                    Id = "ab00ca8d32fa49b0bd9037572f64351d",
                    Name = "Đầm Bầu Thun Trơn Dáng Suông Mini Tay Ngắn Thoải Mái Hằng Ngày",
                    Description =
                        "<p>Đầm bầu mini dáng suông trẻ trung, thiết kế trơn basic dễ phối với nhiều phong cách. Form rộng rãi giúp mẹ bầu thoải mái di chuyển, phù hợp khi đi làm, đi dạo hay mặc ở nhà. Chiều dài mini tạo cảm giác năng động, hiện đại.</p><h3><strong>Chất Liệu</strong></h3><ul><li><p>Vải thun mềm mại, co giãn tốt</p></li><li><p>Thấm hút mồ hôi, thoáng khí</p></li><li><p>Bề mặt vải mịn, ít nhăn</p></li></ul><h3><strong>Cách Chăm Sóc</strong></h3><ul><li><p>Giặt máy chế độ nhẹ hoặc giặt tay</p></li><li><p>Không sử dụng thuốc tẩy mạnh</p></li><li><p>Phơi khô tự nhiên, tránh nắng gắt</p></li><li><p>Ủi ở nhiệt độ thấp</p></li></ul><p></p>",
                    Images = new List<string>
                    {
                        "https://firebasestorage.googleapis.com/v0/b/mamafit-e0138.firebasestorage.app/o/images%2F5_1755664463275_fe2k1i.webp?alt=media&token=0e9677e3-c641-437a-9be6-335c27784ec4",
                        "https://firebasestorage.googleapis.com/v0/b/mamafit-e0138.firebasestorage.app/o/images%2F7_1755664463276_lw9ovp.webp?alt=media&token=030cd258-5c2c-4038-9e08-2d158f7b9011",
                        "https://firebasestorage.googleapis.com/v0/b/mamafit-e0138.firebasestorage.app/o/images%2F8_1755664463276_6usrfm.webp?alt=media&token=80a2cf35-3929-4e73-828b-3d11bd10f86d"
                    },
                    Slug = "dam-bau-thun-tron-dang-suong-mini-tay-ngan-thoai-mai-hang-ngay",
                    SKU = "MD479",
                    AverageRating = 0.0f,
                    TotalRating = 0,
                    Rating = null,
                    StyleId = "5ed8aceab2d34bd8801e8c6a10dfc0d0",
                    GlobalStatus = 0,
                    CreatedBy = "Admin",
                    UpdatedBy = "Admin",
                    CreatedAt = new DateTime(2025, 8, 20, 4, 36, 31, 330, DateTimeKind.Utc),
                    UpdatedAt = new DateTime(2025, 8, 20, 4, 40, 27, 2, DateTimeKind.Utc),
                    IsDeleted = false
                },
                new MaternityDress
                {
                    Id = "6f0fefb8162f458f98e032cf224becf2",
                    Name = "Đầm Bầu Thun Xếp Ly Bất Đối Xứng Dáng Suông Ôm Nhẹ Thời Trang Hiện Đại",
                    Description =
                        "<p>Đầm bầu thiết kế bất đối xứng độc đáo, nhấn nhá chi tiết xếp ly tinh tế tạo cảm giác thon gọn và thời trang. Dáng suông ôm nhẹ, phù hợp cho mẹ bầu vừa muốn thoải mái vừa giữ được phong cách hiện đại. Có thể mặc đi làm, dạo phố hoặc dự tiệc nhẹ.</p><h3><strong>Chất Liệu</strong></h3><ul><li><p>Vải thun cao cấp, co giãn 4 chiều</p></li><li><p>Bề mặt mịn, ôm form vừa vặn</p></li><li><p>Thấm hút tốt, thoáng mát cả ngày</p></li></ul><h3><strong>Cách Chăm Sóc</strong></h3><ul><li><p>Giặt tay hoặc giặt máy chế độ nhẹ</p></li><li><p>Không dùng thuốc tẩy mạnh</p></li><li><p>Phơi trong bóng râm, tránh nắng gắt</p></li><li><p>Ủi nhẹ ở nhiệt độ thấp</p></li></ul><p></p>",
                    Images = new List<string>
                    {
                        "https://firebasestorage.googleapis.com/v0/b/mamafit-e0138.firebasestorage.app/o/images%2F9_1755664927531_8hiuaa.webp?alt=media&token=ead50e57-f8c6-4099-b463-149f74164722",
                        "https://firebasestorage.googleapis.com/v0/b/mamafit-e0138.firebasestorage.app/o/images%2F10_1755664927532_fdcv17.webp?alt=media&token=acb2e49e-624e-444b-b31f-949383c80347",
                        "https://firebasestorage.googleapis.com/v0/b/mamafit-e0138.firebasestorage.app/o/images%2F11_1755664927532_d856sz.webp?alt=media&token=058e96d1-a695-406d-8b03-bcb7760bb2db"
                    },
                    Slug = "dam-bau-thun-xep-ly-bat-doi-xung-dang-suong-om-nhe-thoi-trang-hien-dai",
                    SKU = "MD405",
                    AverageRating = 0.0f,
                    TotalRating = 0,
                    Rating = null,
                    StyleId = "5ed8aceab2d34bd8801e8c6a10dfc0d0",
                    GlobalStatus = 0,
                    CreatedBy = "Admin",
                    UpdatedBy = "Admin",
                    CreatedAt = new DateTime(2025, 8, 20, 4, 42, 12, 832, DateTimeKind.Utc),
                    UpdatedAt = new DateTime(2025, 8, 20, 4, 43, 57, 964, DateTimeKind.Utc),
                    IsDeleted = false
                },
                new MaternityDress
                {
                    Id = "a3295084c7ca4c948017b82fa1d06ebf",
                    Name = "Đầm Bầu Cổ Yếm Xếp Ly Dáng Suông Thời Trang Thanh Lịch",
                    Description =
                        "<p>Đầm bầu thiết kế cổ yếm hiện đại, mang đến vẻ ngoài thanh lịch và nữ tính. Chi tiết xếp ly nhẹ nhàng tạo độ rũ tự nhiên, giúp che dáng hiệu quả mà vẫn giữ sự thoải mái. Phù hợp cho mẹ bầu trong các dịp dự tiệc, sự kiện hoặc dạo phố.</p><h3><strong>Chất Liệu</strong></h3><ul><li><p>Vải voan/lụa cao cấp, mềm mại</p></li><li><p>Xếp ly tinh tế, giữ form tốt</p></li><li><p>Thoáng mát, nhẹ nhàng khi mặc</p></li></ul><h3><strong>Cách Chăm Sóc</strong></h3><ul><li><p>Giặt tay nhẹ, hạn chế giặt máy</p></li><li><p>Không vắt mạnh, tránh nhăn ly</p></li><li><p>Phơi tự nhiên nơi thoáng mát</p></li><li><p>Ủi bằng hơi nước ở nhiệt độ thấp</p></li></ul><p></p>",
                    Images = new List<string>
                    {
                        "https://firebasestorage.googleapis.com/v0/b/mamafit-e0138.firebasestorage.app/o/images%2F13_1755665190797_pl6kg4.webp?alt=media&token=4a602e98-117d-4991-83a0-35fc97900c81",
                        "https://firebasestorage.googleapis.com/v0/b/mamafit-e0138.firebasestorage.app/o/images%2F14_1755665190798_l17w7c.webp?alt=media&token=bb2f0c23-dda4-4892-809e-c1ec2ef61ee5",
                        "https://firebasestorage.googleapis.com/v0/b/mamafit-e0138.firebasestorage.app/o/images%2F15_1755665190798_fk8d63.webp?alt=media&token=8c4bd588-42b3-4338-899d-ea9b81297e93"
                    },
                    Slug = "dam-bau-co-yem-xep-ly-dang-suong-thoi-trang-thanh-lich",
                    SKU = "MD566",
                    AverageRating = 0.0f,
                    TotalRating = 0,
                    Rating = null,
                    StyleId = "bd6ae7259f6d42fbac946b3319a92406",
                    GlobalStatus = 0,
                    CreatedBy = "Admin",
                    UpdatedBy = "Admin",
                    CreatedAt = new DateTime(2025, 8, 20, 4, 46, 52, 475, DateTimeKind.Utc),
                    UpdatedAt = new DateTime(2025, 8, 20, 4, 50, 33, 945, DateTimeKind.Utc),
                    IsDeleted = false
                },
                new MaternityDress
                {
                    Id = "7202c22db49e4511b19661285cf363df",
                    Name = "Đầm Bầu Maxi Cổ Yếm Smocked Co Giãn Dáng Dài Thoải Mái Thanh Lịch",
                    Description =
                        "<p>Đầm bầu maxi dáng dài với thiết kế cổ yếm hiện đại, phần thân trên smocked co giãn ôm vừa vặn, tôn dáng mà vẫn dễ chịu cho mẹ bầu. Chiều dài maxi thướt tha mang lại vẻ nữ tính, thanh lịch, phù hợp cho dạo biển, dự tiệc hay đi chơi cuối tuần.</p><h3><strong>Chất Liệu</strong></h3><ul><li><p>Vải cotton pha rayon/voan mềm mại</p></li><li><p>Phần smocked co giãn linh hoạt</p></li><li><p>Thoáng mát, nhẹ nhàng, dễ chịu cả ngày</p></li></ul><h3><strong>Cách Chăm Sóc</strong></h3><ul><li><p>Giặt tay hoặc giặt máy chế độ nhẹ</p></li><li><p>Không dùng thuốc tẩy mạnh</p></li><li><p>Phơi trong bóng râm, tránh ánh nắng trực tiếp</p></li><li><p>Ủi nhẹ ở nhiệt độ thấp, ưu tiên hơi nước</p></li></ul><p></p>",
                    Images = new List<string>
                    {
                        "https://firebasestorage.googleapis.com/v0/b/mamafit-e0138.firebasestorage.app/o/images%2F16_1755665554429_2dk5aj.webp?alt=media&token=f8bc21ee-0125-49c0-a4ad-92d21ee0ee57",
                        "https://firebasestorage.googleapis.com/v0/b/mamafit-e0138.firebasestorage.app/o/images%2F17_1755665554430_damvff.webp?alt=media&token=424a337e-6cd1-4870-b628-ffac503b0b0a",
                        "https://firebasestorage.googleapis.com/v0/b/mamafit-e0138.firebasestorage.app/o/images%2F18_1755665554430_e2sm12.webp?alt=media&token=dbc2f371-c454-4adf-8775-85166f705e30",
                        "https://firebasestorage.googleapis.com/v0/b/mamafit-e0138.firebasestorage.app/o/images%2F19_1755665554430_z0js4s.webp?alt=media&token=8495676a-d79c-49c8-bb20-64a83bd81f5d"
                    },
                    Slug = "dam-bau-maxi-co-yem-smocked-co-gian-dang-dai-thoai-mai-thanh-lich",
                    SKU = "MD920",
                    AverageRating = 0.0f,
                    TotalRating = 0,
                    Rating = null,
                    StyleId = "bd6ae7259f6d42fbac946b3319a92406",
                    GlobalStatus = 0,
                    CreatedBy = "Admin",
                    UpdatedBy = "Admin",
                    CreatedAt = new DateTime(2025, 8, 20, 4, 52, 38, 837, DateTimeKind.Utc),
                    UpdatedAt = new DateTime(2025, 8, 20, 4, 54, 0, 885, DateTimeKind.Utc),
                    IsDeleted = false
                },
                new MaternityDress
                {
                    Id = "058811e7dd6842a59ea67452a301ce84",
                    Name = "Đầm Bầu Sơ Mi Dáng Suông Tay Ngắn Cơ Nút Chất Liệu Gân Nhẹ Thoáng Mát",
                    Description =
                        "<p>Đầm bầu sơ mi cổ điển với hàng nút phía trước, thiết kế tay ngắn tiện lợi. Chất liệu vải gân nhẹ tạo cảm giác thoáng mát, dễ chịu cho mẹ bầu. Dáng suông thoải mái, phù hợp đi làm, dạo phố hoặc mặc thường ngày.</p><h3><strong>Chất Liệu</strong></h3><ul><li><p>Vải gân cotton pha polyester</p></li><li><p>Thoáng khí, mềm mại, ít nhăn</p></li><li><p>Dễ dàng vận động, giữ form tốt</p></li></ul><h3><strong>Cách Chăm Sóc</strong></h3><ul><li><p>Giặt máy chế độ nhẹ hoặc giặt tay</p></li><li><p>Không dùng thuốc tẩy mạnh</p></li><li><p>Phơi nơi thoáng gió, tránh nắng gắt</p></li><li><p>Ủi ở nhiệt độ thấp, nên ủi khi vải còn ẩm</p></li></ul><p></p>",
                    Images = new List<string>
                    {
                        "https://firebasestorage.googleapis.com/v0/b/mamafit-e0138.firebasestorage.app/o/images%2F21_1755666412679_qzyqzs.webp?alt=media&token=42219328-3264-4f84-9973-306a5e2c9391",
                        "https://firebasestorage.googleapis.com/v0/b/mamafit-e0138.firebasestorage.app/o/images%2F22_1755666412680_915gcc.webp?alt=media&token=ff532280-b9f8-4835-9644-75972703f588",
                        "https://firebasestorage.googleapis.com/v0/b/mamafit-e0138.firebasestorage.app/o/images%2F23_1755666412681_ccsxb9.webp?alt=media&token=8d4395c7-4dd9-4ff1-bf84-19c2258bbdd1",
                        "https://firebasestorage.googleapis.com/v0/b/mamafit-e0138.firebasestorage.app/o/images%2F24_1755666412681_lpxt3c.webp?alt=media&token=42170be9-50f4-4b03-812f-49dadcb718c9"
                    },
                    Slug = "dam-bau-so-mi-dang-suong-tay-ngan-co-nut-chat-lieu-gan-nhe-thoang-mat",
                    SKU = "MD515",
                    AverageRating = 0.0f,
                    TotalRating = 0,
                    Rating = null,
                    StyleId = "11363158846e4478b326bf58a7ca9d21",
                    GlobalStatus = 0,
                    CreatedBy = "Admin",
                    UpdatedBy = "Admin",
                    CreatedAt = new DateTime(2025, 8, 20, 5, 7, 24, 955, DateTimeKind.Utc),
                    UpdatedAt = new DateTime(2025, 8, 20, 5, 10, 30, 365, DateTimeKind.Utc),
                    IsDeleted = false
                },
                new MaternityDress
                {
                    Id = "fdff22c67ad7425ebf40b2f457fc3228",
                    Name = "Đầm Bầu Maxi Jean Chambray Trễ Vai Smocked Co Giãn Thoải Mái Thời Trang",
                    Description =
                        "<p>Đầm bầu maxi dáng dài làm từ chất liệu chambray denim nhẹ, thiết kế trễ vai nữ tính kết hợp phần smocked co giãn ôm vừa vặn phần ngực. Dáng maxi thướt tha mang đến sự thoải mái, trẻ trung và phong cách. Thích hợp cho mẹ bầu khi đi dạo phố, du lịch hay dự tiệc ngoài trời.</p><h3><strong>Chất Liệu</strong></h3><ul><li><p>Chambray denim mềm nhẹ, thoáng khí</p></li><li><p>Smocked co giãn linh hoạt, ôm dáng vừa vặn</p></li><li><p>Bền màu, thoải mái khi mặc</p></li></ul><h3><strong>Cách Chăm Sóc</strong></h3><ul><li><p>Giặt máy chế độ nhẹ hoặc giặt tay</p></li><li><p>Không dùng thuốc tẩy mạnh</p></li><li><p>Phơi nơi thoáng mát, tránh ánh nắng gắt</p></li><li><p>Ủi ở nhiệt độ trung bình, lộn trái khi ủi</p></li></ul><p></p>",
                    Images = new List<string>
                    {
                        "https://firebasestorage.googleapis.com/v0/b/mamafit-e0138.firebasestorage.app/o/images%2F28_1755666977929_37y0c2.webp?alt=media&token=c19e606c-0879-43e8-913f-dd66569fa1c6",
                        "https://firebasestorage.googleapis.com/v0/b/mamafit-e0138.firebasestorage.app/o/images%2F29_1755666977930_x8d6zx.webp?alt=media&token=b372199a-502a-4bab-bd95-d1355d3b9282",
                        "https://firebasestorage.googleapis.com/v0/b/mamafit-e0138.firebasestorage.app/o/images%2F30_1755666977930_8zlgyp.webp?alt=media&token=6e5780b4-1750-4984-ac89-89a4fcb0e615"
                    },
                    Slug = "dam-bau-maxi-jean-chambray-tre-vai-smocked-co-gian-thoai-mai-thoi-trang",
                    SKU = "MD220",
                    AverageRating = 0.0f,
                    TotalRating = 0,
                    Rating = null,
                    StyleId = "f5d9785312d34110a0aaed96272b630a",
                    GlobalStatus = 0,
                    CreatedBy = "Admin",
                    UpdatedBy = "Admin",
                    CreatedAt = new DateTime(2025, 8, 20, 5, 16, 22, 399, DateTimeKind.Utc),
                    UpdatedAt = new DateTime(2025, 8, 20, 5, 17, 1, 406, DateTimeKind.Utc),
                    IsDeleted = false
                },
                new MaternityDress
                {
                    Id = "039fd8cb6da548299d31cd0b3780d528",
                    Name = "Đầm Bầu Maxi Sơ Mi Cotton Phối Họa Tiết Patchwork Có Nút Dài Thoải Mái Cá Tính",
                    Description =
                        "<p>Đầm bầu maxi dáng sơ mi dài, thiết kế phối patchwork nhiều họa tiết độc đáo từ chất liệu cotton mềm nhẹ. Hàng nút phía trước tiện lợi, dễ mặc và thoải mái cho mẹ bầu. Kiểu dáng vừa phóng khoáng vừa thời trang, phù hợp khi đi dạo phố, du lịch hoặc mặc hằng ngày.</p><h3><strong>Chất Liệu</strong></h3><ul><li><p>100% cotton hoặc cotton pha mềm mại</p></li><li><p>Thấm hút mồ hôi, thoáng khí</p></li><li><p>Phối patchwork bền màu, độc đáo</p></li></ul><h3><strong>Cách Chăm Sóc</strong></h3><ul><li><p>Giặt máy chế độ nhẹ, tách màu đậm nhạt</p></li><li><p>Không dùng thuốc tẩy mạnh</p></li><li><p>Phơi nơi thoáng mát, tránh ánh nắng trực tiếp</p></li><li><p>Ủi ở nhiệt độ trung bình, nên ủi khi vải còn hơi ẩm</p></li></ul><p></p>",
                    Images =
                    [
                        "https://firebasestorage.googleapis.com/v0/b/mamafit-e0138.firebasestorage.app/o/images%2F36_1755667248975_kj2tcn.webp?alt=media&token=8bd7c41b-7751-49a9-8eff-0a2c4fdcc8c9,https://firebasestorage.googleapis.com/v0/b/mamafit-e0138.firebasestorage.app/o/images%2F37_1755667248976_9mcq7e.webp?alt=media&token=fc162103-bddd-4a33-8cdd-0ed26445afdf,https://firebasestorage.googleapis.com/v0/b/mamafit-e0138.firebasestorage.app/o/images%2F38_1755667248977_ptwwk3.webp?alt=media&token=0d0e727a-35cd-4333-b0bf-6d836be00e35,https://firebasestorage.googleapis.com/v0/b/mamafit-e0138.firebasestorage.app/o/images%2F39_1755667248977_gew4mo.webp?alt=media&token=8fa985c3-50f6-4c1e-b0e3-a691967eab39"
                    ],
                    Slug = "dam-bau-maxi-so-mi-cotton-phoi-hoa-tiet-patchwork-co-nut-dai-thoai-mai-ca-tinh",
                    SKU = "MD128",
                    AverageRating = 0.0f,
                    TotalRating = 0,
                    Rating = null,
                    StyleId = "037b41302a4c453f8cef135afceb3956",
                    GlobalStatus = 0,
                    CreatedBy = "Admin",
                    UpdatedBy = "Admin",
                    CreatedAt = new DateTime(2025, 8, 20, 5, 16, 22, 399, DateTimeKind.Utc),
                    UpdatedAt = new DateTime(2025, 8, 20, 5, 17, 1, 406, DateTimeKind.Utc),
                    IsDeleted = false
                },
                new MaternityDress
                {
                    Id = "581aec917bea4d9d9054b8cf1d1cc7b8",
                    Name = "Đầm Bầu Midi Trễ Vai Xếp Ly Dáng Ôm Nhẹ Sang Trọng Quyến Rũ",
                    Description =
                        "<p>Đầm bầu midi trễ vai với thiết kế xếp ly dọc thân giúp tạo hiệu ứng thon gọn, tôn dáng mà vẫn thoải mái cho mẹ bầu. Form ôm nhẹ kết hợp chiều dài midi mang đến sự thanh lịch, quyến rũ, thích hợp cho các buổi tiệc, sự kiện hoặc hẹn hò.</p><h3><strong>Chất Liệu</strong></h3><ul><li><p>Vải thun co giãn 4 chiều</p></li><li><p>Mềm mại, thoáng khí, ôm dáng vừa vặn</p></li><li><p>Giữ phom tốt, ít nhăn</p></li></ul><h3><strong>Cách Chăm Sóc</strong></h3><ul><li><p>Giặt tay hoặc giặt máy chế độ nhẹ</p></li><li><p>Không sử dụng thuốc tẩy mạnh</p></li><li><p>Phơi tự nhiên nơi thoáng gió, tránh ánh nắng trực tiếp</p></li><li><p>Ủi nhẹ ở nhiệt độ thấp</p></li></ul><p></p>",
                    Images =
                    [
                        "https://firebasestorage.googleapis.com/v0/b/mamafit-e0138.firebasestorage.app/o/images%2F32_1755667088967_kmint4.webp?alt=media&token=f9fe98a1-b1cb-4b83-97f4-4c6f8715d49c,https://firebasestorage.googleapis.com/v0/b/mamafit-e0138.firebasestorage.app/o/images%2F34_1755667088968_mmep92.webp?alt=media&token=067ca95c-2dfb-4b0b-9ded-1436513fe4d7,https://firebasestorage.googleapis.com/v0/b/mamafit-e0138.firebasestorage.app/o/images%2F35_1755667088968_sl8kv0.webp?alt=media&token=3ea7fe14-0a3f-47f3-9f51-2acba56bd41f"
                    ],
                    Slug = "dam-bau-midi-tre-vai-xep-ly-dang-om-nhe-sang-trong-quyen-ru",
                    SKU = "MD846",
                    AverageRating = 0.0f,
                    TotalRating = 0,
                    Rating = null,
                    StyleId = "f5d9785312d34110a0aaed96272b630a",
                    GlobalStatus = 0,
                    CreatedBy = "Admin",
                    UpdatedBy = "Admin",
                    CreatedAt = new DateTime(2025, 8, 20, 5, 16, 22, 399, DateTimeKind.Utc),
                    UpdatedAt = new DateTime(2025, 8, 20, 5, 17, 1, 406, DateTimeKind.Utc),
                    IsDeleted = false
                });
            modelBuilder.Entity<MaternityDressDetail>().HasData(
                new MaternityDressDetail
                {
                    Id = "5fc32f022d374382b0c7dd67759bbbd3",
                    MaternityDressId = "e610ee9fb2494165a6c0bf196f1b2eba",
                    SKU = "MD582-001",
                    Name = "Váy thun bà bầu tay ngắn dệt kim gân đen cơ bản",
                    Color = "Black",
                    Size = "S",
                    Price = 3000,
                    Quantity = 999,
                    Image = ["https://res.cloudinary.com/dzykiyef5/image/upload/v1755664170/ohnkiuwhpvtrlkv2kljj.webp"],
                    CreatedBy = "Admin",
                    UpdatedBy = "",
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    IsDeleted = false
                },
                new MaternityDressDetail
                {
                    Id = "31836c7be57248fda82a237f0a206875",
                    MaternityDressId = "e610ee9fb2494165a6c0bf196f1b2eba",
                    SKU = "MD582-002",
                    Name = "Váy thun bà bầu tay ngắn dệt kim gân đen cơ bản",
                    Color = "Black",
                    Size = "M",
                    Price = 4000,
                    Quantity = 999,
                    Image = ["https://res.cloudinary.com/dzykiyef5/image/upload/v1755664191/n87jx1o0utbpwtjc0sjc.webp"],
                    CreatedBy = "Admin",
                    UpdatedBy = "",
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    IsDeleted = false
                },
                new MaternityDressDetail
                {
                    Id = "0014266fe6354109aa870bdc11d3a0f1",
                    MaternityDressId = "e610ee9fb2494165a6c0bf196f1b2eba",
                    SKU = "MD582-003",
                    Name = "Váy thun bà bầu tay ngắn dệt kim gân đen cơ bản",
                    Color = "Black",
                    Size = "L",
                    Price = 5000,
                    Quantity = 999,
                    Image = ["https://res.cloudinary.com/dzykiyef5/image/upload/v1755664222/fjfnf1czaqmnobicojg6.webp"],
                    CreatedBy = "Admin",
                    UpdatedBy = "",
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    IsDeleted = false
                },
                new MaternityDressDetail
                {
                    Id = "af9d2d662191461a845b106c09e5e0a3",
                    MaternityDressId = "ab00ca8d32fa49b0bd9037572f64351d",
                    SKU = "MD479-001",
                    Name = "Váy ngắn bà bầu trơn màu xanh hoàng gia",
                    Color = "Blue",
                    Size = "S",
                    Price = 4000,
                    Quantity = 999,
                    Image = ["https://res.cloudinary.com/dzykiyef5/image/upload/v1755664644/cetselylq4gofjm7dqqu.webp"],
                    CreatedBy = "Admin",
                    UpdatedBy = "",
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    IsDeleted = false
                },
                new MaternityDressDetail
                {
                    Id = "9dd310f17d8b4f5d9c5b6fe7e117ed24",
                    MaternityDressId = "ab00ca8d32fa49b0bd9037572f64351d",
                    SKU = "MD479-002",
                    Name = "Váy ngắn bà bầu trơn màu xanh hoàng gia",
                    Color = "Blue",
                    Size = "M",
                    Price = 4000,
                    Quantity = 999,
                    Image = ["https://res.cloudinary.com/dzykiyef5/image/upload/v1755664662/dkekqrmeuph2aw86dzcc.webp"],
                    CreatedBy = "Admin",
                    UpdatedBy = "",
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    IsDeleted = false
                },
                new MaternityDressDetail
                {
                    Id = "35c1394d34ec4edda3e8409d00009031",
                    MaternityDressId = "ab00ca8d32fa49b0bd9037572f64351d",
                    SKU = "MD479-003",
                    Name = "Váy ngắn bà bầu trơn màu xanh hoàng gia",
                    Color = "Blue",
                    Size = "L",
                    Price = 4000,
                    Quantity = 999,
                    Image = ["https://res.cloudinary.com/dzykiyef5/image/upload/v1755664683/qjrw1hpuhnudfvr4ynd7.webp"],
                    CreatedBy = "Admin",
                    UpdatedBy = "",
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    IsDeleted = false
                },
                new MaternityDressDetail
                {
                    Id = "21197e4bd7a94f77b173f69a7d3a326a",
                    MaternityDressId = "ab00ca8d32fa49b0bd9037572f64351d",
                    SKU = "MD479-004",
                    Name = " Váy ngắn bà bầu màu than trơn",
                    Color = "Charcoal",
                    Size = "S",
                    Price = 3000,
                    Quantity = 999,
                    Image = ["https://res.cloudinary.com/dzykiyef5/image/upload/v1755664723/plyadulp1hgcbhu4smzb.webp"],
                    CreatedBy = "Admin",
                    UpdatedBy = "",
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    IsDeleted = false
                },
                new MaternityDressDetail
                {
                    Id = "e8a851a8d34c417e8e391687fd4e3882",
                    MaternityDressId = "ab00ca8d32fa49b0bd9037572f64351d",
                    SKU = "MD479-005",
                    Name = " Váy ngắn bà bầu màu than trơn",
                    Color = "Charcoal",
                    Size = "M",
                    Price = 4000,
                    Quantity = 999,
                    Image = ["https://res.cloudinary.com/dzykiyef5/image/upload/v1755664742/n9s03jrvxv10lgotg3pi.webp"],
                    CreatedBy = "Admin",
                    UpdatedBy = "",
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    IsDeleted = false
                },
                new MaternityDressDetail
                {
                    Id = "0e61a37b07134f94916d47ea4937150e",
                    MaternityDressId = "ab00ca8d32fa49b0bd9037572f64351d",
                    SKU = "MD479-006",
                    Name = "Váy ngắn bà bầu màu Mocha trơn",
                    Color = "Mocha",
                    Size = "L",
                    Price = 5000,
                    Quantity = 999,
                    Image = ["https://res.cloudinary.com/dzykiyef5/image/upload/v1755664802/dro2qohp0twsm79b3ev7.webp"],
                    CreatedBy = "Admin",
                    UpdatedBy = "",
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    IsDeleted = false
                },
                new MaternityDressDetail
                {
                    Id = "e50c86450e534025948a4019d8975cd4",
                    MaternityDressId = "ab00ca8d32fa49b0bd9037572f64351d",
                    SKU = "MD479-007",
                    Name = "Váy ngắn bà bầu màu Mocha trơn",
                    Color = "Mocha",
                    Size = "XL",
                    Price = 10000,
                    Quantity = 999,
                    Image = ["https://res.cloudinary.com/dzykiyef5/image/upload/v1755664824/lk8thux237iaf3xlfb18.webp"],
                    CreatedBy = "Admin",
                    UpdatedBy = "",
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    IsDeleted = false
                },
                new MaternityDressDetail
                {
                    Id = "d5b90f90bd8e4952be7355294e2b004c",
                    MaternityDressId = "6f0fefb8162f458f98e032cf224becf2",
                    SKU = "MD405-001",
                    Name = "Váy bầu nhún bèo bất đối xứng màu xám",
                    Color = "Gray",
                    Size = "S",
                    Price = 3000,
                    Quantity = 999,
                    Image = ["https://res.cloudinary.com/dzykiyef5/image/upload/v1755665002/aairhrfkyt2coikhbi17.webp"],
                    CreatedBy = "Admin",
                    UpdatedBy = "",
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    IsDeleted = false
                },
                new MaternityDressDetail
                {
                    Id = "66253122a0c54cd59f103e7e61cd5b90",
                    MaternityDressId = "6f0fefb8162f458f98e032cf224becf2",
                    SKU = "MD405-002",
                    Name = "Váy bầu nhún bèo bất đối xứng màu xám",
                    Color = "Gray",
                    Size = "M",
                    Price = 4000,
                    Quantity = 999,
                    Image = ["https://res.cloudinary.com/dzykiyef5/image/upload/v1755665017/wzuybbih4zm9ddt0xpom.webp"],
                    CreatedBy = "Admin",
                    UpdatedBy = "",
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    IsDeleted = false
                },
                new MaternityDressDetail
                {
                    Id = "c90a707cd4414c12831e090be2855140",
                    MaternityDressId = "6f0fefb8162f458f98e032cf224becf2",
                    SKU = "MD405-003",
                    Name = "Váy bầu nhún bèo bất đối xứng màu xám",
                    Color = "Gray",
                    Size = "L",
                    Price = 5000,
                    Quantity = 999,
                    Image = ["https://res.cloudinary.com/dzykiyef5/image/upload/v1755665036/ef9y9tybbgmw01fwswr9.webp"],
                    CreatedBy = "Admin",
                    UpdatedBy = "",
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    IsDeleted = false
                },
                new MaternityDressDetail
                {
                    Id = "32c6fdb19661480b84bdebf8de5df8eb",
                    MaternityDressId = "a3295084c7ca4c948017b82fa1d06ebf",
                    SKU = "MD566-001",
                    Name = "Váy yếm xếp ly màu ô liu cho bà bầu",
                    Color = "Olive",
                    Size = "S",
                    Price = 4000,
                    Quantity = 999,
                    Image = ["https://res.cloudinary.com/dzykiyef5/image/upload/v1755665249/cnuulshsmipa0zmuq49b.webp"],
                    CreatedBy = "Admin",
                    UpdatedBy = "",
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    IsDeleted = false
                },
                new MaternityDressDetail
                {
                    Id = "48d92e9e21a54e3c847adac8cc855f62",
                    MaternityDressId = "a3295084c7ca4c948017b82fa1d06ebf",
                    SKU = "MD566-002",
                    Name = "Váy yếm xếp ly màu ô liu cho bà bầu",
                    Color = "Olive",
                    Size = "M",
                    Price = 5000,
                    Quantity = 999,
                    Image = ["https://res.cloudinary.com/dzykiyef5/image/upload/v1755665264/xosnknggvbkecz9munvu.webp"],
                    CreatedBy = "Admin",
                    UpdatedBy = "",
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    IsDeleted = false
                },
                new MaternityDressDetail
                {
                    Id = "20f6cc4a10ff47b1b6a5eeb93b7b2cd6",
                    MaternityDressId = "a3295084c7ca4c948017b82fa1d06ebf",
                    SKU = "MD566-003",
                    Name = "Váy yếm xếp ly màu ô liu cho bà bầu",
                    Color = "Olive",
                    Size = "L",
                    Price = 6000,
                    Quantity = 999,
                    Image = ["https://res.cloudinary.com/dzykiyef5/image/upload/v1755665283/qthh4wyuvv906urp9iyr.webp"],
                    CreatedBy = "Admin",
                    UpdatedBy = "",
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    IsDeleted = false
                },
                new MaternityDressDetail
                {
                    Id = "33de2e83cd6c41f69ee7d34d5a8af5a2",
                    MaternityDressId = "a3295084c7ca4c948017b82fa1d06ebf",
                    SKU = "MD566-004",
                    Name = "Váy yếm xếp ly màu đen cho bà bầu",
                    Color = "Black",
                    Size = "S",
                    Price = 4000,
                    Quantity = 999,
                    Image = ["https://res.cloudinary.com/dzykiyef5/image/upload/v1755665314/rerxwpmm2xm6czpsd4ie.webp"],
                    CreatedBy = "Admin",
                    UpdatedBy = "",
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    IsDeleted = false
                },
                new MaternityDressDetail
                {
                    Id = "be5de84bde154daab7c053d41fc9af8b",
                    MaternityDressId = "a3295084c7ca4c948017b82fa1d06ebf",
                    SKU = "MD566-005",
                    Name = "Váy yếm xếp ly màu đen cho bà bầu",
                    Color = "Black",
                    Size = "M",
                    Price = 4000,
                    Quantity = 999,
                    Image = ["https://res.cloudinary.com/dzykiyef5/image/upload/v1755665331/ixmlsfs7dtojv1avpqtz.webp"],
                    CreatedBy = "Admin",
                    UpdatedBy = "",
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    IsDeleted = false
                },
                new MaternityDressDetail
                {
                    Id = "3595c4086597411caa4a2af7dd3cf062",
                    MaternityDressId = "a3295084c7ca4c948017b82fa1d06ebf",
                    SKU = "MD566-006",
                    Name = "Váy yếm xếp ly màu đen cho bà bầu",
                    Color = "Black",
                    Size = "XL",
                    Price = 10000,
                    Quantity = 999,
                    Image = ["https://res.cloudinary.com/dzykiyef5/image/upload/v1755665362/ns4vd2ytewr9pinzb7yi.webp"],
                    CreatedBy = "Admin",
                    UpdatedBy = "",
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    IsDeleted = false
                },
                new MaternityDressDetail
                {
                    Id = "49a7c65b458a4348b56f3cd414056d8d",
                    MaternityDressId = "a3295084c7ca4c948017b82fa1d06ebf",
                    SKU = "MD566-007",
                    Name = "Váy yếm xếp ly màu xanh nhạt cho bà bầu",
                    Color = "Light Blue",
                    Size = "XL",
                    Price = 10000,
                    Quantity = 999,
                    Image = ["https://res.cloudinary.com/dzykiyef5/image/upload/v1755665415/r0plpmqdwne6ex4geh0w.webp"],
                    CreatedBy = "Admin",
                    UpdatedBy = "",
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    IsDeleted = false
                },
                new MaternityDressDetail
                {
                    Id = "99a9e2e51d834bb88a6ceb7c28df8754",
                    MaternityDressId = "a3295084c7ca4c948017b82fa1d06ebf",
                    SKU = "MD566-008",
                    Name = "Váy yếm xếp ly màu xanh nhạt cho bà bầu",
                    Color = "Light Blue",
                    Size = "2XL",
                    Price = 20000,
                    Quantity = 999,
                    Image = ["https://res.cloudinary.com/dzykiyef5/image/upload/v1755665432/b25c8zu3aayomgdkrlcl.webp"],
                    CreatedBy = "Admin",
                    UpdatedBy = "",
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    IsDeleted = false
                },
                new MaternityDressDetail
                {
                    Id = "23b349cd42fd4c0b9c3ece3a9dbcd148",
                    MaternityDressId = "7202c22db49e4511b19661285cf363df",
                    SKU = "MD920-001",
                    Name = "Váy Maxi Bầu Halter Xếp Ly Màu Be",
                    Color = "Beige",
                    Size = "S",
                    Price = 3000,
                    Quantity = 999,
                    Image = ["https://res.cloudinary.com/dzykiyef5/image/upload/v1755665606/bmdny2uotw2epwbt9l2s.webp"],
                    CreatedBy = "Admin",
                    UpdatedBy = "",
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    IsDeleted = false
                },
                new MaternityDressDetail
                {
                    Id = "4650cd4e5b1845d2b55dd3b062e52ef2",
                    MaternityDressId = "7202c22db49e4511b19661285cf363df",
                    SKU = "MD920-002",
                    Name = "Váy Maxi Bầu Halter Xếp Ly Màu Be",
                    Color = "Beige",
                    Size = "M",
                    Price = 5000,
                    Quantity = 999,
                    Image = ["https://res.cloudinary.com/dzykiyef5/image/upload/v1755665623/wsog1fugbnlicxawbvxu.webp"],
                    CreatedBy = "Admin",
                    UpdatedBy = "",
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    IsDeleted = false
                },
                new MaternityDressDetail
                {
                    Id = "271d58b4d79c415aa7a69142aa0fa98b",
                    MaternityDressId = "7202c22db49e4511b19661285cf363df",
                    SKU = "MD920-003",
                    Name = "Váy Maxi Bầu Halter Xếp Ly Màu Be",
                    Color = "Beige",
                    Size = "L",
                    Price = 5000,
                    Quantity = 999,
                    Image = ["https://res.cloudinary.com/dzykiyef5/image/upload/v1755665638/kfsazm8zrtftilznaxip.webp"],
                    CreatedBy = "Admin",
                    UpdatedBy = "",
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    IsDeleted = false
                },
                new MaternityDressDetail
                {
                    Id = "5945777a788e48bfa05fbfd0b37548f1",
                    MaternityDressId = "058811e7dd6842a59ea67452a301ce84",
                    SKU = "MD515-001",
                    Name = "Váy sơ mi bà bầu tay ngắn cài nút họa tiết màu ngà",
                    Color = "Ivory",
                    Size = "S",
                    Price = 10000,
                    Quantity = 999,
                    Image = ["https://res.cloudinary.com/dzykiyef5/image/upload/v1755666569/rioj80hsfwnwlanzw9et.webp"],
                    CreatedBy = "Admin",
                    UpdatedBy = "",
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    IsDeleted = false
                },
                new MaternityDressDetail
                {
                    Id = "450862bbfdb248f3897c70dc94431070",
                    MaternityDressId = "058811e7dd6842a59ea67452a301ce84",
                    SKU = "MD515-002",
                    Name = "Váy sơ mi bà bầu tay ngắn cài nút họa tiết màu ngà",
                    Color = "Ivory",
                    Size = "M",
                    Price = 10000,
                    Quantity = 999,
                    Image = ["https://res.cloudinary.com/dzykiyef5/image/upload/v1755666595/hjwo0qkax6imh5a2aeh1.webp"],
                    CreatedBy = "Admin",
                    UpdatedBy = "",
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    IsDeleted = false
                },
                new MaternityDressDetail
                {
                    Id = "9cd6a9f5d6ec4c79823401fd1aa0af3f",
                    MaternityDressId = "c5773c8eedba465fb0cfea9f8048cbca",
                    SKU = "MD585-001",
                    Name = "Váy sơ mi bà bầu họa tiết hoa đen thắt eo",
                    Color = "Black",
                    Size = "S",
                    Price = 5000,
                    Quantity = 999,
                    Image = ["https://res.cloudinary.com/dzykiyef5/image/upload/v1755666772/xwtlq28zrztferbprubt.webp"],
                    CreatedBy = "Admin",
                    UpdatedBy = "",
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    IsDeleted = false
                },
                new MaternityDressDetail
                {
                    Id = "a3dd93959fba4f0bbbab424df7bf5aba",
                    MaternityDressId = "c5773c8eedba465fb0cfea9f8048cbca",
                    SKU = "MD585-002",
                    Name = "Váy sơ mi bà bầu họa tiết hoa đen thắt eo",
                    Color = "Black",
                    Size = "L",
                    Price = 10000,
                    Quantity = 999,
                    Image = ["https://res.cloudinary.com/dzykiyef5/image/upload/v1755666772/xwtlq28zrztferbprubt.webp"],
                    CreatedBy = "Admin",
                    UpdatedBy = "",
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    IsDeleted = false
                },
                new MaternityDressDetail
                {
                    Id = "6267df0da52549af8386d32fb5d3cd17",
                    MaternityDressId = "058811e7dd6842a59ea67452a301ce84",
                    SKU = "MD515-003",
                    Name = "Váy sơ mi bà bầu tay ngắn cài nút họa tiết màu ngà",
                    Color = "Ivory",
                    Size = "L",
                    Price = 7000,
                    Quantity = 999,
                    Image = ["https://res.cloudinary.com/dzykiyef5/image/upload/v1755666595/hjwo0qkax6imh5a2aeh1.webp"],
                    CreatedBy = "Admin",
                    UpdatedBy = "",
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    IsDeleted = false
                },
                new MaternityDressDetail
                {
                    Id = "6fea930520964cf0969f9052bc88cd11",
                    MaternityDressId = "fdff22c67ad7425ebf40b2f457fc3228",
                    SKU = "MD220-001",
                    Name = "Váy Maxi hở vai bằng vải denim họa tiết chambray nhạt",
                    Color = "Light Blue",
                    Size = "S",
                    Price = 5000,
                    Quantity = 999,
                    Image = ["https://res.cloudinary.com/dzykiyef5/image/upload/v1755667003/juaqopirnx9hy9uk6fpe.webp"],
                    CreatedBy = "Admin",
                    UpdatedBy = "",
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    IsDeleted = false
                },
                new MaternityDressDetail
                {
                    Id = "be5e7b3b89fe4208bec2fdedf8911f13",
                    MaternityDressId = "fdff22c67ad7425ebf40b2f457fc3228",
                    SKU = "MD220-002",
                    Name = "Váy Maxi hở vai bằng vải denim họa tiết chambray nhạt",
                    Color = "Light Blue",
                    Size = "M",
                    Price = 6000,
                    Quantity = 999,
                    Image = ["https://res.cloudinary.com/dzykiyef5/image/upload/v1755667020/i2qjoezlgbf6n9m57sah.webp"],
                    CreatedBy = "Admin",
                    UpdatedBy = "",
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    IsDeleted = false
                },
                new MaternityDressDetail
                {
                    Id = "e75cfa596866407998267accece512e1",
                    MaternityDressId = "581aec917bea4d9d9054b8cf1d1cc7b8",
                    SKU = "MD846-001",
                    Name = "Váy midi bà bầu trễ vai nhún bèo màu đen",
                    Color = "Black",
                    Size = "S",
                    Price = 5000,
                    Quantity = 999,
                    Image = ["https://res.cloudinary.com/dzykiyef5/image/upload/v1755667122/mntbthkygph097khnszt.webp"],
                    CreatedBy = "Admin",
                    UpdatedBy = "",
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    IsDeleted = false
                },
                new MaternityDressDetail
                {
                    Id = "6ca47d478d104a4c89b677674fd4ae88",
                    MaternityDressId = "581aec917bea4d9d9054b8cf1d1cc7b8",
                    SKU = "MD846-002",
                    Name = "Váy midi bà bầu trễ vai nhún bèo màu đen",
                    Color = "Black",
                    Size = "M",
                    Price = 8000,
                    Quantity = 999,
                    Image = ["https://res.cloudinary.com/dzykiyef5/image/upload/v1755667143/hm4xhlkuxnfpqlo5n053.webp"],
                    CreatedBy = "Admin",
                    UpdatedBy = "",
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    IsDeleted = false
                },
                new MaternityDressDetail
                {
                    Id = "107a36d41c9e44b38f9c00f89fbd61df",
                    MaternityDressId = "581aec917bea4d9d9054b8cf1d1cc7b8",
                    SKU = "MD846-003",
                    Name = "Váy midi bà bầu trễ vai nhún bèo màu đen",
                    Color = "Black",
                    Size = "L",
                    Price = 10000,
                    Quantity = 999,
                    Image = ["https://res.cloudinary.com/dzykiyef5/image/upload/v1755667157/jxildxhcn9fqch6hjj9l.webp"],
                    CreatedBy = "Admin",
                    UpdatedBy = "",
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    IsDeleted = false
                },
                new MaternityDressDetail
                {
                    Id = "1f3a3d61760246f0aae05b43df794304",
                    MaternityDressId = "039fd8cb6da548299d31cd0b3780d528",
                    SKU = "MD128-001",
                    Name = "Áo dài bà bầu in họa tiết chắp vá nhiều màu xanh nhạt",
                    Color = "Light Blue",
                    Size = "XL",
                    Price = 30000,
                    Quantity = 999,
                    Image = ["https://res.cloudinary.com/dzykiyef5/image/upload/v1755667285/vtcwf6bzndx8xfqntyft.webp"],
                    CreatedBy = "System",
                    UpdatedBy = "",
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    IsDeleted = false
                },
                new MaternityDressDetail
                {
                    Id = "8d8c3c7000d845fc9f70bd7487f780fe",
                    MaternityDressId = "039fd8cb6da548299d31cd0b3780d528",
                    SKU = "MD128-002",
                    Name = "Áo dài bà bầu in họa tiết chắp vá nhiều màu xanh nhạt",
                    Color = "Light Blue",
                    Size = "2XL",
                    Price = 40000,
                    Quantity = 999,
                    Image = ["https://res.cloudinary.com/dzykiyef5/image/upload/v1755667299/ameel0u31pqfmhjlwwvz.webp"],
                    CreatedBy = "System",
                    UpdatedBy = "",
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    IsDeleted = false
                }
            );

            #endregion

            #region Seed Order, OrderItem, Add_On, DesignRequest

            modelBuilder.Entity<DesignRequest>().HasData(
                new DesignRequest
                {
                    Id = "163bdbd2e4634e63a2be9b74f7c783b5",
                    UserId = "f49aa51bbd304e77933e24bbed65b165",
                    OrderItemId = "6390cef2d8644fa791480f1e3b7a81e2",
                    Description = "cho tui xin 50k nhe",
                    Images =
                    [
                        "https://firebasestorage.googleapis.com/v0/b/mamafit-e0138.firebasestorage.app/o/design-requests%2F1000028470.jpg?alt=media&token=02876007-5a09-42f1-9c99-e3a4b422ed0b",
                        "https://firebasestorage.googleapis.com/v0/b/mamafit-e0138.firebasestorage.app/o/design-requests%2F59d9afea-509e-4461-b70a-a01d8b6f97a4-1_all_13976.png?alt=media&token=b31b67e8-badd-4bae-b028-2901f47910fa"
                    ],
                    CreatedBy = "user",
                    UpdatedBy = null,
                    CreatedAt = new DateTime(2025, 8, 3, 6, 19, 30, DateTimeKind.Utc),
                    UpdatedAt = new DateTime(2025, 8, 3, 6, 19, 30, DateTimeKind.Utc),
                    IsDeleted = false
                },
                new DesignRequest
                {
                    Id = "6f16cc921d924b38bfb1fdeba3a92a02",
                    UserId = "f49aa51bbd304e77933e24bbed65b165",
                    OrderItemId = "583f8d8a545a4eb19d3257433f84de2d",
                    Description = "thiet ke cho tui 1 cai vay that dep",
                    Images =
                    [
                        "https://firebasestorage.googleapis.com/v0/b/mamafit-e0138.firebasestorage.app/o/design-requests%2F1000026857.jpg?alt=media&token=698e129c-6843-48ed-83f3-759664aa5f15"
                    ],
                    CreatedBy = "user",
                    UpdatedBy = null,
                    CreatedAt = new DateTime(2025, 8, 3, 6, 31, 42, DateTimeKind.Utc),
                    UpdatedAt = new DateTime(2025, 8, 3, 6, 31, 42, DateTimeKind.Utc),
                    IsDeleted = false
                }
            );

            modelBuilder.Entity<Order>().HasData(
                new Order
                {
                    Id = "4951106889514ce2ada27665d3e41a43",
                    AddressId = null,
                    BranchId = null,
                    UserId = "f49aa51bbd304e77933e24bbed65b165",
                    VoucherDiscountId = null,
                    MeasurementId = null,
                    IsOnline = true,
                    Type = OrderType.DESIGN,
                    Code = "ORD3216355",
                    TrackingOrderCode = null,
                    Status = 0,
                    TotalAmount = 3000.0m,
                    ShippingFee = 0,
                    ServiceAmount = null,
                    DiscountSubtotal = null,
                    DepositSubtotal = null,
                    RemainingBalance = null,
                    TotalPaid = null,
                    PaymentStatus = 0,
                    PaymentMethod = 0,
                    DeliveryMethod = 0,
                    PaymentType = PaymentType.FULL,
                    CanceledAt = null,
                    CanceledReason = null,
                    SubTotalAmount = 3000.0m,
                    WarrantyCode = null,
                    ReceivedAt = null,
                    ReceivedAtBranch = null,
                    CreatedBy = "User",
                    UpdatedBy = null,
                    CreatedAt = new DateTime(2025, 8, 3, 6, 19, 30, DateTimeKind.Utc),
                    UpdatedAt = new DateTime(2025, 8, 3, 6, 19, 30, DateTimeKind.Utc),
                    IsDeleted = false
                },
                new Order
                {
                    Id = "b2643d4d3f854ad9926b3be5e63e4914",
                    AddressId = "7aa93cf1dcea43e68da114d7c991a732",
                    BranchId = null,
                    UserId = "f49aa51bbd304e77933e24bbed65b165",
                    VoucherDiscountId = null,
                    MeasurementId = "82e18e44f0984ad08209e575f77f3ca0",
                    IsOnline = true,
                    Type = 0,
                    Code = "ORD2983710",
                    TrackingOrderCode = null,
                    Status = 0,
                    TotalAmount = 30250m,
                    ShippingFee = 22000m,
                    ServiceAmount = 4250m,
                    DiscountSubtotal = 0m,
                    DepositSubtotal = 2000m,
                    RemainingBalance = 2000m,
                    TotalPaid = 28250m,
                    PaymentStatus = 0,
                    PaymentMethod = PaymentMethod.ONLINE_BANKING,
                    DeliveryMethod = DeliveryMethod.DELIVERY,
                    PaymentType = 0,
                    CanceledAt = null,
                    CanceledReason = null,
                    SubTotalAmount = 4000m,
                    WarrantyCode = null,
                    ReceivedAt = null,
                    ReceivedAtBranch = null,
                    CreatedBy = "User",
                    UpdatedBy = "System",
                    CreatedAt = new DateTime(2025, 8, 3, 6, 28, 4, DateTimeKind.Utc),
                    UpdatedAt = new DateTime(2025, 8, 3, 6, 28, 4, DateTimeKind.Utc),
                    IsDeleted = false
                },
                new Order
                {
                    Id = "d7ba2e103448473f9949c10a59243e62",
                    AddressId = null,
                    BranchId = "b536f146c8f04bbc9949fc70a31fda8f",
                    UserId = "f49aa51bbd304e77933e24bbed65b165",
                    VoucherDiscountId = null,
                    MeasurementId = "c49743c2e5d746239bddd1ade3d76ae9",
                    IsOnline = true,
                    Type = 0,
                    Code = "ORD3900133",
                    TrackingOrderCode = null,
                    Status = 0,
                    TotalAmount = 27150.0m,
                    ShippingFee = 22000,
                    ServiceAmount = 3150.0m,
                    DiscountSubtotal = 0,
                    DepositSubtotal = null,
                    RemainingBalance = null,
                    TotalPaid = 27150.0m,
                    PaymentStatus = 0,
                    PaymentMethod = PaymentMethod.ONLINE_BANKING,
                    DeliveryMethod = 0,
                    PaymentType = PaymentType.FULL,
                    CanceledAt = null,
                    CanceledReason = null,
                    SubTotalAmount = 2000.0m,
                    WarrantyCode = null,
                    ReceivedAt = null,
                    ReceivedAtBranch = null,
                    CreatedBy = "User",
                    UpdatedBy = null,
                    CreatedAt = new DateTime(2025, 8, 3, 6, 28, 4, DateTimeKind.Utc),
                    UpdatedAt = new DateTime(2025, 8, 3, 6, 28, 4, DateTimeKind.Utc),
                    IsDeleted = false
                },
                new Order
                {
                    Id = "fffdd078a46040e3bf27839793bb2bed",
                    AddressId = "7aa93cf1dcea43e68da114d7c991a732",
                    BranchId = null,
                    UserId = "f49aa51bbd304e77933e24bbed65b165",
                    VoucherDiscountId = null,
                    MeasurementId = "82e18e44f0984ad08209e575f77f3ca0",
                    IsOnline = true,
                    Type = 0,
                    Code = "ORD6500230",
                    TrackingOrderCode = null,
                    Status = 0,
                    TotalAmount = 25850.0m,
                    ShippingFee = 22000,
                    ServiceAmount = 1850.0m,
                    DiscountSubtotal = 0,
                    DepositSubtotal = 1000.0m,
                    RemainingBalance = 1000.0m,
                    TotalPaid = 24850.0m,
                    PaymentStatus = 0,
                    PaymentMethod = PaymentMethod.ONLINE_BANKING,
                    DeliveryMethod = DeliveryMethod.DELIVERY,
                    PaymentType = 0,
                    CanceledAt = null,
                    CanceledReason = null,
                    SubTotalAmount = 2000.0m,
                    WarrantyCode = null,
                    ReceivedAt = null,
                    ReceivedAtBranch = null,
                    CreatedBy = "User",
                    UpdatedBy = null,
                    CreatedAt = new DateTime(2025, 8, 3, 6, 29, 40, DateTimeKind.Utc),
                    UpdatedAt = new DateTime(2025, 8, 3, 6, 29, 40, DateTimeKind.Utc),
                    IsDeleted = false
                },
                new Order
                {
                    Id = "b1dec711c96840a3b45c14ed8b40d6ba",
                    AddressId = "7aa93cf1dcea43e68da114d7c991a732",
                    BranchId = null,
                    UserId = "f49aa51bbd304e77933e24bbed65b165",
                    VoucherDiscountId = null,
                    MeasurementId = "c49743c2e5d746239bddd1ade3d76ae9",
                    IsOnline = true,
                    Type = 0,
                    Code = "ORD3021123",
                    TrackingOrderCode = null,
                    Status = 0,
                    TotalAmount = 24000.0m,
                    ShippingFee = 22000,
                    ServiceAmount = 0,
                    DiscountSubtotal = 0,
                    DepositSubtotal = null,
                    RemainingBalance = null,
                    TotalPaid = 24000.0m,
                    PaymentStatus = 0,
                    PaymentMethod = PaymentMethod.ONLINE_BANKING,
                    DeliveryMethod = DeliveryMethod.DELIVERY,
                    PaymentType = PaymentType.FULL,
                    CanceledAt = null,
                    CanceledReason = null,
                    SubTotalAmount = 2000.0m,
                    WarrantyCode = null,
                    ReceivedAt = null,
                    ReceivedAtBranch = null,
                    CreatedBy = "User",
                    UpdatedBy = null,
                    CreatedAt = new DateTime(2025, 8, 3, 6, 30, 48, DateTimeKind.Utc),
                    UpdatedAt = new DateTime(2025, 8, 3, 6, 30, 48, DateTimeKind.Utc),
                    IsDeleted = false
                },
                new Order
                {
                    Id = "74dede079f594b58b119a7edf9c21c01",
                    AddressId = null,
                    BranchId = null,
                    UserId = "f49aa51bbd304e77933e24bbed65b165",
                    VoucherDiscountId = null,
                    MeasurementId = null,
                    IsOnline = true,
                    Type = OrderType.DESIGN,
                    Code = "ORD1354972",
                    TrackingOrderCode = null,
                    Status = 0,
                    TotalAmount = 3000.0m,
                    ShippingFee = 0,
                    ServiceAmount = null,
                    DiscountSubtotal = null,
                    DepositSubtotal = null,
                    RemainingBalance = null,
                    TotalPaid = null,
                    PaymentStatus = 0,
                    PaymentMethod = 0,
                    DeliveryMethod = 0,
                    PaymentType = PaymentType.FULL,
                    CanceledAt = null,
                    CanceledReason = null,
                    SubTotalAmount = 3000.0m,
                    WarrantyCode = null,
                    ReceivedAt = null,
                    ReceivedAtBranch = null,
                    CreatedBy = "User",
                    UpdatedBy = null,
                    CreatedAt = new DateTime(2025, 8, 3, 6, 31, 42, DateTimeKind.Utc),
                    UpdatedAt = new DateTime(2025, 8, 3, 6, 31, 42, DateTimeKind.Utc),
                    IsDeleted = false
                },
                new Order
                {
                    Id = "4e0842a05e594525a25de04b1e79f4a8",
                    AddressId = "7aa93cf1dcea43e68da114d7c991a732",
                    BranchId = null,
                    UserId = "f49aa51bbd304e77933e24bbed65b165",
                    VoucherDiscountId = null,
                    MeasurementId = "c49743c2e5d746239bddd1ade3d76ae9",
                    IsOnline = true,
                    Type = 0,
                    Code = "ORD9498301",
                    TrackingOrderCode = null,
                    Status = 0,
                    TotalAmount = 27950.0m,
                    ShippingFee = 22000,
                    ServiceAmount = 1950,
                    DiscountSubtotal = 0,
                    DepositSubtotal = null,
                    RemainingBalance = null,
                    TotalPaid = 27950.0m,
                    PaymentStatus = 0,
                    PaymentMethod = 0,
                    DeliveryMethod = DeliveryMethod.DELIVERY,
                    PaymentType = PaymentType.FULL,
                    CanceledAt = null,
                    CanceledReason = null,
                    SubTotalAmount = 4000.0m,
                    WarrantyCode = null,
                    ReceivedAt = null,
                    ReceivedAtBranch = null,
                    CreatedBy = "User",
                    UpdatedBy = "User",
                    CreatedAt = new DateTime(2025, 8, 9, 10, 3, 21, DateTimeKind.Utc),
                    UpdatedAt = new DateTime(2025, 8, 9, 10, 4, 19, DateTimeKind.Utc),
                    IsDeleted = false
                }
            );


            modelBuilder.Entity<OrderItem>().HasData(
                new OrderItem
                {
                    Id = "6390cef2d8644fa791480f1e3b7a81e2",
                    OrderId = "4951106889514ce2ada27665d3e41a43",
                    ParentOrderItemId = null,
                    MaternityDressDetailId = null,
                    PresetId = null,
                    ItemType = ItemType.DESIGN_REQUEST,
                    Price = 3000.0m,
                    Quantity = 1,
                    WarrantyDate = null,
                    CreatedBy = "user",
                    UpdatedBy = null,
                    CreatedAt = new DateTime(2025, 8, 3, 6, 19, 30, DateTimeKind.Utc),
                    UpdatedAt = new DateTime(2025, 8, 3, 6, 19, 30, DateTimeKind.Utc),
                    IsDeleted = false
                },
                new OrderItem
                {
                    Id = "b9d43051de4f471aa7228bfc5a51af68",
                    OrderId = "b2643d4d3f854ad9926b3be5e63e4914",
                    ParentOrderItemId = null,
                    MaternityDressDetailId = null,
                    PresetId = "fe88b53dce4c4938a7785fb9e3db1f0a",
                    ItemType = ItemType.PRESET,
                    Price = 2000m,
                    Quantity = 1,
                    WarrantyDate = null,
                    CreatedBy = "user",
                    UpdatedBy = "System",
                    CreatedAt = new DateTime(2025, 8, 3, 6, 28, 4, DateTimeKind.Utc),
                    UpdatedAt = new DateTime(2025, 8, 3, 6, 28, 4, DateTimeKind.Utc),
                    IsDeleted = false
                },
                new OrderItem
                {
                    Id = "ebd9a5edae214c4d8f770c29b7aa8b7e",
                    OrderId = "b2643d4d3f854ad9926b3be5e63e4914",
                    ParentOrderItemId = null,
                    MaternityDressDetailId = null,
                    PresetId = "fe7043a37995453db2d03741e81da9df",
                    ItemType = ItemType.PRESET,
                    Price = 2000m,
                    Quantity = 1,
                    WarrantyDate = null,
                    CreatedBy = "user",
                    UpdatedBy = "System",
                    CreatedAt = new DateTime(2025, 8, 3, 6, 28, 4, DateTimeKind.Utc),
                    UpdatedAt = new DateTime(2025, 8, 3, 6, 28, 4, DateTimeKind.Utc),
                    IsDeleted = false
                },
                new OrderItem
                {
                    Id = "71f71d08ac434f1683ae64292fd7c5a5",
                    OrderId = "4e0842a05e594525a25de04b1e79f4a8",
                    ParentOrderItemId = null,
                    MaternityDressDetailId = null,
                    PresetId = "bcdcf89795b2484f96a3a12b25383a12",
                    ItemType = (ItemType)1,
                    Price = 2000m,
                    Quantity = 1,
                    WarrantyDate = null,
                    CreatedBy = "user",
                    UpdatedBy = "User",
                    CreatedAt = new DateTime(2025, 8, 9, 10, 3, 21, 624, DateTimeKind.Utc),
                    UpdatedAt = new DateTime(2025, 8, 9, 10, 7, 38, 212, DateTimeKind.Utc),
                    IsDeleted = false
                },
                new OrderItem
                {
                    Id = "2f0f0502c88f4723a3c6706989bb4b57",
                    OrderId = "4e0842a05e594525a25de04b1e79f4a8",
                    ParentOrderItemId = null,
                    MaternityDressDetailId = null,
                    PresetId = "fe88b53dce4c4938a7785fb9e3db1f0a",
                    ItemType = (ItemType)1,
                    Price = 2000m,
                    Quantity = 1,
                    WarrantyDate = null,
                    CreatedBy = "user",
                    UpdatedBy = "User",
                    CreatedAt = new DateTime(2025, 8, 9, 10, 3, 21, 640, DateTimeKind.Utc),
                    UpdatedAt = new DateTime(2025, 8, 9, 10, 22, 31, 529, DateTimeKind.Utc),
                    IsDeleted = false
                },
                new OrderItem
                {
                    Id = "5c599ac9632447ff814ffcc127910e01",
                    OrderId = "d7ba2e103448473f9949c10a59243e62",
                    ParentOrderItemId = null,
                    MaternityDressDetailId = null,
                    PresetId = "68fca8f086454898ae81b1ea29890b60",
                    ItemType = ItemType.PRESET,
                    Price = 2000.0m,
                    Quantity = 1,
                    WarrantyDate = null,
                    CreatedBy = "user",
                    UpdatedBy = null,
                    CreatedAt = new DateTime(2025, 8, 3, 6, 28, 4, DateTimeKind.Utc),
                    UpdatedAt = new DateTime(2025, 8, 3, 6, 28, 4, DateTimeKind.Utc),
                    IsDeleted = false
                },
                new OrderItem
                {
                    Id = "d0bca4f5b523409d9a8f6c245b2cc42f",
                    OrderId = "fffdd078a46040e3bf27839793bb2bed",
                    ParentOrderItemId = null,
                    MaternityDressDetailId = null,
                    PresetId = "33c8177630a54906b92c0b8c03c57450",
                    ItemType = ItemType.PRESET,
                    Price = 2000.0m,
                    Quantity = 1,
                    WarrantyDate = null,
                    CreatedBy = "user",
                    UpdatedBy = null,
                    CreatedAt = new DateTime(2025, 8, 3, 6, 29, 40, DateTimeKind.Utc),
                    UpdatedAt = new DateTime(2025, 8, 3, 6, 29, 40, DateTimeKind.Utc),
                    IsDeleted = false
                },
                new OrderItem
                {
                    Id = "0c70b067e66a4ac0a9cf7d61a72ec887",
                    OrderId = "b1dec711c96840a3b45c14ed8b40d6ba",
                    ParentOrderItemId = null,
                    MaternityDressDetailId = null,
                    PresetId = "fe88b53dce4c4938a7785fb9e3db1f0a",
                    ItemType = ItemType.PRESET,
                    Price = 2000.0m,
                    Quantity = 1,
                    WarrantyDate = null,
                    CreatedBy = "user",
                    UpdatedBy = null,
                    CreatedAt = new DateTime(2025, 8, 3, 6, 30, 48, DateTimeKind.Utc),
                    UpdatedAt = new DateTime(2025, 8, 3, 6, 30, 48, DateTimeKind.Utc),
                    IsDeleted = false
                },
                new OrderItem
                {
                    Id = "583f8d8a545a4eb19d3257433f84de2d",
                    OrderId = "74dede079f594b58b119a7edf9c21c01",
                    ParentOrderItemId = null,
                    MaternityDressDetailId = null,
                    PresetId = null,
                    ItemType = ItemType.DESIGN_REQUEST,
                    Price = 3000.0m,
                    Quantity = 1,
                    WarrantyDate = null,
                    CreatedBy = "user",
                    UpdatedBy = null,
                    CreatedAt = new DateTime(2025, 8, 3, 6, 31, 42, DateTimeKind.Utc),
                    UpdatedAt = new DateTime(2025, 8, 3, 6, 31, 42, DateTimeKind.Utc),
                    IsDeleted = false
                });

            modelBuilder.Entity<OrderItemAddOnOption>().HasData(
                new OrderItemAddOnOption
                {
                    OrderItemId = "5c599ac9632447ff814ffcc127910e01",
                    AddOnOptionId = "0528ef2e60164a3aa4ee078e25e982b2",
                    Value = ""
                },
                new OrderItemAddOnOption
                {
                    OrderItemId = "b9d43051de4f471aa7228bfc5a51af68",
                    AddOnOptionId = "c289f97217504ea4a0a16b7079cf5f2a",
                    Value = ""
                },
                new OrderItemAddOnOption
                {
                    OrderItemId = "5c599ac9632447ff814ffcc127910e01",
                    AddOnOptionId = "811a8841037a43e0adf5b0bfd7630bad",
                    Value = "MamaFit la so 1"
                },
                new OrderItemAddOnOption
                {
                    OrderItemId = "d0bca4f5b523409d9a8f6c245b2cc42f",
                    AddOnOptionId = "3c7f15f87fc34c3f9448d52a2e98f3ed",
                    Value =
                        "https://firebasestorage.googleapis.com/v0/b/mamafit-e0138.firebasestorage.app/o/add-ons%2F1000029294.png?alt=media&token=b97c8980-cfa7-4988-a335-30c4b5512c29"
                },
                new OrderItemAddOnOption
                {
                    OrderItemId = "d0bca4f5b523409d9a8f6c245b2cc42f",
                    AddOnOptionId = "71655b636da34aa391d8691180ca763b",
                    Value = "Xin chao MamaFit"
                }
            );

            #endregion
        }
    }
}