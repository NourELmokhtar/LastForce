using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Forces.Infrastructure.Migrations
{
    public partial class AddTailersTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TailerId",
                table: "PersonalItemsOperation_Details",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TailerId",
                table: "PersonalItems",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Tailers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BaseId = table.Column<int>(type: "int", nullable: false),
                    TailerCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tailers", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PersonalItemsOperation_Details_TailerId",
                table: "PersonalItemsOperation_Details",
                column: "TailerId");

            migrationBuilder.CreateIndex(
                name: "IX_PersonalItems_TailerId",
                table: "PersonalItems",
                column: "TailerId");

            migrationBuilder.AddForeignKey(
                name: "FK_PersonalItems_Tailers_TailerId",
                table: "PersonalItems",
                column: "TailerId",
                principalTable: "Tailers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PersonalItemsOperation_Details_Tailers_TailerId",
                table: "PersonalItemsOperation_Details",
                column: "TailerId",
                principalTable: "Tailers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PersonalItems_Tailers_TailerId",
                table: "PersonalItems");

            migrationBuilder.DropForeignKey(
                name: "FK_PersonalItemsOperation_Details_Tailers_TailerId",
                table: "PersonalItemsOperation_Details");

            migrationBuilder.DropTable(
                name: "Tailers");

            migrationBuilder.DropIndex(
                name: "IX_PersonalItemsOperation_Details_TailerId",
                table: "PersonalItemsOperation_Details");

            migrationBuilder.DropIndex(
                name: "IX_PersonalItems_TailerId",
                table: "PersonalItems");

            migrationBuilder.DropColumn(
                name: "TailerId",
                table: "PersonalItemsOperation_Details");

            migrationBuilder.DropColumn(
                name: "TailerId",
                table: "PersonalItems");
        }
    }
}
