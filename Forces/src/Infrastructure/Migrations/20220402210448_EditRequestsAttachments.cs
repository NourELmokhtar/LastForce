using Microsoft.EntityFrameworkCore.Migrations;

namespace Forces.Infrastructure.Migrations
{
    public partial class EditRequestsAttachments : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RequestAttachments_Requests_Requests<VoteCodes, Appuser, RequestActions>Id",
                table: "RequestAttachments");

            migrationBuilder.DropIndex(
                name: "IX_RequestAttachments_Requests<VoteCodes, Appuser, RequestActions>Id",
                table: "RequestAttachments");

            migrationBuilder.DropColumn(
                name: "Requests<VoteCodes, Appuser, RequestActions>Id",
                table: "RequestAttachments");

            migrationBuilder.AddColumn<string>(
                name: "Unit",
                table: "Requests",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_RequestAttachments_RequestID",
                table: "RequestAttachments",
                column: "RequestID");

            migrationBuilder.AddForeignKey(
                name: "FK_RequestAttachments_Requests_RequestID",
                table: "RequestAttachments",
                column: "RequestID",
                principalTable: "Requests",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RequestAttachments_Requests_RequestID",
                table: "RequestAttachments");

            migrationBuilder.DropIndex(
                name: "IX_RequestAttachments_RequestID",
                table: "RequestAttachments");

            migrationBuilder.DropColumn(
                name: "Unit",
                table: "Requests");

            migrationBuilder.AddColumn<int>(
                name: "Requests<VoteCodes, Appuser, RequestActions>Id",
                table: "RequestAttachments",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_RequestAttachments_Requests<VoteCodes, Appuser, RequestActions>Id",
                table: "RequestAttachments",
                column: "Requests<VoteCodes, Appuser, RequestActions>Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RequestAttachments_Requests_Requests<VoteCodes, Appuser, RequestActions>Id",
                table: "RequestAttachments",
                column: "Requests<VoteCodes, Appuser, RequestActions>Id",
                principalTable: "Requests",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
