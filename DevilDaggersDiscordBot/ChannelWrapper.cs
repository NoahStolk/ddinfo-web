using DSharpPlus.Entities;

namespace DevilDaggersDiscordBot
{
	public class ChannelWrapper
	{
		public ChannelWrapper(ulong channelId)
		{
			ChannelId = channelId;
		}

		public ulong ChannelId { get; }

		public DiscordChannel? DiscordChannel { get; set; }
	}
}
