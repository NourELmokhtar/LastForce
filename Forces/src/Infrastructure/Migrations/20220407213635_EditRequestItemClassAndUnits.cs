using Microsoft.EntityFrameworkCore.Migrations;

namespace Forces.Infrastructure.Migrations
{
    public partial class EditRequestItemClassAndUnits : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ItemClass",
                table: "Requests",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "TargetDepartMentType",
                table: "RequestActions",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ItemClass",
                table: "Requests");

            migrationBuilder.AlterColumn<int>(
                name: "TargetDepartMentType",
                table: "RequestActions",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);
        }
    }
}
