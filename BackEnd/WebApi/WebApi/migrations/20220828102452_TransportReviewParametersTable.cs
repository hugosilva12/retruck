using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApplication1.Migrations
{
    public partial class TransportReviewParametersTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TransportReviewParameters",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    valueSaturday = table.Column<double>(type: "float", nullable: false),
                    valueSunday = table.Column<double>(type: "float", nullable: false),
                    valueHoliday = table.Column<double>(type: "float", nullable: false),
                    valueFuel = table.Column<double>(type: "float", nullable: false),
                    valueToll = table.Column<double>(type: "float", nullable: false),
                    typeAnalysis = table.Column<int>(type: "int", nullable: false),
                    considerTruckBreakDowns = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransportReviewParameters", x => x.id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TransportReviewParameters");
        }
    }
}
