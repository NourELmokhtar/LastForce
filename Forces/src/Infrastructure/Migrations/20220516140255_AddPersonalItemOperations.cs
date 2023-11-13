using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Forces.Infrastructure.Migrations
{
    public partial class AddPersonalItemOperations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PersonalItemsOperation_Hdr",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OperationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Total = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ForceId = table.Column<int>(type: "int", nullable: false),
                    BaseId = table.Column<int>(type: "int", nullable: true),
                    BaseSectionId = table.Column<int>(type: "int", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PersonalItemsOperation_Hdr", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PersonalItemsOperation_Details",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PersonalItemsOperation_HdrId = table.Column<int>(type: "int", nullable: false),
                    PersonalItemId = table.Column<int>(type: "int", nullable: false),
                    OperationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Qty = table.Column<int>(type: "int", nullable: false),
                    ItemPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalLinePrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PersonalItemsOperation_Details", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PersonalItemsOperation_Details_PersonalItems_PersonalItemId",
                        column: x => x.PersonalItemId,
                        principalTable: "PersonalItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PersonalItemsOperation_Details_PersonalItemsOperation_Hdr_PersonalItemsOperation_HdrId",
                        column: x => x.PersonalItemsOperation_HdrId,
                        principalTable: "PersonalItemsOperation_Hdr",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PersonalItemsOperation_Details_PersonalItemId",
                table: "PersonalItemsOperation_Details",
                column: "PersonalItemId");

            migrationBuilder.CreateIndex(
                name: "IX_PersonalItemsOperation_Details_PersonalItemsOperation_HdrId",
                table: "PersonalItemsOperation_Details",
                column: "PersonalItemsOperation_HdrId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PersonalItemsOperation_Details");

            migrationBuilder.DropTable(
                name: "PersonalItemsOperation_Hdr");
        }
    }
}
