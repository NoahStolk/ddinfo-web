using DevilDaggersInfo.Web.Server.Domain.Entities.Enums;
using DevilDaggersInfo.Web.Server.Domain.Models.Tools;
using System.Diagnostics;
using MainApi = DevilDaggersInfo.Web.ApiSpec.Main.Tools;

namespace DevilDaggersInfo.Web.Server.Converters.DomainToApi.Main;

public static class ToolConverters
{
	public static MainApi.GetTool ToMainApi(this Tool tool) => new()
	{
		DisplayName = tool.DisplayName,
		DownloadCounts = tool.DownloadCounts.Select(kvp => new MainApi.GetToolDownloadCount { Version = kvp.Key, Count = kvp.Value }).ToList(),
		Name = tool.Name,
		VersionNumber = tool.VersionNumber,
		VersionNumberRequired = tool.VersionNumberRequired,
	};

	public static MainApi.GetToolDistribution ToMainApi(this ToolDistribution distribution) => new()
	{
		BuildType = distribution.BuildType.ToMainApi(),
		PublishMethod = distribution.PublishMethod.ToMainApi(),
		VersionNumber = distribution.VersionNumber,
		FileSize = distribution.FileSize,
	};

	private static MainApi.ToolPublishMethod ToMainApi(this ToolPublishMethod publishMethod) => publishMethod switch
	{
		ToolPublishMethod.Default => MainApi.ToolPublishMethod.Default,
		ToolPublishMethod.SelfContained => MainApi.ToolPublishMethod.SelfContained,
		ToolPublishMethod.Aot => MainApi.ToolPublishMethod.Aot,
		_ => throw new UnreachableException(),
	};

	private static MainApi.ToolBuildType ToMainApi(this ToolBuildType buildType) => buildType switch
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
