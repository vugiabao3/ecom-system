using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PromotionService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddSellerIdAndBrandIdToPromotion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "UserPoints",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Code",
                table: "Promotions",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<Guid>(
                name: "BrandId",
                table: "Promotions",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "SellerId",
                table: "Promotions",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_UserPoints_UserId",
                table: "UserPoints",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Promotions_Code",
                table: "Promotions",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Promotions_SellerId",
                table: "Promotions",
                column: "SellerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_UserPoints_UserId",
                table: "UserPoints");

            migrationBuilder.DropIndex(
                name: "IX_Promotions_Code",
                table: "Promotions");

            migrationBuilder.DropIndex(
                name: "IX_Promotions_SellerId",
                table: "Promotions");

            migrationBuilder.DropColumn(
                name: "BrandId",
                table: "Promotions");

            migrationBuilder.DropColumn(
                name: "SellerId",
                table: "Promotions");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "UserPoints",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "Code",
                table: "Promotions",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");
        }
    }
}
