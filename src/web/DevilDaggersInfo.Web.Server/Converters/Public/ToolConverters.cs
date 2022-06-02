using DevilDaggersInfo.Web.Server.Entities.Enums;
using DevilDaggersInfo.Web.Server.InternalModels.Changelog;
using MainApi = DevilDaggersInfo.Api.Main.Tools;

namespace DevilDaggersInfo.Web.Server.Converters.Public;

public static class ToolConverters
{
	public static MainApi.GetTool ToMainApi(this ToolEntity tool, Dictionary<string, int> downloadCounts, List<ChangelogEntry>? changelog) => new()
	{
		Changelog = changelog?.ConvertAll(ce => new MainApi.GetToolVersion
		{
			Changes = ce.Changes.Select(c => c.ToMainApi()).ToList(),
			Date = ce.Date,
			DownloadCount = downloadCounts.ContainsKey(ce.VersionNumber) ? downloadCounts[ce.VersionNumber] : 0,
			VersionNumber = ce.VersionNumber,
		}),
		Name = tool.Name,
		DisplayName = tool.DisplayName,
		VersionNumberRequired = tool.RequiredVersionNumber,
		VersionNumber = tool.CurrentVersionNumber,
	};

	public static MainApi.GetToolDistribution ToMainApi(this ToolDistributionEntity distribution, ToolPublishMethod publishMethod, ToolBuildType buildType, int fileSize) => new()
	{
		BuildType = buildType.ToMainApi(),
		PublishMethod = publishMethod.ToMainApi(),
		VersionNumber = distribution.VersionNumber,
		FileSize = fileSize,
	};

	public static MainApi.GetToolVersionChange ToMainApi(this Change change) => new()
	{
		Description = change.Description,
		SubChanges = change.SubChanges?.Select(c => c.ToMainApi()).ToList(),
	};

	private static MainApi.ToolBuildType ToMainApi(this ToolBuildType buildType) => buildType switch
	{
		ToolBuildType.WindowsWpf => MainApi.ToolBuildType.WindowsWpf,
		ToolBuildType.WindowsConsole => MainApi.ToolBuildType.WindowsConsole,
		_ => throw new NotSupportedException($"Tool build type '{buildType}' is not supported."),
	};

	private static MainApi.ToolPublishMethod ToMainApi(this ToolPublishMethod publishMethod) => publishMethod switch
	{
		ToolPublishMethod.Default => MainApi.ToolPublishMethod.Default,
		ToolPublishMethod.SelfContained => MainApi.ToolPublishMethod.SelfContained,
		_ => throw new NotSupportedException($"Tool publish method '{publishMethod}' is not supported."),
	};
}
