using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Acme.SimpleTaskApp.Migrations
{
    /// <inheritdoc />
    public partial class fix_stock : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "AppStocks");

            migrationBuilder.AddColumn<int>(
                name: "StockQuantity",
                table: "AppStocks",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StockQuantity",
                table: "AppStocks");

            migrationBuilder.AddColumn<int>(
                name: "Quantity",
                table: "AppStocks",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
