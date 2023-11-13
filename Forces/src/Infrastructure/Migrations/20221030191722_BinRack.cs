using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Forces.Infrastructure.Migrations
{
    public partial class BinRack : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RackStore",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RackCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RackName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StoreId = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RackStore", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RackStore_SectionStore_StoreId",
                        column: x => x.StoreId,
                        principalTable: "SectionStore",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ShilfRack",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ShilfCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ShilfName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RackId = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShilfRack", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ShilfRack_RackStore_RackId",
                        column: x => x.RackId,
                        principalTable: "RackStore",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BinRack",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BinCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BinName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ShilfId = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BinRack", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BinRack_ShilfRack_ShilfId",
                        column: x => x.ShilfId,
                        principalTable: "ShilfRack",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BinRack_ShilfId",
                table: "BinRack",
                column: "ShilfId");

            migrationBuilder.CreateIndex(
                name: "IX_RackStore_StoreId",
                table: "RackStore",
                column: "StoreId");

            migrationBuilder.CreateIndex(
                name: "IX_ShilfRack_RackId",
                table: "ShilfRack",
                column: "RackId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BinRack");

            migrationBuilder.DropTable(
                name: "ShilfRack");

            migrationBuilder.DropTable(
                name: "RackStore");
        }
    }
}
