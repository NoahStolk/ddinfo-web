using Microsoft.EntityFrameworkCore.Migrations;

namespace DevilDaggersWebsite.Migrations
{
    public partial class V3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CustomEntry_CustomLeaderboards_CustomLeaderboardID",
                table: "CustomEntry");

            migrationBuilder.RenameColumn(
                name: "DateTime",
                table: "CustomEntry",
                newName: "SubmitDate");

            migrationBuilder.AlterColumn<int>(
                name: "CustomLeaderboardID",
                table: "CustomEntry",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Username",
                table: "CustomEntry",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_CustomEntry_CustomLeaderboards_CustomLeaderboardID",
                table: "CustomEntry",
                column: "CustomLeaderboardID",
                principalTable: "CustomLeaderboards",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CustomEntry_CustomLeaderboards_CustomLeaderboardID",
                table: "CustomEntry");

            migrationBuilder.DropColumn(
                name: "Username",
                table: "CustomEntry");

            migrationBuilder.RenameColumn(
                name: "SubmitDate",
                table: "CustomEntry",
                newName: "DateTime");

            migrationBuilder.AlterColumn<int>(
                name: "CustomLeaderboardID",
                table: "CustomEntry",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_CustomEntry_CustomLeaderboards_CustomLeaderboardID",
                table: "CustomEntry",
                column: "CustomLeaderboardID",
                principalTable: "CustomLeaderboards",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
