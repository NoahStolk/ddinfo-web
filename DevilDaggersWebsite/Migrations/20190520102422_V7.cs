using Microsoft.EntityFrameworkCore.Migrations;

namespace DevilDaggersWebsite.Migrations
{
    public partial class V7 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DDCLClientVersion",
                table: "CustomEntries",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DDCLClientVersion",
                table: "CustomEntries");
        }
    }
}
