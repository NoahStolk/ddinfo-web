using Microsoft.EntityFrameworkCore.Migrations;

namespace DevilDaggersWebsite.Migrations
{
	public partial class RenameManyToManyTables : Migration
	{
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropForeignKey(
				name: "FK_PlayerAssetMod_AssetMods_AssetModId",
				table: "PlayerAssetMod");

			migrationBuilder.DropForeignKey(
				name: "FK_PlayerAssetMod_Players_PlayerId",
				table: "PlayerAssetMod");

			migrationBuilder.DropForeignKey(
				name: "FK_PlayerTitle_Players_PlayerId",
				table: "PlayerTitle");

			migrationBuilder.DropForeignKey(
				name: "FK_PlayerTitle_Titles_TitleId",
				table: "PlayerTitle");

			migrationBuilder.DropPrimaryKey(
				name: "PK_PlayerTitle",
				table: "PlayerTitle");

			migrationBuilder.DropPrimaryKey(
				name: "PK_PlayerAssetMod",
				table: "PlayerAssetMod");

			migrationBuilder.DropColumn(
				name: "Username",
				table: "CustomEntries");

			migrationBuilder.RenameTable(
				name: "PlayerTitle",
				newName: "PlayerTitles");

			migrationBuilder.RenameTable(
				name: "PlayerAssetMod",
				newName: "PlayerAssetMods");

			migrationBuilder.RenameIndex(
				name: "IX_PlayerTitle_TitleId",
				table: "PlayerTitles",
				newName: "IX_PlayerTitles_TitleId");

			migrationBuilder.RenameIndex(
				name: "IX_PlayerAssetMod_AssetModId",
				table: "PlayerAssetMods",
				newName: "IX_PlayerAssetMods_AssetModId");

			migrationBuilder.AlterColumn<string>(
				name: "Name",
				table: "AspNetUserTokens",
				maxLength: 128,
				nullable: false,
				oldClrType: typeof(string),
				oldType: "varchar(255) CHARACTER SET utf8mb4");

			migrationBuilder.AlterColumn<string>(
				name: "LoginProvider",
				table: "AspNetUserTokens",
				maxLength: 128,
				nullable: false,
				oldClrType: typeof(string),
				oldType: "varchar(255) CHARACTER SET utf8mb4");

			migrationBuilder.AlterColumn<string>(
				name: "ProviderKey",
				table: "AspNetUserLogins",
				maxLength: 128,
				nullable: false,
				oldClrType: typeof(string),
				oldType: "varchar(255) CHARACTER SET utf8mb4");

			migrationBuilder.AlterColumn<string>(
				name: "LoginProvider",
				table: "AspNetUserLogins",
				maxLength: 128,
				nullable: false,
				oldClrType: typeof(string),
				oldType: "varchar(255) CHARACTER SET utf8mb4");

			migrationBuilder.AddPrimaryKey(
				name: "PK_PlayerTitles",
				table: "PlayerTitles",
				columns: new[] { "PlayerId", "TitleId" });

			migrationBuilder.AddPrimaryKey(
				name: "PK_PlayerAssetMods",
				table: "PlayerAssetMods",
				columns: new[] { "PlayerId", "AssetModId" });

			migrationBuilder.AddForeignKey(
				name: "FK_PlayerAssetMods_AssetMods_AssetModId",
				table: "PlayerAssetMods",
				column: "AssetModId",
				principalTable: "AssetMods",
				principalColumn: "Id",
				onDelete: ReferentialAction.Cascade);

			migrationBuilder.AddForeignKey(
				name: "FK_PlayerAssetMods_Players_PlayerId",
				table: "PlayerAssetMods",
				column: "PlayerId",
				principalTable: "Players",
				principalColumn: "Id",
				onDelete: ReferentialAction.Cascade);

			migrationBuilder.AddForeignKey(
				name: "FK_PlayerTitles_Players_PlayerId",
				table: "PlayerTitles",
				column: "PlayerId",
				principalTable: "Players",
				principalColumn: "Id",
				onDelete: ReferentialAction.Cascade);

			migrationBuilder.AddForeignKey(
				name: "FK_PlayerTitles_Titles_TitleId",
				table: "PlayerTitles",
				column: "TitleId",
				principalTable: "Titles",
				principalColumn: "Id",
				onDelete: ReferentialAction.Cascade);
		}

		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropForeignKey(
				name: "FK_PlayerAssetMods_AssetMods_AssetModId",
				table: "PlayerAssetMods");

