namespace DevilDaggersInfo.Web.Server.InternalModels.Changelog;

/// <summary>
/// This class must correspond to what's stored in the Changelogs.json file.
/// </summary>
public class ChangelogEntry
{
	public string VersionNumber { get; init; } = string.Empty;

	public DateTime Date { get; init; }

	public IReadOnlyList<Change> Changes { get; init; } = new List<Change>();
}
