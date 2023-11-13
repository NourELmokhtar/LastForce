using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Forces.Infrastructure.Migrations
{
    public partial class AddPaiedColumns : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropForeignKey(
            //    name: "FK_MprRequestAction_MprRequests_MprRequestId",
            //    table: "MprRequestAction");

            //migrationBuilder.DropForeignKey(
            //    name: "FK_MprRequestAttachments_MprRequestAction_MprRequestActionId",
            //    table: "MprRequestAttachments");

            //migrationBuilder.DropForeignKey(
            //    name: "FK_MprRequestAttachments_MprRequests_MprRequestId",
            //    table: "MprRequestAttachments");

            //migrationBuilder.DropIndex(
            //    name: "IX_MprRequestAttachments_MprRequestActionId",
            //    table: "MprRequestAttachments");

            //migrationBuilder.DropIndex(
            //    name: "IX_MprRequestAttachments_MprRequestId",
            //    table: "MprRequestAttachments");

            //migrationBuilder.DropIndex(
            //    name: "IX_MprRequestAction_MprRequestId",
            //    table: "MprRequestAction");

            //migrationBuilder.DropColumn(
            //    name: "MprRequestActionId",
            //    table: "MprRequestAttachments");

            //migrationBuilder.DropColumn(
            //    name: "MprRequestId",
            //    table: "MprRequestAttachments");

            //migrationBuilder.DropColumn(
            //    name: "MprRequestId",
            //    table: "MprRequestAction");

            migrationBuilder.AddColumn<bool>(
                name: "ConfirmPaied",
                table: "MprRequests",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Paied",
                table: "MprRequests",
                type: "bit",
                nullable: true);

            //migrationBuilder.CreateIndex(
            //    name: "IX_MprRequestAttachments_ActionId",
            //    table: "MprRequestAttachments",
            //    column: "ActionId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_MprRequestAttachments_RequestID",
            //    table: "MprRequestAttachments",
            //    column: "RequestID");

            //migrationBuilder.CreateIndex(
            //    name: "IX_MprRequestAction_RequestId",
            //    table: "MprRequestAction",
            //    column: "RequestId");

            //migrationBuilder.AddForeignKey(
            //    name: "FK_MprRequestAction_MprRequests_RequestId",
            //    table: "MprRequestAction",
            //    column: "RequestId",
            //    principalTable: "MprRequests",
            //    principalColumn: "Id",
            //    onDelete: ReferentialAction.Cascade);

            //migrationBuilder.AddForeignKey(
            //    name: "FK_MprRequestAttachments_MprRequestAction_ActionId",
            //    table: "MprRequestAttachments",
            //    column: "ActionId",
            //    principalTable: "MprRequestAction",
            //    principalColumn: "Id");

            //migrationBuilder.AddForeignKey(
            //    name: "FK_MprRequestAttachments_MprRequests_RequestID",
            //    table: "MprRequestAttachments",
            //    column: "RequestID",
            //    principalTable: "MprRequests",
            //    principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MprRequestAction_MprRequests_RequestId",
                table: "MprRequestAction");

            migrationBuilder.DropForeignKey(
                name: "FK_MprRequestAttachments_MprRequestAction_ActionId",
                table: "MprRequestAttachments");

            migrationBuilder.DropForeignKey(
                name: "FK_MprRequestAttachments_MprRequests_RequestID",
                table: "MprRequestAttachments");

            migrationBuilder.DropIndex(
                name: "IX_MprRequestAttachments_ActionId",
                table: "MprRequestAttachments");

            migrationBuilder.DropIndex(
                name: "IX_MprRequestAttachments_RequestID",
                table: "MprRequestAttachments");

            migrationBuilder.DropIndex(
                name: "IX_MprRequestAction_RequestId",
                table: "MprRequestAction");

            migrationBuilder.DropColumn(
                name: "ConfirmPaied",
                table: "MprRequests");

            migrationBuilder.DropColumn(
                name: "Paied",
                table: "MprRequests");

            migrationBuilder.AddColumn<int>(
                name: "MprRequestActionId",
                table: "MprRequestAttachments",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MprRequestId",
                table: "MprRequestAttachments",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MprRequestId",
                table: "MprRequestAction",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_MprRequestAttachments_MprRequestActionId",
                table: "MprRequestAttachments",
                column: "MprRequestActionId");

            migrationBuilder.CreateIndex(
                name: "IX_MprRequestAttachments_MprRequestId",
                table: "MprRequestAttachments",
                column: "MprRequestId");

            migrationBuilder.CreateIndex(
                name: "IX_MprRequestAction_MprRequestId",
                table: "MprRequestAction",
                column: "MprRequestId");

            migrationBuilder.AddForeignKey(
                name: "FK_MprRequestAction_MprRequests_MprRequestId",
                table: "MprRequestAction",
                column: "MprRequestId",
                principalTable: "MprRequests",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_MprRequestAttachments_MprRequestAction_MprRequestActionId",
                table: "MprRequestAttachments",
                column: "MprRequestActionId",
                principalTable: "MprRequestAction",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_MprRequestAttachments_MprRequests_MprRequestId",
                table: "MprRequestAttachments",
                column: "MprRequestId",
                principalTable: "MprRequests",
                principalColumn: "Id");
        }
    }
}
