﻿using DevilDaggersWebsite.BlazorWasm.Server.Extensions;
using DevilDaggersWebsite.BlazorWasm.Server.HostedServices.DdInfoDiscordBot;
using DevilDaggersWebsite.BlazorWasm.Server.Singletons;
using DSharpPlus.Entities;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DevilDaggersWebsite.BlazorWasm.Server.HostedServices
{
	public class FileSystemLoggerBackgroundService : AbstractBackgroundService
	{
		private readonly IWebHostEnvironment _environment;

		public FileSystemLoggerBackgroundService(IWebHostEnvironment environment, BackgroundServiceMonitor backgroundServiceMonitor, DiscordLogger discordLogger)
			: base(backgroundServiceMonitor, discordLogger)
		{
			_environment = environment;
		}

		protected override TimeSpan Interval => TimeSpan.FromMinutes(5);

		protected override async Task ExecuteTaskAsync(CancellationToken stoppingToken)
		{
			if (DevilDaggersInfoServerConstants.FileMessage == null)
				return;

			DirectoryStatistics leaderboardHistory = GetDirectorySize(Path.Combine(_environment.WebRootPath, "leaderboard-history"));
			DirectoryStatistics modScreenshots = GetDirectorySize(Path.Combine(_environment.WebRootPath, "mod-screenshots"));
			DirectoryStatistics mods = GetDirectorySize(Path.Combine(_environment.WebRootPath, "mods"));
			DirectoryStatistics spawnsets = GetDirectorySize(Path.Combine(_environment.WebRootPath, "spawnsets"));

			DiscordEmbedBuilder builder = new()
			{
				Title = $"File {DateTime.UtcNow:yyyy-MM-dd HH:mm:ss}",
				Color = DiscordColor.White,
			};
			AddFieldObject("leaderboard-history", leaderboardHistory);
			AddFieldObject("mod-screenshots", modScreenshots);
			AddFieldObject("mods", mods);
			AddFieldObject("spawnsets", spawnsets);

			await DiscordLogger.TryEditMessage(DevilDaggersInfoServerConstants.FileMessage, builder.Build());

			void AddFieldObject(string name, DirectoryStatistics value)
				=> builder.AddFieldObject(name, $"`{value.Size:n0}` bytes\n`{value.FileCount}` files");
		}

		private static DirectoryStatistics GetDirectorySize(string folderPath)
		{
			DirectoryInfo di = new(folderPath);
			IEnumerable<FileInfo> allFiles = di.EnumerateFiles("*.*", SearchOption.AllDirectories);
			return new()
			{
				Size = allFiles.Sum(fi => fi.Length),
				FileCount = allFiles.Count(),
			};
		}

		// TODO: Use .NET 6 record struct.
		private struct DirectoryStatistics
		{
			public long Size { get; init; }
			public int FileCount { get; init; }
		}
	}
}
