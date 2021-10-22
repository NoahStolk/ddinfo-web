namespace DevilDaggersInfo.Web.BlazorWasm.Server.Transients;

/// <summary>
/// This class must correspond to what's stored in the Changelogs.json file.
/// TODO: Move to JsonModels or something.
/// </summary>
public class Change
{
	public string Description { get; init; } = null!;

	public IReadOnlyList<Change>? SubChanges { get; init; }
}
