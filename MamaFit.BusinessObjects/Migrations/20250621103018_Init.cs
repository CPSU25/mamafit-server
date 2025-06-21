using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MamaFit.BusinessObjects.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Category",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true),
                    Images = table.Column<List<string>>(type: "text[]", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Category", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MaternityDressService",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MaternityDressService", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Milestone",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Milestone", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Role",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    RoleName = table.Column<string>(type: "text", nullable: true),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Role", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "VoucherBatch",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    BatchName = table.Column<string>(type: "text", nullable: false),
                    BatchCode = table.Column<string>(type: "text", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true),
                    StartDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    EndDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    TotalQuantity = table.Column<int>(type: "integer", nullable: true),
                    RemainingQuantity = table.Column<int>(type: "integer", nullable: true),
                    DiscountType = table.Column<string>(type: "text", nullable: true),
                    DiscountPercentValue = table.Column<int>(type: "integer", nullable: true),
                    MinimumOrderValue = table.Column<float>(type: "real", nullable: true),
                    MaximumDiscountValue = table.Column<float>(type: "real", nullable: true),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VoucherBatch", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Style",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    CategoryId = table.Column<string>(type: "text", nullable: true),
                    Name = table.Column<string>(type: "text", nullable: true),
                    IsCustom = table.Column<bool>(type: "boolean", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true),
                    Images = table.Column<List<string>>(type: "text[]", nullable: true),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Style", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Style_Category_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Category",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "MaternityDressServiceOption",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    MaternityDressServiceId = table.Column<string>(type: "text", nullable: true),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true),
                    Price = table.Column<decimal>(type: "numeric", nullable: false),
                    Position = table.Column<string>(type: "text", nullable: true),
                    Size = table.Column<string>(type: "text", nullable: true),
                    ItemServiceType = table.Column<int>(type: "integer", nullable: true),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MaternityDressServiceOption", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MaternityDressServiceOption_MaternityDressService_Maternity~",
                        column: x => x.MaternityDressServiceId,
                        principalTable: "MaternityDressService",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "MaternityDressTask",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    MilestoneId = table.Column<string>(type: "text", nullable: true),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true),
                    Type = table.Column<int>(type: "integer", nullable: true),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MaternityDressTask", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MaternityDressTask_Milestone_MilestoneId",
                        column: x => x.MilestoneId,
                        principalTable: "Milestone",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    UserName = table.Column<string>(type: "text", nullable: true),
                    UserEmail = table.Column<string>(type: "text", nullable: true),
                    HashPassword = table.Column<string>(type: "text", nullable: true),
                    Salt = table.Column<string>(type: "text", nullable: true),
                    FullName = table.Column<string>(type: "text", nullable: true),
                    ProfilePicture = table.Column<string>(type: "text", nullable: true),
                    DateOfBirth = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    PhoneNumber = table.Column<string>(type: "text", nullable: true),
                    IsVerify = table.Column<bool>(type: "boolean", nullable: false),
                    RoleId = table.Column<string>(type: "text", nullable: true),
                    CurrentConnectionId = table.Column<string>(type: "text", nullable: true),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                    table.ForeignKey(
                        name: "FK_User_Role_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Role",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Component",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    StyleId = table.Column<string>(type: "text", nullable: true),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true),
                    Images = table.Column<List<string>>(type: "text[]", nullable: true),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Component", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Component_Style_StyleId",
                        column: x => x.StyleId,
                        principalTable: "Style",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "MaternityDress",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true),
                    Images = table.Column<List<string>>(type: "text[]", nullable: false),
                    Slug = table.Column<string>(type: "text", nullable: true),
                    AverageRating = table.Column<float>(type: "real", nullable: false),
                    TotalRating = table.Column<int>(type: "integer", nullable: false),
                    Rating = table.Column<int>(type: "integer", nullable: true),
                    StyleId = table.Column<string>(type: "text", nullable: true),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MaternityDress", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MaternityDress_Style_StyleId",
                        column: x => x.StyleId,
                        principalTable: "Style",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Address",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    UserId = table.Column<string>(type: "text", nullable: true),
                    MapId = table.Column<string>(type: "text", nullable: true),
                    ShortName = table.Column<string>(type: "text", nullable: true),
                    LongName = table.Column<string>(type: "text", nullable: true),
                    Latitude = table.Column<float>(type: "real", nullable: true),
                    Longitude = table.Column<float>(type: "real", nullable: true),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Address", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Address_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Branch",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    BranchManagerId = table.Column<string>(type: "text", nullable: true),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true),
                    OpeningHour = table.Column<string>(type: "text", nullable: true),
                    Images = table.Column<List<string>>(type: "text[]", nullable: false),
                    MapId = table.Column<string>(type: "text", nullable: true),
                    ShortName = table.Column<string>(type: "text", nullable: true),
                    LongName = table.Column<string>(type: "text", nullable: true),
                    Latitude = table.Column<float>(type: "real", nullable: true),
                    Longitude = table.Column<float>(type: "real", nullable: true),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Branch", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Branch_User_BranchManagerId",
                        column: x => x.BranchManagerId,
                        principalTable: "User",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ChatRoom",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    ApplicationUserId = table.Column<string>(type: "text", nullable: true),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChatRoom", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ChatRoom_User_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "User",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "MeasurementDiary",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    UserId = table.Column<string>(type: "text", nullable: true),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Age = table.Column<int>(type: "integer", nullable: true),
                    Height = table.Column<float>(type: "real", nullable: false),
                    Weight = table.Column<float>(type: "real", nullable: false),
                    Bust = table.Column<float>(type: "real", nullable: false),
                    Waist = table.Column<float>(type: "real", nullable: false),
                    Hip = table.Column<float>(type: "real", nullable: false),
                    FirstDateOfLastPeriod = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    AverageMenstrualCycle = table.Column<int>(type: "integer", nullable: true),
                    NumberOfPregnancy = table.Column<int>(type: "integer", nullable: true),
                    UltrasoundDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    WeeksFromUltrasound = table.Column<int>(type: "integer", nullable: true),
                    PregnancyStartDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MeasurementDiary", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MeasurementDiary_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Notification",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    NotificationTitle = table.Column<string>(type: "text", nullable: true),
                    NotificationContent = table.Column<string>(type: "text", nullable: true),
                    Type = table.Column<int>(type: "integer", nullable: true),
                    ActionUrl = table.Column<string>(type: "text", nullable: true),
                    Metadata = table.Column<string>(type: "text", nullable: true),
                    IsRead = table.Column<bool>(type: "boolean", nullable: true),
                    ReceiverId = table.Column<string>(type: "text", nullable: true),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notification", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Notification_User_ReceiverId",
                        column: x => x.ReceiverId,
                        principalTable: "User",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "OTP",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    UserId = table.Column<string>(type: "text", nullable: true),
                    Code = table.Column<string>(type: "text", nullable: true),
                    ExpiredAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    OTPType = table.Column<int>(type: "integer", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OTP", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OTP_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "UserToken",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    UserId = table.Column<string>(type: "text", nullable: false),
                    Token = table.Column<string>(type: "text", nullable: true),
                    ExpiredAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsRevoked = table.Column<bool>(type: "boolean", nullable: true),
                    TokenType = table.Column<int>(type: "integer", nullable: true),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserToken", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserToken_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "VoucherDiscount",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    UserId = table.Column<string>(type: "text", nullable: true),
                    VoucherBatchId = table.Column<string>(type: "text", nullable: true),
                    Code = table.Column<string>(type: "text", nullable: true),
                    Status = table.Column<int>(type: "integer", nullable: true),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VoucherDiscount", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VoucherDiscount_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_VoucherDiscount_VoucherBatch_VoucherBatchId",
                        column: x => x.VoucherBatchId,
                        principalTable: "VoucherBatch",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ComponentOption",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    ComponentId = table.Column<string>(type: "text", nullable: true),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true),
                    Images = table.Column<List<string>>(type: "text[]", nullable: true),
                    ComponentOptionType = table.Column<int>(type: "integer", nullable: true),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ComponentOption", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ComponentOption_Component_ComponentId",
                        column: x => x.ComponentId,
                        principalTable: "Component",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "MaternityDressDetail",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    MaternityDressId = table.Column<string>(type: "text", nullable: true),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true),
                    Image = table.Column<string>(type: "text", nullable: true),
                    Color = table.Column<string>(type: "text", nullable: true),
                    Size = table.Column<string>(type: "text", nullable: true),
                    Price = table.Column<float>(type: "real", nullable: false),
                    Quantity = table.Column<int>(type: "integer", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MaternityDressDetail", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MaternityDressDetail_MaternityDress_MaternityDressId",
                        column: x => x.MaternityDressId,
                        principalTable: "MaternityDress",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Appointment",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    UserId = table.Column<string>(type: "text", nullable: true),
                    StaffId = table.Column<string>(type: "text", nullable: true),
                    BranchId = table.Column<string>(type: "text", nullable: true),
                    FullName = table.Column<string>(type: "text", nullable: true),
                    PhoneNumber = table.Column<string>(type: "text", nullable: true),
                    BookingTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Note = table.Column<string>(type: "text", nullable: true),
                    Status = table.Column<int>(type: "integer", nullable: true),
                    CanceledAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CanceledReason = table.Column<string>(type: "text", nullable: true),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Appointment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Appointment_Branch_BranchId",
                        column: x => x.BranchId,
                        principalTable: "Branch",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Appointment_User_StaffId",
                        column: x => x.StaffId,
                        principalTable: "User",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Appointment_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ChatMessage",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    Message = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    SenderId = table.Column<string>(type: "text", nullable: false),
                    ChatRoomId = table.Column<string>(type: "text", nullable: false),
                    Type = table.Column<int>(type: "integer", nullable: false),
                    IsRead = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChatMessage", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ChatMessage_ChatRoom_ChatRoomId",
                        column: x => x.ChatRoomId,
                        principalTable: "ChatRoom",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ChatMessage_User_SenderId",
                        column: x => x.SenderId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ChatRoomMember",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "text", nullable: false),
                    ChatRoomId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChatRoomMember", x => new { x.UserId, x.ChatRoomId });
                    table.ForeignKey(
                        name: "FK_ChatRoomMember_ChatRoom_ChatRoomId",
                        column: x => x.ChatRoomId,
                        principalTable: "ChatRoom",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ChatRoomMember_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Measurement",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    MeasurementDiaryId = table.Column<string>(type: "text", nullable: true),
                    WeekOfPregnancy = table.Column<int>(type: "integer", nullable: false),
                    Weight = table.Column<float>(type: "real", nullable: false),
                    Neck = table.Column<float>(type: "real", nullable: false),
                    Coat = table.Column<float>(type: "real", nullable: false),
                    Bust = table.Column<float>(type: "real", nullable: false),
                    ChestAround = table.Column<float>(type: "real", nullable: false),
                    Stomach = table.Column<float>(type: "real", nullable: false),
                    PantsWaist = table.Column<float>(type: "real", nullable: false),
                    Thigh = table.Column<float>(type: "real", nullable: false),
                    DressLength = table.Column<float>(type: "real", nullable: false),
                    SleeveLength = table.Column<float>(type: "real", nullable: false),
                    ShoulderWidth = table.Column<float>(type: "real", nullable: false),
                    Waist = table.Column<float>(type: "real", nullable: false),
                    LegLength = table.Column<float>(type: "real", nullable: false),
                    Hip = table.Column<float>(type: "real", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Measurement", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Measurement_MeasurementDiary_MeasurementDiaryId",
                        column: x => x.MeasurementDiaryId,
                        principalTable: "MeasurementDiary",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Order",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    ParentOrderId = table.Column<string>(type: "text", nullable: true),
                    BranchId = table.Column<string>(type: "text", nullable: true),
                    UserId = table.Column<string>(type: "text", nullable: true),
                    VoucherDiscountId = table.Column<string>(type: "text", nullable: true),
                    IsOnline = table.Column<bool>(type: "boolean", nullable: true),
                    Type = table.Column<int>(type: "integer", nullable: false),
                    Code = table.Column<string>(type: "text", nullable: true),
                    Status = table.Column<int>(type: "integer", nullable: true),
                    TotalAmount = table.Column<float>(type: "real", nullable: false),
                    ShippingFee = table.Column<float>(type: "real", nullable: false),
                    PaymentStatus = table.Column<int>(type: "integer", nullable: false),
                    PaymentMethod = table.Column<int>(type: "integer", nullable: false),
                    DeliveryMethod = table.Column<int>(type: "integer", nullable: false),
                    PaymentType = table.Column<int>(type: "integer", nullable: false),
                    CanceledAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CanceledReason = table.Column<string>(type: "text", nullable: true),
                    SubTotalAmount = table.Column<float>(type: "real", nullable: false),
                    WarrantyCode = table.Column<string>(type: "text", nullable: true),
                    MapId = table.Column<string>(type: "text", nullable: true),
                    ShortName = table.Column<string>(type: "text", nullable: true),
                    LongName = table.Column<string>(type: "text", nullable: true),
                    Latitude = table.Column<float>(type: "real", nullable: true),
                    Longitude = table.Column<float>(type: "real", nullable: true),
                    MeasurementDiaryId = table.Column<string>(type: "text", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Order", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Order_Branch_BranchId",
                        column: x => x.BranchId,
                        principalTable: "Branch",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Order_MeasurementDiary_MeasurementDiaryId",
                        column: x => x.MeasurementDiaryId,
                        principalTable: "MeasurementDiary",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Order_Order_ParentOrderId",
                        column: x => x.ParentOrderId,
                        principalTable: "Order",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Order_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Order_VoucherDiscount_VoucherDiscountId",
                        column: x => x.VoucherDiscountId,
                        principalTable: "VoucherDiscount",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "BranchMaternityDressDetail",
                columns: table => new
                {
                    MaternityDressDetailId = table.Column<string>(type: "text", nullable: false),
                    BranchId = table.Column<string>(type: "text", nullable: false),
                    Quantity = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BranchMaternityDressDetail", x => new { x.BranchId, x.MaternityDressDetailId });
                    table.ForeignKey(
                        name: "FK_BranchMaternityDressDetail_Branch_BranchId",
                        column: x => x.BranchId,
                        principalTable: "Branch",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_BranchMaternityDressDetail_MaternityDressDetail_MaternityDr~",
                        column: x => x.MaternityDressDetailId,
                        principalTable: "MaternityDressDetail",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "CartItem",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    UserId = table.Column<string>(type: "text", nullable: true),
                    Quantity = table.Column<int>(type: "integer", nullable: false),
                    TotalAmount = table.Column<float>(type: "real", nullable: false),
                    MaternityDressDetailId = table.Column<string>(type: "text", nullable: true),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CartItem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CartItem_MaternityDressDetail_MaternityDressDetailId",
                        column: x => x.MaternityDressDetailId,
                        principalTable: "MaternityDressDetail",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CartItem_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "OrderItem",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    OrderId = table.Column<string>(type: "text", nullable: true),
                    MaternityDressDetailId = table.Column<string>(type: "text", nullable: true),
                    ItemType = table.Column<int>(type: "integer", nullable: true),
                    Price = table.Column<float>(type: "real", nullable: false),
                    Quantity = table.Column<int>(type: "integer", nullable: false),
                    WarrantyDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    WarrantyNumber = table.Column<int>(type: "integer", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderItem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderItem_MaternityDressDetail_MaternityDressDetailId",
                        column: x => x.MaternityDressDetailId,
                        principalTable: "MaternityDressDetail",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_OrderItem_Order_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Order",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Transaction",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    OrderId = table.Column<string>(type: "text", nullable: true),
                    SepayId = table.Column<string>(type: "text", nullable: true),
                    Gateway = table.Column<string>(type: "text", nullable: true),
                    TransactionDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    AccountNumber = table.Column<string>(type: "text", nullable: true),
                    Code = table.Column<string>(type: "text", nullable: true),
                    Content = table.Column<string>(type: "text", nullable: true),
                    TransferType = table.Column<string>(type: "text", nullable: true),
                    TransferAmount = table.Column<float>(type: "real", nullable: true),
                    Accumulated = table.Column<string>(type: "text", nullable: true),
                    SubAccount = table.Column<string>(type: "text", nullable: true),
                    ReferenceCode = table.Column<string>(type: "text", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transaction", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Transaction_Order_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Order",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "DesignRequest",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    UserId = table.Column<string>(type: "text", nullable: true),
                    OrderItemId = table.Column<string>(type: "text", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true),
                    Images = table.Column<List<string>>(type: "text[]", nullable: true),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DesignRequest", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DesignRequest_OrderItem_OrderItemId",
                        column: x => x.OrderItemId,
                        principalTable: "OrderItem",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_DesignRequest_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Feedback",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    UserId = table.Column<string>(type: "text", nullable: true),
                    OrderItemId = table.Column<string>(type: "text", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true),
                    Images = table.Column<List<string>>(type: "text[]", nullable: true),
                    Rated = table.Column<float>(type: "real", nullable: true),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Feedback", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Feedback_OrderItem_OrderItemId",
                        column: x => x.OrderItemId,
                        principalTable: "OrderItem",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Feedback_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "OrderItemServiceOption",
                columns: table => new
                {
                    OrderItemId = table.Column<string>(type: "text", nullable: false),
                    MaternityDressServiceOptionId = table.Column<string>(type: "text", nullable: false),
                    Value = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderItemServiceOption", x => new { x.MaternityDressServiceOptionId, x.OrderItemId });
                    table.ForeignKey(
                        name: "FK_OrderItemServiceOption_MaternityDressServiceOption_Maternit~",
                        column: x => x.MaternityDressServiceOptionId,
                        principalTable: "MaternityDressServiceOption",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrderItemServiceOption_OrderItem_OrderItemId",
                        column: x => x.OrderItemId,
                        principalTable: "OrderItem",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OrderItemsTasks",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "text", nullable: false),
                    OrderItemId = table.Column<string>(type: "text", nullable: false),
                    MilestoneId = table.Column<string>(type: "text", nullable: false),
                    Image = table.Column<string>(type: "text", nullable: true),
                    Note = table.Column<string>(type: "text", nullable: true),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    MaternityDressTaskId = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderItemsTasks", x => new { x.UserId, x.OrderItemId, x.MilestoneId });
                    table.ForeignKey(
                        name: "FK_OrderItemsTasks_MaternityDressTask_MaternityDressTaskId",
                        column: x => x.MaternityDressTaskId,
                        principalTable: "MaternityDressTask",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_OrderItemsTasks_Milestone_MilestoneId",
                        column: x => x.MilestoneId,
                        principalTable: "Milestone",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrderItemsTasks_OrderItem_OrderItemId",
                        column: x => x.OrderItemId,
                        principalTable: "OrderItem",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrderItemsTasks_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WarrantyRequest",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    OriginalOrderItemId = table.Column<string>(type: "text", nullable: true),
                    WarrantyOrderItemId = table.Column<string>(type: "text", nullable: true),
                    Images = table.Column<List<string>>(type: "text[]", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true),
                    IsFactoryError = table.Column<bool>(type: "boolean", nullable: true),
                    RejectedReason = table.Column<string>(type: "text", nullable: true),
                    Fee = table.Column<float>(type: "real", nullable: true),
                    Status = table.Column<int>(type: "integer", nullable: true),
                    WarrantyRound = table.Column<int>(type: "integer", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WarrantyRequest", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WarrantyRequest_OrderItem_OriginalOrderItemId",
                        column: x => x.OriginalOrderItemId,
                        principalTable: "OrderItem",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_WarrantyRequest_OrderItem_WarrantyOrderItemId",
                        column: x => x.WarrantyOrderItemId,
                        principalTable: "OrderItem",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "MaternityDressCustomization",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    UserId = table.Column<string>(type: "text", nullable: true),
                    OrderItemId = table.Column<string>(type: "text", nullable: true),
                    DesignRequestId = table.Column<string>(type: "text", nullable: true),
                    CustomizationType = table.Column<int>(type: "integer", nullable: true),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MaternityDressCustomization", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MaternityDressCustomization_DesignRequest_DesignRequestId",
                        column: x => x.DesignRequestId,
                        principalTable: "DesignRequest",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_MaternityDressCustomization_OrderItem_OrderItemId",
                        column: x => x.OrderItemId,
                        principalTable: "OrderItem",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_MaternityDressCustomization_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "WarrantyHistory",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    WarrantyRequestId = table.Column<string>(type: "text", nullable: true),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WarrantyHistory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WarrantyHistory_WarrantyRequest_WarrantyRequestId",
                        column: x => x.WarrantyRequestId,
                        principalTable: "WarrantyRequest",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "MaternityDressSelection",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    ComponentOptionId = table.Column<string>(type: "text", nullable: true),
                    MaternityDressCustomizationId = table.Column<string>(type: "text", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MaternityDressSelection", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MaternityDressSelection_ComponentOption_ComponentOptionId",
                        column: x => x.ComponentOptionId,
                        principalTable: "ComponentOption",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_MaternityDressSelection_MaternityDressCustomization_Materni~",
                        column: x => x.MaternityDressCustomizationId,
                        principalTable: "MaternityDressCustomization",
                        principalColumn: "Id");
                });

            migrationBuilder.InsertData(
                table: "Role",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "IsDeleted", "RoleName", "UpdatedAt", "UpdatedBy" },
                values: new object[,]
                {
                    { "2e7b5a97e42e4e84a08ffbe0bc05d2ea", new DateTime(2025, 6, 21, 10, 30, 17, 748, DateTimeKind.Utc).AddTicks(2340), null, false, "Admin", new DateTime(2025, 6, 21, 10, 30, 17, 748, DateTimeKind.Utc).AddTicks(2340), null },
                    { "5ed8cfa9b62d433c88ab097b6d2baccd", new DateTime(2025, 6, 21, 10, 30, 17, 748, DateTimeKind.Utc).AddTicks(2340), null, false, "Staff", new DateTime(2025, 6, 21, 10, 30, 17, 748, DateTimeKind.Utc).AddTicks(2340), null },
                    { "a3cb88edaf2b4718a9986010c5b9c1d7", new DateTime(2025, 6, 21, 10, 30, 17, 748, DateTimeKind.Utc).AddTicks(2340), null, false, "Manager", new DateTime(2025, 6, 21, 10, 30, 17, 748, DateTimeKind.Utc).AddTicks(2340), null },
                    { "b8d237b8b6f849988d60c6c3c1d0a943", new DateTime(2025, 6, 21, 10, 30, 17, 748, DateTimeKind.Utc).AddTicks(2320), null, false, "User", new DateTime(2025, 6, 21, 10, 30, 17, 748, DateTimeKind.Utc).AddTicks(2320), null },
                    { "bf081015e17a41b8b1cae65b1b17cfdb", new DateTime(2025, 6, 21, 10, 30, 17, 748, DateTimeKind.Utc).AddTicks(2320), null, false, "BranchManager", new DateTime(2025, 6, 21, 10, 30, 17, 748, DateTimeKind.Utc).AddTicks(2320), null },
                    { "c9118b99c0ad486dbb18560a916b630c", new DateTime(2025, 6, 21, 10, 30, 17, 748, DateTimeKind.Utc).AddTicks(2330), null, false, "BranchStaff", new DateTime(2025, 6, 21, 10, 30, 17, 748, DateTimeKind.Utc).AddTicks(2330), null },
                    { "e5b0f987fbf44608b7a6a2d0e313b3b2", new DateTime(2025, 6, 21, 10, 30, 17, 748, DateTimeKind.Utc).AddTicks(2330), null, false, "Designer", new DateTime(2025, 6, 21, 10, 30, 17, 748, DateTimeKind.Utc).AddTicks(2330), null }
                });

            migrationBuilder.InsertData(
                table: "User",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "CurrentConnectionId", "DateOfBirth", "FullName", "HashPassword", "IsDeleted", "IsVerify", "PhoneNumber", "ProfilePicture", "RoleId", "Salt", "UpdatedAt", "UpdatedBy", "UserEmail", "UserName" },
                values: new object[,]
                {
                    { "08ee8586464b43dd9a4507add95b281c", new DateTime(2025, 6, 21, 10, 30, 17, 748, DateTimeKind.Utc).AddTicks(2490), null, null, null, "Designer", "u+7jaa75MJns9ST/6FSw4aQU+zSF/iNldvzpJxBFzwk=", false, true, null, null, "e5b0f987fbf44608b7a6a2d0e313b3b2", "R0kox04KXBTPFFlKjfxIzNeMzlbH1rGO/YJDtl0N894=", new DateTime(2025, 6, 21, 10, 30, 17, 748, DateTimeKind.Utc).AddTicks(2490), null, "designer@mamafit.com", "designer" },
                    { "1a3bcd12345678901234567890123456", new DateTime(2025, 6, 21, 10, 30, 17, 748, DateTimeKind.Utc).AddTicks(2460), null, null, null, "Admin", "u+7jaa75MJns9ST/6FSw4aQU+zSF/iNldvzpJxBFzwk=", false, true, null, null, "2e7b5a97e42e4e84a08ffbe0bc05d2ea", "R0kox04KXBTPFFlKjfxIzNeMzlbH1rGO/YJDtl0N894=", new DateTime(2025, 6, 21, 10, 30, 17, 748, DateTimeKind.Utc).AddTicks(2460), null, "admin@mamafit.com", "admin" },
                    { "29d72211a9f7480c9812d61ee17c92b9", new DateTime(2025, 6, 21, 10, 30, 17, 748, DateTimeKind.Utc).AddTicks(2500), null, null, null, "Branch Manager", "u+7jaa75MJns9ST/6FSw4aQU+zSF/iNldvzpJxBFzwk=", false, true, null, null, "bf081015e17a41b8b1cae65b1b17cfdb", "R0kox04KXBTPFFlKjfxIzNeMzlbH1rGO/YJDtl0N894=", new DateTime(2025, 6, 21, 10, 30, 17, 748, DateTimeKind.Utc).AddTicks(2500), null, "branchmanager@mamafit.com", "branchmanager" },
                    { "4c9804ecc1d645de96fcfc906cc43d6c", new DateTime(2025, 6, 21, 10, 30, 17, 748, DateTimeKind.Utc).AddTicks(2480), null, null, null, "Manager", "u+7jaa75MJns9ST/6FSw4aQU+zSF/iNldvzpJxBFzwk=", false, true, null, null, "a3cb88edaf2b4718a9986010c5b9c1d7", "R0kox04KXBTPFFlKjfxIzNeMzlbH1rGO/YJDtl0N894=", new DateTime(2025, 6, 21, 10, 30, 17, 748, DateTimeKind.Utc).AddTicks(2480), null, "manager@mamafit.com", "manager" },
                    { "ce5235c40924fd5b0792732d3fb1b6f", new DateTime(2025, 6, 21, 10, 30, 17, 748, DateTimeKind.Utc).AddTicks(2470), null, null, null, "Staff", "u+7jaa75MJns9ST/6FSw4aQU+zSF/iNldvzpJxBFzwk=", false, true, null, null, "5ed8cfa9b62d433c88ab097b6d2baccd", "R0kox04KXBTPFFlKjfxIzNeMzlbH1rGO/YJDtl0N894=", new DateTime(2025, 6, 21, 10, 30, 17, 748, DateTimeKind.Utc).AddTicks(2470), null, "staff@mamafit.com", "staff" },
                    { "eb019fbe31e6449b9b92c89b5c893b03", new DateTime(2025, 6, 21, 10, 30, 17, 748, DateTimeKind.Utc).AddTicks(2490), null, null, null, "Branch Staff", "u+7jaa75MJns9ST/6FSw4aQU+zSF/iNldvzpJxBFzwk=", false, true, null, null, "c9118b99c0ad486dbb18560a916b630c", "R0kox04KXBTPFFlKjfxIzNeMzlbH1rGO/YJDtl0N894=", new DateTime(2025, 6, 21, 10, 30, 17, 748, DateTimeKind.Utc).AddTicks(2490), null, "branchstaff@mamafit.com", "branchstaff" },
                    { "f49aa51bbd304e77933e24bbed65b165", new DateTime(2025, 6, 21, 10, 30, 17, 748, DateTimeKind.Utc).AddTicks(2510), null, null, null, "User", "u+7jaa75MJns9ST/6FSw4aQU+zSF/iNldvzpJxBFzwk=", false, true, null, null, "b8d237b8b6f849988d60c6c3c1d0a943", "R0kox04KXBTPFFlKjfxIzNeMzlbH1rGO/YJDtl0N894=", new DateTime(2025, 6, 21, 10, 30, 17, 748, DateTimeKind.Utc).AddTicks(2510), null, "user@mamafit.com", "user" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Address_UserId",
                table: "Address",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Appointment_BranchId",
                table: "Appointment",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_Appointment_StaffId",
                table: "Appointment",
                column: "StaffId");

            migrationBuilder.CreateIndex(
                name: "IX_Appointment_UserId",
                table: "Appointment",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Branch_BranchManagerId",
                table: "Branch",
                column: "BranchManagerId");

            migrationBuilder.CreateIndex(
                name: "IX_BranchMaternityDressDetail_MaternityDressDetailId",
                table: "BranchMaternityDressDetail",
                column: "MaternityDressDetailId");

            migrationBuilder.CreateIndex(
                name: "IX_CartItem_MaternityDressDetailId",
                table: "CartItem",
                column: "MaternityDressDetailId");

            migrationBuilder.CreateIndex(
                name: "IX_CartItem_UserId",
                table: "CartItem",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ChatMessage_ChatRoomId_CreatedAt",
                table: "ChatMessage",
                columns: new[] { "ChatRoomId", "CreatedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_ChatMessage_CreatedAt",
                table: "ChatMessage",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_ChatMessage_SenderId",
                table: "ChatMessage",
                column: "SenderId");

            migrationBuilder.CreateIndex(
                name: "IX_ChatRoom_ApplicationUserId",
                table: "ChatRoom",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ChatRoomMember_ChatRoomId",
                table: "ChatRoomMember",
                column: "ChatRoomId");

            migrationBuilder.CreateIndex(
                name: "IX_ChatRoomMember_UserId",
                table: "ChatRoomMember",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Component_StyleId",
                table: "Component",
                column: "StyleId");

            migrationBuilder.CreateIndex(
                name: "IX_ComponentOption_ComponentId",
                table: "ComponentOption",
                column: "ComponentId");

            migrationBuilder.CreateIndex(
                name: "IX_DesignRequest_OrderItemId",
                table: "DesignRequest",
                column: "OrderItemId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DesignRequest_UserId",
                table: "DesignRequest",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Feedback_OrderItemId",
                table: "Feedback",
                column: "OrderItemId");

            migrationBuilder.CreateIndex(
                name: "IX_Feedback_UserId",
                table: "Feedback",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_MaternityDress_StyleId",
                table: "MaternityDress",
                column: "StyleId");

            migrationBuilder.CreateIndex(
                name: "IX_MaternityDressCustomization_DesignRequestId",
                table: "MaternityDressCustomization",
                column: "DesignRequestId");

            migrationBuilder.CreateIndex(
                name: "IX_MaternityDressCustomization_OrderItemId",
                table: "MaternityDressCustomization",
                column: "OrderItemId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MaternityDressCustomization_UserId",
                table: "MaternityDressCustomization",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_MaternityDressDetail_MaternityDressId",
                table: "MaternityDressDetail",
                column: "MaternityDressId");

            migrationBuilder.CreateIndex(
                name: "IX_MaternityDressSelection_ComponentOptionId",
                table: "MaternityDressSelection",
                column: "ComponentOptionId");

            migrationBuilder.CreateIndex(
                name: "IX_MaternityDressSelection_MaternityDressCustomizationId",
                table: "MaternityDressSelection",
                column: "MaternityDressCustomizationId");

            migrationBuilder.CreateIndex(
                name: "IX_MaternityDressServiceOption_MaternityDressServiceId",
                table: "MaternityDressServiceOption",
                column: "MaternityDressServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_MaternityDressTask_MilestoneId",
                table: "MaternityDressTask",
                column: "MilestoneId");

            migrationBuilder.CreateIndex(
                name: "IX_Measurement_MeasurementDiaryId",
                table: "Measurement",
                column: "MeasurementDiaryId");

            migrationBuilder.CreateIndex(
                name: "IX_MeasurementDiary_UserId",
                table: "MeasurementDiary",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Notification_ReceiverId",
                table: "Notification",
                column: "ReceiverId");

            migrationBuilder.CreateIndex(
                name: "IX_Order_BranchId",
                table: "Order",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_Order_MeasurementDiaryId",
                table: "Order",
                column: "MeasurementDiaryId");

            migrationBuilder.CreateIndex(
                name: "IX_Order_ParentOrderId",
                table: "Order",
                column: "ParentOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_Order_UserId",
                table: "Order",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Order_VoucherDiscountId",
                table: "Order",
                column: "VoucherDiscountId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItem_MaternityDressDetailId",
                table: "OrderItem",
                column: "MaternityDressDetailId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItem_OrderId",
                table: "OrderItem",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItemServiceOption_OrderItemId",
                table: "OrderItemServiceOption",
                column: "OrderItemId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItemsTasks_MaternityDressTaskId",
                table: "OrderItemsTasks",
                column: "MaternityDressTaskId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItemsTasks_MilestoneId",
                table: "OrderItemsTasks",
                column: "MilestoneId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItemsTasks_OrderItemId",
                table: "OrderItemsTasks",
                column: "OrderItemId");

            migrationBuilder.CreateIndex(
                name: "IX_OTP_UserId",
                table: "OTP",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Style_CategoryId",
                table: "Style",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Transaction_OrderId",
                table: "Transaction",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_User_RoleId",
                table: "User",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_UserToken_UserId",
                table: "UserToken",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_VoucherDiscount_UserId",
                table: "VoucherDiscount",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_VoucherDiscount_VoucherBatchId",
                table: "VoucherDiscount",
                column: "VoucherBatchId");

            migrationBuilder.CreateIndex(
                name: "IX_WarrantyHistory_WarrantyRequestId",
                table: "WarrantyHistory",
                column: "WarrantyRequestId");

            migrationBuilder.CreateIndex(
                name: "IX_WarrantyRequest_OriginalOrderItemId",
                table: "WarrantyRequest",
                column: "OriginalOrderItemId");

            migrationBuilder.CreateIndex(
                name: "IX_WarrantyRequest_WarrantyOrderItemId",
                table: "WarrantyRequest",
                column: "WarrantyOrderItemId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Address");

            migrationBuilder.DropTable(
                name: "Appointment");

            migrationBuilder.DropTable(
                name: "BranchMaternityDressDetail");

            migrationBuilder.DropTable(
                name: "CartItem");

            migrationBuilder.DropTable(
                name: "ChatMessage");

            migrationBuilder.DropTable(
                name: "ChatRoomMember");

            migrationBuilder.DropTable(
                name: "Feedback");

            migrationBuilder.DropTable(
                name: "MaternityDressSelection");

            migrationBuilder.DropTable(
                name: "Measurement");

            migrationBuilder.DropTable(
                name: "Notification");

            migrationBuilder.DropTable(
                name: "OrderItemServiceOption");

            migrationBuilder.DropTable(
                name: "OrderItemsTasks");

            migrationBuilder.DropTable(
                name: "OTP");

            migrationBuilder.DropTable(
                name: "Transaction");

            migrationBuilder.DropTable(
                name: "UserToken");

            migrationBuilder.DropTable(
                name: "WarrantyHistory");

            migrationBuilder.DropTable(
                name: "ChatRoom");

            migrationBuilder.DropTable(
                name: "ComponentOption");

            migrationBuilder.DropTable(
                name: "MaternityDressCustomization");

            migrationBuilder.DropTable(
                name: "MaternityDressServiceOption");

            migrationBuilder.DropTable(
                name: "MaternityDressTask");

            migrationBuilder.DropTable(
                name: "WarrantyRequest");

            migrationBuilder.DropTable(
                name: "Component");

            migrationBuilder.DropTable(
                name: "DesignRequest");

            migrationBuilder.DropTable(
                name: "MaternityDressService");

            migrationBuilder.DropTable(
                name: "Milestone");

            migrationBuilder.DropTable(
                name: "OrderItem");

            migrationBuilder.DropTable(
                name: "MaternityDressDetail");

            migrationBuilder.DropTable(
                name: "Order");

            migrationBuilder.DropTable(
                name: "MaternityDress");

            migrationBuilder.DropTable(
                name: "Branch");

            migrationBuilder.DropTable(
                name: "MeasurementDiary");

            migrationBuilder.DropTable(
                name: "VoucherDiscount");

            migrationBuilder.DropTable(
                name: "Style");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropTable(
                name: "VoucherBatch");

            migrationBuilder.DropTable(
                name: "Category");

            migrationBuilder.DropTable(
                name: "Role");
        }
    }
}
