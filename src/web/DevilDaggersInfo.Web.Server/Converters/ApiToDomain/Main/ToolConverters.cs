using DevilDaggersInfo.Web.Server.Domain.Entities.Enums;
using System.Diagnostics;
using MainApi = DevilDaggersInfo.Web.ApiSpec.Main.Tools;

namespace DevilDaggersInfo.Web.Server.Converters.ApiToDomain.Main;

public static class ToolConverters
{
	public static ToolPublishMethod ToDomain(this MainApi.ToolPublishMethod publishMethod) => publishMethod switch
	{
		MainApi.ToolPublishMethod.Default => ToolPublishMethod.Default,
		MainApi.ToolPublishMethod.SelfContained => ToolPublishMethod.SelfContained,
		MainApi.ToolPublishMethod.Aot => ToolPublishMethod.Aot,
		_ => throw new UnreachableException(),
	};

	public static ToolBuildType ToDomain(this MainApi.ToolBuildType buildType) => buildType switch
	{
		MainApi.ToolBuildType.WindowsWpf => ToolBuildType.WindowsWpf,
		MainApi.ToolBuildType.WindowsConsole => ToolBuildType.WindowsConsole,
		MainApi.ToolBuildType.WindowsPhotino => ToolBuildType.WindowsPhotino,
		MainApi.ToolBuildType.LinuxPhotino => ToolBuildType.LinuxPhotino,
		MainApi.ToolBuildType.WindowsWarp => ToolBuildType.WindowsWarp,
		MainApi.ToolBuildType.LinuxWarp => ToolBuildType.LinuxWarp,
		MainApi.ToolBuildType.LinuxConsole => ToolBuildType.LinuxConsole,
		_ => throw new UnreachableException(),
	};
}
