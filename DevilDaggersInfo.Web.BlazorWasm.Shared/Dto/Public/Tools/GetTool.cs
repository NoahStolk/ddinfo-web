namespace DevilDaggersInfo.Web.BlazorWasm.Shared.Dto.Public.Tools;

public class GetTool
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

	public IReadOnlyList<GetToolVersion> Versions { get; init; } = null!;

	public int FileSize { get; set; }
}
