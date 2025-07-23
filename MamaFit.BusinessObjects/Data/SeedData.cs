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
            var categorys = new List<Category> {
    new Category {
        Id = "04a3b452cfd841919b4aed099c28d709",
        Name = "Work/Office",
        Description = "Thiết kế trang nhã dành cho công sở.",
        Images = new List<string> { "https://example.com/images/work.jpg" },
        CreatedBy = "System", UpdatedBy = "System", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow
    },
    new Category {
        Id = "1946de6edbc24354bdd82b7c2c2c4cb2",
        Name = "Party/Special",
        Description = "Dành cho buổi tiệc hoặc dịp đặc biệt.",
        Images = new List<string> { "https://example.com/images/party.jpg" },
        CreatedBy = "System", UpdatedBy = "System", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow
    },
    new Category {
        Id = "4f8c9fd3fb4548af8ca11c6521aa3b33",
        Name = "Casual/Everyday",
        Description = "Phong cách thường ngày, thoải mái.",
        Images = new List<string> { "https://example.com/images/casual.jpg" },
        CreatedBy = "System", UpdatedBy = "System", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow
    },
    new Category {
        Id = "a7a75e41c1a64b4498a81f4b76029a5a",
        Name = "Beach/Resort",
        Description = "Trang phục phù hợp khi đi biển hoặc nghỉ dưỡng.",
        Images = new List<string> { "https://example.com/images/beach.jpg" },
        CreatedBy = "System", UpdatedBy = "System", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow
    },
    new Category {
        Id = "c6a7e90156d44da1ab26f259313e4a0b",
        Name = "Formal/Event",
        Description = "Phù hợp với các sự kiện trang trọng.",
        Images = new List<string> { "https://example.com/images/formal.jpg" },
        CreatedBy = "System", UpdatedBy = "System", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow
    }
};
            modelBuilder.Entity<Category>().HasData(categorys);

            // ===== STYLES =====
            var styles = new List<Style> {
    new Style {
        Id = "037b41302a4c453f8cef135afceb3956",
        Name = "Maxi",
        Description = "Đầm dài maxi, thích hợp đi biển hoặc dạo phố.",
        Images = new List<string> { "https://example.com/images/maxi.jpg" },
        IsCustom = false,
        CategoryId = "a7a75e41c1a64b4498a81f4b76029a5a",
        CreatedBy = "System", UpdatedBy = "System", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow
    }
};
            modelBuilder.Entity<Style>().HasData(styles);

            // ===== COMPONENTS =====
            var components = new List<Component> {
    new Component {
        Id = "0ec808c3046b4d368962c65eb771dea0",
        Name = "Color",
        Description = "Màu sắc.",
        Images = new List<string> { "https://example.com/images/color.jpg" },
        CreatedBy = "System", UpdatedBy = "System", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow
    },
    new Component {
        Id = "3b83e5e47b444880aa9495b58b38a2fd",
        Name = "Sleeves",
        Description = "Kiểu tay áo.",
        Images = new List<string> { "https://example.com/images/sleeves.jpg" },
        CreatedBy = "System", UpdatedBy = "System", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow
    },
    new Component {
        Id = "3e9f3d9beeff47f1977a6713cffe781e",
        Name = "Waist",
        Description = "Kiểu eo.",
        Images = new List<string> { "https://example.com/images/waist.jpg" },
        CreatedBy = "System", UpdatedBy = "System", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow
    },
    new Component {
        Id = "b66ab857cd93410599bd14b79ae37147",
        Name = "Neckline",
        Description = "Kiểu dáng cổ áo.",
        Images = new List<string> { "https://example.com/images/neckline.jpg" },
        CreatedBy = "System", UpdatedBy = "System", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow
    },
    new Component {
        Id = "c615996abfe24d3885ccea361a199dba",
        Name = "Fabric",
        Description = "Chất liệu vải.",
        Images = new List<string> { "https://example.com/images/fabric.jpg" },
        CreatedBy = "System", UpdatedBy = "System", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow
    },
    new Component {
        Id = "f00fcaf79a86427c995daa1fd0915d8e",
        Name = "Hem",
        Description = "Đường viền gấu váy.",
        Images = new List<string> { "https://example.com/images/hem.jpg" },
        CreatedBy = "System", UpdatedBy = "System", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow
    }
};
            modelBuilder.Entity<Component>().HasData(components);

            // ===== COMPONENT OPTIONS =====
            var componentoptions = new List<ComponentOption> {
    new ComponentOption {
        Id = "06e6899bd6544dd3b9020a50789d9421",
        Name = "Normal Waist",
        ComponentId = "3e9f3d9beeff47f1977a6713cffe781e",
        Price = 18000,
        Description = "Thiết kế eo trung bình truyền thống.",
        Images = new List<string> { "https://example.com/images/normalwaist.jpg" },

        CreatedBy = "System", UpdatedBy = "System", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow
    },
    new ComponentOption {
        Id = "31c853d3dde94e68ab57903cf8a80798",
        Name = "Cotton",
        ComponentId = "c615996abfe24d3885ccea361a199dba",
        Price = 9000,
        Description = "Vải cotton thoáng mát, thấm hút tốt.",
        Images = new List<string> { "https://example.com/images/cotton.jpg" },

        CreatedBy = "System", UpdatedBy = "System", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow
    },
    new ComponentOption {
        Id = "4ed7cdc105ab44de81d333d63db0a222",
        Name = "No Sleeves",
        ComponentId = "3b83e5e47b444880aa9495b58b38a2fd",
        Price = 10000,
        Description = "Không tay, thoáng mát.",
        Images = new List<string> { "https://example.com/images/nosleeves.jpg" },

        CreatedBy = "System", UpdatedBy = "System", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow
    },
    new ComponentOption {
        Id = "66645e2f8fa94d228d87450cc4a74706",
        Name = "Emerald Green",
        ComponentId = "0ec808c3046b4d368962c65eb771dea0",
        Price = 10000,
        Description = "Xanh ngọc quý phái, nổi bật.",
        Images = new List<string> { "https://example.com/images/emerald.jpg" },

        CreatedBy = "System", UpdatedBy = "System", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow
    },
    new ComponentOption {
        Id = "70b69ec5ea074c0f8bf904b20b9c7884",
        Name = "High-Low Hem",
        ComponentId = "f00fcaf79a86427c995daa1fd0915d8e",
        Price = 16000,
        Description = "Trước ngắn sau dài tạo nét lạ mắt.",
        Images = new List<string> { "https://example.com/images/highlow.jpg" },

        CreatedBy = "System", UpdatedBy = "System", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow
    },
    new ComponentOption {
        Id = "78b78938aefe4bc6b927ed3722bc5859",
        Name = "Spaghetti Strap",
        ComponentId = "3b83e5e47b444880aa9495b58b38a2fd",
        Price = 12000,
        Description = "Dây mảnh, nữ tính.",
        Images = new List<string> { "https://example.com/images/spaghetti.jpg" },

        CreatedBy = "System", UpdatedBy = "System", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow
    },
    new ComponentOption {
        Id = "8158b8e5ef0340bdaf830fa8eb2e650d",
        Name = "Empire Waist",
        ComponentId = "3e9f3d9beeff47f1977a6713cffe781e",
        Price = 20000,
        Description = "Eo cao, phù hợp cho phong cách nữ tính.",
        Images = new List<string> { "https://example.com/images/empirewaist.jpg" },

        CreatedBy = "System", UpdatedBy = "System", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow
    },
    new ComponentOption {
        Id = "818f875df04a49eebe9c474bba4247b9",
        Name = "V Neck",
        ComponentId = "b66ab857cd93410599bd14b79ae37147",
        Price = 25000,
        Description = "Cổ chữ V tạo cảm giác cổ dài hơn.",
        Images = new List<string> { "https://example.com/images/vneck.jpg" },

        CreatedBy = "System", UpdatedBy = "System", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow
    },
    new ComponentOption {
        Id = "81acc249e2664c72908014a28e1e72ef",
        Name = "Bow Tie Waist",
        ComponentId = "3e9f3d9beeff47f1977a6713cffe781e",
        Price = 22000,
        Description = "Nơ thắt eo tạo điểm nhấn đáng yêu.",
        Images = new List<string> { "https://example.com/images/bowtie.jpg" },

        CreatedBy = "System", UpdatedBy = "System", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow
    },
    new ComponentOption {
        Id = "8cff38afa8464ae191ba152b93438266",
        Name = "Single Layer Hem",
        ComponentId = "f00fcaf79a86427c995daa1fd0915d8e",
        Price = 12000,
        Description = "Gấu váy đơn giản, thanh lịch.",
        Images = new List<string> { "https://example.com/images/singlelayer.jpg" },

        CreatedBy = "System", UpdatedBy = "System", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow
    },
    new ComponentOption {
        Id = "92bdbbdbfd3b4dcf8256b68a66dfa35b",
        Name = "Round Neck",
        ComponentId = "b66ab857cd93410599bd14b79ae37147",
        Price = 20000,
        Description = "Cổ tròn cơ bản, thanh lịch.",
        Images = new List<string> { "https://example.com/images/roundneck.jpg" },

        CreatedBy = "System", UpdatedBy = "System", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow
    },
    new ComponentOption {
        Id = "c454b97a93f341e78b127d5425acc464",
        Name = "Red",
        ComponentId = "0ec808c3046b4d368962c65eb771dea0",
        Price = 10000,
        Description = "Màu đỏ nổi bật, quyến rũ.",
        Images = new List<string> { "https://example.com/images/red.jpg" },

        CreatedBy = "System", UpdatedBy = "System", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow
    },
    new ComponentOption {
        Id = "e2c03da33b12458694a1b01c3ec66474",
        Name = "Navy Blue",
        ComponentId = "0ec808c3046b4d368962c65eb771dea0",
        Price = 10000,
        Description = "Xanh navy lịch sự, thanh nhã.",
        Images = new List<string> { "https://example.com/images/navyblue.jpg" },

        CreatedBy = "System", UpdatedBy = "System", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow
    },
    new ComponentOption {
        Id = "fecd26fe3b6b4e88ba97ebb406fa6df2",
        Name = "Flattered Short Sleeves",
        ComponentId = "3b83e5e47b444880aa9495b58b38a2fd",
        Price = 15000,
        Description = "Tay ngắn bay bổng, thoải mái.",
        Images = new List<string> { "https://example.com/images/shortsleeves.jpg" },

        CreatedBy = "System", UpdatedBy = "System", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow
    }
};
            modelBuilder.Entity<ComponentOption>().HasData(componentoptions);

            // ===== PRESETS =====
            var presets = new List<Preset> {
    new Preset {
        Id = "fe88b53dce4c4938a7785fb9e3db1f0a",
        UserId = "f49aa51bbd304e77933e24bbed65b165",
        StyleId = "037b41302a4c453f8cef135afceb3956",
                Name = "Name",
        Weight = 200,
        Images = new List<string> { "https://firebasestorage.googleapis.com/v0/b/mamafit-e0138.firebasestorage.app/o/presets%2Fmaxi%2Fmaxi.png?alt=media&token=da59cd6d-df28-4605-a464-88951676e0fd" },
        IsDefault = true,
        Price = 2000,
        Type = 0,
        CreatedBy = "User",
        UpdatedBy = "System",
        CreatedAt = DateTime.UtcNow,
        UpdatedAt = DateTime.UtcNow
    },
    new Preset {
        Id = "7ae7818fdf09466f819808beed30e707",
        UserId = "f49aa51bbd304e77933e24bbed65b165",
        StyleId = "037b41302a4c453f8cef135afceb3956",
                Name = "Name",
        Weight = 200,
        Images = new List<string> { "https://firebasestorage.googleapis.com/v0/b/mamafit-e0138.firebasestorage.app/o/presets%2Fmaxi%2Fmaxi-11.png?alt=media&token=14bc30f9-fa14-41ae-8bfa-cc4b94acda1b" },
        IsDefault = false,
        Price = 2000,
        Type = 0,
        CreatedBy = "User",
        UpdatedBy = "System",
        CreatedAt = DateTime.UtcNow,
        UpdatedAt = DateTime.UtcNow
    },
    new Preset {
        Id = "11230115583c4994bfaa3925951d3817",
        UserId = "f49aa51bbd304e77933e24bbed65b165",
        StyleId = "037b41302a4c453f8cef135afceb3956",
                Name = "Name",
        Weight = 200,
        Images = new List<string> { "https://firebasestorage.googleapis.com/v0/b/mamafit-e0138.firebasestorage.app/o/presets%2Fmaxi%2Fmaxi-19.png?alt=media&token=053f4feb-7e75-4645-b8aa-e0e0b1fae0b3" },
        IsDefault = false,
        Price = 2000,
        Type = 0,
        CreatedBy = "User",
        UpdatedBy = "System",
        CreatedAt = DateTime.UtcNow,
        UpdatedAt = DateTime.UtcNow
    },
    new Preset {
        Id = "33c8177630a54906b92c0b8c03c57450",
        UserId = "f49aa51bbd304e77933e24bbed65b165",
        StyleId = "037b41302a4c453f8cef135afceb3956",
                Name = "Name",
        Weight = 200,
        Images = new List<string> { "https://firebasestorage.googleapis.com/v0/b/mamafit-e0138.firebasestorage.app/o/presets%2Fmaxi%2Fmaxi-18.png?alt=media&token=fe931377-e708-4586-b897-4bdef49a0d5f" },
        IsDefault = false,
        Price = 2000,
        Type = 0,
        CreatedBy = "User",
        UpdatedBy = "System",
        CreatedAt = DateTime.UtcNow,
        UpdatedAt = DateTime.UtcNow
    },
    new Preset {
        Id = "c9348ab0418743d08ee786e31083d76b",
        UserId = "f49aa51bbd304e77933e24bbed65b165",
        StyleId = "037b41302a4c453f8cef135afceb3956",
                Name = "Name",
        Weight = 200,
        Images = new List<string> { "https://firebasestorage.googleapis.com/v0/b/mamafit-e0138.firebasestorage.app/o/presets%2Fmaxi%2Fmaxi-17.png?alt=media&token=f90d9acc-504a-496b-8c4d-eb35b2a05fb0" },
        IsDefault = false,
        Price = 2000,
        Type = 0,
        CreatedBy = "User",
        UpdatedBy = "System",
        CreatedAt = DateTime.UtcNow,
        UpdatedAt = DateTime.UtcNow
    },
    new Preset {
        Id = "41e4b32655a44dca862141df8099f646",
        UserId = "f49aa51bbd304e77933e24bbed65b165",
        StyleId = "037b41302a4c453f8cef135afceb3956",
                Name = "Name",
        Weight = 200,
        Images = new List<string> { "https://firebasestorage.googleapis.com/v0/b/mamafit-e0138.firebasestorage.app/o/presets%2Fmaxi%2Fmaxi-16.png?alt=media&token=1d05c179-85bd-444e-9056-be2509352f8c" },
        IsDefault = false,
        Price = 2000,
        Type = 0,
        CreatedBy = "User",
        UpdatedBy = "System",
        CreatedAt = DateTime.UtcNow,
        UpdatedAt = DateTime.UtcNow
    },
    new Preset {
        Id = "bcdcf89795b2484f96a3a12b25383a12",
        UserId = "f49aa51bbd304e77933e24bbed65b165",
        StyleId = "037b41302a4c453f8cef135afceb3956",
        Name = "Name",
        Weight = 200,
        Images = new List<string> { "https://firebasestorage.googleapis.com/v0/b/mamafit-e0138.firebasestorage.app/o/presets%2Fmaxi%2Fmaxi-15.png?alt=media&token=a4a6b9e1-b20a-4939-bf9f-524f0c5911ae" },
        IsDefault = false,
        Price = 2000,
        Type = 0,
        CreatedBy = "User",
        UpdatedBy = "System",
        CreatedAt = DateTime.UtcNow,
        UpdatedAt = DateTime.UtcNow
    },
    new Preset {
        Id = "5ef79dda17e64808afa6287c07c8bac9",
        UserId = "f49aa51bbd304e77933e24bbed65b165",
        StyleId = "037b41302a4c453f8cef135afceb3956",
                Name = "Name",
        Weight = 200,
        Images = new List<string> { "https://firebasestorage.googleapis.com/v0/b/mamafit-e0138.firebasestorage.app/o/presets%2Fmaxi%2Fmaxi-14.png?alt=media&token=77ec4929-09f7-427c-9714-9f287ac89f4c" },
        IsDefault = false,
        Price = 2000,
        Type = 0,
        CreatedBy = "User",
        UpdatedBy = "System",
        CreatedAt = DateTime.UtcNow,
        UpdatedAt = DateTime.UtcNow
    },
    new Preset {
        Id = "3583422ceef647d28a14b82680058ff0",
        UserId = "f49aa51bbd304e77933e24bbed65b165",
        StyleId = "037b41302a4c453f8cef135afceb3956",
                Name = "Name",
        Weight = 200,
        Images = new List<string> { "https://firebasestorage.googleapis.com/v0/b/mamafit-e0138.firebasestorage.app/o/presets%2Fmaxi%2Fmaxi-13.png?alt=media&token=fdf74485-b0af-4c32-b7d3-cc78beb9dcd7" },
        IsDefault = false,
        Price = 2000,
        Type = 0,
        CreatedBy = "User",
        UpdatedBy = "System",
        CreatedAt = DateTime.UtcNow,
        UpdatedAt = DateTime.UtcNow
    },
    new Preset {
        Id = "6ab0daed7fe54e9fa791bfcb6bf1a043",
        UserId = "f49aa51bbd304e77933e24bbed65b165",
        StyleId = "037b41302a4c453f8cef135afceb3956",
                Name = "Name",
        Weight = 200,
        Images = new List<string> { "https://firebasestorage.googleapis.com/v0/b/mamafit-e0138.firebasestorage.app/o/presets%2Fmaxi%2Fmaxi-12.png?alt=media&token=72e10ea4-1d5c-4f50-b614-b8c56eed7cfb" },
        IsDefault = false,
        Price = 2000,
        Type = 0,
        CreatedBy = "User",
        UpdatedBy = "System",
        CreatedAt = DateTime.UtcNow,
        UpdatedAt = DateTime.UtcNow
    },
    new Preset {
        Id = "ccc662f99d7a41d7aa1505e3a2cb8950",
        UserId = "f49aa51bbd304e77933e24bbed65b165",
        StyleId = "037b41302a4c453f8cef135afceb3956",
                Name = "Name",
        Weight = 200,
        Images = new List<string> { "https://firebasestorage.googleapis.com/v0/b/mamafit-e0138.firebasestorage.app/o/presets%2Fmaxi%2Fmaxi-29.png?alt=media&token=25bc32d6-d2b7-4881-8ead-38dbad9096b6" },
        IsDefault = false,
        Price = 2000,
        Type = 0,
        CreatedBy = "User",
        UpdatedBy = "System",
        CreatedAt = DateTime.UtcNow,
        UpdatedAt = DateTime.UtcNow
    },
    new Preset {
        Id = "49e11d4698f643dca14364eed4d9c239",
        UserId = "f49aa51bbd304e77933e24bbed65b165",
        StyleId = "037b41302a4c453f8cef135afceb3956",
                Name = "Name",
        Weight = 200,
        Images = new List<string> { "https://firebasestorage.googleapis.com/v0/b/mamafit-e0138.firebasestorage.app/o/presets%2Fmaxi%2Fmaxi-28.png?alt=media&token=6276c8c2-679d-48fc-a4ec-214ce17fe3b9" },
        IsDefault = false,
        Price = 2000,
        Type = 0,
        CreatedBy = "User",
        UpdatedBy = "System",
        CreatedAt = DateTime.UtcNow,
        UpdatedAt = DateTime.UtcNow
    },
    new Preset {
        Id = "6fd08876dd804c01983236cb5277b406",
        UserId = "f49aa51bbd304e77933e24bbed65b165",
        StyleId = "037b41302a4c453f8cef135afceb3956",
                Name = "Name",
        Weight = 200,
        Images = new List<string> { "https://firebasestorage.googleapis.com/v0/b/mamafit-e0138.firebasestorage.app/o/presets%2Fmaxi%2Fmaxi-27.png?alt=media&token=c80d1ad8-5a45-4d8d-8e77-558ec415f728" },
        IsDefault = false,
        Price = 2000,
        Type = 0,
        CreatedBy = "User",
        UpdatedBy = "System",
        CreatedAt = DateTime.UtcNow,
        UpdatedAt = DateTime.UtcNow
    },
    new Preset {
        Id = "b0daa08ae6064686a59c0835bf58145c",
        UserId = "f49aa51bbd304e77933e24bbed65b165",
        StyleId = "037b41302a4c453f8cef135afceb3956",
                Name = "Name",
        Weight = 200,
        Images = new List<string> { "https://firebasestorage.googleapis.com/v0/b/mamafit-e0138.firebasestorage.app/o/presets%2Fmaxi%2Fmaxi-26.png?alt=media&token=d3653bec-f9bc-4558-bc54-81d6a19c4ce2" },
        IsDefault = false,
        Price = 2000,
        Type = 0,
        CreatedBy = "User",
        UpdatedBy = "System",
        CreatedAt = DateTime.UtcNow,
        UpdatedAt = DateTime.UtcNow
    },
    new Preset {
        Id = "dd737fac7e8b4b0cb353b6d8ac16cb51",
        UserId = "f49aa51bbd304e77933e24bbed65b165",
        StyleId = "037b41302a4c453f8cef135afceb3956",
                Name = "Name",
        Weight = 200,
        Images = new List<string> { "https://firebasestorage.googleapis.com/v0/b/mamafit-e0138.firebasestorage.app/o/presets%2Fmaxi%2Fmaxi-25.png?alt=media&token=f0a6e9b6-05cf-4e31-b1f3-9ecdbdcde3d2" },
        IsDefault = false,
        Price = 2000,
        Type = 0,
        CreatedBy = "User",
        UpdatedBy = "System",
        CreatedAt = DateTime.UtcNow,
        UpdatedAt = DateTime.UtcNow
    },
    new Preset {
        Id = "a80f79e565d240af9d77d2550c6e2933",
        UserId = "f49aa51bbd304e77933e24bbed65b165",
        StyleId = "037b41302a4c453f8cef135afceb3956",
                Name = "Name",
        Weight = 200,
        Images = new List<string> { "https://firebasestorage.googleapis.com/v0/b/mamafit-e0138.firebasestorage.app/o/presets%2Fmaxi%2Fmaxi-24.png?alt=media&token=b0ceddd4-b37a-4be9-951a-e403ae5b9433" },
        IsDefault = false,
        Price = 2000,
        Type = 0,
        CreatedBy = "User",
        UpdatedBy = "System",
        CreatedAt = DateTime.UtcNow,
        UpdatedAt = DateTime.UtcNow
    },
    new Preset {
        Id = "be7b594cc1274fb5b9c4fa5571ed8952",
        UserId = "f49aa51bbd304e77933e24bbed65b165",
        StyleId = "037b41302a4c453f8cef135afceb3956",
                Name = "Name",
        Weight = 200,
        Images = new List<string> { "https://firebasestorage.googleapis.com/v0/b/mamafit-e0138.firebasestorage.app/o/presets%2Fmaxi%2Fmaxi-23.png?alt=media&token=03440f28-4ec7-4fd7-8cd5-8a269106a726" },
        IsDefault = false,
        Price = 2000,
        Type = 0,
        CreatedBy = "User",
        UpdatedBy = "System",
        CreatedAt = DateTime.UtcNow,
        UpdatedAt = DateTime.UtcNow
    },
    new Preset {
        Id = "45f43125d239423893e337915ccaaea3",
        UserId = "f49aa51bbd304e77933e24bbed65b165",
        StyleId = "037b41302a4c453f8cef135afceb3956",
                Name = "Name",
        Weight = 200,
        Images = new List<string> { "https://firebasestorage.googleapis.com/v0/b/mamafit-e0138.firebasestorage.app/o/presets%2Fmaxi%2Fmaxi-22.png?alt=media&token=0d80783e-3267-4743-a2b1-f931a48716fd" },
        IsDefault = false,
        Price = 2000,
        Type = 0,
        CreatedBy = "User",
        UpdatedBy = "System",
        CreatedAt = DateTime.UtcNow,
        UpdatedAt = DateTime.UtcNow
    },
    new Preset {
        Id = "7c2f591168f144d199a8d9e174addc45",
        UserId = "f49aa51bbd304e77933e24bbed65b165",
        StyleId = "037b41302a4c453f8cef135afceb3956",        Name = "Name",
        Weight = 200,
        Images = new List<string> { "https://firebasestorage.googleapis.com/v0/b/mamafit-e0138.firebasestorage.app/o/presets%2Fmaxi%2Fmaxi-21.png?alt=media&token=13248af4-08bf-454c-9b30-6c741dbef82b" },
        IsDefault = false,
        Price = 2000,
        Type = 0,
        CreatedBy = "User",
        UpdatedBy = "System",
        CreatedAt = DateTime.UtcNow,
        UpdatedAt = DateTime.UtcNow
    },
    new Preset {
        Id = "ac1b2b6941c04598ae5a50d48efd8232",
        UserId = "f49aa51bbd304e77933e24bbed65b165",
        StyleId = "037b41302a4c453f8cef135afceb3956",
                Name = "Name",
        Weight = 200,
        Images = new List<string> { "https://firebasestorage.googleapis.com/v0/b/mamafit-e0138.firebasestorage.app/o/presets%2Fmaxi%2Fmaxi-20.png?alt=media&token=2294c5c4-403d-4b33-afec-78f20bf02566" },
        IsDefault = false,
        Price = 2000,
        Type = 0,
        CreatedBy = "User",
        UpdatedBy = "System",
        CreatedAt = DateTime.UtcNow,
        UpdatedAt = DateTime.UtcNow
    },
    new Preset {
        Id = "58d759c12387490fb3a97c4633d6f1bb",
        UserId = "f49aa51bbd304e77933e24bbed65b165",
        StyleId = "037b41302a4c453f8cef135afceb3956",
                Name = "Name",
        Weight = 200,
        Images = new List<string> { "https://firebasestorage.googleapis.com/v0/b/mamafit-e0138.firebasestorage.app/o/presets%2Fmaxi%2Fmaxi-10.png?alt=media&token=8fb01fe1-9ff2-4d7a-bf84-e8e1093f0f9b" },
        IsDefault = false,
        Price = 2000,
        Type = 0,
        CreatedBy = "User",
        UpdatedBy = "System",
        CreatedAt = DateTime.UtcNow,
        UpdatedAt = DateTime.UtcNow
    },
    new Preset {
        Id = "4f4d8b91705f4a9bb95ecf18b57b0306",
        UserId = "f49aa51bbd304e77933e24bbed65b165",
        StyleId = "037b41302a4c453f8cef135afceb3956",
                Name = "Name",
        Weight = 200,
        Images = new List<string> { "https://firebasestorage.googleapis.com/v0/b/mamafit-e0138.firebasestorage.app/o/presets%2Fmaxi%2Fmaxi-2.png?alt=media&token=f6cf6663-58a2-4b43-9c8a-46796b1dd8e1" },
        IsDefault = false,
        Price = 2000,
        Type = 0,
        CreatedBy = "User",
        UpdatedBy = "System",
        CreatedAt = DateTime.UtcNow,
        UpdatedAt = DateTime.UtcNow
    },
    new Preset {
        Id = "694d8135f4ca4238956e9f47460b5dff",
        UserId = "f49aa51bbd304e77933e24bbed65b165",
        StyleId = "037b41302a4c453f8cef135afceb3956",
                Name = "Name",
        Weight = 200,
        Images = new List<string> { "https://firebasestorage.googleapis.com/v0/b/mamafit-e0138.firebasestorage.app/o/presets%2Fmaxi%2Fmaxi-36.png?alt=media&token=dfa8808c-c87d-4ae0-be6b-1108343320ac" },
        IsDefault = false,
        Price = 2000,
        Type = 0,
        CreatedBy = "User",
        UpdatedBy = "System",
        CreatedAt = DateTime.UtcNow,
        UpdatedAt = DateTime.UtcNow
    },
    new Preset {
        Id = "f1ec1932c78e4a5a8035df1864fb32a4",
        UserId = "f49aa51bbd304e77933e24bbed65b165",
        StyleId = "037b41302a4c453f8cef135afceb3956",
                Name = "Name",
        Weight = 200,
        Images = new List<string> { "https://firebasestorage.googleapis.com/v0/b/mamafit-e0138.firebasestorage.app/o/presets%2Fmaxi%2Fmaxi-35.png?alt=media&token=1a81d76f-506a-4b60-9c03-05fa9aab1483" },
        IsDefault = false,
        Price = 2000,
        Type = 0,
        CreatedBy = "User",
        UpdatedBy = "System",
        CreatedAt = DateTime.UtcNow,
        UpdatedAt = DateTime.UtcNow
    },
    new Preset {
        Id = "cc5b36affe2243f2b119fd65cb3434d5",
        UserId = "f49aa51bbd304e77933e24bbed65b165",
        StyleId = "037b41302a4c453f8cef135afceb3956",
                Name = "Name",
        Weight = 200,
        Images = new List<string> { "https://firebasestorage.googleapis.com/v0/b/mamafit-e0138.firebasestorage.app/o/presets%2Fmaxi%2Fmaxi-34.png?alt=media&token=076bb0d1-0496-4353-adb2-1be1fce6e8bb" },
        IsDefault = false,
        Price = 2000,
        Type = 0,
        CreatedBy = "User",
        UpdatedBy = "System",
        CreatedAt = DateTime.UtcNow,
        UpdatedAt = DateTime.UtcNow
    },
    new Preset {
        Id = "7abc9feb7c1e47778ec32b24aac95985",
        UserId = "f49aa51bbd304e77933e24bbed65b165",
        StyleId = "037b41302a4c453f8cef135afceb3956",
                Name = "Name",
        Weight = 200,
        Images = new List<string> { "https://firebasestorage.googleapis.com/v0/b/mamafit-e0138.firebasestorage.app/o/presets%2Fmaxi%2Fmaxi-33.png?alt=media&token=5f0263e2-84b2-4260-9c25-ca2b12411fc2" },
        IsDefault = false,
        Price = 2000,
        Type = 0,
        CreatedBy = "User",
        UpdatedBy = "System",
        CreatedAt = DateTime.UtcNow,
        UpdatedAt = DateTime.UtcNow
    },
    new Preset {
        Id = "380e3d771eac4488ad94b09c9e77ccd2",
        UserId = "f49aa51bbd304e77933e24bbed65b165",
        StyleId = "037b41302a4c453f8cef135afceb3956",
                Name = "Name",
        Weight = 200,
        Images = new List<string> { "https://firebasestorage.googleapis.com/v0/b/mamafit-e0138.firebasestorage.app/o/presets%2Fmaxi%2Fmaxi-32.png?alt=media&token=d9268430-d725-4813-a70f-260ee49cd1c5" },
        IsDefault = false,
        Price = 2000,
        Type = 0,
        CreatedBy = "User",
        UpdatedBy = "System",
        CreatedAt = DateTime.UtcNow,
        UpdatedAt = DateTime.UtcNow
    },
    new Preset {
        Id = "1acd7ac56e5c4d15895f03cc2d6e7723",
        UserId = "f49aa51bbd304e77933e24bbed65b165",
        StyleId = "037b41302a4c453f8cef135afceb3956",
                Name = "Name",
        Weight = 200,
        Images = new List<string> { "https://firebasestorage.googleapis.com/v0/b/mamafit-e0138.firebasestorage.app/o/presets%2Fmaxi%2Fmaxi-31.png?alt=media&token=97b8fadf-6bf1-4ab7-995a-5126f7fbfc65" },
        IsDefault = false,
        Price = 2000,
        Type = 0,
        CreatedBy = "User",
        UpdatedBy = "System",
        CreatedAt = DateTime.UtcNow,
        UpdatedAt = DateTime.UtcNow
    },
    new Preset {
        Id = "50959c7c201e45a9b5255558ac9dfcf7",
        UserId = "f49aa51bbd304e77933e24bbed65b165",
        StyleId = "037b41302a4c453f8cef135afceb3956",
                Name = "Name",
        Weight = 200,
        Images = new List<string> { "https://firebasestorage.googleapis.com/v0/b/mamafit-e0138.firebasestorage.app/o/presets%2Fmaxi%2Fmaxi-30.png?alt=media&token=373eb84b-cd7b-4e81-840d-516781ae9d72" },
        IsDefault = false,
        Price = 2000,
        Type = 0,
        CreatedBy = "User",
        UpdatedBy = "System",
        CreatedAt = DateTime.UtcNow,
        UpdatedAt = DateTime.UtcNow
    },
    new Preset {
        Id = "5a5081ce720141188c09336e4a936ef9",
        UserId = "f49aa51bbd304e77933e24bbed65b165",
        StyleId = "037b41302a4c453f8cef135afceb3956",
                Name = "Name",
        Weight = 200,
        Images = new List<string> { "https://firebasestorage.googleapis.com/v0/b/mamafit-e0138.firebasestorage.app/o/presets%2Fmaxi%2Fmaxi-3.png?alt=media&token=149e97e6-d906-4345-bfcb-dfc3d619a73b" },
        IsDefault = false,
        Price = 2000,
        Type = 0,
        CreatedBy = "User",
        UpdatedBy = "System",
        CreatedAt = DateTime.UtcNow,
        UpdatedAt = DateTime.UtcNow
    },
    new Preset {
        Id = "fe7043a37995453db2d03741e81da9df",
        UserId = "f49aa51bbd304e77933e24bbed65b165",
        StyleId = "037b41302a4c453f8cef135afceb3956",
                Name = "Name",
        Weight = 200,
        Images = new List<string> { "https://firebasestorage.googleapis.com/v0/b/mamafit-e0138.firebasestorage.app/o/presets%2Fmaxi%2Fmaxi-9.png?alt=media&token=221bb4f5-1a84-452a-9a40-48cc8f50afdc" },
        IsDefault = false,
        Price = 2000,
        Type = 0,
        CreatedBy = "User",
        UpdatedBy = "System",
        CreatedAt = DateTime.UtcNow,
        UpdatedAt = DateTime.UtcNow
    },
    new Preset {
        Id = "7faa6eb739d548bf9f9ce7d3de978d3d",
        UserId = "f49aa51bbd304e77933e24bbed65b165",
        StyleId = "037b41302a4c453f8cef135afceb3956",
                Name = "Name",
        Weight = 200,
        Images = new List<string> { "https://firebasestorage.googleapis.com/v0/b/mamafit-e0138.firebasestorage.app/o/presets%2Fmaxi%2Fmaxi-8.png?alt=media&token=ccad2c07-0022-4956-9591-8ddcbda3a31a" },
        IsDefault = false,
        Price = 2000,
        Type = 0,
        CreatedBy = "User",
        UpdatedBy = "System",
        CreatedAt = DateTime.UtcNow,
        UpdatedAt = DateTime.UtcNow
    },
    new Preset {
        Id = "caa36b99b068411aaa62b0f4234c1656",
        UserId = "f49aa51bbd304e77933e24bbed65b165",
        StyleId = "037b41302a4c453f8cef135afceb3956",
                Name = "Name",
        Weight = 200,
        Images = new List<string> { "https://firebasestorage.googleapis.com/v0/b/mamafit-e0138.firebasestorage.app/o/presets%2Fmaxi%2Fmaxi-7.png?alt=media&token=29058fd5-ec1f-4199-94ff-72c77584b562" },
        IsDefault = false,
        Price = 2000,
        Type = 0,
        CreatedBy = "User",
        UpdatedBy = "System",
        CreatedAt = DateTime.UtcNow,
        UpdatedAt = DateTime.UtcNow
    },
    new Preset {
        Id = "68ee38a6554f407abd932d492719a834",
        UserId = "f49aa51bbd304e77933e24bbed65b165",
        StyleId = "037b41302a4c453f8cef135afceb3956",
                Name = "Name",
        Weight = 200,
        Images = new List<string> { "https://firebasestorage.googleapis.com/v0/b/mamafit-e0138.firebasestorage.app/o/presets%2Fmaxi%2Fmaxi-6.png?alt=media&token=28833b6c-ff85-4eb2-8f85-65e9ea4d8d23" },
        IsDefault = false,
        Price = 2000,
        Type = 0,
        CreatedBy = "User",
        UpdatedBy = "System",
        CreatedAt = DateTime.UtcNow,
        UpdatedAt = DateTime.UtcNow
    },
    new Preset {
        Id = "bac18c4350314174b9f4cbe73ca0b24d",
        UserId = "f49aa51bbd304e77933e24bbed65b165",
        StyleId = "037b41302a4c453f8cef135afceb3956",
                Name = "Name",
        Weight = 200,
        Images = new List<string> { "https://firebasestorage.googleapis.com/v0/b/mamafit-e0138.firebasestorage.app/o/presets%2Fmaxi%2Fmaxi-5.png?alt=media&token=778c61b7-68ff-4224-afd8-223d9295c99c" },
        IsDefault = false,
        Price = 2000,
        Type = 0,
        CreatedBy = "User",
        UpdatedBy = "System",
        CreatedAt = DateTime.UtcNow,
        UpdatedAt = DateTime.UtcNow
    },
    new Preset {
        Id = "68fca8f086454898ae81b1ea29890b60",
        UserId = "f49aa51bbd304e77933e24bbed65b165",
        StyleId = "037b41302a4c453f8cef135afceb3956",
        Name = "Name",
        Weight = 200,
        Images = new List<string> { "https://firebasestorage.googleapis.com/v0/b/mamafit-e0138.firebasestorage.app/o/presets%2Fmaxi%2Fmaxi-4.png?alt=media&token=ea8426c2-8bac-4b0d-bff3-e7e2536ac351" },
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
            var componentoptionpresets = new List<ComponentOptionPreset> {
    new ComponentOptionPreset {
        ComponentOptionsId = "06e6899bd6544dd3b9020a50789d9421",
        PresetsId = "fe88b53dce4c4938a7785fb9e3db1f0a"
    },
    new ComponentOptionPreset {
        ComponentOptionsId = "31c853d3dde94e68ab57903cf8a80798",
        PresetsId = "fe88b53dce4c4938a7785fb9e3db1f0a"
    },
    new ComponentOptionPreset {
        ComponentOptionsId = "4ed7cdc105ab44de81d333d63db0a222",
        PresetsId = "fe88b53dce4c4938a7785fb9e3db1f0a"
    },
    new ComponentOptionPreset {
        ComponentOptionsId = "8cff38afa8464ae191ba152b93438266",
        PresetsId = "fe88b53dce4c4938a7785fb9e3db1f0a"
    },
    new ComponentOptionPreset {
        ComponentOptionsId = "92bdbbdbfd3b4dcf8256b68a66dfa35b",
        PresetsId = "fe88b53dce4c4938a7785fb9e3db1f0a"
    },
    new ComponentOptionPreset {
        ComponentOptionsId = "c454b97a93f341e78b127d5425acc464",
        PresetsId = "fe88b53dce4c4938a7785fb9e3db1f0a"
    },
    new ComponentOptionPreset {
        ComponentOptionsId = "31c853d3dde94e68ab57903cf8a80798",
        PresetsId = "58d759c12387490fb3a97c4633d6f1bb"
    },
    new ComponentOptionPreset {
        ComponentOptionsId = "8158b8e5ef0340bdaf830fa8eb2e650d",
        PresetsId = "58d759c12387490fb3a97c4633d6f1bb"
    },
    new ComponentOptionPreset {
        ComponentOptionsId = "8cff38afa8464ae191ba152b93438266",
        PresetsId = "58d759c12387490fb3a97c4633d6f1bb"
    },
    new ComponentOptionPreset {
        ComponentOptionsId = "92bdbbdbfd3b4dcf8256b68a66dfa35b",
        PresetsId = "58d759c12387490fb3a97c4633d6f1bb"
    },
    new ComponentOptionPreset {
        ComponentOptionsId = "c454b97a93f341e78b127d5425acc464",
        PresetsId = "58d759c12387490fb3a97c4633d6f1bb"
    },
    new ComponentOptionPreset {
        ComponentOptionsId = "fecd26fe3b6b4e88ba97ebb406fa6df2",
        PresetsId = "58d759c12387490fb3a97c4633d6f1bb"
    },
    new ComponentOptionPreset {
        ComponentOptionsId = "31c853d3dde94e68ab57903cf8a80798",
        PresetsId = "7ae7818fdf09466f819808beed30e707"
    },
    new ComponentOptionPreset {
        ComponentOptionsId = "818f875df04a49eebe9c474bba4247b9",
        PresetsId = "7ae7818fdf09466f819808beed30e707"
    },
    new ComponentOptionPreset {
        ComponentOptionsId = "81acc249e2664c72908014a28e1e72ef",
        PresetsId = "7ae7818fdf09466f819808beed30e707"
    },
    new ComponentOptionPreset {
        ComponentOptionsId = "8cff38afa8464ae191ba152b93438266",
        PresetsId = "7ae7818fdf09466f819808beed30e707"
    },
    new ComponentOptionPreset {
        ComponentOptionsId = "c454b97a93f341e78b127d5425acc464",
        PresetsId = "7ae7818fdf09466f819808beed30e707"
    },
    new ComponentOptionPreset {
        ComponentOptionsId = "fecd26fe3b6b4e88ba97ebb406fa6df2",
        PresetsId = "7ae7818fdf09466f819808beed30e707"
    },
    new ComponentOptionPreset {
        ComponentOptionsId = "06e6899bd6544dd3b9020a50789d9421",
        PresetsId = "11230115583c4994bfaa3925951d3817"
    },
    new ComponentOptionPreset {
        ComponentOptionsId = "31c853d3dde94e68ab57903cf8a80798",
        PresetsId = "11230115583c4994bfaa3925951d3817"
    },
    new ComponentOptionPreset {
        ComponentOptionsId = "4ed7cdc105ab44de81d333d63db0a222",
        PresetsId = "11230115583c4994bfaa3925951d3817"
    },
    new ComponentOptionPreset {
        ComponentOptionsId = "70b69ec5ea074c0f8bf904b20b9c7884",
        PresetsId = "11230115583c4994bfaa3925951d3817"
    },
    new ComponentOptionPreset {
        ComponentOptionsId = "92bdbbdbfd3b4dcf8256b68a66dfa35b",
        PresetsId = "11230115583c4994bfaa3925951d3817"
    },
    new ComponentOptionPreset {
        ComponentOptionsId = "c454b97a93f341e78b127d5425acc464",
        PresetsId = "11230115583c4994bfaa3925951d3817"
    },
    new ComponentOptionPreset {
        ComponentOptionsId = "31c853d3dde94e68ab57903cf8a80798",
        PresetsId = "33c8177630a54906b92c0b8c03c57450"
    },
    new ComponentOptionPreset {
        ComponentOptionsId = "70b69ec5ea074c0f8bf904b20b9c7884",
        PresetsId = "33c8177630a54906b92c0b8c03c57450"
    },
    new ComponentOptionPreset {
        ComponentOptionsId = "8158b8e5ef0340bdaf830fa8eb2e650d",
        PresetsId = "33c8177630a54906b92c0b8c03c57450"
    },
    new ComponentOptionPreset {
        ComponentOptionsId = "92bdbbdbfd3b4dcf8256b68a66dfa35b",
        PresetsId = "33c8177630a54906b92c0b8c03c57450"
    },
    new ComponentOptionPreset {
        ComponentOptionsId = "c454b97a93f341e78b127d5425acc464",
        PresetsId = "33c8177630a54906b92c0b8c03c57450"
    },
    new ComponentOptionPreset {
        ComponentOptionsId = "fecd26fe3b6b4e88ba97ebb406fa6df2",
        PresetsId = "33c8177630a54906b92c0b8c03c57450"
    },
    new ComponentOptionPreset {
        ComponentOptionsId = "06e6899bd6544dd3b9020a50789d9421",
        PresetsId = "c9348ab0418743d08ee786e31083d76b"
    },
    new ComponentOptionPreset {
        ComponentOptionsId = "31c853d3dde94e68ab57903cf8a80798",
        PresetsId = "c9348ab0418743d08ee786e31083d76b"
    },
    new ComponentOptionPreset {
        ComponentOptionsId = "70b69ec5ea074c0f8bf904b20b9c7884",
        PresetsId = "c9348ab0418743d08ee786e31083d76b"
    },
    new ComponentOptionPreset {
        ComponentOptionsId = "92bdbbdbfd3b4dcf8256b68a66dfa35b",
        PresetsId = "c9348ab0418743d08ee786e31083d76b"
    },
    new ComponentOptionPreset {
        ComponentOptionsId = "c454b97a93f341e78b127d5425acc464",
        PresetsId = "c9348ab0418743d08ee786e31083d76b"
    },
    new ComponentOptionPreset {
        ComponentOptionsId = "fecd26fe3b6b4e88ba97ebb406fa6df2",
        PresetsId = "c9348ab0418743d08ee786e31083d76b"
    },
    new ComponentOptionPreset {
        ComponentOptionsId = "06e6899bd6544dd3b9020a50789d9421",
        PresetsId = "41e4b32655a44dca862141df8099f646"
    },
    new ComponentOptionPreset {
        ComponentOptionsId = "31c853d3dde94e68ab57903cf8a80798",
        PresetsId = "41e4b32655a44dca862141df8099f646"
    },
    new ComponentOptionPreset {
        ComponentOptionsId = "4ed7cdc105ab44de81d333d63db0a222",
        PresetsId = "41e4b32655a44dca862141df8099f646"
    },
    new ComponentOptionPreset {
        ComponentOptionsId = "70b69ec5ea074c0f8bf904b20b9c7884",
        PresetsId = "41e4b32655a44dca862141df8099f646"
    },
    new ComponentOptionPreset {
        ComponentOptionsId = "818f875df04a49eebe9c474bba4247b9",
        PresetsId = "41e4b32655a44dca862141df8099f646"
    },
    new ComponentOptionPreset {
        ComponentOptionsId = "c454b97a93f341e78b127d5425acc464",
        PresetsId = "41e4b32655a44dca862141df8099f646"
    },
    new ComponentOptionPreset {
        ComponentOptionsId = "06e6899bd6544dd3b9020a50789d9421",
        PresetsId = "bcdcf89795b2484f96a3a12b25383a12"
    },
    new ComponentOptionPreset {
        ComponentOptionsId = "31c853d3dde94e68ab57903cf8a80798",
        PresetsId = "bcdcf89795b2484f96a3a12b25383a12"
    },
    new ComponentOptionPreset {
        ComponentOptionsId = "78b78938aefe4bc6b927ed3722bc5859",
        PresetsId = "bcdcf89795b2484f96a3a12b25383a12"
    },
    new ComponentOptionPreset {
        ComponentOptionsId = "818f875df04a49eebe9c474bba4247b9",
        PresetsId = "bcdcf89795b2484f96a3a12b25383a12"
    },
    new ComponentOptionPreset {
        ComponentOptionsId = "8cff38afa8464ae191ba152b93438266",
        PresetsId = "bcdcf89795b2484f96a3a12b25383a12"
    },
    new ComponentOptionPreset {
        ComponentOptionsId = "c454b97a93f341e78b127d5425acc464",
        PresetsId = "bcdcf89795b2484f96a3a12b25383a12"
    },
    new ComponentOptionPreset {
        ComponentOptionsId = "31c853d3dde94e68ab57903cf8a80798",
        PresetsId = "5ef79dda17e64808afa6287c07c8bac9"
    },
    new ComponentOptionPreset {
        ComponentOptionsId = "4ed7cdc105ab44de81d333d63db0a222",
        PresetsId = "5ef79dda17e64808afa6287c07c8bac9"
    },
    new ComponentOptionPreset {
        ComponentOptionsId = "70b69ec5ea074c0f8bf904b20b9c7884",
        PresetsId = "5ef79dda17e64808afa6287c07c8bac9"
    },
    new ComponentOptionPreset {
        ComponentOptionsId = "8158b8e5ef0340bdaf830fa8eb2e650d",
        PresetsId = "5ef79dda17e64808afa6287c07c8bac9"
    },
    new ComponentOptionPreset {
        ComponentOptionsId = "92bdbbdbfd3b4dcf8256b68a66dfa35b",
        PresetsId = "5ef79dda17e64808afa6287c07c8bac9"
    },
    new ComponentOptionPreset {
        ComponentOptionsId = "c454b97a93f341e78b127d5425acc464",
        PresetsId = "5ef79dda17e64808afa6287c07c8bac9"
    },
    new ComponentOptionPreset {
        ComponentOptionsId = "06e6899bd6544dd3b9020a50789d9421",
        PresetsId = "3583422ceef647d28a14b82680058ff0"
    },
    new ComponentOptionPreset {
        ComponentOptionsId = "31c853d3dde94e68ab57903cf8a80798",
        PresetsId = "3583422ceef647d28a14b82680058ff0"
    },
    new ComponentOptionPreset {
        ComponentOptionsId = "78b78938aefe4bc6b927ed3722bc5859",
        PresetsId = "3583422ceef647d28a14b82680058ff0"
    },
    new ComponentOptionPreset {
        ComponentOptionsId = "8cff38afa8464ae191ba152b93438266",
        PresetsId = "3583422ceef647d28a14b82680058ff0"
    },
    new ComponentOptionPreset {
        ComponentOptionsId = "92bdbbdbfd3b4dcf8256b68a66dfa35b",
        PresetsId = "3583422ceef647d28a14b82680058ff0"
    },
    new ComponentOptionPreset {
        ComponentOptionsId = "c454b97a93f341e78b127d5425acc464",
        PresetsId = "3583422ceef647d28a14b82680058ff0"
    },
    new ComponentOptionPreset {
        ComponentOptionsId = "31c853d3dde94e68ab57903cf8a80798",
        PresetsId = "6ab0daed7fe54e9fa791bfcb6bf1a043"
    },
    new ComponentOptionPreset {
        ComponentOptionsId = "81acc249e2664c72908014a28e1e72ef",
        PresetsId = "6ab0daed7fe54e9fa791bfcb6bf1a043"
    },
    new ComponentOptionPreset {
        ComponentOptionsId = "8cff38afa8464ae191ba152b93438266",
        PresetsId = "6ab0daed7fe54e9fa791bfcb6bf1a043"
    },
    new ComponentOptionPreset {
        ComponentOptionsId = "92bdbbdbfd3b4dcf8256b68a66dfa35b",
        PresetsId = "6ab0daed7fe54e9fa791bfcb6bf1a043"
    },
    new ComponentOptionPreset {
        ComponentOptionsId = "c454b97a93f341e78b127d5425acc464",
        PresetsId = "6ab0daed7fe54e9fa791bfcb6bf1a043"
    },
    new ComponentOptionPreset {
        ComponentOptionsId = "fecd26fe3b6b4e88ba97ebb406fa6df2",
        PresetsId = "6ab0daed7fe54e9fa791bfcb6bf1a043"
    },
    new ComponentOptionPreset {
        ComponentOptionsId = "31c853d3dde94e68ab57903cf8a80798",
        PresetsId = "ccc662f99d7a41d7aa1505e3a2cb8950"
    },
    new ComponentOptionPreset {
        ComponentOptionsId = "70b69ec5ea074c0f8bf904b20b9c7884",
        PresetsId = "ccc662f99d7a41d7aa1505e3a2cb8950"
    },
    new ComponentOptionPreset {
        ComponentOptionsId = "8158b8e5ef0340bdaf830fa8eb2e650d",
        PresetsId = "ccc662f99d7a41d7aa1505e3a2cb8950"
    },
    new ComponentOptionPreset {
        ComponentOptionsId = "818f875df04a49eebe9c474bba4247b9",
        PresetsId = "ccc662f99d7a41d7aa1505e3a2cb8950"
    },
    new ComponentOptionPreset {
        ComponentOptionsId = "c454b97a93f341e78b127d5425acc464",
        PresetsId = "ccc662f99d7a41d7aa1505e3a2cb8950"
    },
    new ComponentOptionPreset {
        ComponentOptionsId = "fecd26fe3b6b4e88ba97ebb406fa6df2",
        PresetsId = "ccc662f99d7a41d7aa1505e3a2cb8950"
    },
    new ComponentOptionPreset {
        ComponentOptionsId = "31c853d3dde94e68ab57903cf8a80798",
        PresetsId = "49e11d4698f643dca14364eed4d9c239"
    },
    new ComponentOptionPreset {
        ComponentOptionsId = "78b78938aefe4bc6b927ed3722bc5859",
        PresetsId = "49e11d4698f643dca14364eed4d9c239"
    },
    new ComponentOptionPreset {
        ComponentOptionsId = "818f875df04a49eebe9c474bba4247b9",
        PresetsId = "49e11d4698f643dca14364eed4d9c239"
    },
    new ComponentOptionPreset {
        ComponentOptionsId = "81acc249e2664c72908014a28e1e72ef",
        PresetsId = "49e11d4698f643dca14364eed4d9c239"
    },
    new ComponentOptionPreset {
        ComponentOptionsId = "8cff38afa8464ae191ba152b93438266",
        PresetsId = "49e11d4698f643dca14364eed4d9c239"
    },
    new ComponentOptionPreset {
        ComponentOptionsId = "c454b97a93f341e78b127d5425acc464",
        PresetsId = "49e11d4698f643dca14364eed4d9c239"
    },
    new ComponentOptionPreset {
        ComponentOptionsId = "31c853d3dde94e68ab57903cf8a80798",
        PresetsId = "6fd08876dd804c01983236cb5277b406"
    },
    new ComponentOptionPreset {
        ComponentOptionsId = "78b78938aefe4bc6b927ed3722bc5859",
        PresetsId = "6fd08876dd804c01983236cb5277b406"
    },
    new ComponentOptionPreset {
        ComponentOptionsId = "8158b8e5ef0340bdaf830fa8eb2e650d",
        PresetsId = "6fd08876dd804c01983236cb5277b406"
    },
    new ComponentOptionPreset {
        ComponentOptionsId = "818f875df04a49eebe9c474bba4247b9",
        PresetsId = "6fd08876dd804c01983236cb5277b406"
    },
    new ComponentOptionPreset {
        ComponentOptionsId = "8cff38afa8464ae191ba152b93438266",
        PresetsId = "6fd08876dd804c01983236cb5277b406"
    },
    new ComponentOptionPreset {
        ComponentOptionsId = "c454b97a93f341e78b127d5425acc464",
        PresetsId = "6fd08876dd804c01983236cb5277b406"
    },
    new ComponentOptionPreset {
        ComponentOptionsId = "31c853d3dde94e68ab57903cf8a80798",
        PresetsId = "b0daa08ae6064686a59c0835bf58145c"
    },
    new ComponentOptionPreset {
        ComponentOptionsId = "70b69ec5ea074c0f8bf904b20b9c7884",
        PresetsId = "b0daa08ae6064686a59c0835bf58145c"
    },
    new ComponentOptionPreset {
        ComponentOptionsId = "78b78938aefe4bc6b927ed3722bc5859",
        PresetsId = "b0daa08ae6064686a59c0835bf58145c"
    },
    new ComponentOptionPreset {
        ComponentOptionsId = "81acc249e2664c72908014a28e1e72ef",
        PresetsId = "b0daa08ae6064686a59c0835bf58145c"
    },
    new ComponentOptionPreset {
        ComponentOptionsId = "92bdbbdbfd3b4dcf8256b68a66dfa35b",
        PresetsId = "b0daa08ae6064686a59c0835bf58145c"
    },
    new ComponentOptionPreset {
        ComponentOptionsId = "c454b97a93f341e78b127d5425acc464",
        PresetsId = "b0daa08ae6064686a59c0835bf58145c"
    },
    new ComponentOptionPreset {
        ComponentOptionsId = "31c853d3dde94e68ab57903cf8a80798",
        PresetsId = "dd737fac7e8b4b0cb353b6d8ac16cb51"
    },
    new ComponentOptionPreset {
        ComponentOptionsId = "70b69ec5ea074c0f8bf904b20b9c7884",
        PresetsId = "dd737fac7e8b4b0cb353b6d8ac16cb51"
    },
    new ComponentOptionPreset {
        ComponentOptionsId = "78b78938aefe4bc6b927ed3722bc5859",
        PresetsId = "dd737fac7e8b4b0cb353b6d8ac16cb51"
    },
    new ComponentOptionPreset {
        ComponentOptionsId = "8158b8e5ef0340bdaf830fa8eb2e650d",
        PresetsId = "dd737fac7e8b4b0cb353b6d8ac16cb51"
    },
    new ComponentOptionPreset {
        ComponentOptionsId = "92bdbbdbfd3b4dcf8256b68a66dfa35b",
        PresetsId = "dd737fac7e8b4b0cb353b6d8ac16cb51"
    },
    new ComponentOptionPreset {
        ComponentOptionsId = "c454b97a93f341e78b127d5425acc464",
        PresetsId = "dd737fac7e8b4b0cb353b6d8ac16cb51"
    },
    new ComponentOptionPreset {
        ComponentOptionsId = "06e6899bd6544dd3b9020a50789d9421",
        PresetsId = "a80f79e565d240af9d77d2550c6e2933"
    },
    new ComponentOptionPreset {
        ComponentOptionsId = "31c853d3dde94e68ab57903cf8a80798",
        PresetsId = "a80f79e565d240af9d77d2550c6e2933"
    },
    new ComponentOptionPreset {
        ComponentOptionsId = "70b69ec5ea074c0f8bf904b20b9c7884",
        PresetsId = "a80f79e565d240af9d77d2550c6e2933"
    },
    new ComponentOptionPreset {
        ComponentOptionsId = "78b78938aefe4bc6b927ed3722bc5859",
        PresetsId = "a80f79e565d240af9d77d2550c6e2933"
    },
    new ComponentOptionPreset {
        ComponentOptionsId = "92bdbbdbfd3b4dcf8256b68a66dfa35b",
        PresetsId = "a80f79e565d240af9d77d2550c6e2933"
    },
    new ComponentOptionPreset {
        ComponentOptionsId = "c454b97a93f341e78b127d5425acc464",
        PresetsId = "a80f79e565d240af9d77d2550c6e2933"
    },
    new ComponentOptionPreset {
        ComponentOptionsId = "31c853d3dde94e68ab57903cf8a80798",
        PresetsId = "be7b594cc1274fb5b9c4fa5571ed8952"
    },
    new ComponentOptionPreset {
        ComponentOptionsId = "78b78938aefe4bc6b927ed3722bc5859",
        PresetsId = "be7b594cc1274fb5b9c4fa5571ed8952"
    },
    new ComponentOptionPreset {
        ComponentOptionsId = "81acc249e2664c72908014a28e1e72ef",
        PresetsId = "be7b594cc1274fb5b9c4fa5571ed8952"
    },
    new ComponentOptionPreset {
        ComponentOptionsId = "8cff38afa8464ae191ba152b93438266",
        PresetsId = "be7b594cc1274fb5b9c4fa5571ed8952"
    },
    new ComponentOptionPreset {
        ComponentOptionsId = "92bdbbdbfd3b4dcf8256b68a66dfa35b",
        PresetsId = "be7b594cc1274fb5b9c4fa5571ed8952"
    },
    new ComponentOptionPreset {
        ComponentOptionsId = "c454b97a93f341e78b127d5425acc464",
        PresetsId = "be7b594cc1274fb5b9c4fa5571ed8952"
    },
    new ComponentOptionPreset {
        ComponentOptionsId = "31c853d3dde94e68ab57903cf8a80798",
        PresetsId = "45f43125d239423893e337915ccaaea3"
    },
    new ComponentOptionPreset {
        ComponentOptionsId = "78b78938aefe4bc6b927ed3722bc5859",
        PresetsId = "45f43125d239423893e337915ccaaea3"
    },
    new ComponentOptionPreset {
        ComponentOptionsId = "8158b8e5ef0340bdaf830fa8eb2e650d",
        PresetsId = "45f43125d239423893e337915ccaaea3"
    },
    new ComponentOptionPreset {
        ComponentOptionsId = "8cff38afa8464ae191ba152b93438266",
        PresetsId = "45f43125d239423893e337915ccaaea3"
    },
    new ComponentOptionPreset {
        ComponentOptionsId = "92bdbbdbfd3b4dcf8256b68a66dfa35b",
        PresetsId = "45f43125d239423893e337915ccaaea3"
    },
    new ComponentOptionPreset {
        ComponentOptionsId = "c454b97a93f341e78b127d5425acc464",
        PresetsId = "45f43125d239423893e337915ccaaea3"
    },
    new ComponentOptionPreset {
        ComponentOptionsId = "31c853d3dde94e68ab57903cf8a80798",
        PresetsId = "7c2f591168f144d199a8d9e174addc45"
    },
    new ComponentOptionPreset {
        ComponentOptionsId = "70b69ec5ea074c0f8bf904b20b9c7884",
        PresetsId = "7c2f591168f144d199a8d9e174addc45"
    },
    new ComponentOptionPreset {
        ComponentOptionsId = "81acc249e2664c72908014a28e1e72ef",
        PresetsId = "7c2f591168f144d199a8d9e174addc45"
    },
    new ComponentOptionPreset {
        ComponentOptionsId = "92bdbbdbfd3b4dcf8256b68a66dfa35b",
        PresetsId = "7c2f591168f144d199a8d9e174addc45"
    },
    new ComponentOptionPreset {
        ComponentOptionsId = "c454b97a93f341e78b127d5425acc464",
        PresetsId = "7c2f591168f144d199a8d9e174addc45"
    },
    new ComponentOptionPreset {
        ComponentOptionsId = "fecd26fe3b6b4e88ba97ebb406fa6df2",
        PresetsId = "7c2f591168f144d199a8d9e174addc45"
    },
    new ComponentOptionPreset {
        ComponentOptionsId = "31c853d3dde94e68ab57903cf8a80798",
        PresetsId = "ac1b2b6941c04598ae5a50d48efd8232"
    },
    new ComponentOptionPreset {
        ComponentOptionsId = "4ed7cdc105ab44de81d333d63db0a222",
        PresetsId = "ac1b2b6941c04598ae5a50d48efd8232"
    },
    new ComponentOptionPreset {
        ComponentOptionsId = "70b69ec5ea074c0f8bf904b20b9c7884",
        PresetsId = "ac1b2b6941c04598ae5a50d48efd8232"
    },
    new ComponentOptionPreset {
        ComponentOptionsId = "81acc249e2664c72908014a28e1e72ef",
        PresetsId = "ac1b2b6941c04598ae5a50d48efd8232"
    },
    new ComponentOptionPreset {
        ComponentOptionsId = "92bdbbdbfd3b4dcf8256b68a66dfa35b",
        PresetsId = "ac1b2b6941c04598ae5a50d48efd8232"
    },
    new ComponentOptionPreset {
        ComponentOptionsId = "c454b97a93f341e78b127d5425acc464",
        PresetsId = "ac1b2b6941c04598ae5a50d48efd8232"
    },
    new ComponentOptionPreset {
        ComponentOptionsId = "06e6899bd6544dd3b9020a50789d9421",
        PresetsId = "4f4d8b91705f4a9bb95ecf18b57b0306"
    },
    new ComponentOptionPreset {
        ComponentOptionsId = "31c853d3dde94e68ab57903cf8a80798",
        PresetsId = "4f4d8b91705f4a9bb95ecf18b57b0306"
    },
    new ComponentOptionPreset {
        ComponentOptionsId = "4ed7cdc105ab44de81d333d63db0a222",
        PresetsId = "4f4d8b91705f4a9bb95ecf18b57b0306"
    },
    new ComponentOptionPreset {
        ComponentOptionsId = "818f875df04a49eebe9c474bba4247b9",
        PresetsId = "4f4d8b91705f4a9bb95ecf18b57b0306"
    },
    new ComponentOptionPreset {
        ComponentOptionsId = "8cff38afa8464ae191ba152b93438266",
        PresetsId = "4f4d8b91705f4a9bb95ecf18b57b0306"
    },
    new ComponentOptionPreset {
        ComponentOptionsId = "c454b97a93f341e78b127d5425acc464",
        PresetsId = "4f4d8b91705f4a9bb95ecf18b57b0306"
    },
    new ComponentOptionPreset {
        ComponentOptionsId = "06e6899bd6544dd3b9020a50789d9421",
        PresetsId = "694d8135f4ca4238956e9f47460b5dff"
    },
    new ComponentOptionPreset {
        ComponentOptionsId = "31c853d3dde94e68ab57903cf8a80798",
        PresetsId = "694d8135f4ca4238956e9f47460b5dff"
    },
    new ComponentOptionPreset {
        ComponentOptionsId = "70b69ec5ea074c0f8bf904b20b9c7884",
        PresetsId = "694d8135f4ca4238956e9f47460b5dff"
    },
    new ComponentOptionPreset {
        ComponentOptionsId = "818f875df04a49eebe9c474bba4247b9",
        PresetsId = "694d8135f4ca4238956e9f47460b5dff"
    },
    new ComponentOptionPreset {
        ComponentOptionsId = "c454b97a93f341e78b127d5425acc464",
        PresetsId = "694d8135f4ca4238956e9f47460b5dff"
    },
    new ComponentOptionPreset {
        ComponentOptionsId = "fecd26fe3b6b4e88ba97ebb406fa6df2",
        PresetsId = "694d8135f4ca4238956e9f47460b5dff"
    },
    new ComponentOptionPreset {
        ComponentOptionsId = "31c853d3dde94e68ab57903cf8a80798",
        PresetsId = "f1ec1932c78e4a5a8035df1864fb32a4"
    },
    new ComponentOptionPreset {
        ComponentOptionsId = "4ed7cdc105ab44de81d333d63db0a222",
        PresetsId = "f1ec1932c78e4a5a8035df1864fb32a4"
    },
    new ComponentOptionPreset {
        ComponentOptionsId = "70b69ec5ea074c0f8bf904b20b9c7884",
        PresetsId = "f1ec1932c78e4a5a8035df1864fb32a4"
    },
    new ComponentOptionPreset {
        ComponentOptionsId = "818f875df04a49eebe9c474bba4247b9",
        PresetsId = "f1ec1932c78e4a5a8035df1864fb32a4"
    },
    new ComponentOptionPreset {
        ComponentOptionsId = "81acc249e2664c72908014a28e1e72ef",
        PresetsId = "f1ec1932c78e4a5a8035df1864fb32a4"
    },
    new ComponentOptionPreset {
        ComponentOptionsId = "c454b97a93f341e78b127d5425acc464",
        PresetsId = "f1ec1932c78e4a5a8035df1864fb32a4"
    },
    new ComponentOptionPreset {
        ComponentOptionsId = "31c853d3dde94e68ab57903cf8a80798",
        PresetsId = "cc5b36affe2243f2b119fd65cb3434d5"
    },
    new ComponentOptionPreset {
        ComponentOptionsId = "4ed7cdc105ab44de81d333d63db0a222",
        PresetsId = "cc5b36affe2243f2b119fd65cb3434d5"
    },
    new ComponentOptionPreset {
        ComponentOptionsId = "70b69ec5ea074c0f8bf904b20b9c7884",
        PresetsId = "cc5b36affe2243f2b119fd65cb3434d5"
    },
    new ComponentOptionPreset {
        ComponentOptionsId = "8158b8e5ef0340bdaf830fa8eb2e650d",
        PresetsId = "cc5b36affe2243f2b119fd65cb3434d5"
    },
    new ComponentOptionPreset {
        ComponentOptionsId = "818f875df04a49eebe9c474bba4247b9",
        PresetsId = "cc5b36affe2243f2b119fd65cb3434d5"
    },
    new ComponentOptionPreset {
        ComponentOptionsId = "c454b97a93f341e78b127d5425acc464",
        PresetsId = "cc5b36affe2243f2b119fd65cb3434d5"
    },
    new ComponentOptionPreset {
        ComponentOptionsId = "06e6899bd6544dd3b9020a50789d9421",
        PresetsId = "7abc9feb7c1e47778ec32b24aac95985"
    },
    new ComponentOptionPreset {
        ComponentOptionsId = "31c853d3dde94e68ab57903cf8a80798",
        PresetsId = "7abc9feb7c1e47778ec32b24aac95985"
    },
    new ComponentOptionPreset {
        ComponentOptionsId = "70b69ec5ea074c0f8bf904b20b9c7884",
        PresetsId = "7abc9feb7c1e47778ec32b24aac95985"
    },
    new ComponentOptionPreset {
        ComponentOptionsId = "78b78938aefe4bc6b927ed3722bc5859",
        PresetsId = "7abc9feb7c1e47778ec32b24aac95985"
    },
    new ComponentOptionPreset {
        ComponentOptionsId = "818f875df04a49eebe9c474bba4247b9",
        PresetsId = "7abc9feb7c1e47778ec32b24aac95985"
    },
    new ComponentOptionPreset {
        ComponentOptionsId = "c454b97a93f341e78b127d5425acc464",
        PresetsId = "7abc9feb7c1e47778ec32b24aac95985"
    },
    new ComponentOptionPreset {
        ComponentOptionsId = "31c853d3dde94e68ab57903cf8a80798",
        PresetsId = "380e3d771eac4488ad94b09c9e77ccd2"
    },
    new ComponentOptionPreset {
        ComponentOptionsId = "70b69ec5ea074c0f8bf904b20b9c7884",
        PresetsId = "380e3d771eac4488ad94b09c9e77ccd2"
    },
    new ComponentOptionPreset {
        ComponentOptionsId = "78b78938aefe4bc6b927ed3722bc5859",
        PresetsId = "380e3d771eac4488ad94b09c9e77ccd2"
    },
    new ComponentOptionPreset {
        ComponentOptionsId = "818f875df04a49eebe9c474bba4247b9",
        PresetsId = "380e3d771eac4488ad94b09c9e77ccd2"
    },
    new ComponentOptionPreset {
        ComponentOptionsId = "81acc249e2664c72908014a28e1e72ef",
        PresetsId = "380e3d771eac4488ad94b09c9e77ccd2"
    },
    new ComponentOptionPreset {
        ComponentOptionsId = "c454b97a93f341e78b127d5425acc464",
        PresetsId = "380e3d771eac4488ad94b09c9e77ccd2"
    },
    new ComponentOptionPreset {
        ComponentOptionsId = "31c853d3dde94e68ab57903cf8a80798",
        PresetsId = "1acd7ac56e5c4d15895f03cc2d6e7723"
    },
    new ComponentOptionPreset {
        ComponentOptionsId = "70b69ec5ea074c0f8bf904b20b9c7884",
        PresetsId = "1acd7ac56e5c4d15895f03cc2d6e7723"
    },
    new ComponentOptionPreset {
        ComponentOptionsId = "78b78938aefe4bc6b927ed3722bc5859",
        PresetsId = "1acd7ac56e5c4d15895f03cc2d6e7723"
    },
    new ComponentOptionPreset {
        ComponentOptionsId = "8158b8e5ef0340bdaf830fa8eb2e650d",
        PresetsId = "1acd7ac56e5c4d15895f03cc2d6e7723"
    },
    new ComponentOptionPreset {
        ComponentOptionsId = "818f875df04a49eebe9c474bba4247b9",
        PresetsId = "1acd7ac56e5c4d15895f03cc2d6e7723"
    },
    new ComponentOptionPreset {
        ComponentOptionsId = "c454b97a93f341e78b127d5425acc464",
        PresetsId = "1acd7ac56e5c4d15895f03cc2d6e7723"
    },
    new ComponentOptionPreset {
        ComponentOptionsId = "31c853d3dde94e68ab57903cf8a80798",
        PresetsId = "50959c7c201e45a9b5255558ac9dfcf7"
    },
    new ComponentOptionPreset {
        ComponentOptionsId = "70b69ec5ea074c0f8bf904b20b9c7884",
        PresetsId = "50959c7c201e45a9b5255558ac9dfcf7"
    },
    new ComponentOptionPreset {
        ComponentOptionsId = "818f875df04a49eebe9c474bba4247b9",
        PresetsId = "50959c7c201e45a9b5255558ac9dfcf7"
    },
    new ComponentOptionPreset {
        ComponentOptionsId = "81acc249e2664c72908014a28e1e72ef",
        PresetsId = "50959c7c201e45a9b5255558ac9dfcf7"
    },
    new ComponentOptionPreset {
        ComponentOptionsId = "c454b97a93f341e78b127d5425acc464",
        PresetsId = "50959c7c201e45a9b5255558ac9dfcf7"
    },
    new ComponentOptionPreset {
        ComponentOptionsId = "fecd26fe3b6b4e88ba97ebb406fa6df2",
        PresetsId = "50959c7c201e45a9b5255558ac9dfcf7"
    },
    new ComponentOptionPreset {
        ComponentOptionsId = "31c853d3dde94e68ab57903cf8a80798",
        PresetsId = "5a5081ce720141188c09336e4a936ef9"
    },
    new ComponentOptionPreset {
        ComponentOptionsId = "4ed7cdc105ab44de81d333d63db0a222",
        PresetsId = "5a5081ce720141188c09336e4a936ef9"
    },
    new ComponentOptionPreset {
        ComponentOptionsId = "8158b8e5ef0340bdaf830fa8eb2e650d",
        PresetsId = "5a5081ce720141188c09336e4a936ef9"
    },
    new ComponentOptionPreset {
        ComponentOptionsId = "8cff38afa8464ae191ba152b93438266",
        PresetsId = "5a5081ce720141188c09336e4a936ef9"
    },
    new ComponentOptionPreset {
        ComponentOptionsId = "92bdbbdbfd3b4dcf8256b68a66dfa35b",
        PresetsId = "5a5081ce720141188c09336e4a936ef9"
    },
    new ComponentOptionPreset {
        ComponentOptionsId = "c454b97a93f341e78b127d5425acc464",
        PresetsId = "5a5081ce720141188c09336e4a936ef9"
    },
    new ComponentOptionPreset {
        ComponentOptionsId = "06e6899bd6544dd3b9020a50789d9421",
        PresetsId = "fe7043a37995453db2d03741e81da9df"
    },
    new ComponentOptionPreset {
        ComponentOptionsId = "31c853d3dde94e68ab57903cf8a80798",
        PresetsId = "fe7043a37995453db2d03741e81da9df"
    },
    new ComponentOptionPreset {
        ComponentOptionsId = "8cff38afa8464ae191ba152b93438266",
        PresetsId = "fe7043a37995453db2d03741e81da9df"
    },
    new ComponentOptionPreset {
        ComponentOptionsId = "92bdbbdbfd3b4dcf8256b68a66dfa35b",
        PresetsId = "fe7043a37995453db2d03741e81da9df"
    },
    new ComponentOptionPreset {
        ComponentOptionsId = "c454b97a93f341e78b127d5425acc464",
        PresetsId = "fe7043a37995453db2d03741e81da9df"
    },
    new ComponentOptionPreset {
        ComponentOptionsId = "fecd26fe3b6b4e88ba97ebb406fa6df2",
        PresetsId = "fe7043a37995453db2d03741e81da9df"
    },
    new ComponentOptionPreset {
        ComponentOptionsId = "31c853d3dde94e68ab57903cf8a80798",
        PresetsId = "7faa6eb739d548bf9f9ce7d3de978d3d"
    },
    new ComponentOptionPreset {
        ComponentOptionsId = "8158b8e5ef0340bdaf830fa8eb2e650d",
        PresetsId = "7faa6eb739d548bf9f9ce7d3de978d3d"
    },
    new ComponentOptionPreset {
        ComponentOptionsId = "818f875df04a49eebe9c474bba4247b9",
        PresetsId = "7faa6eb739d548bf9f9ce7d3de978d3d"
    },
    new ComponentOptionPreset {
        ComponentOptionsId = "8cff38afa8464ae191ba152b93438266",
        PresetsId = "7faa6eb739d548bf9f9ce7d3de978d3d"
    },
    new ComponentOptionPreset {
        ComponentOptionsId = "c454b97a93f341e78b127d5425acc464",
        PresetsId = "7faa6eb739d548bf9f9ce7d3de978d3d"
    },
    new ComponentOptionPreset {
        ComponentOptionsId = "fecd26fe3b6b4e88ba97ebb406fa6df2",
        PresetsId = "7faa6eb739d548bf9f9ce7d3de978d3d"
    },
    new ComponentOptionPreset {
        ComponentOptionsId = "31c853d3dde94e68ab57903cf8a80798",
        PresetsId = "caa36b99b068411aaa62b0f4234c1656"
    },
    new ComponentOptionPreset {
        ComponentOptionsId = "4ed7cdc105ab44de81d333d63db0a222",
        PresetsId = "caa36b99b068411aaa62b0f4234c1656"
    },
    new ComponentOptionPreset {
        ComponentOptionsId = "818f875df04a49eebe9c474bba4247b9",
        PresetsId = "caa36b99b068411aaa62b0f4234c1656"
    },
    new ComponentOptionPreset {
        ComponentOptionsId = "81acc249e2664c72908014a28e1e72ef",
        PresetsId = "caa36b99b068411aaa62b0f4234c1656"
    },
    new ComponentOptionPreset {
        ComponentOptionsId = "8cff38afa8464ae191ba152b93438266",
        PresetsId = "caa36b99b068411aaa62b0f4234c1656"
    },
    new ComponentOptionPreset {
        ComponentOptionsId = "c454b97a93f341e78b127d5425acc464",
        PresetsId = "caa36b99b068411aaa62b0f4234c1656"
    },
    new ComponentOptionPreset {
        ComponentOptionsId = "06e6899bd6544dd3b9020a50789d9421",
        PresetsId = "68ee38a6554f407abd932d492719a834"
    },
    new ComponentOptionPreset {
        ComponentOptionsId = "31c853d3dde94e68ab57903cf8a80798",
        PresetsId = "68ee38a6554f407abd932d492719a834"
    },
    new ComponentOptionPreset {
        ComponentOptionsId = "818f875df04a49eebe9c474bba4247b9",
        PresetsId = "68ee38a6554f407abd932d492719a834"
    },
    new ComponentOptionPreset {
        ComponentOptionsId = "8cff38afa8464ae191ba152b93438266",
        PresetsId = "68ee38a6554f407abd932d492719a834"
    },
    new ComponentOptionPreset {
        ComponentOptionsId = "c454b97a93f341e78b127d5425acc464",
        PresetsId = "68ee38a6554f407abd932d492719a834"
    },
    new ComponentOptionPreset {
        ComponentOptionsId = "fecd26fe3b6b4e88ba97ebb406fa6df2",
        PresetsId = "68ee38a6554f407abd932d492719a834"
    },
    new ComponentOptionPreset {
        ComponentOptionsId = "31c853d3dde94e68ab57903cf8a80798",
        PresetsId = "bac18c4350314174b9f4cbe73ca0b24d"
    },
    new ComponentOptionPreset {
        ComponentOptionsId = "4ed7cdc105ab44de81d333d63db0a222",
        PresetsId = "bac18c4350314174b9f4cbe73ca0b24d"
    },
    new ComponentOptionPreset {
        ComponentOptionsId = "8158b8e5ef0340bdaf830fa8eb2e650d",
        PresetsId = "bac18c4350314174b9f4cbe73ca0b24d"
    },
    new ComponentOptionPreset {
        ComponentOptionsId = "818f875df04a49eebe9c474bba4247b9",
        PresetsId = "bac18c4350314174b9f4cbe73ca0b24d"
    },
    new ComponentOptionPreset {
        ComponentOptionsId = "8cff38afa8464ae191ba152b93438266",
        PresetsId = "bac18c4350314174b9f4cbe73ca0b24d"
    },
    new ComponentOptionPreset {
        ComponentOptionsId = "c454b97a93f341e78b127d5425acc464",
        PresetsId = "bac18c4350314174b9f4cbe73ca0b24d"
    },
    new ComponentOptionPreset {
        ComponentOptionsId = "31c853d3dde94e68ab57903cf8a80798",
        PresetsId = "68fca8f086454898ae81b1ea29890b60"
    },
    new ComponentOptionPreset {
        ComponentOptionsId = "4ed7cdc105ab44de81d333d63db0a222",
        PresetsId = "68fca8f086454898ae81b1ea29890b60"
    },
    new ComponentOptionPreset {
        ComponentOptionsId = "81acc249e2664c72908014a28e1e72ef",
        PresetsId = "68fca8f086454898ae81b1ea29890b60"
    },
    new ComponentOptionPreset {
        ComponentOptionsId = "8cff38afa8464ae191ba152b93438266",
        PresetsId = "68fca8f086454898ae81b1ea29890b60"
    },
    new ComponentOptionPreset {
        ComponentOptionsId = "92bdbbdbfd3b4dcf8256b68a66dfa35b",
        PresetsId = "68fca8f086454898ae81b1ea29890b60"
    },
    new ComponentOptionPreset {
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

            #region Seed Milestone, MaternityDressTask
            modelBuilder.Entity<Milestone>().HasData(
                new Milestone
                {
                    Id = "e8e56df1c7e0467d9dfc15df96735c10",
                    Name = "Khám phá nhu cầu",
                    Description = "Tìm hiểu nhu cầu và phong cách của khách hàng",
                    SequenceOrder = 1,
                    ApplyFor = [ItemType.DESIGN_REQUEST, ItemType.PRESET, ItemType.READY_TO_BUY],
                    CreatedAt = now,
                    UpdatedAt = now,
                    CreatedBy = "System",
                    UpdatedBy = "System",
                    IsDeleted = false
                },
                new Milestone
                {
                    Id = "ef9791c0d0e14e48b6b98432cdd9e60c",
                    Name = "Chọn hoặc tạo mẫu đầm",
                    Description = "Khách chọn mẫu có sẵn hoặc thiết kế mới",
                    SequenceOrder = 2,
                    ApplyFor = [ItemType.DESIGN_REQUEST, ItemType.PRESET],
                    CreatedAt = now,
                    UpdatedAt = now,
                    CreatedBy = "System",
                    UpdatedBy = "System",
                    IsDeleted = false
                },
                new Milestone
                {
                    Id = "2a3477f9db80487b89ad8cdebe1e5f13",
                    Name = "Thử và điều chỉnh",
                    Description = "Thử đầm và tinh chỉnh theo phản hồi",
                    SequenceOrder = 3,
                    ApplyFor = [ItemType.DESIGN_REQUEST, ItemType.PRESET],
                    CreatedAt = now,
                    UpdatedAt = now,
                    CreatedBy = "System",
                    UpdatedBy = "System",
                    IsDeleted = false
                },
                new Milestone
                {
                    Id = "b3e3ad7a80b946f9b42f0ddf2f00aeec",
                    Name = "Kiểm tra chất lượng",
                    Description = "Đánh giá và đóng gói trước khi giao",
                    SequenceOrder = 4,
                    ApplyFor = [ItemType.DESIGN_REQUEST, ItemType.PRESET, ItemType.READY_TO_BUY],
                    CreatedAt = now,
                    UpdatedAt = now,
                    CreatedBy = "System",
                    UpdatedBy = "System",
                    IsDeleted = false
                },
                new Milestone
                {
                    Id = "316f055b3e2e4c6fa44f6a16dca0e6f1",
                    Name = "Giao hàng và phản hồi",
                    Description = "Giao đến khách và lấy đánh giá",
                    SequenceOrder = 5,
                    ApplyFor = [ItemType.DESIGN_REQUEST, ItemType.PRESET, ItemType.READY_TO_BUY],
                    CreatedAt = now,
                    UpdatedAt = now,
                    CreatedBy = "System",
                    UpdatedBy = "System",
                    IsDeleted = false
                }
            );

            modelBuilder.Entity<MaternityDressTask>().HasData(
                new MaternityDressTask
                {
                    Id = "9c8ebd7bc6d643efab54ef2b09925f04",
                    MilestoneId = "e8e56df1c7e0467d9dfc15df96735c10",
                    Name = "Tư vấn phong cách",
                    Description = "Chọn style phù hợp",
                    SequenceOrder = 1,
                    CreatedAt = now,
                    UpdatedAt = now,
                    CreatedBy = "System",
                    UpdatedBy = "System",
                    IsDeleted = false
                },
                new MaternityDressTask
                {
                    Id = "5f8ce7b8139f4b0996c2b71f901c7ef6",
                    MilestoneId = "e8e56df1c7e0467d9dfc15df96735c10",
                    Name = "Xác định dịp sử dụng",
                    Description = "Tiệc, chụp ảnh, thường ngày...",
                    SequenceOrder = 2,
                    CreatedAt = now,
                    UpdatedAt = now,
                    CreatedBy = "System",
                    UpdatedBy = "System",
                    IsDeleted = false
                },
                new MaternityDressTask
                {
                    Id = "89bb40304a8d45f7a0e2178c6aeea462",
                    MilestoneId = "ef9791c0d0e14e48b6b98432cdd9e60c",
                    Name = "Chọn mẫu sẵn",
                    Description = "Preset styles",
                    SequenceOrder = 1,
                    CreatedAt = now,
                    UpdatedAt = now,
                    CreatedBy = "System",
                    UpdatedBy = "System",
                    IsDeleted = false
                },
                new MaternityDressTask
                {
                    Id = "86c0cbaac35c4f62b029aaed8cce8e9c",
                    MilestoneId = "ef9791c0d0e14e48b6b98432cdd9e60c",
                    Name = "Thiết kế mới",
                    Description = "Tạo mẫu theo ý khách",
                    SequenceOrder = 2,
                    CreatedAt = now,
                    UpdatedAt = now,
                    CreatedBy = "System",
                    UpdatedBy = "System",
                    IsDeleted = false
                },
                new MaternityDressTask
                {
                    Id = "35f95d06385f4b6994416dcb2bba1f6f",
                    MilestoneId = "2a3477f9db80487b89ad8cdebe1e5f13",
                    Name = "Thử đầm",
                    Description = "Thử trực tiếp hoặc feedback ảnh",
                    SequenceOrder = 1,
                    CreatedAt = now,
                    UpdatedAt = now,
                    CreatedBy = "System",
                    UpdatedBy = "System",
                    IsDeleted = false
                },
                new MaternityDressTask
                {
                    Id = "6fd0f813c191449ab2b7a07039cd9e7c",
                    MilestoneId = "2a3477f9db80487b89ad8cdebe1e5f13",
                    Name = "Tinh chỉnh",
                    Description = "Sửa theo feedback",
                    SequenceOrder = 2,
                    CreatedAt = now,
                    UpdatedAt = now,
                    CreatedBy = "System",
                    UpdatedBy = "System",
                    IsDeleted = false
                },
                new MaternityDressTask
                {
                    Id = "f48b2f18763e4e6b80b4a85ce95b8264",
                    MilestoneId = "b3e3ad7a80b946f9b42f0ddf2f00aeec",
                    Name = "Kiểm tra ngoại hình",
                    Description = "Đường may, màu sắc, nếp vải",
                    SequenceOrder = 1,
                    CreatedAt = now,
                    UpdatedAt = now,
                    CreatedBy = "System",
                    UpdatedBy = "System",
                    IsDeleted = false
                },
                new MaternityDressTask
                {
                    Id = "d6c9fddce1cc4cd28bd54094f5e48c56",
                    MilestoneId = "b3e3ad7a80b946f9b42f0ddf2f00aeec",
                    Name = "Đo đạc & so sánh",
                    Description = "Kiểm tra số đo đúng không",
                    SequenceOrder = 2,
                    CreatedAt = now,
                    UpdatedAt = now,
                    CreatedBy = "System",
                    UpdatedBy = "System",
                    IsDeleted = false
                },
                new MaternityDressTask
                {
                    Id = "197fd3b727a1400cbde58a4f28ab5f93",
                    MilestoneId = "b3e3ad7a80b946f9b42f0ddf2f00aeec",
                    Name = "Đóng gói sản phẩm",
                    Description = "Gói chuẩn theo mẫu thương hiệu",
                    SequenceOrder = 3,
                    CreatedAt = now,
                    UpdatedAt = now,
                    CreatedBy = "System",
                    UpdatedBy = "System",
                    IsDeleted = false
                },
                new MaternityDressTask
                {
                    Id = "c1ff5d242bb14aa59e17c34753c9e189",
                    MilestoneId = "316f055b3e2e4c6fa44f6a16dca0e6f1",
                    Name = "Giao hàng",
                    Description = "Ship theo địa chỉ khách chọn",
                    SequenceOrder = 1,
                    CreatedAt = now,
                    UpdatedAt = now,
                    CreatedBy = "System",
                    UpdatedBy = "System",
                    IsDeleted = false
                },
                new MaternityDressTask
                {
                    Id = "fb2e3a1c38f14091a8b016dfbdcf80e0",
                    MilestoneId = "316f055b3e2e4c6fa44f6a16dca0e6f1",
                    Name = "Ghi nhận phản hồi",
                    Description = "Gửi form đánh giá hoặc khảo sát",
                    SequenceOrder = 2,
                    CreatedAt = now,
                    UpdatedAt = now,
                    CreatedBy = "System",
                    UpdatedBy = "System",
                    IsDeleted = false
                }
            );

            #endregion

            #region Seed Diary, Address, Voucher, Branch
            modelBuilder.Entity<MeasurementDiary>().HasData(
                new MeasurementDiary
                {
                    Id = "107edb79f9f04079afad1a8a42141578",
                    UserId = "f49aa51bbd304e77933e24bbed65b165",
                    Name = "Nhat Ky Cua Danh",
                    Age = 23,
                    Height = 170.0f,
                    Weight = 85.0f,
                    Bust = 72.3f,
                    Waist = 85.2f,
                    Hip = 92.6f,
                    PregnancyStartDate = new DateTime(2025, 04, 19, 00, 00, 00, DateTimeKind.Utc),
                    FirstDateOfLastPeriod = new DateTime(2025, 05, 03, 00, 00, 00, DateTimeKind.Utc),
                    AverageMenstrualCycle = 0,
                    NumberOfPregnancy = 3,
                    UltrasoundDate = null,
                    WeeksFromUltrasound = 0,
                    IsActive = true,
                    CreatedBy = "System",
                    UpdatedBy = "System",
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    IsDeleted = false,
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
                    MapId = "rGHobqy_rVN8V-EjqVDYaXe91lWjE3_1nq9TL69Sd0xO2tkirG-8T3iyZS-thm2aSb1LMaIydjs5_21tYrn-G5UnaMBg",
                    Province = "Bình Dương",
                    District = "Dĩ An",
                    Ward = "Đông Hoà",
                    Street = "Nhà Văn hóa Sinh viên TP.HCM, Lưu Hữu Phước Tân Lập",
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
                    MapId = "qGHuRFi-pARDYqQuX6KC62zUoh2iWOHabK5PNadiqF9zovo3o1m8W3WNdRC4T7SQdrNEVpNyd8sro3W8k2N78D2MZTKScpLbdrJEUJWakl93iFcEkvuCknWgXy-USBenS",
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
                    MapId = "TErhHQFjiV1t_21NqqlGz9XmFIDydhW3HfKxQLKyd7k90rOEYrFe7M0quV16ubp6ZSr6vYkgyjc2ufK9UrXyF5tjZMxs",
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
                    Id = "ed58656f238a4835ae62467f0dbce35c",
                    MeasurementDiaryId = "107edb79f9f04079afad1a8a42141578",
                    WeekOfPregnancy = 13,
                    Weight = 85.0f,
                    Neck = 34.0f,
                    Coat = 78.3f,
                    Bust = 72.3f,
                    ChestAround = 76.3f,
                    Stomach = 91.7f,
                    PantsWaist = 81.7f,
                    Thigh = 49.3f,
                    DressLength = 112.2f,
                    SleeveLength = 26.6f,
                    ShoulderWidth = 39.5f,
                    Waist = 85.2f,
                    LegLength = 81.6f,
                    Hip = 92.6f,
                    CreatedBy = "System",
                    UpdatedBy = "System",
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    IsDeleted = false,
                }
            );

            #endregion
        }
    }
}