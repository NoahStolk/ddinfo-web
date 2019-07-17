using CoreBase.Services;
using DevilDaggersCore.Leaderboard;
using DevilDaggersWebsite.Code.API;
using DevilDaggersWebsite.Code.PageModels;
using Microsoft.AspNetCore.Mvc;
using NetBase.Utils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mime;

namespace DevilDaggersWebsite.Pages.API
{
	[ApiFunction(Description = "Returns the user progression found in the leaderboard history section of the site corresponding to the given userID parameter.", ReturnType = MediaTypeNames.Application.Json)]
	public class GetUserProgressionByIDModel : ApiPageModel
	{
		private readonly ICommonObjects _commonObjects;

		public GetUserProgressionByIDModel(ICommonObjects commonObjects)
		{
			_commonObjects = commonObjects;
		}

		public FileResult OnGet(int userID, bool formatted = false)
		{
			return JsonFile(GetUserProgression(userID), formatted ? Formatting.Indented : Formatting.None);
		}

		public SortedDictionary<DateTime, Entry> GetUserProgression(int userID)
		{
			SortedDictionary<DateTime, Entry> data = new SortedDictionary<DateTime, Entry>();

			if (userID != 0)
			{
				foreach (string leaderboardHistoryPath in Directory.GetFiles(Path.Combine(_commonObjects.Env.WebRootPath, "leaderboard-history"), "*.json"))
				{
					DevilDaggersCore.Leaderboard.Leaderboard leaderboard = JsonConvert.DeserializeObject<DevilDaggersCore.Leaderboard.Leaderboard>(FileUtils.GetContents(leaderboardHistoryPath));
					Entry entry = leaderboard.Entries.Where(e => e.ID == userID).FirstOrDefault();

					if (entry != null && !data.Values.Any(e =>
						e.Time == entry.Time ||
						e.Time == entry.Time + 1 ||
						e.Time == entry.Time - 1)) // Off-by-one errors in the history
					{
						data[leaderboard.DateTime] = entry;
					}
				}
			}

			return data;
		}
	}
}