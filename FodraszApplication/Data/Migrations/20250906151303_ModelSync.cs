using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FodraszApplication.Data.Migrations
{
    /// <inheritdoc />
    public partial class ModelSync : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Nyitvatartasok",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FodraszId = table.Column<int>(type: "int", nullable: false),
                    Nap = table.Column<int>(type: "int", nullable: false),
                    Zarva = table.Column<bool>(type: "bit", nullable: false),
                    Nyitas = table.Column<TimeSpan>(type: "time", nullable: false),
                    Zaras = table.Column<TimeSpan>(type: "time", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Nyitvatartasok", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Nyitvatartasok_Fodraszok_FodraszId",
                        column: x => x.FodraszId,
                        principalTable: "Fodraszok",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Nyitvatartasok_FodraszId_Nap",
                table: "Nyitvatartasok",
                columns: new[] { "FodraszId", "Nap" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Nyitvatartasok");
        }
    }
}
