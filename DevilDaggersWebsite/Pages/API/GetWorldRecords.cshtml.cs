using CoreBase.Services;
using DevilDaggersCore.Game;
using DevilDaggersCore.Leaderboard;
using DevilDaggersUtilities.Website;
using DevilDaggersWebsite.Models.API;
using DevilDaggersWebsite.PageModels;
using Microsoft.AspNetCore.Mvc;
using NetBase.Utils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Mime;

namespace DevilDaggersWebsite.Pages.API
{
	[ApiFunction(Description = "Returns the world record found in the leaderboard history section of the site at the time of the given date parameter (format: yyyy-MM-dd). Returns all the world records if no date parameter was specified or if the parameter was incorrect.", ReturnType = MediaTypeNames.Application.Json)]
	public class GetWorldRecordsModel : ApiPageModel
	{
		private readonly ICommonObjects _commonObjects;

		public GetWorldRecordsModel(ICommonObjects commonObjects)
		{
			_commonObjects = commonObjects;
		}

		public FileResult OnGet(DateTime? date = null, bool formatted = false)
		{
			return JsonFile(GetWorldRecords(date), formatted ? Formatting.Indented : Formatting.None);
		}

		public SortedDictionary<DateTime, Entry> GetWorldRecords(DateTime? date = null)
		{
			bool dateValid = date != null && date > Game.GameVersions["V1"].ReleaseDate && date <= DateTime.Now;

			SortedDictionary<DateTime, Entry> data = new SortedDictionary<DateTime, Entry>();

			int worldRecord = 0;
			foreach (string leaderboardHistoryPath in Directory.GetFiles(Path.Combine(_commonObjects.Env.WebRootPath, "leaderboard-history")))
			{
				DevilDaggersCore.Leaderboard.Leaderboard leaderboard = JsonConvert.DeserializeObject<DevilDaggersCore.Leaderboard.Leaderboard>(FileUtils.GetContents(leaderboardHistoryPath));
				if (leaderboard.Entries[0].Time != worldRecord)
				{
					worldRecord = leaderboard.Entries[0].Time;
					if (dateValid)
					{
						// Ugly and could be optimised but whatever
						if (LeaderboardHistoryUtils.HistoryJsonFileNameToDateTime(Path.GetFileNameWithoutExtension(leaderboardHistoryPath)) > date)
							break;
						data.Clear();
						data[leaderboard.DateTime] = leaderboard.Entries[0];
					}
					else
					{
						data[leaderboard.DateTime] = leaderboard.Entries[0];
					}
				}
			}

			return data;
		}
	}
}