using Microsoft.EntityFrameworkCore.Migrations;

namespace DevilDaggersWebsite.Migrations
{
	public partial class RenameShotsToDaggers : Migration
	{
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropColumn(
				name: "ShotsFired",
				table: "CustomEntries");

			migrationBuilder.DropColumn(
				name: "ShotsHit",
				table: "CustomEntries");

			migrationBuilder.AddColumn<int>(
				name: "DaggersFired",
				table: "CustomEntries",
				nullable: false,
				defaultValue: 0);

			migrationBuilder.AddColumn<int>(
				name: "DaggersHit",
				table: "CustomEntries",
				nullable: false,
				defaultValue: 0);
		}

		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropColumn(
				name: "DaggersFired",
				table: "CustomEntries");

			migrationBuilder.DropColumn(
				name: "DaggersHit",
				table: "CustomEntries");

			migrationBuilder.AddColumn<int>(
				name: "ShotsFired",
				table: "CustomEntries",
				type: "int",
				nullable: false,
				defaultValue: 0);

			migrationBuilder.AddColumn<int>(
				name: "ShotsHit",
				table: "CustomEntries",
				type: "int",
				nullable: false,
				defaultValue: 0);
		}
	}
}