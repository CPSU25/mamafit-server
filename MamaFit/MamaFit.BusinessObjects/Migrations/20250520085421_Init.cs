using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MamaFit.BusinessObjects.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true),
                    Images = table.Column<List<string>>(type: "text[]", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    UpdateBy = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsDelete = table.Column<bool>(type: "boolean", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MaternityDressInspections",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true),
                    Type = table.Column<string>(type: "text", nullable: true),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    UpdateBy = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsDelete = table.Column<bool>(type: "boolean", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MaternityDressInspections", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProductionStage",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true),
                    IsRequire = table.Column<bool>(type: "boolean", nullable: true),
                    SequenceOrder = table.Column<int>(type: "integer", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    UpdateBy = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsDelete = table.Column<bool>(type: "boolean", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductionStage", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    RoleName = table.Column<string>(type: "text", nullable: true),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    UpdateBy = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsDelete = table.Column<bool>(type: "boolean", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserTokens",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    Token = table.Column<string>(type: "text", nullable: true),
                    ExpiredAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsRevoked = table.Column<bool>(type: "boolean", nullable: true),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    UpdateBy = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsDelete = table.Column<bool>(type: "boolean", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserTokens", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Styles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    CategoryId = table.Column<string>(type: "text", nullable: true),
                    Name = table.Column<string>(type: "text", nullable: true),
                    IsCustom = table.Column<bool>(type: "boolean", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true),
                    Images = table.Column<List<string>>(type: "text[]", nullable: true),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    UpdateBy = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsDelete = table.Column<bool>(type: "boolean", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Styles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Styles_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    UserName = table.Column<string>(type: "text", nullable: false),
                    UserEmail = table.Column<string>(type: "text", nullable: false),
                    Password = table.Column<string>(type: "text", nullable: false),
                    FullName = table.Column<string>(type: "text", nullable: true),
                    ProfilePicture = table.Column<string>(type: "text", nullable: true),
                    DateOfBirth = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    PhoneNumber = table.Column<string>(type: "text", nullable: true),
                    IsVerify = table.Column<bool>(type: "boolean", nullable: false),
                    RoleId = table.Column<string>(type: "text", nullable: true),
                    TokenId = table.Column<string>(type: "text", nullable: true),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    UpdateBy = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsDelete = table.Column<bool>(type: "boolean", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Users_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Users_UserTokens_TokenId",
                        column: x => x.TokenId,
                        principalTable: "UserTokens",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Components",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    StyleId = table.Column<string>(type: "text", nullable: true),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true),
                    Images = table.Column<List<string>>(type: "text[]", nullable: true),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    UpdateBy = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsDelete = table.Column<bool>(type: "boolean", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Components", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Components_Styles_StyleId",
                        column: x => x.StyleId,
                        principalTable: "Styles",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "MaternityDresses",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true),
                    Images = table.Column<List<string>>(type: "text[]", nullable: false),
                    Slug = table.Column<string>(type: "text", nullable: true),
                    AverageRating = table.Column<float>(type: "real", nullable: false),
                    TotalRatings = table.Column<int>(type: "integer", nullable: false),
                    Rating = table.Column<int>(type: "integer", nullable: true),
                    StyleId = table.Column<string>(type: "text", nullable: true),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    UpdateBy = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsDelete = table.Column<bool>(type: "boolean", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MaternityDresses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MaternityDresses_Styles_StyleId",
                        column: x => x.StyleId,
                        principalTable: "Styles",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "DesignOrders",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    UserId = table.Column<string>(type: "text", nullable: true),
                    DesignerId = table.Column<string>(type: "text", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true),
                    Images = table.Column<List<string>>(type: "text[]", nullable: true),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    UpdateBy = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsDelete = table.Column<bool>(type: "boolean", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DesignOrders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DesignOrders_Users_DesignerId",
                        column: x => x.DesignerId,
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_DesignOrders_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DressCustomizations",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    UserId = table.Column<string>(type: "text", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true),
                    Status = table.Column<string>(type: "text", nullable: true),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    UpdateBy = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsDelete = table.Column<bool>(type: "boolean", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DressCustomizations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DressCustomizations_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Locations",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    UserId = table.Column<string>(type: "text", nullable: true),
                    MapId = table.Column<string>(type: "text", nullable: true),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Address = table.Column<string>(type: "text", nullable: true),
                    Latitude = table.Column<float>(type: "real", nullable: true),
                    Longitude = table.Column<float>(type: "real", nullable: true),
                    Type = table.Column<int>(type: "integer", nullable: true),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    UpdateBy = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsDelete = table.Column<bool>(type: "boolean", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Locations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Locations_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MeasurementDiaries",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    UserId = table.Column<string>(type: "text", nullable: true),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true),
                    NumberOfPregnancy = table.Column<int>(type: "integer", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    UpdateBy = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsDelete = table.Column<bool>(type: "boolean", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MeasurementDiaries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MeasurementDiaries_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Notifications",
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
                    UpdateBy = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsDelete = table.Column<bool>(type: "boolean", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notifications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Notifications_Users_ReceiverId",
                        column: x => x.ReceiverId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
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
                    UpdateBy = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsDelete = table.Column<bool>(type: "boolean", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OTP", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OTP_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "VoucherDiscount",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    UserId = table.Column<string>(type: "text", nullable: true),
                    Code = table.Column<string>(type: "text", nullable: true),
                    Status = table.Column<int>(type: "integer", nullable: true),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    UpdateBy = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsDelete = table.Column<bool>(type: "boolean", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VoucherDiscount", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VoucherDiscount_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MaternityDressDetails",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    MaternityDressId = table.Column<string>(type: "text", nullable: true),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true),
                    Color = table.Column<string>(type: "text", nullable: true),
                    Size = table.Column<string>(type: "text", nullable: true),
                    Price = table.Column<float>(type: "real", nullable: false),
                    Quantity = table.Column<int>(type: "integer", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    UpdateBy = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsDelete = table.Column<bool>(type: "boolean", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MaternityDressDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MaternityDressDetails_MaternityDresses_MaternityDressId",
                        column: x => x.MaternityDressId,
                        principalTable: "MaternityDresses",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ComponentOptions",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    ComponentId = table.Column<string>(type: "text", nullable: true),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true),
                    Images = table.Column<List<string>>(type: "text[]", nullable: true),
                    DressCustomizationId = table.Column<string>(type: "text", nullable: true),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    UpdateBy = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsDelete = table.Column<bool>(type: "boolean", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ComponentOptions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ComponentOptions_Components_ComponentId",
                        column: x => x.ComponentId,
                        principalTable: "Components",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ComponentOptions_DressCustomizations_DressCustomizationId",
                        column: x => x.DressCustomizationId,
                        principalTable: "DressCustomizations",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Branches",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    BranchManagerId = table.Column<string>(type: "text", nullable: true),
                    LocationId = table.Column<string>(type: "text", nullable: true),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true),
                    OpeningHour = table.Column<string>(type: "text", nullable: true),
                    Images = table.Column<List<string>>(type: "text[]", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    UpdateBy = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsDelete = table.Column<bool>(type: "boolean", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Branches", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Branches_Locations_LocationId",
                        column: x => x.LocationId,
                        principalTable: "Locations",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Branches_Users_BranchManagerId",
                        column: x => x.BranchManagerId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Measurements",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    Height = table.Column<float>(type: "real", nullable: false),
                    Weight = table.Column<float>(type: "real", nullable: false),
                    Neck = table.Column<float>(type: "real", nullable: false),
                    Coat = table.Column<float>(type: "real", nullable: false),
                    ChestAround = table.Column<float>(type: "real", nullable: false),
                    Stomach = table.Column<float>(type: "real", nullable: false),
                    ShoulderWidth = table.Column<float>(type: "real", nullable: false),
                    Hip = table.Column<float>(type: "real", nullable: false),
                    MeasurementDiaryId = table.Column<string>(type: "text", nullable: true),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    UpdateBy = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsDelete = table.Column<bool>(type: "boolean", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Measurements", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Measurements_MeasurementDiaries_MeasurementDiaryId",
                        column: x => x.MeasurementDiaryId,
                        principalTable: "MeasurementDiaries",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "CartItems",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    UserId = table.Column<string>(type: "text", nullable: true),
                    Quantity = table.Column<int>(type: "integer", nullable: false),
                    TotalAmount = table.Column<float>(type: "real", nullable: false),
                    MaternityDressDetailId = table.Column<string>(type: "text", nullable: true),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    UpdateBy = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsDelete = table.Column<bool>(type: "boolean", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CartItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CartItems_MaternityDressDetails_MaternityDressDetailId",
                        column: x => x.MaternityDressDetailId,
                        principalTable: "MaternityDressDetails",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CartItems_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Appointments",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    UserId = table.Column<string>(type: "text", nullable: true),
                    BranchId = table.Column<string>(type: "text", nullable: true),
                    FullName = table.Column<string>(type: "text", nullable: true),
                    PhoneNumber = table.Column<string>(type: "text", nullable: true),
                    BookingTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Note = table.Column<string>(type: "text", nullable: true),
                    Status = table.Column<int>(type: "integer", nullable: true),
                    CanceledAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CanceledReason = table.Column<string>(type: "text", nullable: true),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    UpdateBy = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsDelete = table.Column<bool>(type: "boolean", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Appointments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Appointments_Branches_BranchId",
                        column: x => x.BranchId,
                        principalTable: "Branches",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Appointments_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BranchMaternityDressDetails",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    MaternityDressDetailId = table.Column<string>(type: "text", nullable: true),
                    BranchId = table.Column<string>(type: "text", nullable: true),
                    Quantity = table.Column<int>(type: "integer", nullable: true),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    UpdateBy = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsDelete = table.Column<bool>(type: "boolean", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BranchMaternityDressDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BranchMaternityDressDetails_Branches_BranchId",
                        column: x => x.BranchId,
                        principalTable: "Branches",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_BranchMaternityDressDetails_MaternityDressDetails_Maternity~",
                        column: x => x.MaternityDressDetailId,
                        principalTable: "MaternityDressDetails",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Feedbacks",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    UserId = table.Column<string>(type: "text", nullable: true),
                    OrderItemId = table.Column<string>(type: "text", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true),
                    Images = table.Column<List<string>>(type: "text[]", nullable: true),
                    Rated = table.Column<float>(type: "real", nullable: true),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    UpdateBy = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsDelete = table.Column<bool>(type: "boolean", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Feedbacks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Feedbacks_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OrderItemInspections",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    OrderItemId = table.Column<string>(type: "text", nullable: false),
                    MaternityDressInspectionId = table.Column<string>(type: "text", nullable: false),
                    IsChecked = table.Column<bool>(type: "boolean", nullable: false),
                    Image = table.Column<string>(type: "text", nullable: true),
                    Note = table.Column<string>(type: "text", nullable: true),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    UpdateBy = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsDelete = table.Column<bool>(type: "boolean", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderItemInspections", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderItemInspections_MaternityDressInspections_MaternityDre~",
                        column: x => x.MaternityDressInspectionId,
                        principalTable: "MaternityDressInspections",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OrderItemProductionStages",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    ProductionStageId = table.Column<string>(type: "text", nullable: true),
                    OrderItemId = table.Column<string>(type: "text", nullable: true),
                    IsCompelete = table.Column<bool>(type: "boolean", nullable: true),
                    Images = table.Column<List<string>>(type: "text[]", nullable: true),
                    Note = table.Column<string>(type: "text", nullable: true),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    UpdateBy = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsDelete = table.Column<bool>(type: "boolean", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderItemProductionStages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderItemProductionStages_ProductionStage_ProductionStageId",
                        column: x => x.ProductionStageId,
                        principalTable: "ProductionStage",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "OrderItems",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    OrderItemParentId = table.Column<string>(type: "text", nullable: true),
                    ParentOrderItemId = table.Column<string>(type: "text", nullable: true),
                    MaternityDressDetailId = table.Column<string>(type: "text", nullable: true),
                    OrderId = table.Column<string>(type: "text", nullable: true),
                    ItemType = table.Column<int>(type: "integer", nullable: true),
                    Price = table.Column<float>(type: "real", nullable: false),
                    Quantity = table.Column<int>(type: "integer", nullable: false),
                    WarrantyDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    WarrantyNumber = table.Column<int>(type: "integer", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    UpdateBy = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsDelete = table.Column<bool>(type: "boolean", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderItems_MaternityDressDetails_MaternityDressDetailId",
                        column: x => x.MaternityDressDetailId,
                        principalTable: "MaternityDressDetails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrderItems_OrderItems_ParentOrderItemId",
                        column: x => x.ParentOrderItemId,
                        principalTable: "OrderItems",
                        principalColumn: "Id");
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
                    UpdateBy = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsDelete = table.Column<bool>(type: "boolean", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WarrantyRequest", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WarrantyRequest_OrderItems_OriginalOrderItemId",
                        column: x => x.OriginalOrderItemId,
                        principalTable: "OrderItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WarrantyRequest_OrderItems_WarrantyOrderItemId",
                        column: x => x.WarrantyOrderItemId,
                        principalTable: "OrderItems",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "WarrantyHistories",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    WarrantyRequestId = table.Column<string>(type: "text", nullable: true),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    UpdateBy = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsDelete = table.Column<bool>(type: "boolean", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WarrantyHistories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WarrantyHistories_WarrantyRequest_WarrantyRequestId",
                        column: x => x.WarrantyRequestId,
                        principalTable: "WarrantyRequest",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    PanrentOrderId = table.Column<string>(type: "text", nullable: true),
                    ParentOrderId = table.Column<string>(type: "text", nullable: true),
                    UserId = table.Column<string>(type: "text", nullable: true),
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
                    VoucherDiscountId = table.Column<string>(type: "text", nullable: true),
                    WarrantyCode = table.Column<string>(type: "text", nullable: true),
                    WarrantyHistoryId = table.Column<string>(type: "text", nullable: true),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    UpdateBy = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsDelete = table.Column<bool>(type: "boolean", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Orders_Orders_ParentOrderId",
                        column: x => x.ParentOrderId,
                        principalTable: "Orders",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Orders_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Orders_VoucherDiscount_VoucherDiscountId",
                        column: x => x.VoucherDiscountId,
                        principalTable: "VoucherDiscount",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Orders_WarrantyHistories_WarrantyHistoryId",
                        column: x => x.WarrantyHistoryId,
                        principalTable: "WarrantyHistories",
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
                    TransferAmount = table.Column<float>(type: "real", nullable: true),
                    Accumulated = table.Column<string>(type: "text", nullable: true),
                    SubAccount = table.Column<string>(type: "text", nullable: true),
                    ReferenceCode = table.Column<string>(type: "text", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    UpdateBy = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsDelete = table.Column<bool>(type: "boolean", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transaction", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Transaction_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_BranchId",
                table: "Appointments",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_UserId",
                table: "Appointments",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Branches_BranchManagerId",
                table: "Branches",
                column: "BranchManagerId");

            migrationBuilder.CreateIndex(
                name: "IX_Branches_LocationId",
                table: "Branches",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_BranchMaternityDressDetails_BranchId",
                table: "BranchMaternityDressDetails",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_BranchMaternityDressDetails_MaternityDressDetailId",
                table: "BranchMaternityDressDetails",
                column: "MaternityDressDetailId");

            migrationBuilder.CreateIndex(
                name: "IX_CartItems_MaternityDressDetailId",
                table: "CartItems",
                column: "MaternityDressDetailId");

            migrationBuilder.CreateIndex(
                name: "IX_CartItems_UserId",
                table: "CartItems",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ComponentOptions_ComponentId",
                table: "ComponentOptions",
                column: "ComponentId");

            migrationBuilder.CreateIndex(
                name: "IX_ComponentOptions_DressCustomizationId",
                table: "ComponentOptions",
                column: "DressCustomizationId");

            migrationBuilder.CreateIndex(
                name: "IX_Components_StyleId",
                table: "Components",
                column: "StyleId");

            migrationBuilder.CreateIndex(
                name: "IX_DesignOrders_DesignerId",
                table: "DesignOrders",
                column: "DesignerId");

            migrationBuilder.CreateIndex(
                name: "IX_DesignOrders_UserId",
                table: "DesignOrders",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_DressCustomizations_UserId",
                table: "DressCustomizations",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Feedbacks_OrderItemId",
                table: "Feedbacks",
                column: "OrderItemId");

            migrationBuilder.CreateIndex(
                name: "IX_Feedbacks_UserId",
                table: "Feedbacks",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Locations_UserId",
                table: "Locations",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_MaternityDressDetails_MaternityDressId",
                table: "MaternityDressDetails",
                column: "MaternityDressId");

            migrationBuilder.CreateIndex(
                name: "IX_MaternityDresses_StyleId",
                table: "MaternityDresses",
                column: "StyleId");

            migrationBuilder.CreateIndex(
                name: "IX_MeasurementDiaries_UserId",
                table: "MeasurementDiaries",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Measurements_MeasurementDiaryId",
                table: "Measurements",
                column: "MeasurementDiaryId");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_ReceiverId",
                table: "Notifications",
                column: "ReceiverId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItemInspections_MaternityDressInspectionId",
                table: "OrderItemInspections",
                column: "MaternityDressInspectionId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItemInspections_OrderItemId",
                table: "OrderItemInspections",
                column: "OrderItemId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItemProductionStages_OrderItemId",
                table: "OrderItemProductionStages",
                column: "OrderItemId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItemProductionStages_ProductionStageId",
                table: "OrderItemProductionStages",
                column: "ProductionStageId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_MaternityDressDetailId",
                table: "OrderItems",
                column: "MaternityDressDetailId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_OrderId",
                table: "OrderItems",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_ParentOrderItemId",
                table: "OrderItems",
                column: "ParentOrderItemId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_ParentOrderId",
                table: "Orders",
                column: "ParentOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_UserId",
                table: "Orders",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_VoucherDiscountId",
                table: "Orders",
                column: "VoucherDiscountId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_WarrantyHistoryId",
                table: "Orders",
                column: "WarrantyHistoryId");

            migrationBuilder.CreateIndex(
                name: "IX_OTP_UserId",
                table: "OTP",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Styles_CategoryId",
                table: "Styles",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Transaction_OrderId",
                table: "Transaction",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_RoleId",
                table: "Users",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_TokenId",
                table: "Users",
                column: "TokenId");

            migrationBuilder.CreateIndex(
                name: "IX_VoucherDiscount_UserId",
                table: "VoucherDiscount",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_WarrantyHistories_WarrantyRequestId",
                table: "WarrantyHistories",
                column: "WarrantyRequestId");

            migrationBuilder.CreateIndex(
                name: "IX_WarrantyRequest_OriginalOrderItemId",
                table: "WarrantyRequest",
                column: "OriginalOrderItemId");

            migrationBuilder.CreateIndex(
                name: "IX_WarrantyRequest_WarrantyOrderItemId",
                table: "WarrantyRequest",
                column: "WarrantyOrderItemId");

            migrationBuilder.AddForeignKey(
                name: "FK_Feedbacks_OrderItems_OrderItemId",
                table: "Feedbacks",
                column: "OrderItemId",
                principalTable: "OrderItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderItemInspections_OrderItems_OrderItemId",
                table: "OrderItemInspections",
                column: "OrderItemId",
                principalTable: "OrderItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderItemProductionStages_OrderItems_OrderItemId",
                table: "OrderItemProductionStages",
                column: "OrderItemId",
                principalTable: "OrderItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderItems_Orders_OrderId",
                table: "OrderItems",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Users_UserId",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_VoucherDiscount_Users_UserId",
                table: "VoucherDiscount");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderItems_MaternityDressDetails_MaternityDressDetailId",
                table: "OrderItems");

            migrationBuilder.DropForeignKey(
                name: "FK_WarrantyRequest_OrderItems_OriginalOrderItemId",
                table: "WarrantyRequest");

            migrationBuilder.DropForeignKey(
                name: "FK_WarrantyRequest_OrderItems_WarrantyOrderItemId",
                table: "WarrantyRequest");

            migrationBuilder.DropTable(
                name: "Appointments");

            migrationBuilder.DropTable(
                name: "BranchMaternityDressDetails");

            migrationBuilder.DropTable(
                name: "CartItems");

            migrationBuilder.DropTable(
                name: "ComponentOptions");

            migrationBuilder.DropTable(
                name: "DesignOrders");

            migrationBuilder.DropTable(
                name: "Feedbacks");

            migrationBuilder.DropTable(
                name: "Measurements");

            migrationBuilder.DropTable(
                name: "Notifications");

            migrationBuilder.DropTable(
                name: "OrderItemInspections");

            migrationBuilder.DropTable(
                name: "OrderItemProductionStages");

            migrationBuilder.DropTable(
                name: "OTP");

            migrationBuilder.DropTable(
                name: "Transaction");

            migrationBuilder.DropTable(
                name: "Branches");

            migrationBuilder.DropTable(
                name: "Components");

            migrationBuilder.DropTable(
                name: "DressCustomizations");

            migrationBuilder.DropTable(
                name: "MeasurementDiaries");

            migrationBuilder.DropTable(
                name: "MaternityDressInspections");

            migrationBuilder.DropTable(
                name: "ProductionStage");

            migrationBuilder.DropTable(
                name: "Locations");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "UserTokens");

            migrationBuilder.DropTable(
                name: "MaternityDressDetails");

            migrationBuilder.DropTable(
                name: "MaternityDresses");

            migrationBuilder.DropTable(
                name: "Styles");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "OrderItems");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "VoucherDiscount");

            migrationBuilder.DropTable(
                name: "WarrantyHistories");

            migrationBuilder.DropTable(
                name: "WarrantyRequest");
        }
    }
}
