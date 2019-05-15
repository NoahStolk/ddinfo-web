using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DevilDaggersWebsite.Migrations
{
    public partial class V2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Accuracy",
                table: "CustomEntry");

            migrationBuilder.AddColumn<DateTime>(
                name: "DateTime",
                table: "CustomEntry",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "ShotsFired",
                table: "CustomEntry",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ShotsHit",
                table: "CustomEntry",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateTime",
                table: "CustomEntry");

            migrationBuilder.DropColumn(
                name: "ShotsFired",
                table: "CustomEntry");

            migrationBuilder.DropColumn(
                name: "ShotsHit",
                table: "CustomEntry");

            migrationBuilder.AddColumn<float>(
                name: "Accuracy",
                table: "CustomEntry",
                nullable: false,
                defaultValue: 0f);
        }
    }
}
