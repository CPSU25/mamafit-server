using MamaFit.BusinessObjects.Entity;
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
                    Id = "b8d237b8b6f849988d60c6c3c1d0a943", RoleName = "User"
                },
                new ApplicationUserRole
                {
                    Id = "bf081015e17a41b8b1cae65b1b17cfdb", RoleName = "BranchManager"
                },
                new ApplicationUserRole
                {
                    Id = "c9118b99c0ad486dbb18560a916b630c", RoleName = "BranchStaff"
                },
                new ApplicationUserRole
                {
                    Id = "e5b0f987fbf44608b7a6a2d0e313b3b2", RoleName = "Designer"
                },
                new ApplicationUserRole
                {
                    Id = "a3cb88edaf2b4718a9986010c5b9c1d7", RoleName = "Manager"
                },
                new ApplicationUserRole
                {
                    Id = "5ed8cfa9b62d433c88ab097b6d2baccd", RoleName = "Staff"
                },
                new ApplicationUserRole
                {
                    Id = "2e7b5a97e42e4e84a08ffbe0bc05d2ea", RoleName = "Admin"
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
            modelBuilder.Entity<Category>().HasData(
               new Category { Id = "a1b2c3d4e5f60123456789abcdefabcd", Name = "Category 1", Description = "Description for Category 1" },
               new Category { Id = "b1c2d3e4f5a60123456789abcdefabcd", Name = "Category 2", Description = "Description for Category 2" },
               new Category { Id = "c1d2e3f4a5b60123456789abcdefabcd", Name = "Category 3", Description = "Description for Category 3" },
               new Category { Id = "d1e2f3a4b5c60123456789abcdefabcd", Name = "Category 4", Description = "Description for Category 4" },
               new Category { Id = "e1f2a3b4c5d60123456789abcdefabcd", Name = "Category 5", Description = "Description for Category 5" }
           );

            // Seed Style
            modelBuilder.Entity<Style>().HasData(
                new Style { Id = "f1a2b3c4d5e60123456789abcdefabcd", Name = "Style 1", Description = "Description for Style 1" },
                new Style { Id = "a2b3c4d5e6f70123456789abcdefabcd", Name = "Style 2", Description = "Description for Style 2" },
                new Style { Id = "b2c3d4e5f6a80123456789abcdefabcd", Name = "Style 3", Description = "Description for Style 3" },
                new Style { Id = "c2d3e4f5a6b90123456789abcdefabcd", Name = "Style 4", Description = "Description for Style 4" },
                new Style { Id = "d2e3f4a5b6c00123456789abcdefabcd", Name = "Style 5", Description = "Description for Style 5" }
            );

            // Seed Component
            modelBuilder.Entity<Component>().HasData(
                new Component { Id = "bb3a2d487e9d4bcda5f7b2e7c58a5f40", Name = "Component 1", Description = "Description for Component 1" },
                new Component { Id = "cc4b3e598fab5cd1b6f8c3f8d69b6f51", Name = "Component 2", Description = "Description for Component 2" },
                new Component { Id = "dd5c4f6a0abc6de2c7g9d4g9e7ac7g62", Name = "Component 3", Description = "Description for Component 3" },
                new Component { Id = "ee6d5g7b1bcd7ef3d8h0e5h0f8bd8h73", Name = "Component 4", Description = "Description for Component 4" },
                new Component { Id = "ff7e6h8c2cde8fg4e9i1f6i1g9ce9i84", Name = "Component 5", Description = "Description for Component 5" }
            );

            // Seed ComponentOption (Images lưu dưới dạng JSON string)
            modelBuilder.Entity<ComponentOption>().HasData(
                new ComponentOption
                {
                    Id = "00112233445566778899aabbccddeeff",
                    Name = "Option 1",
                    Description = "Description Option 1",
                    ComponentOptionType = Enum.ComponentOptionType.APPROVAL_PENDING,
                    ComponentId = "bb3a2d487e9d4bcda5f7b2e7c58a5f40"
                },
                new ComponentOption
                {
                    Id = "112233445566778899aabbccddeeff00",
                    Name = "Option 2",
                    Description = "Description Option 2",
                    ComponentOptionType = Enum.ComponentOptionType.QUOTATION_PENDING,
                    ComponentId = "cc4b3e598fab5cd1b6f8c3f8d69b6f51"
                },
                new ComponentOption
                {
                    Id = "2233445566778899aabbccddeeff0011",
                    Name = "Option 3",
                    Description = "Description Option 3",
                    ComponentOptionType = Enum.ComponentOptionType.APPROVAL_PENDING,
                    ComponentId = "dd5c4f6a0abc6de2c7g9d4g9e7ac7g62"
                },
                new ComponentOption
                {
                    Id = "33445566778899aabbccddeeff001122",
                    Name = "Option 4",
                    Description = "Description Option 4",
                    ComponentOptionType = Enum.ComponentOptionType.QUOTATION_PENDING,
                    ComponentId = "ee6d5g7b1bcd7ef3d8h0e5h0f8bd8h73"
                },
                new ComponentOption
                {
                    Id = "445566778899aabbccddeeff00112233",
                    Name = "Option 5",
                    Description = "Description Option 5",
                    ComponentOptionType = Enum.ComponentOptionType.APPROVED,
                    ComponentId = "ff7e6h8c2cde8fg4e9i1f6i1g9ce9i84"
                }
            );
        }
    }
}