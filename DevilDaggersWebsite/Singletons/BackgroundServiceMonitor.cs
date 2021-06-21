using DevilDaggersDiscordBot.Extensions;
using DSharpPlus.Entities;
using System;
using System.Collections.Concurrent;
using System.Linq;

namespace DevilDaggersWebsite.Singletons
{
	public class BackgroundServiceMonitor
	{
		private readonly ConcurrentBag<BackgroundServiceLog> _backgroundServiceLogs = new();

		public void Register(string name, TimeSpan interval)
			=> _backgroundServiceLogs.Add(new(name, interval));

		public void Update(string name, DateTime lastExecuted)
		{
			BackgroundServiceLog? backgroundServiceLog = _backgroundServiceLogs.FirstOrDefault(bsl => bsl.Name == name);
			if (backgroundServiceLog != null)
				backgroundServiceLog.LastExecuted = lastExecuted;
		}

		public DiscordEmbed? BuildDiscordEmbed()
		{
			if (_backgroundServiceLogs.Count == 0)
				return null;

			DiscordEmbedBuilder builder = new()
			{
				Title = $"Background service {DateTime.UtcNow:yyyy-MM-dd HH:mm:ss}",
				Color = DiscordColor.White,
			};
			foreach (BackgroundServiceLog bsl in _backgroundServiceLogs.OrderBy(bsl => bsl.Name))
				builder.AddFieldObject(bsl.Name, $"{nameof(BackgroundServiceLog.LastExecuted)} {bsl.LastExecuted:yyyy-MM-dd HH:mm:ss} | {nameof(BackgroundServiceLog.Interval)} {bsl.Interval:T}");

			return builder.Build();
		}

		private class BackgroundServiceLog
		{
			public BackgroundServiceLog(string name, TimeSpan interval)
			{
				Name = name;
				Interval = interval;
			}

			public string Name { get; }
			public TimeSpan Interval { get; }

			public DateTime LastExecuted { get; set; }
		}
	}
}
