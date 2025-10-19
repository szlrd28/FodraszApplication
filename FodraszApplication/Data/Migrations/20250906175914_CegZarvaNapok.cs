using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FodraszApplication.Data.Migrations
{
    /// <inheritdoc />
    public partial class CegZarvaNapok : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Idopontok_Fodraszok_FodraszId",
                table: "Idopontok");

            migrationBuilder.DropIndex(
                name: "IX_Idopontok_FodraszId_Kezdet",
                table: "Idopontok");

            migrationBuilder.CreateTable(
                name: "ZarvaNapok",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Datum = table.Column<DateTime>(type: "date", nullable: false),
                    Megjegyzes = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ZarvaNapok", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Idopontok_FodraszId",
                table: "Idopontok",
                column: "FodraszId");

            migrationBuilder.CreateIndex(
                name: "IX_ZarvaNapok_Datum",
                table: "ZarvaNapok",
                column: "Datum",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Idopontok_Fodraszok_FodraszId",
                table: "Idopontok",
                column: "FodraszId",
                principalTable: "Fodraszok",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Idopontok_Fodraszok_FodraszId",
                table: "Idopontok");

            migrationBuilder.DropTable(
                name: "ZarvaNapok");

            migrationBuilder.DropIndex(
                name: "IX_Idopontok_FodraszId",
                table: "Idopontok");

            migrationBuilder.CreateIndex(
                name: "IX_Idopontok_FodraszId_Kezdet",
                table: "Idopontok",
                columns: new[] { "FodraszId", "Kezdet" });

            migrationBuilder.AddForeignKey(
                name: "FK_Idopontok_Fodraszok_FodraszId",
                table: "Idopontok",
                column: "FodraszId",
                principalTable: "Fodraszok",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
