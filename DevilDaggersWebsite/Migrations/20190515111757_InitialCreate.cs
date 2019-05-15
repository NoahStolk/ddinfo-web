using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DevilDaggersWebsite.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CustomLeaderboards",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    SpawnsetFileName = table.Column<string>(nullable: true),
                    SpawnsetHash = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomLeaderboards", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "CustomEntry",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Time = table.Column<float>(nullable: false),
                    Gems = table.Column<int>(nullable: false),
                    Kills = table.Column<int>(nullable: false),
                    DeathType = table.Column<int>(nullable: false),
                    Accuracy = table.Column<float>(nullable: false),
                    EnemiesAlive = table.Column<int>(nullable: false),
                    Homing = table.Column<int>(nullable: false),
                    LevelUpTime2 = table.Column<float>(nullable: false),
                    LevelUpTime3 = table.Column<float>(nullable: false),
                    LevelUpTime4 = table.Column<float>(nullable: false),
                    CustomLeaderboardID = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomEntry", x => x.ID);
                    table.ForeignKey(
                        name: "FK_CustomEntry_CustomLeaderboards_CustomLeaderboardID",
                        column: x => x.CustomLeaderboardID,
                        principalTable: "CustomLeaderboards",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CustomEntry_CustomLeaderboardID",
                table: "CustomEntry",
                column: "CustomLeaderboardID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CustomEntry");

            migrationBuilder.DropTable(
                name: "CustomLeaderboards");
        }
    }
}
