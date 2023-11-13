using Microsoft.EntityFrameworkCore.Migrations;

namespace Forces.Infrastructure.Migrations
{
    public partial class AddTargetDepartMent : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TargetDepartId",
                table: "RequestActions",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TargetDepartMentType",
                table: "RequestActions",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TargetDepartId",
                table: "RequestActions");

            migrationBuilder.DropColumn(
                name: "TargetDepartMentType",
                table: "RequestActions");
        }
    }
}
