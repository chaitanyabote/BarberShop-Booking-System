using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BarberShopMVC_2.Migrations
{
    /// <inheritdoc />
    public partial class MedicalSaasFinal : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ShopId",
                table: "Dermatologists",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ShopId",
                table: "DBookings",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Dermatologists_ShopId",
                table: "Dermatologists",
                column: "ShopId");

            migrationBuilder.CreateIndex(
                name: "IX_DBookings_ShopId",
                table: "DBookings",
                column: "ShopId");

            migrationBuilder.AddForeignKey(
                name: "FK_DBookings_Shops_ShopId",
                table: "DBookings",
                column: "ShopId",
                principalTable: "Shops",
                principalColumn: "ShopId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Dermatologists_Shops_ShopId",
                table: "Dermatologists",
                column: "ShopId",
                principalTable: "Shops",
                principalColumn: "ShopId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DBookings_Shops_ShopId",
                table: "DBookings");

            migrationBuilder.DropForeignKey(
                name: "FK_Dermatologists_Shops_ShopId",
                table: "Dermatologists");

            migrationBuilder.DropIndex(
                name: "IX_Dermatologists_ShopId",
                table: "Dermatologists");

            migrationBuilder.DropIndex(
                name: "IX_DBookings_ShopId",
                table: "DBookings");

            migrationBuilder.DropColumn(
                name: "ShopId",
                table: "Dermatologists");

            migrationBuilder.DropColumn(
                name: "ShopId",
                table: "DBookings");
        }
    }
}
