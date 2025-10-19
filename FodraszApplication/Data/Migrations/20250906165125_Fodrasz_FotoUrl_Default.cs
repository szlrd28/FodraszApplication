using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FodraszApplication.Data.Migrations
{
    /// <inheritdoc />
    public partial class Fodrasz_FotoUrl_Default : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
    "UPDATE Fodraszok SET FotoUrl = '/img/bildcomming.jpg' WHERE FotoUrl IS NULL OR LTRIM(RTRIM(FotoUrl)) = ''");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "FotoUrl",
                table: "Fodraszok",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(256)",
                oldMaxLength: 256,
                oldNullable: true,
                oldDefaultValue: "/img/bildcomming.jpg");
        }
    }
}
