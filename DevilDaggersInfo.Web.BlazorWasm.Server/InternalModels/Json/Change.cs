namespace DevilDaggersInfo.Web.BlazorWasm.Server.InternalModels.Json;

/// <summary>
/// This class must correspond to what's stored in the Changelogs.json file.
/// </summary>
public class Change
{
	public string Description { get; init; } = null!;

	public IReadOnlyList<Change>? SubChanges { get; init; }
}
