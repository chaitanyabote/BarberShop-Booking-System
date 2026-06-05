using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BarberShopMVC_2.Migrations
{
    /// <inheritdoc />
    public partial class AddDBookingTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DBookings",
                columns: table => new
                {
                    DBookingId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DermatologistId = table.Column<int>(type: "int", nullable: false),
                    PatientName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PatientEmail = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AppointmentDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TimeSlot = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DBookings", x => x.DBookingId);
                    table.ForeignKey(
                        name: "FK_DBookings_Dermatologists_DermatologistId",
                        column: x => x.DermatologistId,
                        principalTable: "Dermatologists",
                        principalColumn: "DermatologistId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DBookings_DermatologistId",
                table: "DBookings",
                column: "DermatologistId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DBookings");
        }
    }
}
