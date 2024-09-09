using DevilDaggersInfo.Core.Common;
using DevilDaggersInfo.Core.Wiki;
using DevilDaggersInfo.Web.Server.Domain.Models.LeaderboardHistory;
using System.Globalization;
using System.Text;

namespace DevilDaggersInfo.DevUtil.DistributeHistoryStats;

public static class HighscoreSpreadUtils
{
	private static readonly StringBuilder _log = new();

	public static void SpreadAllHighscoreStats(string rootDirectory, bool writeLogToFile, bool useConsole)
	{
		Dictionary<string, LeaderboardHistory> leaderboards = GetAllLeaderboards(rootDirectory);

		_log.Clear();
		foreach (KeyValuePair<string, LeaderboardHistory> kvp in leaderboards)
		{
			SpreadHighscoreStats(leaderboards.Select(lb => lb.Value).ToList(), kvp.Value);
			File.WriteAllBytes(kvp.Key, kvp.Value.ToBytes());
		}

		if (writeLogToFile)
			File.WriteAllText("Results.log", _log.ToString());
		if (useConsole)
			Console.WriteLine(_log.ToString());
	}

	private static Dictionary<string, LeaderboardHistory> GetAllLeaderboards(string rootDirectory)
	{
		Dictionary<string, LeaderboardHistory> leaderboards = new();
		foreach (string path in Directory.GetFiles(rootDirectory, "*.bin"))
		{
			byte[] bytes = File.ReadAllBytes(path);
			leaderboards.Add(path, LeaderboardHistory.CreateFromFile(bytes));
		}

		return leaderboards;
	}

	private static void SpreadHighscoreStats(List<LeaderboardHistory> leaderboards, LeaderboardHistory leaderboard)
	{
		List<EntryHistory> changes = [];
		foreach (EntryHistory entry in leaderboard.Entries)
		{
			if (entry.Id != 0 && entry.HasMissingStats())
			{
				IEnumerable<LeaderboardHistory> leaderboardsWithStats = leaderboards.Where(l => l.Entries.Exists(e => e.Id == entry.Id && e.Time >= entry.Time - 1 && e.Time <= entry.Time + 1)).ToList();
				if (!leaderboardsWithStats.Any())
					continue;

				changes.Add(Combine(entry, leaderboardsWithStats.SelectMany(lb => lb.Entries).Where(e => e.Id == entry.Id).ToList()));
			}
		}

		if (changes.Count == 0)
			return;

		_log.AppendLine(leaderboard.DateTime.ToString(CultureInfo.InvariantCulture));
		foreach (EntryHistory entry in changes)
		{
			_log.Append("\tSet missing stats for ").Append(entry.Username).Append(' ').AppendLine(entry.Time.ToString(StringFormats.TimeFormat));
			_log.Append("\t\tGems: ").Append(entry.Gems).AppendLine();
			_log.Append("\t\tKills: ").Append(entry.Kills).AppendLine();
			_log.Append("\t\tDeathType: ").AppendLine(Deaths.GetDeathByType(GameVersions.GetGameVersionFromDate(leaderboard.DateTime) ?? GameVersion.V1_0, entry.DeathType)?.Name ?? "Unknown");
			_log.Append("\t\tAccuracy: ").Append($"{entry.DaggersHit / (float)entry.DaggersFired:00.00%}").AppendLine();
		}

		_log.AppendLine();
	}

	private static bool HasMissingStats(this EntryHistory entry)
	{
		return entry.Gems == 0 || entry.Kills == 0 || entry.DeathType == 255 || entry.DaggersHit == 0 || entry.DaggersFired is 0 or 10000;
	}

	private static EntryHistory Combine(EntryHistory entry, List<EntryHistory> entries)
	{
		EntryHistory? withGems = entries.Find(e => e.Gems != 0);
		if (withGems != null)
			entry = entry with { Gems = withGems.Gems };

		EntryHistory? withKills = entries.Find(e => e.Kills != 0);
		if (withKills != null)
			entry = entry with { Kills = withKills.Kills };

		EntryHistory? withDeathType = entries.Find(e => e.DeathType != 255);
		if (withDeathType != null)
			entry = entry with { DeathType = withDeathType.DeathType };

		EntryHistory? withFullDaggerStats = entries.Find(e => e.DaggersFired != 0 && e.DaggersFired != 10000);
		if (withFullDaggerStats != null)
		{
			entry = entry with
			{
				DaggersHit = withFullDaggerStats.DaggersHit,
				DaggersFired = withFullDaggerStats.DaggersFired,
			};
		}
		else
		{
			EntryHistory? withPartialDaggerStats = entries.Find(e => e.DaggersFired != 0);
			if (withPartialDaggerStats != null)
			{
				entry = entry with
				{
					DaggersHit = withPartialDaggerStats.DaggersHit,
					DaggersFired = withPartialDaggerStats.DaggersFired,
				};
			}
		}

		return entry;
	}
}
