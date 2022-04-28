using DevilDaggersInfo.Web.Server.InternalModels.Json;
using DevilDaggersInfo.Web.Shared.Dto.Public.Tools;

namespace DevilDaggersInfo.Web.Server.Converters.Public;

public static class ToolConverters
{
	public static GetTool ToDto(this ToolEntity tool, Dictionary<ToolDistributionEntity, int> distributionsWithFileSize, List<ChangelogEntry>? changelog)
	{
		return new()
		{
			Changelog = distributionsWithFileSize
				.Select(d =>
				{
					ChangelogEntry? changelogEntry = changelog?.Find(ce => ce.VersionNumber == d.Key.VersionNumber);
					return new GetToolVersion
					{
						Changes = changelogEntry?.Changes.Select(c => c.ToDto()).ToList(),
						Date = changelogEntry?.Date,
						DownloadCount = d.Key.DownloadCount,
						VersionNumber = d.Key.VersionNumber,
					};
				})
				.ToList(),
			Name = tool.Name,
			DisplayName = tool.DisplayName,
			Distributions = distributionsWithFileSize
				.Select(d => new GetToolDistribution
				{
					BuildType = d.Key.BuildType,
					VersionNumber = d.Key.VersionNumber,
					FileSize = d.Value,
					PublishMethod = d.Key.PublishMethod,
				})
				.ToList(),
			LatestCompatibleVersionNumber = tool.RequiredVersionNumber,
			LatestVersionNumber = tool.CurrentVersionNumber,
		};
	}

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
