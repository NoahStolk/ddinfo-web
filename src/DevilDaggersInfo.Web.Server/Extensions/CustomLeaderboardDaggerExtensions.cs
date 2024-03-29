using DevilDaggersInfo.Web.Server.Domain.Models.CustomLeaderboards;
using DevilDaggersInfo.Web.Server.HostedServices.DdInfoDiscordBot;
using DSharpPlus.Entities;

namespace DevilDaggersInfo.Web.Server.Extensions;

public static class CustomLeaderboardDaggerExtensions
{
	public static DiscordColor GetDiscordColor(this CustomLeaderboardDagger dagger)
	{
		return dagger switch
		{
			CustomLeaderboardDagger.Leviathan => DiscordColors.Leviathan,
			CustomLeaderboardDagger.Devil => DiscordColors.Devil,
			CustomLeaderboardDagger.Golden => DiscordColors.Golden,
			CustomLeaderboardDagger.Silver => DiscordColors.Silver,
			CustomLeaderboardDagger.Bronze => DiscordColors.Bronze,
			_ => DiscordColors.Default,
		};
	}
}
