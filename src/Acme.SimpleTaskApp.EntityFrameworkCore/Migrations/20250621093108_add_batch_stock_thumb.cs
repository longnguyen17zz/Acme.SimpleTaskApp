using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Acme.SimpleTaskApp.Migrations
{
    /// <inheritdoc />
    public partial class add_batch_stock_thumb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "StockQuantity",
                table: "AppStocks",
                newName: "SellQuantity");

            migrationBuilder.RenameColumn(
                name: "LastUpdated",
                table: "AppStocks",
                newName: "DateManufacture");

            migrationBuilder.AddColumn<int>(
                name: "BatchId",
                table: "AppStocks",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<long>(
                name: "DeleterUserId",
                table: "AppStocks",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletionTime",
                table: "AppStocks",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "InitQuantity",
                table: "AppStocks",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "InputPrice",
                table: "AppStocks",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "AppStocks",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<long>(
                name: "DeleterUserId",
                table: "AppProducts",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletionTime",
                table: "AppProducts",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "AppProducts",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreationTime",
                table: "AppOrderItems",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<long>(
                name: "CreatorUserId",
                table: "AppOrderItems",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "DeleterUserId",
                table: "AppOrderItems",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletionTime",
                table: "AppOrderItems",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "AppOrderItems",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModificationTime",
                table: "AppOrderItems",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "LastModifierUserId",
                table: "AppOrderItems",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "AppBatches",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DateEntry = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Importer = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierUserId = table.Column<long>(type: "bigint", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeleterUserId = table.Column<long>(type: "bigint", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppBatches", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AppThumbs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    Image = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierUserId = table.Column<long>(type: "bigint", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeleterUserId = table.Column<long>(type: "bigint", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppThumbs", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AppStocks_BatchId",
                table: "AppStocks",
                column: "BatchId");

            migrationBuilder.AddForeignKey(
                name: "FK_AppStocks_AppBatches_BatchId",
                table: "AppStocks",
                column: "BatchId",
                principalTable: "AppBatches",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppStocks_AppBatches_BatchId",
                table: "AppStocks");

            migrationBuilder.DropTable(
                name: "AppBatches");

            migrationBuilder.DropTable(
                name: "AppThumbs");

            migrationBuilder.DropIndex(
                name: "IX_AppStocks_BatchId",
                table: "AppStocks");

            migrationBuilder.DropColumn(
                name: "BatchId",
                table: "AppStocks");

            migrationBuilder.DropColumn(
                name: "DeleterUserId",
                table: "AppStocks");

            migrationBuilder.DropColumn(
                name: "DeletionTime",
                table: "AppStocks");

            migrationBuilder.DropColumn(
                name: "InitQuantity",
                table: "AppStocks");

            migrationBuilder.DropColumn(
                name: "InputPrice",
                table: "AppStocks");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "AppStocks");

            migrationBuilder.DropColumn(
                name: "DeleterUserId",
                table: "AppProducts");

            migrationBuilder.DropColumn(
                name: "DeletionTime",
                table: "AppProducts");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "AppProducts");

            migrationBuilder.DropColumn(
                name: "CreationTime",
                table: "AppOrderItems");

            migrationBuilder.DropColumn(
                name: "CreatorUserId",
                table: "AppOrderItems");

            migrationBuilder.DropColumn(
                name: "DeleterUserId",
                table: "AppOrderItems");

            migrationBuilder.DropColumn(
                name: "DeletionTime",
                table: "AppOrderItems");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "AppOrderItems");

            migrationBuilder.DropColumn(
                name: "LastModificationTime",
                table: "AppOrderItems");

            migrationBuilder.DropColumn(
                name: "LastModifierUserId",
                table: "AppOrderItems");

            migrationBuilder.RenameColumn(
                name: "SellQuantity",
                table: "AppStocks",
                newName: "StockQuantity");

            migrationBuilder.RenameColumn(
                name: "DateManufacture",
                table: "AppStocks",
                newName: "LastUpdated");
        }
    }
}
