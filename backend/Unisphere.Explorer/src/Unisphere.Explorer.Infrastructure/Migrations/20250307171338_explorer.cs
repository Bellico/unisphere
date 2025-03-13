using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Unisphere.Explorer.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Explorer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "explorer");

            migrationBuilder.CreateTable(
                name: "houses",
                schema: "explorer",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    author_id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    address = table.Column<string>(type: "text", nullable: false),
                    description = table.Column<string>(type: "text", nullable: false),
                    GlobalNote = table.Column<int>(type: "integer", nullable: false),
                    CleanlinessNote = table.Column<int>(type: "integer", nullable: false),
                    CommunicationNote = table.Column<int>(type: "integer", nullable: false),
                    LocationNote = table.Column<int>(type: "integer", nullable: false),
                    image_url = table.Column<string>(type: "text", nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_houses", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "bookings",
                schema: "explorer",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    author_id = table.Column<Guid>(type: "uuid", nullable: false),
                    house_id = table.Column<Guid>(type: "uuid", nullable: false),
                    check_in = table.Column<DateOnly>(type: "date", nullable: false),
                    check_out = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_bookings", x => x.id);
                    table.ForeignKey(
                        name: "fk_bookings_houses_house_id",
                        column: x => x.house_id,
                        principalSchema: "explorer",
                        principalTable: "houses",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_bookings_house_id",
                schema: "explorer",
                table: "bookings",
                column: "house_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "bookings",
                schema: "explorer");

            migrationBuilder.DropTable(
                name: "houses",
                schema: "explorer");
        }
    }
}
