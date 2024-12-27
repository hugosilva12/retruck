using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApplication1.Migrations
{
    public partial class TransportTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Transport",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    truck_category = table.Column<int>(type: "int", nullable: false),
                    origin = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    destiny = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    weight = table.Column<double>(type: "float", nullable: false),
                    capacity = table.Column<double>(type: "float", nullable: false),
                    liters = table.Column<int>(type: "int", nullable: false),
                    value_offered = table.Column<double>(type: "float", nullable: false),
                    user_clientid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transport", x => x.id);
                    table.ForeignKey(
                        name: "FK_Transport_User_user_clientid",
                        column: x => x.user_clientid,
                        principalTable: "User",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Transport_user_clientid",
                table: "Transport",
                column: "user_clientid");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Transport");
        }
    }
}
