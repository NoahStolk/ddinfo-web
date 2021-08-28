namespace DevilDaggersInfo.Web.BlazorWasm.Server.Transients;

/// <summary>
/// This class must correspond to what's stored in the Tools.json file.
/// TODO: Store in database.
/// </summary>
public class Change
{
	public string Description { get; init; } = null!;

	public IReadOnlyList<Change>? SubChanges { get; init; }
}
