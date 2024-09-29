using DevilDaggersInfo.Web.Server.Domain.Models.LeaderboardHistory;
using System.Text;

string baseDirectory = args[0];

string[] files = Directory.GetFiles(baseDirectory, "*.bin", SearchOption.TopDirectoryOnly).Order().ToArray();

List<DayEntry> dayEntries = [];

DateOnly releaseDate = new(2016, 2, 18);
DateOnly currentDate = releaseDate;
while (currentDate < DateOnly.FromDateTime(DateTime.UtcNow))
{
	string dateString = currentDate.ToString("yyyyMMdd");
	string? filePath = Array.Find(files, f => Path.GetFileNameWithoutExtension(f).StartsWith(dateString));
	if (filePath == null)
	{
		Console.WriteLine($"Missing file for {currentDate}.");

		// TODO: Add DayEntry with Completeness = Incomplete and the entries copied from the previous day.
	}
	else
	{
		LeaderboardHistory leaderboardHistory = LeaderboardHistory.CreateFromFile(await File.ReadAllBytesAsync(filePath));

		bool isCompleteTop10 = leaderboardHistory.Entries.DistinctBy(e => e.Rank).Count(e => e.Rank <= 10) == 10;
		if (isCompleteTop10)
		{
			dayEntries.Add(new DayEntry
			{
				DaysSinceRelease = DateOnly.FromDateTime(leaderboardHistory.DateTime).DayNumber - releaseDate.DayNumber,
				Completeness = Completeness.Complete,
				ScoreEntries = leaderboardHistory.Entries
					.Where(e => e.Rank <= 10)
					.Select(scoreEntry => new ScoreEntry
					{
						PlayerId = scoreEntry.Id,
						Rank = scoreEntry.Rank,
						Time = scoreEntry.Time,
					})
					.ToList(),
			});
		}
		else
		{
			Console.WriteLine($"Incomplete top 10 for {currentDate}.");

			// TODO: Add DayEntry with Completeness = Partial and the missing entries copied from the previous day.
		}
	}

	currentDate = currentDate.AddDays(1);
}

StringBuilder csvBuilder = new();
int[] playerIds = dayEntries.SelectMany(d => d.ScoreEntries.Select(s => s.PlayerId)).Distinct().Order().ToArray();
csvBuilder.AppendLine(";;" + string.Join(";", playerIds.Select(p => p.ToString())));
foreach (DayEntry dayEntry in dayEntries)
{
	csvBuilder.AppendLine($"{dayEntry.Completeness};{dayEntry.DaysSinceRelease};" + string.Join(";", playerIds.Select(p =>
	{
		// TODO: Need to keep track of everyone's score for each day, even if they're not present in the top 10.
		ScoreEntry? scoreEntry = dayEntry.ScoreEntries.Find(s => s.PlayerId == p);
		return scoreEntry == null ? string.Empty : (scoreEntry.Time / 10_000f).ToString("0.0000");
	})));
}

await File.WriteAllTextAsync(Path.Combine(baseDirectory, "history.csv"), csvBuilder.ToString());

internal enum Completeness
{
	Complete,
	Partial,
	Incomplete,
}

internal sealed record DayEntry
{
	public required int DaysSinceRelease { get; init; }
	public required Completeness Completeness { get; init; }
	public required List<ScoreEntry> ScoreEntries { get; init; }
}

internal sealed record ScoreEntry
{
	public required int PlayerId { get; init; }
	public required int Rank { get; init; }
	public required int Time { get; init; }
}
