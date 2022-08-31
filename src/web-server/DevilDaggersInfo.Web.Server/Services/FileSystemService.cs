using DevilDaggersInfo.Types.Web;
using DevilDaggersInfo.Web.Server.Domain.Models.FileSystem;
using DevilDaggersInfo.Web.Server.Domain.Services.Inversion;

namespace DevilDaggersInfo.Web.Server.Services;

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
		foreach (string path in paths.Where(p => p.EndsWith(".bin")).OrderByDescending(p => p))
		{
			if (HistoryUtils.HistoryFileNameToDateTime(Path.GetFileName(path)) <= dateTime)
				return path;
		}

		return paths[0];
	}

	public string GetPath(DataSubDirectory subDirectory)
		=> Path.Combine(Root, subDirectory.ToString());

	public string GetToolDistributionPath(string name, ToolPublishMethod publishMethod, ToolBuildType buildType, string version)
	{
		return Path.Combine(GetPath(DataSubDirectory.Tools), $"{name}-{version}-{buildType}-{publishMethod}.zip");
	}
}
