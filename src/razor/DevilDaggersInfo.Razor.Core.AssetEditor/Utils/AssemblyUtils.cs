using System.Reflection;

namespace DevilDaggersInfo.Razor.Core.AssetEditor.Utils;

public static class AssemblyUtils
{
	static AssemblyUtils()
	{
		Version = Assembly.GetExecutingAssembly()?.GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion;
	}

	public static string? Version { get; set; }
}
