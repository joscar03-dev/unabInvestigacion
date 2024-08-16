using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AKDEMIC.INFRASTRUCTURE.Migrations
{
    public partial class MYSQL_16042024 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Priority = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    NormalizedName = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ConcurrencyStamp = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Name = table.Column<string>(type: "varchar(250)", maxLength: 250, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    MaternalSurname = table.Column<string>(type: "varchar(250)", maxLength: 250, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PaternalSurname = table.Column<string>(type: "varchar(250)", maxLength: 250, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    FullName = table.Column<string>(type: "varchar(750)", maxLength: 750, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Picture = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Sex = table.Column<int>(type: "int", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    BirthDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    FirstLoginDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    Dni = table.Column<string>(type: "varchar(8)", maxLength: 8, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Address = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CurriculumVitaeUrl = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CteVitaeConcytecUrl = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    AuthenticationUserId = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    UserName = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    NormalizedUserName = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Email = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    NormalizedEmail = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    EmailConfirmed = table.Column<short>(type: "smallint", nullable: false),
                    PasswordHash = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    SecurityStamp = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ConcurrencyStamp = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PhoneNumber = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PhoneNumberConfirmed = table.Column<short>(type: "smallint", nullable: false),
                    TwoFactorEnabled = table.Column<short>(type: "smallint", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetime(6)", nullable: true),
                    LockoutEnabled = table.Column<short>(type: "smallint", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false),
                    RelationId = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DeletedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    DeletedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    CreatedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    UpdatedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "General_Audits",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    TableName = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DateTime = table.Column<DateTime>(type: "datetime", nullable: false),
                    KeyValues = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    OldValues = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    NewValues = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    UserName = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    AbsoluteUri = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Audits", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "General_Configurations",
                columns: table => new
                {
                    Key = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Value = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Configurations", x => x.Key);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TeacherHiring_Convocations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Name = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    MinScore = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Requirements = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    StartDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    ExternalEvaluationWeight = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PersonalInterviewWeight = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    MasterClassWeight = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    EnabledMasterClass = table.Column<short>(type: "smallint", nullable: false),
                    Type = table.Column<byte>(type: "tinyint unsigned", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    CreatedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    UpdatedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Convocations", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TeacherInvestigation_AuthorshipOrders",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Name = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Code = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    CreatedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    UpdatedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuthorshipOrders", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TeacherInvestigation_ExternalEntities",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Code = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Name = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    CreatedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    UpdatedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExternalEntities", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TeacherInvestigation_FinancingInvestigations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Code = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Name = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    CreatedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    UpdatedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FinancingInvestigations", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TeacherInvestigation_IdentificationTypes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Name = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Code = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    CreatedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    UpdatedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IdentificationTypes", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TeacherInvestigation_IncubatorConvocations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Code = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Name = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    StartDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    InscriptionStartDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    InscriptionEndDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    PicturePath = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    AddressedTo = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Requirements = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DocumentPath = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    TotalWinners = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    CreatedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    UpdatedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IncubatorConvocations", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TeacherInvestigation_IndexPlaces",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Name = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Code = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    CreatedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    UpdatedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IndexPlaces", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TeacherInvestigation_InvestigationAreas",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Code = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Name = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    CreatedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    UpdatedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InvestigationAreas", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TeacherInvestigation_InvestigationConvocations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Name = table.Column<string>(type: "varchar(900)", maxLength: 900, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Code = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Description = table.Column<string>(type: "varchar(3000)", maxLength: 3000, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PicturePath = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    StartDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    InscriptionStartDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    InscriptionEndDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    MinScore = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    State = table.Column<byte>(type: "tinyint unsigned", nullable: false),
                    AllowInquiries = table.Column<short>(type: "smallint", nullable: false),
                    InquiryStartDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    InquiryEndDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    DeletedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    CreatedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    UpdatedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InvestigationConvocations", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TeacherInvestigation_InvestigationPatterns",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Code = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Name = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    CreatedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    UpdatedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InvestigationPatterns", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TeacherInvestigation_InvestigationProjectTypes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Name = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    CreatedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    UpdatedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InvestigationProjectTypes", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TeacherInvestigation_InvestigationTypes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Code = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Name = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    CreatedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    UpdatedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InvestigationTypes", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TeacherInvestigation_MethodologyTypes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Code = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Name = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    CreatedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    UpdatedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MethodologyTypes", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TeacherInvestigation_OpusTypes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Name = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Code = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    CreatedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    UpdatedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OpusTypes", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TeacherInvestigation_PublicationFunctions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Name = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Code = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    CreatedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    UpdatedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PublicationFunctions", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TeacherInvestigation_ResearchCenters",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Name = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    CreatedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    UpdatedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResearchCenters", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TeacherInvestigation_ResearchLineCategories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Code = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Name = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    CreatedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    UpdatedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResearchLineCategories", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TeacherInvestigation_TeamMemberRoles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Name = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    CreatedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    UpdatedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TeamMemberRoles", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    RoleId = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ClaimType = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ClaimValue = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ClaimType = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ClaimValue = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ProviderKey = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ProviderDisplayName = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    UserId = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    RoleId = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    LoginProvider = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Name = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Value = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "General_UserRequests",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Name = table.Column<string>(type: "varchar(250)", maxLength: 250, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    MaternalSurname = table.Column<string>(type: "varchar(250)", maxLength: 250, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PaternalSurname = table.Column<string>(type: "varchar(250)", maxLength: 250, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Dni = table.Column<string>(type: "varchar(8)", maxLength: 8, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Email = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Address = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PhoneNumber = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    State = table.Column<int>(type: "int", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "varchar(255)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    CreatedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    UpdatedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRequests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserRequests_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TeacherInvestigation_PublishedBooks",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    UserId = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    MainAuthor = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Title = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PublishingCity = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PublishingHouse = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PublishingYear = table.Column<int>(type: "int", nullable: false),
                    PagesCount = table.Column<int>(type: "int", nullable: false),
                    ISBN = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    LegalDeposit = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Url = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    CreatedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    UpdatedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PublishedBooks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PublishedBooks_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TeacherInvestigation_PublishedChapterBooks",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    UserId = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    MainAuthor = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    BookTitle = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ChapterTitle = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PublishingCity = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PublishingHouse = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PublishingYear = table.Column<int>(type: "int", nullable: false),
                    StartPage = table.Column<int>(type: "int", nullable: false),
                    EndPage = table.Column<int>(type: "int", nullable: false),
                    DOI = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ISBN = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Url = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    CreatedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    UpdatedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PublishedChapterBooks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PublishedChapterBooks_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TeacherInvestigation_Units",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Name = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    UserId = table.Column<string>(type: "varchar(255)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    CreatedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    UpdatedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Units", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Units_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TeacherHiring_ConvocationCalendars",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    ConvocationId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    StartDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    Description = table.Column<string>(type: "varchar(400)", maxLength: 400, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConvocationCalendars", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ConvocationCalendars_Convocations_ConvocationId",
                        column: x => x.ConvocationId,
                        principalTable: "TeacherHiring_Convocations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TeacherHiring_ConvocationComitees",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ConvocationId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    ApplicationRoleId = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    AcademicDeparmentId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    AcademicDepartmentText = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    CreatedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    UpdatedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TeacherHiring_ConvocationComitees", x => new { x.UserId, x.ConvocationId });
                    table.ForeignKey(
                        name: "FK_ConvocationComitees_AspNetRoles_ApplicationRoleId",
                        column: x => x.ApplicationRoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ConvocationComitees_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ConvocationComitees_Convocations_ConvocationId",
                        column: x => x.ConvocationId,
                        principalTable: "TeacherHiring_Convocations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TeacherHiring_ConvocationDocuments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    ConvocationId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Type = table.Column<byte>(type: "tinyint unsigned", nullable: false, defaultValue: (byte)1),
                    Name = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Url = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    CreatedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    UpdatedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConvocationDocuments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ConvocationDocuments_Convocations_ConvocationId",
                        column: x => x.ConvocationId,
                        principalTable: "TeacherHiring_Convocations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TeacherHiring_ConvocationRubricSections",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Name = table.Column<string>(type: "varchar(1500)", maxLength: 1500, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Description = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    MaxScore = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Type = table.Column<byte>(type: "tinyint unsigned", nullable: false, defaultValue: (byte)1),
                    ConvocationId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    CreatedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    UpdatedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConvocationRubricSections", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ConvocationRubricSections_Convocations_ConvocationId",
                        column: x => x.ConvocationId,
                        principalTable: "TeacherHiring_Convocations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TeacherHiring_ConvocationSections",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Title = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Description = table.Column<string>(type: "varchar(1000)", maxLength: 1000, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ConvocationId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    CreatedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    UpdatedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConvocationSections", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ConvocationSections_Convocations_ConvocationId",
                        column: x => x.ConvocationId,
                        principalTable: "TeacherHiring_Convocations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TeacherHiring_ConvocationVacancies",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Code = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    AcademicDepartmentId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    AcademicDepartmentText = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ConvocationId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    ContractType = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Category = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Dedication = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Subjects = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Requirements = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Vacancies = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    CreatedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    UpdatedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConvocationVacancies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ConvocationVacancies_Convocations_ConvocationId",
                        column: x => x.ConvocationId,
                        principalTable: "TeacherHiring_Convocations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TeacherInvestigation_IncubatorConvocationAnnexes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Code = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Name = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IncubatorConvocationId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    CreatedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    UpdatedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IncubatorConvocationAnnexes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_IncubatorConvocationAnnexes_IncubatorConvocations_IncubatorC~",
                        column: x => x.IncubatorConvocationId,
                        principalTable: "TeacherInvestigation_IncubatorConvocations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TeacherInvestigation_IncubatorConvocationEvaluators",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IncubatorConvocationId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    CreatedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    UpdatedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TeacherInvestigation_IncubatorConvocationEvaluators", x => new { x.UserId, x.IncubatorConvocationId });
                    table.ForeignKey(
                        name: "FK_IncubatorConvocationEvaluators_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_IncubatorConvocationEvaluators_IncubatorConvocations_Incubat~",
                        column: x => x.IncubatorConvocationId,
                        principalTable: "TeacherInvestigation_IncubatorConvocations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TeacherInvestigation_IncubatorConvocationFaculties",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    IncubatorConvocationId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    FacultyId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    FacultyText = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    CreatedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    UpdatedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IncubatorConvocationFaculties", x => x.Id);
                    table.ForeignKey(
                        name: "FK_IncubatorConvocationFaculties_IncubatorConvocations_Incubato~",
                        column: x => x.IncubatorConvocationId,
                        principalTable: "TeacherInvestigation_IncubatorConvocations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TeacherInvestigation_IncubatorConvocationFiles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Number = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Name = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    FilePath = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IncubatorConvocationId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    CreatedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    UpdatedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IncubatorConvocationFiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_IncubatorConvocationFiles_IncubatorConvocations_IncubatorCon~",
                        column: x => x.IncubatorConvocationId,
                        principalTable: "TeacherInvestigation_IncubatorConvocations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TeacherInvestigation_IncubatorCoordinatorMonitors",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IncubatorConvocationId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    CreatedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    UpdatedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TeacherInvestigation_IncubatorCoordinatorMonitors", x => new { x.UserId, x.IncubatorConvocationId });
                    table.ForeignKey(
                        name: "FK_IncubatorCoordinatorMonitors_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_IncubatorCoordinatorMonitors_IncubatorConvocations_Incubator~",
                        column: x => x.IncubatorConvocationId,
                        principalTable: "TeacherInvestigation_IncubatorConvocations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TeacherInvestigation_IncubatorMonitors",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IncubatorConvocationId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    CreatedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    UpdatedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TeacherInvestigation_IncubatorMonitors", x => new { x.UserId, x.IncubatorConvocationId });
                    table.ForeignKey(
                        name: "FK_IncubatorMonitors_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_IncubatorMonitors_IncubatorConvocations_IncubatorConvocation~",
                        column: x => x.IncubatorConvocationId,
                        principalTable: "TeacherInvestigation_IncubatorConvocations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TeacherInvestigation_IncubatorPostulations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    UserId = table.Column<string>(type: "varchar(255)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IncubatorConvocationId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    RegisterDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    ReviewState = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Budget = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    MonthDuration = table.Column<int>(type: "int", nullable: false),
                    DepartmentId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    DepartmentText = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ProvinceId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    ProvinceText = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DistrictId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    DistrictText = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    GeneralGoals = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CVFilePath = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    AdviserId = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CoAdviserId = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    BusinessIdeaDescription = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CompetitiveAdvantages = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    MarketStudy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    MarketingPlan = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Resources = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PotentialStrategicPartners = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Mission = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ProductDescription = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    TechnicalViability = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    EconomicViability = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    MerchandisingPlan = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Breakeven = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    AffectationLevel = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    CreatedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    UpdatedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IncubatorPostulations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_IncubatorPostulations_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_IncubatorPostulations_IncubatorConvocations_IncubatorConvoca~",
                        column: x => x.IncubatorConvocationId,
                        principalTable: "TeacherInvestigation_IncubatorConvocations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TeacherInvestigation_IncubatorRubricSections",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Title = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    MaxSectionScore = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    IncubatorConvocationId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    CreatedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    UpdatedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IncubatorRubricSections", x => x.Id);
                    table.ForeignKey(
                        name: "FK_IncubatorRubricSections_IncubatorConvocations_IncubatorConvo~",
                        column: x => x.IncubatorConvocationId,
                        principalTable: "TeacherInvestigation_IncubatorConvocations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TeacherInvestigation_CoordinatorMonitorConvocations",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    InvestigationConvocationId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    CreatedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    UpdatedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TeacherInvestigation_CoordinatorMonitorConvocations", x => new { x.UserId, x.InvestigationConvocationId });
                    table.ForeignKey(
                        name: "FK_CoordinatorMonitorConvocations_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CoordinatorMonitorConvocations_InvestigationConvocations_Inv~",
                        column: x => x.InvestigationConvocationId,
                        principalTable: "TeacherInvestigation_InvestigationConvocations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TeacherInvestigation_EvaluatorCommitteeConvocations",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    InvestigationConvocationId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    CreatedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    UpdatedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TeacherInvestigation_EvaluatorCommitteeConvocations", x => new { x.UserId, x.InvestigationConvocationId });
                    table.ForeignKey(
                        name: "FK_EvaluatorCommitteeConvocations_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EvaluatorCommitteeConvocations_InvestigationConvocations_Inv~",
                        column: x => x.InvestigationConvocationId,
                        principalTable: "TeacherInvestigation_InvestigationConvocations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TeacherInvestigation_InvestigationConvocationEvaluators",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    InvestigationConvocationId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    CreatedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    UpdatedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TeacherInvestigation_InvestigationConvocationEvaluators", x => new { x.UserId, x.InvestigationConvocationId });
                    table.ForeignKey(
                        name: "FK_InvestigationConvocationEvaluators_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_InvestigationConvocationEvaluators_InvestigationConvocations~",
                        column: x => x.InvestigationConvocationId,
                        principalTable: "TeacherInvestigation_InvestigationConvocations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TeacherInvestigation_InvestigationConvocationFiles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Number = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Name = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    FilePath = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    InvestigationConvocationId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    CreatedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    UpdatedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InvestigationConvocationFiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InvestigationConvocationFiles_InvestigationConvocations_Inve~",
                        column: x => x.InvestigationConvocationId,
                        principalTable: "TeacherInvestigation_InvestigationConvocations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TeacherInvestigation_InvestigationConvocationHistories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    InvestigationConvocationId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    UserId = table.Column<string>(type: "varchar(255)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    OldEndDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    NewEndDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    ResolutionUrl = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    CreatedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    UpdatedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InvestigationConvocationHistories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InvestigationConvocationHistories_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_InvestigationConvocationHistories_InvestigationConvocations_~",
                        column: x => x.InvestigationConvocationId,
                        principalTable: "TeacherInvestigation_InvestigationConvocations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TeacherInvestigation_InvestigationConvocationRequirements",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    InvestigationTypeHidden = table.Column<short>(type: "smallint", nullable: false),
                    InvestigationTypeWeight = table.Column<int>(type: "int", nullable: false),
                    ExternalEntityHidden = table.Column<short>(type: "smallint", nullable: false),
                    ExternalEntityWeight = table.Column<int>(type: "int", nullable: false),
                    BudgetHidden = table.Column<short>(type: "smallint", nullable: false),
                    BudgetWeight = table.Column<int>(type: "int", nullable: false),
                    InvestigationPatternHidden = table.Column<short>(type: "smallint", nullable: false),
                    InvestigationPatternWeight = table.Column<int>(type: "int", nullable: false),
                    AreaHidden = table.Column<short>(type: "smallint", nullable: false),
                    AreaWeight = table.Column<int>(type: "int", nullable: false),
                    FacultyHidden = table.Column<short>(type: "smallint", nullable: false),
                    FacultyWeight = table.Column<int>(type: "int", nullable: false),
                    CareerHidden = table.Column<short>(type: "smallint", nullable: false),
                    CareerWeight = table.Column<int>(type: "int", nullable: false),
                    ResearchCenterHidden = table.Column<short>(type: "smallint", nullable: false),
                    ResearchCenterWeight = table.Column<int>(type: "int", nullable: false),
                    FinancingHidden = table.Column<short>(type: "smallint", nullable: false),
                    FinancingWeight = table.Column<int>(type: "int", nullable: false),
                    MainLocationHidden = table.Column<short>(type: "smallint", nullable: false),
                    MainLocationWeight = table.Column<int>(type: "int", nullable: false),
                    ExecutionPlaceHidden = table.Column<short>(type: "smallint", nullable: false),
                    ExecutionPlaceWeight = table.Column<int>(type: "int", nullable: false),
                    ProjectTitleHidden = table.Column<short>(type: "smallint", nullable: false),
                    ProjectTitleWeight = table.Column<int>(type: "int", nullable: false),
                    ProblemDescriptionHidden = table.Column<short>(type: "smallint", nullable: false),
                    ProblemDescriptionWeight = table.Column<int>(type: "int", nullable: false),
                    GeneralGoalHidden = table.Column<short>(type: "smallint", nullable: false),
                    GeneralGoalWeight = table.Column<int>(type: "int", nullable: false),
                    ProblemFormulationHidden = table.Column<short>(type: "smallint", nullable: false),
                    ProblemFormulationWeight = table.Column<int>(type: "int", nullable: false),
                    SpecificGoalHidden = table.Column<short>(type: "smallint", nullable: false),
                    SpecificGoalWeight = table.Column<int>(type: "int", nullable: false),
                    JustificationHidden = table.Column<short>(type: "smallint", nullable: false),
                    JustificationWeight = table.Column<int>(type: "int", nullable: false),
                    TheoreticalFundamentHidden = table.Column<short>(type: "smallint", nullable: false),
                    TheoreticalFundamentWeight = table.Column<int>(type: "int", nullable: false),
                    ProblemRecordHidden = table.Column<short>(type: "smallint", nullable: false),
                    ProblemRecordWeight = table.Column<int>(type: "int", nullable: false),
                    HypothesisHidden = table.Column<short>(type: "smallint", nullable: false),
                    HypothesisWeight = table.Column<int>(type: "int", nullable: false),
                    VariableHidden = table.Column<short>(type: "smallint", nullable: false),
                    VariableWeight = table.Column<int>(type: "int", nullable: false),
                    MethodologyTypeHidden = table.Column<short>(type: "smallint", nullable: false),
                    MethodologyTypeWeight = table.Column<int>(type: "int", nullable: false),
                    MethodologyDescriptionHidden = table.Column<short>(type: "smallint", nullable: false),
                    MethodologyDescriptionWeight = table.Column<int>(type: "int", nullable: false),
                    PopulationHidden = table.Column<short>(type: "smallint", nullable: false),
                    PopulationWeight = table.Column<int>(type: "int", nullable: false),
                    InformationCollectionTechniqueHidden = table.Column<short>(type: "smallint", nullable: false),
                    InformationCollectionTechniqueWeight = table.Column<int>(type: "int", nullable: false),
                    AnalysisTechniqueHidden = table.Column<short>(type: "smallint", nullable: false),
                    AnalysisTechniqueWeight = table.Column<int>(type: "int", nullable: false),
                    EthicalConsiderationsHidden = table.Column<short>(type: "smallint", nullable: false),
                    EthicalConsiderationsWeight = table.Column<int>(type: "int", nullable: false),
                    FieldWorkHidden = table.Column<short>(type: "smallint", nullable: false),
                    FieldWorkWeight = table.Column<int>(type: "int", nullable: false),
                    ExpectedResultsHidden = table.Column<short>(type: "smallint", nullable: false),
                    ExpectedResultsWeight = table.Column<int>(type: "int", nullable: false),
                    ThesisDevelopmentHidden = table.Column<short>(type: "smallint", nullable: false),
                    ThesisDevelopmentWeight = table.Column<int>(type: "int", nullable: false),
                    PublishedArticleHidden = table.Column<short>(type: "smallint", nullable: false),
                    PublishedArticleWeight = table.Column<int>(type: "int", nullable: false),
                    BroadcastArticleHidden = table.Column<short>(type: "smallint", nullable: false),
                    BroadcastArticleWeight = table.Column<int>(type: "int", nullable: false),
                    ProcessDevelopmentHidden = table.Column<short>(type: "smallint", nullable: false),
                    ProcessDevelopmentWeight = table.Column<int>(type: "int", nullable: false),
                    BibliographicReferencesHidden = table.Column<short>(type: "smallint", nullable: false),
                    BibliographicReferencesWeight = table.Column<int>(type: "int", nullable: false),
                    TeamMemberUserHidden = table.Column<short>(type: "smallint", nullable: false),
                    TeamMemberUserWeight = table.Column<int>(type: "int", nullable: false),
                    ExternalMemberHidden = table.Column<short>(type: "smallint", nullable: false),
                    ExternalMemberWeight = table.Column<int>(type: "int", nullable: false),
                    ProjectDurationHidden = table.Column<short>(type: "smallint", nullable: false),
                    ProjectDurationWeight = table.Column<int>(type: "int", nullable: false),
                    PostulationAttachmentFilesHidden = table.Column<short>(type: "smallint", nullable: false),
                    PostulationAttachmentFilesWeight = table.Column<int>(type: "int", nullable: false),
                    QuestionsHidden = table.Column<short>(type: "smallint", nullable: false),
                    QuestionsWeight = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    CreatedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    UpdatedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InvestigationConvocationRequirements", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InvestigationConvocationRequirements_InvestigationConvocatio~",
                        column: x => x.Id,
                        principalTable: "TeacherInvestigation_InvestigationConvocations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TeacherInvestigation_InvestigationConvocationSupervisors",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    InvestigationConvocationId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    CreatedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    UpdatedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TeacherInvestigation_InvestigationConvocationSupervisors", x => new { x.UserId, x.InvestigationConvocationId });
                    table.ForeignKey(
                        name: "FK_InvestigationConvocationSupervisors_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_InvestigationConvocationSupervisors_InvestigationConvocation~",
                        column: x => x.InvestigationConvocationId,
                        principalTable: "TeacherInvestigation_InvestigationConvocations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TeacherInvestigation_InvestigationRubricSections",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Title = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    MaxSectionScore = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    InvestigationConvocationId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    CreatedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    UpdatedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InvestigationRubricSections", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InvestigationRubricSections_InvestigationConvocations_Invest~",
                        column: x => x.InvestigationConvocationId,
                        principalTable: "TeacherInvestigation_InvestigationConvocations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TeacherInvestigation_MonitorConvocations",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    InvestigationConvocationId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    CreatedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    UpdatedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TeacherInvestigation_MonitorConvocations", x => new { x.UserId, x.InvestigationConvocationId });
                    table.ForeignKey(
                        name: "FK_MonitorConvocations_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MonitorConvocations_InvestigationConvocations_InvestigationC~",
                        column: x => x.InvestigationConvocationId,
                        principalTable: "TeacherInvestigation_InvestigationConvocations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TeacherInvestigation_Conferences",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    OpusTypeId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    UserId = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Type = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Name = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    OrganizerInstitution = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Country = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    City = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    StartDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    MainAuthor = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ISBN = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ISSN = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DOI = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    UrlEvent = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    CreatedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    UpdatedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Conferences", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Conferences_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Conferences_OpusTypes_OpusTypeId",
                        column: x => x.OpusTypeId,
                        principalTable: "TeacherInvestigation_OpusTypes",
                        principalColumn: "Id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TeacherInvestigation_Publications",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    WorkCategory = table.Column<int>(type: "int", nullable: false),
                    OpusTypeId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    PublicationFunctionId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    IndexPlaceId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    IdentificationTypeId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    AuthorshipOrderId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    UserId = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Title = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    SubTitle = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Journal = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Description = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Volume = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Fascicle = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    MainAuthor = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PublishingHouse = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DOI = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Url = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CountryId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    CountryText = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PublishDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    CreatedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    UpdatedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Publications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Publications_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Publications_AuthorshipOrders_AuthorshipOrderId",
                        column: x => x.AuthorshipOrderId,
                        principalTable: "TeacherInvestigation_AuthorshipOrders",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Publications_IdentificationTypes_IdentificationTypeId",
                        column: x => x.IdentificationTypeId,
                        principalTable: "TeacherInvestigation_IdentificationTypes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Publications_IndexPlaces_IndexPlaceId",
                        column: x => x.IndexPlaceId,
                        principalTable: "TeacherInvestigation_IndexPlaces",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Publications_OpusTypes_OpusTypeId",
                        column: x => x.OpusTypeId,
                        principalTable: "TeacherInvestigation_OpusTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Publications_PublicationFunctions_PublicationFunctionId",
                        column: x => x.PublicationFunctionId,
                        principalTable: "TeacherInvestigation_PublicationFunctions",
                        principalColumn: "Id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TeacherInvestigation_InvestigationConvocationPostulants",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    UserId = table.Column<string>(type: "varchar(255)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    InvestigationConvocationId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    ResolutionDocumentPath = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    MonitorDocumentPath = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    MonitorUserId = table.Column<string>(type: "varchar(255)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    InvestigationTypeId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    ExternalEntityId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    Budget = table.Column<int>(type: "int", nullable: true),
                    InvestigationPatternId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    InvestigationAreaId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    FacultyId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    FacultyText = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CareerId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    CareerText = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ResearchCenterId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    FinancingInvestigationId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    MainLocation = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ProjectTitle = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ProblemDescription = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    GeneralGoal = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ProblemFormulation = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    SpecificGoal = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Justification = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    TheoreticalFundament = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ProblemRecord = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Hypothesis = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Variable = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    MethodologyTypeId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    MethodologyDescription = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Population = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    InformationCollectionTechnique = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    AnalysisTechnique = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    EthicalConsiderations = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    FieldWork = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ExpectedResults = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ThesisDevelopment = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PublishedArticle = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    BroadcastArticle = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ProcessDevelopment = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    BibliographicReferences = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ProjectDuration = table.Column<int>(type: "int", nullable: true),
                    ProjectState = table.Column<int>(type: "int", nullable: false),
                    ProgressState = table.Column<int>(type: "int", nullable: false),
                    ReviewState = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    CreatedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    UpdatedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InvestigationConvocationPostulants", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InvestigationConvocationPostulants_AspNetUsers_MonitorUserId",
                        column: x => x.MonitorUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_InvestigationConvocationPostulants_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_InvestigationConvocationPostulants_ExternalEntities_External~",
                        column: x => x.ExternalEntityId,
                        principalTable: "TeacherInvestigation_ExternalEntities",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_InvestigationConvocationPostulants_FinancingInvestigations_F~",
                        column: x => x.FinancingInvestigationId,
                        principalTable: "TeacherInvestigation_FinancingInvestigations",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_InvestigationConvocationPostulants_InvestigationAreas_Invest~",
                        column: x => x.InvestigationAreaId,
                        principalTable: "TeacherInvestigation_InvestigationAreas",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_InvestigationConvocationPostulants_InvestigationConvocations~",
                        column: x => x.InvestigationConvocationId,
                        principalTable: "TeacherInvestigation_InvestigationConvocations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_InvestigationConvocationPostulants_InvestigationPatterns_Inv~",
                        column: x => x.InvestigationPatternId,
                        principalTable: "TeacherInvestigation_InvestigationPatterns",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_InvestigationConvocationPostulants_InvestigationTypes_Invest~",
                        column: x => x.InvestigationTypeId,
                        principalTable: "TeacherInvestigation_InvestigationTypes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_InvestigationConvocationPostulants_MethodologyTypes_Methodol~",
                        column: x => x.MethodologyTypeId,
                        principalTable: "TeacherInvestigation_MethodologyTypes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_InvestigationConvocationPostulants_ResearchCenters_ResearchC~",
                        column: x => x.ResearchCenterId,
                        principalTable: "TeacherInvestigation_ResearchCenters",
                        principalColumn: "Id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TeacherInvestigation_ResearchLines",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Code = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Name = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ResearchLineCategoryId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    CreatedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    UpdatedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResearchLines", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ResearchLines_ResearchLineCategories_ResearchLineCategoryId",
                        column: x => x.ResearchLineCategoryId,
                        principalTable: "TeacherInvestigation_ResearchLineCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TeacherInvestigation_PublishedBookAuthors",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    PaternalSurname = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    MaternalSurname = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Name = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Email = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Dni = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PublishedBookId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    CreatedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    UpdatedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PublishedBookAuthors", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PublishedBookAuthors_PublishedBooks_PublishedBookId",
                        column: x => x.PublishedBookId,
                        principalTable: "TeacherInvestigation_PublishedBooks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TeacherInvestigation_PublishedBookFiles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Name = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    FilePath = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PublishedBookId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    CreatedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    UpdatedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PublishedBookFiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PublishedBookFiles_PublishedBooks_PublishedBookId",
                        column: x => x.PublishedBookId,
                        principalTable: "TeacherInvestigation_PublishedBooks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TeacherInvestigation_PublishedChapterBookAuthors",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    PaternalSurname = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    MaternalSurname = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Name = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Email = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Dni = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PublishedChapterBookId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    CreatedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    UpdatedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PublishedChapterBookAuthors", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PublishedChapterBookAuthors_PublishedChapterBooks_PublishedC~",
                        column: x => x.PublishedChapterBookId,
                        principalTable: "TeacherInvestigation_PublishedChapterBooks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TeacherInvestigation_PublishedChapterBookFiles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Name = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    FilePath = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PublishedChapterBookId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    CreatedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    UpdatedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PublishedChapterBookFiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PublishedChapterBookFiles_PublishedChapterBooks_PublishedCha~",
                        column: x => x.PublishedChapterBookId,
                        principalTable: "TeacherInvestigation_PublishedChapterBooks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TeacherInvestigation_Events",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Title = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Description = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    EventDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    PicturePath = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    VideoUrl = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Cost = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Organizer = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    UnitId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    CreatedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    UpdatedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Events", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Events_Units_UnitId",
                        column: x => x.UnitId,
                        principalTable: "TeacherInvestigation_Units",
                        principalColumn: "Id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TeacherInvestigation_OperativePlans",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Name = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Year = table.Column<int>(type: "int", nullable: false),
                    RegisterDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    State = table.Column<int>(type: "int", nullable: false),
                    FilePath = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Observation = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    UnitId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    CreatedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    UpdatedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OperativePlans", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OperativePlans_Units_UnitId",
                        column: x => x.UnitId,
                        principalTable: "TeacherInvestigation_Units",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TeacherHiring_ConvocationRubricItems",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Name = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    MaxScore = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ConvocationRubricSectionId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    CreatedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    UpdatedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConvocationRubricItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ConvocationRubricItems_ConvocationRubricSections_Convocation~",
                        column: x => x.ConvocationRubricSectionId,
                        principalTable: "TeacherHiring_ConvocationRubricSections",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TeacherHiring_ConvocationQuestions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    StaticType = table.Column<byte>(type: "tinyint unsigned", nullable: false, defaultValue: (byte)1),
                    Type = table.Column<byte>(type: "tinyint unsigned", nullable: false, defaultValue: (byte)1),
                    Description = table.Column<string>(type: "varchar(1000)", maxLength: 1000, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ConvocationSectionId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    CreatedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    UpdatedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConvocationQuestions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ConvocationQuestions_ConvocationSections_ConvocationSectionId",
                        column: x => x.ConvocationSectionId,
                        principalTable: "TeacherHiring_ConvocationSections",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TeacherHiring_ApplicantTeachers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    UserId = table.Column<string>(type: "varchar(255)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ConvocationId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Status = table.Column<byte>(type: "tinyint unsigned", nullable: false, defaultValue: (byte)1),
                    Valid = table.Column<short>(type: "smallint", nullable: true),
                    ConvocationVacancyId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Observation = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    InterviewScore = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    MasterClassScore = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    ExternalEvaluationScore = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    FinalScore = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    CreatedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    UpdatedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicantTeachers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ApplicantTeachers_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ApplicantTeachers_Convocations_ConvocationId",
                        column: x => x.ConvocationId,
                        principalTable: "TeacherHiring_Convocations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ApplicantTeachers_ConvocationVacancies_ConvocationVacancyId",
                        column: x => x.ConvocationVacancyId,
                        principalTable: "TeacherHiring_ConvocationVacancies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TeacherInvestigation_IncubatorEquipmentExpenses",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    ExpenseCode = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Description = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    MeasureUnit = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    UnitPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ActivityJustification = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IncubatorPostulationId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    CreatedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    UpdatedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IncubatorEquipmentExpenses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_IncubatorEquipmentExpenses_IncubatorPostulations_IncubatorPo~",
                        column: x => x.IncubatorPostulationId,
                        principalTable: "TeacherInvestigation_IncubatorPostulations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TeacherInvestigation_IncubatorOtherExpenses",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    ExpenseCode = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Description = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    MeasureUnit = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    UnitPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ActivityJustification = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IncubatorPostulationId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    CreatedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    UpdatedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IncubatorOtherExpenses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_IncubatorOtherExpenses_IncubatorPostulations_IncubatorPostul~",
                        column: x => x.IncubatorPostulationId,
                        principalTable: "TeacherInvestigation_IncubatorPostulations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TeacherInvestigation_IncubatorPostulationAnnexes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    FilePath = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IncubatorPostulationId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    IncubatorConvocationAnnexId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    CreatedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    UpdatedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IncubatorPostulationAnnexes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_IncubatorPostulationAnnexes_IncubatorConvocationAnnexes_Incu~",
                        column: x => x.IncubatorConvocationAnnexId,
                        principalTable: "TeacherInvestigation_IncubatorConvocationAnnexes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_IncubatorPostulationAnnexes_IncubatorPostulations_IncubatorP~",
                        column: x => x.IncubatorPostulationId,
                        principalTable: "TeacherInvestigation_IncubatorPostulations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TeacherInvestigation_IncubatorPostulationSpecificGoals",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    IncubatorPostulationId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Description = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    OrderNumber = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    CreatedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    UpdatedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IncubatorPostulationSpecificGoals", x => x.Id);
                    table.ForeignKey(
                        name: "FK_IncubatorPostulationSpecificGoals_IncubatorPostulations_Incu~",
                        column: x => x.IncubatorPostulationId,
                        principalTable: "TeacherInvestigation_IncubatorPostulations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TeacherInvestigation_IncubatorPostulationTeamMembers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    UserName = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PaternalSurname = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    MaternalSurname = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Name = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Dni = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Sex = table.Column<int>(type: "int", nullable: false),
                    CurrentAcademicYear = table.Column<int>(type: "int", nullable: false),
                    CareerText = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IncubatorPostulationId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    CreatedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    UpdatedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IncubatorPostulationTeamMembers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_IncubatorPostulationTeamMembers_IncubatorPostulations_Incuba~",
                        column: x => x.IncubatorPostulationId,
                        principalTable: "TeacherInvestigation_IncubatorPostulations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TeacherInvestigation_IncubatorSuppliesExpenses",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    ExpenseCode = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Description = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    MeasureUnit = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    UnitPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ActivityJustification = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IncubatorPostulationId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    CreatedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    UpdatedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IncubatorSuppliesExpenses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_IncubatorSuppliesExpenses_IncubatorPostulations_IncubatorPos~",
                        column: x => x.IncubatorPostulationId,
                        principalTable: "TeacherInvestigation_IncubatorPostulations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TeacherInvestigation_IncubatorThirdPartyServiceExpenses",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    ExpenseCode = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Description = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    MeasureUnit = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    UnitPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ActivityJustification = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IncubatorPostulationId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    CreatedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    UpdatedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IncubatorThirdPartyServiceExpenses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_IncubatorThirdPartyServiceExpenses_IncubatorPostulations_Inc~",
                        column: x => x.IncubatorPostulationId,
                        principalTable: "TeacherInvestigation_IncubatorPostulations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TeacherInvestigation_IncubatorRubricCriterions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Name = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Description = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IncubatorRubricSectionId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    CreatedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    UpdatedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IncubatorRubricCriterions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_IncubatorRubricCriterions_IncubatorRubricSections_IncubatorR~",
                        column: x => x.IncubatorRubricSectionId,
                        principalTable: "TeacherInvestigation_IncubatorRubricSections",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TeacherInvestigation_InvestigationQuestions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    IsRequired = table.Column<short>(type: "smallint", nullable: false),
                    Type = table.Column<int>(type: "int", maxLength: 500, nullable: false),
                    Description = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    InvestigationConvocationRequirementId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    CreatedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    UpdatedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InvestigationQuestions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InvestigationQuestions_InvestigationConvocationRequirements_~",
                        column: x => x.InvestigationConvocationRequirementId,
                        principalTable: "TeacherInvestigation_InvestigationConvocationRequirements",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TeacherInvestigation_ResearchLineCategoryRequirements",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    InvestigationConvocationRequirementId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    ResearchLineCategoryId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Weight = table.Column<int>(type: "int", nullable: false),
                    Hidden = table.Column<short>(type: "smallint", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    CreatedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    UpdatedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResearchLineCategoryRequirements", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ResearchLineCategoryRequirements_InvestigationConvocationReq~",
                        column: x => x.InvestigationConvocationRequirementId,
                        principalTable: "TeacherInvestigation_InvestigationConvocationRequirements",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ResearchLineCategoryRequirements_ResearchLineCategories_Rese~",
                        column: x => x.ResearchLineCategoryId,
                        principalTable: "TeacherInvestigation_ResearchLineCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TeacherInvestigation_InvestigationRubricCriterions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Name = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Description = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    InvestigationRubricSectionId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    CreatedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    UpdatedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InvestigationRubricCriterions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InvestigationRubricCriterions_InvestigationRubricSections_In~",
                        column: x => x.InvestigationRubricSectionId,
                        principalTable: "TeacherInvestigation_InvestigationRubricSections",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TeacherInvestigation_ConferenceAuthors",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    PaternalSurname = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    MaternalSurname = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Name = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Email = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Dni = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ConferenceId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    CreatedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    UpdatedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConferenceAuthors", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ConferenceAuthors_Conferences_ConferenceId",
                        column: x => x.ConferenceId,
                        principalTable: "TeacherInvestigation_Conferences",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TeacherInvestigation_ConferenceFiles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Name = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    FilePath = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ConferenceId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    CreatedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    UpdatedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConferenceFiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ConferenceFiles_Conferences_ConferenceId",
                        column: x => x.ConferenceId,
                        principalTable: "TeacherInvestigation_Conferences",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TeacherInvestigation_PublicationAuthors",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    PaternalSurname = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    MaternalSurname = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Name = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Email = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Dni = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PublicationId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    CreatedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    UpdatedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PublicationAuthors", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PublicationAuthors_Publications_PublicationId",
                        column: x => x.PublicationId,
                        principalTable: "TeacherInvestigation_Publications",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TeacherInvestigation_PublicationFiles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Name = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    FilePath = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PublicationId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    CreatedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    UpdatedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PublicationFiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PublicationFiles_Publications_PublicationId",
                        column: x => x.PublicationId,
                        principalTable: "TeacherInvestigation_Publications",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TeacherInvestigation_InvestigationConvocationInquiries",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Inquiry = table.Column<string>(type: "varchar(2000)", maxLength: 2000, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    FilePath = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    InvestigationConvocationPostulantId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    CreatedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    UpdatedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InvestigationConvocationInquiries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InvestigationConvocationInquiries_InvestigationConvocationPo~",
                        column: x => x.InvestigationConvocationPostulantId,
                        principalTable: "TeacherInvestigation_InvestigationConvocationPostulants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TeacherInvestigation_InvestigationProjects",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    InvestigationConvocationPostulantId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    InvestigationProjectTypeId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    GanttDiagramUrl = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ExecutionAddress = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    GeneralGoal = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    SpecificGoal = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    FinalReportUrl = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    CreatedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    UpdatedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InvestigationProjects", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InvestigationProjects_InvestigationConvocationPostulants_Inv~",
                        column: x => x.InvestigationConvocationPostulantId,
                        principalTable: "TeacherInvestigation_InvestigationConvocationPostulants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_InvestigationProjects_InvestigationProjectTypes_Investigatio~",
                        column: x => x.InvestigationProjectTypeId,
                        principalTable: "TeacherInvestigation_InvestigationProjectTypes",
                        principalColumn: "Id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TeacherInvestigation_PostulantAnnexFiles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    InvestigationConvocationPostulantId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Name = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DocumentPath = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    CreatedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    UpdatedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PostulantAnnexFiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PostulantAnnexFiles_InvestigationConvocationPostulants_Inves~",
                        column: x => x.InvestigationConvocationPostulantId,
                        principalTable: "TeacherInvestigation_InvestigationConvocationPostulants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TeacherInvestigation_PostulantExecutionPlaces",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    DepartmentId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    DepartmentText = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ProvinceId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    ProvinceText = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DistrictId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    DistrictText = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    InvestigationConvocationPostulantId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    CreatedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    UpdatedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PostulantExecutionPlaces", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PostulantExecutionPlaces_InvestigationConvocationPostulants_~",
                        column: x => x.InvestigationConvocationPostulantId,
                        principalTable: "TeacherInvestigation_InvestigationConvocationPostulants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TeacherInvestigation_PostulantExternalMembers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Name = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PaternalSurname = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    MaternalSurname = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Dni = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Description = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    InstitutionOrigin = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Profession = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CvFilePath = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Objectives = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Address = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PhoneNumber = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    InvestigationConvocationPostulantId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    CreatedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    UpdatedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PostulantExternalMembers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PostulantExternalMembers_InvestigationConvocationPostulants_~",
                        column: x => x.InvestigationConvocationPostulantId,
                        principalTable: "TeacherInvestigation_InvestigationConvocationPostulants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TeacherInvestigation_PostulantFinancialFiles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Name = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    FilePath = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    InvestigationConvocationPostulantId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    CreatedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    UpdatedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PostulantFinancialFiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PostulantFinancialFiles_InvestigationConvocationPostulants_I~",
                        column: x => x.InvestigationConvocationPostulantId,
                        principalTable: "TeacherInvestigation_InvestigationConvocationPostulants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TeacherInvestigation_PostulantObservations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    State = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    InvestigationConvocationPostulantId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    CreatedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    UpdatedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PostulantObservations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PostulantObservations_InvestigationConvocationPostulants_Inv~",
                        column: x => x.InvestigationConvocationPostulantId,
                        principalTable: "TeacherInvestigation_InvestigationConvocationPostulants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TeacherInvestigation_PostulantTeamMemberUsers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    InvestigationConvocationPostulantId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    UserId = table.Column<string>(type: "varchar(255)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    TeamMemberRoleId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    CvFilePath = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Objectives = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    CreatedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    UpdatedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PostulantTeamMemberUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PostulantTeamMemberUsers_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PostulantTeamMemberUsers_InvestigationConvocationPostulants_~",
                        column: x => x.InvestigationConvocationPostulantId,
                        principalTable: "TeacherInvestigation_InvestigationConvocationPostulants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PostulantTeamMemberUsers_TeamMemberRoles_TeamMemberRoleId",
                        column: x => x.TeamMemberRoleId,
                        principalTable: "TeacherInvestigation_TeamMemberRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TeacherInvestigation_PostulantTechnicalFiles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Name = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    FilePath = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    InvestigationConvocationPostulantId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    CreatedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    UpdatedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PostulantTechnicalFiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PostulantTechnicalFiles_InvestigationConvocationPostulants_I~",
                        column: x => x.InvestigationConvocationPostulantId,
                        principalTable: "TeacherInvestigation_InvestigationConvocationPostulants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TeacherInvestigation_ProgressFileConvocationPostulants",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Name = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    FilePath = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    InvestigationConvocationPostulantId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    CreatedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    UpdatedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProgressFileConvocationPostulants", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProgressFileConvocationPostulants_InvestigationConvocationPo~",
                        column: x => x.InvestigationConvocationPostulantId,
                        principalTable: "TeacherInvestigation_InvestigationConvocationPostulants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TeacherInvestigation_PostulantResearchLines",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    InvestigationConvocationPostulantId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    ResearchLineId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    CreatedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    UpdatedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PostulantResearchLines", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PostulantResearchLines_InvestigationConvocationPostulants_In~",
                        column: x => x.InvestigationConvocationPostulantId,
                        principalTable: "TeacherInvestigation_InvestigationConvocationPostulants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PostulantResearchLines_ResearchLines_ResearchLineId",
                        column: x => x.ResearchLineId,
                        principalTable: "TeacherInvestigation_ResearchLines",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TeacherInvestigation_EventParticipants",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    EventId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Name = table.Column<string>(type: "varchar(250)", maxLength: 250, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    MaternalSurname = table.Column<string>(type: "varchar(250)", maxLength: 250, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PaternalSurname = table.Column<string>(type: "varchar(250)", maxLength: 250, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    BirthDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    Dni = table.Column<string>(type: "varchar(8)", maxLength: 8, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Email = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    University = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PhoneNumber = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    UserId = table.Column<string>(type: "varchar(255)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    CreatedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    UpdatedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventParticipants", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EventParticipants_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_EventParticipants_Events_EventId",
                        column: x => x.EventId,
                        principalTable: "TeacherInvestigation_Events",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TeacherHiring_ConvocationAnswers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Description = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ConvocationQuestionId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    CreatedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    UpdatedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConvocationAnswers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ConvocationAnswers_ConvocationQuestions_ConvocationQuestionId",
                        column: x => x.ConvocationQuestionId,
                        principalTable: "TeacherHiring_ConvocationQuestions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TeacherHiring_ApplicantTeacherDocuments",
                columns: table => new
                {
                    ApplicantTeacherId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    CovocationDocumentId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    ConvocationDocumentId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    Url = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicantTeacherDocuments", x => new { x.ApplicantTeacherId, x.CovocationDocumentId });
                    table.ForeignKey(
                        name: "FK_ApplicantTeacherDocuments_ApplicantTeachers_ApplicantTeacher~",
                        column: x => x.ApplicantTeacherId,
                        principalTable: "TeacherHiring_ApplicantTeachers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ApplicantTeacherDocuments_ConvocationDocuments_ConvocationDo~",
                        column: x => x.ConvocationDocumentId,
                        principalTable: "TeacherHiring_ConvocationDocuments",
                        principalColumn: "Id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TeacherHiring_ApplicantTeacherInterviews",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    ApplicantTeacherId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    DateTime = table.Column<DateTime>(type: "datetime", nullable: false),
                    InterviewLink = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Type = table.Column<byte>(type: "tinyint unsigned", nullable: false),
                    Topic = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicantTeacherInterviews", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ApplicantTeacherInterviews_ApplicantTeachers_ApplicantTeache~",
                        column: x => x.ApplicantTeacherId,
                        principalTable: "TeacherHiring_ApplicantTeachers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TeacherHiring_ApplicantTeacherRubricItems",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    ApplicantTeacherId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    ConvocationRubricItemId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Score = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    EvaluatorId = table.Column<string>(type: "varchar(255)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicantTeacherRubricItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ApplicantTeacherRubricItems_ApplicantTeachers_ApplicantTeach~",
                        column: x => x.ApplicantTeacherId,
                        principalTable: "TeacherHiring_ApplicantTeachers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ApplicantTeacherRubricItems_AspNetUsers_EvaluatorId",
                        column: x => x.EvaluatorId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ApplicantTeacherRubricItems_ConvocationRubricItems_Convocati~",
                        column: x => x.ConvocationRubricItemId,
                        principalTable: "TeacherHiring_ConvocationRubricItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TeacherHiring_ApplicantTeacherRubricSectionDocuments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    ConvocationRubricSectionId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    ApplicantTeacherId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    FileName = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Url = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicantTeacherRubricSectionDocuments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ApplicantTeacherRubricSectionDocuments_ApplicantTeachers_App~",
                        column: x => x.ApplicantTeacherId,
                        principalTable: "TeacherHiring_ApplicantTeachers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ApplicantTeacherRubricSectionDocuments_ConvocationRubricSect~",
                        column: x => x.ConvocationRubricSectionId,
                        principalTable: "TeacherHiring_ConvocationRubricSections",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TeacherInvestigation_IncubatorPostulationActivities",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    IncubatorPostulationSpecificGoalId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Description = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    OrderNumber = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    CreatedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    UpdatedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IncubatorPostulationActivities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_IncubatorPostulationActivities_IncubatorPostulationSpecificG~",
                        column: x => x.IncubatorPostulationSpecificGoalId,
                        principalTable: "TeacherInvestigation_IncubatorPostulationSpecificGoals",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TeacherInvestigation_IncubatorPostulantRubricQualifications",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    EvaluatorId = table.Column<string>(type: "varchar(255)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IncubatorPostulationId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    IncubatorRubricCriterionId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Value = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    CreatedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    UpdatedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IncubatorPostulantRubricQualifications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_IncubatorPostulantRubricQualifications_AspNetUsers_Evaluator~",
                        column: x => x.EvaluatorId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_IncubatorPostulantRubricQualifications_IncubatorPostulations~",
                        column: x => x.IncubatorPostulationId,
                        principalTable: "TeacherInvestigation_IncubatorPostulations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_IncubatorPostulantRubricQualifications_IncubatorRubricCriter~",
                        column: x => x.IncubatorRubricCriterionId,
                        principalTable: "TeacherInvestigation_IncubatorRubricCriterions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TeacherInvestigation_IncubatorRubricLevels",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Score = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    Description = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IncubatorRubricCriterionId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    CreatedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    UpdatedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IncubatorRubricLevels", x => x.Id);
                    table.ForeignKey(
                        name: "FK_IncubatorRubricLevels_IncubatorRubricCriterions_IncubatorRub~",
                        column: x => x.IncubatorRubricCriterionId,
                        principalTable: "TeacherInvestigation_IncubatorRubricCriterions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TeacherInvestigation_InvestigationAnswers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Description = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    InvestigationQuestionId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    CreatedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    UpdatedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InvestigationAnswers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InvestigationAnswers_InvestigationQuestions_InvestigationQue~",
                        column: x => x.InvestigationQuestionId,
                        principalTable: "TeacherInvestigation_InvestigationQuestions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TeacherInvestigation_InvestigationRubricLevels",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Score = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    Description = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    InvestigationRubricCriterionId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    CreatedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    UpdatedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InvestigationRubricLevels", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InvestigationRubricLevels_InvestigationRubricCriterions_Inve~",
                        column: x => x.InvestigationRubricCriterionId,
                        principalTable: "TeacherInvestigation_InvestigationRubricCriterions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TeacherInvestigation_PostulantRubricQualifications",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    EvaluatorId = table.Column<string>(type: "varchar(255)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    InvestigationConvocationPostulantId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    InvestigationRubricCriterionId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Value = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    CreatedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    UpdatedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PostulantRubricQualifications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PostulantRubricQualifications_AspNetUsers_EvaluatorId",
                        column: x => x.EvaluatorId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PostulantRubricQualifications_InvestigationConvocationPostul~",
                        column: x => x.InvestigationConvocationPostulantId,
                        principalTable: "TeacherInvestigation_InvestigationConvocationPostulants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PostulantRubricQualifications_InvestigationRubricCriterions_~",
                        column: x => x.InvestigationRubricCriterionId,
                        principalTable: "TeacherInvestigation_InvestigationRubricCriterions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TeacherInvestigation_InvestigationProjectReports",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Name = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ExpirationDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    ReportUrl = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    LastEmailSendedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    InvestigationProjectId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    CreatedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    UpdatedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InvestigationProjectReports", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InvestigationProjectReports_InvestigationProjects_Investigat~",
                        column: x => x.InvestigationProjectId,
                        principalTable: "TeacherInvestigation_InvestigationProjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TeacherInvestigation_InvestigationProjectTasks",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    InvestigationProjectId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Description = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    UserId = table.Column<string>(type: "varchar(255)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    CreatedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    UpdatedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InvestigationProjectTasks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InvestigationProjectTasks_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_InvestigationProjectTasks_InvestigationProjects_Investigatio~",
                        column: x => x.InvestigationProjectId,
                        principalTable: "TeacherInvestigation_InvestigationProjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TeacherInvestigation_InvestigationProjectTeamMembers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    InvestigationProjectId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    UserId = table.Column<string>(type: "varchar(255)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    TeamMemberRoleId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    CvFilePath = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Objectives = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    CreatedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    UpdatedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InvestigationProjectTeamMembers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InvestigationProjectTeamMembers_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_InvestigationProjectTeamMembers_InvestigationProjects_Invest~",
                        column: x => x.InvestigationProjectId,
                        principalTable: "TeacherInvestigation_InvestigationProjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_InvestigationProjectTeamMembers_TeamMemberRoles_TeamMemberRo~",
                        column: x => x.TeamMemberRoleId,
                        principalTable: "TeacherInvestigation_TeamMemberRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TeacherInvestigation_ScientificArticles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    InvestigationProjectId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Title = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    FilePath = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    CreatedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    UpdatedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScientificArticles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ScientificArticles_InvestigationProjects_InvestigationProjec~",
                        column: x => x.InvestigationProjectId,
                        principalTable: "TeacherInvestigation_InvestigationProjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TeacherHiring_ConvocationAnswerByUsers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    ConvocationQuestionId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    AnswerDescription = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ConvocationAnswerId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    ApplicantTeacherId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConvocationAnswerByUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ConvocationAnswerByUsers_ApplicantTeachers_ApplicantTeacherId",
                        column: x => x.ApplicantTeacherId,
                        principalTable: "TeacherHiring_ApplicantTeachers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ConvocationAnswerByUsers_ConvocationAnswers_ConvocationAnswe~",
                        column: x => x.ConvocationAnswerId,
                        principalTable: "TeacherHiring_ConvocationAnswers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ConvocationAnswerByUsers_ConvocationQuestions_ConvocationQue~",
                        column: x => x.ConvocationQuestionId,
                        principalTable: "TeacherHiring_ConvocationQuestions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TeacherInvestigation_IncubatorPostulationActivityMonths",
                columns: table => new
                {
                    IncubatorPostulationActivityId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    MonthNumber = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    CreatedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    UpdatedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TeacherInvestigation_IncubatorPostulationActivityMonths", x => new { x.IncubatorPostulationActivityId, x.MonthNumber });
                    table.ForeignKey(
                        name: "FK_IncubatorPostulationActivityMonths_IncubatorPostulationActiv~",
                        column: x => x.IncubatorPostulationActivityId,
                        principalTable: "TeacherInvestigation_IncubatorPostulationActivities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TeacherInvestigation_InvestigationAnswerByUsers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    AnswerDescription = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    InvestigationQuestionId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    InvestigationAnswerId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    UserId = table.Column<string>(type: "varchar(255)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    CreatedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    UpdatedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InvestigationAnswerByUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InvestigationAnswerByUsers_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_InvestigationAnswerByUsers_InvestigationAnswers_Investigatio~",
                        column: x => x.InvestigationAnswerId,
                        principalTable: "TeacherInvestigation_InvestigationAnswers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_InvestigationAnswerByUsers_InvestigationQuestions_Investigat~",
                        column: x => x.InvestigationQuestionId,
                        principalTable: "TeacherInvestigation_InvestigationQuestions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TeacherInvestigation_InvestigationProjectExpenses",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    InvestigationProjectTaskId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Description = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ProductType = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ExpenseCode = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    CreatedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    UpdatedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InvestigationProjectExpenses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InvestigationProjectExpenses_InvestigationProjectTasks_Inves~",
                        column: x => x.InvestigationProjectTaskId,
                        principalTable: "TeacherInvestigation_InvestigationProjectTasks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoles_NormalizedName",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_FullName",
                table: "AspNetUsers",
                column: "FullName");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_NormalizedEmail",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_NormalizedUserName",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_UserName",
                table: "AspNetUsers",
                column: "UserName");

            migrationBuilder.CreateIndex(
                name: "IX_UserRequests_UserId",
                table: "General_UserRequests",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicantTeacherDocuments_ConvocationDocumentId",
                table: "TeacherHiring_ApplicantTeacherDocuments",
                column: "ConvocationDocumentId");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicantTeacherInterviews_ApplicantTeacherId",
                table: "TeacherHiring_ApplicantTeacherInterviews",
                column: "ApplicantTeacherId");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicantTeacherRubricItems_ApplicantTeacherId",
                table: "TeacherHiring_ApplicantTeacherRubricItems",
                column: "ApplicantTeacherId");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicantTeacherRubricItems_ConvocationRubricItemId",
                table: "TeacherHiring_ApplicantTeacherRubricItems",
                column: "ConvocationRubricItemId");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicantTeacherRubricItems_EvaluatorId",
                table: "TeacherHiring_ApplicantTeacherRubricItems",
                column: "EvaluatorId");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicantTeacherRubricSectionDocuments_ApplicantTeacherId",
                table: "TeacherHiring_ApplicantTeacherRubricSectionDocuments",
                column: "ApplicantTeacherId");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicantTeacherRubricSectionDocuments_ConvocationRubricSect~",
                table: "TeacherHiring_ApplicantTeacherRubricSectionDocuments",
                column: "ConvocationRubricSectionId");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicantTeachers_ConvocationId",
                table: "TeacherHiring_ApplicantTeachers",
                column: "ConvocationId");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicantTeachers_ConvocationVacancyId",
                table: "TeacherHiring_ApplicantTeachers",
                column: "ConvocationVacancyId");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicantTeachers_UserId",
                table: "TeacherHiring_ApplicantTeachers",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ConvocationAnswerByUsers_ApplicantTeacherId",
                table: "TeacherHiring_ConvocationAnswerByUsers",
                column: "ApplicantTeacherId");

            migrationBuilder.CreateIndex(
                name: "IX_ConvocationAnswerByUsers_ConvocationAnswerId",
                table: "TeacherHiring_ConvocationAnswerByUsers",
                column: "ConvocationAnswerId");

            migrationBuilder.CreateIndex(
                name: "IX_ConvocationAnswerByUsers_ConvocationQuestionId",
                table: "TeacherHiring_ConvocationAnswerByUsers",
                column: "ConvocationQuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_ConvocationAnswers_ConvocationQuestionId",
                table: "TeacherHiring_ConvocationAnswers",
                column: "ConvocationQuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_ConvocationCalendars_ConvocationId",
                table: "TeacherHiring_ConvocationCalendars",
                column: "ConvocationId");

            migrationBuilder.CreateIndex(
                name: "IX_ConvocationComitees_ApplicationRoleId",
                table: "TeacherHiring_ConvocationComitees",
                column: "ApplicationRoleId");

            migrationBuilder.CreateIndex(
                name: "IX_TeacherHiring_ConvocationComitees_ConvocationId",
                table: "TeacherHiring_ConvocationComitees",
                column: "ConvocationId");

            migrationBuilder.CreateIndex(
                name: "IX_ConvocationDocuments_ConvocationId",
                table: "TeacherHiring_ConvocationDocuments",
                column: "ConvocationId");

            migrationBuilder.CreateIndex(
                name: "IX_ConvocationQuestions_ConvocationSectionId",
                table: "TeacherHiring_ConvocationQuestions",
                column: "ConvocationSectionId");

            migrationBuilder.CreateIndex(
                name: "IX_ConvocationRubricItems_ConvocationRubricSectionId",
                table: "TeacherHiring_ConvocationRubricItems",
                column: "ConvocationRubricSectionId");

            migrationBuilder.CreateIndex(
                name: "IX_ConvocationRubricSections_ConvocationId",
                table: "TeacherHiring_ConvocationRubricSections",
                column: "ConvocationId");

            migrationBuilder.CreateIndex(
                name: "IX_ConvocationSections_ConvocationId",
                table: "TeacherHiring_ConvocationSections",
                column: "ConvocationId");

            migrationBuilder.CreateIndex(
                name: "IX_ConvocationVacancies_ConvocationId",
                table: "TeacherHiring_ConvocationVacancies",
                column: "ConvocationId");

            migrationBuilder.CreateIndex(
                name: "IX_ConferenceAuthors_ConferenceId",
                table: "TeacherInvestigation_ConferenceAuthors",
                column: "ConferenceId");

            migrationBuilder.CreateIndex(
                name: "IX_ConferenceFiles_ConferenceId",
                table: "TeacherInvestigation_ConferenceFiles",
                column: "ConferenceId");

            migrationBuilder.CreateIndex(
                name: "IX_Conferences_OpusTypeId",
                table: "TeacherInvestigation_Conferences",
                column: "OpusTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Conferences_UserId",
                table: "TeacherInvestigation_Conferences",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_CoordinatorMonitorConvocations_InvestigationConvocationId",
                table: "TeacherInvestigation_CoordinatorMonitorConvocations",
                column: "InvestigationConvocationId");

            migrationBuilder.CreateIndex(
                name: "IX_EvaluatorCommitteeConvocations_InvestigationConvocationId",
                table: "TeacherInvestigation_EvaluatorCommitteeConvocations",
                column: "InvestigationConvocationId");

            migrationBuilder.CreateIndex(
                name: "IX_EventParticipants_EventId",
                table: "TeacherInvestigation_EventParticipants",
                column: "EventId");

            migrationBuilder.CreateIndex(
                name: "IX_EventParticipants_UserId",
                table: "TeacherInvestigation_EventParticipants",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Events_UnitId",
                table: "TeacherInvestigation_Events",
                column: "UnitId");

            migrationBuilder.CreateIndex(
                name: "IX_IncubatorConvocationAnnexes_IncubatorConvocationId",
                table: "TeacherInvestigation_IncubatorConvocationAnnexes",
                column: "IncubatorConvocationId");

            migrationBuilder.CreateIndex(
                name: "IX_TeacherInvestigation_IncubatorConvocationEvaluators_Incubato~",
                table: "TeacherInvestigation_IncubatorConvocationEvaluators",
                column: "IncubatorConvocationId");

            migrationBuilder.CreateIndex(
                name: "IX_IncubatorConvocationFaculties_IncubatorConvocationId",
                table: "TeacherInvestigation_IncubatorConvocationFaculties",
                column: "IncubatorConvocationId");

            migrationBuilder.CreateIndex(
                name: "IX_IncubatorConvocationFiles_IncubatorConvocationId",
                table: "TeacherInvestigation_IncubatorConvocationFiles",
                column: "IncubatorConvocationId");

            migrationBuilder.CreateIndex(
                name: "IX_IncubatorCoordinatorMonitors_IncubatorConvocationId",
                table: "TeacherInvestigation_IncubatorCoordinatorMonitors",
                column: "IncubatorConvocationId");

            migrationBuilder.CreateIndex(
                name: "IX_IncubatorEquipmentExpenses_IncubatorPostulationId",
                table: "TeacherInvestigation_IncubatorEquipmentExpenses",
                column: "IncubatorPostulationId");

            migrationBuilder.CreateIndex(
                name: "IX_IncubatorMonitors_IncubatorConvocationId",
                table: "TeacherInvestigation_IncubatorMonitors",
                column: "IncubatorConvocationId");

            migrationBuilder.CreateIndex(
                name: "IX_IncubatorOtherExpenses_IncubatorPostulationId",
                table: "TeacherInvestigation_IncubatorOtherExpenses",
                column: "IncubatorPostulationId");

            migrationBuilder.CreateIndex(
                name: "IX_IncubatorPostulantRubricQualifications_EvaluatorId",
                table: "TeacherInvestigation_IncubatorPostulantRubricQualifications",
                column: "EvaluatorId");

            migrationBuilder.CreateIndex(
                name: "IX_IncubatorPostulantRubricQualifications_IncubatorPostulationId",
                table: "TeacherInvestigation_IncubatorPostulantRubricQualifications",
                column: "IncubatorPostulationId");

            migrationBuilder.CreateIndex(
                name: "IX_IncubatorPostulantRubricQualifications_IncubatorRubricCriter~",
                table: "TeacherInvestigation_IncubatorPostulantRubricQualifications",
                column: "IncubatorRubricCriterionId");

            migrationBuilder.CreateIndex(
                name: "IX_IncubatorPostulationActivities_IncubatorPostulationSpecificG~",
                table: "TeacherInvestigation_IncubatorPostulationActivities",
                column: "IncubatorPostulationSpecificGoalId");

            migrationBuilder.CreateIndex(
                name: "IX_IncubatorPostulationAnnexes_IncubatorConvocationAnnexId",
                table: "TeacherInvestigation_IncubatorPostulationAnnexes",
                column: "IncubatorConvocationAnnexId");

            migrationBuilder.CreateIndex(
                name: "IX_IncubatorPostulationAnnexes_IncubatorPostulationId",
                table: "TeacherInvestigation_IncubatorPostulationAnnexes",
                column: "IncubatorPostulationId");

            migrationBuilder.CreateIndex(
                name: "IX_IncubatorPostulations_IncubatorConvocationId",
                table: "TeacherInvestigation_IncubatorPostulations",
                column: "IncubatorConvocationId");

            migrationBuilder.CreateIndex(
                name: "IX_IncubatorPostulations_UserId",
                table: "TeacherInvestigation_IncubatorPostulations",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_IncubatorPostulationSpecificGoals_IncubatorPostulationId",
                table: "TeacherInvestigation_IncubatorPostulationSpecificGoals",
                column: "IncubatorPostulationId");

            migrationBuilder.CreateIndex(
                name: "IX_IncubatorPostulationTeamMembers_IncubatorPostulationId",
                table: "TeacherInvestigation_IncubatorPostulationTeamMembers",
                column: "IncubatorPostulationId");

            migrationBuilder.CreateIndex(
                name: "IX_IncubatorRubricCriterions_IncubatorRubricSectionId",
                table: "TeacherInvestigation_IncubatorRubricCriterions",
                column: "IncubatorRubricSectionId");

            migrationBuilder.CreateIndex(
                name: "IX_IncubatorRubricLevels_IncubatorRubricCriterionId",
                table: "TeacherInvestigation_IncubatorRubricLevels",
                column: "IncubatorRubricCriterionId");

            migrationBuilder.CreateIndex(
                name: "IX_IncubatorRubricSections_IncubatorConvocationId",
                table: "TeacherInvestigation_IncubatorRubricSections",
                column: "IncubatorConvocationId");

            migrationBuilder.CreateIndex(
                name: "IX_IncubatorSuppliesExpenses_IncubatorPostulationId",
                table: "TeacherInvestigation_IncubatorSuppliesExpenses",
                column: "IncubatorPostulationId");

            migrationBuilder.CreateIndex(
                name: "IX_IncubatorThirdPartyServiceExpenses_IncubatorPostulationId",
                table: "TeacherInvestigation_IncubatorThirdPartyServiceExpenses",
                column: "IncubatorPostulationId");

            migrationBuilder.CreateIndex(
                name: "IX_InvestigationAnswerByUsers_InvestigationAnswerId",
                table: "TeacherInvestigation_InvestigationAnswerByUsers",
                column: "InvestigationAnswerId");

            migrationBuilder.CreateIndex(
                name: "IX_InvestigationAnswerByUsers_InvestigationQuestionId",
                table: "TeacherInvestigation_InvestigationAnswerByUsers",
                column: "InvestigationQuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_InvestigationAnswerByUsers_UserId",
                table: "TeacherInvestigation_InvestigationAnswerByUsers",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_InvestigationAnswers_InvestigationQuestionId",
                table: "TeacherInvestigation_InvestigationAnswers",
                column: "InvestigationQuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_TeacherInvestigation_InvestigationConvocationEvaluators_Inve~",
                table: "TeacherInvestigation_InvestigationConvocationEvaluators",
                column: "InvestigationConvocationId");

            migrationBuilder.CreateIndex(
                name: "IX_InvestigationConvocationFiles_InvestigationConvocationId",
                table: "TeacherInvestigation_InvestigationConvocationFiles",
                column: "InvestigationConvocationId");

            migrationBuilder.CreateIndex(
                name: "IX_InvestigationConvocationHistories_InvestigationConvocationId",
                table: "TeacherInvestigation_InvestigationConvocationHistories",
                column: "InvestigationConvocationId");

            migrationBuilder.CreateIndex(
                name: "IX_InvestigationConvocationHistories_UserId",
                table: "TeacherInvestigation_InvestigationConvocationHistories",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_InvestigationConvocationInquiries_InvestigationConvocationPo~",
                table: "TeacherInvestigation_InvestigationConvocationInquiries",
                column: "InvestigationConvocationPostulantId");

            migrationBuilder.CreateIndex(
                name: "IX_InvestigationConvocationPostulants_ExternalEntityId",
                table: "TeacherInvestigation_InvestigationConvocationPostulants",
                column: "ExternalEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_InvestigationConvocationPostulants_FinancingInvestigationId",
                table: "TeacherInvestigation_InvestigationConvocationPostulants",
                column: "FinancingInvestigationId");

            migrationBuilder.CreateIndex(
                name: "IX_InvestigationConvocationPostulants_InvestigationAreaId",
                table: "TeacherInvestigation_InvestigationConvocationPostulants",
                column: "InvestigationAreaId");

            migrationBuilder.CreateIndex(
                name: "IX_InvestigationConvocationPostulants_InvestigationConvocationId",
                table: "TeacherInvestigation_InvestigationConvocationPostulants",
                column: "InvestigationConvocationId");

            migrationBuilder.CreateIndex(
                name: "IX_InvestigationConvocationPostulants_InvestigationPatternId",
                table: "TeacherInvestigation_InvestigationConvocationPostulants",
                column: "InvestigationPatternId");

            migrationBuilder.CreateIndex(
                name: "IX_InvestigationConvocationPostulants_InvestigationTypeId",
                table: "TeacherInvestigation_InvestigationConvocationPostulants",
                column: "InvestigationTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_InvestigationConvocationPostulants_MethodologyTypeId",
                table: "TeacherInvestigation_InvestigationConvocationPostulants",
                column: "MethodologyTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_InvestigationConvocationPostulants_MonitorUserId",
                table: "TeacherInvestigation_InvestigationConvocationPostulants",
                column: "MonitorUserId");

            migrationBuilder.CreateIndex(
                name: "IX_InvestigationConvocationPostulants_ResearchCenterId",
                table: "TeacherInvestigation_InvestigationConvocationPostulants",
                column: "ResearchCenterId");

            migrationBuilder.CreateIndex(
                name: "IX_InvestigationConvocationPostulants_UserId",
                table: "TeacherInvestigation_InvestigationConvocationPostulants",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_InvestigationConvocationSupervisors_InvestigationConvocation~",
                table: "TeacherInvestigation_InvestigationConvocationSupervisors",
                column: "InvestigationConvocationId");

            migrationBuilder.CreateIndex(
                name: "IX_InvestigationProjectExpenses_InvestigationProjectTaskId",
                table: "TeacherInvestigation_InvestigationProjectExpenses",
                column: "InvestigationProjectTaskId");

            migrationBuilder.CreateIndex(
                name: "IX_InvestigationProjectReports_InvestigationProjectId",
                table: "TeacherInvestigation_InvestigationProjectReports",
                column: "InvestigationProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_InvestigationProjects_InvestigationConvocationPostulantId",
                table: "TeacherInvestigation_InvestigationProjects",
                column: "InvestigationConvocationPostulantId");

            migrationBuilder.CreateIndex(
                name: "IX_InvestigationProjects_InvestigationProjectTypeId",
                table: "TeacherInvestigation_InvestigationProjects",
                column: "InvestigationProjectTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_InvestigationProjectTasks_InvestigationProjectId",
                table: "TeacherInvestigation_InvestigationProjectTasks",
                column: "InvestigationProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_InvestigationProjectTasks_UserId",
                table: "TeacherInvestigation_InvestigationProjectTasks",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_InvestigationProjectTeamMembers_InvestigationProjectId",
                table: "TeacherInvestigation_InvestigationProjectTeamMembers",
                column: "InvestigationProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_InvestigationProjectTeamMembers_TeamMemberRoleId",
                table: "TeacherInvestigation_InvestigationProjectTeamMembers",
                column: "TeamMemberRoleId");

            migrationBuilder.CreateIndex(
                name: "IX_InvestigationProjectTeamMembers_UserId",
                table: "TeacherInvestigation_InvestigationProjectTeamMembers",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_InvestigationQuestions_InvestigationConvocationRequirementId",
                table: "TeacherInvestigation_InvestigationQuestions",
                column: "InvestigationConvocationRequirementId");

            migrationBuilder.CreateIndex(
                name: "IX_InvestigationRubricCriterions_InvestigationRubricSectionId",
                table: "TeacherInvestigation_InvestigationRubricCriterions",
                column: "InvestigationRubricSectionId");

            migrationBuilder.CreateIndex(
                name: "IX_InvestigationRubricLevels_InvestigationRubricCriterionId",
                table: "TeacherInvestigation_InvestigationRubricLevels",
                column: "InvestigationRubricCriterionId");

            migrationBuilder.CreateIndex(
                name: "IX_InvestigationRubricSections_InvestigationConvocationId",
                table: "TeacherInvestigation_InvestigationRubricSections",
                column: "InvestigationConvocationId");

            migrationBuilder.CreateIndex(
                name: "IX_MonitorConvocations_InvestigationConvocationId",
                table: "TeacherInvestigation_MonitorConvocations",
                column: "InvestigationConvocationId");

            migrationBuilder.CreateIndex(
                name: "IX_OperativePlans_UnitId",
                table: "TeacherInvestigation_OperativePlans",
                column: "UnitId");

            migrationBuilder.CreateIndex(
                name: "IX_PostulantAnnexFiles_InvestigationConvocationPostulantId",
                table: "TeacherInvestigation_PostulantAnnexFiles",
                column: "InvestigationConvocationPostulantId");

            migrationBuilder.CreateIndex(
                name: "IX_PostulantExecutionPlaces_InvestigationConvocationPostulantId",
                table: "TeacherInvestigation_PostulantExecutionPlaces",
                column: "InvestigationConvocationPostulantId");

            migrationBuilder.CreateIndex(
                name: "IX_PostulantExternalMembers_InvestigationConvocationPostulantId",
                table: "TeacherInvestigation_PostulantExternalMembers",
                column: "InvestigationConvocationPostulantId");

            migrationBuilder.CreateIndex(
                name: "IX_PostulantFinancialFiles_InvestigationConvocationPostulantId",
                table: "TeacherInvestigation_PostulantFinancialFiles",
                column: "InvestigationConvocationPostulantId");

            migrationBuilder.CreateIndex(
                name: "IX_PostulantObservations_InvestigationConvocationPostulantId",
                table: "TeacherInvestigation_PostulantObservations",
                column: "InvestigationConvocationPostulantId");

            migrationBuilder.CreateIndex(
                name: "IX_PostulantResearchLines_InvestigationConvocationPostulantId",
                table: "TeacherInvestigation_PostulantResearchLines",
                column: "InvestigationConvocationPostulantId");

            migrationBuilder.CreateIndex(
                name: "IX_PostulantResearchLines_ResearchLineId",
                table: "TeacherInvestigation_PostulantResearchLines",
                column: "ResearchLineId");

            migrationBuilder.CreateIndex(
                name: "IX_PostulantRubricQualifications_EvaluatorId",
                table: "TeacherInvestigation_PostulantRubricQualifications",
                column: "EvaluatorId");

            migrationBuilder.CreateIndex(
                name: "IX_PostulantRubricQualifications_InvestigationConvocationPostul~",
                table: "TeacherInvestigation_PostulantRubricQualifications",
                column: "InvestigationConvocationPostulantId");

            migrationBuilder.CreateIndex(
                name: "IX_PostulantRubricQualifications_InvestigationRubricCriterionId",
                table: "TeacherInvestigation_PostulantRubricQualifications",
                column: "InvestigationRubricCriterionId");

            migrationBuilder.CreateIndex(
                name: "IX_PostulantTeamMemberUsers_InvestigationConvocationPostulantId",
                table: "TeacherInvestigation_PostulantTeamMemberUsers",
                column: "InvestigationConvocationPostulantId");

            migrationBuilder.CreateIndex(
                name: "IX_PostulantTeamMemberUsers_TeamMemberRoleId",
                table: "TeacherInvestigation_PostulantTeamMemberUsers",
                column: "TeamMemberRoleId");

            migrationBuilder.CreateIndex(
                name: "IX_PostulantTeamMemberUsers_UserId",
                table: "TeacherInvestigation_PostulantTeamMemberUsers",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_PostulantTechnicalFiles_InvestigationConvocationPostulantId",
                table: "TeacherInvestigation_PostulantTechnicalFiles",
                column: "InvestigationConvocationPostulantId");

            migrationBuilder.CreateIndex(
                name: "IX_ProgressFileConvocationPostulants_InvestigationConvocationPo~",
                table: "TeacherInvestigation_ProgressFileConvocationPostulants",
                column: "InvestigationConvocationPostulantId");

            migrationBuilder.CreateIndex(
                name: "IX_PublicationAuthors_PublicationId",
                table: "TeacherInvestigation_PublicationAuthors",
                column: "PublicationId");

            migrationBuilder.CreateIndex(
                name: "IX_PublicationFiles_PublicationId",
                table: "TeacherInvestigation_PublicationFiles",
                column: "PublicationId");

            migrationBuilder.CreateIndex(
                name: "IX_Publications_AuthorshipOrderId",
                table: "TeacherInvestigation_Publications",
                column: "AuthorshipOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_Publications_IdentificationTypeId",
                table: "TeacherInvestigation_Publications",
                column: "IdentificationTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Publications_IndexPlaceId",
                table: "TeacherInvestigation_Publications",
                column: "IndexPlaceId");

            migrationBuilder.CreateIndex(
                name: "IX_Publications_OpusTypeId",
                table: "TeacherInvestigation_Publications",
                column: "OpusTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Publications_PublicationFunctionId",
                table: "TeacherInvestigation_Publications",
                column: "PublicationFunctionId");

            migrationBuilder.CreateIndex(
                name: "IX_Publications_UserId",
                table: "TeacherInvestigation_Publications",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_PublishedBookAuthors_PublishedBookId",
                table: "TeacherInvestigation_PublishedBookAuthors",
                column: "PublishedBookId");

            migrationBuilder.CreateIndex(
                name: "IX_PublishedBookFiles_PublishedBookId",
                table: "TeacherInvestigation_PublishedBookFiles",
                column: "PublishedBookId");

            migrationBuilder.CreateIndex(
                name: "IX_PublishedBooks_UserId",
                table: "TeacherInvestigation_PublishedBooks",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_PublishedChapterBookAuthors_PublishedChapterBookId",
                table: "TeacherInvestigation_PublishedChapterBookAuthors",
                column: "PublishedChapterBookId");

            migrationBuilder.CreateIndex(
                name: "IX_PublishedChapterBookFiles_PublishedChapterBookId",
                table: "TeacherInvestigation_PublishedChapterBookFiles",
                column: "PublishedChapterBookId");

            migrationBuilder.CreateIndex(
                name: "IX_PublishedChapterBooks_UserId",
                table: "TeacherInvestigation_PublishedChapterBooks",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ResearchLineCategoryRequirements_InvestigationConvocationReq~",
                table: "TeacherInvestigation_ResearchLineCategoryRequirements",
                column: "InvestigationConvocationRequirementId");

            migrationBuilder.CreateIndex(
                name: "IX_ResearchLineCategoryRequirements_ResearchLineCategoryId",
                table: "TeacherInvestigation_ResearchLineCategoryRequirements",
                column: "ResearchLineCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_ResearchLines_ResearchLineCategoryId",
                table: "TeacherInvestigation_ResearchLines",
                column: "ResearchLineCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_ScientificArticles_InvestigationProjectId",
                table: "TeacherInvestigation_ScientificArticles",
                column: "InvestigationProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_Units_UserId",
                table: "TeacherInvestigation_Units",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "General_Audits");

            migrationBuilder.DropTable(
                name: "General_Configurations");

            migrationBuilder.DropTable(
                name: "General_UserRequests");

            migrationBuilder.DropTable(
                name: "TeacherHiring_ApplicantTeacherDocuments");

            migrationBuilder.DropTable(
                name: "TeacherHiring_ApplicantTeacherInterviews");

            migrationBuilder.DropTable(
                name: "TeacherHiring_ApplicantTeacherRubricItems");

            migrationBuilder.DropTable(
                name: "TeacherHiring_ApplicantTeacherRubricSectionDocuments");

            migrationBuilder.DropTable(
                name: "TeacherHiring_ConvocationAnswerByUsers");

            migrationBuilder.DropTable(
                name: "TeacherHiring_ConvocationCalendars");

            migrationBuilder.DropTable(
                name: "TeacherHiring_ConvocationComitees");

            migrationBuilder.DropTable(
                name: "TeacherInvestigation_ConferenceAuthors");

            migrationBuilder.DropTable(
                name: "TeacherInvestigation_ConferenceFiles");

            migrationBuilder.DropTable(
                name: "TeacherInvestigation_CoordinatorMonitorConvocations");

            migrationBuilder.DropTable(
                name: "TeacherInvestigation_EvaluatorCommitteeConvocations");

            migrationBuilder.DropTable(
                name: "TeacherInvestigation_EventParticipants");

            migrationBuilder.DropTable(
                name: "TeacherInvestigation_IncubatorConvocationEvaluators");

            migrationBuilder.DropTable(
                name: "TeacherInvestigation_IncubatorConvocationFaculties");

            migrationBuilder.DropTable(
                name: "TeacherInvestigation_IncubatorConvocationFiles");

            migrationBuilder.DropTable(
                name: "TeacherInvestigation_IncubatorCoordinatorMonitors");

            migrationBuilder.DropTable(
                name: "TeacherInvestigation_IncubatorEquipmentExpenses");

            migrationBuilder.DropTable(
                name: "TeacherInvestigation_IncubatorMonitors");

            migrationBuilder.DropTable(
                name: "TeacherInvestigation_IncubatorOtherExpenses");

            migrationBuilder.DropTable(
                name: "TeacherInvestigation_IncubatorPostulantRubricQualifications");

            migrationBuilder.DropTable(
                name: "TeacherInvestigation_IncubatorPostulationActivityMonths");

            migrationBuilder.DropTable(
                name: "TeacherInvestigation_IncubatorPostulationAnnexes");

            migrationBuilder.DropTable(
                name: "TeacherInvestigation_IncubatorPostulationTeamMembers");

            migrationBuilder.DropTable(
                name: "TeacherInvestigation_IncubatorRubricLevels");

            migrationBuilder.DropTable(
                name: "TeacherInvestigation_IncubatorSuppliesExpenses");

            migrationBuilder.DropTable(
                name: "TeacherInvestigation_IncubatorThirdPartyServiceExpenses");

            migrationBuilder.DropTable(
                name: "TeacherInvestigation_InvestigationAnswerByUsers");

            migrationBuilder.DropTable(
                name: "TeacherInvestigation_InvestigationConvocationEvaluators");

            migrationBuilder.DropTable(
                name: "TeacherInvestigation_InvestigationConvocationFiles");

            migrationBuilder.DropTable(
                name: "TeacherInvestigation_InvestigationConvocationHistories");

            migrationBuilder.DropTable(
                name: "TeacherInvestigation_InvestigationConvocationInquiries");

            migrationBuilder.DropTable(
                name: "TeacherInvestigation_InvestigationConvocationSupervisors");

            migrationBuilder.DropTable(
                name: "TeacherInvestigation_InvestigationProjectExpenses");

            migrationBuilder.DropTable(
                name: "TeacherInvestigation_InvestigationProjectReports");

            migrationBuilder.DropTable(
                name: "TeacherInvestigation_InvestigationProjectTeamMembers");

            migrationBuilder.DropTable(
                name: "TeacherInvestigation_InvestigationRubricLevels");

            migrationBuilder.DropTable(
                name: "TeacherInvestigation_MonitorConvocations");

            migrationBuilder.DropTable(
                name: "TeacherInvestigation_OperativePlans");

            migrationBuilder.DropTable(
                name: "TeacherInvestigation_PostulantAnnexFiles");

            migrationBuilder.DropTable(
                name: "TeacherInvestigation_PostulantExecutionPlaces");

            migrationBuilder.DropTable(
                name: "TeacherInvestigation_PostulantExternalMembers");

            migrationBuilder.DropTable(
                name: "TeacherInvestigation_PostulantFinancialFiles");

            migrationBuilder.DropTable(
                name: "TeacherInvestigation_PostulantObservations");

            migrationBuilder.DropTable(
                name: "TeacherInvestigation_PostulantResearchLines");

            migrationBuilder.DropTable(
                name: "TeacherInvestigation_PostulantRubricQualifications");

            migrationBuilder.DropTable(
                name: "TeacherInvestigation_PostulantTeamMemberUsers");

            migrationBuilder.DropTable(
                name: "TeacherInvestigation_PostulantTechnicalFiles");

            migrationBuilder.DropTable(
                name: "TeacherInvestigation_ProgressFileConvocationPostulants");

            migrationBuilder.DropTable(
                name: "TeacherInvestigation_PublicationAuthors");

            migrationBuilder.DropTable(
                name: "TeacherInvestigation_PublicationFiles");

            migrationBuilder.DropTable(
                name: "TeacherInvestigation_PublishedBookAuthors");

            migrationBuilder.DropTable(
                name: "TeacherInvestigation_PublishedBookFiles");

            migrationBuilder.DropTable(
                name: "TeacherInvestigation_PublishedChapterBookAuthors");

            migrationBuilder.DropTable(
                name: "TeacherInvestigation_PublishedChapterBookFiles");

            migrationBuilder.DropTable(
                name: "TeacherInvestigation_ResearchLineCategoryRequirements");

            migrationBuilder.DropTable(
                name: "TeacherInvestigation_ScientificArticles");

            migrationBuilder.DropTable(
                name: "TeacherHiring_ConvocationDocuments");

            migrationBuilder.DropTable(
                name: "TeacherHiring_ConvocationRubricItems");

            migrationBuilder.DropTable(
                name: "TeacherHiring_ApplicantTeachers");

            migrationBuilder.DropTable(
                name: "TeacherHiring_ConvocationAnswers");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "TeacherInvestigation_Conferences");

            migrationBuilder.DropTable(
                name: "TeacherInvestigation_Events");

            migrationBuilder.DropTable(
                name: "TeacherInvestigation_IncubatorPostulationActivities");

            migrationBuilder.DropTable(
                name: "TeacherInvestigation_IncubatorConvocationAnnexes");

            migrationBuilder.DropTable(
                name: "TeacherInvestigation_IncubatorRubricCriterions");

            migrationBuilder.DropTable(
                name: "TeacherInvestigation_InvestigationAnswers");

            migrationBuilder.DropTable(
                name: "TeacherInvestigation_InvestigationProjectTasks");

            migrationBuilder.DropTable(
                name: "TeacherInvestigation_ResearchLines");

            migrationBuilder.DropTable(
                name: "TeacherInvestigation_InvestigationRubricCriterions");

            migrationBuilder.DropTable(
                name: "TeacherInvestigation_TeamMemberRoles");

            migrationBuilder.DropTable(
                name: "TeacherInvestigation_Publications");

            migrationBuilder.DropTable(
                name: "TeacherInvestigation_PublishedBooks");

            migrationBuilder.DropTable(
                name: "TeacherInvestigation_PublishedChapterBooks");

            migrationBuilder.DropTable(
                name: "TeacherHiring_ConvocationRubricSections");

            migrationBuilder.DropTable(
                name: "TeacherHiring_ConvocationVacancies");

            migrationBuilder.DropTable(
                name: "TeacherHiring_ConvocationQuestions");

            migrationBuilder.DropTable(
                name: "TeacherInvestigation_Units");

            migrationBuilder.DropTable(
                name: "TeacherInvestigation_IncubatorPostulationSpecificGoals");

            migrationBuilder.DropTable(
                name: "TeacherInvestigation_IncubatorRubricSections");

            migrationBuilder.DropTable(
                name: "TeacherInvestigation_InvestigationQuestions");

            migrationBuilder.DropTable(
                name: "TeacherInvestigation_InvestigationProjects");

            migrationBuilder.DropTable(
                name: "TeacherInvestigation_ResearchLineCategories");

            migrationBuilder.DropTable(
                name: "TeacherInvestigation_InvestigationRubricSections");

            migrationBuilder.DropTable(
                name: "TeacherInvestigation_AuthorshipOrders");

            migrationBuilder.DropTable(
                name: "TeacherInvestigation_IdentificationTypes");

            migrationBuilder.DropTable(
                name: "TeacherInvestigation_IndexPlaces");

            migrationBuilder.DropTable(
                name: "TeacherInvestigation_OpusTypes");

            migrationBuilder.DropTable(
                name: "TeacherInvestigation_PublicationFunctions");

            migrationBuilder.DropTable(
                name: "TeacherHiring_ConvocationSections");

            migrationBuilder.DropTable(
                name: "TeacherInvestigation_IncubatorPostulations");

            migrationBuilder.DropTable(
                name: "TeacherInvestigation_InvestigationConvocationRequirements");

            migrationBuilder.DropTable(
                name: "TeacherInvestigation_InvestigationConvocationPostulants");

            migrationBuilder.DropTable(
                name: "TeacherInvestigation_InvestigationProjectTypes");

            migrationBuilder.DropTable(
                name: "TeacherHiring_Convocations");

            migrationBuilder.DropTable(
                name: "TeacherInvestigation_IncubatorConvocations");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "TeacherInvestigation_ExternalEntities");

            migrationBuilder.DropTable(
                name: "TeacherInvestigation_FinancingInvestigations");

            migrationBuilder.DropTable(
                name: "TeacherInvestigation_InvestigationAreas");

            migrationBuilder.DropTable(
                name: "TeacherInvestigation_InvestigationConvocations");

            migrationBuilder.DropTable(
                name: "TeacherInvestigation_InvestigationPatterns");

            migrationBuilder.DropTable(
                name: "TeacherInvestigation_InvestigationTypes");

            migrationBuilder.DropTable(
                name: "TeacherInvestigation_MethodologyTypes");

            migrationBuilder.DropTable(
                name: "TeacherInvestigation_ResearchCenters");
        }
    }
}
