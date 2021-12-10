using Microsoft.AspNetCore.Components;

namespace DevilDaggersInfo.Web.BlazorWasm.Client.Utils;

public static class MarkupUtils
{
	public static readonly MarkupString NoDataMarkup = new(@"<span style=""color: #666;"">N/A</span>");

	public static MarkupString DaggerString(Dagger dagger)
	{
		return new(@$"<span class=""font-goethe text-lg {dagger.Name.ToLower()}"">{dagger.Name} Dagger</span>");
	}

	public static MarkupString DeathString(byte deathType, GameVersion gameVersion = GameConstants.CurrentVersion, string textSizeClass = "text-lg")
	{
		Death? death = Deaths.GetDeathByLeaderboardType(gameVersion, deathType);
		string style = $"color: {death?.Color.HexCode ?? "#444"};";
		string name = death?.Name ?? "Unknown";

		return new(@$"<span style=""{style}"" class=""font-goethe {textSizeClass}"">{name}</span>");
	}

	public static MarkupString UpgradeString(Upgrade upgrade)
	{
		string style = $"color: {upgrade.Color.HexCode};";
		string name = upgrade.Name;

		return new(@$"<span style=""{style}"" class=""font-goethe text-lg"">{name}</span>");
	}

	public static MarkupString EnemyString(Enemy enemy, bool plural = false)
	{
		string style = $"color: {enemy.Color.HexCode};";
		string name = enemy.Name;

		return new(@$"<span style=""{style}"" class=""font-goethe text-lg"">{name}{(plural ? "s" : string.Empty)}</span>");
	}
}
