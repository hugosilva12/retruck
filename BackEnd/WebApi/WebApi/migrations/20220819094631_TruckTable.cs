using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApplication1.Migrations
{
    public partial class TruckTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Truck",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    driverid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    matricula = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    year = table.Column<int>(type: "int", nullable: false),
                    truckCategory = table.Column<int>(type: "int", nullable: false),
                    fuelConsumption = table.Column<int>(type: "int", nullable: false),
                    kms = table.Column<int>(type: "int", nullable: false),
                    nextRevision = table.Column<int>(type: "int", nullable: false),
                    photoPath = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    organization_id = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Truck", x => x.id);
                    table.ForeignKey(
                        name: "FK_Truck_User_driverid",
                        column: x => x.driverid,
                        principalTable: "User",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Truck_driverid",
                table: "Truck",
                column: "driverid");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Truck");
        }
    }
}
