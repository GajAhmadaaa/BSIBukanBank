using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinalProject.DAL.Migrations
{
    /// <inheritdoc />
    public partial class MakeSalesPersonIdNullableInLetterOfIntent : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "SalesPersonID",
                table: "LetterOfIntent",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "SalesPersonID",
                table: "LetterOfIntent",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);
        }
    }
}
