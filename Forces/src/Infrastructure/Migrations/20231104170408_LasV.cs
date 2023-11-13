using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Forces.Infrastructure.Migrations
{
    public partial class LasV : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "InventoryId",
                table: "InventoryItem",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_InventoryItem_InventoryId",
                table: "InventoryItem",
                column: "InventoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_InventoryItem_Inventory_InventoryId",
                table: "InventoryItem",
                column: "InventoryId",
                principalTable: "Inventory",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InventoryItem_Inventory_InventoryId",
                table: "InventoryItem");

            migrationBuilder.DropIndex(
                name: "IX_InventoryItem_InventoryId",
                table: "InventoryItem");

            migrationBuilder.DropColumn(
                name: "InventoryId",
                table: "InventoryItem");
        }
    }
}
