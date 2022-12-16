using DevilDaggersInfo.Types.Web;
using DevilDaggersInfo.Web.Server.Domain.Models.FileSystem;
using DevilDaggersInfo.Web.Server.Domain.Models.LeaderboardHistory;
using DevilDaggersInfo.Web.Server.Domain.Services.Caching;
using DevilDaggersInfo.Web.Server.Domain.Services.Inversion;

namespace DevilDaggersInfo.Web.Server.Domain.Main.Tests.Utils;

public class TestData : ILeaderboardHistoryCache, IFileSystemService
{
	private readonly IReadOnlyDictionary<string, LeaderboardHistory> _data = new Dictionary<string, LeaderboardHistory>
	{
		["2022-01-01.bin"] = new()
		{
			DateTime = new(2022, 1, 1),
			Entries = new()
			{
				new() { Rank = 1, Id = 1, Time = 90, Username = "Player 1" },
				new() { Rank = 2, Id = 2, Time = 80, Username = "Player 2" },
			},
		},
		["2022-01-02.bin"] = new()
		{
			DateTime = new(2022, 1, 2),
			Entries = new()
			{
				new() { Rank = 1, Id = 4, Time = 100000, Username = "Cheater" }, // Cheater makes it to first place.
				new() { Rank = 2, Id = 1, Time = 90, Username = "Player 1" },
				new() { Rank = 3, Id = 2, Time = 80, Username = "Player 2" },
			},
		},
		["2022-01-03.bin"] = new()
		{
			DateTime = new(2022, 1, 3),
			Entries = new()
			{
				new() { Rank = 1, Id = 1, Time = 95, Username = "Player 1" },
				new() {	Rank = 2, Id = 2, Time = 85, Username = "Player 2" },
				new() { Rank = 3, Id = 4, Time = 0, Username = "Cheater" }, // Cheater is removed from the leaderboard.
			},
		},
		["2022-01-04.bin"] = new()
		{
			DateTime = new(2022, 1, 4),
			Entries = new()
			{
				new() { Rank = 1, Id = 4, Time = 1000000, Username = "Cheater" }, // Cheater makes it to first place again.
				new() { Rank = 2, Id = 1, Time = 98, Username = "Player 1" },
				new() { Rank = 3, Id = 2, Time = 85, Username = "Player 2" },
				new() { Rank = 4, Id = 3, Time = 82, Username = "Player 3" }, // Player 3 joins the leaderboard.
			},
		},
	};

	public string[] TryGetFiles(DataSubDirectory subDirectory)
	{
		if (subDirectory != DataSubDirectory.LeaderboardHistory)
			throw new NotImplementedException();

		return _data.Keys.ToArray();
	}

	public LeaderboardHistory GetLeaderboardHistoryByFilePath(string filePath)
	{
		return _data[filePath];
	}

#pragma warning disable SA1201
	public string Root => throw new NotImplementedException();
	public string GetLeaderboardHistoryPathFromDate(DateTime dateTime) => throw new NotImplementedException();
	public string GetPath(DataSubDirectory subDirectory) => throw new NotImplementedException();
	public string GetToolDistributionPath(string name, ToolPublishMethod publishMethod, ToolBuildType buildType, string version) => throw new NotImplementedException();
	public int GetCount() => throw new NotImplementedException();
	public void Clear() => throw new NotImplementedException();
#pragma warning restore SA1201
}
