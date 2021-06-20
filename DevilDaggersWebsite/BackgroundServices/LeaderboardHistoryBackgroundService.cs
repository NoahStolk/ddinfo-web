﻿using DevilDaggersCore.Utils;
using DevilDaggersDiscordBot;
using DevilDaggersWebsite.Clients;
using Microsoft.AspNetCore.Hosting;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using BotLogger = DevilDaggersDiscordBot.Logging.DiscordLogger;

namespace DevilDaggersWebsite.BackgroundServices
{
	public class LeaderboardHistoryBackgroundService : AbstractBackgroundService
	{
		private readonly IWebHostEnvironment _env;

		public LeaderboardHistoryBackgroundService(IWebHostEnvironment env)
		{
			_env = env;
		}

		protected override TimeSpan Interval => TimeSpan.FromMinutes(1);

		protected override async Task ExecuteTaskAsync(CancellationToken stoppingToken)
		{
			try
			{
				if (HistoryFileExistsForDate(DateTime.UtcNow))
					return;

				Dto.Leaderboard? lb = await LeaderboardClient.Instance.GetScores(1);
				if (lb != null)
				{
					string fileName = $"{DateTime.UtcNow:yyyyMMddHHmm}.json";
					File.WriteAllText(Path.Combine(_env.WebRootPath, "leaderboard-history", fileName), JsonConvert.SerializeObject(lb));
					await BotLogger.Instance.TryLog(Channel.TaskMonitoring, _env.EnvironmentName, $":white_check_mark: `{nameof(LeaderboardHistoryBackgroundService)}` succeeded. `{fileName}` was created.");
				}
				else
				{
					await BotLogger.Instance.TryLog(Channel.TaskMonitoring, _env.EnvironmentName, $":x: `{nameof(LeaderboardHistoryBackgroundService)}` failed because the Devil Daggers servers didn't return a leaderboard.");
				}
			}
			catch (Exception ex)
			{
				await BotLogger.Instance.TryLog(Channel.TaskMonitoring, _env.EnvironmentName, $":x: `{nameof(LeaderboardHistoryBackgroundService)}` failed with exception: `{ex.Message}`");
			}
		}

		private bool HistoryFileExistsForDate(DateTime dateTime)
		{
			foreach (string path in Directory.GetFiles(Path.Combine(_env.WebRootPath, "leaderboard-history"), "*.json"))
			{
				string fileName = Path.GetFileNameWithoutExtension(path);
				if (HistoryUtils.HistoryJsonFileNameToDateTime(fileName).Date == dateTime.Date)
					return true;
			}

			return false;
		}
	}
}
