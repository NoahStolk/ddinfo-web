using DevilDaggersInfo.Web.Server.InternalModels.Changelog;
using DevilDaggersInfo.Web.Shared.Dto.Public.Tools;

namespace DevilDaggersInfo.Web.Server.Converters.Public;

public static class ToolConverters
{
	public static GetTool ToDto(this ToolEntity tool, Dictionary<string, int> downloadCounts, List<ChangelogEntry>? changelog) => new()
	{
		Changelog = changelog?.ConvertAll(ce => new GetToolVersion
		{
			Changes = ce.Changes.Select(c => c.ToDto()).ToList(),
			Date = ce.Date,
			DownloadCount = downloadCounts.ContainsKey(ce.VersionNumber) ? downloadCounts[ce.VersionNumber] : 0,
			VersionNumber = ce.VersionNumber,
		}),
		Name = tool.Name,
		DisplayName = tool.DisplayName,
		VersionNumberRequired = tool.RequiredVersionNumber,
		VersionNumber = tool.CurrentVersionNumber,
	};

	public static GetToolDistribution ToDto(this ToolDistributionEntity distribution, ToolPublishMethod publishMethod, ToolBuildType buildType, int fileSize) => new()
	{
		BuildType = buildType,
		PublishMethod = publishMethod,
		VersionNumber = distribution.VersionNumber,
		FileSize = fileSize,
	};

	public static GetToolVersionChange ToDto(this Change change) => new()
	{
		Description = change.Description,
		SubChanges = change.SubChanges?.Select(c => c.ToDto()).ToList(),
	};
}
