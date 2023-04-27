using DevilDaggersInfo.Web.Server.Domain.Entities.Enums;
using DevilDaggersInfo.Web.Server.Domain.Models.FileSystem;
using DevilDaggersInfo.Web.Server.Domain.Models.LeaderboardHistory;
using DevilDaggersInfo.Web.Server.Domain.Services.Caching;
using DevilDaggersInfo.Web.Server.Domain.Services.Inversion;
using System.IO.Compression;

namespace DevilDaggersInfo.Web.Server.Domain.Main.Tests.Utils;

// TODO: Remove.
public class TestData : ILeaderboardHistoryCache, IFileSystemService
{
	private readonly IReadOnlyDictionary<string, LeaderboardHistory> _data = new Dictionary<string, LeaderboardHistory>
	{
		["2022-01-01.bin"] = CreateLeaderboardHistory(new(2022, 1, 1), new()
		{
			CreateEntryHistory(1, 1, 90, "Player 1"),
			CreateEntryHistory(2, 2, 80, "Player 2"),
		}),
		["2022-01-02.bin"] = CreateLeaderboardHistory(new(2022, 1, 2), new()
		{
			CreateEntryHistory(1, 4, 100000, "Cheater"), // Cheater makes it to first place.
			CreateEntryHistory(2, 1, 90, "Player 1"),
			CreateEntryHistory(3, 2, 80, "Player 2"),
		}),
		["2022-01-03.bin"] = CreateLeaderboardHistory(new(2022, 1, 3), new()
		{
			CreateEntryHistory(1, 1, 95, "Player 1"),
			CreateEntryHistory(2, 2, 85, "Player 2"),
			CreateEntryHistory(3, 4, 0, "Cheater"), // Cheater is removed from the leaderboard.
		}),
		["2022-01-04.bin"] = CreateLeaderboardHistory(new(2022, 1, 4), new()
		{
			CreateEntryHistory(1, 4, 1000000, "Cheater"), // Cheater makes it to first place again.
			CreateEntryHistory(2, 1, 98, "Player 1"),
			CreateEntryHistory(3, 2, 85, "Player 2"),
			CreateEntryHistory(4, 3, 82, "Player 3"), // Player 3 joins the leaderboard.
		}),
	};

	private static LeaderboardHistory CreateLeaderboardHistory(DateTime dateTime, List<EntryHistory> entries)
	{
		return new()
		{
			DateTime = dateTime,
			Entries = entries,
			Players = entries.Count,
			DeathsGlobal = 0,
			GemsGlobal = 0,
			KillsGlobal = 0,
			TimeGlobal = 0,
			DaggersFiredGlobal = 0,
			DaggersHitGlobal = 0,
		};
	}

	private static EntryHistory CreateEntryHistory(int rank, int id, int time, string username)
	{
		return new()
		{
			Id = id,
			Rank = rank,
			Time = time,
			Username = username,
			Gems = 0,
			Kills = 0,
			DaggersFired = 0,
			DaggersHit = 0,
			DeathsTotal = 0,
			DeathType = 0,
			GemsTotal = 0,
			KillsTotal = 0,
			TimeTotal = 0,
			DaggersFiredTotal = 0,
			DaggersHitTotal = 0,
		};
	}

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
	public string GetLeaderboardHistoryPathFromDate(DateTime dateTime) => throw new NotImplementedException();
	public string GetPath(DataSubDirectory subDirectory) => throw new NotImplementedException();
	public long GetDirectorySize(string path)
	{
		throw new NotImplementedException();
	}
	public string GetToolDistributionPath(string name, ToolPublishMethod publishMethod, ToolBuildType buildType, string version) => throw new NotImplementedException();
	public bool DeleteFileIfExists(string path) => throw new NotImplementedException();
	public bool FileExists(string path)
	{
		throw new NotImplementedException();
	}
	public bool DeleteDirectoryIfExists(string path, bool recursive) => throw new NotImplementedException();
	public bool DirectoryExists(string path)
	{
		throw new NotImplementedException();
	}
	public void MoveDirectory(string sourcePath, string destinationPath)
	{
		throw new NotImplementedException();
	}
	public void CreateDirectory(string path)
	{
		throw new NotImplementedException();
	}
	public string[] GetFiles(string path)
	{
		throw new NotImplementedException();
	}
	public string[] GetFiles(string path, string searchPattern)
	{
		throw new NotImplementedException();
	}
	public byte[] ReadAllBytes(string path)
	{
		throw new NotImplementedException();
	}
	public Task<byte[]> ReadAllBytesAsync(string path)
	{
		throw new NotImplementedException();
	}
	public Task<byte[]> ReadAllBytesAsync(string path, CancellationToken cancellationToken)
	{
		throw new NotImplementedException();
	}
	public void WriteAllBytes(string path, byte[] bytes)
	{
		throw new NotImplementedException();
	}
	public Task WriteAllBytesAsync(string path, byte[] bytes)
	{
		throw new NotImplementedException();
	}
	public Task WriteAllBytesAsync(string path, byte[] bytes, CancellationToken cancellationToken)
	{
		throw new NotImplementedException();
	}
	public void MoveFile(string sourcePath, string destinationPath)
	{
		throw new NotImplementedException();
	}
	public string ReadAllText(string path)
	{
		throw new NotImplementedException();
	}
	public Task<string> ReadAllTextAsync(string path)
	{
		throw new NotImplementedException();
	}
	public Task<string> ReadAllTextAsync(string path, CancellationToken cancellationToken)
	{
		throw new NotImplementedException();
	}
	public void WriteAllText(string path, string text)
	{
		throw new NotImplementedException();
	}
	public Task WriteAllTextAsync(string path, string text)
	{
		throw new NotImplementedException();
	}
	public Task WriteAllTextAsync(string path, string text, CancellationToken cancellationToken)
	{
		throw new NotImplementedException();
	}
	public ZipArchive CreateZipFile(string zipFilePath)
	{
		throw new NotImplementedException();
	}
	public int GetCount() => throw new NotImplementedException();
	public void Clear() => throw new NotImplementedException();
#pragma warning restore SA1201
}
