using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FodraszApplication.Data.Migrations
{
    /// <inheritdoc />
    public partial class FodraszFelhasznaloKapcsolat : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FelhasznaloId",
                table: "Fodraszok",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Fodraszok",
                keyColumn: "Id",
                keyValue: 1,
                column: "FelhasznaloId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Fodraszok",
                keyColumn: "Id",
                keyValue: 2,
                column: "FelhasznaloId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Fodraszok",
                keyColumn: "Id",
                keyValue: 3,
                column: "FelhasznaloId",
                value: null);

            migrationBuilder.CreateIndex(
                name: "IX_Fodraszok_FelhasznaloId",
                table: "Fodraszok",
                column: "FelhasznaloId");

            migrationBuilder.AddForeignKey(
                name: "FK_Fodraszok_AspNetUsers_FelhasznaloId",
                table: "Fodraszok",
                column: "FelhasznaloId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Fodraszok_AspNetUsers_FelhasznaloId",
                table: "Fodraszok");

            migrationBuilder.DropIndex(
                name: "IX_Fodraszok_FelhasznaloId",
                table: "Fodraszok");

            migrationBuilder.DropColumn(
                name: "FelhasznaloId",
                table: "Fodraszok");
        }
    }
}
