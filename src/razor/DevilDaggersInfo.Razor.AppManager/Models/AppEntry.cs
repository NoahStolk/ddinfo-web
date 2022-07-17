using DevilDaggersInfo.Api.Ddiam;
using DevilDaggersInfo.Core.Versioning;
using System.Diagnostics.CodeAnalysis;

namespace DevilDaggersInfo.Razor.AppManager.Models;

public record AppEntry(string Name, AppVersion Version, BuildType BuildType)
{
	private const char _separator = '_';

	public static bool TryParse(string fileName, [NotNullWhen(true)] out AppEntry? appEntry)
	{
		appEntry = null;

		string[] parts = fileName.Split(_separator);
		if (parts.Length != 3)
			return false;

		AppVersion version;
		try
		{
			version = AppVersion.Parse(parts[1]);
		}
		catch (InvalidAppVersionException)
		{
			return false;
		}

		if (!Enum.TryParse(parts[2], out BuildType buildType))
			return false;

		appEntry = new(parts[0], version, buildType);
		return true;
	}

	public override string ToString()
	{
		return string.Join(_separator, Name, Version);
	}
}
