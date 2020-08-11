using DevilDaggersCore.Utils;
using DevilDaggersWebsite.Code.External;
using Microsoft.AspNetCore.Hosting;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Threading.Tasks;

namespace DevilDaggersWebsite.Code.Tasks
{
	public class CreateLeaderboardHistoryFileTask : AbstractTask
	{
		private readonly IWebHostEnvironment env;

		public override string Schedule => "0 0 * * *";

		public CreateLeaderboardHistoryFileTask(IWebHostEnvironment env)
		{
			this.env = env;
		}

		protected override async Task Execute()
		{
			if (!HistoryFileForThisDateExists(LastTriggered))
				File.WriteAllText(Path.Combine(env.WebRootPath, "leaderboard-history", $"{DateTime.UtcNow:yyyyMMddHHmm}.json"), JsonConvert.SerializeObject(await Hasmodai.GetScores(1)));
		}

		private bool HistoryFileForThisDateExists(DateTime dateTime)
		{
			foreach (string path in Directory.GetFiles(Path.Combine(env.WebRootPath, "leaderboard-history"), "*.json"))
			{
				string fileName = Path.GetFileNameWithoutExtension(path);
				if (HistoryUtils.HistoryJsonFileNameToDateTime(fileName).Date == dateTime.Date)
					return true;
			}

			return false;
		}
	}
}