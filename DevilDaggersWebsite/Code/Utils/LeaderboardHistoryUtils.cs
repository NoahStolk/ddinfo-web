using DevilDaggersCore.Leaderboards;
using DevilDaggersCore.Leaderboards.History;
using DevilDaggersCore.Utils;
using Microsoft.AspNetCore.Hosting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DevilDaggersWebsite.Code.Utils
{
	public static class LeaderboardHistoryUtils
	{
		public static List<WorldRecord> GetWorldRecords(IWebHostEnvironment env, DateTime? date)
		{
			bool isDateParameterValid = date.HasValue && date <= DateTime.Now;

			List<WorldRecord> data = new List<WorldRecord>();

			int worldRecord = 0;
			foreach (string leaderboardHistoryPath in Directory.GetFiles(Path.Combine(env.WebRootPath, "leaderboard-history"), "*.json"))
			{
				Leaderboard leaderboard = JsonConvert.DeserializeObject<Leaderboard>(File.ReadAllText(leaderboardHistoryPath, Encoding.UTF8));
				if (leaderboard.Entries[0].Time != worldRecord)
				{
					worldRecord = leaderboard.Entries[0].Time;
					if (isDateParameterValid)
					{
						if (HistoryUtils.HistoryJsonFileNameToDateTime(Path.GetFileNameWithoutExtension(leaderboardHistoryPath)) > date)
							break;
						data.Clear();
					}

					data.Add(new WorldRecord(leaderboard.DateTime, leaderboard.Entries[0]));
				}
			}

			return data;
		}
	}
}