using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TicTacToe.Data.Migration
{
    /// <inheritdoc />
    public partial class AddMultiplayerModeTable : Microsoft.EntityFrameworkCore.Migrations.Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MultiplayerModes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Player1Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Player2Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Player1Color = table.Column<string>(type: "nvarchar(7)", maxLength: 7, nullable: false),
                    Player2Color = table.Column<string>(type: "nvarchar(7)", maxLength: 7, nullable: false),
                    Result = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MultiplayerModes", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MultiplayerModes_Date",
                table: "MultiplayerModes",
                column: "Date");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MultiplayerModes");
        }
    }
}
