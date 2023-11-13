using Microsoft.EntityFrameworkCore.Migrations;

namespace Forces.Infrastructure.Migrations
{
    public partial class EditVoteCodesLogstable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ItemCode",
                table: "VoteCodeLog",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ItemId",
                table: "VoteCodeLog",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ItemNSN",
                table: "VoteCodeLog",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ItemName",
                table: "VoteCodeLog",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RequestRefrance",
                table: "VoteCodeLog",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ItemCode",
                table: "VoteCodeLog");

            migrationBuilder.DropColumn(
                name: "ItemId",
                table: "VoteCodeLog");

            migrationBuilder.DropColumn(
                name: "ItemNSN",
                table: "VoteCodeLog");

            migrationBuilder.DropColumn(
                name: "ItemName",
                table: "VoteCodeLog");

            migrationBuilder.DropColumn(
                name: "RequestRefrance",
                table: "VoteCodeLog");
        }
    }
}
