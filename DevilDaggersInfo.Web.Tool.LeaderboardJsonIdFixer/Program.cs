using DevilDaggersInfo.Web.BlazorWasm.Server.Utils;
using DevilDaggersInfo.Web.BlazorWasm.Shared.Dto.Public.LeaderboardHistory;
using DevilDaggersInfo.Web.BlazorWasm.Shared.Utils;
using DevilDaggersInfo.Web.Tool.Shared;
using Newtonsoft.Json;
using System.Text;

#pragma warning disable IDE0079 // Remove unnecessary suppression
#pragma warning disable S1075 // URIs should not be hardcoded
const string _leaderboardHistoryPath = @"C:\Users\NOAH\source\repos\DevilDaggersInfo\DevilDaggersInfo.Web.BlazorWasm\Server\Data\LeaderboardHistory";
#pragma warning restore S1075 // URIs should not be hardcoded
#pragma warning restore IDE0079 // Remove unnecessary suppression

// Raven fix
SwapIds(new DateTime(2019, 4, 8), new DateTime(2019, 10, 11), 187974, 86805);

// pocket fix
SwapIds(new DateTime(2017, 11, 23), new DateTime(2020, 1, 14), 106722, 116704);

// ApplyNameTable();

// Swaps Ids for players with 2 accounts in the leaderboard history files.
// "dateStart" The date on which the player's alt overtook their main account.
// "dateEnd" The date on which the player's main overtook their alt account.
// "id1" The first Id to swap.
// "id2" The second Id to swap.
static void SwapIds(DateTime dateStart, DateTime dateEnd, int id1, int id2)
{
	foreach (string leaderboardHistoryPath in Directory.GetFiles(_leaderboardHistoryPath, "*.json"))
	{
		string fileName = Path.GetFileNameWithoutExtension(leaderboardHistoryPath);
		GetLeaderboardHistory leaderboard = JsonConvert.DeserializeObject<GetLeaderboardHistory>(File.ReadAllText(leaderboardHistoryPath, Encoding.UTF8)) ?? throw new("Could not deserialize leaderboard.");
		if (leaderboard.DateTime < dateStart || leaderboard.DateTime > dateEnd)
			continue;

		GetEntryHistory? entry1 = leaderboard.Entries.Find(e => e.Id == id1);
		GetEntryHistory? entry2 = leaderboard.Entries.Find(e => e.Id == id2);

		if (entry1 != null)
			entry1.Id = id2;
		if (entry2 != null)
			entry2.Id = id1;

		File.WriteAllText(leaderboardHistoryPath, JsonConvert.SerializeObject(leaderboard));
		Console.WriteLine($"Wrote {fileName}.", ConsoleColor.Green);
	}
}

static void ApplyNameTable()
{
	foreach (string path in Directory.GetFiles(_leaderboardHistoryPath, "*.json"))
	{
		string jsonString = File.ReadAllText(path, Encoding.UTF8);
		GetLeaderboardHistory leaderboard = JsonConvert.DeserializeObject<GetLeaderboardHistory>(jsonString) ?? throw new("Could not deserialize leaderboard.");

		List<GetEntryHistory> changes = new();
		foreach (GetEntryHistory entry in leaderboard.Entries)
		{
			if ((entry.Id == 0 || entry.Id == -1) && NameData.NameTable.ContainsKey(entry.Username))
			{
				entry.Id = NameData.NameTable[entry.Username];
				changes.Add(entry);
			}
		}

		if (changes.Count != 0)
		{
			Console.WriteLine(HistoryUtils.HistoryJsonFileNameToDateTime(Path.GetFileNameWithoutExtension(path)));
			foreach (GetEntryHistory entry in changes)
				Console.WriteLine($"\tSet Id to {entry.Id:D6} for rank {entry.Rank:D3} with name {entry.Username} and score {entry.Time.ToString(FormatUtils.TimeFormat)}");
			Console.WriteLine();
		}

		using StreamWriter sw = File.CreateText(path);
		sw.Write(JsonConvert.SerializeObject(leaderboard));
	}
}
