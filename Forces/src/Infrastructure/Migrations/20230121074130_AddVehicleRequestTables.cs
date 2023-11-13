using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Forces.Infrastructure.Migrations
{
    public partial class AddVehicleRequestTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "VehicleRequestUserType",
                table: "Bases",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "VehicleRequest",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RequestStep = table.Column<int>(type: "int", nullable: false),
                    RequestCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BookingDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ShipmentType = table.Column<int>(type: "int", nullable: false),
                    PassengersCount = table.Column<int>(type: "int", nullable: true),
                    PassengersGender = table.Column<int>(type: "int", nullable: true),
                    BaseId = table.Column<int>(type: "int", nullable: true),
                    ForceId = table.Column<int>(type: "int", nullable: true),
                    WithDriver = table.Column<bool>(type: "bit", nullable: false),
                    RequestNote = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    KindJob = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PublicRequest = table.Column<bool>(type: "bit", nullable: false),
                    IsDone = table.Column<bool>(type: "bit", nullable: false),
                    DriverId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DriverName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PickupBaseId = table.Column<int>(type: "int", nullable: false),
                    DropBaseId = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VehicleRequest", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VehicleRequest_Bases_BaseId",
                        column: x => x.BaseId,
                        principalTable: "Bases",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_VehicleRequest_Bases_DropBaseId",
                        column: x => x.DropBaseId,
                        principalTable: "Bases",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_VehicleRequest_Bases_PickupBaseId",
                        column: x => x.PickupBaseId,
                        principalTable: "Bases",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "VehicleRequestStopStation",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RequestId = table.Column<int>(type: "int", nullable: false),
                    BaseId = table.Column<int>(type: "int", nullable: false),
                    Order = table.Column<int>(type: "int", nullable: false),
                    StopNote = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VehicleRequestStopStation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VehicleRequestStopStation_Bases_BaseId",
                        column: x => x.BaseId,
                        principalTable: "Bases",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_VehicleRequestStopStation_VehicleRequest_RequestId",
                        column: x => x.RequestId,
                        principalTable: "VehicleRequest",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "VehicleRequestPackage",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Height = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Width = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Length = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Weight = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    RequestId = table.Column<int>(type: "int", nullable: false),
                    PickupLocation = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DropLocation = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PickupBaseId = table.Column<int>(type: "int", nullable: false),
                    DropBaseId = table.Column<int>(type: "int", nullable: false),
                    PackageNote = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AthurityCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VehicleRequestStopStationId = table.Column<int>(type: "int", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VehicleRequestPackage", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VehicleRequestPackage_Bases_DropBaseId",
                        column: x => x.DropBaseId,
                        principalTable: "Bases",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_VehicleRequestPackage_Bases_PickupBaseId",
                        column: x => x.PickupBaseId,
                        principalTable: "Bases",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_VehicleRequestPackage_VehicleRequest_RequestId",
                        column: x => x.RequestId,
                        principalTable: "VehicleRequest",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_VehicleRequestPackage_VehicleRequestStopStation_VehicleRequestStopStationId",
                        column: x => x.VehicleRequestStopStationId,
                        principalTable: "VehicleRequestStopStation",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_VehicleRequest_BaseId",
                table: "VehicleRequest",
                column: "BaseId");

            migrationBuilder.CreateIndex(
                name: "IX_VehicleRequest_DropBaseId",
                table: "VehicleRequest",
                column: "DropBaseId");

            migrationBuilder.CreateIndex(
                name: "IX_VehicleRequest_PickupBaseId",
                table: "VehicleRequest",
                column: "PickupBaseId");

            migrationBuilder.CreateIndex(
                name: "IX_VehicleRequestPackage_DropBaseId",
                table: "VehicleRequestPackage",
                column: "DropBaseId");

            migrationBuilder.CreateIndex(
                name: "IX_VehicleRequestPackage_PickupBaseId",
                table: "VehicleRequestPackage",
                column: "PickupBaseId");

            migrationBuilder.CreateIndex(
                name: "IX_VehicleRequestPackage_RequestId",
                table: "VehicleRequestPackage",
                column: "RequestId");

            migrationBuilder.CreateIndex(
                name: "IX_VehicleRequestPackage_VehicleRequestStopStationId",
                table: "VehicleRequestPackage",
                column: "VehicleRequestStopStationId");

            migrationBuilder.CreateIndex(
                name: "IX_VehicleRequestStopStation_BaseId",
                table: "VehicleRequestStopStation",
                column: "BaseId");

            migrationBuilder.CreateIndex(
                name: "IX_VehicleRequestStopStation_RequestId",
                table: "VehicleRequestStopStation",
                column: "RequestId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "VehicleRequestPackage");

            migrationBuilder.DropTable(
                name: "VehicleRequestStopStation");

            migrationBuilder.DropTable(
                name: "VehicleRequest");

            migrationBuilder.DropColumn(
                name: "VehicleRequestUserType",
                table: "Bases");
        }
    }
}
