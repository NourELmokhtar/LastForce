using Microsoft.EntityFrameworkCore.Migrations;

namespace Forces.Infrastructure.Migrations
{
    public partial class EditNPR : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Requests_Items_ItemsId",
                table: "Requests");

            migrationBuilder.DropIndex(
                name: "IX_Requests_ItemsId",
                table: "Requests");

            migrationBuilder.DropColumn(
                name: "ItemsId",
                table: "Requests");

            migrationBuilder.AddColumn<string>(
                name: "ItemArName",
                table: "Requests",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ItemId",
                table: "Requests",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "ItemName",
                table: "Requests",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "ItemPrice",
                table: "Requests",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "ItemQty",
                table: "Requests",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Requests_ItemId",
                table: "Requests",
                column: "ItemId");

            migrationBuilder.AddForeignKey(
                name: "FK_Requests_Items_ItemId",
                table: "Requests",
                column: "ItemId",
                principalTable: "Items",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Requests_Items_ItemId",
                table: "Requests");

            migrationBuilder.DropIndex(
                name: "IX_Requests_ItemId",
                table: "Requests");

            migrationBuilder.DropColumn(
                name: "ItemArName",
                table: "Requests");

            migrationBuilder.DropColumn(
                name: "ItemId",
                table: "Requests");

            migrationBuilder.DropColumn(
                name: "ItemName",
                table: "Requests");

            migrationBuilder.DropColumn(
                name: "ItemPrice",
                table: "Requests");

            migrationBuilder.DropColumn(
                name: "ItemQty",
                table: "Requests");

            migrationBuilder.AddColumn<int>(
                name: "ItemsId",
                table: "Requests",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Requests_ItemsId",
                table: "Requests",
                column: "ItemsId");

            migrationBuilder.AddForeignKey(
                name: "FK_Requests_Items_ItemsId",
                table: "Requests",
                column: "ItemsId",
                principalTable: "Items",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
