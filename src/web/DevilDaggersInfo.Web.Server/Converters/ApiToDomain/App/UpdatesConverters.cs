using DevilDaggersInfo.Web.Server.Domain.Entities.Enums;
using System.Diagnostics;
using AppApi = DevilDaggersInfo.Api.App.Updates;

namespace DevilDaggersInfo.Web.Server.Converters.ApiToDomain.App;

public static class UpdatesConverters
{
	public static ToolPublishMethod ToDomain(this AppApi.ToolPublishMethod publishMethod) => publishMethod switch
	{
		AppApi.ToolPublishMethod.Default => ToolPublishMethod.Default,
		AppApi.ToolPublishMethod.SelfContained => ToolPublishMethod.SelfContained,
		AppApi.ToolPublishMethod.Aot => ToolPublishMethod.Aot,
		_ => throw new UnreachableException(),
	};

	public static ToolBuildType ToDomain(this AppApi.ToolBuildType buildType) => buildType switch
	{
		AppApi.ToolBuildType.WindowsWpf => ToolBuildType.WindowsWpf,
		AppApi.ToolBuildType.WindowsConsole => ToolBuildType.WindowsConsole,
		AppApi.ToolBuildType.WindowsPhotino => ToolBuildType.WindowsPhotino,
		AppApi.ToolBuildType.LinuxPhotino => ToolBuildType.LinuxPhotino,
		AppApi.ToolBuildType.WindowsWarp => ToolBuildType.WindowsWarp,
		AppApi.ToolBuildType.LinuxWarp => ToolBuildType.LinuxWarp,
		AppApi.ToolBuildType.LinuxConsole => ToolBuildType.LinuxConsole,
		_ => throw new UnreachableException(),
	};
}
