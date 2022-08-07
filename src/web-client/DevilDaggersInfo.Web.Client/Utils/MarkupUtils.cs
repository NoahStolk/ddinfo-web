using DevilDaggersInfo.Core.Wiki.Enums;
using DevilDaggersInfo.Razor.Core.Components;
using Microsoft.AspNetCore.Components;

namespace DevilDaggersInfo.Web.Client.Utils;

public static class MarkupUtils
{
	public static MarkupString DaggerString(Dagger dagger)
	{
		return new($@"<span class=""font-goethe text-lg {dagger.Name.ToLower()}"">{dagger.Name} Dagger</span>");
	}

	public static MarkupString CustomLeaderboardDeathString(byte? deathType, GameVersion gameVersion = GameConstants.CurrentVersion, string textSizeClass = "text-lg")
	{
		if (!deathType.HasValue)
			return new($@"<span style=""color: {MarkupStrings.NoDataColor};"" class=""font-goethe {textSizeClass}"">N/A</span>");

		return DeathString(deathType.Value, gameVersion, textSizeClass);
	}

	public static MarkupString DeathString(byte deathType, GameVersion gameVersion = GameConstants.CurrentVersion, string textSizeClass = "text-lg")
	{
		Death? death = Deaths.GetDeathByLeaderboardType(gameVersion, deathType);
		return DeathString(death, textSizeClass);
	}

	public static MarkupString DeathString(Death? death, string textSizeClass = "text-lg")
	{
		string style = $"color: {death?.Color.HexCode ?? MarkupStrings.NoDataColor};";
		return new($@"<span style=""{style}"" class=""font-goethe {textSizeClass}"">{death?.Name ?? "Unknown"}</span>");
	}

	public static MarkupString UpgradeString(Upgrade upgrade)
	{
		string style = $"color: {upgrade.Color.HexCode};";
		return new($@"<span style=""{style}"" class=""font-goethe text-lg"">{upgrade.Name}</span>");
	}

	public static MarkupString EnemyString(Enemy enemy, bool plural = false)
	{
		string style = $"color: {enemy.Color.HexCode};";
		return new($@"<span style=""{style}"" class=""font-goethe text-lg"">{enemy.Name}{(plural ? "s" : string.Empty)}</span>");
	}

	public static MarkupString LeaderboardTime(double timeInSeconds)
	{
		return new($@"<span class=""{Daggers.GetDaggerFromSeconds(GameConstants.CurrentVersion, timeInSeconds).Name.ToLower()} font-goethe text-lg"">{timeInSeconds.ToString(StringFormats.TimeFormat)}</span>");
	}

	public static MarkupString ProhibitedString(bool isProhibited)
	{
		if (isProhibited)
			return new($"<span class='text-orange'>Prohibited</span>");

		return new($"<span class='text-green'>OK</span>");
	}
}
