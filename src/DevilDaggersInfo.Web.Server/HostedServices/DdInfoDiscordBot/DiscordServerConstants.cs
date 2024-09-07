using DSharpPlus;
using DSharpPlus.Entities;

namespace DevilDaggersInfo.Web.Server.HostedServices.DdInfoDiscordBot;

public static class DiscordServerConstants
{
	public const long TestChannelId = 975077574399131718;

	private static readonly Dictionary<Channel, ChannelWrapper> _channels = new()
	{
		{ Channel.MaintainersAuditLog, new ChannelWrapper(975077254046580828) },
		{ Channel.MonitoringCustomLeaderboardValid, new ChannelWrapper(975077530732208148) },
		{ Channel.MonitoringCustomLeaderboardInvalid, new ChannelWrapper(975077543575175228) },
		{ Channel.MonitoringLog, new ChannelWrapper(975077324468920421) },
		{ Channel.MonitoringTest, new ChannelWrapper(TestChannelId) },
		{ Channel.CustomLeaderboards, new ChannelWrapper(578316107836817418) },
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

	public static async Task LoadServerChannels(DiscordClient client)
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
