using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TicTacToe.Data.Migration
{
    /// <inheritdoc />
    public partial class ChangeComputerAndMultiplayerTables : Microsoft.EntityFrameworkCore.Migrations.Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MultiplayerLeaderboard");

            migrationBuilder.RenameColumn(
                name: "Winner",
                table: "ComputerLeaderboard",
                newName: "Result");

            migrationBuilder.AddColumn<string>(
                name: "PlayerColor",
                table: "ComputerLeaderboard",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PlayerColor",
                table: "ComputerLeaderboard");

            migrationBuilder.RenameColumn(
                name: "Result",
                table: "ComputerLeaderboard",
                newName: "Winner");

            migrationBuilder.CreateTable(
                name: "MultiplayerLeaderboard",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Player1Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Player2Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Winner = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MultiplayerLeaderboard", x => x.Id);
                });
        }
    }
}
