#if !DEBUG
using DevilDaggersCore.Utils;
using DevilDaggersWebsite.Clients;
using Microsoft.AspNetCore.Hosting;
using Newtonsoft.Json;
using System;
using System.IO;
#endif
using System.Threading.Tasks;

namespace DevilDaggersWebsite.Tasks
{
	public class CreateLeaderboardHistoryFileTask : AbstractTask
	{
#if !DEBUG
		private readonly IWebHostEnvironment _env;

		public CreateLeaderboardHistoryFileTask(IWebHostEnvironment env)
		{
			_env = env;
		}
#endif

		public override string Schedule => "0 0 * * *";

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
		protected override async Task Execute()
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
		{
#if !DEBUG
			if (!HistoryFileForThisDateExists(LastTriggered))
			{
				Dto.Leaderboard? lb = await DdHasmodaiClient.GetScores(1);
				if (lb != null)
					File.WriteAllText(Path.Combine(_env.WebRootPath, "leaderboard-history", $"{DateTime.UtcNow:yyyyMMddHHmm}.json"), JsonConvert.SerializeObject(lb));
			}
#endif
		}

#if !DEBUG
		private bool HistoryFileForThisDateExists(DateTime dateTime)
		{
			foreach (string path in Directory.GetFiles(Path.Combine(_env.WebRootPath, "leaderboard-history"), "*.json"))
			{
				string fileName = Path.GetFileNameWithoutExtension(path);
				if (HistoryUtils.HistoryJsonFileNameToDateTime(fileName).Date == dateTime.Date)
					return true;
			}

			return false;
		}
#endif
	}
}
