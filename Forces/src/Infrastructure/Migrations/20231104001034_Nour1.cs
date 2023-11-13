using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Forces.Infrastructure.Migrations
{
    public partial class Nour1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Inventory_BaseSection_BaseSectionId",
                table: "Inventory");

            migrationBuilder.DropTable(
                name: "BaseSection");

            migrationBuilder.AddForeignKey(
                name: "FK_Inventory_BasesSections_BaseSectionId",
                table: "Inventory",
                column: "BaseSectionId",
                principalTable: "BasesSections",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Inventory_BasesSections_BaseSectionId",
                table: "Inventory");

            migrationBuilder.CreateTable(
                name: "BaseSection",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BaseSection", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Inventory_BaseSection_BaseSectionId",
                table: "Inventory",
                column: "BaseSectionId",
                principalTable: "BaseSection",
                principalColumn: "Id");
        }
    }
}
