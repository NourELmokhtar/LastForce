using Microsoft.EntityFrameworkCore.Migrations;

namespace Forces.Infrastructure.Migrations
{
    public partial class AddActionAttachment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_RequestAttachments_ActionId",
                table: "RequestAttachments",
                column: "ActionId");

            migrationBuilder.AddForeignKey(
                name: "FK_RequestAttachments_RequestActions_ActionId",
                table: "RequestAttachments",
                column: "ActionId",
                principalTable: "RequestActions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RequestAttachments_RequestActions_ActionId",
                table: "RequestAttachments");

            migrationBuilder.DropIndex(
                name: "IX_RequestAttachments_ActionId",
                table: "RequestAttachments");
        }
    }
}
