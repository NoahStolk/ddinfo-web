using DSharpPlus;
using DSharpPlus.Entities;

namespace DevilDaggersInfo.Web.Server.HostedServices.DdInfoDiscordBot;

public static class DiscordServerConstants
{
	public const long TestChannelId = 813508325705515008;

	public const ulong BotUserId = 645209987949395969;

	private static readonly Dictionary<Channel, ChannelWrapper> _channels = new()
	{
		{ Channel.MaintainersAuditLog, new(821489129615130684) },
		{ Channel.MonitoringCustomLeaderboardValid, new(813506112670007306) },
		{ Channel.MonitoringCustomLeaderboardInvalid, new(952210186859339816) },
		{ Channel.MonitoringLog, new(727227801664618607) },
		{ Channel.MonitoringTest, new(TestChannelId) },
		{ Channel.CustomLeaderboards, new(578316107836817418) },
	};

	/// <summary>
	/// Returns the channel based on the <paramref name="channel"/> enum. Always returns the channel for <see cref="Channel.MonitoringTest"/> when running in development.
	/// </summary>
	public static DiscordChannel? GetDiscordChannel(Channel channel, IWebHostEnvironment environment)
	{
		if (environment.IsDevelopment())
			channel = Channel.MonitoringTest;

		return _channels[channel].DiscordChannel;
	}

	public static async Task LoadServerChannelsAndMessages(DiscordClient client)
	{
		foreach (ChannelWrapper wrapper in _channels.Values)
		{
			if (wrapper.DiscordChannel == null)
				wrapper.DiscordChannel = await client.GetChannelAsync(wrapper.ChannelId);
		}
	}

	private sealed class ChannelWrapper
	{
		public ChannelWrapper(ulong channelId)
		{
			ChannelId = channelId;
		}

		public ulong ChannelId { get; }

		public DiscordChannel? DiscordChannel { get; set; }
	}
}
