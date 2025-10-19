using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FodraszApplication.Data.Migrations
{
    /// <inheritdoc />
    public partial class SzolgaltatasMezo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Szolgaltatas",
                table: "Idopontok",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "Fodraszok",
                keyColumn: "Id",
                keyValue: 1,
                column: "Bemutatkozas",
                value: "Női hajvágás, balayage");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Szolgaltatas",
                table: "Idopontok");

            migrationBuilder.UpdateData(
                table: "Fodraszok",
                keyColumn: "Id",
                keyValue: 1,
                column: "Bemutatkozas",
                value: "Női vágás, balayage");
        }
    }
}
