using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BarberShopMVC_2.Migrations
{
    /// <inheritdoc />
    public partial class SaasArchitectureInit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ShopId",
                table: "Services",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ShopId",
                table: "Bookings",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ShopId",
                table: "Barbers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Shops",
                columns: table => new
                {
                    ShopId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ShopName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    UniqueUrlSlug = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    OwnerName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OwnerEmail = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ContactPhone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Shops", x => x.ShopId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Services_ShopId",
                table: "Services",
                column: "ShopId");

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_ShopId",
                table: "Bookings",
                column: "ShopId");

            migrationBuilder.CreateIndex(
                name: "IX_Barbers_ShopId",
                table: "Barbers",
                column: "ShopId");

            migrationBuilder.AddForeignKey(
                name: "FK_Barbers_Shops_ShopId",
                table: "Barbers",
                column: "ShopId",
                principalTable: "Shops",
                principalColumn: "ShopId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_Shops_ShopId",
                table: "Bookings",
                column: "ShopId",
                principalTable: "Shops",
                principalColumn: "ShopId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Services_Shops_ShopId",
                table: "Services",
                column: "ShopId",
                principalTable: "Shops",
                principalColumn: "ShopId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Barbers_Shops_ShopId",
                table: "Barbers");

            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_Shops_ShopId",
                table: "Bookings");

            migrationBuilder.DropForeignKey(
                name: "FK_Services_Shops_ShopId",
                table: "Services");

            migrationBuilder.DropTable(
                name: "Shops");

            migrationBuilder.DropIndex(
                name: "IX_Services_ShopId",
                table: "Services");

            migrationBuilder.DropIndex(
                name: "IX_Bookings_ShopId",
                table: "Bookings");

            migrationBuilder.DropIndex(
                name: "IX_Barbers_ShopId",
                table: "Barbers");

            migrationBuilder.DropColumn(
                name: "ShopId",
                table: "Services");

            migrationBuilder.DropColumn(
                name: "ShopId",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "ShopId",
                table: "Barbers");
        }
    }
}
