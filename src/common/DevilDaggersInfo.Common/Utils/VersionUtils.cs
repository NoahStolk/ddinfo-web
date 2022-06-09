using System.Reflection;

namespace DevilDaggersInfo.Common.Utils;

public static class VersionUtils
{
	public static readonly string EntryAssemblyVersion = Assembly.GetEntryAssembly()?.GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion ?? throw new("Failed to get version");
}
