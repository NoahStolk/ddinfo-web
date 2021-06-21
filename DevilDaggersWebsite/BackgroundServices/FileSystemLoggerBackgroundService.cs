using DevilDaggersDiscordBot;
using DevilDaggersDiscordBot.Extensions;
using DSharpPlus.Entities;
using Microsoft.AspNetCore.Hosting;
using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DevilDaggersWebsite.BackgroundServices
{
	public class FileSystemLoggerBackgroundService : AbstractBackgroundService
	{
		public FileSystemLoggerBackgroundService(IWebHostEnvironment environment)
			: base(environment)
		{
		}

		protected override TimeSpan Interval => TimeSpan.FromMinutes(5);

		protected override async Task ExecuteTaskAsync(CancellationToken stoppingToken)
		{
			if (ServerConstants.FileMessage == null)
				return;

			long leaderboardHistorySize = GetDirectorySize(Path.Combine(Environment.WebRootPath, "leaderboard-history"));
			long modScreenshotsSize = GetDirectorySize(Path.Combine(Environment.WebRootPath, "mod-screenshots"));
			long modsSize = GetDirectorySize(Path.Combine(Environment.WebRootPath, "mods"));
			long spawnsetsSize = GetDirectorySize(Path.Combine(Environment.WebRootPath, "spawnsets"));

			DiscordEmbedBuilder builder = new()
			{
				Title = $"File {DateTime.UtcNow:yyyy-MM-dd HH:mm:ss}",
				Color = DiscordColor.White,
			};
			builder.AddFieldObject("leaderboard-history", $"{leaderboardHistorySize:n0} bytes");
			builder.AddFieldObject("mod-screenshots", $"{modScreenshotsSize:n0} bytes");
			builder.AddFieldObject("mods", $"{modsSize:n0} bytes");
			builder.AddFieldObject("spawnsets", $"{spawnsetsSize:n0} bytes");

			await DiscordLogger.EditMessage(ServerConstants.FileMessage, builder.Build());
		}

		private static long GetDirectorySize(string folderPath)
		{
			DirectoryInfo di = new(folderPath);
			return di.EnumerateFiles("*.*", SearchOption.AllDirectories).Sum(fi => fi.Length);
		}
	}
}
