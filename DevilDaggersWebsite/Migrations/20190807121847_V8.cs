using Microsoft.EntityFrameworkCore.Migrations;

namespace DevilDaggersWebsite.Migrations
{
    public partial class V8 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SpawnsetHash",
                table: "CustomLeaderboards");

            migrationBuilder.RenameColumn(
                name: "DDCLClientVersion",
                table: "CustomEntries",
                newName: "ClientVersion");

            migrationBuilder.AddColumn<int>(
                name: "Category",
                table: "CustomLeaderboards",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<float>(
                name: "Homing",
                table: "CustomLeaderboards",
                nullable: false,
                defaultValue: 0f);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Category",
                table: "CustomLeaderboards");

            migrationBuilder.DropColumn(
                name: "Homing",
                table: "CustomLeaderboards");

            migrationBuilder.RenameColumn(
                name: "ClientVersion",
                table: "CustomEntries",
                newName: "DDCLClientVersion");

            migrationBuilder.AddColumn<string>(
                name: "SpawnsetHash",
                table: "CustomLeaderboards",
                nullable: true);
        }
    }
}
