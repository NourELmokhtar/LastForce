using Microsoft.EntityFrameworkCore.Migrations;

namespace Forces.Infrastructure.Migrations
{
    public partial class AddVehicleRequestState : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RequestState",
                table: "VehicleRequest",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RequestState",
                table: "VehicleRequest");
        }
    }
}
