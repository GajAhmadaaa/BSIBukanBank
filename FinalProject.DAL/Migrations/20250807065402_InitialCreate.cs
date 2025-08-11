using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinalProject.DAL.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Car",
                columns: table => new
                {
                    CarID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Model = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false),
                    CarType = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    BasePrice = table.Column<decimal>(type: "money", nullable: false),
                    Year = table.Column<int>(type: "int", nullable: true),
                    Color = table.Column<string>(type: "varchar(30)", unicode: false, maxLength: 30, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Car__68A0340EF1B98578", x => x.CarID);
                });

            migrationBuilder.CreateTable(
                name: "Customer",
                columns: table => new
                {
                    CustomerID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false),
                    PhoneNumber = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: true),
                    Email = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    Address = table.Column<string>(type: "varchar(200)", unicode: false, maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Customer__A4AE64B8900658C3", x => x.CustomerID);
                });

            migrationBuilder.CreateTable(
                name: "Dealer",
                columns: table => new
                {
                    DealerID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false),
                    City = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    Address = table.Column<string>(type: "varchar(200)", unicode: false, maxLength: 200, nullable: true),
                    PhoneNumber = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Dealer__CA2F8E9220633F84", x => x.DealerID);
                });

            migrationBuilder.CreateTable(
                name: "LeasingCompany",
                columns: table => new
                {
                    LeasingCompanyID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false),
                    Address = table.Column<string>(type: "varchar(200)", unicode: false, maxLength: 200, nullable: true),
                    PhoneNumber = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__LeasingC__FC92E9E029975E80", x => x.LeasingCompanyID);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DealerInventory",
                columns: table => new
                {
                    DealerInventoryID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DealerID = table.Column<int>(type: "int", nullable: false),
                    CarID = table.Column<int>(type: "int", nullable: false),
                    Stock = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<decimal>(type: "money", nullable: false),
                    DiscountPercent = table.Column<double>(type: "float", nullable: false),
                    FeePercent = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__DealerIn__1499F8869311C33A", x => x.DealerInventoryID);
                    table.ForeignKey(
                        name: "FK__DealerInv__CarID__0C85DE4D",
                        column: x => x.CarID,
                        principalTable: "Car",
                        principalColumn: "CarID");
                    table.ForeignKey(
                        name: "FK__DealerInv__Deale__0B91BA14",
                        column: x => x.DealerID,
                        principalTable: "Dealer",
                        principalColumn: "DealerID");
                });

            migrationBuilder.CreateTable(
                name: "InventoryTransfer",
                columns: table => new
                {
                    InventoryTransferID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FromDealerID = table.Column<int>(type: "int", nullable: false),
                    ToDealerID = table.Column<int>(type: "int", nullable: false),
                    CarID = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    MutationDate = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Inventor__3E82BDBFF33DF1EC", x => x.InventoryTransferID);
                    table.ForeignKey(
                        name: "FK__Inventory__CarID__114A936A",
                        column: x => x.CarID,
                        principalTable: "Car",
                        principalColumn: "CarID");
                    table.ForeignKey(
                        name: "FK__Inventory__FromD__0F624AF8",
                        column: x => x.FromDealerID,
                        principalTable: "Dealer",
                        principalColumn: "DealerID");
                    table.ForeignKey(
                        name: "FK__Inventory__ToDea__10566F31",
                        column: x => x.ToDealerID,
                        principalTable: "Dealer",
                        principalColumn: "DealerID");
                });

            migrationBuilder.CreateTable(
                name: "SalesPerson",
                columns: table => new
                {
                    SalesPersonID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DealerID = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__SalesPer__7A591C184AF9C120", x => x.SalesPersonID);
                    table.ForeignKey(
                        name: "FK__SalesPers__Deale__3D5E1FD2",
                        column: x => x.DealerID,
                        principalTable: "Dealer",
                        principalColumn: "DealerID");
                });

            migrationBuilder.CreateTable(
                name: "ConsultHistory",
                columns: table => new
                {
                    ConsultHistoryID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DealerID = table.Column<int>(type: "int", nullable: false),
                    CustomerID = table.Column<int>(type: "int", nullable: false),
                    SalesPersonID = table.Column<int>(type: "int", nullable: false),
                    CarID = table.Column<int>(type: "int", nullable: true),
                    Budget = table.Column<decimal>(type: "money", nullable: true),
                    PaymentMethod = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: true),
                    ConsultationDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    Note = table.Column<string>(type: "varchar(200)", unicode: false, maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__ConsultH__C5DB29E8D26D8647", x => x.ConsultHistoryID);
                    table.ForeignKey(
                        name: "FK__ConsultHi__CarID__44FF419A",
                        column: x => x.CarID,
                        principalTable: "Car",
                        principalColumn: "CarID");
                    table.ForeignKey(
                        name: "FK__ConsultHi__Custo__4316F928",
                        column: x => x.CustomerID,
                        principalTable: "Customer",
                        principalColumn: "CustomerID");
                    table.ForeignKey(
                        name: "FK__ConsultHi__Deale__4222D4EF",
                        column: x => x.DealerID,
                        principalTable: "Dealer",
                        principalColumn: "DealerID");
                    table.ForeignKey(
                        name: "FK__ConsultHi__Sales__440B1D61",
                        column: x => x.SalesPersonID,
                        principalTable: "SalesPerson",
                        principalColumn: "SalesPersonID");
                });

            migrationBuilder.CreateTable(
                name: "TestDrive",
                columns: table => new
                {
                    TestDriveID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DealerID = table.Column<int>(type: "int", nullable: false),
                    CustomerID = table.Column<int>(type: "int", nullable: false),
                    SalesPersonID = table.Column<int>(type: "int", nullable: false),
                    CarID = table.Column<int>(type: "int", nullable: false),
                    ConsultHistoryID = table.Column<int>(type: "int", nullable: true),
                    TestDriveDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    Note = table.Column<string>(type: "varchar(200)", unicode: false, maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__TestDriv__BF98EF7276B02B41", x => x.TestDriveID);
                    table.ForeignKey(
                        name: "FK__TestDrive__CarID__4AB81AF0",
                        column: x => x.CarID,
                        principalTable: "Car",
                        principalColumn: "CarID");
                    table.ForeignKey(
                        name: "FK__TestDrive__Consu__4BAC3F29",
                        column: x => x.ConsultHistoryID,
                        principalTable: "ConsultHistory",
                        principalColumn: "ConsultHistoryID");
                    table.ForeignKey(
                        name: "FK__TestDrive__Custo__48CFD27E",
                        column: x => x.CustomerID,
                        principalTable: "Customer",
                        principalColumn: "CustomerID");
                    table.ForeignKey(
                        name: "FK__TestDrive__Deale__47DBAE45",
                        column: x => x.DealerID,
                        principalTable: "Dealer",
                        principalColumn: "DealerID");
                    table.ForeignKey(
                        name: "FK__TestDrive__Sales__49C3F6B7",
                        column: x => x.SalesPersonID,
                        principalTable: "SalesPerson",
                        principalColumn: "SalesPersonID");
                });

            migrationBuilder.CreateTable(
                name: "LetterOfIntent",
                columns: table => new
                {
                    LOIID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DealerID = table.Column<int>(type: "int", nullable: false),
                    CustomerID = table.Column<int>(type: "int", nullable: false),
                    SalesPersonID = table.Column<int>(type: "int", nullable: false),
                    ConsultHistoryID = table.Column<int>(type: "int", nullable: true),
                    TestDriveID = table.Column<int>(type: "int", nullable: true),
                    LOIDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    PaymentMethod = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: true),
                    Note = table.Column<string>(type: "varchar(200)", unicode: false, maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__LetterOf__E21E1B6CE8268F30", x => x.LOIID);
                    table.ForeignKey(
                        name: "FK__LetterOfI__Consu__5165187F",
                        column: x => x.ConsultHistoryID,
                        principalTable: "ConsultHistory",
                        principalColumn: "ConsultHistoryID");
                    table.ForeignKey(
                        name: "FK__LetterOfI__Custo__4F7CD00D",
                        column: x => x.CustomerID,
                        principalTable: "Customer",
                        principalColumn: "CustomerID");
                    table.ForeignKey(
                        name: "FK__LetterOfI__Deale__4E88ABD4",
                        column: x => x.DealerID,
                        principalTable: "Dealer",
                        principalColumn: "DealerID");
                    table.ForeignKey(
                        name: "FK__LetterOfI__Sales__5070F446",
                        column: x => x.SalesPersonID,
                        principalTable: "SalesPerson",
                        principalColumn: "SalesPersonID");
                    table.ForeignKey(
                        name: "FK__LetterOfI__TestD__52593CB8",
                        column: x => x.TestDriveID,
                        principalTable: "TestDrive",
                        principalColumn: "TestDriveID");
                });

            migrationBuilder.CreateTable(
                name: "Booking",
                columns: table => new
                {
                    BookingID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LOIID = table.Column<int>(type: "int", nullable: false),
                    BookingFee = table.Column<decimal>(type: "money", nullable: false),
                    BookingDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    Status = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Booking__73951ACD9BAC6C83", x => x.BookingID);
                    table.ForeignKey(
                        name: "FK__Booking__LOIID__59063A47",
                        column: x => x.LOIID,
                        principalTable: "LetterOfIntent",
                        principalColumn: "LOIID");
                });

            migrationBuilder.CreateTable(
                name: "CreditApplication",
                columns: table => new
                {
                    CreditAppID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LOIID = table.Column<int>(type: "int", nullable: false),
                    LeasingCompanyID = table.Column<int>(type: "int", nullable: true),
                    ApplicationDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    Status = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__CreditAp__5F45706C3C1AEBF0", x => x.CreditAppID);
                    table.ForeignKey(
                        name: "FK__CreditApp__LOIID__5DCAEF64",
                        column: x => x.LOIID,
                        principalTable: "LetterOfIntent",
                        principalColumn: "LOIID");
                    table.ForeignKey(
                        name: "FK__CreditApp__Leasi__5EBF139D",
                        column: x => x.LeasingCompanyID,
                        principalTable: "LeasingCompany",
                        principalColumn: "LeasingCompanyID");
                });

            migrationBuilder.CreateTable(
                name: "LetterOfIntentDetail",
                columns: table => new
                {
                    LOIDetailID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LOIID = table.Column<int>(type: "int", nullable: false),
                    CarID = table.Column<int>(type: "int", nullable: false),
                    AgreedPrice = table.Column<decimal>(type: "money", nullable: false),
                    Discount = table.Column<decimal>(type: "money", nullable: true),
                    DownPayment = table.Column<decimal>(type: "money", nullable: true),
                    Note = table.Column<string>(type: "varchar(200)", unicode: false, maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__LetterOf__194560EF4C6A8859", x => x.LOIDetailID);
                    table.ForeignKey(
                        name: "FK__LetterOfI__CarID__5629CD9C",
                        column: x => x.CarID,
                        principalTable: "Car",
                        principalColumn: "CarID");
                    table.ForeignKey(
                        name: "FK__LetterOfI__LOIID__5535A963",
                        column: x => x.LOIID,
                        principalTable: "LetterOfIntent",
                        principalColumn: "LOIID");
                });

            migrationBuilder.CreateTable(
                name: "SalesAgreement",
                columns: table => new
                {
                    SalesAgreementID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DealerID = table.Column<int>(type: "int", nullable: false),
                    CustomerID = table.Column<int>(type: "int", nullable: false),
                    SalesPersonID = table.Column<int>(type: "int", nullable: false),
                    LOIID = table.Column<int>(type: "int", nullable: true),
                    TransactionDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    TotalAmount = table.Column<decimal>(type: "money", nullable: true),
                    Status = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__SalesAgr__4E5E148F0815B176", x => x.SalesAgreementID);
                    table.ForeignKey(
                        name: "FK__SalesAgre__Custo__656C112C",
                        column: x => x.CustomerID,
                        principalTable: "Customer",
                        principalColumn: "CustomerID");
                    table.ForeignKey(
                        name: "FK__SalesAgre__Deale__6477ECF3",
                        column: x => x.DealerID,
                        principalTable: "Dealer",
                        principalColumn: "DealerID");
                    table.ForeignKey(
                        name: "FK__SalesAgre__LOIID__6754599E",
                        column: x => x.LOIID,
                        principalTable: "LetterOfIntent",
                        principalColumn: "LOIID");
                    table.ForeignKey(
                        name: "FK__SalesAgre__Sales__66603565",
                        column: x => x.SalesPersonID,
                        principalTable: "SalesPerson",
                        principalColumn: "SalesPersonID");
                });

            migrationBuilder.CreateTable(
                name: "CreditDocument",
                columns: table => new
                {
                    CreditDocumentID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreditAppID = table.Column<int>(type: "int", nullable: false),
                    DocumentType = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    DocumentPath = table.Column<string>(type: "varchar(200)", unicode: false, maxLength: 200, nullable: true),
                    UploadDate = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__CreditDo__52C1BB22B456C3F6", x => x.CreditDocumentID);
                    table.ForeignKey(
                        name: "FK__CreditDoc__Credi__619B8048",
                        column: x => x.CreditAppID,
                        principalTable: "CreditApplication",
                        principalColumn: "CreditAppID");
                });

            migrationBuilder.CreateTable(
                name: "CarDelivery",
                columns: table => new
                {
                    CarDeliveryID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SalesAgreementID = table.Column<int>(type: "int", nullable: false),
                    DeliveryDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    Status = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__CarDeliv__4A7A87321887DBFB", x => x.CarDeliveryID);
                    table.ForeignKey(
                        name: "FK__CarDelive__Sales__75A278F5",
                        column: x => x.SalesAgreementID,
                        principalTable: "SalesAgreement",
                        principalColumn: "SalesAgreementID");
                });

            migrationBuilder.CreateTable(
                name: "CustomerComplaint",
                columns: table => new
                {
                    CustomerComplaintID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerID = table.Column<int>(type: "int", nullable: false),
                    SalesAgreementID = table.Column<int>(type: "int", nullable: true),
                    ComplaintDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    Description = table.Column<string>(type: "varchar(200)", unicode: false, maxLength: 200, nullable: true),
                    Status = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Customer__ED804C331E897F38", x => x.CustomerComplaintID);
                    table.ForeignKey(
                        name: "FK__CustomerC__Custo__01142BA1",
                        column: x => x.CustomerID,
                        principalTable: "Customer",
                        principalColumn: "CustomerID");
                    table.ForeignKey(
                        name: "FK__CustomerC__Sales__02084FDA",
                        column: x => x.SalesAgreementID,
                        principalTable: "SalesAgreement",
                        principalColumn: "SalesAgreementID");
                });

            migrationBuilder.CreateTable(
                name: "CustomerFeedback",
                columns: table => new
                {
                    CustomerFeedbackID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerID = table.Column<int>(type: "int", nullable: false),
                    SalesAgreementID = table.Column<int>(type: "int", nullable: true),
                    FeedbackDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    Rating = table.Column<int>(type: "int", nullable: true),
                    Comment = table.Column<string>(type: "varchar(200)", unicode: false, maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Customer__88136B1135C655C0", x => x.CustomerFeedbackID);
                    table.ForeignKey(
                        name: "FK__CustomerF__Custo__14270015",
                        column: x => x.CustomerID,
                        principalTable: "Customer",
                        principalColumn: "CustomerID");
                    table.ForeignKey(
                        name: "FK__CustomerF__Sales__151B244E",
                        column: x => x.SalesAgreementID,
                        principalTable: "SalesAgreement",
                        principalColumn: "SalesAgreementID");
                });

            migrationBuilder.CreateTable(
                name: "PaymentHistory",
                columns: table => new
                {
                    PaymentHistoryID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SalesAgreementID = table.Column<int>(type: "int", nullable: true),
                    CreditAppID = table.Column<int>(type: "int", nullable: true),
                    PaymentAmount = table.Column<decimal>(type: "money", nullable: false),
                    PaymentDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    PaymentType = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__PaymentH__F3B93391347E4128", x => x.PaymentHistoryID);
                    table.ForeignKey(
                        name: "FK__PaymentHi__Credi__6FE99F9F",
                        column: x => x.CreditAppID,
                        principalTable: "CreditApplication",
                        principalColumn: "CreditAppID");
                    table.ForeignKey(
                        name: "FK__PaymentHi__Sales__6EF57B66",
                        column: x => x.SalesAgreementID,
                        principalTable: "SalesAgreement",
                        principalColumn: "SalesAgreementID");
                });

            migrationBuilder.CreateTable(
                name: "SalesAgreementDetail",
                columns: table => new
                {
                    SalesAgreementDetailID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SalesAgreementID = table.Column<int>(type: "int", nullable: false),
                    LOIDetailID = table.Column<int>(type: "int", nullable: true),
                    CarID = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<decimal>(type: "money", nullable: false),
                    Discount = table.Column<decimal>(type: "money", nullable: true),
                    Note = table.Column<string>(type: "varchar(200)", unicode: false, maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__SalesAgr__E9B6E9DD6495A820", x => x.SalesAgreementDetailID);
                    table.ForeignKey(
                        name: "FK__SalesAgre__CarID__6C190EBB",
                        column: x => x.CarID,
                        principalTable: "Car",
                        principalColumn: "CarID");
                    table.ForeignKey(
                        name: "FK__SalesAgre__LOIDe__6B24EA82",
                        column: x => x.LOIDetailID,
                        principalTable: "LetterOfIntentDetail",
                        principalColumn: "LOIDetailID");
                    table.ForeignKey(
                        name: "FK__SalesAgre__Sales__6A30C649",
                        column: x => x.SalesAgreementID,
                        principalTable: "SalesAgreement",
                        principalColumn: "SalesAgreementID");
                });

            migrationBuilder.CreateTable(
                name: "CarDeliverySchedule",
                columns: table => new
                {
                    CarDeliveryScheduleID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CarDeliveryID = table.Column<int>(type: "int", nullable: false),
                    ScheduledDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    Note = table.Column<string>(type: "varchar(200)", unicode: false, maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__CarDeliv__7CE2033C585BEF82", x => x.CarDeliveryScheduleID);
                    table.ForeignKey(
                        name: "FK__CarDelive__CarDe__787EE5A0",
                        column: x => x.CarDeliveryID,
                        principalTable: "CarDelivery",
                        principalColumn: "CarDeliveryID");
                });

            migrationBuilder.CreateTable(
                name: "PreDeliveryInspection",
                columns: table => new
                {
                    PreDeliveryInspectionID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CarDeliveryID = table.Column<int>(type: "int", nullable: false),
                    InspectionDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    InspectorName = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    Note = table.Column<string>(type: "varchar(200)", unicode: false, maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__PreDeliv__7E3BE4289EF0E92C", x => x.PreDeliveryInspectionID);
                    table.ForeignKey(
                        name: "FK__PreDelive__CarDe__7B5B524B",
                        column: x => x.CarDeliveryID,
                        principalTable: "CarDelivery",
                        principalColumn: "CarDeliveryID");
                });

            migrationBuilder.CreateTable(
                name: "ServiceHistory",
                columns: table => new
                {
                    ServiceID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SalesAgreementDetailID = table.Column<int>(type: "int", nullable: false),
                    ServiceDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    ServiceType = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    Note = table.Column<string>(type: "varchar(200)", unicode: false, maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__ServiceH__C51BB0EA2DB4198F", x => x.ServiceID);
                    table.ForeignKey(
                        name: "FK__ServiceHi__Sales__7E37BEF6",
                        column: x => x.SalesAgreementDetailID,
                        principalTable: "SalesAgreementDetail",
                        principalColumn: "SalesAgreementDetailID");
                });

            migrationBuilder.CreateTable(
                name: "VehicleRegistration",
                columns: table => new
                {
                    VehicleRegistrationID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SalesAgreementDetailID = table.Column<int>(type: "int", nullable: false),
                    RegistrationNumber = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: true),
                    OwnershipBookNumber = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: true),
                    TaxStatus = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: true),
                    InsuranceStatus = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__VehicleR__4256324830EE9972", x => x.VehicleRegistrationID);
                    table.ForeignKey(
                        name: "FK__VehicleRe__Sales__72C60C4A",
                        column: x => x.SalesAgreementDetailID,
                        principalTable: "SalesAgreementDetail",
                        principalColumn: "SalesAgreementDetailID");
                });

            migrationBuilder.CreateTable(
                name: "WarrantyClaim",
                columns: table => new
                {
                    WarrantyClaimID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerID = table.Column<int>(type: "int", nullable: false),
                    SalesAgreementDetailID = table.Column<int>(type: "int", nullable: false),
                    ClaimDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    Description = table.Column<string>(type: "varchar(200)", unicode: false, maxLength: 200, nullable: true),
                    Status = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Warranty__F216750B97BF12D5", x => x.WarrantyClaimID);
                    table.ForeignKey(
                        name: "FK__WarrantyC__Custo__04E4BC85",
                        column: x => x.CustomerID,
                        principalTable: "Customer",
                        principalColumn: "CustomerID");
                    table.ForeignKey(
                        name: "FK__WarrantyC__Sales__05D8E0BE",
                        column: x => x.SalesAgreementDetailID,
                        principalTable: "SalesAgreementDetail",
                        principalColumn: "SalesAgreementDetailID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

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
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Booking_LOIID",
                table: "Booking",
                column: "LOIID");

            migrationBuilder.CreateIndex(
                name: "IX_CarDelivery_SalesAgreementID",
                table: "CarDelivery",
                column: "SalesAgreementID");

            migrationBuilder.CreateIndex(
                name: "IX_CarDeliverySchedule_CarDeliveryID",
                table: "CarDeliverySchedule",
                column: "CarDeliveryID");

            migrationBuilder.CreateIndex(
                name: "IX_ConsultHistory_CarID",
                table: "ConsultHistory",
                column: "CarID");

            migrationBuilder.CreateIndex(
                name: "IX_ConsultHistory_CustomerID",
                table: "ConsultHistory",
                column: "CustomerID");

            migrationBuilder.CreateIndex(
                name: "IX_ConsultHistory_DealerID",
                table: "ConsultHistory",
                column: "DealerID");

            migrationBuilder.CreateIndex(
                name: "IX_ConsultHistory_SalesPersonID",
                table: "ConsultHistory",
                column: "SalesPersonID");

            migrationBuilder.CreateIndex(
                name: "IX_CreditApplication_LeasingCompanyID",
                table: "CreditApplication",
                column: "LeasingCompanyID");

            migrationBuilder.CreateIndex(
                name: "IX_CreditApplication_LOIID",
                table: "CreditApplication",
                column: "LOIID");

            migrationBuilder.CreateIndex(
                name: "IX_CreditDocument_CreditAppID",
                table: "CreditDocument",
                column: "CreditAppID");

            migrationBuilder.CreateIndex(
                name: "UQ__Customer__A9D10534904204F6",
                table: "Customer",
                column: "Email",
                unique: true,
                filter: "[Email] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerComplaint_CustomerID",
                table: "CustomerComplaint",
                column: "CustomerID");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerComplaint_SalesAgreementID",
                table: "CustomerComplaint",
                column: "SalesAgreementID");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerFeedback_CustomerID",
                table: "CustomerFeedback",
                column: "CustomerID");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerFeedback_SalesAgreementID",
                table: "CustomerFeedback",
                column: "SalesAgreementID");

            migrationBuilder.CreateIndex(
                name: "IX_DealerInventory_CarID",
                table: "DealerInventory",
                column: "CarID");

            migrationBuilder.CreateIndex(
                name: "UQ_DealerInventory_DealerCar",
                table: "DealerInventory",
                columns: new[] { "DealerID", "CarID" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_InventoryTransfer_CarID",
                table: "InventoryTransfer",
                column: "CarID");

            migrationBuilder.CreateIndex(
                name: "IX_InventoryTransfer_FromDealerID",
                table: "InventoryTransfer",
                column: "FromDealerID");

            migrationBuilder.CreateIndex(
                name: "IX_InventoryTransfer_ToDealerID",
                table: "InventoryTransfer",
                column: "ToDealerID");

            migrationBuilder.CreateIndex(
                name: "IX_LetterOfIntent_ConsultHistoryID",
                table: "LetterOfIntent",
                column: "ConsultHistoryID");

            migrationBuilder.CreateIndex(
                name: "IX_LetterOfIntent_CustomerID",
                table: "LetterOfIntent",
                column: "CustomerID");

            migrationBuilder.CreateIndex(
                name: "IX_LetterOfIntent_DealerID",
                table: "LetterOfIntent",
                column: "DealerID");

            migrationBuilder.CreateIndex(
                name: "IX_LetterOfIntent_SalesPersonID",
                table: "LetterOfIntent",
                column: "SalesPersonID");

            migrationBuilder.CreateIndex(
                name: "IX_LetterOfIntent_TestDriveID",
                table: "LetterOfIntent",
                column: "TestDriveID");

            migrationBuilder.CreateIndex(
                name: "IX_LetterOfIntentDetail_CarID",
                table: "LetterOfIntentDetail",
                column: "CarID");

            migrationBuilder.CreateIndex(
                name: "IX_LetterOfIntentDetail_LOIID",
                table: "LetterOfIntentDetail",
                column: "LOIID");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentHistory_CreditAppID",
                table: "PaymentHistory",
                column: "CreditAppID");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentHistory_SalesAgreementID",
                table: "PaymentHistory",
                column: "SalesAgreementID");

            migrationBuilder.CreateIndex(
                name: "IX_PreDeliveryInspection_CarDeliveryID",
                table: "PreDeliveryInspection",
                column: "CarDeliveryID");

            migrationBuilder.CreateIndex(
                name: "IX_SalesAgreement_CustomerID",
                table: "SalesAgreement",
                column: "CustomerID");

            migrationBuilder.CreateIndex(
                name: "IX_SalesAgreement_DealerID",
                table: "SalesAgreement",
                column: "DealerID");

            migrationBuilder.CreateIndex(
                name: "IX_SalesAgreement_LOIID",
                table: "SalesAgreement",
                column: "LOIID");

            migrationBuilder.CreateIndex(
                name: "IX_SalesAgreement_SalesPersonID",
                table: "SalesAgreement",
                column: "SalesPersonID");

            migrationBuilder.CreateIndex(
                name: "IX_SalesAgreementDetail_CarID",
                table: "SalesAgreementDetail",
                column: "CarID");

            migrationBuilder.CreateIndex(
                name: "IX_SalesAgreementDetail_LOIDetailID",
                table: "SalesAgreementDetail",
                column: "LOIDetailID");

            migrationBuilder.CreateIndex(
                name: "IX_SalesAgreementDetail_SalesAgreementID",
                table: "SalesAgreementDetail",
                column: "SalesAgreementID");

            migrationBuilder.CreateIndex(
                name: "IX_SalesPerson_DealerID",
                table: "SalesPerson",
                column: "DealerID");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceHistory_SalesAgreementDetailID",
                table: "ServiceHistory",
                column: "SalesAgreementDetailID");

            migrationBuilder.CreateIndex(
                name: "IX_TestDrive_CarID",
                table: "TestDrive",
                column: "CarID");

            migrationBuilder.CreateIndex(
                name: "IX_TestDrive_ConsultHistoryID",
                table: "TestDrive",
                column: "ConsultHistoryID");

            migrationBuilder.CreateIndex(
                name: "IX_TestDrive_CustomerID",
                table: "TestDrive",
                column: "CustomerID");

            migrationBuilder.CreateIndex(
                name: "IX_TestDrive_DealerID",
                table: "TestDrive",
                column: "DealerID");

            migrationBuilder.CreateIndex(
                name: "IX_TestDrive_SalesPersonID",
                table: "TestDrive",
                column: "SalesPersonID");

            migrationBuilder.CreateIndex(
                name: "IX_VehicleRegistration_SalesAgreementDetailID",
                table: "VehicleRegistration",
                column: "SalesAgreementDetailID");

            migrationBuilder.CreateIndex(
                name: "IX_WarrantyClaim_CustomerID",
                table: "WarrantyClaim",
                column: "CustomerID");

            migrationBuilder.CreateIndex(
                name: "IX_WarrantyClaim_SalesAgreementDetailID",
                table: "WarrantyClaim",
                column: "SalesAgreementDetailID");
        }

        /// <inheritdoc />
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
                name: "Booking");

            migrationBuilder.DropTable(
                name: "CarDeliverySchedule");

            migrationBuilder.DropTable(
                name: "CreditDocument");

            migrationBuilder.DropTable(
                name: "CustomerComplaint");

            migrationBuilder.DropTable(
                name: "CustomerFeedback");

            migrationBuilder.DropTable(
                name: "DealerInventory");

            migrationBuilder.DropTable(
                name: "InventoryTransfer");

            migrationBuilder.DropTable(
                name: "PaymentHistory");

            migrationBuilder.DropTable(
                name: "PreDeliveryInspection");

            migrationBuilder.DropTable(
                name: "ServiceHistory");

            migrationBuilder.DropTable(
                name: "VehicleRegistration");

            migrationBuilder.DropTable(
                name: "WarrantyClaim");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "CreditApplication");

            migrationBuilder.DropTable(
                name: "CarDelivery");

            migrationBuilder.DropTable(
                name: "SalesAgreementDetail");

            migrationBuilder.DropTable(
                name: "LeasingCompany");

            migrationBuilder.DropTable(
                name: "LetterOfIntentDetail");

            migrationBuilder.DropTable(
                name: "SalesAgreement");

            migrationBuilder.DropTable(
                name: "LetterOfIntent");

            migrationBuilder.DropTable(
                name: "TestDrive");

            migrationBuilder.DropTable(
                name: "ConsultHistory");

            migrationBuilder.DropTable(
                name: "Car");

            migrationBuilder.DropTable(
                name: "Customer");

            migrationBuilder.DropTable(
                name: "SalesPerson");

            migrationBuilder.DropTable(
                name: "Dealer");
        }
    }
}
