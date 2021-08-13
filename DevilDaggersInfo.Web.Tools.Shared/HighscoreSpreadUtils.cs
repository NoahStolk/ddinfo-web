using DevilDaggersCore.Game;
using DevilDaggersInfo.Web.BlazorWasm.Shared;
using DevilDaggersInfo.Web.BlazorWasm.Shared.Dto.Public.LeaderboardHistory;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace DevilDaggersInfo.Web.Tools.Shared
{
	public static class HighscoreSpreadUtils
	{
		private static readonly DateTime _fullHistoryDateStart = new(2018, 10, 1);

		private static readonly StringBuilder _log = new();

		public static void SpreadAllHighscoreStats(bool writeLogToFile, bool useConsole)
		{
			Dictionary<string, GetLeaderboardHistory> leaderboards = GetAllLeaderboards();

			_log.Clear();
			foreach (KeyValuePair<string, GetLeaderboardHistory> kvp in leaderboards)
			{
				SpreadHighscoreStats(leaderboards.Select(kvp => kvp.Value).ToList(), kvp.Value);
				File.WriteAllText(kvp.Key, JsonConvert.SerializeObject(kvp.Value, kvp.Value.DateTime > _fullHistoryDateStart ? Formatting.None : Formatting.Indented));
			}

			if (writeLogToFile)
				File.WriteAllText("Results.log", _log.ToString());
			if (useConsole)
				Console.WriteLine(_log.ToString());
		}

		public static Dictionary<string, GetLeaderboardHistory> GetAllLeaderboards()
		{
			Dictionary<string, GetLeaderboardHistory> leaderboards = new();
			foreach (string path in Directory.GetFiles(@"C:\Users\NOAH\source\repos\DevilDaggersInfo\DevilDaggersInfo.Web.BlazorWasm.Server\Data\LeaderboardHistory", "*.json"))
			{
				string jsonString = File.ReadAllText(path, Encoding.UTF8);
				leaderboards.Add(path, JsonConvert.DeserializeObject<GetLeaderboardHistory>(jsonString) ?? throw new("Could not deserialize leaderboard."));
			}

			return leaderboards;
		}

		public static void SpreadHighscoreStats(List<GetLeaderboardHistory> leaderboards, GetLeaderboardHistory leaderboard)
		{
			List<GetEntryHistory> changes = new();
			foreach (GetEntryHistory entry in leaderboard.Entries)
			{
				if (entry.Id != 0 && entry.HasMissingStats())
				{
					IEnumerable<GetLeaderboardHistory> leaderboardsWithStats = leaderboards.Where(l => l.Entries.Any(e => e.Id == entry.Id && e.Time >= entry.Time - 1 && e.Time <= entry.Time + 1));
					if (!leaderboardsWithStats.Any())
						continue;

					Combine(entry, leaderboardsWithStats.SelectMany(lb => lb.Entries).Where(e => e.Id == entry.Id));
					changes.Add(entry);
				}
			}

			if (changes.Count != 0)
			{
				_log.AppendLine(leaderboard.DateTime.ToString());
				foreach (GetEntryHistory entry in changes)
				{
					_log.Append("\tSet missing stats for ").Append(entry.Username).Append(' ').AppendLine(entry.Time.ToString(FormatUtils.TimeFormat));
					_log.Append("\t\tGems: ").Append(entry.Gems).AppendLine();
					_log.Append("\t\tKills: ").Append(entry.Kills).AppendLine();
					_log.Append("\t\tDeathType: ").AppendLine(GameInfo.GetDeathByType(GameInfo.GetGameVersionFromDate(leaderboard.DateTime) ?? GameVersion.V1, entry.DeathType)?.Name ?? "Unknown");
					_log.Append("\t\tAccuracy: ").AppendFormat("{0:00.00%}", entry.DaggersHit / (float)entry.DaggersFired).AppendLine();
				}

				_log.AppendLine();
			}
		}

		public static bool HasMissingStats(this GetEntryHistory entry)
			=> entry.Gems == 0 || entry.Kills == 0 || entry.DeathType == -1 || entry.DaggersHit == 0 || entry.DaggersFired == 0 || entry.DaggersFired == 10000;

		private static void Combine(GetEntryHistory original, IEnumerable<GetEntryHistory> entries)
		{
			GetEntryHistory? withGems = entries.FirstOrDefault(e => e.Gems != 0);
			if (withGems != null)
				original.Gems = withGems.Gems;

			GetEntryHistory? withKills = entries.FirstOrDefault(e => e.Kills != 0);
			if (withKills != null)
				original.Kills = withKills.Kills;

			GetEntryHistory? withDeathType = entries.FirstOrDefault(e => e.DeathType != -1);
			if (withDeathType != null)
				original.DeathType = withDeathType.DeathType;

			GetEntryHistory? withFullDaggerStats = entries.FirstOrDefault(e => e.DaggersFired != 0 && e.DaggersFired != 10000);
			if (withFullDaggerStats != null)
			{
				original.DaggersHit = withFullDaggerStats.DaggersHit;
				original.DaggersFired = withFullDaggerStats.DaggersFired;
			}
			else
			{
				GetEntryHistory? withPartialDaggerStats = entries.FirstOrDefault(e => e.DaggersFired != 0);
				if (withPartialDaggerStats != null)
				{
					original.DaggersHit = withPartialDaggerStats.DaggersHit;
					original.DaggersFired = withPartialDaggerStats.DaggersFired;
				}
			}
		}
	}
}
