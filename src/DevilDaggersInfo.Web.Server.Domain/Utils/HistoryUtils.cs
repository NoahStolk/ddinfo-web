using DevilDaggersInfo.Web.Server.Domain.Models.FileSystem;
using DevilDaggersInfo.Web.Server.Domain.Services.Inversion;

namespace DevilDaggersInfo.Web.Server.Domain.Utils;

public static class HistoryUtils
{
	public static DateTime HistoryFileNameToDateTime(string dateString)
	{
		int year = int.Parse(dateString[..4]);
		int month = int.Parse(dateString[4..6]);
		int day = int.Parse(dateString[6..8]);
		int hour = int.Parse(dateString[8..10]);
		int minute = int.Parse(dateString[10..12]);

		return new DateTime(year, month, day, hour, minute, 0, DateTimeKind.Utc);
	}

	/// <summary>
	/// Returns the names of all leaderboard history files, ordered by name (which is chronological).
	/// </summary>
	public static async Task<List<string>> GetHistoryFileNamesAsync(IFileSystem fileSystem)
	{
		IReadOnlyList<FileEntry> files = await fileSystem.ListAsync(DataSubDirectory.LeaderboardHistory);
		return [.. files.Select(f => f.Name).Where(n => n.EndsWith(".bin", StringComparison.Ordinal))];
	}

	/// <summary>
	/// Returns the name of the latest leaderboard history file at or before <paramref name="dateTime"/>, or the first file when there is none.
	/// </summary>
	public static string GetHistoryFileNameFromDate(IReadOnlyList<string> fileNames, DateTime dateTime)
	{
		string? fileName = fileNames
			.Where(n => n.EndsWith(".bin", StringComparison.Ordinal))
			.OrderByDescending(n => n, StringComparer.Ordinal)
			.FirstOrDefault(n => HistoryFileNameToDateTime(n) <= dateTime);
		return fileName ?? fileNames[0];
	}
}
