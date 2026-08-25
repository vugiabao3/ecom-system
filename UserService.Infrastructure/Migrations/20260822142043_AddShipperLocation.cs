using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UseService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddShipperLocation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CurrentAddress",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CurrentLocation",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CurrentAddress",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "CurrentLocation",
                table: "Users");
        }
    }
}
