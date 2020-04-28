using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace DevilDaggersWebsite.Migrations
{
	public partial class Initial : Migration
	{
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.CreateTable(
				name: "CustomLeaderboardCategories",
				columns: table => new
				{
					Name = table.Column<string>(nullable: true),
					SortingPropertyName = table.Column<string>(nullable: true),
					Ascending = table.Column<bool>(nullable: false),
					LayoutPartialName = table.Column<string>(nullable: true),
					ID = table.Column<int>(nullable: false)
						.Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_CustomLeaderboardCategories", x => x.ID);
				});

			migrationBuilder.CreateTable(
				name: "CustomLeaderboards",
				columns: table => new
				{
					SpawnsetFileName = table.Column<string>(nullable: true),
					Bronze = table.Column<float>(nullable: false),
					Silver = table.Column<float>(nullable: false),
					Golden = table.Column<float>(nullable: false),
					Devil = table.Column<float>(nullable: false),
					Homing = table.Column<float>(nullable: false),
					DateLastPlayed = table.Column<DateTime>(nullable: true),
					DateCreated = table.Column<DateTime>(nullable: true),
					ID = table.Column<int>(nullable: false)
						.Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
					CategoryID = table.Column<int>(nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_CustomLeaderboards", x => x.ID);
					table.ForeignKey(
						name: "FK_CustomLeaderboards_CustomLeaderboardCategories_CategoryID",
						column: x => x.CategoryID,
						principalTable: "CustomLeaderboardCategories",
						principalColumn: "ID",
						onDelete: ReferentialAction.Cascade);
				});

			migrationBuilder.CreateTable(
				name: "CustomEntries",
				columns: table => new
				{
					PlayerID = table.Column<int>(nullable: false),
					Username = table.Column<string>(nullable: true),
					Time = table.Column<float>(nullable: false),
					Gems = table.Column<int>(nullable: false),
					Kills = table.Column<int>(nullable: false),
					DeathType = table.Column<int>(nullable: false),
					ShotsHit = table.Column<int>(nullable: false),
					ShotsFired = table.Column<int>(nullable: false),
					EnemiesAlive = table.Column<int>(nullable: false),
					Homing = table.Column<int>(nullable: false),
					LevelUpTime2 = table.Column<float>(nullable: false),
					LevelUpTime3 = table.Column<float>(nullable: false),
					LevelUpTime4 = table.Column<float>(nullable: false),
					SubmitDate = table.Column<DateTime>(nullable: false),
					ClientVersion = table.Column<string>(nullable: true),
					ID = table.Column<int>(nullable: false)
						.Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
					CustomLeaderboardID = table.Column<int>(nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_CustomEntries", x => x.ID);
					table.ForeignKey(
						name: "FK_CustomEntries_CustomLeaderboards_CustomLeaderboardID",
						column: x => x.CustomLeaderboardID,
						principalTable: "CustomLeaderboards",
						principalColumn: "ID",
						onDelete: ReferentialAction.Cascade);
				});

			migrationBuilder.CreateIndex(
				name: "IX_CustomEntries_CustomLeaderboardID",
				table: "CustomEntries",
				column: "CustomLeaderboardID");

			migrationBuilder.CreateIndex(
				name: "IX_CustomLeaderboards_CategoryID",
				table: "CustomLeaderboards",
				column: "CategoryID");
		}

		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropTable(
				name: "CustomEntries");

			migrationBuilder.DropTable(
				name: "CustomLeaderboards");

			migrationBuilder.DropTable(
				name: "CustomLeaderboardCategories");
		}
	}
}