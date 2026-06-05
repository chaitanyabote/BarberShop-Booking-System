using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BarberShopMVC_2.Migrations
{
    /// <inheritdoc />
    public partial class UpgradeDBookingModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AppointmentDate",
                table: "DBookings");

            migrationBuilder.DropColumn(
                name: "PatientEmail",
                table: "DBookings");

            migrationBuilder.DropColumn(
                name: "PatientName",
                table: "DBookings");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "DBookings",
                newName: "BookingDate");

            migrationBuilder.AddColumn<decimal>(
                name: "AdvancePaid",
                table: "DBookings",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "TotalAmount",
                table: "DBookings",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "DBookings",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_DBookings_UserId",
                table: "DBookings",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_DBookings_Users_UserId",
                table: "DBookings",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DBookings_Users_UserId",
                table: "DBookings");

            migrationBuilder.DropIndex(
                name: "IX_DBookings_UserId",
                table: "DBookings");

            migrationBuilder.DropColumn(
                name: "AdvancePaid",
                table: "DBookings");

            migrationBuilder.DropColumn(
                name: "TotalAmount",
                table: "DBookings");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "DBookings");

            migrationBuilder.RenameColumn(
                name: "BookingDate",
                table: "DBookings",
                newName: "CreatedAt");

            migrationBuilder.AddColumn<DateTime>(
                name: "AppointmentDate",
                table: "DBookings",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "PatientEmail",
                table: "DBookings",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PatientName",
                table: "DBookings",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
