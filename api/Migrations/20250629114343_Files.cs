using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace api.Migrations
{
    /// <inheritdoc />
    public partial class Files : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "372f0dc8-dd99-436a-bdff-cdb6a1830eec");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "96b104f9-e03b-4f2d-a4b7-5de90f6b656b");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Spots",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "FileId",
                table: "Spots",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsToBeDeleted",
                table: "Spots",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Spots",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Roads",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "FileId",
                table: "Roads",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsToBeDeleted",
                table: "Roads",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Roads",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "AvatarId",
                table: "AspNetUsers",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Files",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<string>(type: "text", nullable: false),
                    FileName = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Files", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Files_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "330bfac3-e92d-4e8e-954c-16c03082a972", null, "Admin", "ADMIN" },
                    { "5bb015ca-f662-4540-9838-701c33b2f6bf", null, "User", "USER" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Files_UserId",
                table: "Files",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Files");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "330bfac3-e92d-4e8e-954c-16c03082a972");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "5bb015ca-f662-4540-9838-701c33b2f6bf");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Spots");

            migrationBuilder.DropColumn(
                name: "FileId",
                table: "Spots");

            migrationBuilder.DropColumn(
                name: "IsToBeDeleted",
                table: "Spots");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Spots");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Roads");

            migrationBuilder.DropColumn(
                name: "FileId",
                table: "Roads");

            migrationBuilder.DropColumn(
                name: "IsToBeDeleted",
                table: "Roads");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Roads");

            migrationBuilder.DropColumn(
                name: "AvatarId",
                table: "AspNetUsers");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "372f0dc8-dd99-436a-bdff-cdb6a1830eec", null, "User", "USER" },
                    { "96b104f9-e03b-4f2d-a4b7-5de90f6b656b", null, "Admin", "ADMIN" }
                });
        }
    }
}
