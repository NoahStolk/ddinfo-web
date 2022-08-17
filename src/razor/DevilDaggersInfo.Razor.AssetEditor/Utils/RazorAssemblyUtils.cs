using System.Reflection;

namespace DevilDaggersInfo.Razor.AssetEditor.Utils;

internal static class RazorAssemblyUtils
{
	static RazorAssemblyUtils()
	{
		Version = Assembly.GetExecutingAssembly().GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion;
	}

	public static string? Version { get; set; }
}
