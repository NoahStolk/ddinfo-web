using DevilDaggersCore.Utils;
using DevilDaggersWebsite.Core.Clients;
using Microsoft.AspNetCore.Hosting;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Threading.Tasks;

namespace DevilDaggersWebsite.Core.Tasks
{
	public class CreateLeaderboardHistoryFileTask : AbstractTask
	{
		private readonly IWebHostEnvironment _env;

		public CreateLeaderboardHistoryFileTask(IWebHostEnvironment env)
		{
			_env = env;
		}

		public override string Schedule => "0 0 * * *";

		protected override async Task Execute()
		{
			if (!HistoryFileForThisDateExists(LastTriggered))
			{
				Dto.Leaderboard? lb = await DdHasmodaiClient.GetScores(1);
				if (lb != null)
					File.WriteAllText(Path.Combine(_env.WebRootPath, "leaderboard-history", $"{DateTime.UtcNow:yyyyMMddHHmm}.json"), JsonConvert.SerializeObject(lb));
			}
		}

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
	}
}