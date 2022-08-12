using DevilDaggersInfo.Types.Web;
using DevilDaggersInfo.Web.Server.Domain.Models.FileSystem;

namespace DevilDaggersInfo.Web.Server.Domain.Services.Inversion;

public interface IFileSystemService
{
	string Root { get; }

	string[] TryGetFiles(DataSubDirectory subDirectory);

	string GetLeaderboardHistoryPathFromDate(DateTime dateTime);

	string GetPath(DataSubDirectory subDirectory);

	string FormatPath(string path);

	string GetToolDistributionPath(string name, ToolPublishMethod publishMethod, ToolBuildType buildType, string version);
}
