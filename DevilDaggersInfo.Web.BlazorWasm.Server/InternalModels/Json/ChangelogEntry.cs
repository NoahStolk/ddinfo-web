namespace DevilDaggersInfo.Web.BlazorWasm.Server.InternalModels.Json;

/// <summary>
/// This class must correspond to what's stored in the Changelogs.json file.
/// </summary>
public class ChangelogEntry
{
	public Version VersionNumber { get; init; } = null!;

	public DateTime Date { get; init; }

	public IReadOnlyList<Change> Changes { get; init; } = new List<Change>();
}
