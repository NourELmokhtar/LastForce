using Microsoft.EntityFrameworkCore.Migrations;

namespace Forces.Infrastructure.Migrations
{
    public partial class EditVoteCodeLog : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "RequestId",
                table: "VoteCodeLog",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_VoteCodeLog_RequestId",
                table: "VoteCodeLog",
                column: "RequestId");

            migrationBuilder.AddForeignKey(
                name: "FK_VoteCodeLog_Requests_RequestId",
                table: "VoteCodeLog",
                column: "RequestId",
                principalTable: "Requests",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VoteCodeLog_Requests_RequestId",
                table: "VoteCodeLog");

            migrationBuilder.DropIndex(
                name: "IX_VoteCodeLog_RequestId",
                table: "VoteCodeLog");

            migrationBuilder.AlterColumn<int>(
                name: "RequestId",
                table: "VoteCodeLog",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);
        }
    }
}
