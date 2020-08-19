using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace DevilDaggersWebsite.Migrations
{
	public partial class AddUserFilesToDatabase : Migration
	{
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.CreateTable(
				name: "AssetMods",
				columns: table => new
				{
					Id = table.Column<int>(nullable: false)
						.Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
					AssetModTypes = table.Column<int>(nullable: false),
					AssetModFileContents = table.Column<int>(nullable: false),
					Name = table.Column<string>(nullable: false),
					Url = table.Column<string>(nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_AssetMods", x => x.Id);
				});

			migrationBuilder.CreateTable(
				name: "Donations",
				columns: table => new
				{
					Id = table.Column<int>(nullable: false)
						.Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
					PlayerId = table.Column<int>(nullable: true),
					Amount = table.Column<int>(nullable: false),
					Currency = table.Column<int>(nullable: false),
					ConvertedEuroCentsReceived = table.Column<int>(nullable: false),
					DateReceived = table.Column<DateTime>(nullable: false),
					Note = table.Column<string>(nullable: true),
					IsRefunded = table.Column<bool>(nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_Donations", x => x.Id);
				});

			migrationBuilder.CreateTable(
				name: "Players",
				columns: table => new
				{
					Id = table.Column<int>(nullable: false)
						.Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
					PlayerId = table.Column<int>(nullable: false),
					Username = table.Column<string>(nullable: false),
					IsAnonymous = table.Column<bool>(nullable: false),
					CountryCode = table.Column<string>(nullable: true),
					Dpi = table.Column<int>(nullable: true),
					InGameSens = table.Column<float>(nullable: true),
					Fov = table.Column<int>(nullable: true),
					RightHanded = table.Column<bool>(nullable: true),
					FlashEnabled = table.Column<bool>(nullable: true),
					Gamma = table.Column<float>(nullable: true),
					IsBanned = table.Column<bool>(nullable: false),
					BanDescription = table.Column<string>(nullable: true),
					BanResponsibleId = table.Column<int>(nullable: true),
					AssetModId = table.Column<int>(nullable: true)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_Players", x => x.Id);
					table.ForeignKey(
						name: "FK_Players_AssetMods_AssetModId",
						column: x => x.AssetModId,
						principalTable: "AssetMods",
						principalColumn: "Id",
						onDelete: ReferentialAction.Restrict);
				});

			migrationBuilder.CreateTable(
				name: "Titles",
				columns: table => new
				{
					Id = table.Column<int>(nullable: false)
						.Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
					Name = table.Column<string>(nullable: false),
					PlayerId = table.Column<int>(nullable: true)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_Titles", x => x.Id);
					table.ForeignKey(
						name: "FK_Titles_Players_PlayerId",
						column: x => x.PlayerId,
						principalTable: "Players",
						principalColumn: "Id",
						onDelete: ReferentialAction.Restrict);
				});

			migrationBuilder.CreateIndex(
				name: "IX_Players_AssetModId",
				table: "Players",
				column: "AssetModId");

			migrationBuilder.CreateIndex(
				name: "IX_Titles_PlayerId",
				table: "Titles",
				column: "PlayerId");
		}

		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropTable(
				name: "Donations");

			migrationBuilder.DropTable(
				name: "Titles");

			migrationBuilder.DropTable(
				name: "Players");

			migrationBuilder.DropTable(
				name: "AssetMods");
		}
	}
}