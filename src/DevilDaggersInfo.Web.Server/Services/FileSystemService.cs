using DevilDaggersInfo.Web.Server.Domain.Models.FileSystem;
using DevilDaggersInfo.Web.Server.Domain.Services.Inversion;
using DevilDaggersInfo.Web.Server.Utils;

namespace DevilDaggersInfo.Web.Server.Services;

internal sealed class FileSystemService : IFileSystemService
{
	private const string _root = "Data";

	public FileSystemService()
	{
		foreach (DataSubDirectory e in Enum.GetValues<DataSubDirectory>())
			Directory.CreateDirectory(GetPath(e));
	}

	public string[] TryGetFiles(DataSubDirectory subDirectory)
	{
		try
		{
			// Directory.GetFiles does not guarantee any order. NTFS happens to return entries sorted by name, but ext4
			// returns them in hash order, so callers that depend on file names being chronological break on Linux.
			string[] files = Directory.GetFiles(GetPath(subDirectory));
			Array.Sort(files, StringComparer.Ordinal);
			return files;
		}
		catch
		{
			return [];
		}
	}

	public string GetLeaderboardHistoryPathFromDate(DateTime dateTime)
	{
		string[] paths = TryGetFiles(DataSubDirectory.LeaderboardHistory);
		foreach (string path in paths.Where(p => p.EndsWith(".bin")).OrderByDescending(p => p))
		{
			if (HistoryUtils.HistoryFileNameToDateTime(Path.GetFileName(path)) <= dateTime)
				return path;
		}

		return paths[0];
	}

	public string GetPath(DataSubDirectory subDirectory)
	{
		return Path.Combine(_root, subDirectory.ToString());
	}

	public async Task<string?> GetModArchiveCacheDataJsonAsync(string modName)
	{
		string filePath = Path.Combine(GetPath(DataSubDirectory.ModArchiveCache), $"{modName}.json");
		return IoFile.Exists(filePath) ? await IoFile.ReadAllTextAsync(filePath) : null;
	}
}
