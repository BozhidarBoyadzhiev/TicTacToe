using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TicTacToe.Data.Migration
{
    /// <inheritdoc />
    public partial class MakeChangesToComputerModeTable : Microsoft.EntityFrameworkCore.Migrations.Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ComputerLeaderboard",
                table: "ComputerLeaderboard");

            migrationBuilder.RenameTable(
                name: "ComputerLeaderboard",
                newName: "ComputerModes");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ComputerModes",
                table: "ComputerModes",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ComputerModes",
                table: "ComputerModes");

            migrationBuilder.RenameTable(
                name: "ComputerModes",
                newName: "ComputerLeaderboard");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ComputerLeaderboard",
                table: "ComputerLeaderboard",
                column: "Id");
        }
    }
}
