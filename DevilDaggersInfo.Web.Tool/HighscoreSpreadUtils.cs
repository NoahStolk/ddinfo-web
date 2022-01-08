using DevilDaggersInfo.Core.Wiki;
using DevilDaggersInfo.Core.Wiki.Enums;
using DevilDaggersInfo.Web.BlazorWasm.Server.InternalModels.Json;
using DevilDaggersInfo.Web.BlazorWasm.Shared.Utils;
using Newtonsoft.Json;
using System.Text;

namespace DevilDaggersInfo.Web.Tool;

public static class HighscoreSpreadUtils
{
	private static readonly DateTime _fullHistoryDateStart = new(2018, 10, 1);

	private static readonly StringBuilder _log = new();

	public static void SpreadAllHighscoreStats(bool writeLogToFile, bool useConsole)
	{
		Dictionary<string, LeaderboardHistory> leaderboards = GetAllLeaderboards();

		_log.Clear();
		foreach (KeyValuePair<string, LeaderboardHistory> kvp in leaderboards)
		{
			SpreadHighscoreStats(leaderboards.Select(kvp => kvp.Value).ToList(), kvp.Value);
			File.WriteAllText(kvp.Key, JsonConvert.SerializeObject(kvp.Value, kvp.Value.DateTime > _fullHistoryDateStart ? Formatting.None : Formatting.Indented));
		}

		if (writeLogToFile)
			File.WriteAllText("Results.log", _log.ToString());
		if (useConsole)
			Console.WriteLine(_log.ToString());
	}

	public static Dictionary<string, LeaderboardHistory> GetAllLeaderboards()
	{
		Dictionary<string, LeaderboardHistory> leaderboards = new();
		foreach (string path in Directory.GetFiles(@"C:\Users\NOAH\source\repos\DevilDaggersInfo\DevilDaggersInfo.Web.BlazorWasm.Server\Data\LeaderboardHistory", "*.json"))
		{
			string jsonString = File.ReadAllText(path, Encoding.UTF8);
			leaderboards.Add(path, JsonConvert.DeserializeObject<LeaderboardHistory>(jsonString) ?? throw new("Could not deserialize leaderboard."));
		}

		return leaderboards;
	}

	public static void SpreadHighscoreStats(List<LeaderboardHistory> leaderboards, LeaderboardHistory leaderboard)
	{
		List<EntryHistory> changes = new();
		foreach (EntryHistory entry in leaderboard.Entries)
		{
			if (entry.Id != 0 && entry.HasMissingStats())
			{
				IEnumerable<LeaderboardHistory> leaderboardsWithStats = leaderboards.Where(l => l.Entries.Any(e => e.Id == entry.Id && e.Time >= entry.Time - 1 && e.Time <= entry.Time + 1));
				if (!leaderboardsWithStats.Any())
					continue;

				Combine(entry, leaderboardsWithStats.SelectMany(lb => lb.Entries).Where(e => e.Id == entry.Id));
				changes.Add(entry);
			}
		}

		if (changes.Count != 0)
		{
			_log.AppendLine(leaderboard.DateTime.ToString());
			foreach (EntryHistory entry in changes)
			{
				_log.Append("\tSet missing stats for ").Append(entry.Username).Append(' ').AppendLine(entry.Time.ToString(FormatUtils.TimeFormat));
				_log.Append("\t\tGems: ").Append(entry.Gems).AppendLine();
				_log.Append("\t\tKills: ").Append(entry.Kills).AppendLine();
				_log.Append("\t\tDeathType: ").AppendLine(Deaths.GetDeathByLeaderboardType(GameVersions.GetGameVersionFromDate(leaderboard.DateTime) ?? GameVersion.V1_0, entry.DeathType)?.Name ?? "Unknown");
				_log.Append("\t\tAccuracy: ").AppendFormat("{0:00.00%}", entry.DaggersHit / (float)entry.DaggersFired).AppendLine();
			}

			_log.AppendLine();
		}
	}

	public static bool HasMissingStats(this EntryHistory entry)
		=> entry.Gems == 0 || entry.Kills == 0 || entry.DeathType == 255 || entry.DaggersHit == 0 || entry.DaggersFired == 0 || entry.DaggersFired == 10000;

	private static void Combine(EntryHistory original, IEnumerable<EntryHistory> entries)
	{
		EntryHistory? withGems = entries.FirstOrDefault(e => e.Gems != 0);
		if (withGems != null)
			original.Gems = withGems.Gems;

		EntryHistory? withKills = entries.FirstOrDefault(e => e.Kills != 0);
		if (withKills != null)
			original.Kills = withKills.Kills;

		EntryHistory? withDeathType = entries.FirstOrDefault(e => e.DeathType != 255);
		if (withDeathType != null)
			original.DeathType = withDeathType.DeathType;

		EntryHistory? withFullDaggerStats = entries.FirstOrDefault(e => e.DaggersFired != 0 && e.DaggersFired != 10000);
		if (withFullDaggerStats != null)
		{
			original.DaggersHit = withFullDaggerStats.DaggersHit;
			original.DaggersFired = withFullDaggerStats.DaggersFired;
		}
		else
		{
			EntryHistory? withPartialDaggerStats = entries.FirstOrDefault(e => e.DaggersFired != 0);
			if (withPartialDaggerStats != null)
			{
				original.DaggersHit = withPartialDaggerStats.DaggersHit;
				original.DaggersFired = withPartialDaggerStats.DaggersFired;
			}
		}
	}
}
