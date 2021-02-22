using DevilDaggersCore.Utils;
using DevilDaggersWebsite.Clients;
using DiscordBotDdInfo.Logging;
using Microsoft.AspNetCore.Hosting;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Threading.Tasks;
using BotLogger = DiscordBotDdInfo.Logging.DiscordLogger;

namespace DevilDaggersWebsite.Tasks
{
	public class CreateLeaderboardHistoryFileTask : AbstractTask
	{
		private readonly IWebHostEnvironment _env;

		public CreateLeaderboardHistoryFileTask(IWebHostEnvironment env)
		{
			_env = env;
		}

		public override string Schedule => "* * * * *";

		protected override async Task Execute()
		{
			await BotLogger.Instance.TryLog(LoggingChannel.Task, $"{nameof(CreateLeaderboardHistoryFileTask)} starting... Triggered: {LastTriggered}");

			string? historyFileName = GetHistoryFileNameFromDate(LastTriggered);
			if (historyFileName == null)
			{
				Dto.Leaderboard? lb = await DdHasmodaiClient.GetScores(1);
				if (lb != null)
				{
					string fileName = $"{DateTime.UtcNow:yyyyMMddHHmm}.json";
					File.WriteAllText(Path.Combine(_env.WebRootPath, "leaderboard-history", fileName), JsonConvert.SerializeObject(lb));
					await BotLogger.Instance.TryLog(LoggingChannel.Task, $"{nameof(CreateLeaderboardHistoryFileTask)} succeeded. '{fileName}' was created.");
				}
				else
				{
					await BotLogger.Instance.TryLog(LoggingChannel.Task, $"{nameof(CreateLeaderboardHistoryFileTask)} failed because the Devil Daggers servers didn't return a leaderboard.");
				}
			}
			else
			{
				await BotLogger.Instance.TryLog(LoggingChannel.Task, $"{nameof(CreateLeaderboardHistoryFileTask)} skipped because a file for today's history already exists ({historyFileName}).");
			}
		}

		private string? GetHistoryFileNameFromDate(DateTime dateTime)
		{
			foreach (string path in Directory.GetFiles(Path.Combine(_env.WebRootPath, "leaderboard-history"), "*.json"))
			{
				string fileName = Path.GetFileNameWithoutExtension(path);
				if (HistoryUtils.HistoryJsonFileNameToDateTime(fileName).Date == dateTime.Date)
					return fileName;
			}

			return null;
		}
	}
}
