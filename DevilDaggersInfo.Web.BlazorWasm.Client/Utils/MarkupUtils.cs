using Microsoft.AspNetCore.Components;

namespace DevilDaggersInfo.Web.BlazorWasm.Client.Utils;

public static class MarkupUtils
{
	public static readonly MarkupString NoDataMarkup = new(@"<span class=""text-no-data"">N/A</span>");

	public static MarkupString DeathString(byte deathType, GameVersion gameVersion = GameVersion.V3_1)
	{
		Death? death = Deaths.GetDeathByLeaderboardType(gameVersion, deathType);
		string style = $"color: {death?.Color.HexCode ?? "#444"};";
		string name = death?.Name ?? "Unknown";

		return new(@$"<span style=""{style}"" class=""font-goethe text-lg"">{name}</span>");
	}

	public static MarkupString UpgradeString(byte level, GameVersion gameVersion = GameVersion.V3_1)
	{
		Upgrade? upgrade = Upgrades.GetUpgrades(gameVersion).Find(u => u.Level == level);
		string style = $"color: {upgrade?.Color.HexCode ?? "#444"};";
		string name = upgrade?.Name ?? "Unknown";

		return new(@$"<span style=""{style}"" class=""font-goethe text-lg"">{name}</span>");
	}

	public static MarkupString EnemyString(string enemyName, GameVersion gameVersion = GameVersion.V3_1)
	{
		Enemy? enemy = Enemies.GetEnemies(gameVersion).Find(e => e.Name == enemyName);
		string style = $"color: {enemy?.Color.HexCode ?? "#444"};";
		string name = enemy?.Name ?? "Unknown";

		return new(@$"<span style=""{style}"" class=""font-goethe text-lg"">{name}</span>");
	}
}
