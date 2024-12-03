using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace goonline.Migrations
{
    /// <inheritdoc />
    public partial class migracja2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Todos",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    title = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    expiryDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    percentComplete = table.Column<int>(type: "int", nullable: false),
                    isDone = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Todos", x => x.id);
                });

            migrationBuilder.InsertData(
                table: "Todos",
                columns: new[] { "id", "description", "expiryDate", "isDone", "percentComplete", "title" },
                values: new object[,]
                {
                    { 1, "Resolve the issue causing downtime on the payment gateway.", new DateTime(2024, 12, 4, 14, 3, 34, 638, DateTimeKind.Local).AddTicks(1691), false, 0, "Fix critical bug in production" },
                    { 2, "Finish REST API Task", new DateTime(2024, 12, 6, 14, 3, 34, 639, DateTimeKind.Local).AddTicks(9682), false, 50, "Complete Project" },
                    { 3, "Add detailed API documentation for the new endpoints using Swagger.", new DateTime(2024, 12, 4, 2, 3, 34, 639, DateTimeKind.Local).AddTicks(9702), true, 100, "Update documentation for REST API" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Todos");
        }
    }
}
