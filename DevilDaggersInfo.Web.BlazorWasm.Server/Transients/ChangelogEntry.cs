namespace DevilDaggersInfo.Web.BlazorWasm.Server.Transients;

/// <summary>
/// This class must correspond to what's stored in the Tools.json file.
/// TODO: Store in database.
/// TODO: Rename to ToolVersionEntity.
/// </summary>
public class ChangelogEntry
{
	public Version VersionNumber { get; init; } = null!;

	public DateTime Date { get; init; }

	public IReadOnlyList<Change> Changes { get; init; } = new List<Change>();
}
