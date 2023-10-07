using System.Diagnostics;
using AdminApi = DevilDaggersInfo.Web.ApiSpec.Admin.Tools;
using ToolBuildType = DevilDaggersInfo.Web.Server.Domain.Entities.Enums.ToolBuildType;
using ToolPublishMethod = DevilDaggersInfo.Web.Server.Domain.Entities.Enums.ToolPublishMethod;

namespace DevilDaggersInfo.Web.Server.Domain.Admin.Converters.ApiToDomain;

public static class ToolConverters
{
	public static ToolPublishMethod ToDomain(this AdminApi.ToolPublishMethod publishMethod) => publishMethod switch
	{
		AdminApi.ToolPublishMethod.Default => ToolPublishMethod.Default,
		AdminApi.ToolPublishMethod.SelfContained => ToolPublishMethod.SelfContained,
		AdminApi.ToolPublishMethod.Aot => ToolPublishMethod.Aot,
		_ => throw new UnreachableException(),
	};

	public static ToolBuildType ToDomain(this AdminApi.ToolBuildType buildType) => buildType switch
	{
		AdminApi.ToolBuildType.WindowsWpf => ToolBuildType.WindowsWpf,
		AdminApi.ToolBuildType.WindowsConsole => ToolBuildType.WindowsConsole,
		AdminApi.ToolBuildType.WindowsPhotino => ToolBuildType.WindowsPhotino,
		AdminApi.ToolBuildType.LinuxPhotino => ToolBuildType.LinuxPhotino,
		AdminApi.ToolBuildType.WindowsWarp => ToolBuildType.WindowsWarp,
		AdminApi.ToolBuildType.LinuxWarp => ToolBuildType.LinuxWarp,
		AdminApi.ToolBuildType.LinuxConsole => ToolBuildType.LinuxConsole,
		_ => throw new UnreachableException(),
	};
}
