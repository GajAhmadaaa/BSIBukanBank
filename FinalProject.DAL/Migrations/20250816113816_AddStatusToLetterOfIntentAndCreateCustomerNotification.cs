using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinalProject.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddStatusToLetterOfIntentAndCreateCustomerNotification : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "LetterOfIntent",
                type: "varchar(20)",
                unicode: false,
                maxLength: 20,
                nullable: true);

            migrationBuilder.CreateTable(
                name: "CustomerNotification",
                columns: table => new
                {
                    CustomerNotificationID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerID = table.Column<int>(type: "int", nullable: false),
                    LOIID = table.Column<int>(type: "int", nullable: true),
                    SalesAgreementID = table.Column<int>(type: "int", nullable: true),
                    NotificationType = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    Message = table.Column<string>(type: "varchar(500)", unicode: false, maxLength: 500, nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "GETDATE()"),
                    ReadDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    IsRead = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__CustomerNotification__0000000000000000", x => x.CustomerNotificationID);
                    table.ForeignKey(
                        name: "FK_CustomerNotification_Customer",
                        column: x => x.CustomerID,
                        principalTable: "Customer",
                        principalColumn: "CustomerID");
                    table.ForeignKey(
                        name: "FK_CustomerNotification_LetterOfIntent",
                        column: x => x.LOIID,
                        principalTable: "LetterOfIntent",
                        principalColumn: "LOIID");
                    table.ForeignKey(
                        name: "FK_CustomerNotification_SalesAgreement",
                        column: x => x.SalesAgreementID,
                        principalTable: "SalesAgreement",
                        principalColumn: "SalesAgreementID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_CustomerNotification_CustomerID",
                table: "CustomerNotification",
                column: "CustomerID");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerNotification_LOIID",
                table: "CustomerNotification",
                column: "LOIID");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerNotification_SalesAgreementID",
                table: "CustomerNotification",
                column: "SalesAgreementID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CustomerNotification");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "LetterOfIntent");
        }
    }
}
