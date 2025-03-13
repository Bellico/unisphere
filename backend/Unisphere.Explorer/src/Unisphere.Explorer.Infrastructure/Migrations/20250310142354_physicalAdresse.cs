using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Unisphere.Explorer.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class PhysicalAdresse : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "address",
                schema: "explorer",
                table: "houses",
                newName: "ZipCode");

            migrationBuilder.AddColumn<string>(
                name: "City",
                schema: "explorer",
                table: "houses",
                type: "text",
                nullable: false,
                defaultValue: string.Empty);

            migrationBuilder.AddColumn<string>(
                name: "CountryCode",
                schema: "explorer",
                table: "houses",
                type: "text",
                nullable: false,
                defaultValue: string.Empty);

            migrationBuilder.AddColumn<string>(
                name: "Street",
                schema: "explorer",
                table: "houses",
                type: "text",
                nullable: false,
                defaultValue: string.Empty);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "City",
                schema: "explorer",
                table: "houses");

            migrationBuilder.DropColumn(
                name: "CountryCode",
                schema: "explorer",
                table: "houses");

            migrationBuilder.DropColumn(
                name: "Street",
                schema: "explorer",
                table: "houses");

            migrationBuilder.RenameColumn(
                name: "ZipCode",
                schema: "explorer",
                table: "houses",
                newName: "address");
        }
    }
}
