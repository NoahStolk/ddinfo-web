using CoreBase.Services;
using DevilDaggersWebsite.Models.API;
using DevilDaggersWebsite.Models.Leaderboard;
using Microsoft.AspNetCore.Mvc;
using NetBase.Utils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Mime;

namespace DevilDaggersWebsite.Pages.API
{
	[ApiFunction(Description = "Returns all the world records found in the leaderboard history section of the site.", ReturnType = MediaTypeNames.Application.Json)]
	public class GetWorldRecordsModel : ApiPageModel
	{
		private readonly ICommonObjects _commonObjects;

		public GetWorldRecordsModel(ICommonObjects commonObjects)
		{
			_commonObjects = commonObjects;
		}

		public FileResult OnGet(bool formatted = false)
		{
			return JsonFile(GetWorldRecords(), formatted ? Formatting.Indented : Formatting.None);
		}
		
		public SortedDictionary<DateTime, Entry> GetWorldRecords()
		{
			SortedDictionary<DateTime, Entry> data = new SortedDictionary<DateTime, Entry>();

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

			return data;
		}
	}
}