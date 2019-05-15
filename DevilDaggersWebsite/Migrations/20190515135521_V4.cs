using Microsoft.EntityFrameworkCore.Migrations;

namespace DevilDaggersWebsite.Migrations
{
    public partial class V4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CustomEntry_CustomLeaderboards_CustomLeaderboardID",
                table: "CustomEntry");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CustomEntry",
                table: "CustomEntry");

            migrationBuilder.RenameTable(
                name: "CustomEntry",
                newName: "CustomEntries");

            migrationBuilder.RenameIndex(
                name: "IX_CustomEntry_CustomLeaderboardID",
                table: "CustomEntries",
                newName: "IX_CustomEntries_CustomLeaderboardID");

            migrationBuilder.AddColumn<int>(
                name: "PlayerID",
                table: "CustomEntries",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_CustomEntries",
                table: "CustomEntries",
                column: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_CustomEntries_CustomLeaderboards_CustomLeaderboardID",
                table: "CustomEntries",
                column: "CustomLeaderboardID",
                principalTable: "CustomLeaderboards",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CustomEntries_CustomLeaderboards_CustomLeaderboardID",
                table: "CustomEntries");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CustomEntries",
                table: "CustomEntries");

            migrationBuilder.DropColumn(
                name: "PlayerID",
                table: "CustomEntries");

            migrationBuilder.RenameTable(
                name: "CustomEntries",
                newName: "CustomEntry");

            migrationBuilder.RenameIndex(
                name: "IX_CustomEntries_CustomLeaderboardID",
                table: "CustomEntry",
                newName: "IX_CustomEntry_CustomLeaderboardID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CustomEntry",
                table: "CustomEntry",
                column: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_CustomEntry_CustomLeaderboards_CustomLeaderboardID",
                table: "CustomEntry",
                column: "CustomLeaderboardID",
                principalTable: "CustomLeaderboards",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
