using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PM.INFRASTRUCTURE.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2024, 12, 28, 20, 31, 8, 707, DateTimeKind.Utc).AddTicks(403));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2024, 12, 28, 20, 31, 8, 707, DateTimeKind.Utc).AddTicks(1163));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2024, 12, 28, 19, 47, 30, 427, DateTimeKind.Utc).AddTicks(5924));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2024, 12, 28, 19, 47, 30, 427, DateTimeKind.Utc).AddTicks(6691));
        }
    }
}
