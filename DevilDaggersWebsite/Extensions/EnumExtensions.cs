using DevilDaggersWebsite.Enumerators;
using DevilDaggersWebsite.HostedServices.DdInfoDiscordBot;
using DSharpPlus.Entities;

namespace DevilDaggersWebsite.Extensions
{
	public static class EnumExtensions
	{
		public static char GetChar(this Currency currency) => currency switch
		{
			Currency.Eur => '€',
			Currency.Usd => '$',
			Currency.Aud => '$',
			Currency.Gbp => '£',
			Currency.Sgd => '$',
			_ => '?',
		};

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
}
