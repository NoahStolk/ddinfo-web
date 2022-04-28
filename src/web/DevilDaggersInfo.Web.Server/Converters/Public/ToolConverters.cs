using DevilDaggersInfo.Web.Server.InternalModels.Json;
using DevilDaggersInfo.Web.Shared.Dto.Public.Tools;

namespace DevilDaggersInfo.Web.Server.Converters.Public;

public static class ToolConverters
{
	public static GetTool ToDto(this ToolEntity tool, List<ToolDistributionEntity> distributions, List<ChangelogEntry>? changelog)
	{
		return new()
		{
			Changelog = changelog?.ConvertAll(ce =>
			{
				ToolDistributionEntity? distribution = distributions.Find(td => td.VersionNumber == ce.VersionNumber);
				return new GetToolVersion
				{
					Changes = ce.Changes.Select(c => c.ToDto()).ToList(),
					Date = ce.Date,
					DownloadCount = distribution?.DownloadCount,
					VersionNumber = ce.VersionNumber,
				};
			}),
			Name = tool.Name,
			DisplayName = tool.DisplayName,
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
