using CoreBase.Services;
using DevilDaggersCore.Game;
using DevilDaggersCore.Leaderboards;
using DevilDaggersUtilities.Website;
using DevilDaggersWebsite.Code.API;
using DevilDaggersWebsite.Code.PageModels;
using Microsoft.AspNetCore.Mvc;
using NetBase.Utils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Mime;

namespace DevilDaggersWebsite.Pages.API
{
	[ApiFunction(Description = "Returns the world record found in the leaderboard history section of the site at the time of the given date parameter. Returns all the world records if no date parameter was specified or if the parameter was incorrect.", ReturnType = MediaTypeNames.Application.Json)]
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

		public List<WorldRecord> GetWorldRecords(DateTime? date = null)
		{
			bool isDateParameterValid = date != null && date >= GameInfo.GameVersions["V1"].ReleaseDate && date <= DateTime.Now;

			List<WorldRecord> data = new List<WorldRecord>();

			int worldRecord = 0;
			foreach (string leaderboardHistoryPath in Directory.GetFiles(Path.Combine(_commonObjects.Env.WebRootPath, "leaderboard-history"), "*.json"))
			{
				DevilDaggersCore.Leaderboards.Leaderboard leaderboard = JsonConvert.DeserializeObject<DevilDaggersCore.Leaderboards.Leaderboard>(FileUtils.GetContents(leaderboardHistoryPath));
				if (leaderboard.Entries[0].Time != worldRecord)
				{
					worldRecord = leaderboard.Entries[0].Time;
					if (isDateParameterValid)
					{
						if (LeaderboardHistoryUtils.HistoryJsonFileNameToDateTime(Path.GetFileNameWithoutExtension(leaderboardHistoryPath)) > date)
							break;
						data.Clear();
					}

					if (leaderboard.DateTime >= GameInfo.GameVersions["V1"].ReleaseDate)
						data.Add(new WorldRecord(leaderboard.DateTime, leaderboard.Entries[0]));
				}
			}

			// xvlv PBs
			//SortedDictionary<DateTime, Entry> data = new SortedDictionary<DateTime, Entry>()
			//{
			//	{ new DateTime(2017, 4, 12), new Entry{ Time = 6494522 } },
			//	{ new DateTime(2017, 4, 14), new Entry{ Time = 6799447 } },
			//	{ new DateTime(2017, 5, 1), new Entry{ Time = 7397468 } },
			//	{ new DateTime(2017, 5, 5), new Entry{ Time = 7399967 } },
			//	{ new DateTime(2017, 5, 6, 0, 0, 0), new Entry{ Time = 7598085 } },
			//	{ new DateTime(2017, 5, 6, 0, 0, 1), new Entry{ Time = 7610749 } },
			//	{ new DateTime(2017, 5, 8), new Entry{ Time = 7689896 } },
			//	{ new DateTime(2017, 5, 9, 0, 0, 0), new Entry{ Time = 7726221 } },
			//	{ new DateTime(2017, 5, 9, 0, 0, 1), new Entry{ Time = 7766044 } },
			//	{ new DateTime(2017, 5, 9, 0, 0, 2), new Entry{ Time = 8016483 } },
			//	{ new DateTime(2017, 5, 12), new Entry{ Time = 8301247 } },
			//	{ new DateTime(2017, 5, 19), new Entry{ Time = 8557351 } },
			//	{ new DateTime(2017, 5, 24), new Entry{ Time = 8559017 } },
			//	{ new DateTime(2017, 6, 4), new Entry{ Time = 8629000 } },
			//	{ new DateTime(2017, 6, 10), new Entry{ Time = 8667324 } },
			//	{ new DateTime(2017, 6, 13), new Entry{ Time = 8828784 } },
			//	{ new DateTime(2017, 6, 16), new Entry{ Time = 8931593 } },
			//	{ new DateTime(2017, 6, 21), new Entry{ Time = 9004242 } },
			//	{ new DateTime(2017, 6, 25), new Entry{ Time = 9091054 } },
			//	{ new DateTime(2017, 7, 17), new Entry{ Time = 9102218 } },
			//	{ new DateTime(2017, 7, 18, 0, 0, 0), new Entry{ Time = 9170701 } },
			//	{ new DateTime(2017, 7, 18, 0, 0, 1), new Entry{ Time = 9197194 } },
			//	{ new DateTime(2017, 7, 19), new Entry{ Time = 9219356 } },
			//	{ new DateTime(2017, 7, 21), new Entry{ Time = 9246849 } },
			//	{ new DateTime(2017, 7, 26), new Entry{ Time = 9304168 } },
			//	{ new DateTime(2017, 8, 14), new Entry{ Time = 9485791 } },
			//	{ new DateTime(2017, 8, 21), new Entry{ Time = 9487624 } },
			//	{ new DateTime(2017, 8, 22), new Entry{ Time = 9672912 } },
			//	{ new DateTime(2017, 8, 28), new Entry{ Time = 9696073 } },
			//	{ new DateTime(2017, 10, 20), new Entry{ Time = 9754558 } },
			//	{ new DateTime(2017, 10, 29), new Entry{ Time = 9755059 } },
			//	{ new DateTime(2017, 11, 08), new Entry{ Time = 9820376 } },
			//	{ new DateTime(2017, 11, 12), new Entry{ Time = 9838371 } },
			//	{ new DateTime(2017, 12, 20), new Entry{ Time = 9861699 } },
			//	{ new DateTime(2017, 12, 31), new Entry{ Time = 9892526 } },
			//	{ new DateTime(2018, 1, 13), new Entry{ Time = 10029491 } },
			//	{ new DateTime(2018, 4, 15), new Entry{ Time = 10169124 } },
			//	{ new DateTime(2018, 5, 29), new Entry{ Time = 10177622 } },
			//	{ new DateTime(2018, 6, 2), new Entry{ Time = 10262517 } },
			//	{ new DateTime(2018, 8, 12), new Entry{ Time = 10301817 } },
			//	{ new DateTime(2019, 1, 2), new Entry{ Time = 10309510 } },
			//	{ new DateTime(2019, 1, 12), new Entry{ Time = 10343961 } },
			//	{ new DateTime(2019, 1, 15), new Entry{ Time = 10359681 } },
			//	{ new DateTime(2019, 1, 21), new Entry{ Time = 10384767 } },
			//	{ new DateTime(2019, 1, 22), new Entry{ Time = 10390787 } },
			//	{ new DateTime(2019, 3, 1), new Entry{ Time = 10436443 } },
			//	{ new DateTime(2019, 3, 2), new Entry{ Time = 10453668 } },
			//	{ new DateTime(2019, 4, 3), new Entry{ Time = 10455006 } },
			//};

			// Cookle PBs
			//SortedDictionary<DateTime, Entry> data = new SortedDictionary<DateTime, Entry>()
			//{
			//	{ new DateTime(2017, 4, 20), new Entry{ Time = 4786271 } },
			//	{ new DateTime(2017, 5, 4), new Entry{ Time = 5154515 } },
			//	{ new DateTime(2017, 5, 19), new Entry{ Time = 5285817 } },
			//	{ new DateTime(2017, 5, 20), new Entry{ Time = 7242672 } },
			//	{ new DateTime(2017, 5, 25), new Entry{ Time = 7263666 } },
			//	{ new DateTime(2017, 5, 27), new Entry{ Time = 8169613 } },
			//	{ new DateTime(2017, 6, 8), new Entry{ Time = 8434381 } },
			//	{ new DateTime(2017, 6, 19), new Entry{ Time = 8510363 } },
			//	{ new DateTime(2017, 6, 26), new Entry{ Time = 8618503 } },
			//	{ new DateTime(2017, 6, 28), new Entry{ Time = 8742473 } },
			//	{ new DateTime(2017, 7, 4), new Entry{ Time = 8753637 } },
			//	{ new DateTime(2017, 7, 5), new Entry{ Time = 9011740 } },
			//	{ new DateTime(2017, 7, 10), new Entry{ Time = 9029736 } },
			//	{ new DateTime(2017, 7, 29), new Entry{ Time = 9119547 } },
			//	{ new DateTime(2017, 8, 15), new Entry{ Time = 9121046 } },
			//	{ new DateTime(2017, 8, 16), new Entry{ Time = 9249682 } },
			//	{ new DateTime(2017, 8, 19), new Entry{ Time = 9381483 } },
			//	{ new DateTime(2017, 9, 13), new Entry{ Time = 9582934 } },
			//	{ new DateTime(2017, 12, 1), new Entry{ Time = 9690574 } },
			//	{ new DateTime(2017, 12, 3), new Entry{ Time = 9727232 } },
			//	{ new DateTime(2017, 12, 8), new Entry{ Time = 9920852 } },
			//	{ new DateTime(2018, 1, 1), new Entry{ Time = 9961674 } },
			//	{ new DateTime(2018, 1, 20), new Entry{ Time = 10179288 } },
			//	{ new DateTime(2018, 3, 12), new Entry{ Time = 10232442 } },
			//	{ new DateTime(2018, 3, 28), new Entry{ Time = 10247800 } },
			//	{ new DateTime(2018, 6, 15), new Entry{ Time = 10372224 } },
			//	{ new DateTime(2018, 6, 17), new Entry{ Time = 10688468 } }
			//};

			return data;
		}
	}
}