namespace DevilDaggersInfo.Web.BlazorWasm.Shared.Dto.Public.Tools.Obsolete;

[Obsolete("This DTO is a copy of DevilDaggersInfo.Web.BlazorWasm.Server.Transients.Tool which cannot be removed due to backwards compatibility with the current tools.")]
public class GetToolObsolete
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

	public IReadOnlyList<GetChangelogEntryObsolete> Changelog { get; init; } = null!;
}
