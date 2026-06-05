using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BarberShopMVC_2.Migrations
{
    /// <inheritdoc />
    public partial class AddBarberStoreFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "Barbers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Bio",
                table: "Barbers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "GoogleMapsEmbedLink",
                table: "Barbers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StoreName",
                table: "Barbers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "Barbers",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Address",
                table: "Barbers");

            migrationBuilder.DropColumn(
                name: "Bio",
                table: "Barbers");

            migrationBuilder.DropColumn(
                name: "GoogleMapsEmbedLink",
                table: "Barbers");

            migrationBuilder.DropColumn(
                name: "StoreName",
                table: "Barbers");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Barbers");
        }
    }
}