			migrationBuilder.DropForeignKey(
				name: "FK_PlayerAssetMods_Players_PlayerId",
				table: "PlayerAssetMods");

			migrationBuilder.DropForeignKey(
				name: "FK_PlayerTitles_Players_PlayerId",
				table: "PlayerTitles");

			migrationBuilder.DropForeignKey(
				name: "FK_PlayerTitles_Titles_TitleId",
				table: "PlayerTitles");

			migrationBuilder.DropPrimaryKey(
				name: "PK_PlayerTitles",
				table: "PlayerTitles");

			migrationBuilder.DropPrimaryKey(
				name: "PK_PlayerAssetMods",
				table: "PlayerAssetMods");

			migrationBuilder.RenameTable(
				name: "PlayerTitles",
				newName: "PlayerTitle");

			migrationBuilder.RenameTable(
				name: "PlayerAssetMods",
				newName: "PlayerAssetMod");

			migrationBuilder.RenameIndex(
				name: "IX_PlayerTitles_TitleId",
				table: "PlayerTitle",
				newName: "IX_PlayerTitle_TitleId");

			migrationBuilder.RenameIndex(
				name: "IX_PlayerAssetMods_AssetModId",
				table: "PlayerAssetMod",
				newName: "IX_PlayerAssetMod_AssetModId");

			migrationBuilder.AddColumn<string>(
				name: "Username",
				table: "CustomEntries",
				type: "longtext CHARACTER SET utf8mb4",
				nullable: false,
				defaultValue: "");

			migrationBuilder.AlterColumn<string>(
				name: "Name",
				table: "AspNetUserTokens",
				type: "varchar(255) CHARACTER SET utf8mb4",
				nullable: false,
				oldClrType: typeof(string),
				oldMaxLength: 128);

			migrationBuilder.AlterColumn<string>(
				name: "LoginProvider",
				table: "AspNetUserTokens",
				type: "varchar(255) CHARACTER SET utf8mb4",
				nullable: false,
				oldClrType: typeof(string),
				oldMaxLength: 128);

			migrationBuilder.AlterColumn<string>(
				name: "ProviderKey",
				table: "AspNetUserLogins",
				type: "varchar(255) CHARACTER SET utf8mb4",
				nullable: false,
				oldClrType: typeof(string),
				oldMaxLength: 128);

			migrationBuilder.AlterColumn<string>(
				name: "LoginProvider",
				table: "AspNetUserLogins",
				type: "varchar(255) CHARACTER SET utf8mb4",
				nullable: false,
				oldClrType: typeof(string),
				oldMaxLength: 128);

			migrationBuilder.AddPrimaryKey(
				name: "PK_PlayerTitle",
				table: "PlayerTitle",
				columns: new[] { "PlayerId", "TitleId" });

			migrationBuilder.AddPrimaryKey(
				name: "PK_PlayerAssetMod",
				table: "PlayerAssetMod",
				columns: new[] { "PlayerId", "AssetModId" });

			migrationBuilder.AddForeignKey(
				name: "FK_PlayerAssetMod_AssetMods_AssetModId",
				table: "PlayerAssetMod",
				column: "AssetModId",
				principalTable: "AssetMods",
				principalColumn: "Id",
				onDelete: ReferentialAction.Cascade);

			migrationBuilder.AddForeignKey(
				name: "FK_PlayerAssetMod_Players_PlayerId",
				table: "PlayerAssetMod",
				column: "PlayerId",
				principalTable: "Players",
				principalColumn: "Id",
				onDelete: ReferentialAction.Cascade);

			migrationBuilder.AddForeignKey(
				name: "FK_PlayerTitle_Players_PlayerId",
				table: "PlayerTitle",
				column: "PlayerId",
				principalTable: "Players",
				principalColumn: "Id",
				onDelete: ReferentialAction.Cascade);

			migrationBuilder.AddForeignKey(
				name: "FK_PlayerTitle_Titles_TitleId",
				table: "PlayerTitle",
				column: "TitleId",
				principalTable: "Titles",
				principalColumn: "Id",
				onDelete: ReferentialAction.Cascade);
		}
	}
}