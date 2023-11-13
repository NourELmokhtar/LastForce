using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Forces.Infrastructure.Migrations
{
    public partial class update3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MadeIn",
                table: "InventoryItem");

            migrationBuilder.DropColumn(
                name: "VoteCodesId",
                table: "InventoryItem");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "MadeIn",
                table: "InventoryItem",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "VoteCodesId",
                table: "InventoryItem",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
