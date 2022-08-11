using DevilDaggersInfo.Common.Exceptions;
using DevilDaggersInfo.Types.Web;

namespace DevilDaggersInfo.Razor.AppManager.Extensions;

public static class BuildTypeExtensions
{
	public static string ToDisplayString(this ToolBuildType buildType) => buildType switch
	{
		ToolBuildType.WindowsWpf => "Legacy (WPF)",
		ToolBuildType.WindowsConsole => "Legacy (Windows console)",
		ToolBuildType.WindowsPhotino => "Windows",
		ToolBuildType.LinuxPhotino => "Linux",
		_ => throw new InvalidEnumConversionException(buildType),
	};
}
