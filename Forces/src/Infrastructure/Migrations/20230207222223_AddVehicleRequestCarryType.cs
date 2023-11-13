using Microsoft.EntityFrameworkCore.Migrations;

namespace Forces.Infrastructure.Migrations
{
    public partial class AddVehicleRequestCarryType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "VehicleCarryType",
                table: "Vehicle",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "VehicleCarryType",
                table: "Vehicle");
        }
    }
}
