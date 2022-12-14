namespace DevilDaggersInfo.Web.Server.Domain.Models.Tools;

/// <summary>
/// This class must correspond to what's stored in the Changelogs.json file.
/// </summary>
public class Change
{
	public required string Description { get; init; }

	public IReadOnlyList<Change>? SubChanges { get; init; }
}
