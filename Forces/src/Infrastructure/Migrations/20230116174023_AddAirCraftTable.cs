using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Forces.Infrastructure.Migrations
{
    public partial class AddAirCraftTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AirType",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AirTypeName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AirTypeCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AirType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AirKind",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AirKindName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AirKindCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AirTypeId = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AirKind", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AirKind_AirType_AirTypeId",
                        column: x => x.AirTypeId,
                        principalTable: "AirType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AirCraft",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AirCraftCode = table.Column<int>(type: "int", nullable: false),
                    AirCraftName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BaseId = table.Column<int>(type: "int", nullable: false),
                    SectionId = table.Column<int>(type: "int", nullable: false),
                    DateOfEnter = table.Column<DateTime>(type: "datetime2", nullable: true),
                    MadeIN = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastServes = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Hours = table.Column<int>(type: "int", nullable: true),
                    ServesType = table.Column<int>(type: "int", nullable: false),
                    AirKindId = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AirCraft", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AirCraft_AirKind_AirKindId",
                        column: x => x.AirKindId,
                        principalTable: "AirKind",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AirCraft_Bases_BaseId",
                        column: x => x.BaseId,
                        principalTable: "Bases",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AirCraft_BasesSections_SectionId",
                        column: x => x.SectionId,
                        principalTable: "BasesSections",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AirCraft_AirKindId",
                table: "AirCraft",
                column: "AirKindId");

            migrationBuilder.CreateIndex(
                name: "IX_AirCraft_BaseId",
                table: "AirCraft",
                column: "BaseId");

            migrationBuilder.CreateIndex(
                name: "IX_AirCraft_SectionId",
                table: "AirCraft",
                column: "SectionId");

            migrationBuilder.CreateIndex(
                name: "IX_AirKind_AirTypeId",
                table: "AirKind",
                column: "AirTypeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AirCraft");

            migrationBuilder.DropTable(
                name: "AirKind");

            migrationBuilder.DropTable(
                name: "AirType");
        }
    }
}
