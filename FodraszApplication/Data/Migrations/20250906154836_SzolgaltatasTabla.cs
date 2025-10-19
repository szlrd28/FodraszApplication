using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FodraszApplication.Data.Migrations
{
    /// <inheritdoc />
    public partial class SzolgaltatasTabla : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Szolgaltatasok",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Kod = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false),
                    Nev = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    Kategoria = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    Perc = table.Column<int>(type: "int", nullable: false),
                    ArHuf = table.Column<int>(type: "int", nullable: false),
                    Aktiv = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Szolgaltatasok", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Szolgaltatasok_Kod",
                table: "Szolgaltatasok",
                column: "Kod",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Szolgaltatasok");
        }
    }
}
