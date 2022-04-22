using DevilDaggersInfo.Web.BlazorWasm.Server.InternalModels.Json;

namespace DevilDaggersInfo.Web.BlazorWasm.Server.InternalModels;

public class Tool
{
	public string Name { get; init; } = null!;

	public string DisplayName { get; init; } = null!;

	/// <summary>
	/// Indicates the current version of the tool on the website.
	/// </summary>
	public Version VersionNumber { get; init; } = null!;

	/// <summary>
	/// Indicates the oldest version of the tool which is still fully compatible with the website.
	/// </summary>
	public Version VersionNumberRequired { get; init; } = null!;

	/// <summary>
	/// Contains the changelog for this tool deserialized from the Changelogs.json file, or <see langword="null"/> if the tool does not have a changelog.
	/// </summary>
	public IReadOnlyList<ChangelogEntry>? Changelog { get; init; }
}
