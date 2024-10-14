using DevilDaggersInfo.Web.Server.Domain.Models.LeaderboardHistory;
using System.Text;

const string baseDirectory = """C:\Users\NOAH\source\repos\ddinfo-web\src\DevilDaggersInfo.Web.Server\Data\LeaderboardHistory""";

Dictionary<int, string> names = new()
{
	[0] = "unknown",
	[1] = "m4ttbush",
	[2] = "jtn",
	[4] = "b0necarver",
	[5] = "life",
	[6] = "Boneman",
	[8] = "woods",
	[112] = "weaksauce",
	[229] = "Sojk",
	[362] = "atro",
	[438] = "Kaijt",
	[1029] = "ohsnaplol",
	[1135] = "bowsr",
	[1545] = "KrosisOne",
	[1677] = "ANT4",
	[2049] = "kudemizu",
	[2076] = "Mymeleon",
	[2144] = "pmcc",
	[2316] = "aircawn",
	[3294] = "aiunzovovor",
	[4795] = "Moz",
	[6760] = "TSTAB",
	[10098] = "Stephanstein",
	[10210] = "sjorsbw",
	[14201] = "yokshu",
	[18469] = "trauma queen",
	[19218] = "Jane",
	[21854] = "xvlv",
	[27510] = "L0s Blitz0r",
	[40892] = "Shef",
	[48687] = "Hahmo",
	[82891] = "DraQu",
	[86805] = "L Saille",
	[88424] = "glum",
	[93991] = "cookie",
	[105641] = "jay",
	[106722] = "pocket",
	[116704] = "pocket (alt)",
	[118832] = "Chupacabra",
	[134802] = "gLad",
	[148788] = "Bintr",
	[152846] = "sjorsbw (pocket alt)",
	[173419] = "Kasa",
	[182924] = "axe",
	[187974] = "L Saille (alt)",
	[188777] = "_whipsta",
	[193297] = "Driftkon",
	[219601] = "Disres",
	[233257] = "sotp (alt)",
	[234946] = "sotp",
	[244105] = "skunky",
	[244645] = "pride",
	[247080] = "SilentSong",
	[262630] = "Owl City",
	[316836] = "sotp (alt2)",
	[999999] = "blankname",
};

// Find all players that have ever been in the top 10.
string[] files = Directory.GetFiles(baseDirectory, "*.bin", SearchOption.TopDirectoryOnly).Order().ToArray();
List<LeaderboardHistory> leaderboardHistories = files.Select(file => LeaderboardHistory.CreateFromFile(File.ReadAllBytes(file))).ToList();
List<(int Id, string Name)> top10Players = [];
foreach (LeaderboardHistory leaderboardHistory in leaderboardHistories)
	top10Players.AddRange(leaderboardHistory.Entries.Where(e => e.Rank <= 10).Select(e => (e.Id, names.GetValueOrDefault(e.Id, e.Username))));
top10Players = top10Players
	.DistinctBy(p => p.Id)
	.Where(p => p.Id is not (0 or 152_846 or 233_257 or 316_836 or 999_999))
	.OrderBy(p => p.Id)
	.ToList();

// For each date; find the player's score and keep track of it.
Dictionary<int, int> currentBestTimes = new();
List<DayEntry> dayEntries = [];
DateOnly releaseDate = new(2016, 2, 18);
DateOnly currentDate = releaseDate;
while (currentDate < DateOnly.FromDateTime(DateTime.UtcNow))
{
	LeaderboardHistory? leaderboardHistory = leaderboardHistories.OrderByDescending(lh => lh.DateTime).FirstOrDefault(lh => lh.DateTime <= new DateTime(currentDate, TimeOnly.MinValue, DateTimeKind.Utc));
	if (leaderboardHistory != null)
	{
		// Some history files might not contain all players, so we need to keep track of the best time for each player.
		foreach (EntryHistory entryHistory in leaderboardHistory.Entries)
		{
			if (currentBestTimes.TryGetValue(entryHistory.Id, out int value))
				currentBestTimes[entryHistory.Id] = Math.Max(value, entryHistory.Time);
			else
				currentBestTimes.Add(entryHistory.Id, entryHistory.Time);
		}
	}

	// Always add a day entry, even if there's no history for that day.
	dayEntries.Add(new DayEntry
	{
		DaysSinceRelease = currentDate.DayNumber - releaseDate.DayNumber,
		ScoreEntries = top10Players.ConvertAll(p => new ScoreEntry
		{
			PlayerId = p.Id,
			Time = currentBestTimes.GetValueOrDefault(p.Id, 0),
		}),
	});

	currentDate = currentDate.AddDays(1);
}

StringBuilder csvBuilder = new();
csvBuilder.AppendLine("Days since release;" + string.Join(";", top10Players.Select(p => p.Name)));
foreach (DayEntry dayEntry in dayEntries)
{
	csvBuilder.AppendLine($"{dayEntry.DaysSinceRelease};" + string.Join(";", top10Players.Select(p =>
	{
		ScoreEntry? scoreEntry = dayEntry.ScoreEntries.Find(s => s.PlayerId == p.Id);
		return scoreEntry == null ? "0.0000" : (scoreEntry.Time / 10_000f).ToString("0.0000");
	})));
}

await File.WriteAllTextAsync(Path.Combine(baseDirectory, "history.csv"), csvBuilder.ToString());

internal sealed record DayEntry
{
	public required int DaysSinceRelease { get; init; }
	public required List<ScoreEntry> ScoreEntries { get; init; }
}

internal sealed record ScoreEntry
{
	public required int PlayerId { get; init; }
	public required int Time { get; init; }
}
