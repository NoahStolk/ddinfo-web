using DevilDaggersInfo.Api.Ddiam;
using DevilDaggersInfo.Common.Exceptions;

namespace DevilDaggersInfo.Razor.AppManager.Extensions;

public static class BuildTypeExtensions
{
	public static string ToDisplayString(this BuildType buildType) => buildType switch
	{
		BuildType.WindowsWpf => "Legacy (WPF)",
		BuildType.WindowsConsole => "Legacy (Windows console)",
		BuildType.WindowsPhotino => "Windows",
		BuildType.LinuxPhotino => "Linux",
		_ => throw new InvalidEnumConversionException(buildType),
	};
}
