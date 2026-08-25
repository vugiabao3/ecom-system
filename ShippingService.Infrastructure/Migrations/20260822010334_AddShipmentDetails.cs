using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShippingService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddShipmentDetails : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "Shipments",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<DateTime>(
                name: "DeliveredAt",
                table: "Shipments",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FailureReason",
                table: "Shipments",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ShipperId",
                table: "Shipments",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "Shipments",
                type: "datetime2",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Shipments_OrderId",
                table: "Shipments",
                column: "OrderId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Shipments_ShipperId",
                table: "Shipments",
                column: "ShipperId");

            migrationBuilder.CreateIndex(
                name: "IX_Shipments_Status",
                table: "Shipments",
                column: "Status");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Shipments_OrderId",
                table: "Shipments");

            migrationBuilder.DropIndex(
                name: "IX_Shipments_ShipperId",
                table: "Shipments");

            migrationBuilder.DropIndex(
                name: "IX_Shipments_Status",
                table: "Shipments");

            migrationBuilder.DropColumn(
                name: "DeliveredAt",
                table: "Shipments");

            migrationBuilder.DropColumn(
                name: "FailureReason",
                table: "Shipments");

            migrationBuilder.DropColumn(
                name: "ShipperId",
                table: "Shipments");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "Shipments");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "Shipments",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");
        }
    }
}
