using Microsoft.AspNetCore.Components;

namespace DevilDaggersInfo.Web.BlazorWasm.Client.Utils;

public static class MarkupUtils
{
	public static readonly MarkupString NoDataMarkup = new(@"<span class=""text-no-data"">N/A</span>");

	public static MarkupString DeathString(byte deathType, GameVersion gameVersion = GameVersion.V3_1)
	{
		Death? death = Deaths.GetDeathByLeaderboardType(gameVersion, deathType);
		string deathStyle = $"color: {death?.Color.HexCode ?? "#444"};";
		string deathName = death?.Name ?? "Unknown";

		return new(@$"<span style=""{deathStyle}"" class=""font-goethe text-lg"">{deathName}</span>");
	}
}
