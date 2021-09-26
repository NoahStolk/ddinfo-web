using Microsoft.AspNetCore.Components;

namespace DevilDaggersInfo.Web.BlazorWasm.Client.Utils;

public static class MarkupExtensions
{
	public static MarkupString ToNoBreakString(this object? obj)
		=> ToNoBreakString(obj?.ToString());

	public static MarkupString ToNoBreakString(this string? str)
		=> new(str?.Replace(" ", "&nbsp;") ?? string.Empty);
}
