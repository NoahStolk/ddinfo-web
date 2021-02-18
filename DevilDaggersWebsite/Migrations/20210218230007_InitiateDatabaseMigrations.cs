using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DevilDaggersWebsite.Migrations
{
    public partial class InitiateDatabaseMigrations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(255) CHARACTER SET utf8mb4", nullable: false),
                    Name = table.Column<string>(type: "varchar(256) CHARACTER SET utf8mb4", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "varchar(256) CHARACTER SET utf8mb4", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(255) CHARACTER SET utf8mb4", nullable: false),
                    UserName = table.Column<string>(type: "varchar(256) CHARACTER SET utf8mb4", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "varchar(256) CHARACTER SET utf8mb4", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "varchar(256) CHARACTER SET utf8mb4", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "varchar(256) CHARACTER SET utf8mb4", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    PasswordHash = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    SecurityStamp = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    PhoneNumber = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetime(6)", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AssetMods",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    AssetModTypes = table.Column<int>(type: "int", nullable: false),
                    AssetModFileContents = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: false),
                    Url = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssetMods", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Players",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    PlayerName = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: false),
                    IsAnonymous = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    CountryCode = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    Dpi = table.Column<int>(type: "int", nullable: true),
                    InGameSens = table.Column<float>(type: "float", nullable: true),
                    Fov = table.Column<int>(type: "int", nullable: true),
                    RightHanded = table.Column<bool>(type: "tinyint(1)", nullable: true),
                    FlashEnabled = table.Column<bool>(type: "tinyint(1)", nullable: true),
                    Gamma = table.Column<float>(type: "float", nullable: true),
                    IsBanned = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    BanDescription = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    BanResponsibleId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Players", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Titles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Titles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ToolStatistics",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ToolName = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: false),
                    VersionNumber = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: false),
                    DownloadCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ToolStatistics", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    RoleId = table.Column<string>(type: "varchar(255) CHARACTER SET utf8mb4", nullable: false),
                    ClaimType = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    ClaimValue = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<string>(type: "varchar(255) CHARACTER SET utf8mb4", nullable: false),
                    ClaimType = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    ClaimValue = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "varchar(128) CHARACTER SET utf8mb4", maxLength: 128, nullable: false),
                    ProviderKey = table.Column<string>(type: "varchar(128) CHARACTER SET utf8mb4", maxLength: 128, nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    UserId = table.Column<string>(type: "varchar(255) CHARACTER SET utf8mb4", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "varchar(255) CHARACTER SET utf8mb4", nullable: false),
                    RoleId = table.Column<string>(type: "varchar(255) CHARACTER SET utf8mb4", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "varchar(255) CHARACTER SET utf8mb4", nullable: false),
                    LoginProvider = table.Column<string>(type: "varchar(128) CHARACTER SET utf8mb4", maxLength: 128, nullable: false),
                    Name = table.Column<string>(type: "varchar(128) CHARACTER SET utf8mb4", maxLength: 128, nullable: false),
                    Value = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Donations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    PlayerId = table.Column<int>(type: "int", nullable: false),
                    Amount = table.Column<int>(type: "int", nullable: false),
                    Currency = table.Column<int>(type: "int", nullable: false),
                    ConvertedEuroCentsReceived = table.Column<int>(type: "int", nullable: false),
                    DateReceived = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Note = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    IsRefunded = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Donations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Donations_Players_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "Players",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PlayerAssetMods",
                columns: table => new
                {
                    PlayerId = table.Column<int>(type: "int", nullable: false),
                    AssetModId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlayerAssetMods", x => new { x.PlayerId, x.AssetModId });
                    table.ForeignKey(
                        name: "FK_PlayerAssetMods_AssetMods_AssetModId",
                        column: x => x.AssetModId,
                        principalTable: "AssetMods",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PlayerAssetMods_Players_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "Players",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SpawnsetFiles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: false),
                    PlayerId = table.Column<int>(type: "int", nullable: false),
                    MaxDisplayWaves = table.Column<int>(type: "int", nullable: true),
                    HtmlDescription = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    LastUpdated = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SpawnsetFiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SpawnsetFiles_Players_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "Players",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PlayerTitles",
                columns: table => new
                {
                    PlayerId = table.Column<int>(type: "int", nullable: false),
                    TitleId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlayerTitles", x => new { x.PlayerId, x.TitleId });
                    table.ForeignKey(
                        name: "FK_PlayerTitles_Players_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "Players",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PlayerTitles_Titles_TitleId",
                        column: x => x.TitleId,
                        principalTable: "Titles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CustomLeaderboards",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Category = table.Column<int>(type: "int", nullable: false),
                    SpawnsetFileId = table.Column<int>(type: "int", nullable: false),
                    TimeBronze = table.Column<int>(type: "int", nullable: false),
                    TimeSilver = table.Column<int>(type: "int", nullable: false),
                    TimeGolden = table.Column<int>(type: "int", nullable: false),
                    TimeDevil = table.Column<int>(type: "int", nullable: false),
                    TimeLeviathan = table.Column<int>(type: "int", nullable: false),
                    DateLastPlayed = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    DateCreated = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    TotalRunsSubmitted = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomLeaderboards", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CustomLeaderboards_SpawnsetFiles_SpawnsetFileId",
                        column: x => x.SpawnsetFileId,
                        principalTable: "SpawnsetFiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CustomEntries",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CustomLeaderboardId = table.Column<int>(type: "int", nullable: false),
                    PlayerId = table.Column<int>(type: "int", nullable: false),
                    Time = table.Column<int>(type: "int", nullable: false),
                    GemsCollected = table.Column<int>(type: "int", nullable: false),
                    EnemiesKilled = table.Column<int>(type: "int", nullable: false),
                    DaggersFired = table.Column<int>(type: "int", nullable: false),
                    DaggersHit = table.Column<int>(type: "int", nullable: false),
                    EnemiesAlive = table.Column<int>(type: "int", nullable: false),
                    HomingDaggers = table.Column<int>(type: "int", nullable: false),
                    GemsDespawned = table.Column<int>(type: "int", nullable: false),
                    GemsEaten = table.Column<int>(type: "int", nullable: false),
                    GemsTotal = table.Column<int>(type: "int", nullable: false),
                    DeathType = table.Column<byte>(type: "tinyint unsigned", nullable: false),
                    LevelUpTime2 = table.Column<int>(type: "int", nullable: false),
                    LevelUpTime3 = table.Column<int>(type: "int", nullable: false),
                    LevelUpTime4 = table.Column<int>(type: "int", nullable: false),
                    SubmitDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    ClientVersion = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    GemsCollectedData = table.Column<byte[]>(type: "longblob", nullable: true),
                    EnemiesKilledData = table.Column<byte[]>(type: "longblob", nullable: true),
                    DaggersFiredData = table.Column<byte[]>(type: "longblob", nullable: true),
                    DaggersHitData = table.Column<byte[]>(type: "longblob", nullable: true),
                    EnemiesAliveData = table.Column<byte[]>(type: "longblob", nullable: true),
                    HomingDaggersData = table.Column<byte[]>(type: "longblob", nullable: true),
                    GemsDespawnedData = table.Column<byte[]>(type: "longblob", nullable: true),
                    GemsEatenData = table.Column<byte[]>(type: "longblob", nullable: true),
                    GemsTotalData = table.Column<byte[]>(type: "longblob", nullable: true),
                    Skull1sAliveData = table.Column<byte[]>(type: "longblob", nullable: true),
                    Skull2sAliveData = table.Column<byte[]>(type: "longblob", nullable: true),
                    Skull3sAliveData = table.Column<byte[]>(type: "longblob", nullable: true),
                    SpiderlingsAliveData = table.Column<byte[]>(type: "longblob", nullable: true),
                    Skull4sAliveData = table.Column<byte[]>(type: "longblob", nullable: true),
                    Squid1sAliveData = table.Column<byte[]>(type: "longblob", nullable: true),
                    Squid2sAliveData = table.Column<byte[]>(type: "longblob", nullable: true),
                    Squid3sAliveData = table.Column<byte[]>(type: "longblob", nullable: true),
                    CentipedesAliveData = table.Column<byte[]>(type: "longblob", nullable: true),
                    GigapedesAliveData = table.Column<byte[]>(type: "longblob", nullable: true),
                    Spider1sAliveData = table.Column<byte[]>(type: "longblob", nullable: true),
                    Spider2sAliveData = table.Column<byte[]>(type: "longblob", nullable: true),
                    LeviathansAliveData = table.Column<byte[]>(type: "longblob", nullable: true),
                    OrbsAliveData = table.Column<byte[]>(type: "longblob", nullable: true),
                    ThornsAliveData = table.Column<byte[]>(type: "longblob", nullable: true),
                    GhostpedesAliveData = table.Column<byte[]>(type: "longblob", nullable: true),
                    SpiderEggsAliveData = table.Column<byte[]>(type: "longblob", nullable: true),
                    Skull1sKilledData = table.Column<byte[]>(type: "longblob", nullable: true),
                    Skull2sKilledData = table.Column<byte[]>(type: "longblob", nullable: true),
                    Skull3sKilledData = table.Column<byte[]>(type: "longblob", nullable: true),
                    SpiderlingsKilledData = table.Column<byte[]>(type: "longblob", nullable: true),
                    Skull4sKilledData = table.Column<byte[]>(type: "longblob", nullable: true),
                    Squid1sKilledData = table.Column<byte[]>(type: "longblob", nullable: true),
                    Squid2sKilledData = table.Column<byte[]>(type: "longblob", nullable: true),
                    Squid3sKilledData = table.Column<byte[]>(type: "longblob", nullable: true),
                    CentipedesKilledData = table.Column<byte[]>(type: "longblob", nullable: true),
                    GigapedesKilledData = table.Column<byte[]>(type: "longblob", nullable: true),
                    Spider1sKilledData = table.Column<byte[]>(type: "longblob", nullable: true),
                    Spider2sKilledData = table.Column<byte[]>(type: "longblob", nullable: true),
                    LeviathansKilledData = table.Column<byte[]>(type: "longblob", nullable: true),
                    OrbsKilledData = table.Column<byte[]>(type: "longblob", nullable: true),
                    ThornsKilledData = table.Column<byte[]>(type: "longblob", nullable: true),
                    GhostpedesKilledData = table.Column<byte[]>(type: "longblob", nullable: true),
                    SpiderEggsKilledData = table.Column<byte[]>(type: "longblob", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomEntries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CustomEntries_CustomLeaderboards_CustomLeaderboardId",
                        column: x => x.CustomLeaderboardId,
                        principalTable: "CustomLeaderboards",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CustomEntries_Players_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "Players",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CustomEntries_CustomLeaderboardId",
                table: "CustomEntries",
                column: "CustomLeaderboardId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomEntries_PlayerId",
                table: "CustomEntries",
                column: "PlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomLeaderboards_SpawnsetFileId",
                table: "CustomLeaderboards",
                column: "SpawnsetFileId");

            migrationBuilder.CreateIndex(
                name: "IX_Donations_PlayerId",
                table: "Donations",
                column: "PlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_PlayerAssetMods_AssetModId",
                table: "PlayerAssetMods",
                column: "AssetModId");

            migrationBuilder.CreateIndex(
                name: "IX_PlayerTitles_TitleId",
                table: "PlayerTitles",
                column: "TitleId");

            migrationBuilder.CreateIndex(
                name: "IX_SpawnsetFiles_PlayerId",
                table: "SpawnsetFiles",
                column: "PlayerId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "CustomEntries");

            migrationBuilder.DropTable(
                name: "Donations");

            migrationBuilder.DropTable(
                name: "PlayerAssetMods");

            migrationBuilder.DropTable(
                name: "PlayerTitles");

            migrationBuilder.DropTable(
                name: "ToolStatistics");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "CustomLeaderboards");

            migrationBuilder.DropTable(
                name: "AssetMods");

            migrationBuilder.DropTable(
                name: "Titles");

            migrationBuilder.DropTable(
                name: "SpawnsetFiles");

            migrationBuilder.DropTable(
                name: "Players");
        }
    }
}
