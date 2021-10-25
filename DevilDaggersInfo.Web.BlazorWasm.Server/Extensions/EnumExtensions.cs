using DevilDaggersInfo.Web.BlazorWasm.Server.HostedServices.DdInfoDiscordBot;
using DSharpPlus.Entities;

namespace DevilDaggersInfo.Web.BlazorWasm.Server.Extensions;

public static class EnumExtensions
{
	public static DiscordColor GetDiscordColor(this CustomLeaderboardDagger dagger) => dagger switch
	{
		CustomLeaderboardDagger.Leviathan => DiscordColors.Leviathan,
		CustomLeaderboardDagger.Devil => DiscordColors.Devil,
		CustomLeaderboardDagger.Golden => DiscordColors.Golden,
		CustomLeaderboardDagger.Silver => DiscordColors.Silver,
		CustomLeaderboardDagger.Bronze => DiscordColors.Bronze,
		_ => DiscordColors.Default,
	};
}
