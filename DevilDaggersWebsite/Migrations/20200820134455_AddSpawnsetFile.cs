using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace DevilDaggersWebsite.Migrations
{
	public partial class AddSpawnsetFile : Migration
	{
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.CreateTable(
				name: "SpawnsetFiles",
				columns: table => new
				{
					Id = table.Column<int>(nullable: false)
						.Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
					Name = table.Column<string>(nullable: false),
					PlayerId = table.Column<int>(nullable: false),
					MaxDisplayWaves = table.Column<int>(nullable: true),
					HtmlDescription = table.Column<string>(nullable: true),
					LastUpdated = table.Column<DateTime>(nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_SpawnsetFiles", x => x.Id);
				});
		}

		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropTable(
				name: "SpawnsetFiles");
		}
	}
}