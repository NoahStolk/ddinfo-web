using DevilDaggersInfo.Common.Exceptions;
using DevilDaggersInfo.Types.Web;

namespace DevilDaggersInfo.Web.Server.Converters.DomainToApi.Ddiam;

public static class ToolConverters
{
	public static Api.Ddiam.BuildType ToApi(this ToolBuildType buildType) => buildType switch
	{
		ToolBuildType.WindowsWpf => Api.Ddiam.BuildType.WindowsWpf,
		ToolBuildType.WindowsConsole => Api.Ddiam.BuildType.WindowsConsole,
		ToolBuildType.WindowsPhotino => Api.Ddiam.BuildType.WindowsPhotino,
		ToolBuildType.LinuxPhotino => Api.Ddiam.BuildType.LinuxPhotino,
		_ => throw new InvalidEnumConversionException(buildType),
	};
}
