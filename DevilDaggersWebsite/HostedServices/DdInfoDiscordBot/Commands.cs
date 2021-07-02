using DevilDaggersWebsite.Extensions;
using DSharpPlus.EventArgs;
using System;
using System.Collections.Generic;

namespace DevilDaggersWebsite.HostedServices.DdInfoDiscordBot
{
	public static class Commands
	{
		public static Dictionary<string, Action<MessageCreateEventArgs>> Actions { get; } = new()
		{
			{ ".bot", async (e) => await e.Channel.SendMessageAsyncSafe("Hi.") },
		};
	}
}
