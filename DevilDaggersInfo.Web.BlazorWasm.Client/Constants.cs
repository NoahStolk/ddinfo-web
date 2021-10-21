using Microsoft.AspNetCore.Components;

namespace DevilDaggersInfo.Web.BlazorWasm.Client;

public static class Constants
{
#pragma warning disable S1075 // URIs should not be hardcoded
	public const string DiscordUrl = "https://discord.gg/NF32j8S";
#pragma warning restore S1075 // URIs should not be hardcoded

	public static readonly MarkupString NoDataMarkup = new(@"<span class=""text-no-data"">N/A</span>");
}
