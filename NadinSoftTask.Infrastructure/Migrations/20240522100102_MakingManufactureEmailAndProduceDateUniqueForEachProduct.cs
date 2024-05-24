using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NadinSoftTask.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class MakingManufactureEmailAndProduceDateUniqueForEachProduct : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ManufactureEmail",
                table: "Products",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_Products_ManufactureEmail_ProduceDate",
                table: "Products",
                columns: new[] { "ManufactureEmail", "ProduceDate" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Products_ManufactureEmail_ProduceDate",
                table: "Products");

            migrationBuilder.AlterColumn<string>(
                name: "ManufactureEmail",
                table: "Products",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");
        }
    }
}
