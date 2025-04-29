
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddGridPositionAndGameBoardColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "GameBoard",
                table: "SaveGames",
                type: "TEXT",
                maxLength: 10240,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "GridPositionX",
                table: "SaveGames",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "GridPositionY",
                table: "SaveGames",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GameBoard",
                table: "SaveGames");

            migrationBuilder.DropColumn(
                name: "GridPositionX",
                table: "SaveGames");

            migrationBuilder.DropColumn(
                name: "GridPositionY",
                table: "SaveGames");
        }
    }
}
