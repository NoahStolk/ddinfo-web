using DevilDaggersInfo.Types.Web;
using Microsoft.AspNetCore.Components;

namespace DevilDaggersInfo.Web.Client.Extensions;

public static class MarkupExtensions
{
	public static MarkupString ToNoBreakString(this object? obj)
		=> (obj?.ToString()).ToNoBreakString();

	public static MarkupString ToNoBreakString(this string? str)
		=> new(str?.Replace(" ", "&nbsp;") ?? string.Empty);

	public static string ToCssClass(this CustomLeaderboardDagger customLeaderboardDagger)
		=> customLeaderboardDagger.ToString().ToLower();
}
