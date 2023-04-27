using DevilDaggersInfo.Web.Server.Domain.Entities.Enums;
using DevilDaggersInfo.Web.Server.Domain.Models.Tools;
using System.Diagnostics;
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
		BuildType = distribution.BuildType.ToMainApi(),
		PublishMethod = distribution.PublishMethod.ToMainApi(),
		VersionNumber = distribution.VersionNumber,
		FileSize = distribution.FileSize,
	};

	public static MainApi.ToolPublishMethod ToMainApi(this ToolPublishMethod publishMethod) => publishMethod switch
	{
		ToolPublishMethod.Default => MainApi.ToolPublishMethod.Default,
		ToolPublishMethod.SelfContained => MainApi.ToolPublishMethod.SelfContained,
		ToolPublishMethod.Aot => MainApi.ToolPublishMethod.Aot,
		_ => throw new UnreachableException(),
	};

	public static MainApi.ToolBuildType ToMainApi(this ToolBuildType buildType) => buildType switch
	{
		ToolBuildType.WindowsWpf => MainApi.ToolBuildType.WindowsWpf,
		ToolBuildType.WindowsConsole => MainApi.ToolBuildType.WindowsConsole,
		ToolBuildType.WindowsPhotino => MainApi.ToolBuildType.WindowsPhotino,
		ToolBuildType.LinuxPhotino => MainApi.ToolBuildType.LinuxPhotino,
		ToolBuildType.WindowsWarp => MainApi.ToolBuildType.WindowsWarp,
		ToolBuildType.LinuxWarp => MainApi.ToolBuildType.LinuxWarp,
		ToolBuildType.LinuxConsole => MainApi.ToolBuildType.LinuxConsole,
		_ => throw new UnreachableException(),
	};
}
