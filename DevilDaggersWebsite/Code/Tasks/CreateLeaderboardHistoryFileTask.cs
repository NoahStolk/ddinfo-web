using DevilDaggersCore.Leaderboards.History;
using DevilDaggersWebsite.Pages.Api;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Threading.Tasks;

namespace DevilDaggersWebsite.Code.Tasks
{
	public class CreateLeaderboardHistoryFileTask : AbstractTask
	{
		private readonly IHostingEnvironment _env;

		public override string Schedule => "0 0 * * *";

		public CreateLeaderboardHistoryFileTask(IHostingEnvironment env)
		{
			_env = env;
		}

		protected override async Task Execute()
		{
			if (!HistoryFileForThisDateExists(LastTriggered))
			{
				FileResult file = await new GetLeaderboardModel().OnGetAsync();
				File.WriteAllBytes(Path.Combine(_env.WebRootPath, "leaderboard-history", file.FileDownloadName), ((FileContentResult)file).FileContents);
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