using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace api.Migrations
{
    /// <inheritdoc />
    public partial class Favourites : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "330bfac3-e92d-4e8e-954c-16c03082a972");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "5bb015ca-f662-4540-9838-701c33b2f6bf");

            migrationBuilder.CreateTable(
                name: "FavoriteSpots",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "text", nullable: false),
                    SpotId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FavoriteSpots", x => new { x.UserId, x.SpotId });
                    table.ForeignKey(
                        name: "FK_FavoriteSpots_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FavoriteSpots_Spots_SpotId",
                        column: x => x.SpotId,
                        principalTable: "Spots",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "f44caec7-099f-4bd0-91f5-59bbc7f81316", null, "User", "USER" },
                    { "f800d45d-1824-4c76-ad51-b0e0ced0d09e", null, "Admin", "ADMIN" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_FavoriteSpots_SpotId",
                table: "FavoriteSpots",
                column: "SpotId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FavoriteSpots");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "f44caec7-099f-4bd0-91f5-59bbc7f81316");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "f800d45d-1824-4c76-ad51-b0e0ced0d09e");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "330bfac3-e92d-4e8e-954c-16c03082a972", null, "Admin", "ADMIN" },
                    { "5bb015ca-f662-4540-9838-701c33b2f6bf", null, "User", "USER" }
                });
        }
    }
}
