using Microsoft.EntityFrameworkCore.Migrations;

namespace DevilDaggersWebsite.Migrations
{
    public partial class V5 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<float>(
                name: "Bronze",
                table: "CustomLeaderboards",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "Devil",
                table: "CustomLeaderboards",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "Gold",
                table: "CustomLeaderboards",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "Silver",
                table: "CustomLeaderboards",
                nullable: false,
                defaultValue: 0f);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Bronze",
                table: "CustomLeaderboards");

            migrationBuilder.DropColumn(
                name: "Devil",
                table: "CustomLeaderboards");

            migrationBuilder.DropColumn(
                name: "Gold",
                table: "CustomLeaderboards");

            migrationBuilder.DropColumn(
                name: "Silver",
                table: "CustomLeaderboards");
        }
    }
}
