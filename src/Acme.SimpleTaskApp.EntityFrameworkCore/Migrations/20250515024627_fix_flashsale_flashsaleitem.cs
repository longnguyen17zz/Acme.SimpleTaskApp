using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Acme.SimpleTaskApp.Migrations
{
    /// <inheritdoc />
    public partial class fix_flashsale_flashsaleitem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "DeleterUserId",
                table: "AppFlashSales",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletionTime",
                table: "AppFlashSales",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "AppFlashSales",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<long>(
                name: "DeleterUserId",
                table: "AppFlashSaleItems",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletionTime",
                table: "AppFlashSaleItems",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "AppFlashSaleItems",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeleterUserId",
                table: "AppFlashSales");

            migrationBuilder.DropColumn(
                name: "DeletionTime",
                table: "AppFlashSales");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "AppFlashSales");

            migrationBuilder.DropColumn(
                name: "DeleterUserId",
                table: "AppFlashSaleItems");

            migrationBuilder.DropColumn(
                name: "DeletionTime",
                table: "AppFlashSaleItems");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "AppFlashSaleItems");
        }
    }
}
