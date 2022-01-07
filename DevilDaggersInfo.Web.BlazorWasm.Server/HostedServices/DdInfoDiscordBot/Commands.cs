using DSharpPlus.Entities;
using DSharpPlus.EventArgs;

namespace DevilDaggersInfo.Web.BlazorWasm.Server.HostedServices.DdInfoDiscordBot;

public static class Commands
{
	public static Dictionary<string, Action<MessageCreateEventArgs>> Actions { get; } = new()
	{
		{ ".bot", async (e) => await e.Channel.SendMessageAsyncSafe("Hi.") },
		{ ".default", async (e) => await e.Channel.SendMessageAsyncSafe(null, CreateDaggerEmbed(CustomLeaderboardDagger.Default)) },
		{ ".bronze", async (e) => await e.Channel.SendMessageAsyncSafe(null, CreateDaggerEmbed(CustomLeaderboardDagger.Bronze)) },
		{ ".silver", async (e) => await e.Channel.SendMessageAsyncSafe(null, CreateDaggerEmbed(CustomLeaderboardDagger.Silver)) },
		{ ".golden", async (e) => await e.Channel.SendMessageAsyncSafe(null, CreateDaggerEmbed(CustomLeaderboardDagger.Golden)) },
		{ ".devil", async (e) => await e.Channel.SendMessageAsyncSafe(null, CreateDaggerEmbed(CustomLeaderboardDagger.Devil)) },
		{ ".leviathan", async (e) => await e.Channel.SendMessageAsyncSafe(null, CreateDaggerEmbed(CustomLeaderboardDagger.Leviathan)) },
	};

	private static DiscordEmbed CreateDaggerEmbed(CustomLeaderboardDagger dagger)
	{
		DiscordEmbedBuilder builder = new()
		{
			Title = dagger.ToString(),
			Color = dagger.GetDiscordColor(),
		};

		return builder.Build();
	}
}
