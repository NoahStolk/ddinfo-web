using DevilDaggersInfo.Web.Server.Domain.Models.FileSystem;

namespace DevilDaggersInfo.Web.Server.Domain.Services;

public interface IFileSystemService
{
	string Root { get; }

	string[] TryGetFiles(DataSubDirectory subDirectory);

	string GetLeaderboardHistoryPathFromDate(DateTime dateTime);

	string GetPath(DataSubDirectory subDirectory);

	string FormatPath(string path);
}
