namespace DevilDaggersInfo.Web.BlazorWasm.Server.Transients;

/// <summary>
/// This class must correspond to what's stored in the Tools.json file.
/// TODO: Store in database.
/// TODO: Rename to ToolEntity.
/// </summary>
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

	// TODO: Rename to Versions.
	public IReadOnlyList<ChangelogEntry> Changelog { get; init; } = null!;
}
