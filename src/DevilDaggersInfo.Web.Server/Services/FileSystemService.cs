using DevilDaggersInfo.Web.Server.Domain.Models.FileSystem;
using DevilDaggersInfo.Web.Server.Domain.Services.Inversion;
using DevilDaggersInfo.Web.Server.Utils;

namespace DevilDaggersInfo.Web.Server.Services;

public class FileSystemService : IFileSystemService
{
	private const string _root = "Data";

	public FileSystemService()
	{
		foreach (DataSubDirectory e in (DataSubDirectory[])Enum.GetValues(typeof(DataSubDirectory)))
			Directory.CreateDirectory(GetPath(e));
	}

	public string[] TryGetFiles(DataSubDirectory subDirectory)
	{
		try
		{
			return Directory.GetFiles(GetPath(subDirectory));
		}
		catch
		{
			return Array.Empty<string>();
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
}
