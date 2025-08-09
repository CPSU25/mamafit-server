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
                    Id = "aw9aa51bbd304e77933e24bbed65cd19",
                    UserName = "branchquan9",
                    UserEmail = "branchquan9@mamafit.com",
                    HashPassword = "u+7jaa75MJns9ST/6FSw4aQU+zSF/iNldvzpJxBFzwk=",
                    Salt = "R0kox04KXBTPFFlKjfxIzNeMzlbH1rGO/YJDtl0N894=",
                    PhoneNumber = "0909648632",
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
                    FullName = "Manager",
                    IsVerify = true,
                    RoleId = "a3cb88edaf2b4718a9986010c5b9c1d7",
                },
                new ApplicationUser
                {
                    Id = "ce5235c40924fd5b0792732d3fb1b6f",
                    UserName = "staff",
                    UserEmail = "staff@mamafit.com",
                    HashPassword = "u+7jaa75MJns9ST/6FSw4aQU+zSF/iNldvzpJxBFzwk=",
                    Salt = "R0kox04KXBTPFFlKjfxIzNeMzlbH1rGO/YJDtl0N894=",
                    PhoneNumber = "0909219060",
                    FullName = "Staff",
                    IsVerify = true,
                    RoleId = "5ed8cfa9b62d433c88ab097b6d2baccd",
                },
                new ApplicationUser
                {
                    Id = "eb019fbe31e6449b9b92c89b5c893b03",
                    UserName = "branchstaff",
                    UserEmail = "branchstaff@mamafit.com",
                    HashPassword = "u+7jaa75MJns9ST/6FSw4aQU+zSF/iNldvzpJxBFzwk=",
                    Salt = "R0kox04KXBTPFFlKjfxIzNeMzlbH1rGO/YJDtl0N894=",
                    PhoneNumber = "0909370428",
                    FullName = "Branch Staff",
                    IsVerify = true,
                    RoleId = "c9118b99c0ad486dbb18560a916b630c",
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
                    IsVerify = true,
                    RoleId = "bf081015e17a41b8b1cae65b1b17cfdb",
                }
            );

            #region Seed Category, Style, and Component

            var now = DateTime.UtcNow;
            var createdBy = "System";

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
                    Name = "Party/Special",
                    Description = "Dành cho buổi tiệc hoặc dịp đặc biệt.",
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
                },
                new Category
                {
                    Id = "c6a7e90156d44da1ab26f259313e4a0b",
                    Name = "Formal/Event",
                    Description = "Phù hợp với các sự kiện trang trọng.",
                    Images =
                    [
                        "https://firebasestorage.googleapis.com/v0/b/mamafit-e0138.firebasestorage.app/o/components%2Fformal-event.png?alt=media&token=4faa26e0-3d1e-402f-8bbe-e59360a1bc76"
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
                    Id = "037b41302a4c453f8cef135afceb3956",
                    CategoryId = "a7a75e41c1a64b4498a81f4b76029a5a",
                    Name = "Maxi",
                    IsCustom = false,
                    Description = "Đầm dài maxi, thích hợp đi biển hoặc dạo phố.",
                    Images =
                    [
                        "https://firebasestorage.googleapis.com/v0/b/mamafit-e0138.firebasestorage.app/o/components%2Fmaxi-sketch.png?alt=media&token=b595c2a0-da30-45a4-afbd-31ec1b5dfc28"
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
                    Id = "a9d4e572c3b24f8e9d2f671b5e1a8c07",
                    CategoryId = "a7a75e41c1a64b4498a81f4b76029a5a",
                    Name = "Kaftan",
                    IsCustom = false,
                    Description = "Đầm kaftan rộng rãi, nhẹ, thoáng mát, phù hợp đi biển và mùa hè.",
                    Images =
                    [
                        "https://firebasestorage.googleapis.com/v0/b/mamafit-e0138.firebasestorage.app/o/components%2Fkaftan-sketch.png?alt=media&token=6ba8fcf7-ea28-4576-b4ca-8bf0da0e80ef"
                    ],
                    GlobalStatus = 0,
                    CreatedBy = "System",
                    UpdatedBy = "System",
                    CreatedAt = new DateTime(2025, 8, 4, 14, 5, 3, DateTimeKind.Utc),
                    UpdatedAt = new DateTime(2025, 8, 4, 14, 5, 3, DateTimeKind.Utc),
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

            // ===== COMPONENT OPTIONS =====
            modelBuilder.Entity<ComponentOption>().HasData(
                new ComponentOption
                {
                    Id = "66645e2f8fa94d228d87450cc4a74706",
                    ComponentId = "0ec808c3046b4d368962c65eb771dea0",
                    Name = "Green",
                    Price = 10000.0m,
                    Description = "Xanh ngọc quý phái, nổi bật.",
                    Images =
                    [
                        "https://firebasestorage.googleapis.com/v0/b/mamafit-e0138.firebasestorage.app/o/components%2Femerald-green.png?alt=media&token=d63ad4cc-0037-4ca5-b71b-4d8eb394b6ba"
                    ],
                    GlobalStatus = 0,
                    CreatedBy = "System",
                    UpdatedBy = "System",
                    CreatedAt = new DateTime(2025, 8, 4, 14, 5, 3, DateTimeKind.Utc),
                    UpdatedAt = new DateTime(2025, 8, 4, 14, 5, 3, DateTimeKind.Utc),
                    IsDeleted = false
                },
                new ComponentOption
                {
                    Id = "31c853d3dde94e68ab57903cf8a80798",
                    ComponentId = "c615996abfe24d3885ccea361a199dba",
                    Name = "Cotton",
                    Price = 9000.0m,
                    Description = "Vải cotton thoáng mát, thấm hút tốt.",
                    Images =
                    [
                        "https://firebasestorage.googleapis.com/v0/b/mamafit-e0138.firebasestorage.app/o/components%2Fcotton.png?alt=media&token=bdd471d7-c695-4c05-8770-ffb8eeb0f03e"
                    ],
                    GlobalStatus = 0,
                    CreatedBy = "System",
                    UpdatedBy = "System",
                    CreatedAt = new DateTime(2025, 8, 4, 14, 5, 3, DateTimeKind.Utc),
                    UpdatedAt = new DateTime(2025, 8, 4, 14, 5, 3, DateTimeKind.Utc),
                    IsDeleted = false
                },
                new ComponentOption
                {
                    Id = "4ed7cdc105ab44de81d333d63db0a222",
                    ComponentId = "3b83e5e47b444880aa9495b58b38a2fd",
                    Name = "No Sleeves",
                    Price = 10000.0m,
                    Description = "Không tay, thoáng mát.",
                    Images =
                    [
                        "https://firebasestorage.googleapis.com/v0/b/mamafit-e0138.firebasestorage.app/o/components%2Fno-sleeves.png?alt=media&token=222c148c-22b0-407d-acf9-03b91a793123"
                    ],
                    GlobalStatus = 0,
                    CreatedBy = "System",
                    UpdatedBy = "System",
                    CreatedAt = new DateTime(2025, 8, 4, 14, 5, 3, DateTimeKind.Utc),
                    UpdatedAt = new DateTime(2025, 8, 4, 14, 5, 3, DateTimeKind.Utc),
                    IsDeleted = false
                },
                new ComponentOption
                {
                    Id = "92bdbbdbfd3b4dcf8256b68a66dfa35b",
                    ComponentId = "b66ab857cd93410599bd14b79ae37147",
                    Name = "Round",
                    Price = 20000.0m,
                    Description = "Cổ tròn cơ bản, thanh lịch.",
                    Images =
                    [
                        "https://firebasestorage.googleapis.com/v0/b/mamafit-e0138.firebasestorage.app/o/components%2Fround-neck.png?alt=media&token=1f8e06b6-1aa5-49fc-a05a-e88e29593710"
                    ],
                    GlobalStatus = 0,
                    CreatedBy = "System",
                    UpdatedBy = "System",
                    CreatedAt = new DateTime(2025, 8, 4, 14, 5, 3, DateTimeKind.Utc),
                    UpdatedAt = new DateTime(2025, 8, 4, 14, 5, 3, DateTimeKind.Utc),
                    IsDeleted = false
                },
                new ComponentOption
                {
                    Id = "e2c03da33b12458694a1b01c3ec66474",
                    ComponentId = "0ec808c3046b4d368962c65eb771dea0",
                    Name = "Navy",
                    Price = 10000.0m,
                    Description = "Xanh navy lịch sự, thanh nhã.",
                    Images =
                    [
                        "https://firebasestorage.googleapis.com/v0/b/mamafit-e0138.firebasestorage.app/o/components%2Fnavy-blue.png?alt=media&token=94e2c88c-a38c-4294-8b35-027b255e26ca"
                    ],
                    GlobalStatus = 0,
                    CreatedBy = "System",
                    UpdatedBy = "System",
                    CreatedAt = new DateTime(2025, 8, 4, 14, 5, 3, DateTimeKind.Utc),
                    UpdatedAt = new DateTime(2025, 8, 4, 14, 5, 3, DateTimeKind.Utc),
                    IsDeleted = false
                },
                new ComponentOption
                {
                    Id = "8158b8e5ef0340bdaf830fa8eb2e650d",
                    ComponentId = "3e9f3d9beeff47f1977a6713cffe781e",
                    Name = "Empire Waist",
                    Price = 20000.0m,
                    Description = "Eo cao, phù hợp cho phong cách nữ tính.",
                    Images =
                    [
                        "https://firebasestorage.googleapis.com/v0/b/mamafit-e0138.firebasestorage.app/o/components%2Fempire-waist.png?alt=media&token=292233b5-38d3-4b46-b543-6747d187a702"
                    ],
                    GlobalStatus = 0,
                    CreatedBy = "System",
                    UpdatedBy = "System",
                    CreatedAt = new DateTime(2025, 8, 4, 14, 5, 3, DateTimeKind.Utc),
                    UpdatedAt = new DateTime(2025, 8, 4, 14, 5, 3, DateTimeKind.Utc),
                    IsDeleted = false
                },
                new ComponentOption
                {
                    Id = "818f875df04a49eebe9c474bba4247b9",
                    ComponentId = "b66ab857cd93410599bd14b79ae37147",
                    Name = "V Neck",
                    Price = 25000.0m,
                    Description = "Cổ chữ V tạo cảm giác cổ dài hơn.",
                    Images =
                    [
                        "https://firebasestorage.googleapis.com/v0/b/mamafit-e0138.firebasestorage.app/o/components%2Fv-neck.png?alt=media&token=5418fe58-896e-4559-86ae-e2809c519963"
                    ],
                    GlobalStatus = 0,
                    CreatedBy = "System",
                    UpdatedBy = "System",
                    CreatedAt = new DateTime(2025, 8, 4, 14, 5, 3, DateTimeKind.Utc),
                    UpdatedAt = new DateTime(2025, 8, 4, 14, 5, 3, DateTimeKind.Utc),
                    IsDeleted = false
                },
                new ComponentOption
                {
                    Id = "81acc249e2664c72908014a28e1e72ef",
                    ComponentId = "3e9f3d9beeff47f1977a6713cffe781e",
                    Name = "Bow Tie",
                    Price = 22000.0m,
                    Description = "Nơ thắt eo tạo điểm nhấn đáng yêu.",
                    Images =
                    [
                        "https://firebasestorage.googleapis.com/v0/b/mamafit-e0138.firebasestorage.app/o/components%2Fbow-tie-waist.png?alt=media&token=43522a3e-848a-49d1-8e5a-a6610e619c21"
                    ],
                    GlobalStatus = 0,
                    CreatedBy = "System",
                    UpdatedBy = "System",
                    CreatedAt = new DateTime(2025, 8, 4, 14, 5, 3, DateTimeKind.Utc),
                    UpdatedAt = new DateTime(2025, 8, 4, 14, 5, 3, DateTimeKind.Utc),
                    IsDeleted = false
                },
                new ComponentOption
                {
                    Id = "fecd26fe3b6b4e88ba97ebb406fa6df2",
                    ComponentId = "3b83e5e47b444880aa9495b58b38a2fd",
                    Name = "Flattered",
                    Price = 15000.0m,
                    Description = "Tay ngắn bay bổng, thoải mái.",
                    Images =
                    [
                        "https://firebasestorage.googleapis.com/v0/b/mamafit-e0138.firebasestorage.app/o/components%2Fflattered-sleeves.png?alt=media&token=93a07830-1514-4a65-80ca-599c67cd7334"
                    ],
                    GlobalStatus = 0,
                    CreatedBy = "System",
                    UpdatedBy = "System",
                    CreatedAt = new DateTime(2025, 8, 4, 14, 5, 3, DateTimeKind.Utc),
                    UpdatedAt = new DateTime(2025, 8, 4, 14, 5, 3, DateTimeKind.Utc),
                    IsDeleted = false
                },
                new ComponentOption
                {
                    Id = "c454b97a93f341e78b127d5425acc464",
                    ComponentId = "0ec808c3046b4d368962c65eb771dea0",
                    Name = "Red",
                    Price = 10000.0m,
                    Description = "Màu đỏ nổi bật, quyến rũ.",
                    Images =
                    [
                        "https://firebasestorage.googleapis.com/v0/b/mamafit-e0138.firebasestorage.app/o/components%2Fred.png?alt=media&token=b520d1e1-69b9-48bd-aa84-d3824897c0b8"
                    ],
                    GlobalStatus = 0,
                    CreatedBy = "System",
                    UpdatedBy = "System",
                    CreatedAt = new DateTime(2025, 8, 4, 14, 5, 3, DateTimeKind.Utc),
                    UpdatedAt = new DateTime(2025, 8, 4, 14, 5, 3, DateTimeKind.Utc),
                    IsDeleted = false
                },
                new ComponentOption
                {
                    Id = "06e6899bd6544dd3b9020a50789d9421",
                    ComponentId = "3e9f3d9beeff47f1977a6713cffe781e",
                    Name = "Normal",
                    Price = 18000.0m,
                    Description = "Thiết kế eo trung bình truyền thống.",
                    Images =
                    [
                        "https://firebasestorage.googleapis.com/v0/b/mamafit-e0138.firebasestorage.app/o/components%2Fnormal-waist.png?alt=media&token=6b608665-6975-4993-abd7-c57106c913f8"
                    ],
                    GlobalStatus = 0,
                    CreatedBy = "System",
                    UpdatedBy = "System",
                    CreatedAt = new DateTime(2025, 8, 4, 14, 5, 3, DateTimeKind.Utc),
                    UpdatedAt = new DateTime(2025, 8, 4, 14, 5, 3, DateTimeKind.Utc),
                    IsDeleted = false
                },
                new ComponentOption
                {
                    Id = "70b69ec5ea074c0f8bf904b20b9c7884",
                    ComponentId = "f00fcaf79a86427c995daa1fd0915d8e",
                    Name = "High-Low",
                    Price = 16000.0m,
                    Description = "Trước ngắn sau dài tạo nét lạ mắt.",
                    Images =
                    [
                        "https://firebasestorage.googleapis.com/v0/b/mamafit-e0138.firebasestorage.app/o/components%2Fhigh-low-hem.png?alt=media&token=258a7314-654c-4a9a-8af7-90a5b23597e3"
                    ],
                    GlobalStatus = 0,
                    CreatedBy = "System",
                    UpdatedBy = "System",
                    CreatedAt = new DateTime(2025, 8, 4, 14, 5, 3, DateTimeKind.Utc),
                    UpdatedAt = new DateTime(2025, 8, 4, 14, 5, 3, DateTimeKind.Utc),
                    IsDeleted = false
                },
                new ComponentOption
                {
                    Id = "78b78938aefe4bc6b927ed3722bc5859",
                    ComponentId = "3b83e5e47b444880aa9495b58b38a2fd",
                    Name = "Strap",
                    Price = 12000.0m,
                    Description = "Dây mảnh, nữ tính.",
                    Images =
                    [
                        "https://firebasestorage.googleapis.com/v0/b/mamafit-e0138.firebasestorage.app/o/components%2Fspaghetti-strap.png?alt=media&token=db6fb73c-11e8-4b72-8a64-3f87c82eb34c"
                    ],
                    GlobalStatus = 0,
                    CreatedBy = "System",
                    UpdatedBy = "System",
                    CreatedAt = new DateTime(2025, 8, 4, 14, 5, 3, DateTimeKind.Utc),
                    UpdatedAt = new DateTime(2025, 8, 4, 14, 5, 3, DateTimeKind.Utc),
                    IsDeleted = false
                },
                new ComponentOption
                {
                    Id = "8cff38afa8464ae191ba152b93438266",
                    ComponentId = "f00fcaf79a86427c995daa1fd0915d8e",
                    Name = "Single Layer",
                    Price = 12000.0m,
                    Description = "Gấu váy đơn giản, thanh lịch.",
                    Images =
                    [
                        "https://firebasestorage.googleapis.com/v0/b/mamafit-e0138.firebasestorage.app/o/components%2Fsingle-layer-hem.png?alt=media&token=bfd6b737-ca41-4a9d-9257-b35320fd63b4"
                    ],
                    GlobalStatus = 0,
                    CreatedBy = "System",
                    UpdatedBy = "System",
                    CreatedAt = new DateTime(2025, 8, 4, 14, 5, 3, DateTimeKind.Utc),
                    UpdatedAt = new DateTime(2025, 8, 4, 14, 5, 3, DateTimeKind.Utc),
                    IsDeleted = false
                }
            );

            // ===== PRESETS =====
            var presets = new List<Preset>
            {
                new Preset
                {
                    Id = "fe88b53dce4c4938a7785fb9e3db1f0a",
                    UserId = "f49aa51bbd304e77933e24bbed65b165",
                    StyleId = "037b41302a4c453f8cef135afceb3956",
                    Name = "Name",
                    Weight = 200,
                    Images = new List<string>
                    {
                        "https://firebasestorage.googleapis.com/v0/b/mamafit-e0138.firebasestorage.app/o/presets%2Fmaxi%2Fmaxi.png?alt=media&token=da59cd6d-df28-4605-a464-88951676e0fd"
                    },
                    IsDefault = true,
                    Price = 2000,
                    Type = 0,
                    CreatedBy = "User",
                    UpdatedBy = "System",
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new Preset
                {
                    Id = "7ae7818fdf09466f819808beed30e707",
                    UserId = "f49aa51bbd304e77933e24bbed65b165",
                    StyleId = "037b41302a4c453f8cef135afceb3956",
                    Name = "Name",
                    Weight = 200,
                    Images = new List<string>
                    {
                        "https://firebasestorage.googleapis.com/v0/b/mamafit-e0138.firebasestorage.app/o/presets%2Fmaxi%2Fmaxi-11.png?alt=media&token=14bc30f9-fa14-41ae-8bfa-cc4b94acda1b"
                    },
                    IsDefault = false,
                    Price = 2000,
                    Type = 0,
                    CreatedBy = "User",
                    UpdatedBy = "System",
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new Preset
                {
                    Id = "11230115583c4994bfaa3925951d3817",
                    UserId = "f49aa51bbd304e77933e24bbed65b165",
                    StyleId = "037b41302a4c453f8cef135afceb3956",
                    Name = "Name",
                    Weight = 200,
                    Images = new List<string>
                    {
                        "https://firebasestorage.googleapis.com/v0/b/mamafit-e0138.firebasestorage.app/o/presets%2Fmaxi%2Fmaxi-19.png?alt=media&token=053f4feb-7e75-4645-b8aa-e0e0b1fae0b3"
                    },
                    IsDefault = false,
                    Price = 2000,
                    Type = 0,
                    CreatedBy = "User",
                    UpdatedBy = "System",
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new Preset
                {
                    Id = "33c8177630a54906b92c0b8c03c57450",
                    UserId = "f49aa51bbd304e77933e24bbed65b165",
                    StyleId = "037b41302a4c453f8cef135afceb3956",
                    Name = "Name",
                    Weight = 200,
                    Images = new List<string>
                    {
                        "https://firebasestorage.googleapis.com/v0/b/mamafit-e0138.firebasestorage.app/o/presets%2Fmaxi%2Fmaxi-18.png?alt=media&token=fe931377-e708-4586-b897-4bdef49a0d5f"
                    },
                    IsDefault = false,
                    Price = 2000,
                    Type = 0,
                    CreatedBy = "User",
                    UpdatedBy = "System",
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new Preset
                {
                    Id = "c9348ab0418743d08ee786e31083d76b",
                    UserId = "f49aa51bbd304e77933e24bbed65b165",
                    StyleId = "037b41302a4c453f8cef135afceb3956",
                    Name = "Name",
                    Weight = 200,
                    Images = new List<string>
                    {
                        "https://firebasestorage.googleapis.com/v0/b/mamafit-e0138.firebasestorage.app/o/presets%2Fmaxi%2Fmaxi-17.png?alt=media&token=f90d9acc-504a-496b-8c4d-eb35b2a05fb0"
                    },
                    IsDefault = false,
                    Price = 2000,
                    Type = 0,
                    CreatedBy = "User",
                    UpdatedBy = "System",
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new Preset
                {
                    Id = "41e4b32655a44dca862141df8099f646",
                    UserId = "f49aa51bbd304e77933e24bbed65b165",
                    StyleId = "037b41302a4c453f8cef135afceb3956",
                    Name = "Name",
                    Weight = 200,
                    Images = new List<string>
                    {
                        "https://firebasestorage.googleapis.com/v0/b/mamafit-e0138.firebasestorage.app/o/presets%2Fmaxi%2Fmaxi-16.png?alt=media&token=1d05c179-85bd-444e-9056-be2509352f8c"
                    },
                    IsDefault = false,
                    Price = 2000,
                    Type = 0,
                    CreatedBy = "User",
                    UpdatedBy = "System",
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new Preset
                {
                    Id = "bcdcf89795b2484f96a3a12b25383a12",
                    UserId = "f49aa51bbd304e77933e24bbed65b165",
                    StyleId = "037b41302a4c453f8cef135afceb3956",
                    Name = "Name",
                    Weight = 200,
                    Images = new List<string>
                    {
                        "https://firebasestorage.googleapis.com/v0/b/mamafit-e0138.firebasestorage.app/o/presets%2Fmaxi%2Fmaxi-15.png?alt=media&token=a4a6b9e1-b20a-4939-bf9f-524f0c5911ae"
                    },
                    IsDefault = false,
                    Price = 2000,
                    Type = 0,
                    CreatedBy = "User",
                    UpdatedBy = "System",
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new Preset
                {
                    Id = "5ef79dda17e64808afa6287c07c8bac9",
                    UserId = "f49aa51bbd304e77933e24bbed65b165",
                    StyleId = "037b41302a4c453f8cef135afceb3956",
                    Name = "Name",
                    Weight = 200,
                    Images = new List<string>
                    {
                        "https://firebasestorage.googleapis.com/v0/b/mamafit-e0138.firebasestorage.app/o/presets%2Fmaxi%2Fmaxi-14.png?alt=media&token=77ec4929-09f7-427c-9714-9f287ac89f4c"
                    },
                    IsDefault = false,
                    Price = 2000,
                    Type = 0,
                    CreatedBy = "User",
                    UpdatedBy = "System",
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new Preset
                {
                    Id = "3583422ceef647d28a14b82680058ff0",
                    UserId = "f49aa51bbd304e77933e24bbed65b165",
                    StyleId = "037b41302a4c453f8cef135afceb3956",
                    Name = "Name",
                    Weight = 200,
                    Images = new List<string>
                    {
                        "https://firebasestorage.googleapis.com/v0/b/mamafit-e0138.firebasestorage.app/o/presets%2Fmaxi%2Fmaxi-13.png?alt=media&token=fdf74485-b0af-4c32-b7d3-cc78beb9dcd7"
                    },
                    IsDefault = false,
                    Price = 2000,
                    Type = 0,
                    CreatedBy = "User",
                    UpdatedBy = "System",
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new Preset
                {
                    Id = "6ab0daed7fe54e9fa791bfcb6bf1a043",
                    UserId = "f49aa51bbd304e77933e24bbed65b165",
                    StyleId = "037b41302a4c453f8cef135afceb3956",
                    Name = "Name",
                    Weight = 200,
                    Images = new List<string>
                    {
                        "https://firebasestorage.googleapis.com/v0/b/mamafit-e0138.firebasestorage.app/o/presets%2Fmaxi%2Fmaxi-12.png?alt=media&token=72e10ea4-1d5c-4f50-b614-b8c56eed7cfb"
                    },
                    IsDefault = false,
                    Price = 2000,
                    Type = 0,
                    CreatedBy = "User",
                    UpdatedBy = "System",
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new Preset
                {
                    Id = "ccc662f99d7a41d7aa1505e3a2cb8950",
                    UserId = "f49aa51bbd304e77933e24bbed65b165",
                    StyleId = "037b41302a4c453f8cef135afceb3956",
                    Name = "Name",
                    Weight = 200,
                    Images = new List<string>
                    {
                        "https://firebasestorage.googleapis.com/v0/b/mamafit-e0138.firebasestorage.app/o/presets%2Fmaxi%2Fmaxi-29.png?alt=media&token=25bc32d6-d2b7-4881-8ead-38dbad9096b6"
                    },
                    IsDefault = false,
                    Price = 2000,
                    Type = 0,
                    CreatedBy = "User",
                    UpdatedBy = "System",
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new Preset
                {
                    Id = "49e11d4698f643dca14364eed4d9c239",
                    UserId = "f49aa51bbd304e77933e24bbed65b165",
                    StyleId = "037b41302a4c453f8cef135afceb3956",
                    Name = "Name",
                    Weight = 200,
                    Images = new List<string>
                    {
                        "https://firebasestorage.googleapis.com/v0/b/mamafit-e0138.firebasestorage.app/o/presets%2Fmaxi%2Fmaxi-28.png?alt=media&token=6276c8c2-679d-48fc-a4ec-214ce17fe3b9"
                    },
                    IsDefault = false,
                    Price = 2000,
                    Type = 0,
                    CreatedBy = "User",
                    UpdatedBy = "System",
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new Preset
                {
                    Id = "6fd08876dd804c01983236cb5277b406",
                    UserId = "f49aa51bbd304e77933e24bbed65b165",
                    StyleId = "037b41302a4c453f8cef135afceb3956",
                    Name = "Name",
                    Weight = 200,
                    Images = new List<string>
                    {
                        "https://firebasestorage.googleapis.com/v0/b/mamafit-e0138.firebasestorage.app/o/presets%2Fmaxi%2Fmaxi-27.png?alt=media&token=c80d1ad8-5a45-4d8d-8e77-558ec415f728"
                    },
                    IsDefault = false,
                    Price = 2000,
                    Type = 0,
                    CreatedBy = "User",
                    UpdatedBy = "System",
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new Preset
                {
                    Id = "b0daa08ae6064686a59c0835bf58145c",
                    UserId = "f49aa51bbd304e77933e24bbed65b165",
                    StyleId = "037b41302a4c453f8cef135afceb3956",
                    Name = "Name",
                    Weight = 200,
                    Images = new List<string>
                    {
                        "https://firebasestorage.googleapis.com/v0/b/mamafit-e0138.firebasestorage.app/o/presets%2Fmaxi%2Fmaxi-26.png?alt=media&token=d3653bec-f9bc-4558-bc54-81d6a19c4ce2"
                    },
                    IsDefault = false,
                    Price = 2000,
                    Type = 0,
                    CreatedBy = "User",
                    UpdatedBy = "System",
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new Preset
                {
                    Id = "dd737fac7e8b4b0cb353b6d8ac16cb51",
                    UserId = "f49aa51bbd304e77933e24bbed65b165",
                    StyleId = "037b41302a4c453f8cef135afceb3956",
                    Name = "Name",
                    Weight = 200,
                    Images = new List<string>
                    {
                        "https://firebasestorage.googleapis.com/v0/b/mamafit-e0138.firebasestorage.app/o/presets%2Fmaxi%2Fmaxi-25.png?alt=media&token=f0a6e9b6-05cf-4e31-b1f3-9ecdbdcde3d2"
                    },
                    IsDefault = false,
                    Price = 2000,
                    Type = 0,
                    CreatedBy = "User",
                    UpdatedBy = "System",
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new Preset
                {
                    Id = "a80f79e565d240af9d77d2550c6e2933",
                    UserId = "f49aa51bbd304e77933e24bbed65b165",
                    StyleId = "037b41302a4c453f8cef135afceb3956",
                    Name = "Name",
                    Weight = 200,
                    Images = new List<string>
                    {
                        "https://firebasestorage.googleapis.com/v0/b/mamafit-e0138.firebasestorage.app/o/presets%2Fmaxi%2Fmaxi-24.png?alt=media&token=b0ceddd4-b37a-4be9-951a-e403ae5b9433"
                    },
                    IsDefault = false,
                    Price = 2000,
                    Type = 0,
                    CreatedBy = "User",
                    UpdatedBy = "System",
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new Preset
                {
                    Id = "be7b594cc1274fb5b9c4fa5571ed8952",
                    UserId = "f49aa51bbd304e77933e24bbed65b165",
                    StyleId = "037b41302a4c453f8cef135afceb3956",
                    Name = "Name",
                    Weight = 200,
                    Images = new List<string>
                    {
                        "https://firebasestorage.googleapis.com/v0/b/mamafit-e0138.firebasestorage.app/o/presets%2Fmaxi%2Fmaxi-23.png?alt=media&token=03440f28-4ec7-4fd7-8cd5-8a269106a726"
                    },
                    IsDefault = false,
                    Price = 2000,
                    Type = 0,
                    CreatedBy = "User",
                    UpdatedBy = "System",
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new Preset
                {
                    Id = "45f43125d239423893e337915ccaaea3",
                    UserId = "f49aa51bbd304e77933e24bbed65b165",
                    StyleId = "037b41302a4c453f8cef135afceb3956",
                    Name = "Name",
                    Weight = 200,
                    Images = new List<string>
                    {
                        "https://firebasestorage.googleapis.com/v0/b/mamafit-e0138.firebasestorage.app/o/presets%2Fmaxi%2Fmaxi-22.png?alt=media&token=0d80783e-3267-4743-a2b1-f931a48716fd"
                    },
                    IsDefault = false,
                    Price = 2000,
                    Type = 0,
                    CreatedBy = "User",
                    UpdatedBy = "System",
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new Preset
                {
                    Id = "7c2f591168f144d199a8d9e174addc45",
                    UserId = "f49aa51bbd304e77933e24bbed65b165",
                    StyleId = "037b41302a4c453f8cef135afceb3956", Name = "Name",
                    Weight = 200,
                    Images = new List<string>
                    {
                        "https://firebasestorage.googleapis.com/v0/b/mamafit-e0138.firebasestorage.app/o/presets%2Fmaxi%2Fmaxi-21.png?alt=media&token=13248af4-08bf-454c-9b30-6c741dbef82b"
                    },
                    IsDefault = false,
                    Price = 2000,
                    Type = 0,
                    CreatedBy = "User",
                    UpdatedBy = "System",
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new Preset
                {
                    Id = "ac1b2b6941c04598ae5a50d48efd8232",
                    UserId = "f49aa51bbd304e77933e24bbed65b165",
                    StyleId = "037b41302a4c453f8cef135afceb3956",
                    Name = "Name",
                    Weight = 200,
                    Images = new List<string>
                    {
                        "https://firebasestorage.googleapis.com/v0/b/mamafit-e0138.firebasestorage.app/o/presets%2Fmaxi%2Fmaxi-20.png?alt=media&token=2294c5c4-403d-4b33-afec-78f20bf02566"
                    },
                    IsDefault = false,
                    Price = 2000,
                    Type = 0,
                    CreatedBy = "User",
                    UpdatedBy = "System",
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new Preset
                {
                    Id = "58d759c12387490fb3a97c4633d6f1bb",
                    UserId = "f49aa51bbd304e77933e24bbed65b165",
                    StyleId = "037b41302a4c453f8cef135afceb3956",
                    Name = "Name",
                    Weight = 200,
                    Images = new List<string>
                    {
                        "https://firebasestorage.googleapis.com/v0/b/mamafit-e0138.firebasestorage.app/o/presets%2Fmaxi%2Fmaxi-10.png?alt=media&token=8fb01fe1-9ff2-4d7a-bf84-e8e1093f0f9b"
                    },
                    IsDefault = false,
                    Price = 2000,
                    Type = 0,
                    CreatedBy = "User",
                    UpdatedBy = "System",
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new Preset
                {
                    Id = "4f4d8b91705f4a9bb95ecf18b57b0306",
                    UserId = "f49aa51bbd304e77933e24bbed65b165",
                    StyleId = "037b41302a4c453f8cef135afceb3956",
                    Name = "Name",
                    Weight = 200,
                    Images = new List<string>
                    {
                        "https://firebasestorage.googleapis.com/v0/b/mamafit-e0138.firebasestorage.app/o/presets%2Fmaxi%2Fmaxi-2.png?alt=media&token=f6cf6663-58a2-4b43-9c8a-46796b1dd8e1"
                    },
                    IsDefault = false,
                    Price = 2000,
                    Type = 0,
                    CreatedBy = "User",
                    UpdatedBy = "System",
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new Preset
                {
                    Id = "694d8135f4ca4238956e9f47460b5dff",
                    UserId = "f49aa51bbd304e77933e24bbed65b165",
                    StyleId = "037b41302a4c453f8cef135afceb3956",
                    Name = "Name",
                    Weight = 200,
                    Images = new List<string>
                    {
                        "https://firebasestorage.googleapis.com/v0/b/mamafit-e0138.firebasestorage.app/o/presets%2Fmaxi%2Fmaxi-36.png?alt=media&token=dfa8808c-c87d-4ae0-be6b-1108343320ac"
                    },
                    IsDefault = false,
                    Price = 2000,
                    Type = 0,
                    CreatedBy = "User",
                    UpdatedBy = "System",
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new Preset
                {
                    Id = "f1ec1932c78e4a5a8035df1864fb32a4",
                    UserId = "f49aa51bbd304e77933e24bbed65b165",
                    StyleId = "037b41302a4c453f8cef135afceb3956",
                    Name = "Name",
                    Weight = 200,
                    Images = new List<string>
                    {
                        "https://firebasestorage.googleapis.com/v0/b/mamafit-e0138.firebasestorage.app/o/presets%2Fmaxi%2Fmaxi-35.png?alt=media&token=1a81d76f-506a-4b60-9c03-05fa9aab1483"
                    },
                    IsDefault = false,
                    Price = 2000,
                    Type = 0,
                    CreatedBy = "User",
                    UpdatedBy = "System",
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new Preset
                {
                    Id = "cc5b36affe2243f2b119fd65cb3434d5",
                    UserId = "f49aa51bbd304e77933e24bbed65b165",
                    StyleId = "037b41302a4c453f8cef135afceb3956",
                    Name = "Name",
                    Weight = 200,
                    Images = new List<string>
                    {
                        "https://firebasestorage.googleapis.com/v0/b/mamafit-e0138.firebasestorage.app/o/presets%2Fmaxi%2Fmaxi-34.png?alt=media&token=076bb0d1-0496-4353-adb2-1be1fce6e8bb"
                    },
                    IsDefault = false,
                    Price = 2000,
                    Type = 0,
                    CreatedBy = "User",
                    UpdatedBy = "System",
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new Preset
                {
                    Id = "7abc9feb7c1e47778ec32b24aac95985",
                    UserId = "f49aa51bbd304e77933e24bbed65b165",
                    StyleId = "037b41302a4c453f8cef135afceb3956",
                    Name = "Name",
                    Weight = 200,
                    Images = new List<string>
                    {
                        "https://firebasestorage.googleapis.com/v0/b/mamafit-e0138.firebasestorage.app/o/presets%2Fmaxi%2Fmaxi-33.png?alt=media&token=5f0263e2-84b2-4260-9c25-ca2b12411fc2"
                    },
                    IsDefault = false,
                    Price = 2000,
                    Type = 0,
                    CreatedBy = "User",
                    UpdatedBy = "System",
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new Preset
                {
                    Id = "380e3d771eac4488ad94b09c9e77ccd2",
                    UserId = "f49aa51bbd304e77933e24bbed65b165",
                    StyleId = "037b41302a4c453f8cef135afceb3956",
                    Name = "Name",
                    Weight = 200,
                    Images = new List<string>
                    {
                        "https://firebasestorage.googleapis.com/v0/b/mamafit-e0138.firebasestorage.app/o/presets%2Fmaxi%2Fmaxi-32.png?alt=media&token=d9268430-d725-4813-a70f-260ee49cd1c5"
                    },
                    IsDefault = false,
                    Price = 2000,
                    Type = 0,
                    CreatedBy = "User",
                    UpdatedBy = "System",
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new Preset
                {
                    Id = "1acd7ac56e5c4d15895f03cc2d6e7723",
                    UserId = "f49aa51bbd304e77933e24bbed65b165",
                    StyleId = "037b41302a4c453f8cef135afceb3956",
                    Name = "Name",
                    Weight = 200,
                    Images = new List<string>
                    {
                        "https://firebasestorage.googleapis.com/v0/b/mamafit-e0138.firebasestorage.app/o/presets%2Fmaxi%2Fmaxi-31.png?alt=media&token=97b8fadf-6bf1-4ab7-995a-5126f7fbfc65"
                    },
                    IsDefault = false,
                    Price = 2000,
                    Type = 0,
                    CreatedBy = "User",
                    UpdatedBy = "System",
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new Preset
                {
                    Id = "50959c7c201e45a9b5255558ac9dfcf7",
                    UserId = "f49aa51bbd304e77933e24bbed65b165",
                    StyleId = "037b41302a4c453f8cef135afceb3956",
                    Name = "Name",
                    Weight = 200,
                    Images = new List<string>
                    {
                        "https://firebasestorage.googleapis.com/v0/b/mamafit-e0138.firebasestorage.app/o/presets%2Fmaxi%2Fmaxi-30.png?alt=media&token=373eb84b-cd7b-4e81-840d-516781ae9d72"
                    },
                    IsDefault = false,
                    Price = 2000,
                    Type = 0,
                    CreatedBy = "User",
                    UpdatedBy = "System",
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new Preset
                {
                    Id = "5a5081ce720141188c09336e4a936ef9",
                    UserId = "f49aa51bbd304e77933e24bbed65b165",
                    StyleId = "037b41302a4c453f8cef135afceb3956",
                    Name = "Name",
                    Weight = 200,
                    Images = new List<string>
                    {
                        "https://firebasestorage.googleapis.com/v0/b/mamafit-e0138.firebasestorage.app/o/presets%2Fmaxi%2Fmaxi-3.png?alt=media&token=149e97e6-d906-4345-bfcb-dfc3d619a73b"
                    },
                    IsDefault = false,
                    Price = 2000,
                    Type = 0,
                    CreatedBy = "User",
                    UpdatedBy = "System",
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new Preset
                {
                    Id = "fe7043a37995453db2d03741e81da9df",
                    UserId = "f49aa51bbd304e77933e24bbed65b165",
                    StyleId = "037b41302a4c453f8cef135afceb3956",
                    Name = "Name",
                    Weight = 200,
                    Images = new List<string>
                    {
                        "https://firebasestorage.googleapis.com/v0/b/mamafit-e0138.firebasestorage.app/o/presets%2Fmaxi%2Fmaxi-9.png?alt=media&token=221bb4f5-1a84-452a-9a40-48cc8f50afdc"
                    },
                    IsDefault = false,
                    Price = 2000,
                    Type = 0,
                    CreatedBy = "User",
                    UpdatedBy = "System",
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new Preset
                {
                    Id = "7faa6eb739d548bf9f9ce7d3de978d3d",
                    UserId = "f49aa51bbd304e77933e24bbed65b165",
                    StyleId = "037b41302a4c453f8cef135afceb3956",
                    Name = "Name",
                    Weight = 200,
                    Images = new List<string>
                    {
                        "https://firebasestorage.googleapis.com/v0/b/mamafit-e0138.firebasestorage.app/o/presets%2Fmaxi%2Fmaxi-8.png?alt=media&token=ccad2c07-0022-4956-9591-8ddcbda3a31a"
                    },
                    IsDefault = false,
                    Price = 2000,
                    Type = 0,
                    CreatedBy = "User",
                    UpdatedBy = "System",
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new Preset
                {
                    Id = "caa36b99b068411aaa62b0f4234c1656",
                    UserId = "f49aa51bbd304e77933e24bbed65b165",
                    StyleId = "037b41302a4c453f8cef135afceb3956",
                    Name = "Name",
                    Weight = 200,
                    Images = new List<string>
                    {
                        "https://firebasestorage.googleapis.com/v0/b/mamafit-e0138.firebasestorage.app/o/presets%2Fmaxi%2Fmaxi-7.png?alt=media&token=29058fd5-ec1f-4199-94ff-72c77584b562"
                    },
                    IsDefault = false,
                    Price = 2000,
                    Type = 0,
                    CreatedBy = "User",
                    UpdatedBy = "System",
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new Preset
                {
                    Id = "68ee38a6554f407abd932d492719a834",
                    UserId = "f49aa51bbd304e77933e24bbed65b165",
                    StyleId = "037b41302a4c453f8cef135afceb3956",
                    Name = "Name",
                    Weight = 200,
                    Images = new List<string>
                    {
                        "https://firebasestorage.googleapis.com/v0/b/mamafit-e0138.firebasestorage.app/o/presets%2Fmaxi%2Fmaxi-6.png?alt=media&token=28833b6c-ff85-4eb2-8f85-65e9ea4d8d23"
                    },
                    IsDefault = false,
                    Price = 2000,
                    Type = 0,
                    CreatedBy = "User",
                    UpdatedBy = "System",
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new Preset
                {
                    Id = "bac18c4350314174b9f4cbe73ca0b24d",
                    UserId = "f49aa51bbd304e77933e24bbed65b165",
                    StyleId = "037b41302a4c453f8cef135afceb3956",
                    Name = "Name",
                    Weight = 200,
                    Images = new List<string>
                    {
                        "https://firebasestorage.googleapis.com/v0/b/mamafit-e0138.firebasestorage.app/o/presets%2Fmaxi%2Fmaxi-5.png?alt=media&token=778c61b7-68ff-4224-afd8-223d9295c99c"
                    },
                    IsDefault = false,
                    Price = 2000,
                    Type = 0,
                    CreatedBy = "User",
                    UpdatedBy = "System",
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new Preset
                {
                    Id = "68fca8f086454898ae81b1ea29890b60",
                    UserId = "f49aa51bbd304e77933e24bbed65b165",
                    StyleId = "037b41302a4c453f8cef135afceb3956",
                    Name = "Name",
                    Weight = 200,
                    Images = new List<string>
                    {
                        "https://firebasestorage.googleapis.com/v0/b/mamafit-e0138.firebasestorage.app/o/presets%2Fmaxi%2Fmaxi-4.png?alt=media&token=ea8426c2-8bac-4b0d-bff3-e7e2536ac351"
                    },
                    IsDefault = false,
                    Price = 2000,
                    Type = 0,
                    CreatedBy = "User",
                    UpdatedBy = "System",
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                }
            };
            modelBuilder.Entity<Preset>().HasData(presets);

            // ===== COMPONENT OPTION PRESETS =====
            var componentoptionpresets = new List<ComponentOptionPreset>
            {
                new ComponentOptionPreset
                {
                    ComponentOptionsId = "06e6899bd6544dd3b9020a50789d9421",
                    PresetsId = "fe88b53dce4c4938a7785fb9e3db1f0a"
                },
                new ComponentOptionPreset
                {
                    ComponentOptionsId = "31c853d3dde94e68ab57903cf8a80798",
                    PresetsId = "fe88b53dce4c4938a7785fb9e3db1f0a"
                },
                new ComponentOptionPreset
                {
                    ComponentOptionsId = "4ed7cdc105ab44de81d333d63db0a222",
                    PresetsId = "fe88b53dce4c4938a7785fb9e3db1f0a"
                },
                new ComponentOptionPreset
                {
                    ComponentOptionsId = "8cff38afa8464ae191ba152b93438266",
                    PresetsId = "fe88b53dce4c4938a7785fb9e3db1f0a"
                },
                new ComponentOptionPreset
                {
                    ComponentOptionsId = "92bdbbdbfd3b4dcf8256b68a66dfa35b",
                    PresetsId = "fe88b53dce4c4938a7785fb9e3db1f0a"
                },
                new ComponentOptionPreset
                {
                    ComponentOptionsId = "c454b97a93f341e78b127d5425acc464",
                    PresetsId = "fe88b53dce4c4938a7785fb9e3db1f0a"
                },
                new ComponentOptionPreset
                {
                    ComponentOptionsId = "31c853d3dde94e68ab57903cf8a80798",
                    PresetsId = "58d759c12387490fb3a97c4633d6f1bb"
                },
                new ComponentOptionPreset
                {
                    ComponentOptionsId = "8158b8e5ef0340bdaf830fa8eb2e650d",
                    PresetsId = "58d759c12387490fb3a97c4633d6f1bb"
                },
                new ComponentOptionPreset
                {
                    ComponentOptionsId = "8cff38afa8464ae191ba152b93438266",
                    PresetsId = "58d759c12387490fb3a97c4633d6f1bb"
                },
                new ComponentOptionPreset
                {
                    ComponentOptionsId = "92bdbbdbfd3b4dcf8256b68a66dfa35b",
                    PresetsId = "58d759c12387490fb3a97c4633d6f1bb"
                },
                new ComponentOptionPreset
                {
                    ComponentOptionsId = "c454b97a93f341e78b127d5425acc464",
                    PresetsId = "58d759c12387490fb3a97c4633d6f1bb"
                },
                new ComponentOptionPreset
                {
                    ComponentOptionsId = "fecd26fe3b6b4e88ba97ebb406fa6df2",
                    PresetsId = "58d759c12387490fb3a97c4633d6f1bb"
                },
                new ComponentOptionPreset
                {
                    ComponentOptionsId = "31c853d3dde94e68ab57903cf8a80798",
                    PresetsId = "7ae7818fdf09466f819808beed30e707"
                },
                new ComponentOptionPreset
                {
                    ComponentOptionsId = "818f875df04a49eebe9c474bba4247b9",
                    PresetsId = "7ae7818fdf09466f819808beed30e707"
                },
                new ComponentOptionPreset
                {
                    ComponentOptionsId = "81acc249e2664c72908014a28e1e72ef",
                    PresetsId = "7ae7818fdf09466f819808beed30e707"
                },
                new ComponentOptionPreset
                {
                    ComponentOptionsId = "8cff38afa8464ae191ba152b93438266",
                    PresetsId = "7ae7818fdf09466f819808beed30e707"
                },
                new ComponentOptionPreset
                {
                    ComponentOptionsId = "c454b97a93f341e78b127d5425acc464",
                    PresetsId = "7ae7818fdf09466f819808beed30e707"
                },
                new ComponentOptionPreset
                {
                    ComponentOptionsId = "fecd26fe3b6b4e88ba97ebb406fa6df2",
                    PresetsId = "7ae7818fdf09466f819808beed30e707"
                },
                new ComponentOptionPreset
                {
                    ComponentOptionsId = "06e6899bd6544dd3b9020a50789d9421",
                    PresetsId = "11230115583c4994bfaa3925951d3817"
                },
                new ComponentOptionPreset
                {
                    ComponentOptionsId = "31c853d3dde94e68ab57903cf8a80798",
                    PresetsId = "11230115583c4994bfaa3925951d3817"
                },
                new ComponentOptionPreset
                {
                    ComponentOptionsId = "4ed7cdc105ab44de81d333d63db0a222",
                    PresetsId = "11230115583c4994bfaa3925951d3817"
                },
                new ComponentOptionPreset
                {
                    ComponentOptionsId = "70b69ec5ea074c0f8bf904b20b9c7884",
                    PresetsId = "11230115583c4994bfaa3925951d3817"
                },
                new ComponentOptionPreset
                {
                    ComponentOptionsId = "92bdbbdbfd3b4dcf8256b68a66dfa35b",
                    PresetsId = "11230115583c4994bfaa3925951d3817"
                },
                new ComponentOptionPreset
                {
                    ComponentOptionsId = "c454b97a93f341e78b127d5425acc464",
                    PresetsId = "11230115583c4994bfaa3925951d3817"
                },
                new ComponentOptionPreset
                {
                    ComponentOptionsId = "31c853d3dde94e68ab57903cf8a80798",
                    PresetsId = "33c8177630a54906b92c0b8c03c57450"
                },
                new ComponentOptionPreset
                {
                    ComponentOptionsId = "70b69ec5ea074c0f8bf904b20b9c7884",
                    PresetsId = "33c8177630a54906b92c0b8c03c57450"
                },
                new ComponentOptionPreset
                {
                    ComponentOptionsId = "8158b8e5ef0340bdaf830fa8eb2e650d",
                    PresetsId = "33c8177630a54906b92c0b8c03c57450"
                },
                new ComponentOptionPreset
                {
                    ComponentOptionsId = "92bdbbdbfd3b4dcf8256b68a66dfa35b",
                    PresetsId = "33c8177630a54906b92c0b8c03c57450"
                },
                new ComponentOptionPreset
                {
                    ComponentOptionsId = "c454b97a93f341e78b127d5425acc464",
                    PresetsId = "33c8177630a54906b92c0b8c03c57450"
                },
                new ComponentOptionPreset
                {
                    ComponentOptionsId = "fecd26fe3b6b4e88ba97ebb406fa6df2",
                    PresetsId = "33c8177630a54906b92c0b8c03c57450"
                },
                new ComponentOptionPreset
                {
                    ComponentOptionsId = "06e6899bd6544dd3b9020a50789d9421",
                    PresetsId = "c9348ab0418743d08ee786e31083d76b"
                },
                new ComponentOptionPreset
                {
                    ComponentOptionsId = "31c853d3dde94e68ab57903cf8a80798",
                    PresetsId = "c9348ab0418743d08ee786e31083d76b"
                },
                new ComponentOptionPreset
                {
                    ComponentOptionsId = "70b69ec5ea074c0f8bf904b20b9c7884",
                    PresetsId = "c9348ab0418743d08ee786e31083d76b"
                },
                new ComponentOptionPreset
                {
                    ComponentOptionsId = "92bdbbdbfd3b4dcf8256b68a66dfa35b",
                    PresetsId = "c9348ab0418743d08ee786e31083d76b"
                },
                new ComponentOptionPreset
                {
                    ComponentOptionsId = "c454b97a93f341e78b127d5425acc464",
                    PresetsId = "c9348ab0418743d08ee786e31083d76b"
                },
                new ComponentOptionPreset
                {
                    ComponentOptionsId = "fecd26fe3b6b4e88ba97ebb406fa6df2",
                    PresetsId = "c9348ab0418743d08ee786e31083d76b"
                },
                new ComponentOptionPreset
                {
                    ComponentOptionsId = "06e6899bd6544dd3b9020a50789d9421",
                    PresetsId = "41e4b32655a44dca862141df8099f646"
                },
                new ComponentOptionPreset
                {
                    ComponentOptionsId = "31c853d3dde94e68ab57903cf8a80798",
                    PresetsId = "41e4b32655a44dca862141df8099f646"
                },
                new ComponentOptionPreset
                {
                    ComponentOptionsId = "4ed7cdc105ab44de81d333d63db0a222",
                    PresetsId = "41e4b32655a44dca862141df8099f646"
                },
                new ComponentOptionPreset
                {
                    ComponentOptionsId = "70b69ec5ea074c0f8bf904b20b9c7884",
                    PresetsId = "41e4b32655a44dca862141df8099f646"
                },
                new ComponentOptionPreset
                {
                    ComponentOptionsId = "818f875df04a49eebe9c474bba4247b9",
                    PresetsId = "41e4b32655a44dca862141df8099f646"
                },
                new ComponentOptionPreset
                {
                    ComponentOptionsId = "c454b97a93f341e78b127d5425acc464",
                    PresetsId = "41e4b32655a44dca862141df8099f646"
                },
                new ComponentOptionPreset
                {
                    ComponentOptionsId = "06e6899bd6544dd3b9020a50789d9421",
                    PresetsId = "bcdcf89795b2484f96a3a12b25383a12"
                },
                new ComponentOptionPreset
                {
                    ComponentOptionsId = "31c853d3dde94e68ab57903cf8a80798",
                    PresetsId = "bcdcf89795b2484f96a3a12b25383a12"
                },
                new ComponentOptionPreset
                {
                    ComponentOptionsId = "78b78938aefe4bc6b927ed3722bc5859",
                    PresetsId = "bcdcf89795b2484f96a3a12b25383a12"
                },
                new ComponentOptionPreset
                {
                    ComponentOptionsId = "818f875df04a49eebe9c474bba4247b9",
                    PresetsId = "bcdcf89795b2484f96a3a12b25383a12"
                },
                new ComponentOptionPreset
                {
                    ComponentOptionsId = "8cff38afa8464ae191ba152b93438266",
                    PresetsId = "bcdcf89795b2484f96a3a12b25383a12"
                },
                new ComponentOptionPreset
                {
                    ComponentOptionsId = "c454b97a93f341e78b127d5425acc464",
                    PresetsId = "bcdcf89795b2484f96a3a12b25383a12"
                },
                new ComponentOptionPreset
                {
                    ComponentOptionsId = "31c853d3dde94e68ab57903cf8a80798",
                    PresetsId = "5ef79dda17e64808afa6287c07c8bac9"
                },
                new ComponentOptionPreset
                {
                    ComponentOptionsId = "4ed7cdc105ab44de81d333d63db0a222",
                    PresetsId = "5ef79dda17e64808afa6287c07c8bac9"
                },
                new ComponentOptionPreset
                {
                    ComponentOptionsId = "70b69ec5ea074c0f8bf904b20b9c7884",
                    PresetsId = "5ef79dda17e64808afa6287c07c8bac9"
                },
                new ComponentOptionPreset
                {
                    ComponentOptionsId = "8158b8e5ef0340bdaf830fa8eb2e650d",
                    PresetsId = "5ef79dda17e64808afa6287c07c8bac9"
                },
                new ComponentOptionPreset
                {
                    ComponentOptionsId = "92bdbbdbfd3b4dcf8256b68a66dfa35b",
                    PresetsId = "5ef79dda17e64808afa6287c07c8bac9"
                },
                new ComponentOptionPreset
                {
                    ComponentOptionsId = "c454b97a93f341e78b127d5425acc464",
                    PresetsId = "5ef79dda17e64808afa6287c07c8bac9"
                },
                new ComponentOptionPreset
                {
                    ComponentOptionsId = "06e6899bd6544dd3b9020a50789d9421",
                    PresetsId = "3583422ceef647d28a14b82680058ff0"
                },
                new ComponentOptionPreset
                {
                    ComponentOptionsId = "31c853d3dde94e68ab57903cf8a80798",
                    PresetsId = "3583422ceef647d28a14b82680058ff0"
                },
                new ComponentOptionPreset
                {
                    ComponentOptionsId = "78b78938aefe4bc6b927ed3722bc5859",
                    PresetsId = "3583422ceef647d28a14b82680058ff0"
                },
                new ComponentOptionPreset
                {
                    ComponentOptionsId = "8cff38afa8464ae191ba152b93438266",
                    PresetsId = "3583422ceef647d28a14b82680058ff0"
                },
                new ComponentOptionPreset
                {
                    ComponentOptionsId = "92bdbbdbfd3b4dcf8256b68a66dfa35b",
                    PresetsId = "3583422ceef647d28a14b82680058ff0"
                },
                new ComponentOptionPreset
                {
                    ComponentOptionsId = "c454b97a93f341e78b127d5425acc464",
                    PresetsId = "3583422ceef647d28a14b82680058ff0"
                },
                new ComponentOptionPreset
                {
                    ComponentOptionsId = "31c853d3dde94e68ab57903cf8a80798",
                    PresetsId = "6ab0daed7fe54e9fa791bfcb6bf1a043"
                },
                new ComponentOptionPreset
                {
                    ComponentOptionsId = "81acc249e2664c72908014a28e1e72ef",
                    PresetsId = "6ab0daed7fe54e9fa791bfcb6bf1a043"
                },
                new ComponentOptionPreset
                {
                    ComponentOptionsId = "8cff38afa8464ae191ba152b93438266",
                    PresetsId = "6ab0daed7fe54e9fa791bfcb6bf1a043"
                },
                new ComponentOptionPreset
                {
                    ComponentOptionsId = "92bdbbdbfd3b4dcf8256b68a66dfa35b",
                    PresetsId = "6ab0daed7fe54e9fa791bfcb6bf1a043"
                },
                new ComponentOptionPreset
                {
                    ComponentOptionsId = "c454b97a93f341e78b127d5425acc464",
                    PresetsId = "6ab0daed7fe54e9fa791bfcb6bf1a043"
                },
                new ComponentOptionPreset
                {
                    ComponentOptionsId = "fecd26fe3b6b4e88ba97ebb406fa6df2",
                    PresetsId = "6ab0daed7fe54e9fa791bfcb6bf1a043"
                },
                new ComponentOptionPreset
                {
                    ComponentOptionsId = "31c853d3dde94e68ab57903cf8a80798",
                    PresetsId = "ccc662f99d7a41d7aa1505e3a2cb8950"
                },
                new ComponentOptionPreset
                {
                    ComponentOptionsId = "70b69ec5ea074c0f8bf904b20b9c7884",
                    PresetsId = "ccc662f99d7a41d7aa1505e3a2cb8950"
                },
                new ComponentOptionPreset
                {
                    ComponentOptionsId = "8158b8e5ef0340bdaf830fa8eb2e650d",
                    PresetsId = "ccc662f99d7a41d7aa1505e3a2cb8950"
                },
                new ComponentOptionPreset
                {
                    ComponentOptionsId = "818f875df04a49eebe9c474bba4247b9",
                    PresetsId = "ccc662f99d7a41d7aa1505e3a2cb8950"
                },
                new ComponentOptionPreset
                {
                    ComponentOptionsId = "c454b97a93f341e78b127d5425acc464",
                    PresetsId = "ccc662f99d7a41d7aa1505e3a2cb8950"
                },
                new ComponentOptionPreset
                {
                    ComponentOptionsId = "fecd26fe3b6b4e88ba97ebb406fa6df2",
                    PresetsId = "ccc662f99d7a41d7aa1505e3a2cb8950"
                },
                new ComponentOptionPreset
                {
                    ComponentOptionsId = "31c853d3dde94e68ab57903cf8a80798",
                    PresetsId = "49e11d4698f643dca14364eed4d9c239"
                },
                new ComponentOptionPreset
                {
                    ComponentOptionsId = "78b78938aefe4bc6b927ed3722bc5859",
                    PresetsId = "49e11d4698f643dca14364eed4d9c239"
                },
                new ComponentOptionPreset
                {
                    ComponentOptionsId = "818f875df04a49eebe9c474bba4247b9",
                    PresetsId = "49e11d4698f643dca14364eed4d9c239"
                },
                new ComponentOptionPreset
                {
                    ComponentOptionsId = "81acc249e2664c72908014a28e1e72ef",
                    PresetsId = "49e11d4698f643dca14364eed4d9c239"
                },
                new ComponentOptionPreset
                {
                    ComponentOptionsId = "8cff38afa8464ae191ba152b93438266",
                    PresetsId = "49e11d4698f643dca14364eed4d9c239"
                },
                new ComponentOptionPreset
                {
                    ComponentOptionsId = "c454b97a93f341e78b127d5425acc464",
                    PresetsId = "49e11d4698f643dca14364eed4d9c239"
                },
                new ComponentOptionPreset
                {
                    ComponentOptionsId = "31c853d3dde94e68ab57903cf8a80798",
                    PresetsId = "6fd08876dd804c01983236cb5277b406"
                },
                new ComponentOptionPreset
                {
                    ComponentOptionsId = "78b78938aefe4bc6b927ed3722bc5859",
                    PresetsId = "6fd08876dd804c01983236cb5277b406"
                },
                new ComponentOptionPreset
                {
                    ComponentOptionsId = "8158b8e5ef0340bdaf830fa8eb2e650d",
                    PresetsId = "6fd08876dd804c01983236cb5277b406"
                },
                new ComponentOptionPreset
                {
                    ComponentOptionsId = "818f875df04a49eebe9c474bba4247b9",
                    PresetsId = "6fd08876dd804c01983236cb5277b406"
                },
                new ComponentOptionPreset
                {
                    ComponentOptionsId = "8cff38afa8464ae191ba152b93438266",
                    PresetsId = "6fd08876dd804c01983236cb5277b406"
                },
                new ComponentOptionPreset
                {
                    ComponentOptionsId = "c454b97a93f341e78b127d5425acc464",
                    PresetsId = "6fd08876dd804c01983236cb5277b406"
                },
                new ComponentOptionPreset
                {
                    ComponentOptionsId = "31c853d3dde94e68ab57903cf8a80798",
                    PresetsId = "b0daa08ae6064686a59c0835bf58145c"
                },
                new ComponentOptionPreset
                {
                    ComponentOptionsId = "70b69ec5ea074c0f8bf904b20b9c7884",
                    PresetsId = "b0daa08ae6064686a59c0835bf58145c"
                },
                new ComponentOptionPreset
                {
                    ComponentOptionsId = "78b78938aefe4bc6b927ed3722bc5859",
                    PresetsId = "b0daa08ae6064686a59c0835bf58145c"
                },
                new ComponentOptionPreset
                {
                    ComponentOptionsId = "81acc249e2664c72908014a28e1e72ef",
                    PresetsId = "b0daa08ae6064686a59c0835bf58145c"
                },
                new ComponentOptionPreset
                {
                    ComponentOptionsId = "92bdbbdbfd3b4dcf8256b68a66dfa35b",
                    PresetsId = "b0daa08ae6064686a59c0835bf58145c"
                },
                new ComponentOptionPreset
                {
                    ComponentOptionsId = "c454b97a93f341e78b127d5425acc464",
                    PresetsId = "b0daa08ae6064686a59c0835bf58145c"
                },
                new ComponentOptionPreset
                {
                    ComponentOptionsId = "31c853d3dde94e68ab57903cf8a80798",
                    PresetsId = "dd737fac7e8b4b0cb353b6d8ac16cb51"
                },
                new ComponentOptionPreset
                {
                    ComponentOptionsId = "70b69ec5ea074c0f8bf904b20b9c7884",
                    PresetsId = "dd737fac7e8b4b0cb353b6d8ac16cb51"
                },
                new ComponentOptionPreset
                {
                    ComponentOptionsId = "78b78938aefe4bc6b927ed3722bc5859",
                    PresetsId = "dd737fac7e8b4b0cb353b6d8ac16cb51"
                },
                new ComponentOptionPreset
                {
                    ComponentOptionsId = "8158b8e5ef0340bdaf830fa8eb2e650d",
                    PresetsId = "dd737fac7e8b4b0cb353b6d8ac16cb51"
                },
                new ComponentOptionPreset
                {
                    ComponentOptionsId = "92bdbbdbfd3b4dcf8256b68a66dfa35b",
                    PresetsId = "dd737fac7e8b4b0cb353b6d8ac16cb51"
                },
                new ComponentOptionPreset
                {
                    ComponentOptionsId = "c454b97a93f341e78b127d5425acc464",
                    PresetsId = "dd737fac7e8b4b0cb353b6d8ac16cb51"
                },
                new ComponentOptionPreset
                {
                    ComponentOptionsId = "06e6899bd6544dd3b9020a50789d9421",
                    PresetsId = "a80f79e565d240af9d77d2550c6e2933"
                },
                new ComponentOptionPreset
                {
                    ComponentOptionsId = "31c853d3dde94e68ab57903cf8a80798",
                    PresetsId = "a80f79e565d240af9d77d2550c6e2933"
                },
                new ComponentOptionPreset
                {
                    ComponentOptionsId = "70b69ec5ea074c0f8bf904b20b9c7884",
                    PresetsId = "a80f79e565d240af9d77d2550c6e2933"
                },
                new ComponentOptionPreset
                {
                    ComponentOptionsId = "78b78938aefe4bc6b927ed3722bc5859",
                    PresetsId = "a80f79e565d240af9d77d2550c6e2933"
                },
                new ComponentOptionPreset
                {
                    ComponentOptionsId = "92bdbbdbfd3b4dcf8256b68a66dfa35b",
                    PresetsId = "a80f79e565d240af9d77d2550c6e2933"
                },
                new ComponentOptionPreset
                {
                    ComponentOptionsId = "c454b97a93f341e78b127d5425acc464",
                    PresetsId = "a80f79e565d240af9d77d2550c6e2933"
                },
                new ComponentOptionPreset
                {
                    ComponentOptionsId = "31c853d3dde94e68ab57903cf8a80798",
                    PresetsId = "be7b594cc1274fb5b9c4fa5571ed8952"
                },
                new ComponentOptionPreset
                {
                    ComponentOptionsId = "78b78938aefe4bc6b927ed3722bc5859",
                    PresetsId = "be7b594cc1274fb5b9c4fa5571ed8952"
                },
                new ComponentOptionPreset
                {
                    ComponentOptionsId = "81acc249e2664c72908014a28e1e72ef",
                    PresetsId = "be7b594cc1274fb5b9c4fa5571ed8952"
                },
                new ComponentOptionPreset
                {
                    ComponentOptionsId = "8cff38afa8464ae191ba152b93438266",
                    PresetsId = "be7b594cc1274fb5b9c4fa5571ed8952"
                },
                new ComponentOptionPreset
                {
                    ComponentOptionsId = "92bdbbdbfd3b4dcf8256b68a66dfa35b",
                    PresetsId = "be7b594cc1274fb5b9c4fa5571ed8952"
                },
                new ComponentOptionPreset
                {
                    ComponentOptionsId = "c454b97a93f341e78b127d5425acc464",
                    PresetsId = "be7b594cc1274fb5b9c4fa5571ed8952"
                },
                new ComponentOptionPreset
                {
                    ComponentOptionsId = "31c853d3dde94e68ab57903cf8a80798",
                    PresetsId = "45f43125d239423893e337915ccaaea3"
                },
                new ComponentOptionPreset
                {
                    ComponentOptionsId = "78b78938aefe4bc6b927ed3722bc5859",
                    PresetsId = "45f43125d239423893e337915ccaaea3"
                },
                new ComponentOptionPreset
                {
                    ComponentOptionsId = "8158b8e5ef0340bdaf830fa8eb2e650d",
                    PresetsId = "45f43125d239423893e337915ccaaea3"
                },
                new ComponentOptionPreset
                {
                    ComponentOptionsId = "8cff38afa8464ae191ba152b93438266",
                    PresetsId = "45f43125d239423893e337915ccaaea3"
                },
                new ComponentOptionPreset
                {
                    ComponentOptionsId = "92bdbbdbfd3b4dcf8256b68a66dfa35b",
                    PresetsId = "45f43125d239423893e337915ccaaea3"
                },
                new ComponentOptionPreset
                {
                    ComponentOptionsId = "c454b97a93f341e78b127d5425acc464",
                    PresetsId = "45f43125d239423893e337915ccaaea3"
                },
                new ComponentOptionPreset
                {
                    ComponentOptionsId = "31c853d3dde94e68ab57903cf8a80798",
                    PresetsId = "7c2f591168f144d199a8d9e174addc45"
                },
                new ComponentOptionPreset
                {
                    ComponentOptionsId = "70b69ec5ea074c0f8bf904b20b9c7884",
                    PresetsId = "7c2f591168f144d199a8d9e174addc45"
                },
                new ComponentOptionPreset
                {
                    ComponentOptionsId = "81acc249e2664c72908014a28e1e72ef",
                    PresetsId = "7c2f591168f144d199a8d9e174addc45"
                },
                new ComponentOptionPreset
                {
                    ComponentOptionsId = "92bdbbdbfd3b4dcf8256b68a66dfa35b",
                    PresetsId = "7c2f591168f144d199a8d9e174addc45"
                },
                new ComponentOptionPreset
                {
                    ComponentOptionsId = "c454b97a93f341e78b127d5425acc464",
                    PresetsId = "7c2f591168f144d199a8d9e174addc45"
                },
                new ComponentOptionPreset
                {
                    ComponentOptionsId = "fecd26fe3b6b4e88ba97ebb406fa6df2",
                    PresetsId = "7c2f591168f144d199a8d9e174addc45"
                },
                new ComponentOptionPreset
                {
                    ComponentOptionsId = "31c853d3dde94e68ab57903cf8a80798",
                    PresetsId = "ac1b2b6941c04598ae5a50d48efd8232"
                },
                new ComponentOptionPreset
                {
                    ComponentOptionsId = "4ed7cdc105ab44de81d333d63db0a222",
                    PresetsId = "ac1b2b6941c04598ae5a50d48efd8232"
                },
                new ComponentOptionPreset
                {
                    ComponentOptionsId = "70b69ec5ea074c0f8bf904b20b9c7884",
                    PresetsId = "ac1b2b6941c04598ae5a50d48efd8232"
                },
                new ComponentOptionPreset
                {
                    ComponentOptionsId = "81acc249e2664c72908014a28e1e72ef",
                    PresetsId = "ac1b2b6941c04598ae5a50d48efd8232"
                },
                new ComponentOptionPreset
                {
                    ComponentOptionsId = "92bdbbdbfd3b4dcf8256b68a66dfa35b",
                    PresetsId = "ac1b2b6941c04598ae5a50d48efd8232"
                },
                new ComponentOptionPreset
                {
                    ComponentOptionsId = "c454b97a93f341e78b127d5425acc464",
                    PresetsId = "ac1b2b6941c04598ae5a50d48efd8232"
                },
                new ComponentOptionPreset
                {
                    ComponentOptionsId = "06e6899bd6544dd3b9020a50789d9421",
                    PresetsId = "4f4d8b91705f4a9bb95ecf18b57b0306"
                },
                new ComponentOptionPreset
                {
                    ComponentOptionsId = "31c853d3dde94e68ab57903cf8a80798",
                    PresetsId = "4f4d8b91705f4a9bb95ecf18b57b0306"
                },
                new ComponentOptionPreset
                {
                    ComponentOptionsId = "4ed7cdc105ab44de81d333d63db0a222",
                    PresetsId = "4f4d8b91705f4a9bb95ecf18b57b0306"
                },
                new ComponentOptionPreset
                {
                    ComponentOptionsId = "818f875df04a49eebe9c474bba4247b9",
                    PresetsId = "4f4d8b91705f4a9bb95ecf18b57b0306"
                },
                new ComponentOptionPreset
                {
                    ComponentOptionsId = "8cff38afa8464ae191ba152b93438266",
                    PresetsId = "4f4d8b91705f4a9bb95ecf18b57b0306"
                },
                new ComponentOptionPreset
                {
                    ComponentOptionsId = "c454b97a93f341e78b127d5425acc464",
                    PresetsId = "4f4d8b91705f4a9bb95ecf18b57b0306"
                },
                new ComponentOptionPreset
                {
                    ComponentOptionsId = "06e6899bd6544dd3b9020a50789d9421",
                    PresetsId = "694d8135f4ca4238956e9f47460b5dff"
                },
                new ComponentOptionPreset
                {
                    ComponentOptionsId = "31c853d3dde94e68ab57903cf8a80798",
                    PresetsId = "694d8135f4ca4238956e9f47460b5dff"
                },
                new ComponentOptionPreset
                {
                    ComponentOptionsId = "70b69ec5ea074c0f8bf904b20b9c7884",
                    PresetsId = "694d8135f4ca4238956e9f47460b5dff"
                },
                new ComponentOptionPreset
                {
                    ComponentOptionsId = "818f875df04a49eebe9c474bba4247b9",
                    PresetsId = "694d8135f4ca4238956e9f47460b5dff"
                },
                new ComponentOptionPreset
                {
                    ComponentOptionsId = "c454b97a93f341e78b127d5425acc464",
                    PresetsId = "694d8135f4ca4238956e9f47460b5dff"
                },
                new ComponentOptionPreset
                {
                    ComponentOptionsId = "fecd26fe3b6b4e88ba97ebb406fa6df2",
                    PresetsId = "694d8135f4ca4238956e9f47460b5dff"
                },
                new ComponentOptionPreset
                {
                    ComponentOptionsId = "31c853d3dde94e68ab57903cf8a80798",
                    PresetsId = "f1ec1932c78e4a5a8035df1864fb32a4"
                },
                new ComponentOptionPreset
                {
                    ComponentOptionsId = "4ed7cdc105ab44de81d333d63db0a222",
                    PresetsId = "f1ec1932c78e4a5a8035df1864fb32a4"
                },
                new ComponentOptionPreset
                {
                    ComponentOptionsId = "70b69ec5ea074c0f8bf904b20b9c7884",
                    PresetsId = "f1ec1932c78e4a5a8035df1864fb32a4"
                },
                new ComponentOptionPreset
                {
                    ComponentOptionsId = "818f875df04a49eebe9c474bba4247b9",
                    PresetsId = "f1ec1932c78e4a5a8035df1864fb32a4"
                },
                new ComponentOptionPreset
                {
                    ComponentOptionsId = "81acc249e2664c72908014a28e1e72ef",
                    PresetsId = "f1ec1932c78e4a5a8035df1864fb32a4"
                },
                new ComponentOptionPreset
                {
                    ComponentOptionsId = "c454b97a93f341e78b127d5425acc464",
                    PresetsId = "f1ec1932c78e4a5a8035df1864fb32a4"
                },
                new ComponentOptionPreset
                {
                    ComponentOptionsId = "31c853d3dde94e68ab57903cf8a80798",
                    PresetsId = "cc5b36affe2243f2b119fd65cb3434d5"
                },
                new ComponentOptionPreset
                {
                    ComponentOptionsId = "4ed7cdc105ab44de81d333d63db0a222",
                    PresetsId = "cc5b36affe2243f2b119fd65cb3434d5"
                },
                new ComponentOptionPreset
                {
                    ComponentOptionsId = "70b69ec5ea074c0f8bf904b20b9c7884",
                    PresetsId = "cc5b36affe2243f2b119fd65cb3434d5"
                },
                new ComponentOptionPreset
                {
                    ComponentOptionsId = "8158b8e5ef0340bdaf830fa8eb2e650d",
                    PresetsId = "cc5b36affe2243f2b119fd65cb3434d5"
                },
                new ComponentOptionPreset
                {
                    ComponentOptionsId = "818f875df04a49eebe9c474bba4247b9",
                    PresetsId = "cc5b36affe2243f2b119fd65cb3434d5"
                },
                new ComponentOptionPreset
                {
                    ComponentOptionsId = "c454b97a93f341e78b127d5425acc464",
                    PresetsId = "cc5b36affe2243f2b119fd65cb3434d5"
                },
                new ComponentOptionPreset
                {
                    ComponentOptionsId = "06e6899bd6544dd3b9020a50789d9421",
                    PresetsId = "7abc9feb7c1e47778ec32b24aac95985"
                },
                new ComponentOptionPreset
                {
                    ComponentOptionsId = "31c853d3dde94e68ab57903cf8a80798",
                    PresetsId = "7abc9feb7c1e47778ec32b24aac95985"
                },
                new ComponentOptionPreset
                {
                    ComponentOptionsId = "70b69ec5ea074c0f8bf904b20b9c7884",
                    PresetsId = "7abc9feb7c1e47778ec32b24aac95985"
                },
                new ComponentOptionPreset
                {
                    ComponentOptionsId = "78b78938aefe4bc6b927ed3722bc5859",
                    PresetsId = "7abc9feb7c1e47778ec32b24aac95985"
                },
                new ComponentOptionPreset
                {
                    ComponentOptionsId = "818f875df04a49eebe9c474bba4247b9",
                    PresetsId = "7abc9feb7c1e47778ec32b24aac95985"
                },
                new ComponentOptionPreset
                {
                    ComponentOptionsId = "c454b97a93f341e78b127d5425acc464",
                    PresetsId = "7abc9feb7c1e47778ec32b24aac95985"
                },
                new ComponentOptionPreset
                {
                    ComponentOptionsId = "31c853d3dde94e68ab57903cf8a80798",
                    PresetsId = "380e3d771eac4488ad94b09c9e77ccd2"
                },
                new ComponentOptionPreset
                {
                    ComponentOptionsId = "70b69ec5ea074c0f8bf904b20b9c7884",
                    PresetsId = "380e3d771eac4488ad94b09c9e77ccd2"
                },
                new ComponentOptionPreset
                {
                    ComponentOptionsId = "78b78938aefe4bc6b927ed3722bc5859",
                    PresetsId = "380e3d771eac4488ad94b09c9e77ccd2"
                },
                new ComponentOptionPreset
                {
                    ComponentOptionsId = "818f875df04a49eebe9c474bba4247b9",
                    PresetsId = "380e3d771eac4488ad94b09c9e77ccd2"
                },
                new ComponentOptionPreset
                {
                    ComponentOptionsId = "81acc249e2664c72908014a28e1e72ef",
                    PresetsId = "380e3d771eac4488ad94b09c9e77ccd2"
                },
                new ComponentOptionPreset
                {
                    ComponentOptionsId = "c454b97a93f341e78b127d5425acc464",
                    PresetsId = "380e3d771eac4488ad94b09c9e77ccd2"
                },
                new ComponentOptionPreset
                {
                    ComponentOptionsId = "31c853d3dde94e68ab57903cf8a80798",
                    PresetsId = "1acd7ac56e5c4d15895f03cc2d6e7723"
                },
                new ComponentOptionPreset
                {
                    ComponentOptionsId = "70b69ec5ea074c0f8bf904b20b9c7884",
                    PresetsId = "1acd7ac56e5c4d15895f03cc2d6e7723"
                },
                new ComponentOptionPreset
                {
                    ComponentOptionsId = "78b78938aefe4bc6b927ed3722bc5859",
                    PresetsId = "1acd7ac56e5c4d15895f03cc2d6e7723"
                },
                new ComponentOptionPreset
                {
                    ComponentOptionsId = "8158b8e5ef0340bdaf830fa8eb2e650d",
                    PresetsId = "1acd7ac56e5c4d15895f03cc2d6e7723"
                },
                new ComponentOptionPreset
                {
                    ComponentOptionsId = "818f875df04a49eebe9c474bba4247b9",
                    PresetsId = "1acd7ac56e5c4d15895f03cc2d6e7723"
                },
                new ComponentOptionPreset
                {
                    ComponentOptionsId = "c454b97a93f341e78b127d5425acc464",
                    PresetsId = "1acd7ac56e5c4d15895f03cc2d6e7723"
                },
                new ComponentOptionPreset
                {
                    ComponentOptionsId = "31c853d3dde94e68ab57903cf8a80798",
                    PresetsId = "50959c7c201e45a9b5255558ac9dfcf7"
                },
                new ComponentOptionPreset
                {
                    ComponentOptionsId = "70b69ec5ea074c0f8bf904b20b9c7884",
                    PresetsId = "50959c7c201e45a9b5255558ac9dfcf7"
                },
                new ComponentOptionPreset
                {
                    ComponentOptionsId = "818f875df04a49eebe9c474bba4247b9",
                    PresetsId = "50959c7c201e45a9b5255558ac9dfcf7"
                },
                new ComponentOptionPreset
                {
                    ComponentOptionsId = "81acc249e2664c72908014a28e1e72ef",
                    PresetsId = "50959c7c201e45a9b5255558ac9dfcf7"
                },
                new ComponentOptionPreset
                {
                    ComponentOptionsId = "c454b97a93f341e78b127d5425acc464",
                    PresetsId = "50959c7c201e45a9b5255558ac9dfcf7"
                },
                new ComponentOptionPreset
                {
                    ComponentOptionsId = "fecd26fe3b6b4e88ba97ebb406fa6df2",
                    PresetsId = "50959c7c201e45a9b5255558ac9dfcf7"
                },
                new ComponentOptionPreset
                {
                    ComponentOptionsId = "31c853d3dde94e68ab57903cf8a80798",
                    PresetsId = "5a5081ce720141188c09336e4a936ef9"
                },
                new ComponentOptionPreset
                {
                    ComponentOptionsId = "4ed7cdc105ab44de81d333d63db0a222",
                    PresetsId = "5a5081ce720141188c09336e4a936ef9"
                },
                new ComponentOptionPreset
                {
                    ComponentOptionsId = "8158b8e5ef0340bdaf830fa8eb2e650d",
                    PresetsId = "5a5081ce720141188c09336e4a936ef9"
                },
                new ComponentOptionPreset
                {
                    ComponentOptionsId = "8cff38afa8464ae191ba152b93438266",
                    PresetsId = "5a5081ce720141188c09336e4a936ef9"
                },
                new ComponentOptionPreset
                {
                    ComponentOptionsId = "92bdbbdbfd3b4dcf8256b68a66dfa35b",
                    PresetsId = "5a5081ce720141188c09336e4a936ef9"
                },
                new ComponentOptionPreset
                {
                    ComponentOptionsId = "c454b97a93f341e78b127d5425acc464",
                    PresetsId = "5a5081ce720141188c09336e4a936ef9"
                },
                new ComponentOptionPreset
                {
                    ComponentOptionsId = "06e6899bd6544dd3b9020a50789d9421",
                    PresetsId = "fe7043a37995453db2d03741e81da9df"
                },
                new ComponentOptionPreset
                {
                    ComponentOptionsId = "31c853d3dde94e68ab57903cf8a80798",
                    PresetsId = "fe7043a37995453db2d03741e81da9df"
                },
                new ComponentOptionPreset
                {
                    ComponentOptionsId = "8cff38afa8464ae191ba152b93438266",
                    PresetsId = "fe7043a37995453db2d03741e81da9df"
                },
                new ComponentOptionPreset
                {
                    ComponentOptionsId = "92bdbbdbfd3b4dcf8256b68a66dfa35b",
                    PresetsId = "fe7043a37995453db2d03741e81da9df"
                },
                new ComponentOptionPreset
                {
                    ComponentOptionsId = "c454b97a93f341e78b127d5425acc464",
                    PresetsId = "fe7043a37995453db2d03741e81da9df"
                },
                new ComponentOptionPreset
                {
                    ComponentOptionsId = "fecd26fe3b6b4e88ba97ebb406fa6df2",
                    PresetsId = "fe7043a37995453db2d03741e81da9df"
                },
                new ComponentOptionPreset
                {
                    ComponentOptionsId = "31c853d3dde94e68ab57903cf8a80798",
                    PresetsId = "7faa6eb739d548bf9f9ce7d3de978d3d"
                },
                new ComponentOptionPreset
                {
                    ComponentOptionsId = "8158b8e5ef0340bdaf830fa8eb2e650d",
                    PresetsId = "7faa6eb739d548bf9f9ce7d3de978d3d"
                },
                new ComponentOptionPreset
                {
                    ComponentOptionsId = "818f875df04a49eebe9c474bba4247b9",
                    PresetsId = "7faa6eb739d548bf9f9ce7d3de978d3d"
                },
                new ComponentOptionPreset
                {
                    ComponentOptionsId = "8cff38afa8464ae191ba152b93438266",
                    PresetsId = "7faa6eb739d548bf9f9ce7d3de978d3d"
                },
                new ComponentOptionPreset
                {
                    ComponentOptionsId = "c454b97a93f341e78b127d5425acc464",
                    PresetsId = "7faa6eb739d548bf9f9ce7d3de978d3d"
                },
                new ComponentOptionPreset
                {
                    ComponentOptionsId = "fecd26fe3b6b4e88ba97ebb406fa6df2",
                    PresetsId = "7faa6eb739d548bf9f9ce7d3de978d3d"
                },
                new ComponentOptionPreset
                {
                    ComponentOptionsId = "31c853d3dde94e68ab57903cf8a80798",
                    PresetsId = "caa36b99b068411aaa62b0f4234c1656"
                },
                new ComponentOptionPreset
                {
                    ComponentOptionsId = "4ed7cdc105ab44de81d333d63db0a222",
                    PresetsId = "caa36b99b068411aaa62b0f4234c1656"
                },
                new ComponentOptionPreset
                {
                    ComponentOptionsId = "818f875df04a49eebe9c474bba4247b9",
                    PresetsId = "caa36b99b068411aaa62b0f4234c1656"
                },
                new ComponentOptionPreset
                {
                    ComponentOptionsId = "81acc249e2664c72908014a28e1e72ef",
                    PresetsId = "caa36b99b068411aaa62b0f4234c1656"
                },
                new ComponentOptionPreset
                {
                    ComponentOptionsId = "8cff38afa8464ae191ba152b93438266",
                    PresetsId = "caa36b99b068411aaa62b0f4234c1656"
                },
                new ComponentOptionPreset
                {
                    ComponentOptionsId = "c454b97a93f341e78b127d5425acc464",
                    PresetsId = "caa36b99b068411aaa62b0f4234c1656"
                },
                new ComponentOptionPreset
                {
                    ComponentOptionsId = "06e6899bd6544dd3b9020a50789d9421",
                    PresetsId = "68ee38a6554f407abd932d492719a834"
                },
                new ComponentOptionPreset
                {
                    ComponentOptionsId = "31c853d3dde94e68ab57903cf8a80798",
                    PresetsId = "68ee38a6554f407abd932d492719a834"
                },
                new ComponentOptionPreset
                {
                    ComponentOptionsId = "818f875df04a49eebe9c474bba4247b9",
                    PresetsId = "68ee38a6554f407abd932d492719a834"
                },
                new ComponentOptionPreset
                {
                    ComponentOptionsId = "8cff38afa8464ae191ba152b93438266",
                    PresetsId = "68ee38a6554f407abd932d492719a834"
                },
                new ComponentOptionPreset
                {
                    ComponentOptionsId = "c454b97a93f341e78b127d5425acc464",
                    PresetsId = "68ee38a6554f407abd932d492719a834"
                },
                new ComponentOptionPreset
                {
                    ComponentOptionsId = "fecd26fe3b6b4e88ba97ebb406fa6df2",
                    PresetsId = "68ee38a6554f407abd932d492719a834"
                },
                new ComponentOptionPreset
                {
                    ComponentOptionsId = "31c853d3dde94e68ab57903cf8a80798",
                    PresetsId = "bac18c4350314174b9f4cbe73ca0b24d"
                },
                new ComponentOptionPreset
                {
                    ComponentOptionsId = "4ed7cdc105ab44de81d333d63db0a222",
                    PresetsId = "bac18c4350314174b9f4cbe73ca0b24d"
                },
                new ComponentOptionPreset
                {
                    ComponentOptionsId = "8158b8e5ef0340bdaf830fa8eb2e650d",
                    PresetsId = "bac18c4350314174b9f4cbe73ca0b24d"
                },
                new ComponentOptionPreset
                {
                    ComponentOptionsId = "818f875df04a49eebe9c474bba4247b9",
                    PresetsId = "bac18c4350314174b9f4cbe73ca0b24d"
                },
                new ComponentOptionPreset
                {
                    ComponentOptionsId = "8cff38afa8464ae191ba152b93438266",
                    PresetsId = "bac18c4350314174b9f4cbe73ca0b24d"
                },
                new ComponentOptionPreset
                {
                    ComponentOptionsId = "c454b97a93f341e78b127d5425acc464",
                    PresetsId = "bac18c4350314174b9f4cbe73ca0b24d"
                },
                new ComponentOptionPreset
                {
                    ComponentOptionsId = "31c853d3dde94e68ab57903cf8a80798",
                    PresetsId = "68fca8f086454898ae81b1ea29890b60"
                },
                new ComponentOptionPreset
                {
                    ComponentOptionsId = "4ed7cdc105ab44de81d333d63db0a222",
                    PresetsId = "68fca8f086454898ae81b1ea29890b60"
                },
                new ComponentOptionPreset
                {
                    ComponentOptionsId = "81acc249e2664c72908014a28e1e72ef",
                    PresetsId = "68fca8f086454898ae81b1ea29890b60"
                },
                new ComponentOptionPreset
                {
                    ComponentOptionsId = "8cff38afa8464ae191ba152b93438266",
                    PresetsId = "68fca8f086454898ae81b1ea29890b60"
                },
                new ComponentOptionPreset
                {
                    ComponentOptionsId = "92bdbbdbfd3b4dcf8256b68a66dfa35b",
                    PresetsId = "68fca8f086454898ae81b1ea29890b60"
                },
                new ComponentOptionPreset
                {
                    ComponentOptionsId = "c454b97a93f341e78b127d5425acc464",
                    PresetsId = "68fca8f086454898ae81b1ea29890b60"
                }
            };
            modelBuilder.Entity<ComponentOptionPreset>().HasData(componentoptionpresets);

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
                    MinimumOrderValue = 100000.0f,
                    MaximumDiscountValue = 50000.0f,
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
                    MinimumOrderValue = 50000.0f,
                    MaximumDiscountValue = 10000.0f,
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
                    MinimumOrderValue = 30000.0f,
                    MaximumDiscountValue = 25000.0f,
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
                    MinimumOrderValue = 0.0f,
                    MaximumDiscountValue = 5000.0f,
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
                    MinimumOrderValue = 150000.0f,
                    MaximumDiscountValue = 100000.0f,
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
                        "{https://firebasestorage.googleapis.com/v0/b/mamafit-e0138.firebasestorage.app/o/images%2Fdownload_1753254774304_m5fg15.jpeg?alt=media&token=02b0e1ee-3126-4c80-ad66-59aea3fdfd70}"
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
                        "{https://firebasestorage.googleapis.com/v0/b/mamafit-e0138.firebasestorage.app/o/images%2Fdownload_1753254877456_iubawj.jpeg?alt=media&token=e343f866-8c6b-4fa3-b5fa-17026bed0922}"
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
                    Name = "In Warrnaty",
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
                    Id = "5110afc434844342984a1db6944536dc",
                    Name = "Warranty Check",
                    Description = "Initial warranty assessment for preset orders",
                    ApplyFor = [ItemType.WARRANTY],
                    SequenceOrder = 3,
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
                    ApplyFor = [ItemType.PRESET, ItemType.READY_TO_BUY],
                    SequenceOrder = 7,
                    CreatedBy = "Admin",
                    UpdatedBy = "Admin",
                    CreatedAt = new DateTime(2025, 7, 23, 14, 17, 54, DateTimeKind.Utc),
                    UpdatedAt = new DateTime(2025, 7, 23, 14, 17, 54, DateTimeKind.Utc),
                    IsDeleted = false
                },
                new Milestone
                {
                    Id = "e2c7f3d2a1b345ef8a92c1f0b5d7e4a6",
                    Name = "Waiting For Delivery",
                    Description = "Awaiting delivery order creation",
                    ApplyFor = [ItemType.PRESET, ItemType.READY_TO_BUY],
                    SequenceOrder = 8,
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
                    CreatedBy = "System",
                    UpdatedBy = "System",
                    CreatedAt = new DateTime(2025, 7, 23, 14, 17, 54, DateTimeKind.Utc),
                    UpdatedAt = new DateTime(2025, 7, 23, 14, 17, 54, DateTimeKind.Utc),
                    IsDeleted = false
                },
                new MaternityDressTask
                {
                    Id = "16a506ee77974fdaac4eeb2310f56ff0",
                    MilestoneId = "5110afc434844342984a1db6944536dc",
                    Name = "Inspect Returned Item",
                    Description = "Examine returned garment for reported issues",
                    SequenceOrder = 1,
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
                    CreatedBy = "System",
                    UpdatedBy = "System",
                    CreatedAt = new DateTime(2025, 7, 23, 14, 17, 54, DateTimeKind.Utc),
                    UpdatedAt = new DateTime(2025, 7, 23, 14, 17, 54, DateTimeKind.Utc),
                    IsDeleted = false
                },
                new MaternityDressTask
                {
                    Id = "efcc1a38a9cd4fd38a68bc18437f726b",
                    MilestoneId = "5110afc434844342984a1db6944536dc",
                    Name = "Assess Warranty Validity",
                    Description = "Determine if issue is covered under warranty terms",
                    SequenceOrder = 2,
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
                    CreatedBy = "System",
                    UpdatedBy = "System",
                    CreatedAt = new DateTime(2025, 7, 23, 14, 17, 54, DateTimeKind.Utc),
                    UpdatedAt = new DateTime(2025, 7, 23, 14, 17, 54, DateTimeKind.Utc),
                    IsDeleted = false
                },
                new MaternityDressTask
                {
                    Id = "c9a4e7d8f5b342e1ad03b8c7f6a1e2d4",
                    MilestoneId = "e2c7f3d2a1b345ef8a92c1f0b5d7e4a6",
                    Name = "Create Delivery Order",
                    Description = "Initiate and submit the delivery order for processing",
                    SequenceOrder = 1,
                    CreatedBy = "Admin",
                    UpdatedBy = "Admin",
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
                    CreatedBy = "Admin",
                    UpdatedBy = "Admin",
                    CreatedAt = new DateTime(2025, 7, 23, 14, 17, 54, DateTimeKind.Utc),
                    UpdatedAt = new DateTime(2025, 7, 23, 14, 17, 54, DateTimeKind.Utc),
                    IsDeleted = false
                });

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
                    Type = 0,
                    Code = "O75387",
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
                    CreatedBy = "User",
                    UpdatedBy = null,
                    CreatedAt = new DateTime(2025, 8, 3, 6, 19, 30, DateTimeKind.Utc),
                    UpdatedAt = new DateTime(2025, 8, 3, 6, 19, 30, DateTimeKind.Utc),
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
                    Code = "O71956",
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
                    Code = "O89007",
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
                    Code = "O84288",
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
                    Type = 0,
                    Code = "O47111",
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
                    Code = "O46002",
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
                    DeliveryMethod = 0,
                    PaymentType = PaymentType.FULL,
                    CanceledAt = null,
                    CanceledReason = null,
                    SubTotalAmount = 4000.0m,
                    WarrantyCode = null,
                    ReceivedAt = null,
                    CreatedBy = "User",
                    UpdatedBy = "User",
                    CreatedAt = new DateTime(2025, 8, 9, 10, 3, 21, 665369, DateTimeKind.Utc),
                    UpdatedAt = new DateTime(2025, 8, 9, 10, 4, 19, 224667, DateTimeKind.Utc),
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
                    Id = "71f71d08ac434f1683ae64292fd7c5a5",
                    OrderId = "4e0842a05e594525a25de04b1e79f4a8",
                    ParentOrderItemId = null,
                    MaternityDressDetailId = null,
                    PresetId = "bcdcf89795b2484f96a3a12b25383a12",
                    ItemType = (ItemType)1,
                    Price = 2000m,
                    Quantity = 1,
                    WarrantyDate = new DateTime(2025, 8, 9, 10, 7, 38, 212, DateTimeKind.Utc),
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
                    WarrantyDate = new DateTime(2025, 8, 9, 10, 22, 31, 529, DateTimeKind.Utc),
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