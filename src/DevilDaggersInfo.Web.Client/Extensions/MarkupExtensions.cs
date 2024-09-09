using DevilDaggersInfo.Web.ApiSpec.Main.CustomLeaderboards;
using Microsoft.AspNetCore.Components;

namespace DevilDaggersInfo.Web.Client.Extensions;

public static class MarkupExtensions
{
	public static MarkupString ToNoBreakString(this object? obj)
	{
		return (obj?.ToString()).ToNoBreakString();
	}

	public static MarkupString ToNoBreakString(this string? str)
	{
		return new MarkupString(str?.Replace(" ", "&nbsp;") ?? string.Empty);
	}

	public static string ToCssClass(this CustomLeaderboardDagger customLeaderboardDagger)
	{
		return customLeaderboardDagger.ToString().ToLower();
	}
}
