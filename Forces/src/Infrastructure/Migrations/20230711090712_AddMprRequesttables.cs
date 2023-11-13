using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Forces.Infrastructure.Migrations
{
    public partial class AddMprRequesttables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.AddColumn<bool>(
            //    name: "Selected",
            //    table: "RequestAttachments",
            //    type: "bit",
            //    nullable: true);

            migrationBuilder.CreateTable(
                name: "MprRequests",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RequestRefranceCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Priority = table.Column<int>(type: "int", nullable: false),
                    RequestState = table.Column<int>(type: "int", nullable: false),
                    VoteCodeId = table.Column<int>(type: "int", nullable: false),
                    ForceId = table.Column<int>(type: "int", nullable: true),
                    BaseId = table.Column<int>(type: "int", nullable: true),
                    SectionId = table.Column<int>(type: "int", nullable: true),
                    isDone = table.Column<bool>(type: "bit", nullable: false),
                    RequestNote = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CurrentStep = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MprRequests", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MprRequestAction",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RequestId = table.Column<int>(type: "int", nullable: false),
                    Step = table.Column<int>(type: "int", nullable: false),
                    ActionResult = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ActionNote = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Seen = table.Column<bool>(type: "bit", nullable: false),
                    Readed = table.Column<bool>(type: "bit", nullable: false),
                    TargetUserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ForceId = table.Column<int>(type: "int", nullable: true),
                    BaseId = table.Column<int>(type: "int", nullable: true),
                    SectionId = table.Column<int>(type: "int", nullable: true),
                    TargetDepartId = table.Column<int>(type: "int", nullable: true),
                    TargetDepartMentType = table.Column<int>(type: "int", nullable: true),
                    ActionState = table.Column<int>(type: "int", nullable: false),
                    ActionsType = table.Column<int>(type: "int", nullable: true),
                    MprRequestId = table.Column<int>(type: "int", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MprRequestAction", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MprRequestAction_MprRequests_MprRequestId",
                        column: x => x.MprRequestId,
                        principalTable: "MprRequests",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "RequestItem",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RequestId = table.Column<int>(type: "int", nullable: false),
                    ItemId = table.Column<int>(type: "int", nullable: false),
                    ItemPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Priority = table.Column<int>(type: "int", nullable: false),
                    ItemName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ItemArName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ItemQty = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ItemNSN = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ItemCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Unit = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ItemClass = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MprRequestId = table.Column<int>(type: "int", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RequestItem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RequestItem_Items_ItemId",
                        column: x => x.ItemId,
                        principalTable: "Items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RequestItem_MprRequests_MprRequestId",
                        column: x => x.MprRequestId,
                        principalTable: "MprRequests",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "MprRequestAttachments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FileUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RequestID = table.Column<int>(type: "int", nullable: true),
                    ActionId = table.Column<int>(type: "int", nullable: true),
                    AttachmentType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Selected = table.Column<bool>(type: "bit", nullable: true),
                    MprRequestActionId = table.Column<int>(type: "int", nullable: true),
                    MprRequestId = table.Column<int>(type: "int", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MprRequestAttachments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MprRequestAttachments_MprRequestAction_MprRequestActionId",
                        column: x => x.MprRequestActionId,
                        principalTable: "MprRequestAction",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_MprRequestAttachments_MprRequests_MprRequestId",
                        column: x => x.MprRequestId,
                        principalTable: "MprRequests",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_MprRequestAction_MprRequestId",
                table: "MprRequestAction",
                column: "MprRequestId");

            migrationBuilder.CreateIndex(
                name: "IX_MprRequestAttachments_MprRequestActionId",
                table: "MprRequestAttachments",
                column: "MprRequestActionId");

            migrationBuilder.CreateIndex(
                name: "IX_MprRequestAttachments_MprRequestId",
                table: "MprRequestAttachments",
                column: "MprRequestId");

            migrationBuilder.CreateIndex(
                name: "IX_RequestItem_ItemId",
                table: "RequestItem",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_RequestItem_MprRequestId",
                table: "RequestItem",
                column: "MprRequestId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MprRequestAttachments");

            migrationBuilder.DropTable(
                name: "RequestItem");

            migrationBuilder.DropTable(
                name: "MprRequestAction");

            migrationBuilder.DropTable(
                name: "MprRequests");

            migrationBuilder.DropColumn(
                name: "Selected",
                table: "RequestAttachments");
        }
    }
}
