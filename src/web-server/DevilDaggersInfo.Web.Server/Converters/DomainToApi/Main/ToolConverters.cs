using DevilDaggersInfo.Web.Server.Domain.Models.Tools;
using MainApi = DevilDaggersInfo.Api.Main.Tools;

namespace DevilDaggersInfo.Web.Server.Converters.DomainToApi.Main;

public static class ToolConverters
{
	public static MainApi.GetTool ToMainApi(this Tool tool) => new()
	{
		Changelog = tool.Changelog?.Select(ToMainApi).ToList(),
		DisplayName = tool.DisplayName,
		Name = tool.Name,
		VersionNumber = tool.VersionNumber,
		VersionNumberRequired = tool.VersionNumberRequired,
	};

	private static MainApi.GetToolVersion ToMainApi(this ToolVersion toolVersion) => new()
	{
		Changes = toolVersion.Changes.Select(tvc => tvc.ToMainApi()).ToList(),
		Date = toolVersion.Date,
		DownloadCount = toolVersion.DownloadCount,
		VersionNumber = toolVersion.VersionNumber,
	};

	private static MainApi.GetToolVersionChange ToMainApi(this ToolVersionChange toolVersionChange) => new()
	{
		Description = toolVersionChange.Description,
		SubChanges = toolVersionChange.SubChanges?.Select(tvc => tvc.ToMainApi()).ToList(),
	};

	public static MainApi.GetToolDistribution ToMainApi(this ToolDistribution distribution) => new()
	{
		BuildType = distribution.BuildType,
		PublishMethod = distribution.PublishMethod,
		VersionNumber = distribution.VersionNumber,
		FileSize = distribution.FileSize,
	};

	public static MainApi.GetToolVersionChange ToMainApi(this Change change) => new()
	{
		Description = change.Description,
		SubChanges = change.SubChanges?.Select(c => c.ToMainApi()).ToList(),
	};
}
