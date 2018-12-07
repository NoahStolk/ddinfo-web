using CoreBase.Services;
using DevilDaggersWebsite.Models.API;
using DevilDaggersWebsite.Models.Leaderboard;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NetBase.Utils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Mime;
using System.Text;

namespace DevilDaggersWebsite.Pages.API
{
	[ApiFunction(Description = "Returns all the world records found in the leaderboard history section of the site.", ReturnType = MediaTypeNames.Application.Json)]
	public class GetWorldRecordsModel : PageModel
	{
		private readonly ICommonObjects _commonObjects;

		public GetWorldRecordsModel(ICommonObjects commonObjects)
		{
			_commonObjects = commonObjects;
		}

		public FileResult OnGet()
		{
			Dictionary<DateTime, Entry> data = new Dictionary<DateTime, Entry>();
			int worldRecord = 0;

			foreach (string leaderboardHistoryPath in Directory.GetFiles(Path.Combine(_commonObjects.Env.WebRootPath, "leaderboard-history")))
			{
				Models.Leaderboard.Leaderboard leaderboard = JsonConvert.DeserializeObject<Models.Leaderboard.Leaderboard>(FileUtils.GetContents(leaderboardHistoryPath));
				if (leaderboard.Entries[0].Time != worldRecord)
				{
					worldRecord = leaderboard.Entries[0].Time;
					data[leaderboard.DateTime] = leaderboard.Entries[0];
				}
			}

			string jsonResult = JsonConvert.SerializeObject(data);
			return File(Encoding.UTF8.GetBytes(jsonResult), MediaTypeNames.Application.Json, $"{GetType().Name.Replace("Model", "")}.json");
		}
	}
}