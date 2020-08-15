using Microsoft.EntityFrameworkCore.Migrations;

namespace DevilDaggersWebsite.Migrations
{
	public partial class RemoveDuplicatePlayerId : Migration
	{
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropColumn(
				name: "PlayerId",
				table: "Players");
		}

		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.AddColumn<int>(
				name: "PlayerId",
				table: "Players",
				type: "int",
				nullable: false,
				defaultValue: 0);
		}
	}
}