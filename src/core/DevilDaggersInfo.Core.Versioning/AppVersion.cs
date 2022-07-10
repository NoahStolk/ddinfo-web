using System.Diagnostics.CodeAnalysis;

namespace DevilDaggersInfo.Core.Versioning;

/// <summary>
/// <para>
/// Represents a simple version number used by the apps. It's a bit of a mix between the legacy <see cref="Version"/> class and <see href="https://semver.org/">Semantic Versioning</see>.
/// For historical reasons, and since these versions will not be used for packages, we do not strictly need to use Semantic Versioning. We can simplify things a little.
/// </para>
/// <para>
/// Examples (in order):
/// </para>
/// <list type="bullet">
/// <item>0.0.0.0</item>
/// <item>0.0.0.1</item>
/// <item>0.0.1.0</item>
/// <item>0.2.0.0</item>
/// <item>1.0.0.0</item>
/// <item>1.14.0.0</item>
/// <item>2.0.0-alpha.0</item>
/// <item>2.0.0-alpha.1</item>
/// <item>2.0.0-alpha.36</item>
/// <item>2.0.0.0</item>
/// </list>
/// </summary>
public record AppVersion
{
	public AppVersion(int major, int minor, int patch, int build = 0, bool isAlpha = false)
	{
		if (major < 0)
			throw new InvalidAppVersionException("Major must be a non-negative integer.");
		if (minor < 0)
			throw new InvalidAppVersionException("Minor must be a non-negative integer.");
		if (patch < 0)
			throw new InvalidAppVersionException("Patch must be a non-negative integer.");
		if (build < 0)
			throw new InvalidAppVersionException("Build must be a non-negative integer.");

		Major = major;
		Minor = minor;
		Patch = patch;
		IsAlpha = isAlpha;
		Build = build;
	}

	public int Major { get; }

	public int Minor { get; }

	public int Patch { get; }

	public bool IsAlpha { get; }

	public int Build { get; }

	public static bool operator >(AppVersion a, AppVersion b)
	{
		if (a.Major != b.Major)
			return a.Major > b.Major;

		if (a.Minor != b.Minor)
			return a.Minor > b.Minor;

		if (a.Patch != b.Patch)
			return a.Patch > b.Patch;

		if (a.IsAlpha != b.IsAlpha)
			return b.IsAlpha;

		if (a.Build != b.Build)
			return a.Build > b.Build;

		return false;
	}

	public static bool operator <(AppVersion a, AppVersion b)
		=> a != b && !(a > b);

	public static bool operator >=(AppVersion a, AppVersion b)
		=> a == b || a > b;

	public static bool operator <=(AppVersion a, AppVersion b)
		=> a == b || a < b;

	public override string ToString()
	{
		if (IsAlpha)
			return $"{Major}.{Minor}.{Patch}-alpha.{Build}";

		return $"{Major}.{Minor}.{Patch}.{Build}";
	}

	public static bool TryParse(string versionString, [NotNullWhen(true)] out AppVersion? appVersion)
	{
		try
		{
			appVersion = Parse(versionString);
			return true;
		}
		catch (InvalidAppVersionException)
		{
			appVersion = null;
			return false;
		}
	}

	public static AppVersion Parse(string versionString)
	{
		string[] parts = versionString.Split('.');
		if (parts.Length != 4)
			throw new InvalidAppVersionException("App version must contain 4 parts (major, minor, patch, build) separated by the '.' character.");

		if (!int.TryParse(parts[0], out int major) || major < 0)
			throw new InvalidAppVersionException("Major must be a non-negative integer.");
		if (!int.TryParse(parts[1], out int minor) || minor < 0)
			throw new InvalidAppVersionException("Minor must be a non-negative integer.");
		if (!int.TryParse(parts[3], out int build) || build < 0)
			throw new InvalidAppVersionException("Build must be a non-negative integer.");

		const string alphaId = "-alpha";
		bool isAlpha = parts[2].EndsWith(alphaId);
		string patchToParse = isAlpha ? parts[2][..^alphaId.Length] : parts[2];
		if (!int.TryParse(patchToParse, out int patch) || patch < 0)
			throw new InvalidAppVersionException($"Patch must be a non-negative integer. It can optionally contain the exact string {alphaId} at the end to indicate that this is an alpha version.");

		return new(major, minor, patch, build, isAlpha);
	}
}
