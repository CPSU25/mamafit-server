﻿using MamaFit.BusinessObjects.Entity;
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

            #region Seed Category,Style,Component,ComponentOption
            var createdAt = DateTime.UtcNow;

            var categories = new List<Category>();
            var styles = new List<Style>();
            var components = new List<Component>();
            var options = new List<ComponentOption>();

            for (int i = 1; i <= 5; i++)
            {
                var categoryId = $"cat{i}";
                categories.Add(new Category
                {
                    Id = categoryId,
                    Name = $"Category {i}",
                    Description = $"Description for Category {i}",
                    Images = new List<string> { $"https://example.com/category{i}.jpg" },
                    CreatedAt = createdAt,
                    UpdatedAt = createdAt,
                    IsDeleted = false
                });

                for (int j = 1; j <= 4; j++)
                {
                    var styleId = $"style{i}{j}";
                    styles.Add(new Style
                    {
                        Id = styleId,
                        CategoryId = categoryId,
                        Name = $"Style {i}-{j}",
                        Description = $"Description for Style {i}-{j}",
                        IsCustom = j % 2 == 0,
                        Images = new List<string> { $"https://example.com/style{i}{j}.jpg" },
                        CreatedAt = createdAt,
                        UpdatedAt = createdAt,
                        IsDeleted = false
                    });

                    for (int k = 1; k <= 4; k++)
                    {
                        var componentId = $"comp{i}{j}{k}";
                        components.Add(new Component
                        {
                            Id = componentId,
                            StyleId = styleId,
                            Name = $"Component {i}-{j}-{k}",
                            Description = $"Description for Component {i}-{j}-{k}",
                            Images = new List<string> { $"https://example.com/component{i}{j}{k}.jpg" },
                            CreatedAt = createdAt,
                            UpdatedAt = createdAt,
                            IsDeleted = false
                        });

                        for (int l = 1; l <= 5; l++)
                        {
                            var optionId = $"opt{i}{j}{k}{l}";
                            options.Add(new ComponentOption
                            {
                                Id = optionId,
                                ComponentId = componentId,
                                Name = $"Option {i}-{j}-{k}-{l}",
                                Price = new Random().Next(50000, 100001),
                                Description = $"Description for Option {i}-{j}-{k}-{l}",
                                Images = new List<string> { $"https://example.com/option{i}{j}{k}{l}.jpg" },
                                CreatedAt = createdAt,
                                UpdatedAt = createdAt,
                                IsDeleted = false
                            });
                        }
                    }
                }
            }

            modelBuilder.Entity<Category>().HasData(categories);
            modelBuilder.Entity<Style>().HasData(styles);
            modelBuilder.Entity<Component>().HasData(components);
            modelBuilder.Entity<ComponentOption>().HasData(options);
            #endregion
        }
    }
}