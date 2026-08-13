using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CartService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Addupvali : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsValid",
                table: "CartItems",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsValid",
                table: "CartItems");
        }
    }
}
