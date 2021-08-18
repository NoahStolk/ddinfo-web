using DevilDaggersInfo.Web.BlazorWasm.Server.Enums;
using DevilDaggersInfo.Web.BlazorWasm.Server.Utils;

namespace DevilDaggersInfo.Web.BlazorWasm.Server.Transients;

public class FileSystemService : IFileSystemService
{
	public FileSystemService()
	{
		foreach (DataSubDirectory e in (DataSubDirectory[])Enum.GetValues(typeof(DataSubDirectory)))
			Directory.CreateDirectory(GetPath(e));
	}

	public string Root => "Data";

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
		foreach (string path in paths.OrderByDescending(p => p))
		{
			if (HistoryUtils.HistoryJsonFileNameToDateTime(Path.GetFileName(path)) < dateTime)
				return path;
		}

		return paths[^1];
	}

	public string GetPath(DataSubDirectory subDirectory)
		=> Path.Combine(Root, subDirectory.ToString());

	public string FormatPath(string path)
	{
		char sep = Path.DirectorySeparatorChar;
		string rootIndicator = $"{sep}{Root}{sep}";
		if (!path.Contains(rootIndicator))
			return $"`{path}`";

		return $"`{path[path.IndexOf(rootIndicator)..]}`";
	}
}
