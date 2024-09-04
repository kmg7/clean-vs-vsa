using Microsoft.EntityFrameworkCore.Migrations;
using System.Text.Json;

#nullable disable

namespace Infrastructure.Migrations;

/// <inheritdoc />
public partial class Init : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        _ = migrationBuilder.CreateTable(
            name: "Presentations",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uuid", nullable: false),
                UserId = table.Column<Guid>(type: "uuid", nullable: false),
                Title = table.Column<string>(type: "text", nullable: false),
                CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
            },
            constraints: table =>
            {
                _ = table.PrimaryKey("PK_Presentations", x => x.Id);
            });

        _ = migrationBuilder.CreateTable(
            name: "Slides",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uuid", nullable: false),
                PresentationId = table.Column<Guid>(type: "uuid", nullable: false),
                Index = table.Column<int>(type: "integer", nullable: false),
                IsVisible = table.Column<bool>(type: "boolean", nullable: false),
                Type = table.Column<string>(type: "text", nullable: false),
                Content = table.Column<JsonDocument>(type: "jsonb", nullable: false)
            },
            constraints: table =>
            {
                _ = table.PrimaryKey("PK_Slides", x => x.Id);
            });

        _ = migrationBuilder.CreateTable(
            name: "Users",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uuid", nullable: false),
                Username = table.Column<string>(type: "text", nullable: false),
                Email = table.Column<string>(type: "text", nullable: false),
                ClerkId = table.Column<string>(type: "text", nullable: false),
                IsActive = table.Column<bool>(type: "boolean", nullable: false),
                CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
            },
            constraints: table =>
            {
                _ = table.PrimaryKey("PK_Users", x => x.Id);
            });
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        _ = migrationBuilder.DropTable(
            name: "Presentations");

        _ = migrationBuilder.DropTable(
            name: "Slides");

        _ = migrationBuilder.DropTable(
            name: "Users");
    }
}
