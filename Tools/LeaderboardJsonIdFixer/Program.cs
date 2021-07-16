using DevilDaggersWebsite.BlazorWasm.Server.Utils;
using DevilDaggersWebsite.BlazorWasm.Shared;
using DevilDaggersWebsite.BlazorWasm.Shared.Dto.LeaderboardHistory;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using ToolsShared;

#pragma warning disable IDE0079 // Remove unnecessary suppression
#pragma warning disable S1075 // URIs should not be hardcoded
const string _leaderboardHistoryPath = @"C:\Users\NOAH\source\repos\DevilDaggersWebsite\DevilDaggersWebsite.BlazorWasm\Server\Data\LeaderboardHistory";
#pragma warning restore S1075 // URIs should not be hardcoded
#pragma warning restore IDE0079 // Remove unnecessary suppression

#pragma warning disable S125 // Sections of code should not be commented out

// Raven fix
SwapIds(new DateTime(2019, 4, 8), new DateTime(2019, 10, 11), 187974, 86805);

// pocket fix
SwapIds(new DateTime(2017, 11, 23), new DateTime(2020, 1, 14), 106722, 116704);
#pragma warning restore S125 // Sections of code should not be commented out

//ApplyNameTable();

// Swaps Ids for players with 2 accounts in the leaderboard history files.
// "dateStart" The date on which the player's alt overtook their main account.
// "dateEnd" The date on which the player's main overtook their alt account.
// "id1" The first Id to swap.
// "id2" The second Id to swap.
#pragma warning disable CS8321 // Local function is declared but never used
static void SwapIds(DateTime dateStart, DateTime dateEnd, int id1, int id2)
#pragma warning restore CS8321 // Local function is declared but never used
{
	foreach (string leaderboardHistoryPath in Directory.GetFiles(_leaderboardHistoryPath, "*.json"))
	{
		string fileName = Path.GetFileNameWithoutExtension(leaderboardHistoryPath);
		GetLeaderboardHistoryPublic leaderboard = JsonConvert.DeserializeObject<GetLeaderboardHistoryPublic>(File.ReadAllText(leaderboardHistoryPath, Encoding.UTF8)) ?? throw new("Could not deserialize leaderboard.");
		if (leaderboard.DateTime < dateStart || leaderboard.DateTime > dateEnd)
			continue;

		GetEntryHistoryPublic? entry1 = leaderboard.Entries.Find(e => e.Id == id1);
		GetEntryHistoryPublic? entry2 = leaderboard.Entries.Find(e => e.Id == id2);

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
		GetLeaderboardHistoryPublic leaderboard = JsonConvert.DeserializeObject<GetLeaderboardHistoryPublic>(jsonString) ?? throw new("Could not deserialize leaderboard.");

		List<GetEntryHistoryPublic> changes = new();
		foreach (GetEntryHistoryPublic entry in leaderboard.Entries)
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
			foreach (GetEntryHistoryPublic entry in changes)
				Console.WriteLine($"\tSet Id to {entry.Id:D6} for rank {entry.Rank:D3} with name {entry.Username} and score {entry.Time.ToString(FormatUtils.TimeFormat)}");
			Console.WriteLine();
		}

		using StreamWriter sw = File.CreateText(path);
		sw.Write(JsonConvert.SerializeObject(leaderboard));
	}
}
