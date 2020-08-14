using Microsoft.EntityFrameworkCore.Migrations;

namespace DevilDaggersWebsite.Migrations
{
    public partial class AddManyToManyRelations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Players_AssetMods_AssetModId",
                table: "Players");

            migrationBuilder.DropForeignKey(
                name: "FK_Titles_Players_PlayerId",
                table: "Titles");

            migrationBuilder.DropIndex(
                name: "IX_Titles_PlayerId",
                table: "Titles");

            migrationBuilder.DropIndex(
                name: "IX_Players_AssetModId",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "PlayerId",
                table: "Titles");

            migrationBuilder.DropColumn(
                name: "AssetModId",
                table: "Players");

            migrationBuilder.CreateTable(
                name: "PlayerAssetMod",
                columns: table => new
                {
                    PlayerId = table.Column<int>(nullable: false),
                    AssetModId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlayerAssetMod", x => new { x.PlayerId, x.AssetModId });
                    table.ForeignKey(
                        name: "FK_PlayerAssetMod_AssetMods_AssetModId",
                        column: x => x.AssetModId,
                        principalTable: "AssetMods",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PlayerAssetMod_Players_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "Players",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PlayerTitle",
                columns: table => new
                {
                    PlayerId = table.Column<int>(nullable: false),
                    TitleId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlayerTitle", x => new { x.PlayerId, x.TitleId });
                    table.ForeignKey(
                        name: "FK_PlayerTitle_Players_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "Players",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PlayerTitle_Titles_TitleId",
                        column: x => x.TitleId,
                        principalTable: "Titles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PlayerAssetMod_AssetModId",
                table: "PlayerAssetMod",
                column: "AssetModId");

            migrationBuilder.CreateIndex(
                name: "IX_PlayerTitle_TitleId",
                table: "PlayerTitle",
                column: "TitleId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PlayerAssetMod");

            migrationBuilder.DropTable(
                name: "PlayerTitle");

            migrationBuilder.AddColumn<int>(
                name: "PlayerId",
                table: "Titles",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AssetModId",
                table: "Players",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Titles_PlayerId",
                table: "Titles",
                column: "PlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_Players_AssetModId",
                table: "Players",
                column: "AssetModId");

            migrationBuilder.AddForeignKey(
                name: "FK_Players_AssetMods_AssetModId",
                table: "Players",
                column: "AssetModId",
                principalTable: "AssetMods",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Titles_Players_PlayerId",
                table: "Titles",
                column: "PlayerId",
                principalTable: "Players",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
